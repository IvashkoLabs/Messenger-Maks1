using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Messenger_Maks.Models; 
using Messenger_Maks.Services; 
namespace Messenger_Maks
{
    public partial class ChatForm : Form
    {
        private string _currentUser;

        public ChatForm(string userName)
        {
            InitializeComponent();
            _currentUser = userName;
            this.Text = "Chat: " + _currentUser;

            var allUsers = MessengerServer.Service.GetAllUsers();

            if (_currentUser == "Admin")
            {
                foreach (var user in allUsers)
                {
                    if (user != "Admin")
                        listBox1.Items.Add(user + " notes");
                }

                for (int i = 0; i < allUsers.Count; i++)
                {
                    Random rnd = new Random();
                    int a= rnd.Next(-5,5);
                    //a=1;
                    for (int j = i + a; j < allUsers.Count; j++)
                    {

                        if (allUsers[i] != "Admin" && allUsers[j] != "Admin")
                            listBox1.Items.Add($"{allUsers[i]}-{allUsers[j]}");//chat between all users
                        
                    }
                }
            }
            else
            {
                foreach (var user in allUsers)
                {
                    if(user != "Admin")
                    listBox1.Items.Add(user);
                }
            }
            listBox1.Items.Add("General Chat");
            MessengerServer.OnNewMessage += MessengerServer_OnNewMessage;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e) {MessengerServer.OnNewMessage -= MessengerServer_OnNewMessage;}

        private void MessengerServer_OnNewMessage(MessageModel msg)
        {
            if (listBox1.SelectedItem == null) return;
            string selected = listBox1.SelectedItem.ToString();
            bool shouldShow = false;

            if (selected == "General Chat" && msg.ConversationId == "General")
            {
                shouldShow = true;
                msg.DeliveryStatus[_currentUser] = true; 
            }

            else if (_currentUser == "Admin")
            {
                if (selected.Contains("notes"))
                {
                    string targetUser = selected.Replace(" notes", "");
                    shouldShow = (msg.SenderId == targetUser && msg.ConversationId == targetUser);
                }
                else if (selected.Contains("-"))
                {
                    string[] users = selected.Split('-');
                    shouldShow = (msg.SenderId == users[0] && msg.ConversationId == users[1]) || (msg.SenderId == users[1] && msg.ConversationId == users[0]);
                }
            }

            else
                shouldShow = (msg.SenderId == selected && msg.ConversationId == _currentUser) || (msg.SenderId == _currentUser && msg.ConversationId == selected);
            
            if (shouldShow)
            {
                this.Invoke(new Action(() => 
                {
                    AddMessageBubble(msg.Text, msg.SenderId == _currentUser, msg.SenderId, msg);
                }));
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            //Debug.WriteLine($"Sendm messege {textBox1.Text} from {sender} to {listBox1.SelectedItem.ToString()} ");
           // if (listBox1.SelectedItem == null)
            //{
            //    MessageBox.Show("no chat selected");
            //    return;
            //}
            string text = textBox1.Text;
            string selected = listBox1.SelectedItem.ToString();

            string targetReceiver = (selected == "General Chat") ? "General" : selected;

            var msg = new MessageModel(_currentUser, targetReceiver, text);
            MessengerServer.Send(msg);
            textBox1.Clear();
        }

        private void AddMessageBubble(string text, bool isMe, string authorName, MessageModel msg)
        {
            //viusals
            Panel pnl = new Panel();
            pnl.AutoSize = true;
            pnl.Padding = new Padding(10);
            pnl.Margin = new Padding(5);
            if (isMe) pnl.BackColor = Color.LightBlue;
            else pnl.BackColor = Color.LightGray;

            Label content = new Label();
            content.Text = $"{authorName}:\n{text}";
            content.AutoSize = true;
            content.Font = new Font("Segoe UI", 10); 
            content.MaximumSize = new Size(300, 0);
            content.Location = new Point(10, 10);
            //delivery
            pnl.Controls.Add(content);

            var deliveredTo = msg.DeliveryStatus.Where(x => x.Value == true&& x.Key != "Admin"&& x.Key != authorName).Select(x => x.Key).ToList();

            if (deliveredTo.Count > 0)
                {
                    Label statusLbl = new Label();
                    statusLbl.Text = "read: " + string.Join(", ", deliveredTo);
                    statusLbl.Font = new Font("Segoe UI", 8, FontStyle.Italic); 
                    statusLbl.ForeColor = Color.DimGray;
                    statusLbl.AutoSize = true;
                    statusLbl.Location = new Point(10, content.Bottom + 5);
                    pnl.Controls.Add(statusLbl);
                    pnl.Padding = new Padding(10, 10, 10, 25);
                }
            
            flowLayoutPanel1.Controls.Add(pnl);

            flowLayoutPanel1.ScrollControlIntoView(pnl);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {RefreshChat();}

        private void RefreshChat()
        {
            if (listBox1.SelectedItem == null) return;
            flowLayoutPanel1.Controls.Clear();
            string selected = listBox1.SelectedItem.ToString();

            foreach (var msg in MessengerServer.AllHistory)
            {
                bool show = false;

                if (selected == "General Chat" && msg.ConversationId == "General")
                {
                    show = true;
                    msg.DeliveryStatus[_currentUser] = true; 
                }
                else if (_currentUser == "Admin")
                {
                    if (selected.Contains("notes"))
                    {
                        string targetUser = selected.Replace(" notes", "");
                        show = (msg.SenderId == targetUser && msg.ConversationId == targetUser);
                    }
                    else if (selected.Contains("-"))
                    {
                        string[] users = selected.Split('-');
                        show = (msg.SenderId == users[0] && msg.ConversationId == users[1]) ||
                               (msg.SenderId == users[1] && msg.ConversationId == users[0]);
                    }
                }
                else
                {
                    show = (msg.SenderId == _currentUser && msg.ConversationId == selected) ||
                           (msg.SenderId == selected && msg.ConversationId == _currentUser);
                }

                if (show) AddMessageBubble(msg.Text, msg.SenderId == _currentUser, msg.SenderId, msg);
            }
        }

    }

}