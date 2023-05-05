namespace exam_api_project.Services.Interfaces.Security;

/// <summary>
///     Interface for forcing hash and verify password on implementer
/// </summary>
public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}