using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record PostsRequest(string title, string content, Guid authorId, Topic topic);