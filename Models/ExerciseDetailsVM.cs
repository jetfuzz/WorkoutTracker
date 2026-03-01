namespace WorkoutTracker.Models
{
    public class ExerciseDetailsVM
    {
        public Exercise? Exercise { get; set; } 
        public List<WorkoutVM> WorkoutData { get; set; } = new(); 
    }

    public class WorkoutVM
    {
        public DateTime Date { get; set; } 
        public double HighestWeight { get; set; } 
    }
}
