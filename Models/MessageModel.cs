using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Maks.Models
{
    public class MessageModel
    {
        public Guid MessageId { get; set; } = Guid.NewGuid(); // phenomenal ID
        public string ConversationId { get; set; } // "General" / "User1-User2"
        public string SenderId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Dictionary<string, bool> DeliveryStatus { get; set; } = new Dictionary<string, bool>();
        public MessageModel(string senderId, string conversationId, string text)
        {
            this.SenderId = senderId;
            this.ConversationId = conversationId;
            this.Text = text;
        }
        public MessageModel() { }// only for JSON

    }
}
