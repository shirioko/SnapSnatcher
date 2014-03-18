using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnapSnatcher
{
    public partial class frmSettings : Form
    {
        public bool dlSnaps;
        public bool dlStories;
        public bool autoStart;
        public decimal interval;
        public string path;
        private DataConnector connector;

        public frmSettings(DataConnector connector)
        {
            this.connector = connector;
            InitializeComponent();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new FolderBrowserDialog();
            f.Description = "Select where to download new snaps";
            DialogResult r = f.ShowDialog(this);
            if (r == System.Windows.Forms.DialogResult.OK && Directory.Exists(f.SelectedPath))
            {
                this.txtPath.Text = f.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.dlSnaps = this.chkSnaps.Checked;
            this.autoStart = this.chkAutostart.Checked;
            this.dlStories = this.chkStories.Checked;
            this.path = this.txtPath.Text;
            this.interval = this.numInterval.Value;
            //save
            this.connector.SetAppSetting(DataConnector.Settings.DL_SNAPS, this.dlSnaps.ToString());
            this.connector.SetAppSetting(DataConnector.Settings.AUTOSTART, this.autoStart.ToString());
            this.connector.SetAppSetting(DataConnector.Settings.DL_STORIES, this.dlStories.ToString());
            this.connector.SetAppSetting(DataConnector.Settings.PATH, this.path);
            this.connector.SetAppSetting(DataConnector.Settings.INTERVAL, this.interval.ToString());
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            //load settings
            this.path = this.connector.GetAppSetting(DataConnector.Settings.PATH);
            if (string.IsNullOrEmpty(this.path) || !Directory.Exists(this.path))
            {
                //use default
                this.path = Path.Combine(Directory.GetCurrentDirectory(), "snaps");
                this.connector.SetAppSetting(DataConnector.Settings.PATH, this.path);
            }

            decimal foo = 5;
            if (decimal.TryParse(this.connector.GetAppSetting(DataConnector.Settings.INTERVAL), out foo))
            {
                this.interval = foo;
            }
            else
            {
                this.interval = 5;
            }

            bool bar;
            if(bool.TryParse(this.connector.GetAppSetting(DataConnector.Settings.AUTOSTART), out bar))
            {
                this.autoStart = bar;
            }
            else
            {
                this.autoStart = false;
            }
            if (bool.TryParse(this.connector.GetAppSetting(DataConnector.Settings.DL_SNAPS), out bar))
            {
                this.dlSnaps = bar;
            }
            else
            {
                this.dlSnaps = true;
            }
            if (bool.TryParse(this.connector.GetAppSetting(DataConnector.Settings.DL_STORIES), out bar))
            {
                this.dlStories = bar;
            }
            else
            {
                this.dlStories = true;
            }

            //apply values
            this.numInterval.Value = this.interval;
            this.chkSnaps.Checked = this.dlSnaps;
            this.chkStories.Checked = this.dlStories;
            this.chkAutostart.Checked = this.autoStart;
            this.txtPath.Text = this.path;
        }
    }
}
