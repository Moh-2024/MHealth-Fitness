using System.ComponentModel.DataAnnotations;

namespace MHealth_Fitness.Shared.DTO
{
    // Data Transfer Object for Exercise
    public class ExerciseDto
    {
        
        public int ExerciseID { get; set; }
        [Required(ErrorMessage = "Exercise name is required")]//Validation
        public string ExerciseName { get; set; }
        [StringLength(1000, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? ExerciseDesc { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Body Part must be selected")]
        public int BodyPartID { get; set; }

        // Image URLs
        public string? ImageUrl1 { get; set; }
        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }

    }

}
