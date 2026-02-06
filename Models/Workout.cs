namespace WorkoutTracker.Models
{
    public class Workout
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
