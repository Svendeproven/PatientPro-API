using exam_api_project.models.Dtos;

namespace exam_api_project.Services.Interfaces.Security;

public interface ILoginService
{
    public Task<TokenModelDto> Login(UserLoginDto userLoginDto);
    Task SignOut(UserSignOutDto userLoginDto);
}