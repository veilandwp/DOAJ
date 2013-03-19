using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinformAM
{
    public partial class AMLogin : Form
    {
        public AMLogin()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;
            bool res1 = (username == System.Configuration.ConfigurationSettings.AppSettings["username"]);
            bool res2 = (password == System.Configuration.ConfigurationSettings.AppSettings["password"]);
            if (res1 == true && res2 == true)
            {
                this.Hide();
                MainForm mainform = new MainForm();
                mainform.Show();
            }
            else
            {
                labelTips.Text = "The username or password typed was wrong,please try again!";
            }
        }
        private void textBoxUsername_TextChanged(object sender, EventArgs e)
        {
            labelTips.Text = "";
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            labelTips.Text = "";
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AMLogin_Load(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar='●';
        }
    }
}
