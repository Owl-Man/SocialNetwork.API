using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IPostsRepository
    {
        Guid Create(Guid authorID, string title, string content, Topic topic);
        Guid Delete(Guid id);
        List<Post> GetAll();
        List<Post> GetByAuthor(Guid authorId);
        List<Post> GetByFilter(string searchValue);
        List<Post> GetByTopic(Topic topic);
        Guid Update(Guid id, string title, string content);
    }
}