using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record PostsResponse(Guid id, string title, string content, Guid authorId, Topic topic);