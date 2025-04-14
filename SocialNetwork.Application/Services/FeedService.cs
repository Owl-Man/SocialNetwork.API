using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.Application.Services;

public class FeedService : IFeedService
{
    private readonly IPostsRepository _postsRepository;
    private readonly IUsersRepository _usersRepository;

    private const int FeedSize = 50, FetchSize = 100, MaxPostAgeDays = 20;

    public FeedService(IPostsRepository postsRepository, IUsersRepository usersRepository)
    {
        _postsRepository = postsRepository;
        _usersRepository = usersRepository;
    }

    public (List<Post>, string) GetUserFeed(Guid userId)
    {
        User? user = _usersRepository.GetById(userId);

        string error = string.Empty;

        if (user == null)
        {
            error = "Пользователь не найден c id " + userId;

            return (new List<Post>(), error);
        }

        if (user.PreferredTopics.Count == 0) return (_postsRepository.GetByTopicsOnPage(new List<Topic>() { Topic.Art }, 1, FetchSize), error);

        List<Post> postPool = _postsRepository.GetByTopicsOnPage(user.PreferredTopics, 1, FetchSize);

        DateTime currentTime = DateTime.Now;

        List<Post> feed = postPool
            .Select(p => new
            {
                Post = p,
                Value = CalculatePostValue(p, currentTime)
            })
            .OrderByDescending(p => p.Value)
            .Take(FeedSize)
            .Select(x => x.Post)
            .ToList();

        return (feed, error);
    }

    private double CalculatePostValue(Post post, DateTime currentTime)
    {
        double ageInDays = (currentTime - post.PublishTime).TotalDays;
        double ageValue = Math.Max(0, 1.0 - (ageInDays / MaxPostAgeDays));

        double popularityValue = Math.Log10(1 + post.UpvotesNumber);

        //70 % актуальность + 30 % популярность
        double value = 0.7 * ageValue + 0.3 * popularityValue;

        return value;
    }
}
