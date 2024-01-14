namespace Simple_Chat_ASP.NET_Core_MVC_App.Models.Chat
{
	public class ChatViewModel
	{
        public ChatViewModel()
        {
            AllMessages = new HashSet<MessageViewModel>();
        }

        public MessageViewModel CurrentMessage { get; set; } = null!;

		public ICollection <MessageViewModel> AllMessages { get; set; } = null!;
    }
}
