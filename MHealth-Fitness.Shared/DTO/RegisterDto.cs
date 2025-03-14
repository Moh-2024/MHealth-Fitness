using System.ComponentModel.DataAnnotations;

namespace MHealth_Fitness.Shared.DTO
{
    // Data Transfer Object for Register
    public class RegisterDto
    {
        // User properties
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public int RoleID { get; set; }
    }
}
