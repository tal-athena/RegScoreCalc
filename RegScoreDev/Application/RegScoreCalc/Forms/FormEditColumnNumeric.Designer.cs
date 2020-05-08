namespace RegScoreCalc
{
	partial class FormEditColumnNumeric
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
			this.lblInfo = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblMin = new System.Windows.Forms.Label();
			this.lblMax = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cmbValues = new System.Windows.Forms.ComboBox();
			this.txtValue = new RegScoreCalc.NumericTextBox();
			((System.ComponentModel.ISupportInitialize)(this.txtValue)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.DrawNormalBorder = false;
			this.btnCancel.HoverImage = null;
			this.btnCancel.IsHighlighted = false;
			this.btnCancel.Location = new System.Drawing.Point(174, 97);
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
			this.btnOK.Location = new System.Drawing.Point(93, 97);
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
			this.groupBox1.Location = new System.Drawing.Point(-27, 86);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(295, 2);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// lblInfo
			// 
			this.lblInfo.AutoSize = true;
			this.lblInfo.Location = new System.Drawing.Point(12, 18);
			this.lblInfo.Name = "lblInfo";
			this.lblInfo.Size = new System.Drawing.Size(74, 13);
			this.lblInfo.TabIndex = 0;
			this.lblInfo.Text = "Specify value:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(51, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Minimum:";
			// 
			// lblMin
			// 
			this.lblMin.AutoSize = true;
			this.lblMin.Location = new System.Drawing.Point(92, 40);
			this.lblMin.Name = "lblMin";
			this.lblMin.Size = new System.Drawing.Size(51, 13);
			this.lblMin.TabIndex = 3;
			this.lblMin.Text = "<not set>";
			// 
			// lblMax
			// 
			this.lblMax.AutoSize = true;
			this.lblMax.Location = new System.Drawing.Point(92, 60);
			this.lblMax.Name = "lblMax";
			this.lblMax.Size = new System.Drawing.Size(51, 13);
			this.lblMax.TabIndex = 5;
			this.lblMax.Text = "<not set>";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(54, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Maximum:";
			// 
			// cmbValues
			// 
			this.cmbValues.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbValues.DropDownHeight = 300;
			this.cmbValues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbValues.DropDownWidth = 75;
			this.cmbValues.FormattingEnabled = true;
			this.cmbValues.IntegralHeight = false;
			this.cmbValues.Location = new System.Drawing.Point(174, 12);
			this.cmbValues.Name = "cmbValues";
			this.cmbValues.Size = new System.Drawing.Size(75, 21);
			this.cmbValues.TabIndex = 1;
			this.cmbValues.Visible = false;
			this.cmbValues.SelectedIndexChanged += new System.EventHandler(this.cmbValues_SelectedIndexChanged);
			// 
			// txtValue
			// 
			this.txtValue.AllowSpace = false;
			this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtValue.Location = new System.Drawing.Point(93, 13);
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(75, 20);
			this.txtValue.TabIndex = 0;
			this.txtValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtValue.ValueChanged += new System.EventHandler(this.txtValue_ValueChanged);
			this.txtValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValue_KeyPress);
			this.txtValue.Leave += new System.EventHandler(this.txtValue_Leave);
			this.txtValue.Validated += new System.EventHandler(this.txtValue_Validated);
			// 
			// FormEditColumnNumeric
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(261, 132);
			this.Controls.Add(this.cmbValues);
			this.Controls.Add(this.lblMax);
			this.Controls.Add(this.lblMin);
			this.Controls.Add(this.txtValue);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblInfo);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEditColumnNumeric";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Numeric Value";
			this.Deactivate += new System.EventHandler(this.FormEditColumnNumeric_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditColumnNumeric_FormClosing);
			this.Load += new System.EventHandler(this.FormEditColumnNumeric_Load);
			((System.ComponentModel.ISupportInitialize)(this.txtValue)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblInfo;
		private NumericTextBox txtValue;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblMin;
		private System.Windows.Forms.Label lblMax;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cmbValues;
	}
}