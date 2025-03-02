using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> GetById(Guid id);
        Task<List<User>> GetWithPosts(Guid id);
        Task<Guid> Create(User user);
        Task<Guid> Update(Guid id, string firstName, string secondName, string bio);
        Task<Guid> Delete(Guid id);
    }
}