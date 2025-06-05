using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course? Course { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student? Student { get; set; }
        public bool isActive { get; set; }
    }
}
