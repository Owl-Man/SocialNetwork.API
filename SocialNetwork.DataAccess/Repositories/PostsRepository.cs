using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Repositories;

public class PostsRepository(SocialNetworkDbContext context, ILogger<PostsRepository> logger) : IPostsRepository
{
    public List<Post>? GetAll() 
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
                    User author = User.Create(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio, p.Author.PreferredTopics).user;

                    return Post.Create(p.Id, p.Title, p.Content, author, p.Topic, p.PublishTime, p.UpvotesNumber).post;
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
            //var postEntities = context.Posts
            //    .Include(entity => entity.Author)
            //    .AsNoTracking()
            //    .Where(p => p.AuthorId == authorId)
            //    .ToList();

            List<PostEntity> postEntities = context.Users
                .Include(entity => entity.Posts)
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == authorId).Posts;

            var posts = postEntities
                .Select(p => Post.Create(
                    p.Id,
                    p.Title,
                    p.Content,
                    User.Create(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio, p.Author.PreferredTopics).user,
                    p.Topic,
                    p.PublishTime,
                    p.UpvotesNumber
                ).post)
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
                .Select(p => Post.Create(
                    p.Id,
                    p.Title,
                    p.Content,
                    User.Create(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio, p.Author.PreferredTopics).user,
                    p.Topic,
                    p.PublishTime,
                    p.UpvotesNumber
                ).post)
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
                .Select(p => Post.Create(
                    p.Id,
                    p.Title,
                    p.Content,
                    User.Create(p.Author.Id, p.Author.FirstName, p.Author.SecondName, p.Author.Bio, p.Author.PreferredTopics).user,
                    p.Topic,
                    p.PublishTime,
                    p.UpvotesNumber
                ).post)
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

            var post = Post.Create(
                    postEntity.Id,
                    postEntity.Title,
                    postEntity.Content,
                    User.Create(postEntity.Author.Id, postEntity.Author.FirstName, postEntity.Author.SecondName, postEntity.Author.Bio, postEntity.Author.PreferredTopics).user,
                    postEntity.Topic,
                    postEntity.PublishTime,
                    postEntity.UpvotesNumber).post;

            return post;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении поста {id}.", id);
            return null;
        }
    }

    public (Guid, string) Create(Guid authorID, string title, string content, Topic topic)
    {
        try
        {
            var userEntity = context.Users.FirstOrDefault(u => u.Id == authorID);

            var postEntity = new PostEntity()
            {
                Title = title,
                AuthorId = authorID,
                Author = userEntity,
                Content = content,
                Topic = topic,
                PublishTime = DateTime.UtcNow,
                UpvotesNumber = 0
            };

            var error = Post.CheckPostDataValid(title);

            if (!string.IsNullOrEmpty(error)) return (Guid.Empty, error);

            context.Posts.Add(postEntity);
            context.SaveChanges();

            return (postEntity.Id, error);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании поста с title {title}.", title);
            throw;
        }
    }

    public (Guid, string) Update(Guid id, string title, string content)
    {
        try
        {
            var postEntity = context.Posts
                .FirstOrDefault(p => p.Id == id);

            if (postEntity == null)
            {
                string idError = $"Не найден пост для обновления с ID {id}";
                logger.LogError(idError);
                return (id, idError);
            }

            var error = Post.CheckPostDataValid(title);

            if (!string.IsNullOrEmpty(error)) return (Guid.Empty, error);

            postEntity.Title = title; 
            postEntity.Content = content;

            context.SaveChanges();

            return (id, error);
        }
        catch (Exception ex)
        {
            string error = $"Ошибка при обновлении поста с ID {id}";
            logger.LogError(ex, error);
            return (id, error);
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