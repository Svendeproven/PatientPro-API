global using Serilog;
global using AutoMapper;
using System.Text;
using DotNetEnv;
using exam_api_project.MiddleWares;
using exam_api_project.models;
using exam_api_project.Repositories;
using exam_api_project.Repositories.Context;
using exam_api_project.Repositories.Interfaces;
using exam_api_project.Services;
using exam_api_project.Services.Interfaces;
using exam_api_project.Services.Interfaces.Security;
using exam_api_project.Services.Security;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// variable for CORS
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.
builder.Services.AddControllers();

//* add automapper to the project for mapping between entities and DTOs
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//* Setup dependency injection for repositories START
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IPushNotificationService, PushNotificationService>();

builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IMedicineService, MedicineService>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddScoped<IPatientTodoRepository, PatientTodoRepository>();
builder.Services.AddScoped<IPatientTodoService, PatientTodoService>();

builder.Services.AddScoped<IPatientMedicineRepository, PatientMedicineRepository>();
builder.Services.AddScoped<IPatientMedicineService, PatientMedicineService>();

builder.Services.AddScoped<IPatientJournalRepository, PatientJournalRepository>();
builder.Services.AddScoped<IPatientJournalService, PatientJournalService>();

//* implement Serial logger for logging of errors and information
Log.Logger = new LoggerConfiguration() // create a new logger
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.File("log/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.ClearProviders(); // clear default logging providers
builder.Logging.AddSerilog(); // add serilog as logging provider


//* Load the .env file if present.
Env.Load();
//* getting the environment variables that is injected into the docker container in runtime
var username = Environment.GetEnvironmentVariable("POSTGRES_USER");
var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
var database = Environment.GetEnvironmentVariable("POSTGRES_DB");
var host = Environment.GetEnvironmentVariable("POSTGRES_HOST");
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT");
var postgresConnectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

//* add the connection string to the database context
builder.Services.AddDbContext<ExamContext>(options => options.UseNpgsql(postgresConnectionString));

//* add the first user token to the options
var firstUserToken = Environment.GetEnvironmentVariable("FIRST_USER_TOKEN");
//* injects a new instance of FirstUserEnvironmentTokenOptions during test
builder.Services.Configure<FirstUserEnvironmentTokenOptions>(options => options.Token = firstUserToken);


//* enable cors for specific origin allow put / delete / post / get and options
builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins(
                    Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "https://svendeprøven.dk"
                )
                .AllowAnyHeader()
                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
        });
});
//* Get JWT Key from environment variable
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
//* add this following line to add JWT authentication and in the controller add [Authorize] attribute
//* also install Microsoft.AspNetCore.Authentication.JwtBearer nuget Package
//* to generate a test token use "dotnet user-jwts create" command
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "exam-api",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});


var app = builder.Build();


//* Migrate latest database changes during startup if there are any new migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ExamContext>();
    // Here is the migration executed
    dbContext.Database.Migrate();
}

// prepares the firebase app for use
try
{
    FirebaseApp.Create(new AppOptions
    {
        Credential = GoogleCredential.GetApplicationDefault()
    });
}
catch (Exception e)
{
    Log.Information(e.Message);
}

//* Get JWT Key from environment variable
app.UseMiddleware<RoleMiddleware>();
app.UseAuthorization();
app.UseCors(myAllowSpecificOrigins);
app.MapControllers();

app.Run();

//* Exposing the program class to the test project
public partial class Program
{
}