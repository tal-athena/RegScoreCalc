namespace RegScoreCalc
{
	partial class FormSplashScreen
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
			this.picSplashScreen = new System.Windows.Forms.PictureBox();
			this.lblAssemblyDescription = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.picSplashScreen)).BeginInit();
			this.SuspendLayout();
			// 
			// picSplashScreen
			// 
			this.picSplashScreen.Image = global::RegScoreCalc.Properties.Resources.Splash_screen;
			this.picSplashScreen.Location = new System.Drawing.Point(0, 0);
			this.picSplashScreen.Name = "picSplashScreen";
			this.picSplashScreen.Size = new System.Drawing.Size(620, 350);
			this.picSplashScreen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.picSplashScreen.TabIndex = 0;
			this.picSplashScreen.TabStop = false;
			// 
			// lblAssemblyDescription
			// 
			this.lblAssemblyDescription.AutoSize = true;
			this.lblAssemblyDescription.BackColor = System.Drawing.Color.Transparent;
			this.lblAssemblyDescription.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblAssemblyDescription.ForeColor = System.Drawing.Color.SteelBlue;
			this.lblAssemblyDescription.Location = new System.Drawing.Point(12, 332);
			this.lblAssemblyDescription.Name = "lblAssemblyDescription";
			this.lblAssemblyDescription.Size = new System.Drawing.Size(172, 19);
			this.lblAssemblyDescription.TabIndex = 1;
			this.lblAssemblyDescription.Text = "AssemblyDescription";
			// 
			// FormSplashScreen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(620, 350);
			this.ControlBox = false;
			this.Controls.Add(this.lblAssemblyDescription);
			this.Controls.Add(this.picSplashScreen);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormSplashScreen";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.FormSplashScreen_Load);
			((System.ComponentModel.ISupportInitialize)(this.picSplashScreen)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picSplashScreen;
		private System.Windows.Forms.Label lblAssemblyDescription;
	}
}