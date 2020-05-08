namespace DRTAccessFileSetup.Forms
{
	partial class FormMain
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnValidateDatabase = new System.Windows.Forms.Button();
			this.lblDbVersion = new System.Windows.Forms.Label();
			this.lblAppVersion = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtLog = new System.Windows.Forms.RichTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupLog = new System.Windows.Forms.GroupBox();
			this.btnClearLog = new System.Windows.Forms.Button();
			this.lbQueries = new System.Windows.Forms.CheckedListBox();
			this.groupQueries = new System.Windows.Forms.GroupBox();
			this.chkbAutoCommit = new System.Windows.Forms.CheckBox();
			this.btnOpenFolder = new System.Windows.Forms.Button();
			this.btnOpenFile = new System.Windows.Forms.Button();
			this.btnUpgrade = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.validationWorker = new System.ComponentModel.BackgroundWorker();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.groupLog.SuspendLayout();
			this.groupQueries.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.btnValidateDatabase);
			this.groupBox1.Controls.Add(this.lblDbVersion);
			this.groupBox1.Controls.Add(this.lblAppVersion);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(275, 127);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Info";
			// 
			// btnValidateDatabase
			// 
			this.btnValidateDatabase.Location = new System.Drawing.Point(49, 88);
			this.btnValidateDatabase.Name = "btnValidateDatabase";
			this.btnValidateDatabase.Size = new System.Drawing.Size(178, 23);
			this.btnValidateDatabase.TabIndex = 4;
			this.btnValidateDatabase.Text = "Validate Database Schema";
			this.btnValidateDatabase.UseVisualStyleBackColor = true;
			this.btnValidateDatabase.Click += new System.EventHandler(this.btnValidateDatabase_Click);
			// 
			// lblDbVersion
			// 
			this.lblDbVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblDbVersion.Location = new System.Drawing.Point(157, 22);
			this.lblDbVersion.Name = "lblDbVersion";
			this.lblDbVersion.Size = new System.Drawing.Size(100, 23);
			this.lblDbVersion.TabIndex = 1;
			this.lblDbVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblAppVersion
			// 
			this.lblAppVersion.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblAppVersion.Location = new System.Drawing.Point(157, 54);
			this.lblAppVersion.Name = "lblAppVersion";
			this.lblAppVersion.Size = new System.Drawing.Size(100, 23);
			this.lblAppVersion.TabIndex = 3;
			this.lblAppVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Selected database version:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Current application version:";
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.Color.White;
			this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Location = new System.Drawing.Point(0, 0);
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.Size = new System.Drawing.Size(520, 510);
			this.txtLog.TabIndex = 0;
			this.txtLog.Text = "";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.txtLog);
			this.panel1.Location = new System.Drawing.Point(18, 22);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(522, 512);
			this.panel1.TabIndex = 2;
			// 
			// groupLog
			// 
			this.groupLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupLog.Controls.Add(this.btnClearLog);
			this.groupLog.Controls.Add(this.panel1);
			this.groupLog.Location = new System.Drawing.Point(303, 12);
			this.groupLog.Name = "groupLog";
			this.groupLog.Size = new System.Drawing.Size(558, 584);
			this.groupLog.TabIndex = 2;
			this.groupLog.TabStop = false;
			this.groupLog.Text = "Log";
			// 
			// btnClearLog
			// 
			this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClearLog.Location = new System.Drawing.Point(465, 545);
			this.btnClearLog.Name = "btnClearLog";
			this.btnClearLog.Size = new System.Drawing.Size(75, 23);
			this.btnClearLog.TabIndex = 0;
			this.btnClearLog.Text = "Clear";
			this.btnClearLog.UseVisualStyleBackColor = true;
			this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
			// 
			// lbQueries
			// 
			this.lbQueries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbQueries.FormattingEnabled = true;
			this.lbQueries.IntegralHeight = false;
			this.lbQueries.Location = new System.Drawing.Point(17, 60);
			this.lbQueries.Name = "lbQueries";
			this.lbQueries.Size = new System.Drawing.Size(240, 343);
			this.lbQueries.TabIndex = 2;
			// 
			// groupQueries
			// 
			this.groupQueries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupQueries.Controls.Add(this.chkbAutoCommit);
			this.groupQueries.Controls.Add(this.btnOpenFolder);
			this.groupQueries.Controls.Add(this.btnOpenFile);
			this.groupQueries.Controls.Add(this.btnUpgrade);
			this.groupQueries.Controls.Add(this.lbQueries);
			this.groupQueries.Location = new System.Drawing.Point(12, 145);
			this.groupQueries.Name = "groupQueries";
			this.groupQueries.Size = new System.Drawing.Size(275, 451);
			this.groupQueries.TabIndex = 1;
			this.groupQueries.TabStop = false;
			this.groupQueries.Text = "Queries";
			// 
			// chkbAutoCommit
			// 
			this.chkbAutoCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkbAutoCommit.AutoSize = true;
			this.chkbAutoCommit.Location = new System.Drawing.Point(18, 416);
			this.chkbAutoCommit.Name = "chkbAutoCommit";
			this.chkbAutoCommit.Size = new System.Drawing.Size(84, 17);
			this.chkbAutoCommit.TabIndex = 3;
			this.chkbAutoCommit.Text = "Auto-commit";
			this.chkbAutoCommit.UseVisualStyleBackColor = true;
			// 
			// btnOpenFolder
			// 
			this.btnOpenFolder.Location = new System.Drawing.Point(98, 28);
			this.btnOpenFolder.Name = "btnOpenFolder";
			this.btnOpenFolder.Size = new System.Drawing.Size(75, 23);
			this.btnOpenFolder.TabIndex = 1;
			this.btnOpenFolder.Text = "Open Folder";
			this.btnOpenFolder.UseVisualStyleBackColor = true;
			this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
			// 
			// btnOpenFile
			// 
			this.btnOpenFile.Location = new System.Drawing.Point(17, 28);
			this.btnOpenFile.Name = "btnOpenFile";
			this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
			this.btnOpenFile.TabIndex = 0;
			this.btnOpenFile.Text = "Open File";
			this.btnOpenFile.UseVisualStyleBackColor = true;
			this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
			// 
			// btnUpgrade
			// 
			this.btnUpgrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnUpgrade.Location = new System.Drawing.Point(132, 412);
			this.btnUpgrade.Name = "btnUpgrade";
			this.btnUpgrade.Size = new System.Drawing.Size(125, 23);
			this.btnUpgrade.TabIndex = 4;
			this.btnUpgrade.Text = "Start Upgrade";
			this.btnUpgrade.UseVisualStyleBackColor = true;
			this.btnUpgrade.Click += new System.EventHandler(this.btnUpgrade_Click);
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(786, 609);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 4;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// progress
			// 
			this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progress.Location = new System.Drawing.Point(12, 609);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(768, 23);
			this.progress.Step = 1;
			this.progress.TabIndex = 3;
			this.progress.Visible = false;
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
			// 
			// validationWorker
			// 
			this.validationWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.validationWorker_DoWork);
			this.validationWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.validationWorker_RunWorkerCompleted);
			// 
			// FormMain
			// 
			this.AcceptButton = this.btnUpgrade;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(873, 644);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.groupQueries);
			this.Controls.Add(this.groupLog);
			this.Controls.Add(this.groupBox1);
			this.Name = "FormMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DRT Access File Setup";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.groupLog.ResumeLayout(false);
			this.groupQueries.ResumeLayout(false);
			this.groupQueries.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblAppVersion;
		private System.Windows.Forms.Label lblDbVersion;
		private System.Windows.Forms.RichTextBox txtLog;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupLog;
		private System.Windows.Forms.CheckedListBox lbQueries;
		private System.Windows.Forms.GroupBox groupQueries;
		private System.Windows.Forms.Button btnUpgrade;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.Button btnClearLog;
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.Button btnOpenFile;
		private System.Windows.Forms.Button btnOpenFolder;
		private System.Windows.Forms.CheckBox chkbAutoCommit;
		private System.Windows.Forms.Button btnValidateDatabase;
		private System.ComponentModel.BackgroundWorker validationWorker;
	}
}

