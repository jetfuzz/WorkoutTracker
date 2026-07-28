using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WorkoutTracker.Data;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    [Authorize]
    public class WorkoutsController : Controller
    {
        private readonly WorkoutTrackerContext _context;

        public WorkoutsController(WorkoutTrackerContext context)
        {
            _context = context;
        }

        // GET: Workouts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var context = _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Sets)
                .Where(m => m.UserId == userId)
                .OrderByDescending(w => w.Date);
            return View(await context.ToListAsync());
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Sets)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (workout == null) return NotFound();

            WorkoutDetailsVM vm = new WorkoutDetailsVM();

            vm.WorkoutId = workout.Id;
            vm.Name = workout.Name;
            vm.Date = workout.Date;

            if (workout.WorkoutExercises != null)
            {
                vm.Exercises = workout.WorkoutExercises.Select(we => new WorkoutExerciseDetailsVM
                {
                    ExerciseId = we.ExerciseId,
                    ExerciseName = we.Exercise.Name,
                    Sets = we.Sets.Select(s => new SetVM
                    {
                        Repetitions = s.Repetitions,
                        Weight = s.Weight,
                        SetNumber = s.SetNumber
                    }).ToList()
                }).ToList();
            }

            var bestWeights = await _context.Set
                .Where(s => s.WorkoutExercise.Workout.UserId == userId)
                .GroupBy(s => s.WorkoutExercise.ExerciseId)
                .Select(g => new { ExerciseId = g.Key, BestWeight = g.Max(x => x.Weight) })
                .ToDictionaryAsync(x => x.ExerciseId, x => x.BestWeight);

            foreach (var exercise in vm.Exercises)
            {
                var bestWeight = bestWeights.GetValueOrDefault(exercise.ExerciseId, 0);
                exercise.isBestWeight = exercise.Sets.Any(s => s.Weight >= bestWeight);
            }

            return View(vm);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            WorkoutFormVM vm = new WorkoutFormVM();
            vm.AllExercises = _context.Exercises.Include(e => e.MuscleGroup).ToList();
            vm.MuscleGroups = _context.MuscleGroups.ToList();
            return View(vm);
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutFormVM vm)
        {
            if (ModelState.IsValid)
            {
                Workout workout = new Workout
                {
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
                    Name = vm.Name,
                    Date = vm.Date,
                    WorkoutExercises = new List<WorkoutExercise>()
                };

                foreach (var exercise in vm.Exercises)
                {
                    WorkoutExercise workoutExercise = new WorkoutExercise
                    {
                        ExerciseId = exercise.ExerciseId,
                        Sets = new List<Set>()
                    };
                    foreach (var set in exercise.Sets)
                    {
                        Set workoutSet = new Set
                        {
                            Repetitions = set.Repetitions,
                            Weight = set.Weight,
                            SetNumber = set.SetNumber
                        };
                        workoutExercise.Sets.Add(workoutSet);
                    }
                    workout.WorkoutExercises.Add(workoutExercise);
                }

                _context.Add(workout);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            WorkoutFormVM vm = new WorkoutFormVM();
            vm.AllExercises = _context.Exercises.Include(e => e.MuscleGroup).ToList();
            vm.MuscleGroups = _context.MuscleGroups.ToList();

            if (id == null)
            {
                return NotFound();
            }


            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Sets)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (workout == null)
            {
                return NotFound();
            }

            //map the workout object to the workout form view model
            vm.WorkoutId = workout.Id;
            vm.Name = workout.Name;
            vm.Date = workout.Date;

            if (workout.WorkoutExercises != null)
            {
                vm.Exercises = workout.WorkoutExercises.Select(we => new WorkoutExerciseVM
                {
                    Id = we.Id,
                    ExerciseId = we.ExerciseId,
                    Sets = we.Sets.Select(s => new SetVM
                    {
                        Id = s.Id,
                        Repetitions = s.Repetitions,
                        Weight = s.Weight,
                        SetNumber = s.SetNumber
                    }).ToList()
                }).ToList();
            }

            return View(vm);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkoutFormVM vm)
        {
            if (id != vm.WorkoutId) return NotFound();

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var workout = await _context.Workouts
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Sets)
                    .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
                if (workout == null) return NotFound();

                try
                {
                    workout.Date = vm.Date;
                    workout.Name = vm.Name;

                    var exercisesToRemove = workout.WorkoutExercises
                        .Where(we => !vm.Exercises.Any(e => e.Id == we.Id))
                        .ToList();
                    
                    _context.WorkoutExercises.RemoveRange(exercisesToRemove);

                    // Update existing exercises and sets, and add new ones
                    foreach (var exerciseVM in vm.Exercises)
                    {
                        var existingExercise = workout.WorkoutExercises
                            .FirstOrDefault(we => we.Id == exerciseVM.Id);
                        if (existingExercise != null)
                        {
                            // Update existing exercise
                            existingExercise.ExerciseId = exerciseVM.ExerciseId;
                            // Remove deleted sets
                            var setsToRemove = existingExercise.Sets
                                .Where(s => !exerciseVM.Sets.Any(svm => svm.Id == s.Id))
                                .ToList();
                            _context.Set.RemoveRange(setsToRemove);
                            // Update existing sets and add new ones
                            foreach (var setVM in exerciseVM.Sets)
                            {
                                var existingSet = existingExercise.Sets
                                    .FirstOrDefault(s => s.Id == setVM.Id);
                                if (existingSet != null)
                                {
                                    // Update existing set
                                    existingSet.Repetitions = setVM.Repetitions;
                                    existingSet.Weight = setVM.Weight;
                                    existingSet.SetNumber = setVM.SetNumber;
                                }
                                else
                                {
                                    // Add new set
                                    Set newSet = new Set
                                    {
                                        Repetitions = setVM.Repetitions,
                                        Weight = setVM.Weight,
                                        SetNumber = setVM.SetNumber,
                                        WorkoutExerciseId = existingExercise.Id
                                    };
                                    _context.Set.Add(newSet);
                                }
                            }
                        }
                        else
                        {
                            // Add new exercise and its sets
                            WorkoutExercise newWorkoutExercise = new WorkoutExercise
                            {
                                ExerciseId = exerciseVM.ExerciseId,
                                WorkoutId = workout.Id,
                                Sets = exerciseVM.Sets.Select(svm => new Set
                                {
                                    Repetitions = svm.Repetitions,
                                    Weight = svm.Weight,
                                    SetNumber = svm.SetNumber
                                }).ToList()
                            };
                            _context.WorkoutExercises.Add(newWorkoutExercise);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            vm.AllExercises = _context.Exercises.Include(e => e.MuscleGroup).ToList();
            vm.MuscleGroups = _context.MuscleGroups.ToList();
            return View(vm);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workouts.
                FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workouts.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (workout != null)
            {
                _context.Workouts.Remove(workout);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(int id)
        {
            return _context.Workouts.Any(e => e.Id == id);
        }
    }
}
