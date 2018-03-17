using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ERP
{
    public partial class Login : Form
    {
        public string _token;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginAction();
        }


        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        private void tmLogin_Tick(object sender, EventArgs e)
        {
            btnLogin.Focus();
            btnLogin_Click(sender, e);
            tmLogin.Enabled = false;
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {
            //tmLogin.Interval = 800;
            //tmLogin.Enabled = true;
        }

        private void txtUserId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 13)
            {
                btnLogin_Click(sender, e);
                e.Handled = true;
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void LoginAction()
        {
            ApiResponse res = new ApiResponse();
            try
            {
                string url = "auth/token";
                var paras = "{\"email\": \"" + txtUserId.Text.Trim() + "\",\"password\": \"" + txtPass.Text.Trim() + "\",\"rememberMe\": " + "true" + " }";
                res = HTTP.PostJson(url, paras);
                if (res.Status && Util.IsJson(res.RawText))
                {
                    TokenObject _TokenObject = JsonConvert.DeserializeObject<TokenObject>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    ERP.Base.Config.Token = "Bearer " + _TokenObject.id_token;

                    Main frmMain = new Main();
                    frmMain.Show();
                    this.Hide();
                    frmMain.frmLogin = this;
                }
                else
                {
                    ERP.Base.Config.Token = null;
                    MessageBox.Show("Failed to sign in! Please check your credentials and try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                
            }
            catch (Exception ex)
            {
                ERP.Base.Config.Token = null;
                MessageBox.Show("Failed to sign in! Please check your credentials and try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
    }

    public class TokenObject
    {
        public string id_token { get; set; }
    }
}