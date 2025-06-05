using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models.ViewModel
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
