using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record PostResponse(Guid Id, string Title, string Content, Guid AuthorId, Topic Topic, DateTime PublishTime, int UpvoteNumber);
