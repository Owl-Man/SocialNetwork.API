using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess;

public class SocialNetworkDbContext : DbContext
{
    public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<PostEntity> Posts { get; set; }
}