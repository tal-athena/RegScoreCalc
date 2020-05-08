namespace DRTAccessFileSetup.Forms
{
	partial class FormSelectDatabase
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
			this.lbHistory = new System.Windows.Forms.ListBox();
			this.btnExit = new System.Windows.Forms.Button();
			this.btnOpenSelected = new System.Windows.Forms.Button();
			this.btnSelectFile = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbHistory
			// 
			this.lbHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbHistory.FormattingEnabled = true;
			this.lbHistory.IntegralHeight = false;
			this.lbHistory.Location = new System.Drawing.Point(12, 45);
			this.lbHistory.Name = "lbHistory";
			this.lbHistory.Size = new System.Drawing.Size(527, 255);
			this.lbHistory.TabIndex = 2;
			this.lbHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbHistory_MouseDoubleClick);
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Location = new System.Drawing.Point(464, 309);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 4;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			// 
			// btnOpenSelected
			// 
			this.btnOpenSelected.Location = new System.Drawing.Point(12, 12);
			this.btnOpenSelected.Name = "btnOpenSelected";
			this.btnOpenSelected.Size = new System.Drawing.Size(128, 23);
			this.btnOpenSelected.TabIndex = 0;
			this.btnOpenSelected.Text = "Open Selected";
			this.btnOpenSelected.UseVisualStyleBackColor = true;
			this.btnOpenSelected.Click += new System.EventHandler(this.btnOpenSelected_Click);
			// 
			// btnSelectFile
			// 
			this.btnSelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectFile.Location = new System.Drawing.Point(411, 12);
			this.btnSelectFile.Name = "btnSelectFile";
			this.btnSelectFile.Size = new System.Drawing.Size(128, 23);
			this.btnSelectFile.TabIndex = 1;
			this.btnSelectFile.Text = "Open File from Disk";
			this.btnSelectFile.UseVisualStyleBackColor = true;
			this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClear.Location = new System.Drawing.Point(12, 309);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 3;
			this.btnClear.Text = "Clear History";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// FormSelectDatabase
			// 
			this.AcceptButton = this.btnOpenSelected;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(551, 344);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnSelectFile);
			this.Controls.Add(this.btnOpenSelected);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.lbHistory);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSelectDatabase";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select Database";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSelectDatabase_FormClosing);
			this.Load += new System.EventHandler(this.FormSelectDatabase_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox lbHistory;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Button btnOpenSelected;
		private System.Windows.Forms.Button btnSelectFile;
		private System.Windows.Forms.Button btnClear;
	}
}