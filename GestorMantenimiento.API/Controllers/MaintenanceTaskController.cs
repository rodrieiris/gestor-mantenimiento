using Microsoft.AspNetCore.Mvc;
using GestorMantenimiento.API.Models;
using GestorMantenimiento.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GestorMantenimiento.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceTaskController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MaintenanceTaskController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/MaintenanceTask
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetAllTasks()
        {
            return await _context.MaintenanceTasks.Include(t => t.Machine).ToListAsync();
        }

        // GET: api/MaintenanceTask/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceTask>> GetTaskById(int id)
        {
            var task = await _context.MaintenanceTasks.Include(t => t.Machine).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();
            return task;
        }

        // GET: api/MaintenanceTask/by-machine/3
        [HttpGet("by-machine/{machineId}")]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetTasksByMachine(int machineId)
        {
            var tasks = await _context.MaintenanceTasks
                .Where(t => t.MachineId == machineId)
                .Include(t => t.Machine)
                .ToListAsync();
            return tasks;
        }

        // POST: api/MaintenanceTask
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MaintenanceTask>> CreateTask(MaintenanceTask task)
        {
            // Validar si la máquina existe
            var machine = await _context.Machines.FindAsync(task.MachineId);
            if (machine == null)
                return BadRequest($"No existe una máquina con Id {task.MachineId}");

            _context.MaintenanceTasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // PUT: api/MaintenanceTask/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTask(int id, MaintenanceTask task)
        {
            if (id != task.Id) return BadRequest();

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.MaintenanceTasks.Any(t => t.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/MaintenanceTask/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.MaintenanceTasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.MaintenanceTasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/maintenancetask/{id}/complete
        [HttpPatch("{id}/complete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarkTaskAsCompleted(int id)
        {
            var task = await _context.MaintenanceTasks.FindAsync(id);

            if (task == null)
                return NotFound();

            if (task.IsCompleted)
                return BadRequest("La tarea ya está marcada como completada.");

            task.IsCompleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/maintenancetask/by-status/Completed
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<MaintenanceTask>>> GetTasksByStatus(MaintenanceStatus status)
        {
            var tasks = await _context.MaintenanceTasks
                                    .Where(t => t.Status == status)
                                    .Include(t => t.Machine)
                                    .ToListAsync();

            return Ok(tasks);
        }

        // GET: api/maintenancetask/stats
        [HttpGet("stats")]
        public async Task<ActionResult<DashboardStats>> GetStats()
        {
            var stats = new DashboardStats
            {
                Total = await _context.MaintenanceTasks.CountAsync(),
                Pending = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.Pending),
                InProgress = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.InProgress),
                Completed = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.Completed),
                Cancelled = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.Cancelled),
            };
            return Ok(stats);
        }


        // GET: api/maintenancetask/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboard()
        {
            var total = await _context.MaintenanceTasks.CountAsync();
            var completed = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.Completed);
            var pending = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.Pending);
            var inProgress = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.InProgress);
            var cancelled = await _context.MaintenanceTasks.CountAsync(t => t.Status == MaintenanceStatus.Cancelled);

            var nextScheduled = await _context.MaintenanceTasks
                .Where(t => t.ScheduledDate > DateTime.Now && t.Status != MaintenanceStatus.Completed)
                .OrderBy(t => t.ScheduledDate)
                .Select(t => new
                {
                    t.Id,
                    t.Description,
                    t.ScheduledDate,
                    MachineName = t.Machine != null ? t.Machine.Name : null
                })
                .FirstOrDefaultAsync();

            return Ok(new
            {
                Total = total,
                Completed = completed,
                Pending = pending,
                InProgress = inProgress,
                Cancelled = cancelled,
                NextTask = nextScheduled
            });
        }
    }
}