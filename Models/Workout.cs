using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Workout
    {
        public int Id { get; set; }
        //public int UserId { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public string? Name { get; set; } = string.Empty;
        public List<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}
