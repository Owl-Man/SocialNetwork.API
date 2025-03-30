using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IUserService
    {
        List<User> GetAll();
        Guid Create(string firstName, string secondName, string bio);
        Guid Delete(Guid id);
        User? GetById(Guid id);
        List<User> GetWithPosts(Guid id);
        Guid Update(Guid id, string firstName, string secondName, string bio);
    }
}