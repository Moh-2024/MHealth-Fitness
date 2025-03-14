using System.ComponentModel.DataAnnotations;

namespace MHealth_FitnessAPI.Models
{
    // Role class
    public class Role
	{
		[Key]// Primary key
        public int RoleID {  get; set; }
		public string RoleName { get; set; }
		public string RoleDesc { get; set; }

		public ICollection<User> Users { get; set; }// Users property
    }
}
