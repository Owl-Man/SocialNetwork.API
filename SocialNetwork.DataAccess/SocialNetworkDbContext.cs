using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataAccess.Configurations;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess;

public class SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    
    public DbSet<PostVoteEntity> PostVotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}