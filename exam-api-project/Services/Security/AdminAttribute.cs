using exam_api_project.models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace exam_api_project.Services.Security;

/// <summary>
///     Custom attribute to check if the user is an admin
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class AdminAttribute : Attribute, IActionFilter
{
    private readonly HttpError _httpError = new()
    {
        Title = "Access denied", Text = "You cant access this resource", Status = 403
    };

    /// <summary>
    /// This method runs before the controller action is executed
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        try
        {
            // Get the role service from the context
            context.HttpContext.Items.TryGetValue("roleService", out var roleService);
            var role = (RoleService)roleService;
            // If the user is not an admin, return a 403
            if (role != null && !role.IsAdmin())
            {
                var result = new ObjectResult(_httpError)
                {
                    StatusCode = 403
                };
                context.Result = result;
            }
        }
        catch (Exception e)
        {
            Log.Warning("Could not find a user in the middleware, {Error}", e.Message);
            context.Result = new UnauthorizedObjectResult(new HttpError
                {
                    Title = "Could not find the user", Text = "User does not exists in the middleware", Status = 401
                })
                ;
        }
    }

    /// <summary>
    /// This method runs after the controller action is executed
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No action here is required in this case
    }
}