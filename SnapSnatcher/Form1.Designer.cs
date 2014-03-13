namespace SnapSnatcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtToken = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpAuth = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtReqToken = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.grpSettings = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.chkSnaps = new System.Windows.Forms.CheckBox();
            this.chkStories = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.itemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.itemSnaps = new System.Windows.Forms.ToolStripMenuItem();
            this.itemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnCapture = new System.Windows.Forms.Button();
            this.grpAuth.SuspendLayout();
            this.grpSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtToken
            // 
            this.txtToken.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtToken.Location = new System.Drawing.Point(75, 46);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(135, 20);
            this.txtToken.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Auth Token";
            // 
            // grpAuth
            // 
            this.grpAuth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpAuth.Controls.Add(this.btnCapture);
            this.grpAuth.Controls.Add(this.label6);
            this.grpAuth.Controls.Add(this.txtReqToken);
            this.grpAuth.Controls.Add(this.label5);
            this.grpAuth.Controls.Add(this.txtUsername);
            this.grpAuth.Controls.Add(this.label2);
            this.grpAuth.Controls.Add(this.btnLogin);
            this.grpAuth.Controls.Add(this.btnRegister);
            this.grpAuth.Controls.Add(this.label1);
            this.grpAuth.Controls.Add(this.txtToken);
            this.grpAuth.Location = new System.Drawing.Point(12, 12);
            this.grpAuth.Name = "grpAuth";
            this.grpAuth.Size = new System.Drawing.Size(220, 142);
            this.grpAuth.TabIndex = 2;
            this.grpAuth.TabStop = false;
            this.grpAuth.Text = "Authentication";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Req Token";
            // 
            // txtReqToken
            // 
            this.txtReqToken.Location = new System.Drawing.Point(75, 86);
            this.txtReqToken.Name = "txtReqToken";
            this.txtReqToken.Size = new System.Drawing.Size(135, 20);
            this.txtReqToken.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(75, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "or";
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(75, 20);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(135, 20);
            this.txtUsername.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(75, 112);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(60, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Enabled = false;
            this.btnRegister.Location = new System.Drawing.Point(141, 112);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(69, 23);
            this.btnRegister.TabIndex = 3;
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;
            // 
            // grpSettings
            // 
            this.grpSettings.Controls.Add(this.label4);
            this.grpSettings.Controls.Add(this.btnStart);
            this.grpSettings.Controls.Add(this.chkAutostart);
            this.grpSettings.Controls.Add(this.numInterval);
            this.grpSettings.Controls.Add(this.chkSnaps);
            this.grpSettings.Controls.Add(this.chkStories);
            this.grpSettings.Controls.Add(this.label3);
            this.grpSettings.Location = new System.Drawing.Point(12, 164);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Size = new System.Drawing.Size(220, 116);
            this.grpSettings.TabIndex = 4;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "seconds";
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(125, 46);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(85, 63);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(12, 92);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(71, 17);
            this.chkAutostart.TabIndex = 7;
            this.chkAutostart.Text = "Auto start";
            this.chkAutostart.UseVisualStyleBackColor = true;
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(90, 20);
            this.numInterval.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numInterval.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(45, 20);
            this.numInterval.TabIndex = 4;
            this.numInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // chkSnaps
            // 
            this.chkSnaps.AutoSize = true;
            this.chkSnaps.Checked = true;
            this.chkSnaps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSnaps.Location = new System.Drawing.Point(12, 69);
            this.chkSnaps.Name = "chkSnaps";
            this.chkSnaps.Size = new System.Drawing.Size(105, 17);
            this.chkSnaps.TabIndex = 6;
            this.chkSnaps.Text = "Download snaps";
            this.chkSnaps.UseVisualStyleBackColor = true;
            // 
            // chkStories
            // 
            this.chkStories.AutoSize = true;
            this.chkStories.Checked = true;
            this.chkStories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStories.Location = new System.Drawing.Point(12, 46);
            this.chkStories.Name = "chkStories";
            this.chkStories.Size = new System.Drawing.Size(107, 17);
            this.chkStories.TabIndex = 5;
            this.chkStories.Text = "Download stories";
            this.chkStories.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Polling interval";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "SnapSnatcher";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.BalloonTipClicked += new System.EventHandler(this.notifyIcon1_BalloonTipClicked);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemShow,
            this.itemSnaps,
            this.itemExit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(171, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // itemShow
            // 
            this.itemShow.Name = "itemShow";
            this.itemShow.Size = new System.Drawing.Size(170, 22);
            this.itemShow.Text = "Show program";
            this.itemShow.Click += new System.EventHandler(this.itemShow_Click);
            // 
            // itemSnaps
            // 
            this.itemSnaps.Name = "itemSnaps";
            this.itemSnaps.Size = new System.Drawing.Size(170, 22);
            this.itemSnaps.Text = "Open snaps folder";
            this.itemSnaps.Click += new System.EventHandler(this.itemSnaps_Click);
            // 
            // itemExit
            // 
            this.itemExit.Name = "itemExit";
            this.itemExit.Size = new System.Drawing.Size(170, 22);
            this.itemExit.Text = "Exit";
            this.itemExit.Click += new System.EventHandler(this.itemExit_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(137, 210);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(85, 63);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(7, 112);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(62, 23);
            this.btnCapture.TabIndex = 8;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 291);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.grpSettings);
            this.Controls.Add(this.grpAuth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SnapSnatcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.grpAuth.ResumeLayout(false);
            this.grpAuth.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtToken;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpAuth;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.GroupBox grpSettings;
        private System.Windows.Forms.CheckBox chkAutostart;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.CheckBox chkSnaps;
        private System.Windows.Forms.CheckBox chkStories;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem itemExit;
        private System.Windows.Forms.ToolStripMenuItem itemShow;
        private System.Windows.Forms.ToolStripMenuItem itemSnaps;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtReqToken;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCapture;
    }
}

