using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record PostsResponse(List<Post> posts);