using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Application.Services;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public ActionResult<List<UserResponse>> GetUsers() 
    {
        var users = userService.GetAll();

        var response = users.Select(u => new UserResponse(u.Id, u.FirstName, u.SecondName, u.Bio));

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<Guid> CreateUser([FromBody] UserRequest request) 
    {
        var user = new User(Guid.NewGuid(), request.FirstName, request.SecondName, request.Bio);

        var userID = userService.Create(user);

        return Ok(userID);
    }
}