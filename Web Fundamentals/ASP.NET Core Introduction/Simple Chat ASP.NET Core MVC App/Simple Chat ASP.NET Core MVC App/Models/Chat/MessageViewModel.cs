using System.ComponentModel.DataAnnotations;

namespace Simple_Chat_ASP.NET_Core_MVC_App.Models.Chat
{
	public class MessageViewModel
	{
		[Required]
		public string Sender { get; set; } = null!;

		[Required]
		[MaxLength(255)]
		public string MessageText { get; set; } = null!;
    }
}
