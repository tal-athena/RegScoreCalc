namespace WebAppTest.Action_Controls
{
    partial class LogoutControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.lblLogoutMessage = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblLogoutMessage
			// 
			this.lblLogoutMessage.AutoSize = true;
			this.lblLogoutMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblLogoutMessage.Location = new System.Drawing.Point(9, 8);
			this.lblLogoutMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblLogoutMessage.Name = "lblLogoutMessage";
			this.lblLogoutMessage.Size = new System.Drawing.Size(93, 13);
			this.lblLogoutMessage.TabIndex = 0;
			this.lblLogoutMessage.Text = "I promise to logout";
			// 
			// LogoutControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblLogoutMessage);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "LogoutControl";
			this.Size = new System.Drawing.Size(265, 38);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLogoutMessage;
    }
}
