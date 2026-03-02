using System.ComponentModel.DataAnnotations;

namespace WorkoutTracker.Models
{
    public class Set
    {
        public int Id { get; set; }
        [Required]
        public int WorkoutExerciseId { get; set; }
        public WorkoutExercise? WorkoutExercise { get; set; }
        [Required]
        public int Repetitions { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public int SetNumber { get; set; }
    }
}
