using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Interfaces.Security;

namespace exam_api_project.Services.Security;

/// <summary>
///     Class for handling The encryption of passwords
/// </summary>
public class PasswordService : IPasswordService
{
    // Hash password
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Verify password is correct
    public bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}