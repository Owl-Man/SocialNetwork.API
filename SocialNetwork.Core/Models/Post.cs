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

    public DateTime PublishTime { get; }

    public int UpvotesNumber { get; }

    public Post(Guid id, string title, string content, User author, Topic topic, DateTime publishTime, int upvotesNumber)
    {
        Id = id;
        Title = title;
        Content = content;
        Author = author;
        AuthorId = author.Id;
        Topic = topic;
        PublishTime = publishTime;
        UpvotesNumber = upvotesNumber;
    }

    public static (Post post, string Error) Create(Guid id, string title, string content, User author, Topic topic, DateTime publishTime, int upvotesNumber) 
    {
        var error = CheckPostDataValid(title);

        if (!string.IsNullOrEmpty(error))
            return (new Post(Guid.Empty, error, error, author, topic, publishTime, upvotesNumber), error);

        var post = new Post(id, title, content, author, topic, publishTime, upvotesNumber);

        return (post, error);
    }

    public static string CheckPostDataValid(string title)
    {
        if (string.IsNullOrEmpty(title) || title.Length > MaxTitleLength)
        {
            if (string.IsNullOrEmpty(title))
                return "Название не может быть пустым";
            else
                return "Нзвание не может быть длинее чем " + MaxTitleLength;
        }

        return string.Empty;
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