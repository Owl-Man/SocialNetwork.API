using SocialNetwork.Core.Models;

namespace SocialNetwork.Application.Services
{
    public interface IUserService
    {
        List<User> GetAll();
        Guid Create(User user);
        Guid Delete(Guid id);
        User? GetById(Guid id);
        List<User> GetWithPosts(Guid id);
        Guid Update(Guid id, string firstName, string secondName, string bio);
    }
}