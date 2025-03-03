using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Application.Services;
using SocialNetwork.Core.Models;

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

    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePost([FromBody] PostsRequest request) 
    {
        var post = new Post(Guid.NewGuid(), request.title, request.content, new User(Guid.NewGuid(), "First", "Second", "Bio"), request.topic);

        var postId = await _postsService.CreatePost(post);

        return Ok(postId);
    }
}