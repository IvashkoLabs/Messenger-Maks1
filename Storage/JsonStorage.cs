/*
 using Messenger_Maks.Models;
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Xml;
using Newtonsoft.Json;
using System.IO;
namespace Messenger_Maks.Storage {

    class JsonStorage
    {

        private readonly string _filePath = "messages_db.json"
        private const string UserFile = "users_db.json"
        public void SaveMessages(List<MessageModel> messages) 


            string json = JsonConvert.SerializeObject(messages, Formatting.Indented);
            File.WriteAllText(_filePath, json);
            Console.WriteLine("Json file saved path: " + Path.GetFullPath(_filePath));

        

        public List<MessageModel> LoadMessages() 

            if (!File.Exists(_filePath)) return new List<MessageModel>()
            string json = File.ReadAllText(_filePath)
            return JsonConvert.DeserializeObject<List<MessageModel>>(json)
        }
        public void SaveUsers(List<string> users) =>
            File.WriteAllText(UserFile, JsonConvert.SerializeObject(users))

        public List<string> LoadUsers() =>
            File.Exists(UserFile) ? JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(UserFile)) : new List<string> { "Admin" }
    }
}*/
using System.Security.Cryptography;
using System.Text;

import json
import os

class JsonStorage :
    _filePath = "messages_db.json"
    UserFile = "users_db.json"

    def save_messages(self, messages):
        json_str = json.dumps(messages, indent = 4)
        with open(self._filePath, 'w', encoding= 'utf-8') as f:
            f.write(json_str)
        print("Json file saved path: " + os.path.abspath(self._filePath))

    def load_messages(self):
        if not os.path.exists(self._filePath):
            return []
        with open(self._filePath, 'r', encoding= 'utf-8') as f:
            json_str = f.read()
        return json.loads(json_str)

    def save_users(self, users):
        with open(self.UserFile, 'w', encoding= 'utf-8') as f:
            f.write(json.dumps(users))

    def load_users(self):
        if os.path.exists(self.UserFile):
            with open(self.UserFile, 'r', encoding= 'utf-8') as f:
                return json.loads(f.read())
        else:
            return ["Admin"]*/

