using Microsoft.AspNetCore.Mvc;
using GestorMantenimiento.API.Models;
using GestorMantenimiento.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GestorMantenimiento.API.Controllers
{
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
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.MaintenanceTasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.MaintenanceTasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}