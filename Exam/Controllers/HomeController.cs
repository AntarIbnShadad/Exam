using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Exam.Data;

namespace Exam.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private UserManager<ApplicationUser> _userManager;
    private AppDbContext _db;

    public HomeController(ILogger<HomeController> logger, AppDbContext db, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        string id = _userManager.GetUserId(User) ?? "";
        if (id!="")
        {
            bool inStudent = _db.Students.Where(s => s.UserId == id).Count()!=0? true:false;
            bool inInstructor = _db.Instructors.Where(s => s.UserId == id).Count() != 0 ? true : false;
            if ( inStudent==false && inInstructor ==false)
            {
                ViewBag.EnableAccount = true;
            }
            else
            {
                ViewBag.EnableAccount = false;
            }
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
