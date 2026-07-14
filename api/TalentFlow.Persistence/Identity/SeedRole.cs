using Microsoft.AspNetCore.Identity;
using TalentFlow.Domain.Entities.IdentityModule;
using TalentFlow.Domain.Enums;


    public static class SeedRole
    {
        public static async Task SeedRoleAsync(RoleManager<Role> roleManager)
        {
            var roles = Enum.GetNames(typeof(Roles));
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new Role
                    {
                  ///      Id = Guid.NewGuid(),          
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }
        }
    }
