using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using Newtonsoft.Json;
using Messenger_Maks.Models;
using Messenger_Maks.Services;
using System.Windows.Forms;
namespace Messenger_Maks.API
{
    class SimpleHttpServer
    {
        private HttpListener _listener;
        private MessageService _service = MessengerServer.Service;

        public void Start()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:5000/"); // listen to port 5000 
            //_listener.Start();
            Task.Run(() => Listen());
        }

        private async void Listen()
        {
            while (_listener.IsListening)
            {
                var context = await _listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;

                if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/messages")
                {
                    using (var reader = new System.IO.StreamReader(request.InputStream))
                    {
                        var json = await reader.ReadToEndAsync();
                        System.Console.WriteLine("Postman send: " + json);
                        MessageBox.Show("Postman send: " + json, "Postman", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var msg = JsonConvert.DeserializeObject<MessageModel>(json);
                        _service.SendMessage(msg);
                    }
                   // byte[] buffer = "{\"status\":\"sent\"}";
                    byte[] buffer = Encoding.UTF8.GetBytes("{\"status\":\"sent\"}");
                    response.OutputStream.Write(buffer, 0, buffer.Length);

                }
                else if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/users")
                {
                    using (var reader = new System.IO.StreamReader(request.InputStream))
                    {
                        var json = await reader.ReadToEndAsync();
                        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                        if (data != null && data.ContainsKey("Username"))
                        {
                            try
                            {
                                MessengerServer.Service.CreateUser(data["Username"]);

                                byte[] buffer = Encoding.UTF8.GetBytes("{\"status\":\"user created\"}");
                                response.ContentType = "application/json";
                                response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                            catch (Exception ex)
                            {
                                byte[] buffer = Encoding.UTF8.GetBytes($"{{\"error\":\"{ex.Message}\"}}");
                                response.StatusCode = 400;
                                response.ContentType = "application/json";
                                response.OutputStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                    }
                }
                else if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/history")
                {
                    var history = _service.GetAllMessages(); 
                    string json = JsonConvert.SerializeObject(history);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);

                    response.ContentType = "application/json";
                    response.OutputStream.Write(buffer, 0, buffer.Length);
                }

                response.Close();
            }
        }
        public void Stop()
        {
            if (_listener != null && _listener.IsListening)
            {
                _listener.Stop();
                _listener.Close();
            }
        }
    }
}
/*
{
  "SenderId": "Postman",
  "ConversationId": "General",
  "Text": "this messege was sent by API postman"
}

{
  "Username": "гіук тфьу"
}
*/