using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class RolesViewModel
    {
        [Required(ErrorMessage = "Enter Role Name")]
        [MinLength(2)]
        public string RoleName { get; set; }
    }
}
