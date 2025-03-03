using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Application.Services;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PostsResponse>>> GetUsers() 
    {
        var users = await _userService.GetAll();

        var response = users.Select(u => new UserResponse(u.Id, u.FirstName, u.SecondName, u.Bio));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateUser([FromBody] UserRequest request) 
    {
        var user = new User(Guid.NewGuid(), request.FirstName, request.SecondName, request.Bio);

        var userID = await _userService.Create(user);

        return Ok(userID);
    }
}