namespace RegScoreCalc
{
	partial class FormColumnPropsDateTime
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
			this.cmbTimeFormat = new System.Windows.Forms.ComboBox();
			this.cmbDateFormat = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.rdbDateTime = new System.Windows.Forms.RadioButton();
			this.rdbTimeOnly = new System.Windows.Forms.RadioButton();
			this.rdbDateOnly = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cmbTimeFormat);
			this.groupBox1.Controls.Add(this.cmbDateFormat);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.rdbDateTime);
			this.groupBox1.Controls.Add(this.rdbTimeOnly);
			this.groupBox1.Controls.Add(this.rdbDateOnly);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(259, 321);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Properties";
			// 
			// cmbTimeFormat
			// 
			this.cmbTimeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbTimeFormat.Enabled = false;
			this.cmbTimeFormat.FormattingEnabled = true;
			this.cmbTimeFormat.Items.AddRange(new object[] {
            "HH:mm:ss (24-hour clock)",
            "hh:mm:ss (12-hour clock)",
            "HH:mm  (24-hour clock)",
            "hh:mm  (12-hour clock)",
            "mm:ss"});
			this.cmbTimeFormat.Location = new System.Drawing.Point(12, 183);
			this.cmbTimeFormat.Name = "cmbTimeFormat";
			this.cmbTimeFormat.Size = new System.Drawing.Size(148, 21);
			this.cmbTimeFormat.TabIndex = 6;
			// 
			// cmbDateFormat
			// 
			this.cmbDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDateFormat.FormattingEnabled = true;
			this.cmbDateFormat.Items.AddRange(new object[] {
            "MM/dd/yy",
            "MM/dd/yyyy",
            "MM/dd"});
			this.cmbDateFormat.Location = new System.Drawing.Point(12, 126);
			this.cmbDateFormat.Name = "cmbDateFormat";
			this.cmbDateFormat.Size = new System.Drawing.Size(148, 21);
			this.cmbDateFormat.TabIndex = 4;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 163);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(65, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Time format:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 106);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Date format:";
			// 
			// rdbDateTime
			// 
			this.rdbDateTime.AutoSize = true;
			this.rdbDateTime.Location = new System.Drawing.Point(11, 70);
			this.rdbDateTime.Name = "rdbDateTime";
			this.rdbDateTime.Size = new System.Drawing.Size(91, 17);
			this.rdbDateTime.TabIndex = 2;
			this.rdbDateTime.Text = "Date and time";
			this.rdbDateTime.UseVisualStyleBackColor = true;
			this.rdbDateTime.CheckedChanged += new System.EventHandler(this.rdbDateTime_CheckedChanged);
			// 
			// rdbTimeOnly
			// 
			this.rdbTimeOnly.AutoSize = true;
			this.rdbTimeOnly.Location = new System.Drawing.Point(11, 47);
			this.rdbTimeOnly.Name = "rdbTimeOnly";
			this.rdbTimeOnly.Size = new System.Drawing.Size(70, 17);
			this.rdbTimeOnly.TabIndex = 1;
			this.rdbTimeOnly.Text = "Time only";
			this.rdbTimeOnly.UseVisualStyleBackColor = true;
			this.rdbTimeOnly.CheckedChanged += new System.EventHandler(this.rdbTimeOnly_CheckedChanged);
			// 
			// rdbDateOnly
			// 
			this.rdbDateOnly.AutoSize = true;
			this.rdbDateOnly.Checked = true;
			this.rdbDateOnly.Location = new System.Drawing.Point(11, 24);
			this.rdbDateOnly.Name = "rdbDateOnly";
			this.rdbDateOnly.Size = new System.Drawing.Size(70, 17);
			this.rdbDateOnly.TabIndex = 0;
			this.rdbDateOnly.TabStop = true;
			this.rdbDateOnly.Text = "Date only";
			this.rdbDateOnly.UseVisualStyleBackColor = true;
			this.rdbDateOnly.CheckedChanged += new System.EventHandler(this.rdbDateOnly_CheckedChanged);
			// 
			// FormColumnPropsDateTime
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(259, 321);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormColumnPropsDateTime";
			this.Text = "FormColumnPropsFreeText";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rdbDateOnly;
		private System.Windows.Forms.RadioButton rdbTimeOnly;
		private System.Windows.Forms.RadioButton rdbDateTime;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbDateFormat;
		private System.Windows.Forms.ComboBox cmbTimeFormat;
		private System.Windows.Forms.Label label2;
	}
}