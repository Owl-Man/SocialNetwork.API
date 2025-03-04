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

    public List<Post> GetAllPosts()
    {
        return _postsRepository.GetAll();
    }

    public  List<Post> GetByAuthor(User author)
    {
        return  _postsRepository.GetByAuthor(author);
    }

    public  List<Post> GetByFilter(string searchValue)
    {
        return  _postsRepository.GetByFilter(searchValue);
    }

    public  List<Post> GetByTopic(Topic topic)
    {
        return  _postsRepository.GetByTopic(topic);
    }

    public  Guid CreatePost(Post post)
    {
        return  _postsRepository.Create(post);
    }

    public  Guid UpdatePost(Guid id, string title, string content)
    {
        return  _postsRepository.Update(id, title, content);
    }

    public  Guid DeletePost(Guid id)
    {
        return  _postsRepository.Delete(id);
    }
}
