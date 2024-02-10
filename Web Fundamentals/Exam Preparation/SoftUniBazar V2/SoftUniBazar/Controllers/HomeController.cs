using Microsoft.AspNetCore.Mvc;
using SoftUniBazar.Extensions;
using SoftUniBazar.Models;
using System.Diagnostics;

namespace SoftUniBazar.Controllers
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

            return RedirectToAction("All", "Ad");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}