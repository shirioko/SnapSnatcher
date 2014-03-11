using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapSnatcher
{
    public partial class frmLogin : Form
    {
        public string username;
        public string password;
        public string authToken;

        public frmLogin(string username)
        {
            this.username = username;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtUsername.Text) && !string.IsNullOrEmpty(this.txtPassword.Text))
            {
                this.username = this.txtUsername.Text;
                string password = this.txtPassword.Text;
                SnapConnector sc = new SnapConnector(this.username, null, null);
                string result = sc.Login(this.txtUsername.Text, this.txtPassword.Text);
                JObject jsonResult = JObject.Parse(result);
                JToken logged = jsonResult["logged"];
                if (logged != null)
                {
                    bool loggedBool = logged.ToObject<bool>();
                    if (loggedBool)
                    {
                        //success!
                        this.authToken = jsonResult["auth_token"].ToObject<string>();
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        string message = jsonResult["message"].ToObject<string>();
                        MessageBox.Show(message, "Login failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(result, "Mmm, something went wrong...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.txtUsername.Text = this.username;
        }
    }
}
