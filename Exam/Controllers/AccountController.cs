using Exam.Data;
using Exam.Models;
using Exam.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signinManager;
        private RoleManager<IdentityRole> _roleManager;
        private AppDbContext _db;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, RoleManager<IdentityRole> roleManager, AppDbContext db)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _db = db;
        }
        public IActionResult Account()
        {
            return View();
        }
        #region Registration
        public IActionResult StudentRegister()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> StudentRegister(RegistrationStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Username,
                    PhoneNumber = model.Mobile,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Student student = new Student
                    {
                        UserId = user.Id,
                        Name = $"{model.FirstName} {model.LastName}",
                        Email = model.Email,
                        Phone = model.Mobile,
                        City = model.City
                    };
                    _db.Students.Add(student);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }

            return View(model);
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            if (_signinManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Username,
                };
                var result = await _signinManager.PasswordSignInAsync(user.UserName, model.Password, model.RemeberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return View(model);
                }

            }
            return View(model);


        }
        public async Task<IActionResult> SignOut()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
        #region Roles
        public IActionResult Roles()
        {
            IEnumerable <IdentityRole> roles= _roleManager.Roles;
            return View(roles);
        }

        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Roles));
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View(model);
        }
        //public async Task<IActionResult> EditRole(string id)
        //{
        //    if (id == null || id == "")
        //    {
        //        return RedirectToAction(nameof(Roles));
        //    }
        //    IdentityRole role = await _roleManager.FindByIdAsync(id);
        //    if (role == null)
        //    {
        //        return NotFound();
        //    }
        //    EditRoleViewModel toEdit = new EditRoleViewModel
        //    {
        //        Id = role.Id,
        //        RoleName = role.Name
        //    };
        //    return View(toEdit);
        //}
        //[HttpPost]
        //public async Task<IActionResult> EditRole(EditRoleViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var role = await _roleManager.FindByIdAsync(model.Id);
        //        if (role == null)
        //        {
        //            return NotFound();
        //        }
        //        role.Name = model.RoleName;

        //        var result = await _roleManager.UpdateAsync(role);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction(nameof(RolesList));
        //        }

        //        foreach (var err in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, err.Description);
        //        }
        //    }

        //    return View(model);
        //}
        #endregion
    }
}
