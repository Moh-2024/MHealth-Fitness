using System.ComponentModel.DataAnnotations;

namespace MHealth_FitnessAPI.Models
{
    // Favorite class
    public class Favorite
	{
		[Key]// Primary key
        public int FavoriteID { get; set; } // FavoriteID property
        public int UserID { get; set; }
		public int ExerciseID { get; set; }
	
	}
}
