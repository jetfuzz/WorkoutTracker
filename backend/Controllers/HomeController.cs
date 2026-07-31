using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
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
            var recentWorkout = new List<Workout>();

            if (!string.IsNullOrEmpty(userId))
            {
                //workout data for chart. filters workouts within the last 3 months
                workouts = await _context.Workouts
                    .Where(w => w.UserId == userId &&
                                w.Date >= DateTime.Now.AddMonths(-3) &&
                                w.WorkoutExercises
                                    .SelectMany(we => we.Sets)
                                    .Sum(s => s.Repetitions) > 0)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Sets)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
                    .OrderBy(w => w.Date)
                    .ToListAsync();

                //data for recent workout div
                recentWorkout = await _context.Workouts
                    .Where(w => w.UserId == userId)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Sets)
                    .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
                    .OrderByDescending(w => w.Date)
                    .Take(1)
                    .ToListAsync();
            }

            //workout sum of reps for each week
            ViewBag.RepLabels = workouts
                .GroupBy(w => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(w.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => g.First().Date.ToString("MMM dd"))
                .ToList();

            ViewBag.RepData = workouts
                .GroupBy(w => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(w.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => g.SelectMany(w => w.WorkoutExercises)
                    .SelectMany(we => we.Sets)
                    .Sum(s => s.Repetitions))
                .ToList();

            return View(recentWorkout);
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