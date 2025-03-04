using SocialNetwork.Application.Services;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Controllers;
using SocialNetwork.API.Contracts;
using SocialNetwork.Core.Models;


namespace SocialNetwork.Core.test;

public class PostsControllerTest(Mock<IPostsService> postServiceMock, Mock<IUserService> userServiceMock, PostsController postsController) {
    private readonly Mock<IPostsService> postServiceMock = postServiceMock;
    private readonly Mock<IUserService> userServiceMock = userServiceMock;
    private readonly PostsController postsController = postsController;

    [Fact]
    public void getPosts_ReturnListOfPosts() {
        User user = new User(Guid.NewGuid(), "Я", "РЫБА", "я крутая рыба, дам всем пизды если высунетесь на меня");
        List<Post> posts = new List<Post>() {
            new Post(Guid.NewGuid(), "Post 1", "Content 1", user, Topic.Technology),
            new Post(Guid.NewGuid(), "Post 2", "Content 2", user, Topic.Science)
        };

        postServiceMock.Setup(service => service.GetAllPosts()).Returns(posts);

        var result = postsController.GetPosts();

        var actionResult = Assert.IsType<ActionResult<List<PostsResponse>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returnValue = Assert.IsAssignableFrom<List<PostsResponse>>(okResult.Value);
        
        Assert.Equal(posts.Count, returnValue.Count);
        Assert.Equal(posts[0].Title, returnValue[0].title);
        Assert.Equal(posts[1].Content, returnValue[1].content);
    }
    
    
    
    
}