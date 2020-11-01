using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.Repositories;
using CourseWorksHandler.WEB.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseWorksHandler.WEB.Controllers
{
    public class UsersController : Controller
    {
        private AppUserRepository usersRepository;

        public UsersController(AppUserRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await usersRepository.OpenConnectionAsync();
                    AppUser user = await usersRepository.GetByEmailAsync(model.Email);
                    if (user != null && usersRepository.VerifyPassword(user, model.Password))
                    {
                        await Authenticate(user);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Invalid login and/or password");
                }
                finally
                {
                    usersRepository.CloseConnection();
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterTeacher()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterTeacher(RegisterTeacherModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await usersRepository.OpenConnectionAsync();
                    AppUser user = await usersRepository.GetByEmailAsync(model.Email);
                    if (user == null)
                    {
                        await usersRepository.RegisterTeacherAsync(model);
                        user = await usersRepository.GetByEmailAsync(model.Email);
                        await Authenticate(user);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Teacher is already registered");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
                finally
                {
                    usersRepository.CloseConnection();
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterStudent()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterStudent(RegisterStudentModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await usersRepository.OpenConnectionAsync();
                    AppUser user = await usersRepository.GetByEmailAsync(model.Email);
                    if (user == null)
                    {
                        await usersRepository.RegisterStudentAsync(model);
                        user = await usersRepository.GetByEmailAsync(model.Email);
                        await Authenticate(user);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User is already registered");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(model);
                }
                finally
                {
                    usersRepository.CloseConnection();
                }
            }
            return View(model);
        }

        private async Task Authenticate(AppUser user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.RoleName),
                new Claim("id", user.Id.ToString())
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}