namespace WorkoutTracker.Models
{
    public class ExerciseDetailsVM
    {
        public Exercise? Exercise { get; set; } 
        //public int ExerciseId { get; set; } //used by controller for filtering workouts
        //public string? ExerciseName { get; set; } = string.Empty;
        //public string? MuscleGroupName { get; set; } = string.Empty;
        public List<WorkoutVM> WorkoutData { get; set; } = new(); //workout details in list form
    }

    public class WorkoutVM
    {
        //public int Id { get; set; }
        //public string Name { get; set; } = string.Empty;    
        public DateTime Date { get; set; } //contains all dates for each workout
        public double HighestWeight { get; set; } //controller only sends highest weight per workout
    }
}
