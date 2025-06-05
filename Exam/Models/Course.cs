using Microsoft.AspNetCore.Routing.Constraints;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public virtual Instructor? Instructor { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }
        public string? SyllabusFileName { get; set; }
    }
}
