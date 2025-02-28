using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Repositories;

public class PostRepository
{
    private readonly SocialNetworkDbContext _context;

    public PostRepository(SocialNetworkDbContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetAll()
    {
        var postEntities = await _context.Posts
            .AsNoTracking()
            .ToListAsync();

        var posts = postEntities
            .Select(p => new Post(p.Id, p.Title, p.Content, p.Author))
            .ToList();

        return posts;
    }

    public async Task<Guid> Create(Post post)
    {
        var postEntity = new PostEntity()
        {
            Id = post.Id,
            Title = post.Title,
            Author = post.Author,
            Content = post.Content
        };

        await _context.Posts.AddAsync(postEntity);
        await _context.SaveChangesAsync();

        return postEntity.Id;
    }
}