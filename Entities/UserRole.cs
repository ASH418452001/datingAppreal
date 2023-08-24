using Microsoft.AspNetCore.Identity;

namespace datingAppreal.Entities
{
    public class UserRole : IdentityUserRole<int>
    {
        public User USER { get; set; }
        public AppRole Role { get; set; }
    }
}
