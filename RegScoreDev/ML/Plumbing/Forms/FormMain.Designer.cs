namespace Plumbing.Forms
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
			this.btnExit = new System.Windows.Forms.Button();
			this.txtJSON = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtOutput = new System.Windows.Forms.RichTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.progress = new System.Windows.Forms.ProgressBar();
			this.txtOutputFolder = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtInputFileName = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtParametersFileName = new System.Windows.Forms.TextBox();
			this.txtLogFileName = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.chkbKeepOpen = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Enabled = false;
			this.btnExit.Location = new System.Drawing.Point(800, 620);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 7;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// txtJSON
			// 
			this.txtJSON.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.txtJSON.Location = new System.Drawing.Point(12, 191);
			this.txtJSON.Multiline = true;
			this.txtJSON.Name = "txtJSON";
			this.txtJSON.ReadOnly = true;
			this.txtJSON.Size = new System.Drawing.Size(343, 348);
			this.txtJSON.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 171);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "JSON parameters:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-28, 608);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(943, 2);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// txtOutput
			// 
			this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutput.Location = new System.Drawing.Point(368, 35);
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ReadOnly = true;
			this.txtOutput.Size = new System.Drawing.Size(507, 560);
			this.txtOutput.TabIndex = 5;
			this.txtOutput.Text = "";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(370, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(42, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Output:";
			// 
			// progress
			// 
			this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progress.Location = new System.Drawing.Point(12, 620);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(692, 23);
			this.progress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progress.TabIndex = 6;
			this.progress.Visible = false;
			// 
			// txtOutputFolder
			// 
			this.txtOutputFolder.Location = new System.Drawing.Point(12, 35);
			this.txtOutputFolder.Name = "txtOutputFolder";
			this.txtOutputFolder.ReadOnly = true;
			this.txtOutputFolder.Size = new System.Drawing.Size(343, 20);
			this.txtOutputFolder.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(71, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Output folder:";
			// 
			// txtInputFileName
			// 
			this.txtInputFileName.Location = new System.Drawing.Point(12, 87);
			this.txtInputFileName.Name = "txtInputFileName";
			this.txtInputFileName.ReadOnly = true;
			this.txtInputFileName.Size = new System.Drawing.Size(343, 20);
			this.txtInputFileName.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(10, 67);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(137, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Access database file name:";
			// 
			// txtParametersFileName
			// 
			this.txtParametersFileName.Location = new System.Drawing.Point(12, 139);
			this.txtParametersFileName.Name = "txtParametersFileName";
			this.txtParametersFileName.ReadOnly = true;
			this.txtParametersFileName.Size = new System.Drawing.Size(343, 20);
			this.txtParametersFileName.TabIndex = 2;
			// 
			// txtLogFileName
			// 
			this.txtLogFileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.txtLogFileName.Location = new System.Drawing.Point(12, 574);
			this.txtLogFileName.Name = "txtLogFileName";
			this.txtLogFileName.ReadOnly = true;
			this.txtLogFileName.Size = new System.Drawing.Size(343, 20);
			this.txtLogFileName.TabIndex = 4;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(10, 119);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(114, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Pparameters file name:";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(10, 553);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(73, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "Log file name:";
			// 
			// worker
			// 
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
			// 
			// chkbKeepOpen
			// 
			this.chkbKeepOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkbKeepOpen.AutoSize = true;
			this.chkbKeepOpen.Location = new System.Drawing.Point(716, 624);
			this.chkbKeepOpen.Name = "chkbKeepOpen";
			this.chkbKeepOpen.Size = new System.Drawing.Size(78, 17);
			this.chkbKeepOpen.TabIndex = 8;
			this.chkbKeepOpen.Text = "Keep open";
			this.chkbKeepOpen.UseVisualStyleBackColor = true;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(887, 655);
			this.Controls.Add(this.chkbKeepOpen);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtLogFileName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtInputFileName);
			this.Controls.Add(this.txtParametersFileName);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.txtOutputFolder);
			this.Controls.Add(this.txtJSON);
			this.Controls.Add(this.btnExit);
			this.DoubleBuffered = true;
			this.Name = "FormMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Plumbing Tool";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.TextBox txtJSON;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RichTextBox txtOutput;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ProgressBar progress;
		private System.Windows.Forms.TextBox txtOutputFolder;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtInputFileName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtParametersFileName;
		private System.Windows.Forms.TextBox txtLogFileName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.CheckBox chkbKeepOpen;
	}
}

