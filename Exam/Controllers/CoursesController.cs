using Exam.Business;
using Exam.Data;
using Exam.Migrations;
using Exam.Models;
using Exam.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;

namespace Exam.Controllers
{
    public class CoursesController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signinManager;
        private RoleManager<IdentityRole> _roleManager;
        private Repository _repository ;


        public CoursesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, RoleManager<IdentityRole> roleManager, Repository repository)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _repository = repository;
        }
        public async Task<IActionResult> Courses()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Student")) 
            {
                var courseList = _repository.Get<Course>().Include(c => c.Instructor);
                return View(courseList);
            }
            else
            {
                string userId = _userManager.GetUserId(User) ?? "";
                var courseList = _repository.Get<Course>().Include(c => c.Instructor)
                    .Where(c => c.Instructor.UserId == userId)
                    .Select(record => record);
                return View(courseList);
            }

        }
        public IActionResult CreateCourse()
        {
            var instructors = _repository.Get<Instructor>();
            ViewBag.Instructors = new SelectList(instructors, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreateCourse(CreateCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Course course = new Course
                {
                    Title = model.Title,
                    Description = model.Description,
                    InstructorId = model.InstructorId,
                    Capacity = model.Capacity,
                    Price = model.Price,
                    Duration = model.Duration
                };

                _repository.Add<Course>(course);
            }

            return RedirectToAction(nameof(Courses));
        }
        public IActionResult Schedule()
        {
            string userId = _userManager.GetUserId(User) ?? "";
            var student = _repository.GetBy<Student>(s => s.UserId == userId);

            var schedule = _repository.Get<Enrollment>()
                .Where(e => e.StudentId == student.Id)
                .Include(e => e.Course)
                .ThenInclude(c => c.Instructor)
                .Select(e => new StudentScheduleViewModel
                {
                    CourseTitle = e.Course.Title,
                    CourseDescription = e.Course.Description,
                    InstructorName = e.Course.Instructor != null ? e.Course.Instructor.Name : "Unknown",
                    Duration = e.Course.Duration,
                    IsActive = e.isActive
                })
                .ToList();
            return View(schedule);
        }
        public IActionResult Enroll(int Id)
        {
            string userId = _userManager.GetUserId(User) ?? "";
            _repository.Add<Enrollment>(new Enrollment
            {
                CourseId = Id,
                StudentId = _repository.GetBy<Student>(record => record.UserId == userId).Id,
                isActive = false
            });
            return RedirectToAction(nameof(Schedule));
        }
        public IActionResult EnrollmentRequests()
        {
            var nonActive = _repository.Get<Enrollment>()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ThenInclude(e => e.Instructor)
                .Where(record => record.isActive == false)
                .Select(e => new ApproveEnrollmentViewModel
                {
                    Id = e.Id,
                    CourseTitle = e.Course.Title,
                    CourseDescription = e.Course.Description,
                    InstructorName = e.Course.Instructor != null ? e.Course.Instructor.Name : "Unknown",
                    StrudentName = e.Student != null ? e.Student.Name : "Unknown",
                    Duration = e.Course.Duration
                });
            return View(nonActive);
        }
        public IActionResult EnrollmentApproval(int Id)
        {
            _repository.Update<Enrollment>(Id, c => c.isActive = true);
            return RedirectToAction(nameof(EnrollmentRequests));
        }
    }
}
