using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController(IPostsService postsService) : ControllerBase
{
    [HttpGet("GetAllPosts")]
    public ActionResult<List<PostsResponse>> GetPosts()
    {
        List<Post>? posts = postsService.GetAllPosts();

        if (posts == null || posts.Count == 0)
        {
            return NotFound($"Не найден постов");
        }

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpGet("GetByAuthorId")]
    public ActionResult<List<PostsResponse>> GetPostsByAuthorId([FromBody] OnlyId request)
    {
        List<Post>? posts = postsService.GetByAuthor(request.Id);

        if (posts == null || posts.Count == 0)
        {
            return NotFound($"Не найден постов от пользователя с ID: {request.Id}");
        }

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpGet("GetPostsByFilterSearch")]
    public ActionResult<List<PostsResponse>> GetPostsByFilter([FromBody] GetFilteredPostsRequest request) 
    {
        List<Post>? posts = postsService.GetByFilter(request.searchValue);

        if (posts == null || posts.Count == 0)
        {
            return NotFound($"Не найден постов с запросом: {request.searchValue}");
        }

        var response = posts.Select(p => new PostsResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic));

        return Ok(response);
    }

    [HttpPost("CreatePost")]
    public ActionResult<OnlyId> CreatePost([FromBody] CreatePostDataRequest request)
    {
        (Guid postId, string error) = postsService.CreatePost(request.AuthorId, request.Title, request.Content, request.Topic);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        return Ok(new OnlyId(postId));
    }

    [HttpPut("UpdatePostById")]
    public ActionResult<OnlyId> UpdatePost([FromBody] ChangePostDataRequest request) 
    {
        (Guid postId, string error) = postsService.UpdatePost(request.Id, request.Title, request.Content);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        return Ok(new OnlyId(request.Id));
    }

    [HttpDelete("DeletePostById")]
    public ActionResult<OnlyId> DeletePost([FromBody] OnlyId request)
    {
        Guid? postId = postsService.DeletePost(request.Id);

        if (postId == null)
        {
            return NotFound($"Не найден пост с ID: {request.Id}");
        }

        return Ok(new OnlyId(request.Id));
    }
}