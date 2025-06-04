using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Enter Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RemeberMe { get; set; }
    }
}
