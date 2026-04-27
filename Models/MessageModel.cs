using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
    /*
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
   */

import uuid
from datetime import datetime

class MessageModel :
    def __init__(self, sender_id=None, conversation_id=None, text=None):
        self.MessageId = uuid.uuid4()  # phenomenal ID
        self.ConversationId = conversation_id  # "General" / "User1-User2"
        self.SenderId = sender_id
        self.Text = text
        self.CreatedAt = datetime.now()
        self.DeliveryStatus = { }
*/