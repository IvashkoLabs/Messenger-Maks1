using Messenger_Maks.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messenger_Maks
{
    public partial class Form1: Form//First window with all users and + new users
    {
        public Form1()
        {
            InitializeComponent();
            RefreshUserList();
            MessengerServer.Service.OnUserListChanged += () =>
            {
                this.Invoke(new Action(RefreshUserList));
            };
        }
        private void RefreshUserList()
        {
            userListBox.Items.Clear();
            foreach (var user in MessengerServer.Service.GetAllUsers())
                userListBox.Items.Add(user);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //if (userListBox.SelectedItem == null) return;
            new ChatForm(userListBox.SelectedItem.ToString()).Show();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            //try
           // {
                MessengerServer.Service.CreateUser(userNameTextBox.Text);
                RefreshUserList();
                userNameTextBox.Clear();
            //}
            //catch (Exception ex){

                //MessageBox.Show(ex.Message, "Name is empty or taken", MessageBoxButtons.OK, MessageBoxIcon.Warning);}
        }

    }
}
