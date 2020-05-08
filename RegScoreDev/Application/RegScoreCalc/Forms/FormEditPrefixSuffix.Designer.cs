namespace RegScoreCalc
{
	partial class FormEditPrefixSuffix
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lbSuffixValues = new System.Windows.Forms.ListBox();
			this.btnCopySuffixValue = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.txtSuffixValue = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.txtPrefixValue = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.lbPrefixValues = new System.Windows.Forms.ListBox();
			this.btnCopyPrefixValue = new System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-16, 340);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(696, 2);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(577, 353);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(496, 352);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lbSuffixValues
			// 
			this.lbSuffixValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbSuffixValues.FormattingEnabled = true;
			this.lbSuffixValues.IntegralHeight = false;
			this.lbSuffixValues.Location = new System.Drawing.Point(14, 57);
			this.lbSuffixValues.Name = "lbSuffixValues";
			this.lbSuffixValues.Size = new System.Drawing.Size(289, 215);
			this.lbSuffixValues.TabIndex = 2;
			this.lbSuffixValues.SelectedIndexChanged += new System.EventHandler(this.lbSuffixValues_SelectedIndexChanged);
			// 
			// btnCopySuffixValue
			// 
			this.btnCopySuffixValue.Location = new System.Drawing.Point(14, 278);
			this.btnCopySuffixValue.Name = "btnCopySuffixValue";
			this.btnCopySuffixValue.Size = new System.Drawing.Size(132, 23);
			this.btnCopySuffixValue.TabIndex = 3;
			this.btnCopySuffixValue.Text = "Copy Selected Value";
			this.btnCopySuffixValue.UseVisualStyleBackColor = true;
			this.btnCopySuffixValue.Click += new System.EventHandler(this.btnCopySuffixValue_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.txtSuffixValue);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.lbSuffixValues);
			this.groupBox2.Controls.Add(this.btnCopySuffixValue);
			this.groupBox2.Location = new System.Drawing.Point(335, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(317, 313);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Suffix";
			// 
			// txtSuffixValue
			// 
			this.txtSuffixValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSuffixValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtSuffixValue.Location = new System.Drawing.Point(57, 24);
			this.txtSuffixValue.Name = "txtSuffixValue";
			this.txtSuffixValue.Size = new System.Drawing.Size(246, 20);
			this.txtSuffixValue.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 27);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(37, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Value:";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox3.Controls.Add(this.txtPrefixValue);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Controls.Add(this.lbPrefixValues);
			this.groupBox3.Controls.Add(this.btnCopyPrefixValue);
			this.groupBox3.Location = new System.Drawing.Point(12, 12);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(317, 313);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Prefix";
			// 
			// txtPrefixValue
			// 
			this.txtPrefixValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtPrefixValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtPrefixValue.Location = new System.Drawing.Point(57, 24);
			this.txtPrefixValue.Name = "txtPrefixValue";
			this.txtPrefixValue.Size = new System.Drawing.Size(246, 20);
			this.txtPrefixValue.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 27);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(37, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Value:";
			// 
			// lbPrefixValues
			// 
			this.lbPrefixValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbPrefixValues.FormattingEnabled = true;
			this.lbPrefixValues.IntegralHeight = false;
			this.lbPrefixValues.Location = new System.Drawing.Point(14, 57);
			this.lbPrefixValues.Name = "lbPrefixValues";
			this.lbPrefixValues.Size = new System.Drawing.Size(289, 215);
			this.lbPrefixValues.TabIndex = 2;
			this.lbPrefixValues.SelectedIndexChanged += new System.EventHandler(this.lbPrefixValues_SelectedIndexChanged);
			// 
			// btnCopyPrefixValue
			// 
			this.btnCopyPrefixValue.Location = new System.Drawing.Point(14, 278);
			this.btnCopyPrefixValue.Name = "btnCopyPrefixValue";
			this.btnCopyPrefixValue.Size = new System.Drawing.Size(132, 23);
			this.btnCopyPrefixValue.TabIndex = 3;
			this.btnCopyPrefixValue.Text = "Copy Selected Value";
			this.btnCopyPrefixValue.UseVisualStyleBackColor = true;
			this.btnCopyPrefixValue.Click += new System.EventHandler(this.btnCopyPrefixValue_Click);
			// 
			// FormEditPrefixSuffix
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(664, 388);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEditPrefixSuffix";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit regular suffix and prefix";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditPrefixSuffix_FormClosing);
			this.Load += new System.EventHandler(this.FormEditPrefixSuffix_Load);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ListBox lbSuffixValues;
		private System.Windows.Forms.Button btnCopySuffixValue;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtSuffixValue;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TextBox txtPrefixValue;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbPrefixValues;
		private System.Windows.Forms.Button btnCopyPrefixValue;
	}
}