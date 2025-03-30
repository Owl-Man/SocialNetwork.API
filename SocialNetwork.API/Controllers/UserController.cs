using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Core.Abstractions;
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

        var response = users.Select(u => new UserResponse(u.Id, u.FirstName, u.SecondName, u.Bio)).ToList();

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<UserResponse> CreateUser([FromBody] UserRequest request) 
    {
        var userID = userService.Create(request.FirstName, request.SecondName, request.Bio);
        var response = new UserResponse(userID, request.FirstName, request.SecondName, request.Bio);
        
        return Ok(response);
    }
}