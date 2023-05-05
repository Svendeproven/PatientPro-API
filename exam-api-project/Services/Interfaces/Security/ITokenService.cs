using exam_api_project.models.Entities;

namespace exam_api_project.Services.Interfaces.Security;

public interface ITokenService
{
    public string GenerateToken(UserModel user);
}