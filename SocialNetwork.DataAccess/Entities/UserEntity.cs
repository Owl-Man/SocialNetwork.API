namespace SocialNetwork.DataAccess.Entities;

public class UserEntity
{
    public Guid Id;

    public string FirstName { get; set; } = string.Empty;

    public string SecondName { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
    
    public List<PostEntity> Posts { get; set; } = new();
}