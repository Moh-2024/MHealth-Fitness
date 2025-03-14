using System.ComponentModel.DataAnnotations;

namespace MHealth_Fitness.Shared.DTO
{
    // Data Transfer Object for Routine
    public class RoutineDto
    {
        // UserID and RoutineID properties
        public int UserID { get; set; }
        public int RoutineID { get; set; }
        [Required(ErrorMessage ="Routine Name is required")]//Validation
        public string RoutineName { get; set; }
    }
}
