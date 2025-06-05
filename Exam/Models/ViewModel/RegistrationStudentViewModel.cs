using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class RegistrationStudentViewModel
    {
        [Required(ErrorMessage = "Enter City")]
        public string City { get; set; }

    }
}
