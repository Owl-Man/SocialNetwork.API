namespace SocialNetwork.DataAccess.Entities;

public class PostVoteEntity
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public bool IsUpvote { get; set; }
}