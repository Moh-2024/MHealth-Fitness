using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHealth_Fitness.Shared.DTO
{
    // Data Transfer Object for Favorite
    public class FavoriteDto
	{
        // FavoriteID property
        public int FavoriteID { get; set; }
        // UserID and ExerciseID properties
        public int UserID { get; set; }
		public int ExerciseID { get; set; }
    }
}
