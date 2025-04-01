using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Repositories;

public class CachedPostsRepository : IPostsRepository
{
    private readonly PostsRepository _decorated;
    private readonly IDistributedCache _distributedCache;

    public CachedPostsRepository(PostsRepository postsRepository, IDistributedCache distributedCache)
    {
        _decorated = postsRepository;
        _distributedCache = distributedCache;
    }

    public Guid Create(Guid authorID, string title, string content, Topic topic) => _decorated.Create(authorID, title, content, topic);

    public Guid Delete(Guid id) => _decorated.Delete(id);

    public List<Post> GetAll() => _decorated.GetAll();

    public List<Post> GetByAuthor(Guid authorId)
    {
        string key = $"Post-withAuthor-{authorId}";

        string? cachedPosts = _distributedCache.GetString(key);

        List<Post> posts;

        if (string.IsNullOrEmpty(cachedPosts))
        {
            posts = _decorated.GetByAuthor(authorId);

            if (posts.Count == 0)
            {
                return posts;
            }

            _distributedCache.SetString(key, JsonConvert.SerializeObject(posts));
        }
        else
        {
            posts = JsonConvert.DeserializeObject<List<Post>>(cachedPosts, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            }) ?? new List<Post>();

            //ONLY FOR TESTING API
            Post originPost = posts[0];

            posts[0] = new Post(originPost.Id, "From redis cache: " + originPost.Title, originPost
                .Content, originPost.Author, originPost.Topic);
        }

        return posts;
    }

    public List<Post> GetByFilter(string searchValue) => _decorated.GetByFilter(searchValue);

    public List<Post> GetByTopic(Topic topic) => _decorated.GetByTopic(topic);

    public Guid Update(Guid id, string title, string content) => _decorated.Update(id, title, content);
}
