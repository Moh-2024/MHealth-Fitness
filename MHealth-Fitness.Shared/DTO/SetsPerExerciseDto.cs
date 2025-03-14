using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHealth_Fitness.Shared.DTO
{
    // Data Transfer Object for SetsPerExercise
    public class SetsPerExerciseDto
    {
        public int SetId { get; set; } 

        public int RoutineExerciseId { get; set; }  // Foreign key to RoutineExercise

        public int SetNumber { get; set; }  // Auto-incremented in API

        public int? Reps { get; set; }  // Nullable reps 

        public double Weight { get; set; } = 0.0;  // Default weight in pounds
    }

}
