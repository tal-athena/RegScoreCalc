namespace RegScoreCalc.Forms
{
	partial class FormAssignCategory
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
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lbSelectCategory = new System.Windows.Forms.ListBox();
			this.lbDynamicColumns = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnApply = new RibbonStyleButton();
			this.btnApplyAndClose = new RibbonStyleButton();
			this.btnClose = new RibbonStyleButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tabAssignmentRules = new System.Windows.Forms.TabControl();
			this.pageScore = new System.Windows.Forms.TabPage();
			this.txtScoreValue = new System.Windows.Forms.NumericUpDown();
			this.cmbCondition = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tabAssignmentRules.SuspendLayout();
			this.pageScore.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtScoreValue)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.lbSelectCategory);
			this.groupBox1.Controls.Add(this.lbDynamicColumns);
			this.groupBox1.Location = new System.Drawing.Point(16, 15);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(449, 300);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Assign Category";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(227, 26);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(105, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Select category:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 26);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(119, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Category columns:";
			// 
			// lbSelectCategory
			// 
			this.lbSelectCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbSelectCategory.FormattingEnabled = true;
			this.lbSelectCategory.IntegralHeight = false;
			this.lbSelectCategory.ItemHeight = 16;
			this.lbSelectCategory.Location = new System.Drawing.Point(225, 49);
			this.lbSelectCategory.Margin = new System.Windows.Forms.Padding(4);
			this.lbSelectCategory.Name = "lbSelectCategory";
			this.lbSelectCategory.Size = new System.Drawing.Size(209, 237);
			this.lbSelectCategory.TabIndex = 3;
			this.lbSelectCategory.SelectedIndexChanged += new System.EventHandler(this.lbSelectCategory_SelectedIndexChanged);
			// 
			// lbDynamicColumns
			// 
			this.lbDynamicColumns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbDynamicColumns.FormattingEnabled = true;
			this.lbDynamicColumns.IntegralHeight = false;
			this.lbDynamicColumns.ItemHeight = 16;
			this.lbDynamicColumns.Location = new System.Drawing.Point(15, 49);
			this.lbDynamicColumns.Margin = new System.Windows.Forms.Padding(4);
			this.lbDynamicColumns.Name = "lbDynamicColumns";
			this.lbDynamicColumns.Size = new System.Drawing.Size(201, 237);
			this.lbDynamicColumns.TabIndex = 1;
			this.lbDynamicColumns.SelectedIndexChanged += new System.EventHandler(this.lbCategoryColumns_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Location = new System.Drawing.Point(-25, 330);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox2.Size = new System.Drawing.Size(919, 2);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnApply.Location = new System.Drawing.Point(186, 343);
			this.btnApply.Margin = new System.Windows.Forms.Padding(4);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(160, 28);
			this.btnApply.TabIndex = 2;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// btnApplyAndClose
			// 
			this.btnApplyAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnApplyAndClose.Location = new System.Drawing.Point(354, 343);
			this.btnApplyAndClose.Margin = new System.Windows.Forms.Padding(4);
			this.btnApplyAndClose.Name = "btnApplyAndClose";
			this.btnApplyAndClose.Size = new System.Drawing.Size(160, 28);
			this.btnApplyAndClose.TabIndex = 3;
			this.btnApplyAndClose.Text = "Apply and Close";
			this.btnApplyAndClose.UseVisualStyleBackColor = true;
			this.btnApplyAndClose.Click += new System.EventHandler(this.btnApplyAndClose_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(522, 343);
			this.btnClose.Margin = new System.Windows.Forms.Padding(4);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(160, 28);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.tabAssignmentRules);
			this.groupBox3.Location = new System.Drawing.Point(473, 15);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox3.Size = new System.Drawing.Size(381, 300);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Assignment Rules";
			// 
			// tabAssignmentRules
			// 
			this.tabAssignmentRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabAssignmentRules.Controls.Add(this.pageScore);
			this.tabAssignmentRules.Location = new System.Drawing.Point(16, 27);
			this.tabAssignmentRules.Margin = new System.Windows.Forms.Padding(0);
			this.tabAssignmentRules.Name = "tabAssignmentRules";
			this.tabAssignmentRules.SelectedIndex = 0;
			this.tabAssignmentRules.Size = new System.Drawing.Size(355, 260);
			this.tabAssignmentRules.TabIndex = 0;
			// 
			// pageScore
			// 
			this.pageScore.Controls.Add(this.txtScoreValue);
			this.pageScore.Controls.Add(this.cmbCondition);
			this.pageScore.Controls.Add(this.label3);
			this.pageScore.Location = new System.Drawing.Point(4, 25);
			this.pageScore.Margin = new System.Windows.Forms.Padding(0);
			this.pageScore.Name = "pageScore";
			this.pageScore.Size = new System.Drawing.Size(347, 231);
			this.pageScore.TabIndex = 0;
			this.pageScore.Text = "Score";
			this.pageScore.UseVisualStyleBackColor = true;
			// 
			// txtScoreValue
			// 
			this.txtScoreValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtScoreValue.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
			this.txtScoreValue.Location = new System.Drawing.Point(197, 11);
			this.txtScoreValue.Margin = new System.Windows.Forms.Padding(4);
			this.txtScoreValue.Name = "txtScoreValue";
			this.txtScoreValue.Size = new System.Drawing.Size(136, 22);
			this.txtScoreValue.TabIndex = 2;
			this.txtScoreValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtScoreValue.ThousandsSeparator = true;
			// 
			// cmbCondition
			// 
			this.cmbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbCondition.FormattingEnabled = true;
			this.cmbCondition.Items.AddRange(new object[] {
            "less than",
            "equal to",
            "greater than"});
			this.cmbCondition.Location = new System.Drawing.Point(80, 10);
			this.cmbCondition.Margin = new System.Windows.Forms.Padding(4);
			this.cmbCondition.Name = "cmbCondition";
			this.cmbCondition.Size = new System.Drawing.Size(108, 24);
			this.cmbCondition.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 14);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(57, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Score is";
			// 
			// FormAssignCategory
			// 
			this.AcceptButton = this.btnApply;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(869, 386);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnApplyAndClose);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAssignCategory";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Assign Category Automatically";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAssignCategory_FormClosing);
			this.Load += new System.EventHandler(this.FormAssignCategory_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.tabAssignmentRules.ResumeLayout(false);
			this.pageScore.ResumeLayout(false);
			this.pageScore.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.txtScoreValue)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private RibbonStyleButton btnApply;
		private RibbonStyleButton btnApplyAndClose;
		private RibbonStyleButton btnClose;
		private System.Windows.Forms.ListBox lbSelectCategory;
		private System.Windows.Forms.ListBox lbDynamicColumns;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TabControl tabAssignmentRules;
		private System.Windows.Forms.TabPage pageScore;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbCondition;
		private System.Windows.Forms.NumericUpDown txtScoreValue;
	}
}