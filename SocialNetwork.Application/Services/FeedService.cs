using SocialNetwork.Core.Abstractions;

namespace SocialNetwork.Application.Services;

public class FeedService
{
    private readonly IPostsRepository _postsRepository;

    public FeedService(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }
}
