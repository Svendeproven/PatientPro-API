using exam_api_project.models.Dtos;

namespace exam_api_project.Services.Interfaces;

/// <summary>
/// CRUD operations for User table
/// </summary>
public interface IUserService
{
    Task<UserReadDto> CreateNewUserAsync(UserWriteDto user);
    Task<UserReadDto> UpdateExistingUserAsync(UserWriteDto userUpdate, int id);
    Task<UserReadDto> CreateTheFirstUserAsync(UserWriteDto user);
    Task<UserReadDto> GetUserByEmailAsync(string email);
    Task<UserReadDto> DeleteUserByIdAsync(int id);
    Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
    Task<UserReadDto> GetUserByIdAsync(int id);
}