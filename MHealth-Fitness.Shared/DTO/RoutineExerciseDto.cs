namespace MHealth_Fitness.Shared.DTO
{
    public class RoutineExerciseDto
    {
        // RoutineExercise properties
        public int RoutineExerciseID { get; set; }
        public int RoutineID { get; set; }
        public int ExerciseID { get; set; }
        // List of sets for this exercise
        public List<SetsPerExerciseDto> SetsList { get; set; } = new List<SetsPerExerciseDto>();

        // Ensure Date Only (No Time)
        private DateTime? _date;
        public DateTime? Date
        {
            
            get => _date;// Return the stored date
            set => _date = value?.Date;// Ensure only date part is stored
        }
    }
}
