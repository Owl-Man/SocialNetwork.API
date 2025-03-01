using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Entities;

namespace SocialNetwork.DataAccess.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Author)
            .IsRequired();

        builder.Property(p => p.Title)
            .HasMaxLength(Post.MaxTitleLength)
            .IsRequired();

        builder.HasOne(p => p.Author)
            .WithMany(p => p.Posts);
    }
}