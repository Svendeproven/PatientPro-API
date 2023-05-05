using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;

namespace exam_api_project.Services;

/// <summary>
///     Provides user-related services and interacts with the UserRepository.
/// </summary>
public class UserService : IUserService
{
    
    private readonly IUserRepository _userRepository;
    private readonly IDepartmentService _departmentService;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the UserService class.
    /// </summary>
    /// <param name="userRepository">The UserRepository to interact with the database.</param>
    /// <param name="departmentService"></param>
    /// <param name="mapper">The AutoMapper for mapping between data transfer objects and entity models.</param>
    public UserService(IUserRepository userRepository,IDepartmentService departmentService, IMapper mapper)
    {
        _userRepository = userRepository;
        _departmentService = departmentService;
        _mapper = mapper;
    }

    /// <summary>
    ///     Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user data to be created.</param>
    /// <returns>The created user's data.</returns>
    public async Task<UserReadDto> CreateNewUserAsync(UserWriteDto user)
    {
        var result = await _userRepository.CreateNewUserAsync(_mapper.Map<UserModel>(user));
        return _mapper.Map<UserReadDto>(result);
    }

    /// <summary>
    /// Updates an existing user's information asynchronously.
    /// </summary>
    /// <param name="userUpdate">The updated user data.</param>
    /// <param name="id">The user's database identifier.</param>
    /// <returns>The updated user's data.</returns>
    public async Task<UserReadDto> UpdateExistingUserAsync(UserWriteDto userUpdate, int id)
    {
        var result = await _userRepository.UpdateExistingUserAsync(userUpdate, id);
        return _mapper.Map<UserReadDto>(result);
    }

    /// <summary>
    /// Creates the first user in the database asynchronously if none exist.
    /// </summary>
    /// <param name="user">The first user to be created.</param>
    /// <returns>The created user's data.</returns>
    public async Task<UserReadDto> CreateTheFirstUserAsync(UserWriteDto user)
    {
        // gets all the departments and converts it to a list
        var departments = (await _departmentService.GetAllDepartmentsAsync()).ToList();
        int departmentId = 0;
       
        if (departments.Count == 0)
        {
            // save a new department
            var department = new DepartmentWriteDto(Title: "Administration");
            departmentId = _departmentService.CreateDepartmentAsync(department).Id;
        }
        else
        {
            // if a department exists gets the id of the firs department
            departmentId = departments[0].Id;
        }
        // creates a new user with the departmentId
        user = new UserWriteDto(Name: user.Name
            , Password: user.Password
            , Email: user.Email
            , Role: user.Role
            , JobTitle: user.JobTitle
            , DepartmentModelId: 1
            );
        // saves the first user with a apartment
        var result = await _userRepository.CreateTheFirstUserAsync(_mapper.Map<UserModel>(user));
        return _mapper.Map<UserReadDto>(result);
    }

    /// <summary>
    /// Retrieves a user's data from the database asynchronously based on their email address.
    /// </summary>
    /// <param name="email">The email address of the user to be retrieved.</param>
    /// <returns>The user data with the specified email address.</returns>
    public async Task<UserReadDto> GetUserByEmailAsync(string email)
    {
        var result = await _userRepository.GetUserByEmailAsync(email);
        return _mapper.Map<UserReadDto>(result);
    }

    /// <summary>
    /// Deletes a user from the database asynchronously based on their identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to be deleted.</param>
    /// <returns>The deleted user's data.</returns>
    public async Task<UserReadDto> DeleteUserByIdAsync(int id)
    {
        var result = await _userRepository.DeleteUserByIdAsync(id);
        return _mapper.Map<UserReadDto>(result);
    }

    /// <summary>
    /// Retrieves a user's data from the database asynchronously based on their identifier.
    /// </summary>
    /// <returns>The user data with the specified identifier.</returns>
    public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
    {
        var result = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<List<UserReadDto>>(result);
    }

    /// <summary>
    /// Retrieves a user's data from the database asynchronously based on their identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to be retrieved.</param>
    /// <returns>The user data with the specified identifier.</returns>
    public async Task<UserReadDto> GetUserByIdAsync(int id)
    {
        var result = await _userRepository.GetUserByIdAsync(id);
        return _mapper.Map<UserReadDto>(result);
    }
}