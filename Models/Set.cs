namespace WorkoutTracker.Models
{
    public class Set
    {
        public int Id { get; set; }
        public int WorkoutExerciseId { get; set; }
        public WorkoutExercise? WorkoutExercise { get; set; }
        public int Repetitions { get; set; }
        public double Weight { get; set; }
        public int SetNumber { get; set; }
    }
}
