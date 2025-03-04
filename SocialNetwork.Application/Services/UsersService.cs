using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Repositories;

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

    public Guid Create(User user)
    {
        return _usersRepository.Create(user);
    }

    public Guid Update(Guid id, string firstName, string secondName, string bio)
    {
        return _usersRepository.Update(id, firstName, secondName, bio);
    }

    public Guid Delete(Guid id)
    {
        return _usersRepository.Delete(id);
    }
}
