using TalentFlow.Api.Middleware;
using TalentFlow.Api.Converters;
using TalentFlow.Infrastructure;

using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using TalentFlow.Application;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Text.Json.Serialization;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Asp.Versioning;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .MinimumLevel.Override("Swashbuckle", Serilog.Events.LogEventLevel.Debug)
    .WriteTo.Console()
    .WriteTo.File("Logs/talentflow-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add environment variables to configuration
builder.Configuration.AddEnvironmentVariables();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
        options.JsonSerializerOptions.Converters.Add(new NullableTimeSpanConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Talent Flow API", Version = "v1" });
    
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Use full names to avoid collisions between DTOs with same names in different namespaces
    options.CustomSchemaIds(type => type.FullName?.Replace("`", "_").Replace("[", "_").Replace("]", "_").Replace(",", "_").Replace(" ", ""));
});

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddRateLimiter(options =>
{
    var config = builder.Configuration.GetSection("RateLimiting");
    options.AddFixedWindowLimiter("Api", opt =>
    {
        opt.PermitLimit = config.GetValue<int>("ApiLimit");
        opt.Window = TimeSpan.FromMinutes(config.GetValue<int>("ApiWindowMinutes"));
    });
    
    options.AddFixedWindowLimiter("Auth", opt =>
    {
        opt.PermitLimit = config.GetValue<int>("AuthLimit");
        opt.Window = TimeSpan.FromMinutes(config.GetValue<int>("AuthWindowMinutes"));
    });

    options.AddFixedWindowLimiter("Feed", opt =>
    {
        opt.PermitLimit = config.GetValue<int>("FeedLimit");
        opt.Window = TimeSpan.FromMinutes(config.GetValue<int>("FeedWindowMinutes"));
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.WithOrigins(
            "http://localhost:4200", 
            "https://localhost:4200",
            "http://localhost:5000",
            "https://localhost:5001",
            "http://localhost:5001",
            "https://localhost:5000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials()
               .SetIsOriginAllowed(_ => true)); // Allow any origin for development
});

// Add Infrastructure Services
builder.Services.AddInfrastructure(builder.Configuration);

// Add Application Services
builder.Services.AddApplication(builder.Configuration);



// Add Memory Cache for search results
builder.Services.AddMemoryCache();

// Add Hangfire for background job processing
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add Hangfire server
builder.Services.AddHangfireServer();



// Identity.Application cookie scheme used automatically by AddIdentity for external login callbacks

// Add OAuth providers (chains to existing JWT auth configured in AddPersistence)
var googleClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
var facebookAppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "";
var facebookAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "";

// Check if OAuth credentials are configured (not empty and not placeholders)
bool hasGoogleOAuth = !string.IsNullOrWhiteSpace(googleClientId) && 
                      !string.IsNullOrWhiteSpace(googleClientSecret) &&
                      !googleClientId.Contains("YOUR_") &&
                      !googleClientId.Contains("example");

bool hasFacebookOAuth = !string.IsNullOrWhiteSpace(facebookAppId) && 
                        !string.IsNullOrWhiteSpace(facebookAppSecret) &&
                        !facebookAppId.Contains("YOUR_") &&
                        !facebookAppId.Contains("example");

// Build authentication chain
var authBuilder = builder.Services.AddAuthentication();

// Identity.External cookie scheme provided automatically by AddIdentity
// OAuth providers will chain to it via SignInScheme = "Identity.External"

// Add Google OAuth if credentials exist
if (hasGoogleOAuth)
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.CallbackPath = "/signin-google";
        options.SignInScheme = "Identity.External";
        options.SaveTokens = true;
    });
    Log.Information("Google OAuth configured");
}

// Add Facebook OAuth if credentials exist
if (hasFacebookOAuth)
{
    authBuilder.AddFacebook(options =>
    {
        options.AppId = facebookAppId;
        options.AppSecret = facebookAppSecret;
        options.CallbackPath = "/signin-facebook";
        options.SignInScheme = "Identity.External";
        options.SaveTokens = true;
    });
    Log.Information("Facebook OAuth configured");
}

if (!hasGoogleOAuth && !hasFacebookOAuth)
{
    Log.Warning("OAuth credentials not found. Social login buttons will fail if clicked.");
}

var app = builder.Build();

// Apply CORS before logging and exceptions to ensure headers are present on error responses
app.UseCors("AllowAll");
app.UseRateLimiter();

// Add Request Logging Middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Talent Flow API v1");
        options.RoutePrefix = "swagger";
    });
}

// Ensure wwwroot/uploads exists
var uploadsPath = Path.Combine(app.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();



// Hangfire Dashboard - accessible at /hangfire
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Talent Flow Jobs",
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

// Redirect to Swagger UI instead of Scalar for free documentation
app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapGet("/api", () => Results.Redirect("/swagger"));

app.MapHealthChecks("/health");
app.MapControllers();

try 
{
    Log.Information("Starting web host");
    
   
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Make Program class accessible for integration testing
public partial class Program { }
