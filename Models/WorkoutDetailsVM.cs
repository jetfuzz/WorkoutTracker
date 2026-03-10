namespace WorkoutTracker.Models
{
    public class WorkoutDetailsVM
    {
        public int WorkoutId { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public List<WorkoutExerciseDetailsVM> Exercises { get; set; } = new();
    }

    public class WorkoutExerciseDetailsVM
    {
        public int ExerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public List<SetVM> Sets { get; set; } = new();
        public bool isBestWeight { get; set; }
    }
}
