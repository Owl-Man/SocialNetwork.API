using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Contracts;

public record UpdateUserPreferredTopicsRequest(Guid Id, List<Topic> Topics);
