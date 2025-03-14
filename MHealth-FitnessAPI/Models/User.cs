using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MHealth_FitnessAPI.Models
{
    // User class
    public class User
    {
        [Key]// Primary key
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int RoleID { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }

        public Role Roles { get; set; }// Roles property
    }
}
