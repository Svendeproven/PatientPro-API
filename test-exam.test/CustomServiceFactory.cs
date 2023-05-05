using exam_api_project.Repositories;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Interfaces.Security;
using exam_api_project.Services.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace test_exam.test;

/// <summary>
///     Class for creating a service provider for dependency injection in the tests
/// </summary>
public class CustomServiceFactory : IDisposable, IAsyncLifetime
{
    // Prepare a docker container with postgres
    private readonly PostgreSqlContainer _postgreSqlContainer;

    public CustomServiceFactory()
    {
        // Register services for dependency injection
        var services = new ServiceCollection();

        // Register other services if needed
        services.AddTransient<ILoginService, LoginService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IPasswordService, PasswordService>();


        // create a container with postgres database
        _postgreSqlContainer = new PostgreSqlBuilder()
            .Build();

        // Register the ExamContext with the connection string from the container
        services.AddDbContext<ExamContext>(options =>
        {
            options.UseNpgsql(_postgreSqlContainer.GetConnectionString());
        });
        // Build the service provider
        ServiceProvider = services.BuildServiceProvider();
    }

    // This is the service provider that will be used to resolve dependencies
    public ServiceProvider ServiceProvider { get; }


    // Initialize Postgres container because of the IAsyncLifetime
    public async Task InitializeAsync()
    {
        // Start the container
        await _postgreSqlContainer.StartAsync();
        // Get the ExamContext from the service provider
        var dbContext = ServiceProvider.GetService<ExamContext>();
        // Migrate the database to the Docker container
        await dbContext.Database.MigrateAsync();
    }

    // Disposes the container after use and clean up docker containers 
    public async Task DisposeAsync()
    {
        // Dispose the container in a nice way
        await _postgreSqlContainer.DisposeAsync();
    }

    public void Dispose()
    {
        // Clean up your ExamContext or other resources, if needed
        ServiceProvider.Dispose();
    }
}