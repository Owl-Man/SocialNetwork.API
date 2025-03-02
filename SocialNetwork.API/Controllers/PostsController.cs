using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Application.Services;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostsService _postsService;

    public PostsController(IPostsService postsService)
    {
        _postsService = postsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PostsResponse>>> GetPosts() 
    {
        var posts = await _postsService.GetAllPosts();

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }
}