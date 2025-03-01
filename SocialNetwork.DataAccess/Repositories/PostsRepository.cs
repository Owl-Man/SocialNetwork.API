using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Repositories;

public class PostsRepository
{
    private readonly SocialNetworkDbContext _context;

    public PostsRepository(SocialNetworkDbContext context)
    {
        _context = context;
    }

    public async Task<List<Post>> GetAll()
    {
        var postEntities = await _context.Posts
            .AsNoTracking()
            .ToListAsync();

        var posts = postEntities
            .Select(p =>
            {
                User author = new(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio);

                return new Post(p.Id, p.Title, p.Content, author, p.Topic);
            })
            .ToList();

        return posts;
    }

    public async Task<List<Post>> GetByAuthor(User author) 
    {
        var postEntities = await _context.Posts
            .AsNoTracking()
            .ToListAsync();

        var posts = postEntities
            .Select(p =>
            {
                User author = new(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio);

                return new Post(p.Id, p.Title, p.Content, author, p.Topic);
            })
            .ToList();

        return posts;
    }

    public async Task<Guid> Create(Post post)
    {
        var userEntity = new UserEntity()
        {
            Id = post.Author.Id,
            FirstName = post.Author.FirstName,
            SecondName = post.Author.SecondName,
            Bio = post.Author.Bio
        };

        var postEntity = new PostEntity()
        {
            Id = post.Id,
            Title = post.Title,
            Author = userEntity,
            Content = post.Content
        };

        await _context.Posts.AddAsync(postEntity);
        await _context.SaveChangesAsync();

        return postEntity.Id;
    }

    public async Task<Guid> Update(Guid id, string title, string content)
    {
        await _context.Posts
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Title, p => title)
                .SetProperty(p => p.Content, p => content));

        return id;
    }

    public async Task<Guid> Delete(Guid id) 
    {
        await _context.Posts
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}