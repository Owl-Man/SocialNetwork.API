using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(IPostsService postsService, IUserService userService) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<PostsResponse>> GetPosts()
    {
        var posts = postsService.GetAllPosts();

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpGet("GetByAuthorId")]
    public ActionResult<List<PostsResponse>> GetPostsByAuthorId([FromQuery] Guid id)
    {
        var posts = postsService.GetByAuthor(id);

        if (posts == null || posts.Count == 0)
        {
            return NotFound($"No posts found for author with id: {id}");
        }

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<Guid> CreatePost([FromBody] PostsRequest request)
    {
        var postId = postsService.CreatePost(request.authorId, request.title, request.content, request.topic);

        return Ok(postId);
    }
}