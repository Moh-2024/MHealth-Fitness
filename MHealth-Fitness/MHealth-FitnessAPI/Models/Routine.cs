using System.ComponentModel.DataAnnotations;

namespace MHealth_FitnessAPI.Models
{
    // Routine class
    public class Routine
	{
		[Key]// Primary key
        public int RoutineID { get; set; }
		public string RoutineName { get; set; }
		public int UserID { get; set; }
		public User user { get; set; } // User property
    }
}
