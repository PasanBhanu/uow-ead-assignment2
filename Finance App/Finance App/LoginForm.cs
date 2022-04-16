using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Finance_App
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginToApp(object sender, EventArgs e)
        {
            if (txtUsername.Text == "pasan" && txtPassword.Text == "pass")
            {
                Close();
            } else
            {
                MessageBox.Show("Invalid username or password! Please try again.", "Simply Finance App", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ExitFromApp(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
