using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Repositories
{
    public interface IPostsRepository
    {
        Task<Guid> Create(Post post);
        Task<Guid> Delete(Guid id);
        Task<List<Post>> GetAll();
        Task<List<Post>> GetByAuthor(User author);
        Task<List<Post>> GetByFilter(string searchValue);
        Task<List<Post>> GetByTopic(Topic topic);
        Task<Guid> Update(Guid id, string title, string content);
    }
}