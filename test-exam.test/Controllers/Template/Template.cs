// using System.Diagnostics;
// using System.Net.Http.Headers;
// using System.Text;
// using System.Text.Json;
// using exam_api_project.models.Dtos;
// using exam_api_project.Repositories.Context;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace test_exam.test.Controllers.Template;
//
// public class Template : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
// {
//     private readonly CustomWebApplicationFactory<Program> _factory;
//     private HttpClient _client = null!;
//     private ExamContext _dbContext = null!;
//
//     public Template(CustomWebApplicationFactory<Program> factory)
//     {
//         _factory = factory;
//     }
//
//     /// <summary>
//     ///     Initialize is run before each test
//     /// </summary>
//     public async Task InitializeAsync()
//     {
//         await _factory.StartDatabaseContainerAsync();
//         _client = _factory.CreateClient();
//         var createTest = new CreateTestData(_client);
//         await createTest.CreateUsersTestDataAsync();
//
//         var scope = _factory.Services.CreateScope();
//         _dbContext = scope.ServiceProvider.GetRequiredService<ExamContext>();
//     }
//
//     /// <summary>
//     ///     Cleans upp the database after each test run and resets the primary key sequence
//     ///     And prepares the database for the next test run
//     /// </summary>
//     public async Task DisposeAsync()
//     {
//         // Retrieve all users from the database
//         var allUsers = _dbContext.Users.ToList();
//
//         // Remove all users from the database
//         _dbContext.Users.RemoveRange(allUsers);
//
//         // Save changes to the database
//         await _dbContext.SaveChangesAsync();
//
//         // Reset the primary key sequence (assumes the table name is "Users" and the primary key column is "Id")
//         var tableName = "Users";
//         var primaryKeyColumnName = "Id";
//         var sequenceName = $"{tableName}_{primaryKeyColumnName}_seq";
//
//         // Create the raw SQL command to reset the primary key sequence
//         var resetSequenceSqlCommand = $"ALTER SEQUENCE \"{sequenceName}\" RESTART WITH 1;";
//
//         // Execute the raw SQL command
//         await _dbContext.Database.ExecuteSqlRawAsync(resetSequenceSqlCommand);
//     }
//
//     /// <summary>
//     ///     Helper method for all test that needs to be authorized Login and setup the client with the token
//     /// </summary>
//     private async Task LoginAndSetupClientAsync()
//     {
//         var stopwatch = Stopwatch.StartNew();
//
//         // serialize the user to json
//         var content = new StringContent(
//             JsonSerializer.Serialize(new UserLoginDto("user1@svendeprøven.dk", "Password12345", null)),
//             Encoding.UTF8,
//             "application/json"
//         );
//
//         var loginResponse = await _client.PostAsync("/api/login", content);
//         var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
//
//         var resultToken = JsonSerializer.Deserialize<TokenModelDto>(loginResponseString, new JsonSerializerOptions
//         {
//             PropertyNameCaseInsensitive = true
//         });
//
//         var tokenHeader = new AuthenticationHeaderValue("Bearer", resultToken.Token);
//         _client.DefaultRequestHeaders.Authorization = tokenHeader;
//
//         stopwatch.Stop();
//         Assert.True(stopwatch.ElapsedMilliseconds < 1000, "Login and setup took too long.");
//     }
//
//     [Fact]
//     public async Task GetUserByIdAsync()
//     {
//         await LoginAndSetupClientAsync();
//         Assert.True(true);
//     }
// }

