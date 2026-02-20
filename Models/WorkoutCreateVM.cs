using Microsoft.AspNetCore.Mvc.Rendering;

namespace WorkoutTracker.Models
{
    public class WorkoutCreateVM
    {
        public string? Name { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;
        public SelectList? ExerciseSelectList { get; set; }
        public List<WorkoutExerciseVM> Exercises { get; set; } = new();
    }

    public class WorkoutExerciseVM
    {
        public int ExerciseId { get; set; }
        public List<SetVM> Sets { get; set; } = new();
    }

    public class SetVM
    {
        public int SetNumber { get; set; }
        public int Repetitions { get; set; }
        public double Weight { get; set; }
    }
}
