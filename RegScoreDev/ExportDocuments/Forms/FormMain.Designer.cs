namespace ExportDocuments.Forms
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
			this.txtInput = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtOutput = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnBrowseInput = new System.Windows.Forms.Button();
			this.btnBrowseOutput = new System.Windows.Forms.Button();
			this.gridLog = new System.Windows.Forms.DataGridView();
			this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnStart = new System.Windows.Forms.Button();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.chkbUseTransaction = new System.Windows.Forms.CheckBox();
			this.chkbShowWarnings = new System.Windows.Forms.CheckBox();
			this.lblTimeElapsedLabel = new System.Windows.Forms.Label();
			this.lblTimeElapsed = new System.Windows.Forms.Label();
			this.lblTotalRowsProcessedLabel = new System.Windows.Forms.Label();
			this.lblTotalRowsProcessed = new System.Windows.Forms.Label();
			this.lblProcessingSpeed = new System.Windows.Forms.Label();
			this.lblProcessingSpeedLabel = new System.Windows.Forms.Label();
			this.chkbDeleteDocuments = new System.Windows.Forms.CheckBox();
			this.lblTimeRemainingLabel = new System.Windows.Forms.Label();
			this.lblTimeRemaining = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
			this.SuspendLayout();
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(573, 377);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 20;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// txtInput
			// 
			this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtInput.Location = new System.Drawing.Point(107, 12);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(504, 20);
			this.txtInput.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Input database:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-20, 365);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(700, 2);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// txtOutput
			// 
			this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutput.Location = new System.Drawing.Point(107, 41);
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.Size = new System.Drawing.Size(504, 20);
			this.txtOutput.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Output database:";
			// 
			// btnBrowseInput
			// 
			this.btnBrowseInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseInput.Location = new System.Drawing.Point(617, 10);
			this.btnBrowseInput.Name = "btnBrowseInput";
			this.btnBrowseInput.Size = new System.Drawing.Size(31, 23);
			this.btnBrowseInput.TabIndex = 2;
			this.btnBrowseInput.Text = "...";
			this.btnBrowseInput.UseVisualStyleBackColor = true;
			this.btnBrowseInput.Click += new System.EventHandler(this.btnBrowseInput_Click);
			// 
			// btnBrowseOutput
			// 
			this.btnBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseOutput.Location = new System.Drawing.Point(617, 39);
			this.btnBrowseOutput.Name = "btnBrowseOutput";
			this.btnBrowseOutput.Size = new System.Drawing.Size(31, 23);
			this.btnBrowseOutput.TabIndex = 5;
			this.btnBrowseOutput.Text = "...";
			this.btnBrowseOutput.UseVisualStyleBackColor = true;
			this.btnBrowseOutput.Click += new System.EventHandler(this.btnBrowseOutput_Click);
			// 
			// gridLog
			// 
			this.gridLog.AllowUserToAddRows = false;
			this.gridLog.AllowUserToDeleteRows = false;
			this.gridLog.AllowUserToResizeRows = false;
			this.gridLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridLog.BackgroundColor = System.Drawing.Color.White;
			this.gridLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMessage});
			this.gridLog.Location = new System.Drawing.Point(12, 105);
			this.gridLog.Name = "gridLog";
			this.gridLog.ReadOnly = true;
			this.gridLog.RowHeadersVisible = false;
			this.gridLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridLog.Size = new System.Drawing.Size(636, 185);
			this.gridLog.TabIndex = 7;
			// 
			// colMessage
			// 
			this.colMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colMessage.HeaderText = "Message";
			this.colMessage.Name = "colMessage";
			this.colMessage.ReadOnly = true;
			this.colMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// btnStart
			// 
			this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStart.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnStart.Location = new System.Drawing.Point(492, 377);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 19;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.progressBar.Location = new System.Drawing.Point(12, 377);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(327, 23);
			this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar.TabIndex = 18;
			// 
			// chkbUseTransaction
			// 
			this.chkbUseTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkbUseTransaction.AutoSize = true;
			this.chkbUseTransaction.Checked = true;
			this.chkbUseTransaction.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkbUseTransaction.Location = new System.Drawing.Point(15, 381);
			this.chkbUseTransaction.Name = "chkbUseTransaction";
			this.chkbUseTransaction.Size = new System.Drawing.Size(100, 17);
			this.chkbUseTransaction.TabIndex = 15;
			this.chkbUseTransaction.Text = "Use transaction";
			this.chkbUseTransaction.UseVisualStyleBackColor = true;
			// 
			// chkbShowWarnings
			// 
			this.chkbShowWarnings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkbShowWarnings.AutoSize = true;
			this.chkbShowWarnings.Location = new System.Drawing.Point(352, 381);
			this.chkbShowWarnings.Name = "chkbShowWarnings";
			this.chkbShowWarnings.Size = new System.Drawing.Size(98, 17);
			this.chkbShowWarnings.TabIndex = 17;
			this.chkbShowWarnings.Text = "Show warnings";
			this.chkbShowWarnings.UseVisualStyleBackColor = true;
			this.chkbShowWarnings.CheckedChanged += new System.EventHandler(this.chkbShowWarnings_CheckedChanged);
			// 
			// lblTimeElapsedLabel
			// 
			this.lblTimeElapsedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTimeElapsedLabel.AutoSize = true;
			this.lblTimeElapsedLabel.Location = new System.Drawing.Point(12, 306);
			this.lblTimeElapsedLabel.Name = "lblTimeElapsedLabel";
			this.lblTimeElapsedLabel.Size = new System.Drawing.Size(73, 13);
			this.lblTimeElapsedLabel.TabIndex = 7;
			this.lblTimeElapsedLabel.Text = "Time elapsed:";
			this.lblTimeElapsedLabel.Visible = false;
			// 
			// lblTimeElapsed
			// 
			this.lblTimeElapsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTimeElapsed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblTimeElapsed.Location = new System.Drawing.Point(97, 304);
			this.lblTimeElapsed.Name = "lblTimeElapsed";
			this.lblTimeElapsed.Size = new System.Drawing.Size(80, 17);
			this.lblTimeElapsed.TabIndex = 8;
			this.lblTimeElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblTimeElapsed.Visible = false;
			// 
			// lblTotalRowsProcessedLabel
			// 
			this.lblTotalRowsProcessedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTotalRowsProcessedLabel.AutoSize = true;
			this.lblTotalRowsProcessedLabel.Location = new System.Drawing.Point(228, 306);
			this.lblTotalRowsProcessedLabel.Name = "lblTotalRowsProcessedLabel";
			this.lblTotalRowsProcessedLabel.Size = new System.Drawing.Size(91, 13);
			this.lblTotalRowsProcessedLabel.TabIndex = 11;
			this.lblTotalRowsProcessedLabel.Text = "Total rows copied";
			this.lblTotalRowsProcessedLabel.Visible = false;
			// 
			// lblTotalRowsProcessed
			// 
			this.lblTotalRowsProcessed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTotalRowsProcessed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblTotalRowsProcessed.Location = new System.Drawing.Point(327, 304);
			this.lblTotalRowsProcessed.Name = "lblTotalRowsProcessed";
			this.lblTotalRowsProcessed.Size = new System.Drawing.Size(80, 17);
			this.lblTotalRowsProcessed.TabIndex = 12;
			this.lblTotalRowsProcessed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblTotalRowsProcessed.Visible = false;
			// 
			// lblProcessingSpeed
			// 
			this.lblProcessingSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProcessingSpeed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblProcessingSpeed.Location = new System.Drawing.Point(327, 334);
			this.lblProcessingSpeed.Name = "lblProcessingSpeed";
			this.lblProcessingSpeed.Size = new System.Drawing.Size(80, 17);
			this.lblProcessingSpeed.TabIndex = 14;
			this.lblProcessingSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblProcessingSpeed.Visible = false;
			// 
			// lblProcessingSpeedLabel
			// 
			this.lblProcessingSpeedLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblProcessingSpeedLabel.AutoSize = true;
			this.lblProcessingSpeedLabel.Location = new System.Drawing.Point(228, 336);
			this.lblProcessingSpeedLabel.Name = "lblProcessingSpeedLabel";
			this.lblProcessingSpeedLabel.Size = new System.Drawing.Size(93, 13);
			this.lblProcessingSpeedLabel.TabIndex = 13;
			this.lblProcessingSpeedLabel.Text = "Rows per second:";
			this.lblProcessingSpeedLabel.Visible = false;
			// 
			// chkbDeleteDocuments
			// 
			this.chkbDeleteDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chkbDeleteDocuments.AutoSize = true;
			this.chkbDeleteDocuments.Checked = true;
			this.chkbDeleteDocuments.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkbDeleteDocuments.Location = new System.Drawing.Point(152, 381);
			this.chkbDeleteDocuments.Name = "chkbDeleteDocuments";
			this.chkbDeleteDocuments.Size = new System.Drawing.Size(163, 17);
			this.chkbDeleteDocuments.TabIndex = 16;
			this.chkbDeleteDocuments.Text = "Delete all existing documents";
			this.chkbDeleteDocuments.UseVisualStyleBackColor = true;
			// 
			// lblTimeRemainingLabel
			// 
			this.lblTimeRemainingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTimeRemainingLabel.AutoSize = true;
			this.lblTimeRemainingLabel.Location = new System.Drawing.Point(12, 336);
			this.lblTimeRemainingLabel.Name = "lblTimeRemainingLabel";
			this.lblTimeRemainingLabel.Size = new System.Drawing.Size(81, 13);
			this.lblTimeRemainingLabel.TabIndex = 9;
			this.lblTimeRemainingLabel.Text = "Time remaining:";
			this.lblTimeRemainingLabel.Visible = false;
			// 
			// lblTimeRemaining
			// 
			this.lblTimeRemaining.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblTimeRemaining.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblTimeRemaining.Location = new System.Drawing.Point(97, 334);
			this.lblTimeRemaining.Name = "lblTimeRemaining";
			this.lblTimeRemaining.Size = new System.Drawing.Size(80, 17);
			this.lblTimeRemaining.TabIndex = 10;
			this.lblTimeRemaining.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lblTimeRemaining.Visible = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 21;
			this.label3.Text = "Password:";
			// 
			// txtPassword
			// 
			this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPassword.Location = new System.Drawing.Point(107, 67);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(133, 20);
			this.txtPassword.TabIndex = 6;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// FormMain
			// 
			this.AcceptButton = this.btnStart;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(660, 412);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.chkbDeleteDocuments);
			this.Controls.Add(this.lblProcessingSpeed);
			this.Controls.Add(this.lblProcessingSpeedLabel);
			this.Controls.Add(this.lblTotalRowsProcessed);
			this.Controls.Add(this.lblTotalRowsProcessedLabel);
			this.Controls.Add(this.lblTimeRemaining);
			this.Controls.Add(this.lblTimeRemainingLabel);
			this.Controls.Add(this.lblTimeElapsed);
			this.Controls.Add(this.lblTimeElapsedLabel);
			this.Controls.Add(this.chkbShowWarnings);
			this.Controls.Add(this.chkbUseTransaction);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.gridLog);
			this.Controls.Add(this.btnBrowseOutput);
			this.Controls.Add(this.btnBrowseInput);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtOutput);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnExit);
			this.MinimumSize = new System.Drawing.Size(676, 451);
			this.Name = "FormMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Export Documents";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridLog)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.TextBox txtInput;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtOutput;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnBrowseInput;
		private System.Windows.Forms.Button btnBrowseOutput;
		private System.Windows.Forms.DataGridView gridLog;
		private System.Windows.Forms.Button btnStart;
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.CheckBox chkbUseTransaction;
		private System.Windows.Forms.CheckBox chkbShowWarnings;
		private System.Windows.Forms.Label lblTimeElapsedLabel;
		private System.Windows.Forms.Label lblTimeElapsed;
		private System.Windows.Forms.Label lblTotalRowsProcessedLabel;
		private System.Windows.Forms.Label lblTotalRowsProcessed;
		private System.Windows.Forms.Label lblProcessingSpeed;
		private System.Windows.Forms.Label lblProcessingSpeedLabel;
		private System.Windows.Forms.CheckBox chkbDeleteDocuments;
		private System.Windows.Forms.Label lblTimeRemainingLabel;
		private System.Windows.Forms.Label lblTimeRemaining;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtPassword;
	}
}

