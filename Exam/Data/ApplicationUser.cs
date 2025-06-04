using Exam.Models;
using Microsoft.AspNetCore.Identity;

namespace Exam.Data
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Instructor? Instructor { get; set; }
        public virtual Student? Student { get; set; }
    }

}
