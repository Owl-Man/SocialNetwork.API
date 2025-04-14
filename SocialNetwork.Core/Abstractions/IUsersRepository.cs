using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IUsersRepository
    {
        List<User> GetAll();
        User? GetById(Guid id);
        List<User> GetWithPosts(Guid id);
        (Guid, string) Create(string firstName, string secondName, string bio);
        (Guid, string) Update(Guid id, string firstName, string secondName, string bio, List<Topic> preferredTopics);
        Guid? UpdatePreferredTopics(Guid id, List<Topic> preferredTopics);
        Guid? Delete(Guid id);
    }
}