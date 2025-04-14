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
    public ActionResult<List<PostResponse>> GetPosts()
    {
        List<Post>? posts = postsService.GetAllPosts();

        if (posts == null || posts.Count == 0)
        {
            return NotFound($"Не найден постов");
        }

        var response = posts.Select(p => new PostResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic, p.PublishTime, p.UpvotesNumber));

        return Ok(response);
    }

    [HttpGet("GetByAuthorId")]
    public ActionResult<List<PostResponse>> GetPostsByAuthorId([FromBody] OnlyAuthorId request)
    {
        List<Post>? posts = postsService.GetByAuthor(request.AuthorId);

        if (posts == null || posts.Count == 0)
        {
            return NotFound($"Не найден постов от пользователя с ID: {request.AuthorId}");
        }

        var response = posts.Select(p => new PostResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic, p.PublishTime, p.UpvotesNumber));

        return Ok(response);
    }

    [HttpGet("GetPostsByFilterSearch")]
    public ActionResult<List<PostResponse>> GetPostsByFilter([FromBody] GetFilteredPostsRequest request) 
    {
        List<Post>? posts = postsService.GetByFilter(request.searchValue);

        if (posts == null || posts.Count == 0)
        {
            return NotFound($"Не найден постов с запросом: {request.searchValue}");
        }

        var response = posts.Select(p => new PostResponse(p.Id, p.Title, p.Content, p.AuthorId, p.Topic, p.PublishTime, p.UpvotesNumber));

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

    [HttpPut("UpdatePost")]
    public ActionResult<OnlyId> UpdatePost([FromBody] ChangePostDataRequest request) 
    {
        (Guid postId, string error) = postsService.UpdatePost(request.Id, request.Title, request.Content, request.Topic);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        return Ok(new OnlyId(postId));
    }

    [HttpPost("UpvotePost")]
    public async Task<ActionResult<OnlyId>> UpvotePostAsync([FromBody] OnlyId request)
    {
        (Guid postId, string error) = await postsService.UpvoteToPost(request.Id);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        return Ok(new OnlyId(postId));
    }

    [HttpPost("DownvotePost")]
    public async Task<ActionResult<OnlyId>> DownvotePostAsync([FromBody] OnlyId request)
    {
        (Guid postId, string error) = await postsService.DownvoteToPost(request.Id);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        return Ok(new OnlyId(postId));
    }

    [HttpDelete("DeletePost")]
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