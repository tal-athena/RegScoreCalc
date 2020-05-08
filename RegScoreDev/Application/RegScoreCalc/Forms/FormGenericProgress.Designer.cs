namespace RegScoreCalc
{
	partial class FormGenericProgress
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
			this.progress = new System.Windows.Forms.ProgressBar();
			this.btnStop = new RegScoreCalc.RibbonStyleButton();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblElapsed = new System.Windows.Forms.Label();
			this.lblLeft = new System.Windows.Forms.Label();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.lblProgress = new System.Windows.Forms.Label();
			this.groupLog = new System.Windows.Forms.GroupBox();
			this.gridLog = new System.Windows.Forms.DataGridView();
			this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.progressInd = new RegScoreCalc.IndProgressBar();
			this.groupLog.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridLog)).BeginInit();
			this.SuspendLayout();
			// 
			// progress
			// 
			this.progress.Location = new System.Drawing.Point(12, 42);
			this.progress.Name = "progress";
			this.progress.Size = new System.Drawing.Size(396, 23);
			this.progress.Step = 0;
			this.progress.TabIndex = 5;
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.BackColor = System.Drawing.Color.Transparent;
			this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnStop.HoverImage = null;
			this.btnStop.Location = new System.Drawing.Point(421, 42);
			this.btnStop.Name = "btnStop";
			this.btnStop.NormalImage = null;
			this.btnStop.PressedImage = null;
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 7;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(12, 81);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(86, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Time elapsed:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.Location = new System.Drawing.Point(272, 81);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Time left:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(12, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Progress:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblElapsed
			// 
			this.lblElapsed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblElapsed.Location = new System.Drawing.Point(104, 77);
			this.lblElapsed.Name = "lblElapsed";
			this.lblElapsed.Size = new System.Drawing.Size(59, 20);
			this.lblElapsed.TabIndex = 1;
			this.lblElapsed.Text = "00:00:00";
			this.lblElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblLeft
			// 
			this.lblLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLeft.Location = new System.Drawing.Point(338, 77);
			this.lblLeft.Name = "lblLeft";
			this.lblLeft.Size = new System.Drawing.Size(70, 20);
			this.lblLeft.TabIndex = 3;
			this.lblLeft.Text = "estimating";
			this.lblLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
			// 
			// lblProgress
			// 
			this.lblProgress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblProgress.Location = new System.Drawing.Point(104, 9);
			this.lblProgress.Name = "lblProgress";
			this.lblProgress.Size = new System.Drawing.Size(304, 20);
			this.lblProgress.TabIndex = 3;
			this.lblProgress.Text = "estimating";
			this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupLog
			// 
			this.groupLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupLog.Controls.Add(this.gridLog);
			this.groupLog.Location = new System.Drawing.Point(12, 109);
			this.groupLog.Name = "groupLog";
			this.groupLog.Size = new System.Drawing.Size(492, 3);
			this.groupLog.TabIndex = 8;
			this.groupLog.TabStop = false;
			this.groupLog.Text = "Log";
			this.groupLog.Visible = false;
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
			this.gridLog.Location = new System.Drawing.Point(6, 19);
			this.gridLog.Name = "gridLog";
			this.gridLog.ReadOnly = true;
			this.gridLog.RowHeadersVisible = false;
			this.gridLog.Size = new System.Drawing.Size(480, 0);
			this.gridLog.TabIndex = 0;
			// 
			// colMessage
			// 
			this.colMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colMessage.HeaderText = "Message";
			this.colMessage.Name = "colMessage";
			this.colMessage.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn1.HeaderText = "Messagr";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			// 
			// progressInd
			// 
			this.progressInd.Location = new System.Drawing.Point(12, 42);
			this.progressInd.Name = "progressInd";
			this.progressInd.Size = new System.Drawing.Size(396, 23);
			this.progressInd.Step = 0;
			this.progressInd.TabIndex = 5;
			this.progressInd.Visible = false;
			// 
			// FormGenericProgress
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnStop;
			this.ClientSize = new System.Drawing.Size(516, 110);
			this.ControlBox = false;
			this.Controls.Add(this.groupLog);
			this.Controls.Add(this.lblProgress);
			this.Controls.Add(this.lblLeft);
			this.Controls.Add(this.lblElapsed);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.progress);
			this.Controls.Add(this.progressInd);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormGenericProgress";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Operation";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGenericProgress_FormClosing);
			this.Load += new System.EventHandler(this.FormGenericProgress_Load);
			this.groupLog.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridLog)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ProgressBar progress;
		private RibbonStyleButton btnStop;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblElapsed;
		private System.Windows.Forms.Label lblLeft;
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.Label lblProgress;
		private IndProgressBar progressInd;
		private System.Windows.Forms.GroupBox groupLog;
		private System.Windows.Forms.DataGridView gridLog;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
	}
}