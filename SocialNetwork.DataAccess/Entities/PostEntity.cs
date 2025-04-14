using SocialNetwork.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.DataAccess.Entities;

public class PostEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public UserEntity? Author { get; set; }
    public Topic Topic { get; set; }
    public DateTime PublishTime { get; set; } = DateTime.UtcNow;
    public int UpvotesNumber { get; set; } = 0;
}