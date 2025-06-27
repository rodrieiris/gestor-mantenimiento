using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestorMantenimiento.API.Models
{
    public class MaintenanceTask
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime ScheduledDate { get; set; }

        public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Pending;

        // Relación con Machine
        [ForeignKey("Machine")]
        public int MachineId { get; set; }

        public Machine? Machine { get; set; }
    }
}
