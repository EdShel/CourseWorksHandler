using System.Threading.Tasks;
using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.Repositories;
using CourseWorksHandler.WEB.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorksHandler.WEB.Controllers
{

    public class CourseWorkController : Controller
    {
        private AppUserRepository users;

        private CourseWorkRepository courseWorks;

        public CourseWorkController(AppUserRepository users, CourseWorkRepository courseWorks)
        {
            this.users = users;
            this.courseWorks = courseWorks;
        }

        [HttpGet, Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitCourseWork()
        {
            AppUser currentStudent = null;
            try
            {
                await users.OpenConnectionAsync();
                currentStudent = await users.GetByEmailAsync(User.Identity.Name);
            }
            finally
            {
                users.CloseConnection();
            }

            try
            {
                await courseWorks.OpenConnectionAsync();
                CourseWork courseWork = await courseWorks.GetAsync(currentStudent.Id);
                return View(new CourseWorkSubmissionModel
                {
                    StudentId = currentStudent.Id,
                    Theme = courseWork?.Theme ?? string.Empty,
                    Task = courseWork?.Task ?? string.Empty
                });
            }
            finally
            {
                courseWorks.CloseConnection();
            }
        }

        [HttpPost, Authorize(Roles = "Student")]
        public async Task<IActionResult> SubmitCourseWork(CourseWorkSubmissionModel model)
        {
            try
            {
                await courseWorks.OpenConnectionAsync();
                await courseWorks.SubmitCourseWork(model);
            }
            finally
            {
                courseWorks.CloseConnection();
            }
            return View(model);
        }

    }
}