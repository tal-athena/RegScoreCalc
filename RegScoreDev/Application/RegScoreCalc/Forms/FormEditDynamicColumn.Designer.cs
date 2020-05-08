namespace RegScoreCalc
{
	partial class FormEditDynamicColumn
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
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnOK = new RegScoreCalc.RibbonStyleButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblTitle = new System.Windows.Forms.Label();
			this.cmbType = new System.Windows.Forms.ComboBox();
			this.lblType = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.DrawNormalBorder = false;
			this.btnCancel.HoverImage = null;
			this.btnCancel.IsHighlighted = false;
			this.btnCancel.Location = new System.Drawing.Point(203, 85);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.DrawNormalBorder = false;
			this.btnOK.HoverImage = null;
			this.btnOK.IsHighlighted = false;
			this.btnOK.Location = new System.Drawing.Point(122, 85);
			this.btnOK.Name = "btnOK";
			this.btnOK.NormalImage = null;
			this.btnOK.PressedImage = null;
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-27, 74);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(344, 2);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// txtName
			// 
			this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtName.Location = new System.Drawing.Point(56, 12);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(222, 20);
			this.txtName.TabIndex = 0;
			// 
			// lblTitle
			// 
			this.lblTitle.AutoSize = true;
			this.lblTitle.Location = new System.Drawing.Point(12, 15);
			this.lblTitle.Name = "lblTitle";
			this.lblTitle.Size = new System.Drawing.Size(38, 13);
			this.lblTitle.TabIndex = 0;
			this.lblTitle.Text = "Name:";
			// 
			// cmbType
			// 
			this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbType.FormattingEnabled = true;
			this.cmbType.Items.AddRange(new object[] {
            "Category",
            "Free text",
            "Numeric",
            "Date/time"});
			this.cmbType.Location = new System.Drawing.Point(56, 41);
			this.cmbType.Name = "cmbType";
			this.cmbType.Size = new System.Drawing.Size(141, 21);
			this.cmbType.TabIndex = 1;
			// 
			// lblType
			// 
			this.lblType.AutoSize = true;
			this.lblType.Location = new System.Drawing.Point(12, 44);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(34, 13);
			this.lblType.TabIndex = 0;
			this.lblType.Text = "Type:";
			// 
			// FormEditDynamicColumn
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(290, 120);
			this.Controls.Add(this.cmbType);
			this.Controls.Add(this.lblType);
			this.Controls.Add(this.lblTitle);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEditDynamicColumn";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Column";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditCategoryClass_FormClosing);
			this.Load += new System.EventHandler(this.FormEditDynamicColumn_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblTitle;
		public System.Windows.Forms.ComboBox cmbType;
		public System.Windows.Forms.Label lblType;
	}
}