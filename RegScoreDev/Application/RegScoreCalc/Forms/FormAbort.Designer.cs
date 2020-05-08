namespace RegScoreCalc
{
	partial class FormAbort
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
			this.lblMessage = new System.Windows.Forms.Label();
			this.btnAbort = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.picture = new System.Windows.Forms.PictureBox();
			this.btnContinue = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picture)).BeginInit();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblMessage.Location = new System.Drawing.Point(79, 16);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(352, 64);
			this.lblMessage.TabIndex = 0;
			this.lblMessage.Text = "Message";
			// 
			// btnAbort
			// 
			this.btnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAbort.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btnAbort.Location = new System.Drawing.Point(356, 108);
			this.btnAbort.Name = "btnAbort";
			this.btnAbort.Size = new System.Drawing.Size(75, 23);
			this.btnAbort.TabIndex = 1;
			this.btnAbort.Text = "Abort";
			this.btnAbort.UseVisualStyleBackColor = true;
			this.btnAbort.Click += new System.EventHandler(this.btnAbort_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-79, 96);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(600, 2);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// picture
			// 
			this.picture.Image = global::RegScoreCalc.Properties.Resources.Error;
			this.picture.Location = new System.Drawing.Point(24, 32);
			this.picture.Name = "picture";
			this.picture.Size = new System.Drawing.Size(32, 32);
			this.picture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picture.TabIndex = 3;
			this.picture.TabStop = false;
			// 
			// btnContinue
			// 
			this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnContinue.Location = new System.Drawing.Point(275, 108);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(75, 23);
			this.btnContinue.TabIndex = 1;
			this.btnContinue.Text = "Continue";
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			// 
			// FormAbort
			// 
			this.AcceptButton = this.btnContinue;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnAbort;
			this.ClientSize = new System.Drawing.Size(443, 143);
			this.Controls.Add(this.picture);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.btnAbort);
			this.Controls.Add(this.lblMessage);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAbort";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "An error occurred";
			((System.ComponentModel.ISupportInitialize)(this.picture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Button btnAbort;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox picture;
		private System.Windows.Forms.Button btnContinue;
	}
}