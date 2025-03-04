using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Repositories;

public class PostsRepository(SocialNetworkDbContext context, ILogger<PostsRepository> logger) : IPostsRepository {
    public List<Post> GetAll() {
        try {
            var postEntities = context.Posts
                .Include(entity => entity.Author)
                .AsNoTracking()
                .ToList();
            var posts = postEntities
                .Select(p =>
                {
                    User author = new(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio);

                    return new Post(p.Id, p.Title, p.Content, author, p.Topic);
                })
                .ToList();

            return posts;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Ошибка при получении всех постов.");
            throw;
        }
    }

    public List<Post> GetByAuthor(User author) {
        try {
            var postEntities = context.Posts
                .Include(entity => entity.Author)
                .AsNoTracking()
                .Where(p => p.AuthorId == author.Id)
                .ToList();

            var posts = postEntities
                .Select(p => new Post(
                    p.Id,
                    p.Title,
                    p.Content,
                    new User(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio),
                    p.Topic
                ))
                .ToList();

            return posts;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Ошибка при получении постов автора {AuthorId}.", author.Id);
            throw;
        }
    }

    public List<Post> GetByTopic(Topic topic)
    {
        try
        {
            var postEntities = context.Posts
                .Include(entity => entity.Author)
                .AsNoTracking()
                .Where(p => p.Topic.Equals(topic))
                .ToList();

            var posts = postEntities
                .Select(p => new Post(
                    p.Id,
                    p.Title,
                    p.Content,
                    new User(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio),
                    p.Topic
                ))
                .ToList();

            return posts;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении постов по теме {Topic}.", topic);
            throw;
        }
    }

    public List<Post> GetByFilter(string searchValue)
    {
        try
        {
            var query = context.Posts.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(p => p.Title.Contains(searchValue));
            }

            return query
                .Select(p => new Post(
                    p.Id,
                    p.Title,
                    p.Content,
                    new User(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio),
                    p.Topic
                ))
                .ToList();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении постов по фильтру {SearchValue}.", searchValue);
            throw;
        }
    }

    public Guid Create(Post post)
    {
        try
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

            context.Posts.Add(postEntity);
            context.SaveChanges();

            return postEntity.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании поста с Id {PostId}.", post.Id);
            throw;
        }
    }

    public Guid Update(Guid id, string title, string content)
    {
        try
        {
            var postEntity = context.Posts.Find(id); 
            if (postEntity == null)
            {
                logger.LogError("Не найден пост для обновления с ID {PostId}.", id);
                throw new ArgumentException($"Не найден пост для обновления с ID {id}.");
            }

            postEntity.Title = title; 
            postEntity.Content = content;

            context.SaveChanges();

            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении поста с Id {PostId}.", id);
            throw;
        }
    }

    public List<Post> GetPostsByAuthorId(Guid authorId)
    {
        try
        {
            var postEntities = context.Posts
                .Include(entity => entity.Author)
                .AsNoTracking()
                .Where(p => p.AuthorId == authorId)
                .ToList();

            var posts = postEntities
                .Select(p => new Post(
                    p.Id,
                    p.Title,
                    p.Content,
                    new User(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio),
                    p.Topic
                ))
                .ToList();

            return posts;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении постов автора с Id {AuthorId}.", authorId);
            throw;
        }
    }

    public Guid Delete(Guid id)
    {
        try
        {
            var postEntity = context.Posts.Find(id);
            if (postEntity == null)
            {
                logger.LogError("Не найден пост для удаления с ID {PostId}.", id);
                throw new ArgumentException($"Не найден пост для удаления с ID {id}.");
            }

            context.Posts.Remove(postEntity);
            context.SaveChanges();

            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при удалении поста с Id {PostId}.", id);
            throw;
        }
    } 
}