using exam_api_project.models.Dtos;
using exam_api_project.models.Exceptions;
using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Interfaces.Security;
using exam_api_project.Services.Security;
using Microsoft.AspNetCore.Mvc;

namespace exam_api_project.Controllers
{

    /// <summary>
    ///     Controller for handling login and returning a token
    /// </summary>
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        ///     Method for signin in
        /// </summary>
        /// <param name="userLoginDto">A UserLogin which contains the provided email and password</param>
        /// <returns>A result with a status code</returns>
        // POST: api/Login
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenModelDto>> Login([FromBody] UserLoginDto userLoginDto)
        {
            try
            {
                var token = await _loginService.Login(userLoginDto);
                return Ok(token);
            }
            catch (HttpError e)
            {
                return StatusCode(e.Status, new HttpError(){Title = e.Title,Text = e.Text,Status = e.Status});
            }
        }

        /// <summary>
        ///     Method for signin out
        /// </summary>
        /// <param name="userSignOutDto"></param>
        /// <returns>A result with a status code</returns>
        // POST: api/Login
        [HttpPost]
        [Route("signOut")]
        public async Task<ActionResult> SignOut([FromBody] UserSignOutDto userSignOutDto)
        {
            try
            {
                
                HttpContext.Items.TryGetValue("roleService", out var roleService);
                var role = (RoleService) roleService;
                // Check if user is self
                if (!role!.isSelf(userSignOutDto.UserId))
                {
                    return Forbid();
                }
                // Else sign out
                await _loginService.SignOut(userSignOutDto);
                // Return no content
                return NoContent();
            }
            catch(ArgumentException e)
            {
                return BadRequest(new HttpError(){Title = "Error trying to sign out",Text = e.Message,Status =400});
                
            }
        }


    }
}