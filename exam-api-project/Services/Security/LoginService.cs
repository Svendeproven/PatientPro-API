using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.models.Exceptions;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Interfaces.Security;

namespace exam_api_project.Services.Security;

/// <summary>
///     this class is responsible for the login process
/// </summary>
public class LoginService : ILoginService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    // Constructor injection
    public LoginService(
        ITokenService tokenService
        , IUserRepository userRepository
        , IPasswordService passwordService
        , IDeviceRepository deviceRepository
        , IMapper mapper
    )
    {
        _tokenService = tokenService;
        _userRepository = userRepository;
        _passwordService = passwordService;
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }


    // Login and get JWT token 
    public async Task<TokenModelDto> Login(UserLoginDto userLoginDto)
    {
        // Get user from email
        var user = await _userRepository.GetUserByEmailAsync(userLoginDto.Email);
        if (user == null)
        {
            var error = new HttpError
            {
                Title = "Error trying to login",
                Text = "Check that you have entered the correct email and password",
                Status = 401
            };
            throw error;
        }


        // Check password with some encryption
        if (!_passwordService.VerifyPassword(userLoginDto.Password, user.Password))
        {
            var error = new HttpError
            {
                Title = "Error trying to login",
                Text = "Check that you have entered the correct email and password",
                Status = 401
            };
            throw error;
        }

        // maps the user to a UserReadDto
        var usr = _mapper.Map<UserReadDto>(user);

        // Generate token and store it in a object
        var token = new TokenModelDto(_tokenService.GenerateToken(user), usr);

        // If correct login Set the device token for getting push notifications
        await SetDeviceTokenAsync(new DeviceModel() {Token = userLoginDto.DeviceToken, UserModelId = user.Id});
        return token;
    }

    public async Task SignOut(UserSignOutDto userSignOutDto)
    {
        await _deviceRepository.DeleteDeviceByTokenIdAsync(userSignOutDto.DeviceToken);
    }

    // Set the device token for a user if it does not exist and if it does update the UserModelId
    private async Task SetDeviceTokenAsync(DeviceModel deviceModel)
    {
        // Check if the device token is null
        if (deviceModel.Token == null) return;
        // Check if the device token already exists
        var exists = await _deviceRepository.DeviceTokenExistsAsync(deviceModel.Token);

        // If it does update the UserModelId
        if (exists)
        {
            await _deviceRepository.UpdateDeviceAsync(deviceModel);
        }
        else
        {
            // If it does not exist create a new record
            await _deviceRepository.CreateDeviceAsync(deviceModel);
        }
    }
}