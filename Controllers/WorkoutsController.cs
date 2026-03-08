using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Data;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
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
            var context = _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Sets)
                .OrderByDescending(w => w.Date);
            return View(await context.ToListAsync());
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises) 
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Sets)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {
            WorkoutFormVM vm = new WorkoutFormVM();
            vm.ExerciseSelectList = new SelectList(_context.Exercises, "Id", "Name");
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
                //create a new workout object
                Workout workout = new Workout();

                //map the properties from the workout to the workout object
                //workout.UserId = (get user id based on the current logged in user)
                workout.Name = vm.Name;
                workout.Date = vm.Date;
                _context.Add(workout);
                await _context.SaveChangesAsync();

                //now workoutId exists, loop through the exercises in the view model and create workout exercise objects
                foreach (var exercise in vm.Exercises)
                {
                    WorkoutExercise workoutExercise = new WorkoutExercise();
                    workoutExercise.WorkoutId = workout.Id;
                    workoutExercise.ExerciseId = exercise.ExerciseId;
                    workout.WorkoutExercises.Add(workoutExercise);
                    //add the workout exercise to the context so we can get the id for the sets
                    _context.Add(workoutExercise);
                    await _context.SaveChangesAsync();
                    //loop through the sets for each exercise and create set objects
                    foreach (var set in exercise.Sets)
                    {
                        Set workoutSet = new Set();
                        workoutSet.WorkoutExerciseId = workoutExercise.Id;
                        workoutSet.Repetitions = set.Repetitions;
                        workoutSet.Weight = set.Weight;
                        workoutSet.SetNumber = set.SetNumber;
                        workoutExercise.Sets.Add(workoutSet);
                        //finally, add the set to the context
                        _context.Add(workoutSet);
                        await _context.SaveChangesAsync();
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            WorkoutFormVM vm = new WorkoutFormVM();
            vm.ExerciseSelectList = new SelectList(_context.Exercises, "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Exercise)
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.Sets)
                .FirstOrDefaultAsync(m => m.Id == id);

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
                var workout = await _context.Workouts
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Sets)
                    .FirstOrDefaultAsync(m => m.Id == id);
                try
                {
                    workout.Date = vm.Date;
                    workout.Name = vm.Name;
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
            vm.ExerciseSelectList = new SelectList(_context.Exercises, "Id", "Name");
            return View(vm);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workout = await _context.Workouts
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var workout = await _context.Workouts.FindAsync(id);
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
