using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class RegistrationInstructorViewModel
    {
        
        [Required(ErrorMessage = "Enter Code")]
        [StringLength(8)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Enter Major")]
        public string Major { get; set; }

    }
}
