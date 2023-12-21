using Microsoft.AspNetCore.Mvc;
using Simple_Chat_ASP.NET_Core_MVC_App.Models.Chat;
using System.Reflection.Metadata.Ecma335;

namespace Simple_Chat_ASP.NET_Core_MVC_App.Controllers
{
	public class ChatController : Controller
	{
		private static readonly IList<KeyValuePair<string, string>> messages
			= new List<KeyValuePair<string, string>>();

		public ActionResult Show()
		{
			if (messages.Count < 1)
			{
				return View(new ChatViewModel());
			}

			var chatViewModel = new ChatViewModel()
			{
				AllMessages = messages
				.Select(m => new MessageViewModel()
				{
					Sender = m.Key,
					MessageText = m.Value
				})
				.ToArray()
			};

			return View(chatViewModel);
		}

		[HttpPost]
		public IActionResult Send(ChatViewModel chatViewModel)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Show");
			}

			KeyValuePair<string, string> currentMessage = new(chatViewModel.CurrentMessage.Sender, chatViewModel.CurrentMessage.MessageText);
			messages.Add(currentMessage);
			return RedirectToAction("Show");
		}
	}
}
