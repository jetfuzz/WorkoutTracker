using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;
using WorkoutTracker.Data;
using WorkoutTracker.Dtos;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class WorkoutsApiController : ControllerBase
{
    private readonly WorkoutTrackerContext _context;
    public WorkoutsApiController(WorkoutTrackerContext context)
    {
        _context = context;
    }

    // GET: api/Workout
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutDto>>> GetWorkout()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var workouts = await _context.Workouts
            .Where(w => w.UserId == userId)
            .Select(w => new WorkoutDto 
            {
                Id = w.Id,
                Date = w.Date,
                Name = w.Name,
                Exercises = w.WorkoutExercises.Select(we => new WorkoutExerciseDto
                {
                    Id = we.Id,
                    ExerciseName = we.Exercise.Name,
                    Sets = we.Sets.Select(s => new SetDto
                    {
                        Id = s.Id,
                        SetNumber = s.SetNumber,
                        Repetitions = s.Repetitions,
                        Weight = s.Weight
                    }).ToList()
                }).ToList() 
            })
            .ToListAsync();
        return workouts;
    }

    // GET: api/Workout/5
    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutDto>> GetWorkout(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var workout = await _context.Workouts
            .Where(w => w.Id == id && w.UserId == userId)
            .Select(w => new WorkoutDto
            {
                Id = w.Id,
                Date = w.Date,
                Name = w.Name,
                Exercises = w.WorkoutExercises.Select(we => new WorkoutExerciseDto
                {
                    Id = we.Id,
                    ExerciseName = we.Exercise.Name,
                    Sets = we.Sets.Select(s => new SetDto
                    {
                        Id = s.Id,
                        SetNumber = s.SetNumber,
                        Repetitions = s.Repetitions,
                        Weight = s.Weight
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (workout == null)
        {
            return NotFound();
        }

        return Ok(workout);
    }

    // PUT: api/Workout/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutWorkout(int id, WorkoutWriteDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var workout = await _context.Workouts
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Sets)
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

        if (workout == null) return NotFound();

        workout.Name = dto.Name;
        workout.Date = dto.Date;

        _context.WorkoutExercises.RemoveRange(workout.WorkoutExercises);

        foreach (var exerciseDto in dto.Exercises)
        {
            var workoutExercise = new WorkoutExercise
            {
                ExerciseId = exerciseDto.ExerciseId,
                Sets = exerciseDto.Sets.Select(s => new Set
                {
                    SetNumber = s.SetNumber,
                    Repetitions = s.Repetitions,
                    Weight = s.Weight
                }).ToList()
            };
            workout.WorkoutExercises.Add(workoutExercise);
        }

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // POST: api/Workout
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Workout>> PostWorkout(WorkoutWriteDto dto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var workout = new Workout
        {
            UserId = userId,
            Name = dto.Name,
            Date = dto.Date,
            WorkoutExercises = dto.Exercises.Select(e => new WorkoutExercise
            {
                ExerciseId = e.ExerciseId,
                Sets = e.Sets.Select(s => new Set
                {
                    SetNumber = s.SetNumber,
                    Repetitions = s.Repetitions,
                    Weight = s.Weight
                }).ToList()
            }).ToList()
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        var createdWorkout = await _context.Workouts
            .Where(w => w.Id == workout.Id && w.UserId == userId)
            .Select(w => new WorkoutDto
            {
                Id = w.Id,
                Date = w.Date,
                Name = w.Name,
                Exercises = w.WorkoutExercises.Select(we => new WorkoutExerciseDto
                {
                    Id = we.Id,
                    ExerciseName = we.Exercise.Name,
                    Sets = we.Sets.Select(s => new SetDto
                    {
                        Id = s.Id,
                        SetNumber = s.SetNumber,
                        Repetitions = s.Repetitions,
                        Weight = s.Weight
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

        return CreatedAtAction("GetWorkout", new { id = createdWorkout.Id }, createdWorkout);
    }

    // DELETE: api/Workout/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkout(int? id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var workout = await _context.Workouts
            .Where(w => w.Id == id && w.UserId == userId)
            .FirstOrDefaultAsync();
        if (workout == null)
        {
            return NotFound();
        }

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
