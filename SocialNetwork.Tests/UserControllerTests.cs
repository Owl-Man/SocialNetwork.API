using Microsoft.AspNetCore.Mvc;
using Moq;
using SocialNetwork.API.Contracts;
using SocialNetwork.Application.Services;
using SocialNetwork.API.Controllers;
using SocialNetwork.Core.Models;
using Xunit;

namespace SocialNetwork.API.Tests;
public class UserControllerTests
{
    private readonly Mock<IUserService> mockUserService;
    private readonly UserController userController;

    public UserControllerTests()
    {
        mockUserService = new Mock<IUserService>();
        userController = new UserController(mockUserService.Object);
    }
    
    [Fact]
    public void GetUsers_ReturnsListOfUsers()
    {
        List<User> users = new List<User>
        {
            new User(Guid.NewGuid(), "John", "Doe", "Bio1"),
            new User(Guid.NewGuid(), "Jane", "Doe", "Bio2")
        };

        mockUserService.Setup(service => service.GetAll()).Returns(users);

        var result = userController.GetUsers();

        var actionResult = Assert.IsType<ActionResult<List<UserResponse>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); 
        var returnUsers = Assert.IsType<List<UserResponse>>(okResult.Value);
        Assert.Equal(2, returnUsers.Count);
    }

    [Fact]
    public void CreateUser_ReturnsGuid()
    {
        UserRequest userRequest = new UserRequest("John", "Doe", "Bio1");

        var newUserId = Guid.NewGuid();
        mockUserService.Setup(service => service.Create(It.IsAny<User>())).Returns(newUserId);

        var result = userController.CreateUser(userRequest);

        var actionResult = Assert.IsType<ActionResult<Guid>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); 
        Assert.Equal(newUserId, okResult.Value); 
    }
}