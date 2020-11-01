using System.Threading.Tasks;
using CourseWorksHandler.WEB.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseWorksHandler.WEB.Controllers
{
    public class GroupsController : Controller
    {
        private GroupsRepository groups;

        public GroupsController(GroupsRepository groups)
        {
            this.groups = groups;
        }

        [HttpGet, Authorize]
        public async Task<JsonResult> All()
        {
            try
            {
                await groups.OpenConnectionAsync();
                var groupsInfo = await groups.GetGroupsInfoAsync();
                return Json(groupsInfo);
            }
            finally
            {
                groups.CloseConnection();
            }
        }
    }
}