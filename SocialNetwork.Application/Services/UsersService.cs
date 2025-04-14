using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.Application.Services;

public class UserService : IUserService
{
    private readonly IUsersRepository _usersRepository;

    public UserService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public List<User> GetAll() 
    {
        return _usersRepository.GetAll();
    }

    public User? GetById(Guid id)
    {
        return _usersRepository.GetById(id);
    }

    public List<User> GetWithPosts(Guid id)
    {
        return _usersRepository.GetWithPosts(id);
    }

    public (Guid, string) Create(string firstName, string secondName, string bio)
    {
        return _usersRepository.Create(firstName, secondName, bio);
    }

    public (Guid, string) Update(Guid id, string firstName, string secondName, string bio, List<Topic> preferredTopics)
    {
        return _usersRepository.Update(id, firstName, secondName, bio, preferredTopics);
    }

    public Guid? UpdatePreferredTopics(Guid id, List<Topic> preferredTopics)
    {
        return _usersRepository.UpdatePreferredTopics(id, preferredTopics);
    }

    public Guid? Delete(Guid id)
    {
        return _usersRepository.Delete(id);
    }
}
