namespace RegScoreCalc.Forms
{
    partial class AddNewICD9Item
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.chbOneWay = new System.Windows.Forms.CheckBox();
			this.chbRegExp = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDiagnosis = new System.Windows.Forms.TextBox();
			this.txtCode = new System.Windows.Forms.TextBox();
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnSave = new RegScoreCalc.RibbonStyleButton();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txtDiagnosis3 = new System.Windows.Forms.TextBox();
			this.txtICD3 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.txtDiagnosis2 = new System.Windows.Forms.TextBox();
			this.txtICD2 = new System.Windows.Forms.TextBox();
			this.chbRegExpCombination = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtDiagnosis1 = new System.Windows.Forms.TextBox();
			this.txtICD1 = new System.Windows.Forms.TextBox();
			this.btnCancelCombination = new RegScoreCalc.RibbonStyleButton();
			this.btnSaveCombination = new RegScoreCalc.RibbonStyleButton();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(284, 336);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.chbOneWay);
			this.tabPage1.Controls.Add(this.chbRegExp);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.txtDiagnosis);
			this.tabPage1.Controls.Add(this.txtCode);
			this.tabPage1.Controls.Add(this.btnCancel);
			this.tabPage1.Controls.Add(this.btnSave);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(276, 310);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Single";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// chbOneWay
			// 
			this.chbOneWay.AutoSize = true;
			this.chbOneWay.Location = new System.Drawing.Point(11, 243);
			this.chbOneWay.Name = "chbOneWay";
			this.chbOneWay.Size = new System.Drawing.Size(68, 17);
			this.chbOneWay.TabIndex = 3;
			this.chbOneWay.Text = "One-way";
			this.chbOneWay.UseVisualStyleBackColor = true;
			// 
			// chbRegExp
			// 
			this.chbRegExp.AutoSize = true;
			this.chbRegExp.Location = new System.Drawing.Point(11, 220);
			this.chbRegExp.Name = "chbRegExp";
			this.chbRegExp.Size = new System.Drawing.Size(116, 17);
			this.chbRegExp.TabIndex = 2;
			this.chbRegExp.Text = "Regular expression";
			this.chbRegExp.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 57);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "Diagnosis";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(59, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "ICD9 Code";
			// 
			// txtDiagnosis
			// 
			this.txtDiagnosis.Location = new System.Drawing.Point(8, 73);
			this.txtDiagnosis.Multiline = true;
			this.txtDiagnosis.Name = "txtDiagnosis";
			this.txtDiagnosis.Size = new System.Drawing.Size(260, 141);
			this.txtDiagnosis.TabIndex = 1;
			// 
			// txtCode
			// 
			this.txtCode.Location = new System.Drawing.Point(8, 21);
			this.txtCode.Name = "txtCode";
			this.txtCode.Size = new System.Drawing.Size(260, 20);
			this.txtCode.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(160, 269);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.HoverImage = null;
			this.btnSave.Location = new System.Drawing.Point(26, 269);
			this.btnSave.Name = "btnSave";
			this.btnSave.NormalImage = null;
			this.btnSave.PressedImage = null;
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.label7);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.txtDiagnosis3);
			this.tabPage2.Controls.Add(this.txtICD3);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.txtDiagnosis2);
			this.tabPage2.Controls.Add(this.txtICD2);
			this.tabPage2.Controls.Add(this.chbRegExpCombination);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.txtDiagnosis1);
			this.tabPage2.Controls.Add(this.txtICD1);
			this.tabPage2.Controls.Add(this.btnCancelCombination);
			this.tabPage2.Controls.Add(this.btnSaveCombination);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(276, 310);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Combination";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(8, 217);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(62, 13);
			this.label7.TabIndex = 30;
			this.label7.Text = "Diagnosis 3";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(8, 176);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(68, 13);
			this.label8.TabIndex = 29;
			this.label8.Text = "ICD9 Code 3";
			// 
			// txtDiagnosis3
			// 
			this.txtDiagnosis3.Location = new System.Drawing.Point(8, 233);
			this.txtDiagnosis3.Name = "txtDiagnosis3";
			this.txtDiagnosis3.Size = new System.Drawing.Size(260, 20);
			this.txtDiagnosis3.TabIndex = 5;
			// 
			// txtICD3
			// 
			this.txtICD3.Location = new System.Drawing.Point(8, 192);
			this.txtICD3.Name = "txtICD3";
			this.txtICD3.Size = new System.Drawing.Size(260, 20);
			this.txtICD3.TabIndex = 4;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 132);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(62, 13);
			this.label5.TabIndex = 26;
			this.label5.Text = "Diagnosis 2";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 91);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(68, 13);
			this.label6.TabIndex = 25;
			this.label6.Text = "ICD9 Code 2";
			// 
			// txtDiagnosis2
			// 
			this.txtDiagnosis2.Location = new System.Drawing.Point(8, 148);
			this.txtDiagnosis2.Name = "txtDiagnosis2";
			this.txtDiagnosis2.Size = new System.Drawing.Size(260, 20);
			this.txtDiagnosis2.TabIndex = 3;
			// 
			// txtICD2
			// 
			this.txtICD2.Location = new System.Drawing.Point(8, 107);
			this.txtICD2.Name = "txtICD2";
			this.txtICD2.Size = new System.Drawing.Size(260, 20);
			this.txtICD2.TabIndex = 2;
			// 
			// chbRegExpCombination
			// 
			this.chbRegExpCombination.AutoSize = true;
			this.chbRegExpCombination.Location = new System.Drawing.Point(6, 259);
			this.chbRegExpCombination.Name = "chbRegExpCombination";
			this.chbRegExpCombination.Size = new System.Drawing.Size(116, 17);
			this.chbRegExpCombination.TabIndex = 6;
			this.chbRegExpCombination.Text = "Regular expression";
			this.chbRegExpCombination.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(62, 13);
			this.label3.TabIndex = 21;
			this.label3.Text = "Diagnosis 1";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 7);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(68, 13);
			this.label4.TabIndex = 20;
			this.label4.Text = "ICD9 Code 1";
			// 
			// txtDiagnosis1
			// 
			this.txtDiagnosis1.Location = new System.Drawing.Point(8, 64);
			this.txtDiagnosis1.Name = "txtDiagnosis1";
			this.txtDiagnosis1.Size = new System.Drawing.Size(260, 20);
			this.txtDiagnosis1.TabIndex = 1;
			// 
			// txtICD1
			// 
			this.txtICD1.Location = new System.Drawing.Point(8, 23);
			this.txtICD1.Name = "txtICD1";
			this.txtICD1.Size = new System.Drawing.Size(260, 20);
			this.txtICD1.TabIndex = 0;
			// 
			// btnCancelCombination
			// 
			this.btnCancelCombination.HoverImage = null;
			this.btnCancelCombination.Location = new System.Drawing.Point(164, 282);
			this.btnCancelCombination.Name = "btnCancelCombination";
			this.btnCancelCombination.NormalImage = null;
			this.btnCancelCombination.PressedImage = null;
			this.btnCancelCombination.Size = new System.Drawing.Size(75, 23);
			this.btnCancelCombination.TabIndex = 8;
			this.btnCancelCombination.Text = "Cancel";
			this.btnCancelCombination.UseVisualStyleBackColor = true;
			this.btnCancelCombination.Click += new System.EventHandler(this.btnCancelCombination_Click);
			// 
			// btnSaveCombination
			// 
			this.btnSaveCombination.HoverImage = null;
			this.btnSaveCombination.Location = new System.Drawing.Point(24, 282);
			this.btnSaveCombination.Name = "btnSaveCombination";
			this.btnSaveCombination.NormalImage = null;
			this.btnSaveCombination.PressedImage = null;
			this.btnSaveCombination.Size = new System.Drawing.Size(75, 23);
			this.btnSaveCombination.TabIndex = 7;
			this.btnSaveCombination.Text = "Save";
			this.btnSaveCombination.UseVisualStyleBackColor = true;
			this.btnSaveCombination.Click += new System.EventHandler(this.btnSaveCombination_Click);
			// 
			// AddNewICD9Item
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(284, 336);
			this.Controls.Add(this.tabControl1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddNewICD9Item";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AddNewICD9Item";
			this.Load += new System.EventHandler(this.AddNewICD9Item_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox chbOneWay;
        private System.Windows.Forms.CheckBox chbRegExp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDiagnosis;
        private System.Windows.Forms.TextBox txtCode;
		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnSave;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chbRegExpCombination;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDiagnosis1;
        private System.Windows.Forms.TextBox txtICD1;
		private RibbonStyleButton btnCancelCombination;
		private RibbonStyleButton btnSaveCombination;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDiagnosis3;
        private System.Windows.Forms.TextBox txtICD3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDiagnosis2;
        private System.Windows.Forms.TextBox txtICD2;

    }
}