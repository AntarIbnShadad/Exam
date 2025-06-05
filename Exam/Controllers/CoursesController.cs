using Exam.Business;
using Exam.Data;
using Exam.Migrations;
using Exam.Models;
using Exam.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CoursesController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, RoleManager<IdentityRole> roleManager, Repository repository, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Courses()
        {
            if (User.IsInRole("Admin")) 
            {
                var courseList = _repository.Get<Course>().Include(c => c.Instructor);
                return View(courseList);
            }
            else if(User.IsInRole("Student"))
            {
                var courseList = _repository.Get<Course>().Include(c => c.Instructor);
                string userId = _userManager.GetUserId(User) ?? "";
                int currentId = _repository.GetBy<Student>(e => e.UserId == userId).Id;

                var enrolledCourseIds = _repository.Get<Enrollment>()
                    .Where(e => e.StudentId == currentId)
                    .Select(e => e.CourseId)
                    .ToList();

                ViewBag.EnrolledCourseIds = enrolledCourseIds;
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

        [HttpPost]
        public async Task<IActionResult> UploadPDF(int CourseId, IFormFile pdfFile)
        {
            if (pdfFile != null && pdfFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "syllabus");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(pdfFile.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(fileStream);
                }
                _repository.Update<Course>(CourseId,c => c.SyllabusFileName = uniqueFileName);

                return RedirectToAction(nameof(Courses));
            }
            ModelState.AddModelError("", "Please select a file.");
            return View();
        }
        [HttpPost]
        public IActionResult DeleteSyllabus(int courseId)
        {
            var course = _repository.Get<Course>().FirstOrDefault(c => c.Id == courseId);
            if (course == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrEmpty(course.SyllabusFileName))
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "syllabus", course.SyllabusFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _repository.Update<Course>(courseId, c => c.SyllabusFileName = null); 

            return RedirectToAction(nameof(Courses)); 
        }
    }
}
