using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class CreateCourseViewModel
    {
        [Required(ErrorMessage = "Enter Title")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Select Instructor")]
        public int InstructorId { get; set; }
        [Required(ErrorMessage = "Enter Capacity")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Enter Price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Enter Duration")]
        public TimeSpan Duration { get; set; }
    }
}
