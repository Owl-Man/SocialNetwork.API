using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Entities;

public class PostEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public UserEntity? Author { get; set; }
    public Topic Topic { get; set; }
}