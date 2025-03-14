using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHealth_FitnessAPI.Models
{
    public class SetsPerExercise
    {
        [Key]
        public int SetId { get; set; }

        public int RoutineExerciseId { get; set; }
        public virtual RoutineExercise RoutineExercise { get; set; }

        public int SetNumber { get; set; } // Auto-incremented in API

        public int? Reps { get; set; } // Nullable field, allowing sets without defined reps

        public decimal Weight { get; set; } = 0.0m; // Default weight in pounds
    }
}
