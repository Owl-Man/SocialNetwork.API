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

        var response = users.Select(u => new UserResponse(u.Id, u.FirstName, u.SecondName, u.Bio, u.PreferredTopics)).ToList();

        return Ok(response);
    }

    [HttpGet("GetUserById")]
    public ActionResult<UserResponse> GetUser([FromBody] OnlyId idRequest)
    {
        User? user = userService.GetById(idRequest.Id);

        if (user is null)
        {
            return NotFound($"Не найден пользовател с ID: {idRequest.Id}");
        }

        var response = new UserResponse(user.Id, user.FirstName, user.SecondName, user.Bio, user.PreferredTopics);
        return Ok(response);
    }

    [HttpPost("CreateUser")]
    public ActionResult<UserResponse> CreateUser([FromBody] CreateUserDataRequest request) 
    {
        (Guid userID, string error) = userService.Create(request.FirstName, request.SecondName, request.Bio);

        User? user = userService.GetById(userID);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        if (user is null) return NotFound("Ошибка создании пользователя " + userID);

        var response = new UserResponse(userID, user.FirstName, user.SecondName, user.Bio, user.PreferredTopics);
        
        return Ok(response);
    }

    [HttpPut("UpdateUserInfo")]
    public ActionResult<UserResponse> UpdateUser([FromBody] ChangeUserDataRequest request)
    {
        (Guid userId, string error) = userService.Update(request.Id, request.FirstName, request.SecondName, request.Bio, request.PreferredTopics);

        if (!string.IsNullOrEmpty(error))
        {
            return BadRequest(error);
        }

        var response = new UserResponse(userId, request.FirstName, request.SecondName, request.Bio, request.PreferredTopics);

        return Ok(response);
    }

    [HttpPut("UpdateUserPreferredTopics")]
    public ActionResult<OnlyId> UpdateUserPreferredTopics([FromBody] UpdateUserPreferredTopicsRequest request)
    {
        Guid? userId = userService.UpdatePreferredTopics(request.Id, request.Topics);

        if (userId == null)
        {
            return NotFound($"Не найден пользовател с ID: {request.Id}");
        }

        return Ok(new OnlyId(userId.Value));
    }

    [HttpDelete("DeleteUserById")]
    public ActionResult<OnlyId> DeleteUser([FromBody] OnlyId idRequest) 
    {
        Guid? userId = userService.Delete(idRequest.Id);

        if (userId == null)
        {
            return NotFound($"Не найден пользовател с ID: {idRequest.Id}");
        }

        return Ok(new OnlyId(idRequest.Id));
    }
}