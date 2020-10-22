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
        private SqlConnection db;

        private StudentRepository students;

        public StudentsController(SqlConnection db)
        {
            this.db = db;
            this.students = new StudentRepository(db);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetGeneralInfoTable(int pageIndex)
        {
            await db.OpenAsync();
            const int pageSize = 20;
            var rows = await students.GetStudentsGeneralInfoPaginated(pageIndex, pageSize);
            db.Close();
            return Json(rows);
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