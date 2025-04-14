using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedController : ControllerBase
{
    private readonly IFeedService _feedService;

    public FeedController(IFeedService feedService)
    {
        _feedService = feedService;
    }

    [HttpGet("GetFeedForUser")]
    public ActionResult<PostsResponse> GetUesrFeed([FromBody] OnlyId userId)
    {
        (List<Post> posts, string error) = _feedService.GetUserFeed(userId.Id);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        PostsResponse response = new PostsResponse(posts);

        return Ok(posts);
    }
}
