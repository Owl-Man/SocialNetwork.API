using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IUsersRepository
    {
        List<User> GetAll();
        User? GetById(Guid id);
        List<User> GetWithPosts(Guid id);
        Guid Create(string firstName, string secondName, string bio);
        Guid Update(Guid id, string firstName, string secondName, string bio);
        Guid Delete(Guid id);
    }
}