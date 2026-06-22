using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ChatBotWPF.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserMessage { get; set; }
        public string BotMessage { get; set; }
        public DateTime Timestamp { get; set; }
    }
}