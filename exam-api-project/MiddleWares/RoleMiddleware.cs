using System.Security.Claims;
using exam_api_project.Repositories;
using exam_api_project.Repositories.Context;
using exam_api_project.Services.Security;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.MiddleWares;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;

    public RoleMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ExamContext examContext)
    {
        try
        {
            // Fetch the UserId from the token claims
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // Fetch the user from the database
            var user = await examContext.Users.FirstAsync(user => user.Id == int.Parse(userId));
            // Add the user to the context
            context.Items.Add("user", user);
            // Add the role service to the context
            context.Items.Add("roleService", new RoleService(user));
            // Clear the change tracker
            examContext.ChangeTracker.Clear();
            // Continue the request
            await _next(context);
        }
        catch (Exception e)
        {
            // If the user is not authenticated, continue the request
            await _next(context);
        }
    }
}