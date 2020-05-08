namespace RegScoreCalc.Forms
{
    partial class FormAddGroup
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
			this.lbGroups = new System.Windows.Forms.CheckedListBox();
			this.btnCheckAll = new RegScoreCalc.RibbonStyleButton();
			this.btnUncheckAll = new RegScoreCalc.RibbonStyleButton();
			this.btnInvertCheck = new RegScoreCalc.RibbonStyleButton();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(175, 258);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.HoverImage = null;
			this.btnOK.Location = new System.Drawing.Point(94, 258);
			this.btnOK.Name = "btnOK";
			this.btnOK.NormalImage = null;
			this.btnOK.PressedImage = null;
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-58, 245);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(378, 2);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			// 
			// lbGroups
			// 
			this.lbGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbGroups.FormattingEnabled = true;
			this.lbGroups.IntegralHeight = false;
			this.lbGroups.Location = new System.Drawing.Point(12, 41);
			this.lbGroups.Name = "lbGroups";
			this.lbGroups.Size = new System.Drawing.Size(238, 188);
			this.lbGroups.TabIndex = 0;
			// 
			// btnCheckAll
			// 
			this.btnCheckAll.HoverImage = null;
			this.btnCheckAll.Location = new System.Drawing.Point(12, 12);
			this.btnCheckAll.Name = "btnCheckAll";
			this.btnCheckAll.NormalImage = null;
			this.btnCheckAll.PressedImage = null;
			this.btnCheckAll.Size = new System.Drawing.Size(75, 23);
			this.btnCheckAll.TabIndex = 3;
			this.btnCheckAll.Text = "Check All";
			this.btnCheckAll.UseVisualStyleBackColor = true;
			this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
			// 
			// btnUncheckAll
			// 
			this.btnUncheckAll.HoverImage = null;
			this.btnUncheckAll.Location = new System.Drawing.Point(93, 12);
			this.btnUncheckAll.Name = "btnUncheckAll";
			this.btnUncheckAll.NormalImage = null;
			this.btnUncheckAll.PressedImage = null;
			this.btnUncheckAll.Size = new System.Drawing.Size(75, 23);
			this.btnUncheckAll.TabIndex = 4;
			this.btnUncheckAll.Text = "Uncheck All";
			this.btnUncheckAll.UseVisualStyleBackColor = true;
			this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
			// 
			// btnInvertCheck
			// 
			this.btnInvertCheck.HoverImage = null;
			this.btnInvertCheck.Location = new System.Drawing.Point(174, 12);
			this.btnInvertCheck.Name = "btnInvertCheck";
			this.btnInvertCheck.NormalImage = null;
			this.btnInvertCheck.PressedImage = null;
			this.btnInvertCheck.Size = new System.Drawing.Size(75, 23);
			this.btnInvertCheck.TabIndex = 5;
			this.btnInvertCheck.Text = "Invert Check";
			this.btnInvertCheck.UseVisualStyleBackColor = true;
			this.btnInvertCheck.Click += new System.EventHandler(this.btnInvertCheck_Click);
			// 
			// FormAddGroup
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(262, 293);
			this.Controls.Add(this.btnInvertCheck);
			this.Controls.Add(this.btnUncheckAll);
			this.Controls.Add(this.btnCheckAll);
			this.Controls.Add(this.lbGroups);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAddGroup";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Groups";
			this.Load += new System.EventHandler(this.FormAddGroup_Load);
			this.ResumeLayout(false);

        }

        #endregion

		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckedListBox lbGroups;
		private RibbonStyleButton btnCheckAll;
		private RibbonStyleButton btnUncheckAll;
		private RibbonStyleButton btnInvertCheck;
    }
}