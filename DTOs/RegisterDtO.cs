using System.ComponentModel.DataAnnotations;

namespace datingAppreal.DTOs
{
    public class RegisterDtO
    {
       [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(15,MinimumLength =6)]
        public string Password { get; set; }
    }
}
