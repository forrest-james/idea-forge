using Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Persistence
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await EnsureRole(roleManager, Roles.Admin);
            await EnsureRole(roleManager, Roles.Standard);
            await EnsureRole(roleManager, Roles.Test);

            await EnsureUser(userManager, "admin@ideaforge.local", "4Dm1n!01", Roles.Admin);
            await EnsureUser(userManager, "testuser@ideaforge.local", "Test123!", Roles.Test);
        }

        private static async Task EnsureRole(RoleManager<IdentityRole> roleManager, string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        private static async Task EnsureUser(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
                var create = await userManager.CreateAsync(user, password);
                if (!create.Succeeded)
                    throw new InvalidOperationException(string.Join("; ", create.Errors.Select(e => e.Description)));

            }

            if (!await userManager.IsInRoleAsync(user, role))
                await userManager.AddToRoleAsync(user, role);
        }
    }
}