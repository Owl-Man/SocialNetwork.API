using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Repositories;

public class UsersRepository(SocialNetworkDbContext context, ILogger<UsersRepository> logger) : IUsersRepository
{
    public List<User> GetAll() 
    {
        try
        {
            var userEntities = context.Users
            .AsNoTracking()
            .ToList();

            var users = userEntities
                .Select(u => User.Create(u.Id, u.FirstName, u.SecondName, u.Bio, u.PreferredTopics).user)
                .ToList();

            return users;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка получения всех пользователей.");
            throw;
        }
    }

    public List<User> GetWithPosts(Guid id)
    {
        try
        {
            var userEntities = context.Users
                .AsNoTracking()
                .Where(u => u.Id == id)
                .Include(u => u.Posts)
                .ToList();

            var users = userEntities
                .Select(u => User.Create(u.Id, u.FirstName, u.SecondName, u.Bio, u.PreferredTopics).user)
                .ToList();

            return users;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка получения всех пользователей.");
            throw;
        }
    }

    public User? GetById(Guid id)
    {
        try
        {
            var userEntity = context.Users
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);

            var user = User.Create(userEntity.Id, userEntity.FirstName, userEntity.SecondName, userEntity.Bio, userEntity.PreferredTopics).user;
            
            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении пользователя {UserId}.", id);
            return null;
        }
    }

    public (Guid, string) Create(string firstName, string secondName, string bio)
    {
        try
        {
            var userEntity = new UserEntity()
            {
                FirstName = firstName,
                SecondName = secondName,
                Bio = bio
            };

            var error = User.CheckUserDataValid(firstName, secondName);

            if (!string.IsNullOrEmpty(error)) return (Guid.Empty, error);

            context.Users.AddAsync(userEntity);
            context.SaveChangesAsync();

            return (userEntity.Id, error);
        }
        catch (Exception ex)
        {
            string error = $"Ошибка при создании пользователя: {firstName}";
            logger.LogError(ex, error);
            return (Guid.Empty, error);
        }
    }

    public (Guid, string) Update(Guid id, string firstName, string secondName, string bio, List<Topic> preferredTopics)
    {
        try
        {
            var error = User.CheckUserDataValid(firstName, secondName);

            if (!string.IsNullOrEmpty(error)) return (Guid.Empty, error);

            context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdate(s => s
                .SetProperty(u => u.FirstName, u => firstName)
                .SetProperty(u => u.SecondName, u => secondName)
                .SetProperty(u => u.Bio, u => bio)
                .SetProperty(u => u.PreferredTopics, u => preferredTopics)
                );

            return (id, error);
        }
        catch (Exception ex)
        {
            string error = $"Ошибка при обновлении пользователя с Id {id}";
            logger.LogError(ex, error);
            return (id, error);
        }
    }

    public Guid? UpdatePreferredTopics(Guid id, List<Topic> preferredTopics)
    {
        try
        {
            var userEntity = context.Users
                .FirstOrDefault(u => u.Id == id);

            if (userEntity == null) return null;

            userEntity.PreferredTopics = preferredTopics;
            context.SaveChanges();
            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении предпочтительных тем пользователя с Id {UserId}.", id);
            return null;
        }
    }

    public Guid? Delete(Guid id)
    {
        try
        {
            context.Users
            .Where(u => u.Id == id)
            .ExecuteDelete();

            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при удалении пользователя с Id {UserId}.", id);
            return null;
        }
    }
}