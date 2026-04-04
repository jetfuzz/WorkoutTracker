using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Data;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    public class MuscleGroupsController : Controller
    {
        private readonly WorkoutTrackerContext _context;

        public MuscleGroupsController(WorkoutTrackerContext context)
        {
            _context = context;
        }

        // GET: MuscleGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.MuscleGroups.ToListAsync());
        }

        // GET: MuscleGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var muscleGroup = await _context.MuscleGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (muscleGroup == null)
            {
                return NotFound();
            }

            return View(muscleGroup);
        }

        // GET: MuscleGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MuscleGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] MuscleGroup muscleGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(muscleGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(muscleGroup);
        }

        // GET: MuscleGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var muscleGroup = await _context.MuscleGroups.FindAsync(id);
            if (muscleGroup == null)
            {
                return NotFound();
            }
            return View(muscleGroup);
        }

        // POST: MuscleGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] MuscleGroup muscleGroup)
        {
            if (id != muscleGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(muscleGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MuscleGroupExists(muscleGroup.Id))
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
            return View(muscleGroup);
        }

        // GET: MuscleGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var muscleGroup = await _context.MuscleGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (muscleGroup == null)
            {
                return NotFound();
            }

            return View(muscleGroup);
        }

        // POST: MuscleGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var muscleGroup = await _context.MuscleGroups.FindAsync(id);
            if (muscleGroup != null)
            {
                _context.MuscleGroups.Remove(muscleGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MuscleGroupExists(int id)
        {
            return _context.MuscleGroups.Any(e => e.Id == id);
        }
    }
}
