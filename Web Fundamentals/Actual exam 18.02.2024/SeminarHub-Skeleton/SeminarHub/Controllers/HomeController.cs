﻿using Microsoft.AspNetCore.Mvc;
using SeminarHub.Extensions;
using SeminarHub.Models;
using System.Diagnostics;

namespace SeminarHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string userId = User.GetUserId();
            if (userId == null)
            {
                return View();
            }

            return RedirectToAction("All", "Seminar");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}