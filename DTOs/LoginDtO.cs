using System.ComponentModel.DataAnnotations;

namespace datingAppreal.DTOs
{
    public class LoginDtO
    {
        [Required]
       public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    } 
}
    