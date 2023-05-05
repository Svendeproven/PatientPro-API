using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using exam_api_project.models.Dtos;
using exam_api_project.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace test_exam.test.Controllers;

public class DepartmentControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private HttpClient _client = null!;
    private ExamContext _dbContext = null!;

    public DepartmentControllerTest(CustomWebApplicationFactory<Program> factory)
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
    ///     Creates a Department and returns the Department with status code 201
    /// </summary>
    [Fact]
    public async Task CreateDepartmentAsync()
    {
        await LoginAndSetupClientAsync();
        // Arrange
        var department = new DepartmentWriteDto("TestDepartment");
        var content = new StringContent(
            JsonSerializer.Serialize(department),
            Encoding.UTF8,
            "application/json"
        );
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.PostAsync("/api/department", content);
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "CreateDepartmentAsync took too long.");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DepartmentReadDto>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // Assert
        // in this case we expect a 201 status code
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        // and we expect the title to be the same as the one we sent in
        Assert.Equal("TestDepartment", result.Title);
    }


    /// <summary>
    ///     Test the GetDepartmentByIdAsync method in the DepartmentController with status code 200
    /// </summary>
    [Fact]
    public async Task GetDepartmentByIdReturnsForExistingDepartment()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.GetAsync("/api/department/1");
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "GetDepartmentByIdReturnsForExistingDepartment took too long.");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DepartmentReadDto>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // Assert
        // in this case we expect a 200 status code
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(result.Id == 1);
    }

    /// <summary>
    ///     Method to test the GetDepartmentByIdAsync method in the DepartmentController with status code 404
    /// </summary>
    [Fact]
    public async Task GetDepartmentByIdReturnsNotFoundForNonexistentDepartment()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.GetAsync("/api/department/100");
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "GetDepartmentByIdReturnsNotFoundForNonexistentDepartment took too long.");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    ///     Test the GetDepartmentByIdAsync method in the DepartmentController with status code 400
    /// </summary>
    [Fact]
    public async Task GetDepartmentByIdReturnsBadRequestForInvalidId()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.GetAsync("/api/department/t");
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "GetDepartmentByIdReturnsBadRequestForInvalidId took too long.");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAllDepartmentsAsync()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.GetAsync("/api/department");
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "GetAllDepartmentsAsync took too long.");
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<DepartmentReadDto>>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // Assert
        // in this case we expect a 200 status code
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(result.Count > 5);
    }

    /// <summary>
    ///     Test the UpdateDepartmentAsync method in the DepartmentController with status code 204
    /// </summary>
    [Fact]
    public async Task DeleteDepartmentByIdAsync()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        // create a new department to delete

        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.DeleteAsync("/api/department/1");
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "DeleteDepartmentByIdAsync took too long.");
        // Assert cant delete department with employees
        Assert.True(HttpStatusCode.InternalServerError == response.StatusCode);

        // Act
        response = await _client.DeleteAsync("/api/department/6");
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "DeleteDepartmentByIdAsync took too long.");
        Assert.True(HttpStatusCode.NoContent == response.StatusCode);
    }

    /// <summary>
    ///     Test the UpdateDepartmentAsync method in the DepartmentController with status code 200 and the updated department
    /// </summary>
    [Fact]
    public async Task UpdateExistingDepartmentById()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        var department = new DepartmentWriteDto("New Department");
        var content = new StringContent(
            JsonSerializer.Serialize(department),
            Encoding.UTF8,
            "application/json"
        );
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.PutAsync("/api/department/1", content);
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "UpdateDepartmentById took too long.");
        Assert.True(HttpStatusCode.OK == response.StatusCode);

        // Serialize the response
        var responseString = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<DepartmentReadDto>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // Assert
        Assert.Equal("New Department", result.Title);
    }

    /// <summary>
    ///     Test the UpdateDepartmentAsync method in the DepartmentController with status code 400
    /// </summary>
    [Fact]
    public async void UpdateDepartmentWithInvalidId()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        var department = new DepartmentWriteDto("New Department");
        var content = new StringContent(
            JsonSerializer.Serialize(department),
            Encoding.UTF8,
            "application/json"
        );
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.PutAsync("/api/department/t", content);
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "UpdateDepartmentById took too long.");
        Assert.True(HttpStatusCode.BadRequest == response.StatusCode);
    }

    /// <summary>
    ///     Test the UpdateDepartmentAsync method in the DepartmentController with status code 404
    /// </summary>
    [Fact]
    public async void UpdateNonExistingDepartmentTest()
    {
        // Arrange
        await LoginAndSetupClientAsync();
        var department = new DepartmentWriteDto("New Department");
        var content = new StringContent(
            JsonSerializer.Serialize(department),
            Encoding.UTF8,
            "application/json"
        );
        var stopwatch = Stopwatch.StartNew();
        // Act
        var response = await _client.PutAsync("/api/department/100", content);
        stopwatch.Stop();
        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 1000,
            "UpdateDepartmentById took too long.");
        Assert.True(HttpStatusCode.NotFound == response.StatusCode);
    }

    /// <summary>
    ///     Helper method for all test that needs to be authorized Login and setup the client with the token
    /// </summary>
    private async Task LoginAndSetupClientAsync()
    {
        var stopwatch = Stopwatch.StartNew();

        // serialize the user to json
        var content = new StringContent(
            JsonSerializer.Serialize(new UserLoginDto("user1@svendeprøven.dk", "Password12345", null)),
            Encoding.UTF8,
            "application/json"
        );
        // send a login request
        var loginResponse = await _client.PostAsync("/api/login", content);
        // get the token from the response
        var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
        // deserialize the token
        var resultToken = JsonSerializer.Deserialize<TokenModelDto>(loginResponseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        // set the token in the header
        var tokenHeader = new AuthenticationHeaderValue("Bearer", resultToken.Token);
        // set the token in the client
        _client.DefaultRequestHeaders.Authorization = tokenHeader;

        stopwatch.Stop();
        // assert that the login and setup took less than 1 second
        Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Login and setup took too long.");
    }
}