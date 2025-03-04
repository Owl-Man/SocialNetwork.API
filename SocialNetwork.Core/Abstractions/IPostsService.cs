﻿using SocialNetwork.Core.Models;

namespace SocialNetwork.Application.Services
{
    public interface IPostsService
    {
        Guid CreatePost(Post post);
        Guid DeletePost(Guid id);
        List<Post> GetAllPosts();
        List<Post> GetByAuthor(Guid authorId);
        List<Post> GetByFilter(string searchValue);
        List<Post> GetByTopic(Topic topic);
        Guid UpdatePost(Guid id, string title, string content);
    }
}