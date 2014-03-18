namespace SnapSnatcher
{
    partial class frmSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.label4 = new System.Windows.Forms.Label();
            this.chkAutostart = new System.Windows.Forms.CheckBox();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.chkSnaps = new System.Windows.Forms.CheckBox();
            this.chkStories = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnFolder = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(144, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "seconds";
            // 
            // chkAutostart
            // 
            this.chkAutostart.AutoSize = true;
            this.chkAutostart.Location = new System.Drawing.Point(12, 79);
            this.chkAutostart.Name = "chkAutostart";
            this.chkAutostart.Size = new System.Drawing.Size(71, 17);
            this.chkAutostart.TabIndex = 13;
            this.chkAutostart.Text = "Auto start";
            this.chkAutostart.UseVisualStyleBackColor = true;
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(93, 7);
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
            this.numInterval.TabIndex = 9;
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
            this.chkSnaps.Location = new System.Drawing.Point(12, 56);
            this.chkSnaps.Name = "chkSnaps";
            this.chkSnaps.Size = new System.Drawing.Size(105, 17);
            this.chkSnaps.TabIndex = 12;
            this.chkSnaps.Text = "Download snaps";
            this.chkSnaps.UseVisualStyleBackColor = true;
            // 
            // chkStories
            // 
            this.chkStories.AutoSize = true;
            this.chkStories.Checked = true;
            this.chkStories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStories.Location = new System.Drawing.Point(12, 33);
            this.chkStories.Name = "chkStories";
            this.chkStories.Size = new System.Drawing.Size(107, 17);
            this.chkStories.TabIndex = 10;
            this.chkStories.Text = "Download stories";
            this.chkStories.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Polling interval";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(12, 115);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(154, 20);
            this.txtPath.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Snaps directory:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(163, 114);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(25, 21);
            this.btnFolder.TabIndex = 16;
            this.btnFolder.Text = "...";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(113, 141);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 174);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.chkAutostart);
            this.Controls.Add(this.numInterval);
            this.Controls.Add(this.chkSnaps);
            this.Controls.Add(this.chkStories);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAutostart;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.CheckBox chkSnaps;
        private System.Windows.Forms.CheckBox chkStories;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.Button btnSave;

    }
}