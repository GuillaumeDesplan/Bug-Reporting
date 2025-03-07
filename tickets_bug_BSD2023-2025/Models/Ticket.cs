using System.ComponentModel.DataAnnotations;

namespace tickets_bug_BSD2023_2025.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public statusTicket Status { get; set; } = statusTicket.Low;
        public bool IsResolved { get; set; } = false;
        public string? AssignedTo { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
    public enum statusTicket
    {
        Low,
        Medium,
        High,
        Urgent,
    }
}
