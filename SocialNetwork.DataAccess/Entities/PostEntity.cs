namespace SocialNetwork.DataAccess.Entities;

public class PostEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Author { get; set; }
    
    public int Upvotes { get; set; }
}