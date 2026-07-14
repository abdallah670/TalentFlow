using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Entities.TenantModule;
using TalentFlow.Domain.Enums;
using TalentFlow.Persistence;

public static class SeedUser
{
    public static async Task SeedUserAsync(
        UserManager<User> userManager,
        AppDbContext context)
    {
        var tenant = await context.Tenants.FirstOrDefaultAsync(t => t.Slug == "demo-company");
        if (tenant is null)
        {
            tenant = new Tenant
            {
                Name = "Demo Company",
                Slug = "demo-company",
                SubscriptionPlan = "Free",
                IsActive = true
            };
            context.Tenants.Add(tenant);
            await context.SaveChangesAsync();
        }

        var usersToSeed = new List<(string Email, string FirstName, string LastName, string Role, Guid? TenantId)>
        {
            ("systemadmin@talentflow.com", "System", "Admin", nameof(Roles.SystemAdmin), null),
            ("tenantadmin@demo.com",       "Tenant", "Admin", nameof(Roles.TenantAdmin), tenant.Id),
            ("recruiter@demo.com",         "Sara",   "Recruiter", nameof(Roles.Recruiter), tenant.Id),
            ("hiringmanager@demo.com",     "Omar",   "Manager", nameof(Roles.HiringManager), tenant.Id),
            ("interviewer@demo.com",       "Laila",  "Interviewer", nameof(Roles.Interviewer), tenant.Id),
        };

        const string defaultPassword = "P@ssw0rd123!";

        foreach (var u in usersToSeed)
        {
            var existingUser = await userManager.FindByEmailAsync(u.Email);
            if (existingUser is not null)
                continue;

            var user = new User
            {
                UserName = u.Email,
                Email = u.Email,
                EmailConfirmed = true,
                FirstName = u.FirstName,
                LastName = u.LastName,
                TenantId = u.TenantId ?? Guid.Empty, 
                IsActive = true
            };

            var result = await userManager.CreateAsync(user, defaultPassword);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, u.Role);
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                Console.WriteLine($"Failed to seed user {u.Email}: {errors}");
            }
        }
    }
}