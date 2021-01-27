using System;
using System.Collections.Generic;
using System.Text;

namespace chat_server.Entity
{
	public class MessageVM
	{
		public long ChatId { get; set; }
		public Guid SenderId { get; set; }
		public Guid ReceiverId { get; set; }
		public string Message { get; set; }
		public DateTime Date { get; set; }
		public string Time{ get; set; }
		public bool IsDeleteFromReceiver { get; set; }
		public bool IsDeleteFromSender { get; set; }
	}
}
