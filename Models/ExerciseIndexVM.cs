namespace WorkoutTracker.Models
{
    public class ExerciseIndexVM
    {
        public List<Exercise> Exercises { get; set; } = new();
        public List<MuscleGroup> MuscleGroups { get; set; } = new();
    }
}
