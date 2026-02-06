using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;

namespace WorkoutTracker.Data
{
    public class WorkoutTrackerContext : DbContext
    {
        public WorkoutTrackerContext (DbContextOptions<WorkoutTrackerContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //many to many relationship between Workout and Exercise through WorkoutExercise
            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);
            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany(e => e.WorkoutExercises)
                .HasForeignKey(we => we.ExerciseId);
        }

        public DbSet<WorkoutExercise> WorkoutExercises { get; set; } = default!;
        public DbSet<Exercise> Exercises { get; set; } = default!;
        public DbSet<Workout> Workouts { get; set; } = default!;
        public DbSet<MuscleGroup> MuscleGroups { get; set; } = default!;
    }
}
