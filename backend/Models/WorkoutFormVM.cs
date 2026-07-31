namespace WorkoutTracker.Models
{
    public class WorkoutFormVM
    {
        public int WorkoutId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Today;
        public List<WorkoutExerciseVM> Exercises { get; set; } = new();

        //for populating modal
        public List<MuscleGroup> MuscleGroups { get; set; } = new();
        public List<Exercise> AllExercises { get; set; } = new();
    }

    public class WorkoutExerciseVM
    {
        //nullable Id fields for distinguishing new exercises & sets on Edit/Post
        public int? Id { get; set; }
        public int ExerciseId { get; set; }
        public List<SetVM> Sets { get; set; } = new();
    }

    public class SetVM
    {
        public int? Id { get; set; }
        public int SetNumber { get; set; }
        public int Repetitions { get; set; }
        public double Weight { get; set; }
    }
}
