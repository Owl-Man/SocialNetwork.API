using SocialNetwork.Core.Models;

namespace SocialNetwork.Application.Services
{
    public interface IUserService
    {
        Task<Guid> Create(User user);
        Task<Guid> Delete(Guid id);
        Task<User?> GetById(Guid id);
        Task<List<User>> GetWithPosts(Guid id);
        Task<Guid> Update(Guid id, string firstName, string secondName, string bio);
    }
}