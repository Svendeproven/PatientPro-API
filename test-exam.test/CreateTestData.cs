using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using exam_api_project.models.Dtos;

namespace test_exam.test;

/// <summary>
///     Class for creating test data in the database before testing
/// </summary>
public class CreateTestData
{
    private readonly HttpClient _client;

    public CreateTestData(HttpClient client)
    {
        _client = client;
    }

    // method for arranging data in the database before testing
    public async Task CreateUsersTestDataAsync()
    {
        // create the first user with the valid test token
        // Prepare a user object to be added to the database by calling the FirstUserController
        var firstUser = new UserWriteDto(
            "John Doe"
            , Email: "John@Doe.dk"
            , Password: "JohnDoe12345"
            , Role: "admin"
            , JobTitle: "Supervisor"
            , DepartmentModelId: 1
        );

        // Serialize the user object to JSON and prepare the HTTP request content
        var contentUser = new StringContent(
            JsonSerializer.Serialize(firstUser),
            Encoding.UTF8,
            "application/json"
        );

        // Define the valid test token
        var token = "valid_token";

        // Send a request to create the first user with the valid test token
        var responseUser = await _client.PostAsync($"/api/FirstUser/{token}", contentUser);


        // login and get the token for the first user
        var login = new UserLoginDto(
            firstUser.Email,
            firstUser.Password,
            null
        );
        // Serialize the user object to JSON and prepare the HTTP request content
        var contentLogin = new StringContent(
            JsonSerializer.Serialize(login),
            Encoding.UTF8,
            "application/json"
        );

        // Send a request to login with the first user
        var responseLogin = await _client.PostAsync("/api/login", contentLogin);

        // convert the response to a string
        var responseString = await responseLogin.Content.ReadAsStringAsync();
        // Deserialize the response string to a TokenDto object
        var resultToken = JsonSerializer.Deserialize<TokenModelDto>(responseString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });


        // create a header with the token
        var tokenHeader = new AuthenticationHeaderValue("Bearer", resultToken.Token);

        // Add the token to the client
        _client.DefaultRequestHeaders.Authorization = tokenHeader;

        var users = new List<UserWriteDto>();
        // Create 10 new users 
        for (var i = 1; i <= 10; i++)
            users.Add(new UserWriteDto
            (
                $"User{i}"
                , Email: $"user{i}@svendeprøven.dk"
                , Password: $"Password{i}2345"
                , Role: i < 4 ? "admin" : "user"
                , JobTitle: "TestPerson"
                , DepartmentModelId: 1
            ));

        foreach (var user in users)
        {
            var resp = await _client.PostAsync("/api/User", new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json"
            ));
            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
            Console.WriteLine($"User {user.Name} created");
        }

        await CreateDepartmentsTestDataAsync();
    }

    /// <summary>
    ///     Create 10 new departments as mock data
    /// </summary>
    public async Task CreateDepartmentsTestDataAsync()
    {
        var departments = new List<DepartmentWriteDto>();
        // Create 10 new departments
        for (var i = 1; i <= 10; i++)
            departments.Add(new DepartmentWriteDto
            (
                $"Department{i}"
            ));
        // Send a request to create the departments
        foreach (var department in departments)
        {
            var resp = await _client.PostAsync("/api/Department", new StringContent(
                JsonSerializer.Serialize(department),
                Encoding.UTF8,
                "application/json"
            ));
            Assert.Equal(HttpStatusCode.Created, resp.StatusCode);
        }
    }
}