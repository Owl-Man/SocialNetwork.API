using SocialNetwork.Core.Models;
using SocialNetwork.DataAccess.Repositories;

namespace SocialNetwork.Application.Services;

public class PostsService : IPostsService
{
    private readonly IPostsRepository _postsRepository;

    public PostsService(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public async Task<List<Post>> GetAllPosts()
    {
        return await _postsRepository.GetAll();
    }

    public async Task<List<Post>> GetByAuthor(User author)
    {
        return await _postsRepository.GetByAuthor(author);
    }

    public async Task<List<Post>> GetByFilter(string searchValue)
    {
        return await _postsRepository.GetByFilter(searchValue);
    }

    public async Task<List<Post>> GetByTopic(Topic topic)
    {
        return await _postsRepository.GetByTopic(topic);
    }

    public async Task<Guid> CreatePost(Post post)
    {
        return await _postsRepository.Create(post);
    }

    public async Task<Guid> UpdatePost(Guid id, string title, string content)
    {
        return await _postsRepository.Update(id, title, content);
    }

    public async Task<Guid> DeletePost(Guid id)
    {
        return await _postsRepository.Delete(id);
    }
}
