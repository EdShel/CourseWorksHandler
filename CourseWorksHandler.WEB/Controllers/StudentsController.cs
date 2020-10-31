using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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

        [HttpGet, Authorize(Roles ="Student")]
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
                    Theme = courseWork.Theme,
                    Task = courseWork.Task
                });
            }
            finally
            {
                courseWorks.CloseConnection();
            }
        }

        [HttpGet, Authorize(Roles = "Student")]
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

    public class StudentsController : Controller
    {
        private const int GENERAL_INFO_PAGE_SIZE = 5;

        private StudentRepository students;

        public StudentsController(StudentRepository studentRepository)
        {
            this.students = studentRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetGeneralInfoTable(int pageIndex)
        {
            await students.OpenConnectionAsync();
            int pagesCount = await students.GetStudentsCount() / GENERAL_INFO_PAGE_SIZE;
            int pageNumber = Math.Max(0, Math.Min(pagesCount, pageIndex));
            IEnumerable<StudentsGeneralInfo> rows = await students.GetStudentsGeneralInfoPaginated(pageNumber, GENERAL_INFO_PAGE_SIZE);
            students.CloseConnection();
            return Json(new
            {
                rows = rows,
                totalPages = pagesCount
            });
        }
    }

    public class TeachersController : Controller
    {
        private SqlConnection db;

        private TeacherRepository teachers;

        public TeachersController(SqlConnection db)
        {
            this.db = db;
            teachers = new TeacherRepository(db);
        }

        public async Task<IActionResult> Index()
        {
            await db.OpenAsync();
            var allTeachers = await teachers.GetAllAsync();
            db.Close();
            return View(allTeachers);
        }
    }
}