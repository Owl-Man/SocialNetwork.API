using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IUserService
    {
        List<User> GetAll();
        (Guid, string) Create(string firstName, string secondName, string bio);
        Guid? Delete(Guid id);
        User? GetById(Guid id);
        List<User> GetWithPosts(Guid id);
        (Guid, string) Update(Guid id, string firstName, string secondName, string bio, List<Topic> preferredTopics);
        Guid? UpdatePreferredTopics(Guid id, List<Topic> preferredTopics);
    }
}