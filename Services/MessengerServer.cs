using Messenger_Maks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger_Maks.Services
{
    class MessengerServer
    {

        public static MessageService Service = new MessageService();

        public static event Action<MessageModel> OnNewMessage
        {
            add { Service.OnNewMessage += value; }
            remove { Service.OnNewMessage -= value; }
        }

        public static List<MessageModel> AllHistory => Service.GetAllMessages();

        public static void Send(MessageModel msg)
        {
            Service.SendMessage(msg);
        }
    }
}
