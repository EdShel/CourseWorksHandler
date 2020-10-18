using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorksHandler.WEB.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
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