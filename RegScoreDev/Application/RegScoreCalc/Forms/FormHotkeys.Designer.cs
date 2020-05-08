namespace RegScoreCalc.Forms
{
	partial class FormHotkeys
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
			this.btnCancel = new RibbonStyleButton();
			this.btnOK = new RibbonStyleButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtEditValue = new RegScoreCalc.HotkeyControl();
			this.txtSelectCategory = new RegScoreCalc.HotkeyControl();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(165, 85);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(84, 85);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-53, 72);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(357, 2);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select Category:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(78, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Edit Cell Value:";
			// 
			// txtEditValue
			// 
			this.txtEditValue.Hotkey = System.Windows.Forms.Keys.None;
			this.txtEditValue.HotkeyModifiers = System.Windows.Forms.Keys.None;
			this.txtEditValue.Location = new System.Drawing.Point(103, 38);
			this.txtEditValue.Name = "txtEditValue";
			this.txtEditValue.Size = new System.Drawing.Size(136, 20);
			this.txtEditValue.TabIndex = 3;
			this.txtEditValue.Text = "None";
			// 
			// txtSelectCategory
			// 
			this.txtSelectCategory.Hotkey = System.Windows.Forms.Keys.None;
			this.txtSelectCategory.HotkeyModifiers = System.Windows.Forms.Keys.None;
			this.txtSelectCategory.Location = new System.Drawing.Point(103, 12);
			this.txtSelectCategory.Name = "txtSelectCategory";
			this.txtSelectCategory.Size = new System.Drawing.Size(136, 20);
			this.txtSelectCategory.TabIndex = 1;
			this.txtSelectCategory.Text = "None";
			// 
			// FormHotkeys
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(252, 120);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtEditValue);
			this.Controls.Add(this.txtSelectCategory);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormHotkeys";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Hotkeys";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHotkeys_FormClosing);
			this.Load += new System.EventHandler(this.FormHotkeys_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private HotkeyControl txtSelectCategory;
		private System.Windows.Forms.Label label1;
		private HotkeyControl txtEditValue;
		private System.Windows.Forms.Label label2;
	}
}