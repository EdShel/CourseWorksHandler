using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CourseWorksHandler.WEB.Models;
using CourseWorksHandler.WEB.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorksHandler.WEB.Controllers
{
    [Authorize(Roles = "Teacher")]
    public class TeachersController : Controller
    {
        private TeacherRepository teachers;

        private GroupsRepository groups;

        public TeachersController(TeacherRepository teachers, GroupsRepository groups)
        {
            this.teachers = teachers;
            this.groups = groups;
        }

        [HttpPost]
        public async Task<IActionResult> PutMark(int studentId, int mark)
        {
            if (mark < 0 || mark > 100)
            {
                return RedirectToAction("ViewWork", "Students", new { id = studentId });
            }

            try
            {
                await teachers.OpenConnectionAsync();
                await teachers.PutMarkToStudent(studentId, mark);

                return RedirectToAction("ViewWork", "Students", new { id = studentId });
            }
            finally
            {
                teachers.CloseConnection();
            }
        }

        [HttpPost]
        public async Task<JsonResult> DiscardFromGroup(int groupId)
        {
            try
            {
                await groups.OpenConnectionAsync();
                var group = await groups.GetAsync(groupId);
                if (group == null)
                {
                    return Json(new { Error = "noGroup" });
                }

                if (group.TeacherId != GetCurrentTeacherId())
                {
                    return Json(new { Error = "notYourGroup" });
                }

                try
                {
                    await groups.RemoveTeacherFormGroup(group.Id);
                    return Json(new { Error = "ok" });
                }
                catch(SqlException ex)
                {
                    if (ex.Number == 50_005)
                    {
                        return Json("noGroup");
                    }
                    throw ex;
                }
            }
            finally
            {
                groups.CloseConnection();
            }
        }

        [HttpPost]
        public async Task<JsonResult> ApplyForGroup(int groupId)
        {
            try
            {
                await groups.OpenConnectionAsync();
                AcademicGroup group = await groups.GetAsync(groupId);
                if (group == null)
                {
                    return Json(new { Error = "noGroup" });
                }
                int teacherId = GetCurrentTeacherId();
                group.TeacherId = teacherId;

                try
                {
                    await groups.UpdateAsync(group);
                    return Json(new { Error = "ok" });
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50_004)
                    {
                        return Json(new { Error = "maxGroups" });
                    }
                    throw ex;
                }
            }
            finally
            {
                groups.CloseConnection();
            }
        }

        private int GetCurrentTeacherId()
        {
            string userIdClaim = User.Claims.First(claim => claim.Type == "id").Value;
            int teacherId = Convert.ToInt32(userIdClaim);
            return teacherId;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}