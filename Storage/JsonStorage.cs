using Messenger_Maks.Models;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Xml;
using Newtonsoft.Json;
using System.IO;
namespace Messenger_Maks.Storage
{
    class JsonStorage
    {
        private readonly string _filePath = "messages_db.json";
        private const string UserFile = "users_db.json";
        public void SaveMessages(List<MessageModel> messages)
        {
            string json = JsonConvert.SerializeObject(messages, Formatting.Indented);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Json file saved path: " + Path.GetFullPath(_filePath));
        }

        public List<MessageModel> LoadMessages()
        {
            if (!File.Exists(_filePath)) return new List<MessageModel>();
            string json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<MessageModel>>(json);
        }
        public void SaveUsers(List<string> users) =>
            File.WriteAllText(UserFile, JsonConvert.SerializeObject(users));

        public List<string> LoadUsers() =>
            File.Exists(UserFile) ? JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(UserFile)) : new List<string> { "Admin" };
    }
}
