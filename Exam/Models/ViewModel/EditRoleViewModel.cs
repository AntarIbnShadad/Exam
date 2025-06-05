using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
