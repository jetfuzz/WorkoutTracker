using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkoutTracker.Data;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    [Authorize]
    public class ExercisesController : Controller
    {
        private readonly WorkoutTrackerContext _context;

        public ExercisesController(WorkoutTrackerContext context)
        {
            _context = context;
        }

        // GET: Exercises
        public async Task<IActionResult> Index()
        {
            ExerciseIndexVM vm = new ExerciseIndexVM();
            vm.Exercises = await _context.Exercises.Include(e => e.MuscleGroup).ToListAsync();
            vm.MuscleGroups = await _context.MuscleGroups.ToListAsync();
            return View(vm);
        }

        // GET: Exercises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ExerciseDetailsVM vm = new ExerciseDetailsVM();

            vm.Exercise = await _context.Exercises
                .Include(e => e.MuscleGroup)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (vm.Exercise == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            vm.WorkoutData = await _context.WorkoutExercises
                .Where(we => we.ExerciseId == id)
                .Include(we => we.Workout)
                .Where(we => we.Workout.UserId == userId) 
                .Select(we => new WorkoutVM
                {
                    Date = we.Workout.Date,
                    HighestWeight = we.Sets.Max(s => s.Weight)
                })
                .OrderBy(w => w.Date)
                .ToListAsync();

            return View(vm);
        }

        // GET: Exercises/Create
        public IActionResult Create()
        {
            ViewData["MuscleGroupId"] = new SelectList(_context.MuscleGroups, "Id", "Id");
            return View();
        }

        // POST: Exercises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MuscleGroupId,Name")] Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exercise);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MuscleGroupId"] = new SelectList(_context.MuscleGroups, "Id", "Id", exercise.MuscleGroupId);
            return View(exercise);
        }

        // GET: Exercises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
            {
                return NotFound();
            }
            ViewData["MuscleGroupId"] = new SelectList(_context.MuscleGroups, "Id", "Id", exercise.MuscleGroupId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MuscleGroupId,Name")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
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
            ViewData["MuscleGroupId"] = new SelectList(_context.MuscleGroups, "Id", "Id", exercise.MuscleGroupId);
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = await _context.Exercises
                .Include(e => e.MuscleGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
