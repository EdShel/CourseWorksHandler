using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWorksHandler.WEB.Models;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Teacher teacher)
        {

        }
    }
}