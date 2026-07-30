using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Models;
using WorkoutTracker.Data;
using WorkoutTracker.Dtos;

[Route("api/[controller]")]
[ApiController]
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
        var workouts = await _context.Workouts
            .Select(w => new WorkoutDto 
            {
                Id = w.Id,
                //UserId = w.UserId,
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
        var workout = await _context.Workouts
            .Where(w => w.Id == id)
            .Select(w => new WorkoutDto
            {
                Id = w.Id,
                //UserId = w.UserId,
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
    public async Task<IActionResult> PutWorkout(int? id, WorkoutWriteDto dto)
    {
        var workout = await _context.Workouts
            .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Sets)
            .FirstOrDefaultAsync(w => w.Id == id);

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
    public async Task<ActionResult<Workout>> PostWorkout(Workout workout)
    {
        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetWorkout", new { id = workout.Id }, workout);
    }

    // DELETE: api/Workout/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWorkout(int? id)
    {
        var workout = await _context.Workouts.FindAsync(id);
        if (workout == null)
        {
            return NotFound();
        }

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WorkoutExists(int? id)
    {
        return _context.Workouts.Any(e => e.Id == id);
    }
}
