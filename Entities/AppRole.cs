using Microsoft.AspNetCore.Identity;

namespace datingAppreal.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
      
    }
}
