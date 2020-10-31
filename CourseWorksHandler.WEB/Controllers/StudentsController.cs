﻿using System;
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