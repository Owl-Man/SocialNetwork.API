using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using SocialNetwork.API.Contracts;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController(IPostsService postsService, IUserService userService) : ControllerBase
{
    [HttpGet("GetAllPosts")]
    public ActionResult<List<PostsResponse>> GetPosts()
    {
        List<Post>? posts = postsService.GetAllPosts();

        if (posts == null)
        {
            return NotFound($"No posts found");
        }

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpGet("GetByAuthorId")]
    public ActionResult<List<PostsResponse>> GetPostsByAuthorId([FromBody] OnlyId request)
    {
        List<Post>? posts = postsService.GetByAuthor(request.Id);

        if (posts == null)
        {
            return NotFound($"No posts found by author with id: {request.Id}");
        }

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpGet("GetPostsByFilterSearch")]
    public ActionResult<List<PostsResponse>> GetPostsByFilter([FromBody] GetFilteredPostsRequest request) 
    {
        List<Post>? posts = postsService.GetByFilter(request.searchValue);

        if (posts == null)
        {
            return NotFound($"No posts found by filter: {request.searchValue}");
        }

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpPost("CreatePost")]
    public ActionResult<Guid> CreatePost([FromBody] CreatePostDataRequest request)
    {
        Guid postId = postsService.CreatePost(request.AuthorId, request.Title, request.Content, request.Topic);

        return Ok(new OnlyId(postId));
    }

    [HttpPut("UpdatePostById")]
    public ActionResult<Guid> UpdatePost([FromBody] ChangePostDataRequest request) 
    {
        Guid? postId = postsService.UpdatePost(request.Id, request.Title, request.Content);

        if (postId == null)
        {
            return NotFound($"No posts found for author with id: {request.Id}");
        }

        return Ok(new OnlyId(request.Id));
    }

    [HttpDelete("DeletePostById")]
    public ActionResult<Guid> DeletePost([FromBody] OnlyId request)
    {
        Guid? postId = postsService.DeletePost(request.Id);

        if (postId == null)
        {
            return NotFound($"No posts found for author with id: {request.Id}");
        }

        return Ok(new OnlyId(request.Id));
    }
}