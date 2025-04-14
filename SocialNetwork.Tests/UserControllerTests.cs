using Microsoft.AspNetCore.Mvc;
using Moq;
using SocialNetwork.API.Contracts;
using SocialNetwork.API.Controllers;
using SocialNetwork.Core.Models;
using Xunit;
using SocialNetwork.Core.Abstractions;

namespace SocialNetwork.API.Tests;
public class UserControllerTests
{
    private readonly Mock<IUserService> mockUserService;
    private readonly UserController userController;
    private readonly ILogger<UserControllerTests> logger;

    public UserControllerTests()
    {
        mockUserService = new Mock<IUserService>();
        userController = new UserController(mockUserService.Object);
        logger = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddConsole();
        }).CreateLogger<UserControllerTests>();
    }
    
    [Fact]
    public void GetUsers_ReturnsListOfUsers()
    {
        List<User> users = new List<User>
        {
            User.Create(Guid.NewGuid(), "John", "Doe", "Bio1").user,
            User.Create(Guid.NewGuid(), "Jane", "Doe", "Bio2").user
        };

        mockUserService.Setup(service => service.GetAll()).Returns(users);

        var result = userController.GetUsers();

        var actionResult = Assert.IsType<ActionResult<List<UserResponse>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result); 
        var returnUsers = Assert.IsType<List<UserResponse>>(okResult.Value);
        logger.LogDebug("users object: {}", returnUsers);
        Assert.Equal(2, returnUsers.Count);
    }

    [Fact]
    public void CreateUser_ReturnsGuid()
    {
        CreateUserDataRequest userRequest = new CreateUserDataRequest("John", "Doe", "Bio1");

        var newUserId = Guid.NewGuid();
        mockUserService.Setup(service => service.Create(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns((newUserId, ""));

        var result = userController.CreateUser(userRequest);

        var actionResult = Assert.IsType<ActionResult<UserResponse>>(result); 
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

        var userResponse = Assert.IsType<UserResponse>(okResult.Value);
        logger.LogDebug("userResponsePOST: {}", userResponse);
        Assert.Equal(newUserId, userResponse.id); 
        Assert.Equal("John", userResponse.FirstName); 
        Assert.Equal("Doe", userResponse.SecondName); 
        Assert.Equal("Bio1", userResponse.Bio); 
    }
}