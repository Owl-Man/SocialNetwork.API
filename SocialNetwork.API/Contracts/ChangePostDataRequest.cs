using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;
public record class ChangePostDataRequest(Guid Id, string Title, string Content, Topic Topic);
