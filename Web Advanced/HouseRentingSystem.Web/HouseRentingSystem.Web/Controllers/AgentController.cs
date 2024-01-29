using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Web.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        public async Task<IActionResult> Become()
        {
            return View();
        }
    }
}
