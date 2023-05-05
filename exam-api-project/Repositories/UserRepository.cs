using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Interfaces.Security;
using Microsoft.EntityFrameworkCore;

namespace exam_api_project.Repositories;

/// <summary>
///     Class for handling user related logic
/// </summary>
public class UserRepository : IUserRepository
{
    // Database context for dependency injection
    private readonly ExamContext _dbContext;
    private readonly IPasswordService _passwordService;

    /// <summary>
    ///     Initializes a new instance of the UserRepository class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    /// <param name="passwordService">The password service for hashing passwords.</param>
    public UserRepository(ExamContext dbContext, IPasswordService passwordService)
    {
        _dbContext = dbContext;
        _passwordService = passwordService;
    }


    /// <summary>
    ///     Creates a new user in the database asynchronously.
    /// </summary>
    /// <param name="user">The user to be created.</param>
    /// <returns>The created user.</returns>
    public async Task<UserModel> CreateNewUserAsync(UserModel user)
    {
        try
        {
            // Check if a user with the same email already exists
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
                throw new InvalidOperationException($"User with email {user.Email} already exists.");

            user.Password = _passwordService.HashPassword(user.Password);
            var newUser = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return newUser.Entity;
        }
        catch (Exception e)
        {
            // Throw an exception if the user could not be added to the database
            throw new InvalidOperationException("Error creating user.", e);
        }
    }

    /// <summary>
    ///     Updates an existing user's information asynchronously, except for the password.
    /// </summary>
    /// <param name="user">The updated user data.</param>
    /// <param name="id">The user's database identifier.</param>
    /// <returns>The updated user.</returns>
    public async Task<UserModel> UpdateExistingUserAsync(UserWriteDto user, int id)
    {
        try
        {
            var password = "";
            // encrypt new password if not null
            if (user.Password != null)
                password = _passwordService.HashPassword(user.Password);

            // Gets the user in the data base by id
            var oldUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            // Returns null if no user is found
            if (oldUser == null) return null;
            // Else update the user
            oldUser.Name = user.Name ?? oldUser.Name; // Sets new name if name not null
            oldUser.Email = user.Email ?? oldUser.Email; // Sets new email if email not null
            oldUser.Role = user.Role ?? oldUser.Role; // Sets user role if user role not null
            oldUser.Password = password ?? oldUser.Password; // Sets a new password if not null
            oldUser.JobTitle = user.JobTitle ?? oldUser.JobTitle; // Sets a new job title if not null

            await _dbContext.SaveChangesAsync();
            return oldUser;
        }
        catch (Exception e)
        {
            // Throw an exception if the user could not be updated in the database
            throw new InvalidOperationException("Error updating user.", e);
        }
    }


    /// <summary>
    ///     Creates the first user in the database asynchronously if none exist.
    /// </summary>
    /// <param name="user">The first user to be created.</param>
    /// <returns>The created user.</returns>
    public async Task<UserModel> CreateTheFirstUserAsync(UserModel user)
    {
        var count = _dbContext.Users.Count();
        if (count == 0)
        {
            user.Role = "admin";
            user.Password = _passwordService.HashPassword(user.Password);
            var newUser = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return newUser.Entity;
        }

        throw new InvalidOperationException("Cannot create the first user. There is already a user in the database.");
    }

    /// <summary>
    ///     Retrieves a user from the database asynchronously based on their email address.
    /// </summary>
    /// <param name="email">The email address of the user to be retrieved.</param>
    /// <returns>The user with the specified email address.</returns>
    public async Task<UserModel> GetUserByEmailAsync(string email)
    {
        try
        {
            // Gets the user in the data base by email with no case sensitivity
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            return user;
        }
        catch (Exception e)
        {
            // Throw an exception if the user could not be found in the database
            throw new InvalidOperationException("Error getting user by email.", e);
        }
    }

    /// <summary>
    ///     Deletes a user from the database asynchronously based on their identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to be deleted.</param>
    /// <returns>The deleted user.</returns>
    public async Task<UserModel> DeleteUserByIdAsync(int id)
    {
        try
        {
            // Gets the user in the data base by id
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            // Returns null if no user is found
            if (user == null) return null;
            // Else remove the user
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        catch (Exception e)
        {
            // Throw an exception if the user could not be removed from the database
            throw new InvalidOperationException("Error deleting user.", e);
        }
    }

    /// <summary>
    ///     Retrieves a user from the database asynchronously based on their identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to be retrieved.</param>
    /// <returns>The user with the specified identifier.</returns>
    public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
    {
        try
        {
            var users = await _dbContext.Users.ToListAsync();
            return users;
        }
        catch (Exception e)
        {
            // Throw an exception if the users could not be found in the database
            throw new InvalidOperationException("Error getting all users.", e);
        }
    }

    /// <summary>
    ///     Retrieves a user from the database asynchronously based on their identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to be retrieved.</param>
    /// <returns>The user with the specified identifier.</returns>
    public async Task<UserModel> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        catch (Exception e)
        {
            // Throw an exception if the user could not be found in the database
            throw new InvalidOperationException("Error getting user by id.", e);
        }
    }
}