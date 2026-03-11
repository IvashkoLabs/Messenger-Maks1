using Messenger_Maks.Models;
using Messenger_Maks.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Maks.Services
{
    public class MessageService
    {
        private readonly JsonStorage _storage = new JsonStorage();
        private List<MessageModel> _allMessages;

        public event Action<MessageModel> OnNewMessage;

        public MessageService()
        {

            _allMessages = _storage.LoadMessages();
        }

        public void SendMessage(MessageModel msg)
        {
            if (string.IsNullOrWhiteSpace(msg.Text))
                throw new Exception("Message text cannot be empty!");
            _allMessages.Add(msg);
            _storage.SaveMessages(_allMessages); 

            OnNewMessage?.Invoke(msg);
        }

        public List<MessageModel> GetAllMessages()
        {
            return _allMessages;
        }

        public List<MessageModel> GetHistory(string conversationId)
        {
            return _allMessages.Where(m => m.ConversationId == conversationId).ToList();
        }
    }
}
