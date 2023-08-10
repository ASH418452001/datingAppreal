using datingAppreal.Entities;

using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace datingAppreal.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.User.AnyAsync()) return;
            var UserData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var Users = JsonSerializer.Deserialize<List<User>>(UserData,options);

            foreach (var user in Users)
            {
                var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$0rd"));
                user.PasswordSalt = hmac.Key;
                context.User.Add(user);

                
            }await context.SaveChangesAsync();
        }
    }
}
