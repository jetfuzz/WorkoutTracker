using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Data;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WorkoutTrackerContext _context;

        public HomeController(ILogger<HomeController> logger, WorkoutTrackerContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var workouts = new List<Workout>();

            if (!string.IsNullOrEmpty(userId))
            {
                workouts = await _context.Workouts
                    .Where(w => w.UserId == userId)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Sets)
                    .OrderBy(w => w.Date)
                    .ToListAsync();
            }


            ViewBag.RepLabels = workouts
                .Select(w => w.Date.ToString("MMM dd"))
                .ToList();

            ViewBag.RepData = workouts
                .Select(w => w.WorkoutExercises
                    .SelectMany(we => we.Sets)
                    .Sum(s => s.Repetitions))
                .ToList();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}