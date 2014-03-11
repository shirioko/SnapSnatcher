using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapSnatcher
{
    public partial class Form1 : Form
    {
        protected int unseenCounter = 0;
        protected string username;
        protected string authToken;

        protected Thread listener;

        protected bool dlSnaps = false;
        protected bool dlStories = false;
        protected bool autoStart = false;
        protected decimal interval = 5;

        const string SNAPS_FOLDER = "snaps";

        const string BLOB_KEY = "TTAyY25RNTFKaTk3dndUNA==";

        private DataConnector connector;

        public SnapConnector snapconnector;
        
        public Form1()
        {
            this.connector = new DataConnector();
            this.username = this.connector.GetAppSetting("username");
            this.authToken = this.connector.GetAppSetting("auth_token");
            if (!Decimal.TryParse(this.connector.GetAppSetting("interval"), out this.interval))
            {
                this.interval = 1;
            }
            bool foo = true;
            if (bool.TryParse(this.connector.GetAppSetting("dlsnaps"), out foo))
            {
                this.dlSnaps = foo;
            }
            if (bool.TryParse(this.connector.GetAppSetting("dlstories"), out foo))
            {
                this.dlStories = foo;
            }
            if (bool.TryParse(this.connector.GetAppSetting("autostart"), out foo))
            {
                this.autoStart = foo;
            }
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            //auto fill
            this.txtUsername.Text = this.username;
            this.txtToken.Text = this.authToken;
            this.chkAutostart.Checked = this.autoStart;
            this.chkSnaps.Checked = this.dlSnaps;
            this.chkStories.Checked = this.dlStories;
            if (this.interval < this.numInterval.Minimum)
            {
                this.interval = this.numInterval.Minimum;
            }
            if (this.interval > this.numInterval.Maximum)
            {
                this.interval = this.numInterval.Maximum;
            }
            this.numInterval.Value = this.interval;
            if(this.autoStart && !string.IsNullOrEmpty(this.username) && !string.IsNullOrEmpty(this.authToken))
            {
                //autostart
                this.Start();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //show warning
            frmLogin l = new frmLogin(this.username);
            DialogResult r = l.ShowDialog(this);
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                //update data
                this.username = l.username;
                this.authToken = l.authToken;
                this.txtUsername.Text = this.username;
                this.txtToken.Text = this.authToken;
                this.connector.SetAppSetting("username", this.username);
                this.connector.SetAppSetting("auth_token", this.authToken);
            }
        }

        protected void Start()
        {
            this.snapconnector = new SnapConnector(this.username, this.authToken);
            this.listener = new Thread(new ThreadStart(Listen));
            this.listener.IsBackground = true;
            this.listener.Start();

            //hide
            this.Visible = false;
            this.ShowInTaskbar = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //validate credentials presence
            this.username = this.txtUsername.Text;
            this.authToken = this.txtToken.Text;
            if (string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(this.authToken))
            {
                //show error
                MessageBox.Show("Please enter username and auth token", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //read settings
                this.dlSnaps = this.chkSnaps.Checked;
                this.dlStories = this.chkStories.Checked;
                this.autoStart = this.chkAutostart.Checked;

                //disable controls
                this.grpAuth.Enabled = false;
                this.grpSettings.Enabled = false;

                //update config
                this.connector.SetAppSetting("username", this.username);
                this.connector.SetAppSetting("auth_token", this.authToken);
                this.connector.SetAppSetting("interval", this.interval.ToString());
                this.connector.SetAppSetting("dlsnaps", this.dlSnaps.ToString());
                this.connector.SetAppSetting("dlstories", this.dlStories.ToString());
                this.connector.SetAppSetting("autostart", this.autoStart.ToString());

                //start doing shit
                this.Start();
            }
        }

        protected void Listen()
        {
            int timeout = (int)(this.interval * 1000);//milliseconds
            while (this.FetchUpdates())
            {
                Thread.Sleep(timeout);
            }
            this.onAuthError();
        }

        protected bool FetchUpdates()
        {
            try
            {
                string data = this.snapconnector.GetUpdates();

                if (this.dlSnaps)
                {
                    JsonClasses.Snap[] snaps = this.snapconnector.GetSnaps(data);
                    //process snaps
                    foreach (JsonClasses.Snap snap in snaps)
                    {
                        if (snap.st < 2 && !string.IsNullOrEmpty(snap.sn))
                        {
                            bool dlSnap = this.connector.AddSnap(snap);
                            if (dlSnap)
                            {
                                try
                                {
                                    byte[] image = this.snapconnector.GetMedia(snap.id);
                                    if (!isMedia(image))
                                    {
                                        image = decryptECB(image);
                                    }
                                    this.SaveMedia(image, snap);
                                    if (this.Visible == false)
                                    {
                                        this.NotifyTray(snap);
                                    }
                                }
                                catch (WebException w)
                                {
                                    //too bad
                                    HttpWebResponse resp = w.Response as HttpWebResponse;
                                    if (resp.StatusCode != HttpStatusCode.Gone)
                                    {
                                        throw w;
                                    }
                                }
                            }
                        }
                    }
                }
                if (this.dlStories)
                {
                    JsonClasses.Story[] stories = this.snapconnector.GetStories(data);
                    //process stories
                    foreach (JsonClasses.Story story in stories)
                    {
                        if (story.media_type < 2)
                        {
                            bool dlStory = this.connector.AddStory(story);
                            if (dlStory)
                            {
                                try
                                {
                                    byte[] image = this.snapconnector.GetStoryMedia(story.media_id, story.media_key, story.media_iv);
                                    this.SaveMedia(image, story);
                                    if (this.Visible == false)
                                    {
                                        this.NotifyTray(story);
                                    }
                                }
                                catch (WebException w)
                                {
                                    //too bad
                                    if (w.Status != WebExceptionStatus.ProtocolError)
                                    {
                                        throw w;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (WebException wex)
            {
                HttpWebResponse resp = wex.Response as HttpWebResponse;
                if (resp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Invalid credentials", "Auth error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(wex.Message, "WebException in listener thread", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            catch (Exception ex)
            {
                //TODO: log
                MessageBox.Show(ex.Message, "General Exception in listener thread", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        protected static bool isMedia(byte[] image)
        {
            if (image[0] == 0xff && image[1] == 0xd8)
            {
                //jpg
                return true;
            }
            if (image[0] == 0x00 && image[1] == 0x00)
            {
                //mp4
                return true;
            }
            return false;
        }

        protected static byte[] decryptECB(byte[] image)
        {
            using (RijndaelManaged rm = new RijndaelManaged())
            {
                rm.Mode = CipherMode.ECB;
                rm.Key = Convert.FromBase64String(BLOB_KEY);
                rm.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                rm.Padding = PaddingMode.Zeros;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, rm.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(image, 0, image.Length);
                        return ms.ToArray();
                    }
                }
            }
        }

        public static byte[] decryptCBC(byte[] image, string key, string iv)
        {
            using (RijndaelManaged rm = new RijndaelManaged())
            {
                rm.Mode = CipherMode.CBC;
                rm.Key = Convert.FromBase64String(key);
                rm.IV = Convert.FromBase64String(iv);
                rm.Padding = PaddingMode.Zeros;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, rm.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(image, 0, image.Length);
                        return ms.ToArray();
                    }
                }
            }
        }

        protected delegate void NotifySnapDelegate(JsonClasses.Snap snap);
        protected delegate void NotifyStoryDelegate(JsonClasses.Story storyu);

        protected void NotifyTray(JsonClasses.Snap snap)
        {
            
            if (this.InvokeRequired)
            {
                NotifySnapDelegate d = new NotifySnapDelegate(NotifyTray);
                this.Invoke(d, snap);
            }
            else
            {
                this.unseenCounter++;
                this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                this.notifyIcon1.BalloonTipTitle = string.Format("SnapSnatcher({0})", this.unseenCounter);
                this.notifyIcon1.BalloonTipText = string.Format("New snap from {0}!", snap.sn);
                notifyIcon1.ShowBalloonTip(5000);
                this.UpdateNotifyText();
            }
        }

        protected void NotifyTray(JsonClasses.Story story)
        {
            
            if (this.InvokeRequired)
            {
                NotifyStoryDelegate d = new NotifyStoryDelegate(NotifyTray);
                this.Invoke(d, story);
            }
            else
            {
                this.unseenCounter++;
                this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                this.notifyIcon1.BalloonTipTitle = string.Format("SnapSnatcher({0})", this.unseenCounter);
                this.notifyIcon1.BalloonTipText = string.Format("New story update from {0}!", story.username);
                notifyIcon1.ShowBalloonTip(5000);
                this.UpdateNotifyText();
            }
        }

        private void SaveMedia(byte[] image, JsonClasses.Snap snap)
        {
            if (!Directory.Exists("snaps"))
            {
                Directory.CreateDirectory("snaps");
            }
            string filename = string.Format("snaps\\{0}-{1}.{2}", snap.sn, snap.id, snap.m == 0 ? "jpg" : "mov");
            File.WriteAllBytes(filename, image);
        }

        private void SaveMedia(byte[] image, JsonClasses.Story story)
        {
            if (!Directory.Exists("snaps"))
            {
                Directory.CreateDirectory("snaps");
            }
            string filename = string.Format("snaps\\{0}.{1}", story.id, story.media_type == 0 ? "jpg" : "mov");
            File.WriteAllBytes(filename, image);
        }

        protected delegate void onAuthErrorDelegate();
        protected void onAuthError()
        {
            if (this.InvokeRequired)
            {
                onAuthErrorDelegate d = new onAuthErrorDelegate(onAuthError);
                this.Invoke(d);
            }
            else
            {
               //reset
                this.grpAuth.Enabled = true;
                this.grpSettings.Enabled = true;
                this.autoStart = false;
                this.chkAutostart.Checked = false;
                this.connector.SetAppSetting("autostart", this.autoStart.ToString());
                this.Restore();
            }
        }

        protected void Minimize()
        {
            this.Visible = false;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = false;
        }

        protected void Restore()
        {
            this.unseenCounter = 0;
            this.UpdateNotifyText();
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.Refresh();
            this.Focus();
        }

        private void UpdateNotifyText()
        {
            if (this.unseenCounter > 0)
            {
                this.notifyIcon1.Text = string.Format("SnapSnatcher({0})", this.unseenCounter);
            }
            else
            {
                this.notifyIcon1.Text = "SnapSnatcher";
            }
            this.Refresh();

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (this.unseenCounter > 0)
            {
                this.contextMenuStrip1.Items["itemSnaps"].Text = string.Format("Open snaps folder ({0})", this.unseenCounter);
            }
            else
            {
                this.contextMenuStrip1.Items["itemSnaps"].Text = "Open snaps folder";
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Minimize();
            }
        }

        private void itemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void itemShow_Click(object sender, EventArgs e)
        {
            this.Restore();
        }

        private void itemSnaps_Click(object sender, EventArgs e)
        {
            this.OpenSnapsFolder();
        }

        protected void OpenSnapsFolder()
        {
            string snapsFolder = Path.Combine(Directory.GetCurrentDirectory(), SNAPS_FOLDER);
            if (!Directory.Exists(snapsFolder))
            {
                Directory.CreateDirectory(snapsFolder);
            }

            Process.Start(snapsFolder);
            this.unseenCounter = 0;
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.OpenSnapsFolder();
        }
    }
}
