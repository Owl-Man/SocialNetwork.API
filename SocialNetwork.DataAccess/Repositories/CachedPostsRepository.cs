using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.DataAccess.Repositories;

public class CachedPostsRepository : IPostsRepository
{
    private readonly PostsRepository _decorated;
    private readonly IDistributedCache _distributedCache;

    public CachedPostsRepository(PostsRepository postsRepository, IDistributedCache distributedCache)
    {
        _decorated = postsRepository;
        _distributedCache = distributedCache;
    }

    public (Guid, string) Create(Guid authorID, string title, string content, Topic topic)
    {
        TryDeletePostsFromCacheByAuthorid(authorID);
        return _decorated.Create(authorID, title, content, topic);
    }

    public List<Post> GetAll() => _decorated.GetAll();

    public List<Post>? GetByAuthor(Guid authorId)
    {
        string key = $"Posts-withAuthor-{authorId}";

        string? cachedPosts = _distributedCache.GetString(key);

        List<Post>? posts;

        if (string.IsNullOrEmpty(cachedPosts))
        {
            posts = _decorated.GetByAuthor(authorId);

            if (posts.Count == 0)
            {
                return posts;
            }

            _distributedCache.SetString(key, JsonConvert.SerializeObject(posts));
        }
        else
        {
            posts = JsonConvert.DeserializeObject<List<Post>>(cachedPosts, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            }) ?? new List<Post>();

            //ONLY FOR TESTING API
            Post originPost = posts[0];

            posts[0] = Post.Create(originPost.Id, "From redis cache: " + originPost.Title, originPost
                .Content, originPost.Author, originPost.Topic, originPost.PublishTime, originPost.UpvotesNumber).post;
        }

        return posts;
    }

    public List<Post>? GetByFilter(string searchValue) => _decorated.GetByFilter(searchValue);

    public List<Post>? GetByTopic(Topic topic) => _decorated.GetByTopic(topic);

    public (Guid, string) Update(Guid id, string title, string content)
    {
        TryDeletePostsFromCacheById(id);

        return _decorated.Update(id, title, content);
    }

    public Guid? Delete(Guid id)
    {
        TryDeletePostsFromCacheById(id);
        return _decorated.Delete(id);
    }

    private void TryDeletePostsFromCacheById(Guid id)
    {
        Post? post = _decorated.GetById(id);

        if (post != null)
        {
            TryDeletePostsFromCacheByAuthorid(post.AuthorId);
        }
    }

    private void TryDeletePostsFromCacheByAuthorid(Guid authorid)
    {
        string key = $"Posts-withAuthor-{authorid}";

        string? cachedPosts = _distributedCache.GetString(key);

        if (!string.IsNullOrEmpty(cachedPosts))
        {
            _distributedCache.Remove(key);
        }
    }
}
