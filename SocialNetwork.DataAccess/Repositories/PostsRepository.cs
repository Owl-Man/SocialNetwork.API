using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Repositories;

public class PostsRepository(SocialNetworkDbContext context, ILogger<PostsRepository> logger) : IPostsRepository
{
    public List<Post> GetAll() 
    {
        try
        {
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
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении всех постов.");
            return null;
        }
    }

    public List<Post>? GetByAuthor(Guid authorId) 
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
            logger.LogError(ex, "Ошибка при получении постов автора {AuthorId}.", authorId);
            return null;
        }
    }

    public List<Post>? GetByTopic(Topic topic)
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
            return null;
        }
    }

    public List<Post>? GetByFilter(string searchValue)
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
            return null;
        }
    }

    public Post? GetById(Guid id)
    {
        try
        {
            var postEntity = context.Posts
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);

            var post = new Post(
                    postEntity.Id,
                    postEntity.Title,
                    postEntity.Content,
                    new User(postEntity.Author.Id, postEntity.Author.FirstName, postEntity.Author.SecondName, postEntity.Author.Bio),
                    postEntity.Topic);

            return post;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении поста {id}.", id);
            return null;
        }
    }

    public Guid Create(Guid authorID, string title, string content, Topic topic)
    {
        try
        {
            var userEntity = context.Users.FirstOrDefault(u => u.Id == authorID);

            var postEntity = new PostEntity()
            {
                Title = title,
                Author = userEntity,
                Content = content
            };

            context.Posts.Add(postEntity);
            context.SaveChanges();

            return postEntity.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании поста с title {title}.", title);
            throw;
        }
    }

    public Guid? Update(Guid id, string title, string content)
    {
        try
        {
            var postEntity = context.Posts
                .FirstOrDefault(p => p.Id == id);

            if (postEntity == null)
            {
                logger.LogError("Не найден пост для обновления с ID {PostId}.", id);
                return null;
            }

            postEntity.Title = title; 
            postEntity.Content = content;

            context.SaveChanges();

            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении поста с Id {PostId}.", id);
            return null;
        }
    }

    public Guid? Delete(Guid id)
    {
        try
        {
            var postEntity = context.Posts
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);

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
            return null;
        }
    } 
}