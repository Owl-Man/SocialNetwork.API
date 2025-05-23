﻿using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.Application.Services;

public class PostsService : IPostsService
{
    private readonly IPostsRepository _postsRepository;

    public PostsService(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public List<Post>? GetAllPosts()
    {
        return _postsRepository.GetAll();
    }

    public List<Post>? GetByAuthor(Guid authorId)
    {
        return _postsRepository.GetByAuthor(authorId);
    }

    public List<Post>? GetByFilter(string searchValue)
    {
        return  _postsRepository.GetByFilter(searchValue);
    }

    public List<Post>? GetByTopic(Topic topic)
    {
        return  _postsRepository.GetByTopic(topic);
    }

    public (Guid, string) CreatePost(Guid authorID, string title, string content, Topic topic)
    {
        return _postsRepository.Create(authorID, title, content, topic);
    }

    public (Guid, string) UpdatePost(Guid id, string title, string content, Topic topic)
    {
        return  _postsRepository.Update(id, title, content, topic);
    }

    public async Task<(Guid, string)> UpvoteToPost(Guid id) => await _postsRepository.VoteToPost(id, true);

    public async Task<(Guid, string)> DownvoteToPost(Guid id) => await _postsRepository.VoteToPost(id, false);

    public Guid? DeletePost(Guid id)
    {
        return  _postsRepository.Delete(id);
    }
}
