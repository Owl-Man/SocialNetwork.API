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

    public async Task<User?> GetById(Guid id)
    {
        return await _usersRepository.GetById(id);
    }

    public async Task<List<User>> GetWithPosts(Guid id)
    {
        return await _usersRepository.GetWithPosts(id);
    }

    public async Task<Guid> Create(User user)
    {
        return await _usersRepository.Create(user);
    }

    public async Task<Guid> Update(Guid id, string firstName, string secondName, string bio)
    {
        return await _usersRepository.Update(id, firstName, secondName, bio);
    }

    public async Task<Guid> Delete(Guid id)
    {
        return await _usersRepository.Delete(id);
    }
}
