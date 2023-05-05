using exam_api_project.models;
using exam_api_project.Repositories;
using exam_api_project.Repositories.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace test_exam.test;


/// <summary>
/// This class creates a new WebApplicationFactory
/// with a new ExamContext and a new FirstUserTokenOptions
/// </summary>
/// <typeparam name="TStartup"></typeparam>
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Program>
{
    // Create a new container for the PostgreSQL database
    private PostgreSqlContainer _postgreSqlContainers = new PostgreSqlBuilder()
        // .WithDatabase("exam_db")
        // .WithUsername("svend")
        // .WithPassword("Svend_postgres")
        .Build();
    
    /// <summary>
    /// Method for replacing the ExamContext with a new one with the connection string from the container
    /// and configuring the FirstUserTokenOptions with a test token
    /// Read more here: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0#customize-webapplicationfactory
    /// </summary>
    /// <param name="builder"></param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Call the base method to configure the web host
        builder.ConfigureServices(services =>
        {
            // Find the existing registration for the ExamContext
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ExamContext>));

            if (dbContextDescriptor != null)
            {
                // Remove the existing registration
                services.Remove(dbContextDescriptor);
            }

            // Add a new registration for the ExamContext with the test container's connection string
            services.AddDbContext<ExamContext>(options =>
                options.UseNpgsql(_postgreSqlContainers.GetConnectionString()));

            // Configure the FirstUserTokenOptions with a test token
            services.Configure<FirstUserEnvironmentTokenOptions>(options => options.Token = "valid_token");
       
        });
    }

    // Start the PostgreSQL container and set the JWT_SECRET environment variable
    public async Task StartDatabaseContainerAsync()
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS","/Users/flemminglyng/Repos/svendeprøven/exam-api/exam-api-project/svendeproven-ee539-firebase-adminsdk-6r2si-aba7112932.json");
        Environment.SetEnvironmentVariable("JWT_SECRET", "5ed1aac2d035758e9b0a81fe4a6f814f72c395242a9758faad0fa5c56080e755a06133d202b68c34ad4af12b3a5b3210074a4bcbf9f8c565886c56965436f773");
        await _postgreSqlContainers.StartAsync();
    }
    
  
}