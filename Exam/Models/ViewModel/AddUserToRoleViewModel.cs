using System.ComponentModel.DataAnnotations;

namespace Exam.Models.ViewModel
{
    public class AddUserToRoleViewModel
    {
        public string RoleName { get; set; }

        [Required]
        [Display(Name = "Select User")]
        public string SelectedUserId { get; set; }
    }
}
