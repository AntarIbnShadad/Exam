using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class RegistrationInstructorViewModel
    {
        [Required(ErrorMessage = "Enter Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Enter Email Address")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Not Match")]
        public string ConfirmPassword { get; set; }
        public string Major { get; set; }
        public string Mobile { get; set; }
    }
}
