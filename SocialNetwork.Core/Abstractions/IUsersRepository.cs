using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Repositories
{
    public interface IUsersRepository
    {
        List<User> GetAll();
        User? GetById(Guid id);
        List<User> GetWithPosts(Guid id);
        Guid Create(User user);
        Guid Update(Guid id, string firstName, string secondName, string bio);
        Guid Delete(Guid id);
    }
}