using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record CreatePostDataRequest(string Title, string Content, Guid AuthorId, Topic Topic);