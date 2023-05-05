using exam_api_project.models.Dtos;
using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers;

/// <summary>
///     This controller is for handling user related logic
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    // User service for dependency injection
    private readonly IUserService _userService;

    /// <summary>
    ///     Initializes a new instance of the UserController class.
    /// </summary>
    /// <param name="userService">A user service instance.</param>
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    ///     Retrieves all users from the database.
    /// </summary>
    /// <returns>An ActionResult containing a collection of UserReadDto objects.</returns>
    [HttpGet]
    [Admin]
    public async Task<ActionResult<IEnumerable<UserReadDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in GetAllUsersAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    ///     Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>An ActionResult containing a UserReadDto object.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserReadDto>> GetUserByIdAsync(int id)
    {
        try
        {
            // Get RoleService from httpContext
            HttpContext.Items.TryGetValue("roleService", out var roleService);
            // Cast to RoleService
            var role = (RoleService)roleService;
            if (!role.IsAdmin() && !role.isSelf(id)) return Forbid();
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound($"User with ID {id} not found.");
            return Ok(user);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in GetUserByIdAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    ///     Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>An ActionResult containing a UserReadDto object.</returns>
    [HttpGet("current")]
    public async Task<ActionResult<UserReadDto>> GetCurrentUserAsync()
    {
        try
        {
            HttpContext.Items.TryGetValue("user", out var user);
            return Ok(user);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetCurrentUserAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }


    /// <summary>
    ///     Creates a new user in the database.
    /// </summary>
    /// <param name="user">A UserWriteDto object containing the user information.</param>
    /// <returns>An ActionResult containing the created UserReadDto object.</returns>
    [HttpPost]
    [Admin]
    public async Task<ActionResult<UserReadDto>> CreateNewUserAsync([FromBody] UserWriteDto user)
    {
        try
        {
            var newUser = await _userService.CreateNewUserAsync(user);
            // return user created 201 status code
            return CreatedAtAction("GetUserById", new { id = newUser.Id }, newUser);
        }
        catch (InvalidOperationException e)
        {
            Log.Warning("Error in CreateNewUserAsync: {@Message}", e.Message);
            return Conflict(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in CreateNewUserAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred while creating user.");
        }
    }

    /// <summary>
    ///     Updates a user by their ID in the database.
    /// </summary>
    /// <param name="userUpdate">A UserWriteDto object containing updated user information.</param>
    /// <param name="id">The ID of the user to update.</param>
    /// <returns>An ActionResult containing the updated UserReadDto object.</returns>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<UserReadDto>> UpdateExistingUserByIdAsync([FromBody] UserWriteDto userUpdate,
        int id)
    {
        try
        {
            // Get RoleService from httpContext
            HttpContext.Items.TryGetValue("roleService", out var roleService);
            // Cast to RoleService
            var role = (RoleService)roleService;
            if (!role.IsAdmin() && !role.isSelf(id)) return Forbid();
            var newUser = await _userService.UpdateExistingUserAsync(userUpdate, id);
            if (newUser == null) return NotFound($"User with ID {id} not found.");
            return Ok(newUser);
        }
        catch (InvalidOperationException e)
        {
            Log.Error("Error in UpdateExistingUserByIdAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in UpdateExistingUserByIdAsync: {@Message} {@StackTrace}", e.Message,
                e.StackTrace);
            return StatusCode(500, "An unexpected error occurred.");
        }
    }

    /// <summary>
    ///     Deletes a user by their ID in the database.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <returns>An ActionResult containing the deleted UserReadDto object.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<UserReadDto>> DeleteUserByIdAsync(int id)
    {
        try
        {
            // Get RoleService from httpContext
            HttpContext.Items.TryGetValue("roleService", out var roleService);
            // Cast to RoleService
            var role = (RoleService)roleService;
            if (!role.IsAdmin() && !role.isSelf(id)) return Forbid();
            var user = await _userService.DeleteUserByIdAsync(id);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }
        catch (InvalidOperationException e)
        {
            Log.Error("Error in DeleteUserByIdAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Log.Error("Unexpected error in DeleteUserByIdAsync: {@Message} {@StackTrace}", e.Message, e.StackTrace);
            return StatusCode(500, "An unexpected error occurred while deleting user.");
        }
    }
}