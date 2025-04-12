namespace SocialNetwork.API.Contracts;

public record ChangeUserDataRequest(Guid Id, string FirstName, string SecondName, string Bio);