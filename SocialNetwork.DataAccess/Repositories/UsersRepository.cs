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
                .Select(u => new User(u.Id, u.FirstName, u.SecondName, u.Bio))
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
                .Select(u => new User(u.Id, u.FirstName, u.SecondName, u.Bio))
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

            var user = new User(userEntity.Id, userEntity.FirstName, userEntity.SecondName, userEntity.Bio);
            
            return user;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при получении пользователя {UserId}.", id);
            throw;
        }
    }

    public Guid Create(string firstName, string secondName, string bio)
    {
        try
        {
            var userEntity = new UserEntity()
            {
                FirstName = firstName,
                SecondName = secondName,
                Bio = bio
            };

            context.Users.AddAsync(userEntity);
            context.SaveChangesAsync();

            return userEntity.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при создании пользователя: {firstName}", firstName);
            throw;
        }
    }

    public Guid Update(Guid id, string firstName, string secondName, string bio)
    {
        try
        {
            context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdate(s => s
                .SetProperty(u => u.FirstName, u => firstName)
                .SetProperty(u => u.SecondName, u => secondName)
                .SetProperty(u => u.Bio, u => bio)
                );

            return id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка при обновлении пользователя с Id {UserId}.", id);
            throw;
        }
    }

    public Guid Delete(Guid id)
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
            throw;
        }
    }
}