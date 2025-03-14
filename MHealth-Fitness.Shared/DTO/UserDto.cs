namespace MHealth_Fitness.Shared.DTO
{
    // Data Transfer Object for User
    public class UserDto
    {
        // UserID and other properties
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public string? RoleName { get; set; }// Optional RoleName for display purposes
        public string Password { get; set; }
    }
}
