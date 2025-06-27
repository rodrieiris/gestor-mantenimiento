namespace GestorMantenimiento.API.Models
{
    public class Machine
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public DateTime InstallationDate { get; set; }
        public bool IsActive { get; set; } = true;


        // Lista de tareas de mantenimiento asociadas a esta máquina (relación 1:N).
        public ICollection<MaintenanceTask>? MaintenanceTasks { get; set; }
    }
}