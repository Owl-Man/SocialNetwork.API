namespace SocialNetwork.Core.Models;

public class Post
{
    public const int MaxTitleLength = 300;
    public Guid Id { get; }
    public string Title { get; } = string.Empty;
    
    public string Content { get; } = string.Empty;
    public string Author { get; }

    public Post(Guid id, string title, string content, string author)
    {
        if (string.IsNullOrEmpty(title) || title.Length > MaxTitleLength)
        {
            //Error
        }
        
        Id = id;
        Title = title;
        Author = author;
    }
}