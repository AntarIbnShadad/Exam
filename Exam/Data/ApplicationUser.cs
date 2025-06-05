using Exam.Models;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace Exam.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual Instructor? Instructor { get; set; }
        public virtual Student? Student { get; set; }
    }

}
