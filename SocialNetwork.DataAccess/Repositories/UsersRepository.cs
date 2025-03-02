using Microsoft.EntityFrameworkCore;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly SocialNetworkDbContext _context;

    public UsersRepository(SocialNetworkDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetWithPosts(Guid id)
    {
        var userEntities = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Include(u => u.Posts)
            .ToListAsync();

        var users = userEntities
            .Select(u => new User(u.Id, u.FirstName, u.SecondName, u.Bio))
            .ToList();

        return users;
    }

    public async Task<User?> GetById(Guid id)
    {
        var userEntity = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        var user = new User(userEntity.Id, userEntity.FirstName, userEntity.SecondName, userEntity.Bio);

        return user;
    }

    public async Task<Guid> Create(User user)
    {
        var userEntity = new UserEntity()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            SecondName = user.SecondName,
            Bio = user.Bio
        };

        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();

        return userEntity.Id;
    }
    public async Task<Guid> Update(Guid id, string firstName, string secondName, string bio)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(u => u.FirstName, u => firstName)
                .SetProperty(u => u.SecondName, u => secondName)
                .SetProperty(u => u.Bio, u => bio)
                );

        return id;
    }

    public async Task<Guid> Delete(Guid id)
    {
        await _context.Users
            .Where(u => u.Id == id)
            .ExecuteDeleteAsync();

        return id;
    }
}