using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Repositories
{
    public interface IPostsRepository
    {
        Guid Create(Post post);
        Guid Delete(Guid id);
        List<Post> GetAll();
        List<Post> GetByAuthor(User author);
        List<Post> GetByFilter(string searchValue);
        List<Post> GetByTopic(Topic topic);
        Guid Update(Guid id, string title, string content);
        List<Post> GetPostsByAuthorId(Guid authorId);
    }
}