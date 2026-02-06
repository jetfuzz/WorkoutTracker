namespace WorkoutTracker.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public int MuscleGroupId { get; set; }
        public MuscleGroup? MuscleGroup { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
