namespace RegScoreCalc
{
	partial class FormProgress
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
			this.progOperation = new System.Windows.Forms.ProgressBar();
			this.btnStop = new System.Windows.Forms.Button();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.SuspendLayout();
			// 
			// progOperation
			// 
			this.progOperation.Location = new System.Drawing.Point(12, 12);
			this.progOperation.Name = "progOperation";
			this.progOperation.Size = new System.Drawing.Size(337, 23);
			this.progOperation.Step = 1;
			this.progOperation.TabIndex = 1;
			// 
			// btnStop
			// 
			this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnStop.Location = new System.Drawing.Point(355, 12);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 2;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.WorkerSupportsCancellation = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
			// 
			// FormProgress
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnStop;
			this.ClientSize = new System.Drawing.Size(442, 49);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.progOperation);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormProgress";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Operation Progress";
			this.Load += new System.EventHandler(this.FormProgress_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar progOperation;
		private System.Windows.Forms.Button btnStop;
		private System.ComponentModel.BackgroundWorker worker;

	}
}