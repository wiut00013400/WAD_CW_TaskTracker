using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using WAD13400.DAL.Data;

namespace WAD13400.DAL.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public required string ContributionComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        [JsonIgnore] // done to avoid circular serialization error
        public ProjectItem? Project { get; set; }
    }
}
