using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;

namespace exam_api_project.Repositories.Interfaces;

public interface IUserRepository
{
    Task<UserModel> CreateNewUserAsync(UserModel user);
    Task<UserModel> UpdateExistingUserAsync(UserWriteDto userUpdate, int id);
    Task<UserModel> CreateTheFirstUserAsync(UserModel user);
    Task<UserModel> GetUserByEmailAsync(string email);
    Task<UserModel> DeleteUserByIdAsync(int id);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(int id);
}