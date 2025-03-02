using SocialNetwork.Core.Models;

namespace SocialNetwork.Application.Services
{
    public interface IPostsService
    {
        Task<Guid> CreatePost(Post post);
        Task<Guid> DeletePost(Guid id);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetByAuthor(User author);
        Task<List<Post>> GetByFilter(string searchValue);
        Task<List<Post>> GetByTopic(Topic topic);
        Task<Guid> UpdatePost(Guid id, string title, string content);
    }
}