using System.Text.Json.Serialization;

namespace WAD13400.DAL.Models
{
    public class ProjectItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
