using System.ComponentModel.DataAnnotations;

namespace MHealth_Fitness.Shared.DTO
{
    // Data Transfer Object for Login
    public class LoginDto
    {
        // UserName and Password properties
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
