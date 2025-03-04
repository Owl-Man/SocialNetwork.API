using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Application.Services;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(IPostsService postsService) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<PostsResponse>> GetPosts() 
    {
        var posts = postsService.GetAllPosts();

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<Guid> CreatePost([FromBody] PostsRequest request) 
    {
        var post = new Post(Guid.NewGuid(), request.title, request.content, new User(Guid.NewGuid(), "First", "Second", "Bio"), request.topic);

        var postId = postsService.CreatePost(post);

        return Ok(postId);
    }

    [HttpGet("getByAuthorId")] 
    public ActionResult<List<PostsResponse>> GetPostsByAuthorId([FromQuery] Guid guild_id)
    {
        try
        {
            var posts = postsService.GetPostsByAuthorId(guild_id);

            if (posts == null || !posts.Any())
            {
                return NotFound($"No posts found for author with id: {guild_id}");
            }

            var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic)).ToList();
            return Ok(response);
        }
        catch (Exception ex)
        {
            // Логирование ошибки (важно!)
            Console.Error.WriteLine($"Error getting posts by author id {guild_id}: {ex}");
            return StatusCode(500, "Internal Server Error");
        }
    } 
}