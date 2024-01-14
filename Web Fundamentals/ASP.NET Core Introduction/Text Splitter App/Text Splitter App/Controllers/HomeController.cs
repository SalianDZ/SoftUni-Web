using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Text_Splitter_App.Models;
using Text_Splitter_App.Models.Text;

namespace Text_Splitter_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(TextViewModel viewModel)
        {
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Split(TextViewModel viewModel) 
        {
            if (String.IsNullOrWhiteSpace(viewModel.TextToSplit)) 
            {
                return RedirectToAction("Index", new TextViewModel()
                { 
                    SplitText = String.Empty,
                    TextToSplit = String.Empty
                });
            }

            string[] words = viewModel.TextToSplit.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            string splitText = String.Join(Environment.NewLine, words);

            viewModel.SplitText = splitText;
            return RedirectToAction("Index", viewModel);
        }
    }
}