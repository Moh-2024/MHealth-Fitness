using System.ComponentModel.DataAnnotations;

namespace MHealth_FitnessAPI.Models
{
    // Exercise class
    public class Exercise
	{
		[Key]// Primary key
        public int ExerciseID { get; set; } // ExerciseID property
        public string ExerciseName { get; set; }
		[MaxLength(1000)] // Max length of 1000 characters
        public string? ExerciseDesc { get; set; }
		public int BodyPartID { get; set; }
		public BodyPart BodyPart { get; set; }
		public string? ImageUrl1 { get; set; }// ImageUrl1 property
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }
    }
}
