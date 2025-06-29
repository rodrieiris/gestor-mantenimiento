namespace GestorMantenimiento.API.Models
{
    public class DashboardStats
    {
        public int Total { get; set; }
        public int Pending { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}
