using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record ChangeUserDataRequest(Guid Id, string FirstName, string SecondName, string Bio, List<Topic> PreferredTopics);