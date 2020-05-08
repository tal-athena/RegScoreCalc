
namespace RegScoreCalc
{
	partial class MatchEditingControl
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
			this.btnEdit = new RegScoreCalc.ImageButton();
			this.SuspendLayout();
			// 
			// btnEdit
			// 
			this.btnEdit.HoverImage = global::RegScoreCalc.Properties.Resources.edit_m_over_new;
			this.btnEdit.Location = new System.Drawing.Point(1, 1);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.NormalImage = global::RegScoreCalc.Properties.Resources.edit_normal_new;
			this.btnEdit.PressedImage = global::RegScoreCalc.Properties.Resources.edit_pressed_new;
			this.btnEdit.Size = new System.Drawing.Size(26, 19);
			this.btnEdit.TabIndex = 1;
			this.btnEdit.Text = "Edit...";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// MatchEditingControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.btnEdit);
			this.Name = "MatchEditingControl";
			this.Size = new System.Drawing.Size(28, 19);
			this.Load += new System.EventHandler(this.MatchEditingControl_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private ImageButton btnEdit;
	}
}
