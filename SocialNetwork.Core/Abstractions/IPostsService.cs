using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IPostsService
    {
        Guid CreatePost(Guid authorID, string title, string content, Topic topic);
        Guid DeletePost(Guid id);
        List<Post> GetAllPosts();
        List<Post> GetByAuthor(Guid authorId);
        List<Post> GetByFilter(string searchValue);
        List<Post> GetByTopic(Topic topic);
        Guid UpdatePost(Guid id, string title, string content);
    }
}