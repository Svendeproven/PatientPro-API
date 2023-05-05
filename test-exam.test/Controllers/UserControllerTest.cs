using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using exam_api_project.models.Dtos;
using exam_api_project.models.Entities;
using exam_api_project.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace test_exam.test.Controllers;

public class UserControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient _client = null!;
    private ExamContext _dbContext = null!;

    public UserControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    /// <summary>
    ///     Initialize is run before each test
    /// </summary>
    public async Task InitializeAsync()
    {
        await _factory.StartDatabaseContainerAsync();
        _client = _factory.CreateClient();
        var createTest = new CreateTestData(_client);
        await createTest.CreateUsersTestDataAsync();

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<ExamContext>();
    }

    /// <summary>
    ///     Cleans upp the database after each test run and resets the primary key sequence
    ///     And prepares the database for the next test run
    /// </summary>
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


    /// <summary>
    ///     Test if the GetUserByIdAsync method returns NotFound for nonexistent user
    /// </summary>
    [Fact]
    public async Task GetUserByIdAsyncReturnsNotFoundForNonexistentUser()
    {
        // Start a new stopwatch
        var stopwatch = Stopwatch.StartNew();

        // Get a user that does not exist in the database and check if the response is NotFound
        var responseNotFound = await _client.GetAsync("/api/User/99999");

        // Stop the stopwatch
        stopwatch.Stop();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "The request took too long.");
    }

    /// <summary>
    ///     Test if the GetUserByIdAsync method returns BadRequest for invalid id
    /// </summary>
    [Fact]
    public async Task GetUserByIdAsyncReturnsBadRequestForInvalidId()
    {
        // Start a new stopwatch
        var stopwatch = Stopwatch.StartNew();

        // Get a user with a wrong input id and check if the response is BadRequest
        var responseNegative = await _client.GetAsync("/api/User/t");

        // Stop the stopwatch
        stopwatch.Stop();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, responseNegative.StatusCode);
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "The request took too long.");
    }

    /// <summary>
    ///     TEST if the GetUserByIdAsync method returns a user for existing user
    /// </summary>
    [Fact]
    public async Task GetUserByIdAsyncReturnsUserForExistingUser()
    {
        // Start a new stopwatch
        var stopwatch = Stopwatch.StartNew();

        // Get the first user from the database
        var response = await _client.GetAsync("/api/User/1");

        // Stop the stopwatch
        stopwatch.Stop();

        // Assert that the request was successful
        response.EnsureSuccessStatusCode();

        // Assert that the response is in JSON format
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());

        // Get the JSON response
        var jsonString = await response.Content.ReadAsStringAsync();

        // Set the options for the deserialization to ignore the case of the properties
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var user = JsonSerializer.Deserialize<UserModel>(jsonString, options);


        // Assert that the elapsed time is less than 1000 ms
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "The request took too long.");
        // Assert that the response is a user with id 1
        Assert.True(user!.Id == 1);
    }


    /// <summary>
    ///     Test the DeleteUserByIdAsync methods
    /// </summary>
    [Fact]
    public async Task DeleteUserById()
    {
        // Arrange
        await LoginAndSetupClientAsync();

        // Act and Assert
        await DeleteNonExistentUserAsync();
        await DeleteUserWithInvalidIdAsync();
        await DeleteExistingUserAsync();
    }


    /// <summary>
    ///     delete an existing user
    /// </summary>
    private async Task DeleteNonExistentUserAsync()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();

        var requestNotFound = new HttpRequestMessage(HttpMethod.Delete, "/api/user/99999");
        var responseNotFound = await _client.SendAsync(requestNotFound);

        stopwatch.Stop();
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Deleting non-existent user took too long.");
        Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
    }

    /// <summary>
    ///     Delete a user with an invalid id
    /// </summary>
    private async Task DeleteUserWithInvalidIdAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        var requestBadRequest = new HttpRequestMessage(HttpMethod.Delete, "/api/user/t");
        var responseBadRequest = await _client.SendAsync(requestBadRequest);

        stopwatch.Stop();
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Deleting user with invalid id took too long.");
        Assert.Equal(HttpStatusCode.BadRequest, responseBadRequest.StatusCode);
    }

    /// <summary>
    ///     Delete an existing user
    /// </summary>
    private async Task DeleteExistingUserAsync()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();
        // Act
        // Delete the user with id 1
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/user/1");
        var response = await _client.SendAsync(request);

        stopwatch.Stop();
        // Assert that the response time is less than 1000 ms
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Deleting existing user took too long.");
        response.EnsureSuccessStatusCode();
        // Assert that the response is not Unauthorized
        Assert.False(response.StatusCode == HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateAnNewUserAsync()
    {
        // Arrange
        var user = new UserWriteDto("Test", "Test", "Test@test.dk", "Test", "Test", 1);

        // serialize the user to json
        var content = new StringContent(
            JsonSerializer.Serialize(user),
            Encoding.UTF8,
            "application/json"
        );

        // Login and setup the client with the token and a authorization header
        await LoginAndSetupClientAsync();

        var stopwatch = Stopwatch.StartNew();

        // Act
        var response = await _client.PostAsync("/api/user", content);
        stopwatch.Stop();

        // Assert that the response time is less than 1000 ms
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "The request took too long.");
        // Assert that the request was successful
        response.EnsureSuccessStatusCode();
        // Assert that the response is not Unauthorized
        Assert.False(response.StatusCode == HttpStatusCode.Unauthorized);
        // Assert that the response is Created
        Assert.True(response.StatusCode == HttpStatusCode.Created);

        // Assert the response body
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var createdUser = JsonSerializer.Deserialize<UserModel>(await response.Content.ReadAsStringAsync(), options);
        Assert.Equal(user.Name, createdUser.Name); // Adjust these assertions based on your user model


        // Confirm the user was actually created
        stopwatch.Restart();
        var confirmResponse = await _client.GetAsync($"/api/user/{createdUser.Id}");
        stopwatch.Stop();

        // Assert that the response time is less than 1000 ms
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "The request took too long.");
        // Assert that the request was successful
        confirmResponse.EnsureSuccessStatusCode();
        var confirmedUser =
            JsonSerializer.Deserialize<UserModel>(await confirmResponse.Content.ReadAsStringAsync(), options);

        // Assert that the confirmed user matches the created user
        Assert.Equal(createdUser.Id, confirmedUser.Id);
        Assert.Equal(createdUser.Name, confirmedUser.Name); // Adjust these assertions based on your user model
    }

    [Fact]
    public async Task UpdateExistingUserTest()
    {
        // Arrange
        var updateUser = new UserWriteDto("TestName", "Test", "updated@email.dk", "Test", "UpdatedTest", 1);

        // Login and setup the client with the token and an authorization header
        await LoginAndSetupClientAsync();

        // Act and Assert
        var updatedUser = await UpdateUserAndAssert("/api/user/1", updateUser);

        // Assert that the response contains an updated user
        Assert.True(updatedUser.Email == "updated@email.dk");
        Assert.True(updatedUser.Name == "TestName");
    }


    [Fact]
    public async Task UpdateNonExistingUserTest()
    {
        // Arrange
        var updateUser = new UserWriteDto("Test", "Test", "updated@email.dk", "Test", "UpdatedTest", 1);

        // Login and setup the client with the token and an authorization header
        await LoginAndSetupClientAsync();

        // Act and Assert
        var responseNotFound = await UpdateUser("/api/user/99999", updateUser);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, responseNotFound.StatusCode);
    }

    [Fact]
    public async Task UpdateUserWithInvalidIdTest()
    {
        // Arrange
        var updateUser = new UserWriteDto("Test", "Test", "updated@email.dk", "Test", "UpdatedTest", 1);

        // Login and setup the client with the token and an authorization header
        await LoginAndSetupClientAsync();

        // Act and Assert
        var responseBadRequest = await UpdateUser("/api/user/t", updateUser);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, responseBadRequest.StatusCode);
    }

    private async Task<UserModel> UpdateUserAndAssert(string url, UserWriteDto updateUser)
    {
        var response = await UpdateUser(url, updateUser);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.True(response.StatusCode == HttpStatusCode.OK);

        // Deserialize the response to a user
        var updatedUser = JsonSerializer.Deserialize<UserModel>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        );

        return updatedUser;
    }

    private async Task<HttpResponseMessage> UpdateUser(string url, UserWriteDto updateUser)
    {
        var stopwatch = Stopwatch.StartNew();

        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = new StringContent(
                JsonSerializer.Serialize(updateUser),
                Encoding.UTF8,
                "application/json"
            )
        };
        var response = await _client.SendAsync(request);

        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "The request took too long.");

        return response;
    }

    /// <summary>
    ///     Helper method for all test that needs to be authorized Login and setup the client with the token
    /// </summary>
    private async Task LoginAndSetupClientAsync()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();

        // serialize the user to json
        var content = new StringContent(
            JsonSerializer.Serialize(new UserLoginDto("user1@svendeprøven.dk", "Password12345", null)),
            Encoding.UTF8,
            "application/json"
        );
        // Act
        // send the request
        var loginResponse = await _client.PostAsync("/api/login", content);
        var loginResponseString = await loginResponse.Content.ReadAsStringAsync();

        var resultToken = JsonSerializer.Deserialize<TokenModelDto>(loginResponseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var tokenHeader = new AuthenticationHeaderValue("Bearer", resultToken.Token);
        _client.DefaultRequestHeaders.Authorization = tokenHeader;

        stopwatch.Stop();
        // Assert that the request was successful and took less than 1000 ms
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Login and setup took too long.");
    }
}