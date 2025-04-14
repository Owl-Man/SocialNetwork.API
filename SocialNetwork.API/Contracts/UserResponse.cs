using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record UserResponse(Guid id, string FirstName, string SecondName, string Bio, List<Topic> PreferredTopics);
