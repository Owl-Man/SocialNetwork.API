using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IPostsRepository
    {
        (Guid, string) Create(Guid authorID, string title, string content, Topic topic);
        Guid? Delete(Guid id);
        List<Post>? GetAll();
        List<Post>? GetByAuthor(Guid authorId);
        List<Post>? GetByFilter(string searchValue);
        List<Post>? GetByTopic(Topic topic);
        List<Post> GetByTopicsOnPage(List<Topic> topics, int page, int pageSize);
        (Guid, string) Update(Guid id, string title, string content, Topic topic);
        Task<(Guid, string)> VoteToPost(Guid id, bool isUpvote);
    }
}