using SocialNetwork.Core.Models;

namespace SocialNetwork.Core.Abstractions
{
    public interface IFeedService
    {
        (List<Post>, string) GetUserFeed(Guid userId);
    }
}