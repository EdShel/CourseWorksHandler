using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.Repositories;
using CourseWorksHandler.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> ViewWork(int id)
        {
            try
            {
                await students.OpenConnectionAsync();
                StudentInfo info = await students.GetStudentInfo(id);
                if (info == null)
                {
                    return NotFound();
                }

                return View(info);
            }
            finally
            {
                students.CloseConnection();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetGeneralInfoTable(int pageIndex, string search = null)
        {
            await students.OpenConnectionAsync();
            int pagesCount = await students.GetStudentsCount() / GENERAL_INFO_PAGE_SIZE;
            int pageNumber = Math.Max(0, Math.Min(pagesCount, pageIndex));
            IEnumerable<StudentsGeneralInfo> rows = await students.GetStudentsGeneralInfoPaginated(pageNumber, GENERAL_INFO_PAGE_SIZE, search);
            students.CloseConnection();
            return Json(new
            {
                rows = rows,
                totalPages = pagesCount
            });
        }

        [HttpGet]
        public async Task<JsonResult> CourseWorkChanges(int id)
        {
            try
            {
                await students.OpenConnectionAsync();
                IEnumerable<CourseWorkHistoryEntry> history = await students.GetCourseWorkHistory(id);
                return Json(history);
            }
            finally
            {
                students.CloseConnection();
            }
        }
    }
}