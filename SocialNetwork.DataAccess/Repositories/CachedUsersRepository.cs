using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Repositories;

public class CachedUsersRepository : IUsersRepository
{
    private readonly UsersRepository _decorated;
    private readonly IDistributedCache _distributedCache;

    public CachedUsersRepository(UsersRepository usersRepository, IDistributedCache distributedCache)
    {
        _decorated = usersRepository;
        _distributedCache = distributedCache;
    }

    public (Guid, string) Create(string firstName, string secondName, string bio) => _decorated.Create(firstName, secondName, bio);

    public List<User> GetAll() => _decorated.GetAll();

    public User? GetById(Guid id)
    {
        string key = $"User-{id}";

        string? cachedUser = _distributedCache.GetString(key);

        User? user;

        if (string.IsNullOrEmpty(cachedUser))
        {
            user = _decorated.GetById(id);

            if (user is null)
            {
                return user;
            }

            _distributedCache.SetString(key, JsonConvert.SerializeObject(user));
        }
        else
        {
            user = JsonConvert.DeserializeObject<User>(cachedUser, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }

        return user;
    }

    public List<User> GetWithPosts(Guid id) => _decorated.GetWithPosts(id);

    public (Guid, string) Update(Guid id, string firstName, string secondName, string bio, List<Topic> preferredTopics)
    {
        TryDeleteUserFromCache(id);

        return _decorated.Update(id, firstName, secondName, bio, preferredTopics);
    }

    public Guid? UpdatePreferredTopics(Guid id, List<Topic> preferredTopics)
    {
        TryDeleteUserFromCache(id);

        return _decorated.UpdatePreferredTopics(id, preferredTopics);
    }

    public Guid? Delete(Guid id)
    {
        TryDeleteUserFromCache(id);

        return _decorated.Delete(id);
    }

    private void TryDeleteUserFromCache(Guid id) 
    {
        string key = $"User-{id}";

        string? cachedUser = _distributedCache.GetString(key);

        if (!string.IsNullOrEmpty(cachedUser)) 
        {
            _distributedCache.Remove(key);
        }
    }
}
