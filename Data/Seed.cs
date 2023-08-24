using datingAppreal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace datingAppreal.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager ,RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;
            var UserData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var Users = JsonSerializer.Deserialize<List<User>>(UserData,options);

            var roles = new List<AppRole>
            {
                new AppRole{Name ="Member" },
                new AppRole{Name ="Admin" },
                new AppRole{Name ="Moderator" },

            };

            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in Users)
            {
                user.UserName = user.UserName.ToLower();
               
               await userManager.CreateAsync(user, "Pa$$w0rd");

                await userManager.AddToRoleAsync(user, "Member");
                
            }

            var admin = new User
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa$$0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }
    }
}
