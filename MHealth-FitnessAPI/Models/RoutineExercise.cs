using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MHealth_FitnessAPI.Models
{
    // RoutineExercise class
    public class RoutineExercise
    {
        [Key]// Primary key
        public int RoutineExerciseID { get; set; }
        public int RoutineID { get; set; }
        public int ExerciseID { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public virtual ICollection<SetsPerExercise> SetsPerExercise { get; set; } = new List<SetsPerExercise>();

    }
}
