namespace SocialNetwork.Core.Models;

public class Post
{
    public const int MaxTitleLength = 300;
    public Guid Id { get; }
    public string Title { get; } = string.Empty;
   
    public string Content { get; } = string.Empty;

    public Guid AuthorId { get; }

    public User? Author { get; }

    public Topic Topic { get; set; }

    public Post(Guid id, string title, string content, User author, Topic topic)
    {
        if (string.IsNullOrEmpty(title) || title.Length > MaxTitleLength)
        {
            //Error
        }
        
        Id = id;
        Title = title;
        Content = content;
        Author = author;
        AuthorId = author.Id;
        Topic = topic;
    }

    public static (Post post, string Error) Create(Guid id, string title, string content, User author, Topic topic) 
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(title) || title.Length > MaxTitleLength)
        {
            error = "Title cannot be empty or longer then " + MaxTitleLength;
        }

        var post = new Post(id, title, content, author, topic);

        return (post, error);
    }
}

public enum Topic
{
    Technology,
    Science,
    Gaming,
    Sports,
    Music,
    Movies,
    Books,
    Travel,
    Food,
    Fashion,
    Health,
    Fitness,
    Art,
    Photography,
    Politics,
    Business,
    Education,
    Nature
}