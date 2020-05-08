namespace RegScoreCalc
{
	partial class FormColumnPropsNumeric
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
			this.txtMaximum = new RegScoreCalc.NumericTextBox();
			this.txtMinimum = new RegScoreCalc.NumericTextBox();
			this.txtDecimalPlaces = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.rdbDecimal = new System.Windows.Forms.RadioButton();
			this.rdbInteger = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtMaximum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtMinimum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDecimalPlaces)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.txtMaximum);
			this.groupBox1.Controls.Add(this.txtMinimum);
			this.groupBox1.Controls.Add(this.txtDecimalPlaces);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.rdbDecimal);
			this.groupBox1.Controls.Add(this.rdbInteger);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(259, 321);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Properties";
			// 
			// txtMaximum
			// 
			this.txtMaximum.AllowSpace = false;
			this.txtMaximum.Location = new System.Drawing.Point(96, 109);
			this.txtMaximum.Name = "txtMaximum";
			this.txtMaximum.Size = new System.Drawing.Size(84, 20);
			this.txtMaximum.TabIndex = 5;
			this.txtMaximum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// txtMinimum
			// 
			this.txtMinimum.AllowSpace = false;
			this.txtMinimum.Location = new System.Drawing.Point(96, 83);
			this.txtMinimum.Name = "txtMinimum";
			this.txtMinimum.Size = new System.Drawing.Size(84, 20);
			this.txtMinimum.TabIndex = 3;
			this.txtMinimum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// txtDecimalPlaces
			// 
			this.txtDecimalPlaces.Enabled = false;
			this.txtDecimalPlaces.Location = new System.Drawing.Point(96, 135);
			this.txtDecimalPlaces.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.txtDecimalPlaces.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.txtDecimalPlaces.Name = "txtDecimalPlaces";
			this.txtDecimalPlaces.Size = new System.Drawing.Size(51, 20);
			this.txtDecimalPlaces.TabIndex = 7;
			this.txtDecimalPlaces.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtDecimalPlaces.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.txtDecimalPlaces.ValueChanged += new System.EventHandler(this.txtDecimalPlaces_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 137);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Decimal places:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Maximum:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 86);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Minimum:";
			// 
			// rdbDecimal
			// 
			this.rdbDecimal.AutoSize = true;
			this.rdbDecimal.Location = new System.Drawing.Point(11, 47);
			this.rdbDecimal.Name = "rdbDecimal";
			this.rdbDecimal.Size = new System.Drawing.Size(63, 17);
			this.rdbDecimal.TabIndex = 1;
			this.rdbDecimal.Text = "Decimal";
			this.rdbDecimal.UseVisualStyleBackColor = true;
			this.rdbDecimal.CheckedChanged += new System.EventHandler(this.rdbDecimal_CheckedChanged);
			// 
			// rdbInteger
			// 
			this.rdbInteger.AutoSize = true;
			this.rdbInteger.Checked = true;
			this.rdbInteger.Location = new System.Drawing.Point(11, 24);
			this.rdbInteger.Name = "rdbInteger";
			this.rdbInteger.Size = new System.Drawing.Size(58, 17);
			this.rdbInteger.TabIndex = 0;
			this.rdbInteger.TabStop = true;
			this.rdbInteger.Text = "Integer";
			this.rdbInteger.UseVisualStyleBackColor = true;
			this.rdbInteger.CheckedChanged += new System.EventHandler(this.rdbInteger_CheckedChanged);
			// 
			// FormColumnPropsNumeric
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(259, 321);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "FormColumnPropsNumeric";
			this.Text = "FormColumnPropsFreeText";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtMaximum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtMinimum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDecimalPlaces)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rdbDecimal;
		private System.Windows.Forms.RadioButton rdbInteger;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown txtDecimalPlaces;
		private System.Windows.Forms.Label label3;
		private NumericTextBox txtMaximum;
		private NumericTextBox txtMinimum;
	}
}