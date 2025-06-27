using Microsoft.EntityFrameworkCore;
using GestorMantenimiento.API.Models;

namespace GestorMantenimiento.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Machine> Machines { get; set; }

        // Tabla de tareas de mantenimiento relacionadas con las máquinas
        public DbSet<MaintenanceTask> MaintenanceTasks { get; set; }
    }
}
