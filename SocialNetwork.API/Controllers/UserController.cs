using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.API.Contracts;
using SocialNetwork.Core.Abstractions;
using SocialNetwork.Core.Models;

namespace SocialNetwork.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("GetAllUsers")]
    public ActionResult<List<UserResponse>> GetUsers() 
    {
        List<User> users = userService.GetAll();

        var response = users.Select(u => new UserResponse(u.Id, u.FirstName, u.SecondName, u.Bio)).ToList();

        return Ok(response);
    }

    [HttpGet("GetUserById")]
    public ActionResult<UserResponse> GetUser([FromBody] OnlyId idRequest)
    {
        User? user = userService.GetById(idRequest.Id);

        if (user is null)
        {
            return NotFound();
        }

        var response = new UserResponse(user.Id, user.FirstName, user.SecondName, user.Bio);
        return Ok(response);
    }

    [HttpPost("CreateUser")]
    public ActionResult<UserResponse> CreateUser([FromBody] CreateUserDataRequest request) 
    {
        Guid userID = userService.Create(request.FirstName, request.SecondName, request.Bio);

        var response = new UserResponse(userID, request.FirstName, request.SecondName, request.Bio);
        
        return Ok(response);
    }

    [HttpPut("UpdateUserInfo")]
    public ActionResult<UserResponse> UpdateUser([FromBody] ChangeUserDataRequest request)
    {
        Guid userId = userService.Update(request.Id, request.FirstName, request.SecondName, request.Bio);

        var response = new UserResponse(userId, request.FirstName, request.SecondName, request.Bio);

        return Ok(response);
    }

    [HttpDelete("DeleteUserById")]
    public ActionResult<UserResponse> DeleteUser([FromBody] OnlyId idRequest) 
    {
        Guid userId = userService.Delete(idRequest.Id);

        return Ok(new OnlyId(userId));
    }
}