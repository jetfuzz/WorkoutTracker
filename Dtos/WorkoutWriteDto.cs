namespace WorkoutTracker.Dtos
{
    public class WorkoutWriteDto
    {
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public List<WorkoutExerciseWriteDto> Exercises { get; set; } = new();
    }
    public class WorkoutExerciseWriteDto
    {
        public int? Id { get; set; } 
        public int ExerciseId { get; set; }
        public List<SetWriteDto> Sets { get; set; } = new();
    }
    public class SetWriteDto
    {
        public int? Id { get; set; } 
        public int SetNumber { get; set; }
        public int Repetitions { get; set; }
        public double Weight { get; set; }
    }
}
