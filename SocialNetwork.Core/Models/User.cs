namespace SocialNetwork.Core.Models;

public class User
{
    public Guid Id;

    public string FirstName { get; } = string.Empty;

    public string SecondName { get; } = string.Empty;

    public string Bio { get; } = string.Empty;

    public List<Post> Posts { get; } = new();

    public List<Topic> PreferredTopics { get; } = new();

    public User(Guid id, string firstName, string secondName, string bio, List<Topic> preferredTopics)
    {
        Id = id;
        FirstName = firstName;
        SecondName = secondName;
        Bio = bio;
        PreferredTopics = preferredTopics;
    }

    public static (User user, string Error) Create(Guid id, string firstName, string secondName, string bio, List<Topic> preferredTopics)
    {
        var error = CheckUserDataValid(firstName, secondName);

        if (!string.IsNullOrEmpty(error)) return (new User(Guid.Empty, error, error, error, preferredTopics), error);

        var user = new User(id, firstName, secondName, bio, preferredTopics);

        return (user, error);
    }


    public static string CheckUserDataValid(string firstName, string secondName)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(secondName))
        {
            if (string.IsNullOrEmpty(firstName))
                return "Имя не может быть пустым";
            else
                return "Фамилия не может быть пустой";
        }

        return string.Empty;
    }
}