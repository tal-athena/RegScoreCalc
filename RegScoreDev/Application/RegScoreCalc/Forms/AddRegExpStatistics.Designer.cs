namespace RegScoreCalc.Forms
{
    partial class AddRegExpStatistics
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReplace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRegExp = new System.Windows.Forms.TextBox();
            this.chbReplace = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(169, 122);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(35, 122);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Replace text";
            // 
            // txtReplace
            // 
            this.txtReplace.Location = new System.Drawing.Point(12, 96);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new System.Drawing.Size(260, 20);
            this.txtReplace.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "RegExp";
            // 
            // txtRegExp
            // 
            this.txtRegExp.Location = new System.Drawing.Point(12, 25);
            this.txtRegExp.Name = "txtRegExp";
            this.txtRegExp.Size = new System.Drawing.Size(260, 20);
            this.txtRegExp.TabIndex = 17;
            // 
            // chbReplace
            // 
            this.chbReplace.AutoSize = true;
            this.chbReplace.Location = new System.Drawing.Point(12, 51);
            this.chbReplace.Name = "chbReplace";
            this.chbReplace.Size = new System.Drawing.Size(66, 17);
            this.chbReplace.TabIndex = 19;
            this.chbReplace.Text = "Replace";
            this.chbReplace.UseVisualStyleBackColor = true;
            // 
            // AddRegExpStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 157);
            this.Controls.Add(this.chbReplace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRegExp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtReplace);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 196);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 196);
            this.Name = "AddRegExpStatistics";
            this.Text = "AddRegExpStatistics";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRegExp;
        private System.Windows.Forms.CheckBox chbReplace;
    }
}