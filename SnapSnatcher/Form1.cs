using ICSharpCode.SharpZipLib.Zip;
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
        protected string reqToken;
        protected string path;

        protected static bool Run = false;

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
            this.username = this.connector.GetAppSetting(DataConnector.Settings.USERNAME);
            this.authToken = this.connector.GetAppSetting(DataConnector.Settings.AUTH_TOKEN);
            this.reqToken = this.connector.GetAppSetting(DataConnector.Settings.REQ_TOKEN);
            this.path = this.connector.GetAppSetting(DataConnector.Settings.PATH);
            if (string.IsNullOrEmpty(this.path) || !Directory.Exists(this.path))
            {
                this.path = Path.Combine(Directory.GetCurrentDirectory(), "snaps");
            }
            if (!Decimal.TryParse(this.connector.GetAppSetting(DataConnector.Settings.INTERVAL), out this.interval))
            {
                this.interval = 1;
            }
            bool foo = true;
            if (bool.TryParse(this.connector.GetAppSetting(DataConnector.Settings.DL_SNAPS), out foo))
            {
                this.dlSnaps = foo;
            }
            else
            {
                this.dlSnaps = true;
            }
            if (bool.TryParse(this.connector.GetAppSetting(DataConnector.Settings.DL_STORIES), out foo))
            {
                this.dlStories = foo;
            }
            else
            {
                this.dlStories = true;
            }
            if (bool.TryParse(this.connector.GetAppSetting(DataConnector.Settings.AUTOSTART), out foo))
            {
                this.autoStart = foo;
            }
            else
            {
                this.autoStart = false;
            }
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //auto fill
            this.txtUsername.Text = this.username;
            this.txtToken.Text = this.authToken;
            this.txtReqToken.Text = this.reqToken;
            if(this.autoStart && !string.IsNullOrEmpty(this.username) &&
                (!string.IsNullOrEmpty(this.authToken) || !string.IsNullOrEmpty(this.reqToken)))
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
                this.connector.SetAppSetting(DataConnector.Settings.USERNAME, this.username);
                this.connector.SetAppSetting(DataConnector.Settings.AUTH_TOKEN, this.authToken);
            }
        }

        protected void Start()
        {
            //disable controls
            this.grpAuth.Enabled = false;
            this.btnStop.Visible = true;
            
            //hide
            this.Visible = false;
            this.ShowInTaskbar = false;

            this.snapconnector = new SnapConnector(this.username, this.authToken, this.reqToken);

            Run = true;

            this.listener = new Thread(new ThreadStart(Listen));
            this.listener.IsBackground = true;
            this.listener.Start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //validate credentials presence
            this.username = this.txtUsername.Text;
            this.authToken = this.txtToken.Text;
            this.reqToken = this.txtReqToken.Text;
            if (string.IsNullOrEmpty(this.username) || (string.IsNullOrEmpty(this.authToken) && string.IsNullOrEmpty(this.reqToken)))
            {
                //show error
                MessageBox.Show("Please enter username and auth token or req token", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                //update config
                this.connector.SetAppSetting(DataConnector.Settings.USERNAME, this.username);
                this.connector.SetAppSetting(DataConnector.Settings.AUTH_TOKEN, this.authToken);
                this.connector.SetAppSetting(DataConnector.Settings.REQ_TOKEN, this.reqToken);

                //start doing shit
                this.Start();
            }
        }

        protected void Listen()
        {
            int timeout = (int)(this.interval * 1000);//milliseconds
            while (Run && this.FetchUpdates())
            {
                Thread.Sleep(timeout);
            }
            this.ResetFormState();
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
                                    this.NotifyTray(snap);
                                }
                                catch (WebException w)
                                {
                                    //too bad
                                    HttpWebResponse resp = w.Response as HttpWebResponse;
                                    if (resp.StatusCode != HttpStatusCode.Gone && resp.StatusCode != HttpStatusCode.InternalServerError)
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
                                    this.NotifyTray(story);
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
                else if (resp.StatusCode == HttpStatusCode.InternalServerError)
                {
                    //shit happens
                    return true;
                }
                else
                {
                    MessageBox.Show(wex.Message, "WebException in listener thread", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.autoStart = false;
                return false;
            }
            catch (Exception ex)
            {
                //TODO: log
                MessageBox.Show(ex.Message, "General Exception in listener thread", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.autoStart = false;
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
            string filename = Path.Combine(this.path, string.Format("{0}-{1}.{2}", snap.sn, snap.id, snap.m == 0 ? "jpg" : "mov"));
            File.WriteAllBytes(filename, image);
        }

        private void SaveMedia(byte[] image, JsonClasses.Story story)
        {
            if (story.zipped)
            {
                File.WriteAllBytes("story.zip", image);
                image = this.Unzip(image);
            }
            if (image != null)
            {
                string filename = Path.Combine(this.path, string.Format("{0}.{1}", story.id, story.media_type == 0 ? "jpg" : "mov"));
                File.WriteAllBytes(filename, image);
            }
        }

        protected byte[] Unzip(byte[] zipData)
        {
            using (var compressedStream = new MemoryStream(zipData))
            {
                using (var zipStream = new ZipInputStream(compressedStream))
                {
                    ZipEntry entry;
                    while ((entry = zipStream.GetNextEntry()) != null)
                    {
                        if (entry.IsFile && entry.Name.StartsWith("media"))
                        {
                            using (var resultStream = new MemoryStream())
                            {
                                int size = 2048;
                                byte[] data = new byte[2048];
                                while (true)
                                {
                                    size = zipStream.Read(data, 0, data.Length);
                                    if (size > 0)
                                    {
                                        resultStream.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        return resultStream.ToArray();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        protected delegate void NoArgumentDelegate();
        protected void ResetFormState()
        {
            if (this.InvokeRequired)
            {
                NoArgumentDelegate d = new NoArgumentDelegate(ResetFormState);
                this.Invoke(d);
            }
            else
            {
               //reset
                this.btnStop.Visible = false;
                this.grpAuth.Enabled = true;
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
            Process.Start(this.path);
            this.unseenCounter = 0;
            this.UpdateNotifyText();
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.OpenSnapsFolder();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnStop.Visible = false;
            Run = false;
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            frmCapture cap = new frmCapture();
            DialogResult res = cap.ShowDialog(this);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                //good good
                this.reqToken = cap.reqToken;
                this.authToken = cap.authToken;
                this.username = cap.username;
                this.txtReqToken.Text = this.reqToken;
                this.txtUsername.Text = this.username;
                this.txtToken.Text = this.authToken;
                this.connector.SetAppSetting(DataConnector.Settings.AUTH_TOKEN, this.authToken);
                this.connector.SetAppSetting(DataConnector.Settings.USERNAME, this.username);
                this.connector.SetAppSetting(DataConnector.Settings.REQ_TOKEN, this.reqToken);
                MessageBox.Show(string.Format("Captured token for {0}\r\nYou should disable the proxy settings on your mobile device\r\n\r\nPress the [Start] button to start listening to new snaps!", this.username), "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            frmSettings s = new frmSettings(this.connector);
            DialogResult r = s.ShowDialog(this);
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                //reload settings
                this.interval = s.interval;
                this.dlSnaps = s.dlSnaps;
                this.dlStories = s.dlStories;
                this.autoStart = s.autoStart;
                this.path = s.path;
            }
        }
    }
}
