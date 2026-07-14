<<<<<<< HEAD
﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
=======
using Asp.Versioning;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
>>>>>>> 03bfb6c64f9b0d5db4cb2b9bda19a411c4c31c7a
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Persistence;
using MediatR;
using TalentFlow.Application;
using System.Reflection;
using TalentFlow.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// =========================
// Add Services
// =========================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

// =========================
// Swagger
// =========================

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TalentFlow API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT token only"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// =========================
// Database
// =========================

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TalentFlowConnection"));
});

// =========================
// JWT Settings
// =========================

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()!;

// =========================
// Identity
// =========================

builder.Services
    .AddIdentity<User, Role>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// =========================
// Authentication
// =========================

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key)),

            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateLifetime = true,

            ClockSkew = TimeSpan.Zero
        };
    });

// =========================
// Authorization
// =========================

builder.Services.AddAuthorization();

// =========================
// CORS
// =========================

builder.Services.AddCors(options =>
{
<<<<<<< HEAD
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
=======
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
builder.Services.AddHealthChecks();

// Add Hangfire for background job processing
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("TalentFlowConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true
    }));

// Add Hangfire server
builder.Services.AddHangfireServer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = false;

    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,

        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])
        )
    };
});

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
>>>>>>> 03bfb6c64f9b0d5db4cb2b9bda19a411c4c31c7a
    });
});

var app = builder.Build();

// =========================
// Seed Database
// =========================

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

<<<<<<< HEAD
    await SeedRole.SeedRoleAsync(roleManager);
    await SeedUser.SeedUserAsync(userManager, context);
}

// =========================
// Middleware
// =========================

if (app.Environment.IsDevelopment())
=======
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Configuration.GetValue<bool>("Swagger:Enabled"))
>>>>>>> 03bfb6c64f9b0d5db4cb2b9bda19a411c4c31c7a
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TalentFlow API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

<<<<<<< HEAD
app.MapControllers();

app.Run();
=======
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
>>>>>>> 03bfb6c64f9b0d5db4cb2b9bda19a411c4c31c7a
