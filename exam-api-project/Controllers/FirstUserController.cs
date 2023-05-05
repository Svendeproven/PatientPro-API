using exam_api_project.models;
using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace exam_api_project.Controllers;

/// <summary>
///     Represents a controller for managing the first user creation in the API.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class FirstUserController : ControllerBase
{
    private readonly string _firstUserToken;
    private readonly IUserService _userService;

    /// <summary>
    ///     Initializes a new instance of the FirstUserController class.
    /// </summary>
    /// <param name="userService">An implementation of the IUserService interface.</param>
    /// <param name="firstUserTokenOptions">Options containing the first user environment token.</param>
    public FirstUserController(IUserService userService,
        IOptions<FirstUserEnvironmentTokenOptions> firstUserTokenOptions)
    {
        _userService = userService;
        _firstUserToken = firstUserTokenOptions.Value.Token;
    }

    /// <summary>
    ///     Method for creating the first user in the database (admin) if no users exist
    /// </summary>
    /// <param name="user"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost("{Token}")]
    public async Task<ActionResult<UserReadDto>> CreateTheFirstUser([FromBody] UserWriteDto user, string token)
    {
        // Compares the token from the request with the token from the environment variable
        if (token != _firstUserToken)
            
            return Unauthorized("Invalid token" + _firstUserToken);


        try
        {
            var isUserExists = (List<UserReadDto>) await _userService.GetAllUsersAsync();
            // Return 400 Bad Request if user already exists
            if (isUserExists.Count >= 1) return BadRequest("User already exists");

            var newUser = await _userService.CreateTheFirstUserAsync(user);
            // Return 201 CreatedAtAction with the new user
            return CreatedAtAction(nameof(CreateTheFirstUser), new { newUser.Id }, newUser);
        }
        catch (ArgumentNullException e)
        {
            // Return invalid argument exception
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            Log.Error(e.StackTrace);
            // Return 500 Internal Server Error with the exception message
            return StatusCode(500, e.Message);
        }
    }
}