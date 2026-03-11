using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using Newtonsoft.Json;
using Messenger_Maks.Models;
using Messenger_Maks.Services;
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
            _listener.Start();
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
                        var msg = JsonConvert.DeserializeObject<MessageModel>(json);
                        _service.SendMessage(msg);
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes("{\"status\":\"sent\"}");
                    response.OutputStream.Write(buffer, 0, buffer.Length);
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
    }
}
