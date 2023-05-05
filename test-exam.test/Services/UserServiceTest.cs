using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace test_exam.test.Services;

public class UserServiceTest : IClassFixture<CustomServiceFactory>, IAsyncLifetime
{
    private readonly ExamContext? _dbContext;

    private readonly IUserRepository? _userService;

    public UserServiceTest(CustomServiceFactory startup)
    {
        _userService = startup.ServiceProvider.GetService<IUserRepository>();
        _dbContext = startup.ServiceProvider.GetService<ExamContext>();
    }

    /// <summary>
    ///     Integration test that creates a user and assert if it exists
    /// </summary>
    [Fact]
    private async Task CreateNewUserAsync()
    {
        var user = await _userService.CreateNewUserAsync(new UserModel
        {
            Role = "admin",
            Name = "John Doe",
            Email = "John@svendeprøven.dk",
            Password = "Hallo-Teacher"
        });

        Assert.True(user.Email == "John@svendeprøven.dk");
        Assert.True(user.Role == "admin");
    }

    /// <summary>
    ///     Test for creating a new user
    /// </summary>
    [Fact]
    private async Task UpdateUserAsync()
    {
        //var users = await _userService.GetAllUsersAsync();
        var userId = await _userService!.GetUserByEmailAsync("user1@svendeprøven.dk");
        var newUser = new UserWriteDto(
            "user1", null, "user1@svendeprøven.dk", "admin", "CareTaker", 1
        );

        // Update without a password
        var user = await _userService.UpdateExistingUserAsync(newUser, userId.Id);

        Assert.True(user.Role == "admin");
        Assert.True(user.Email == "user1@svendeprøven.dk");
        Assert.True(user.Name == "user1");

        // Update user with new password
        var newUser1 = new UserWriteDto(
            "user1", null, "user1@svendeprøven.dk", "admin", "CareTaker", 1
        );

        user = await _userService.UpdateExistingUserAsync(newUser1, userId.Id);
        Assert.True(user.Role == "admin");
        Assert.True(user.Email == "user1@svendeprøven.dk");
        Assert.True(user.Name == "user1");
        Assert.True(user.Password != "Hello-World");
    }

    /// <summary>
    ///     Method for testing get user by email
    /// </summary>
    [Fact]
    private async Task GetUserByEmailAsync()
    {
        // Get user by email and assert if it is the correct user 
        var user = await _userService!.GetUserByEmailAsync("user6@svendeprøven.dk");
        Assert.True(user.Role == "user");
        Assert.True(user.Name == "User6");
    }

    /// <summary>
    ///     Get a user by user id
    /// </summary>
    [Fact]
    private async Task GetUserByIdAsync()
    {
        // Get user by id 3 and assert if it is the correct user
        var user = await _userService!.GetUserByIdAsync(3);
        Assert.True(user.Email == "user3@svendeprøven.dk");
        Assert.True(user.Role == "admin");
        Assert.True(user.Name == "User3");
    }

    /// <summary>
    ///     Deletes a user from the database
    /// </summary>
    [Fact]
    private async Task DeleteUserByIdAsync()
    {
        // Delete user by id 3 and assert if it is the correct user
        var user = await _userService!.DeleteUserByIdAsync(3);
        Assert.True(user.Email == "user3@svendeprøven.dk");
        Assert.True(user.Role == "admin");
        Assert.True(user.Name == "User3");
        Assert.True(_dbContext!.Users.Count() == 9);
    }

    /// <summary>
    ///     Creates the first user in the database
    /// </summary>
    [Fact]
    private async Task CreateTheFirstUserAsync()
    {
        // Calls DisposeAsync to delete all users in the database 
        await DisposeAsync();
        // Create the first user in the database
        
        var user = await _userService!.CreateTheFirstUserAsync(new UserModel
        {
            Role = "admin",
            Name = "John Doe",
            Email = "John@svendeprøven.dk",
            Password = "Hallo-Teacher"
        });
        // Assert if the user is the correct user  
        Assert.True(user.Email == "John@svendeprøven.dk");
        Assert.True(user.Role == "admin");


        // Assert that the user is not created because the user already exists 
        //await Assert.ThrowsAsync<Exception>(() => _userService.CreateTheFirstUserAsync(user));
    }

    /// <summary>
    ///     Checks if there is a user in the database
    /// </summary>
    [Fact]
    private async Task GetAllUsersAsync()
    {
        // Get all users from the database
        var users = await _userService!.GetAllUsersAsync();
        // Assert if the user is the correct user  
        Assert.True(users.Count() == 10);
    }
    
    
    /// <summary>
    ///     InitializeAsync this creates 10 new users for testing purposes
    /// </summary>
    public async Task InitializeAsync()
    {
        // Create a list of users 
        var users = new List<UserModel>();

        // Create 10 new users 
        for (var i = 1; i <= 10; i++)
            users.Add(new UserModel
            {
                Role = i < 4 ? "admin" : "user",
                Name = $"User{i}",
                Email = $"user{i}@svendeprøven.dk",
                Password = $"Password{i}2345"
            });
        // Create the users in the database 
        foreach (var user in users) await _userService.CreateNewUserAsync(user);

        Assert.True(_dbContext.Users.Count() == 10);
    }

    /// <summary>
    ///     Cleans up the database after each integration test
    /// </summary>
    /// <returns></returns>
    public async Task DisposeAsync()
    {
        // Retrieve all users from the database
        var allUsers = _dbContext.Users.ToList();

        // Remove all users from the database
        _dbContext.Users.RemoveRange(allUsers);

        // Save changes to the database
        await _dbContext.SaveChangesAsync();

        // Reset the primary key sequence (assumes the table name is "Users" and the primary key column is "Id")
        var tableName = "Users";
        var primaryKeyColumnName = "Id";
        var sequenceName = $"{tableName}_{primaryKeyColumnName}_seq";

        // Create the raw SQL command to reset the primary key sequence
        var resetSequenceSqlCommand = $"ALTER SEQUENCE \"{sequenceName}\" RESTART WITH 1;";

        // Execute the raw SQL command
        await _dbContext.Database.ExecuteSqlRawAsync(resetSequenceSqlCommand);
    }
}