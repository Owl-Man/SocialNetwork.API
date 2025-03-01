namespace SocialNetwork.Core.Models;

public class User
{
    public Guid Id;

    public string FirstName { get; } = string.Empty;

    public string SecondName { get; } = string.Empty;

    public string Bio { get; } = string.Empty;

    public List<Post> Posts { get; } = new();

    public User(Guid id, string firstName, string secondName, string bio)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(secondName))
        {
            //Error
        }

        Id = id;
        FirstName = firstName;
        SecondName = secondName;
        Bio = bio;
    }
}