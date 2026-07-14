using Asp.Versioning;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using TalentFlow.Api.Middleware;
using TalentFlow.Application;
using TalentFlow.Application.Models.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Infrastructure;
using TalentFlow.Persistence;

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
        In = ParameterLocation.Header
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
// JWT
// =========================

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>()!;

// =========================
// Identity
// =========================

builder.Services.AddIdentity<User, Role>(options =>
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

builder.Services.AddAuthentication(options =>
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
// Google OAuth
// =========================

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

if (!string.IsNullOrWhiteSpace(googleClientId) &&
    !string.IsNullOrWhiteSpace(googleClientSecret))
{
    builder.Services.AddAuthentication()
        .AddGoogle(options =>
        {
            options.ClientId = googleClientId;
            options.ClientSecret = googleClientSecret;
        });
}

// =========================
// Authorization
// =========================

builder.Services.AddAuthorization();

// =========================
// CORS
// =========================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// =========================
// Other Services
// =========================

builder.Services.AddMemoryCache();

builder.Services.AddHealthChecks();

builder.Services.AddHangfire(config => config
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(
        builder.Configuration.GetConnectionString("TalentFlowConnection")));

builder.Services.AddHangfireServer();

var app = builder.Build();


// =========================
// Seed Database
// =========================

using (var scope = app.Services.CreateScope())
{
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

await SeedRole.SeedRoleAsync(roleManager);
await SeedUser.SeedUserAsync(userManager, context);
}

// =========================
// Configure Middleware
// =========================

if (app.Environment.IsDevelopment())
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

// =========================
// Hangfire Dashboard
// =========================

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
DashboardTitle = "TalentFlow Jobs",
Authorization = new[] { new HangfireAuthorizationFilter() }
});

// =========================
// Endpoints
// =========================

app.MapHealthChecks("/health");

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapGet("/api", () => Results.Redirect("/swagger"));

app.MapControllers();

// =========================
// Run
// =========================

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

public partial class Program
{
}