namespace WorkoutTracker.Dtos
{
    public class WorkoutDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public List<WorkoutExerciseDto> Exercises { get; set; } = new();
    }

    public class WorkoutExerciseDto
    {
        public int Id { get; set; }
        public string? ExerciseName { get; set; }
        public List<SetDto> Sets { get; set; } = new();
    }

    public class SetDto
    {
        public int SetNumber { get; set; }
        public int Repetitions { get; set; }
        public double Weight { get; set; }
    }
}
