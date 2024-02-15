using Homies.Extensions;
using Homies.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Homies.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
			string userId = User.GetUserId();
			if (userId == null)
			{
				return View();
			}

			return RedirectToAction("All", "Event");
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}