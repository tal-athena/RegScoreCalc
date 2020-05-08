namespace RegScoreCalc
{
	partial class FormFilterByCategory
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
			this.gridCategories = new System.Windows.Forms.DataGridView();
			this.colCheckmark = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnUncheckAll = new System.Windows.Forms.Button();
			this.btnInvertCheck = new System.Windows.Forms.Button();
			this.chkbShowAllDocuments = new System.Windows.Forms.CheckBox();
			this.chkbShowUncategorisedDocuments = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.gridCategories)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-16, 380);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(320, 2);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(201, 393);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(120, 393);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// gridCategories
			// 
			this.gridCategories.AllowUserToAddRows = false;
			this.gridCategories.AllowUserToDeleteRows = false;
			this.gridCategories.AllowUserToOrderColumns = true;
			this.gridCategories.AllowUserToResizeRows = false;
			this.gridCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCategories.BackgroundColor = System.Drawing.Color.White;
			this.gridCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckmark,
            this.colID,
            this.colName});
			this.gridCategories.Location = new System.Drawing.Point(12, 99);
			this.gridCategories.MultiSelect = false;
			this.gridCategories.Name = "gridCategories";
			this.gridCategories.ReadOnly = true;
			this.gridCategories.RowHeadersVisible = false;
			this.gridCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridCategories.Size = new System.Drawing.Size(263, 269);
			this.gridCategories.TabIndex = 5;
			this.gridCategories.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCategories_CellClick);
			this.gridCategories.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCategories_CellDoubleClick);
			this.gridCategories.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridCategories_KeyDown);
			// 
			// colCheckmark
			// 
			this.colCheckmark.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colCheckmark.DataPropertyName = "IsSelected";
			this.colCheckmark.HeaderText = "";
			this.colCheckmark.Name = "colCheckmark";
			this.colCheckmark.ReadOnly = true;
			this.colCheckmark.Width = 40;
			// 
			// colID
			// 
			this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colID.DataPropertyName = "ID";
			this.colID.HeaderText = "ID";
			this.colID.Name = "colID";
			this.colID.ReadOnly = true;
			this.colID.Width = 65;
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colName.DataPropertyName = "Category";
			this.colName.HeaderText = "Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn1.DataPropertyName = "ID";
			this.dataGridViewTextBoxColumn1.HeaderText = "ID";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Category";
			this.dataGridViewTextBoxColumn2.HeaderText = "Name";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// btnUncheckAll
			// 
			this.btnUncheckAll.Location = new System.Drawing.Point(12, 70);
			this.btnUncheckAll.Name = "btnUncheckAll";
			this.btnUncheckAll.Size = new System.Drawing.Size(103, 23);
			this.btnUncheckAll.TabIndex = 3;
			this.btnUncheckAll.Text = "Uncheck All";
			this.btnUncheckAll.UseVisualStyleBackColor = true;
			this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
			// 
			// btnInvertCheck
			// 
			this.btnInvertCheck.Location = new System.Drawing.Point(121, 70);
			this.btnInvertCheck.Name = "btnInvertCheck";
			this.btnInvertCheck.Size = new System.Drawing.Size(103, 23);
			this.btnInvertCheck.TabIndex = 4;
			this.btnInvertCheck.Text = "Invert";
			this.btnInvertCheck.UseVisualStyleBackColor = true;
			this.btnInvertCheck.Click += new System.EventHandler(this.btnInvertCheck_Click);
			// 
			// chkbShowAllDocuments
			// 
			this.chkbShowAllDocuments.AutoSize = true;
			this.chkbShowAllDocuments.Location = new System.Drawing.Point(12, 12);
			this.chkbShowAllDocuments.Name = "chkbShowAllDocuments";
			this.chkbShowAllDocuments.Size = new System.Drawing.Size(121, 17);
			this.chkbShowAllDocuments.TabIndex = 0;
			this.chkbShowAllDocuments.Text = "Show all documents";
			this.chkbShowAllDocuments.UseVisualStyleBackColor = true;
			this.chkbShowAllDocuments.CheckedChanged += new System.EventHandler(this.chkbShowAllDocuments_CheckedChanged);
			// 
			// chkbShowUncategorisedDocuments
			// 
			this.chkbShowUncategorisedDocuments.AutoSize = true;
			this.chkbShowUncategorisedDocuments.Location = new System.Drawing.Point(12, 37);
			this.chkbShowUncategorisedDocuments.Name = "chkbShowUncategorisedDocuments";
			this.chkbShowUncategorisedDocuments.Size = new System.Drawing.Size(157, 17);
			this.chkbShowUncategorisedDocuments.TabIndex = 1;
			this.chkbShowUncategorisedDocuments.Text = "Show unlabeled documents";
			this.chkbShowUncategorisedDocuments.UseVisualStyleBackColor = true;
			this.chkbShowUncategorisedDocuments.CheckedChanged += new System.EventHandler(this.chkbShowUncategorisedDocuments_CheckedChanged);
			// 
			// FormFilterByCategory
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(288, 428);
			this.Controls.Add(this.chkbShowUncategorisedDocuments);
			this.Controls.Add(this.chkbShowAllDocuments);
			this.Controls.Add(this.btnInvertCheck);
			this.Controls.Add(this.btnUncheckAll);
			this.Controls.Add(this.gridCategories);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormFilterByCategory";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Filter Documents by Category";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormFilterByCategory_FormClosing);
			this.Load += new System.EventHandler(this.FormFilterByCategory_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridCategories)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.DataGridView gridCategories;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.Button btnUncheckAll;
		private System.Windows.Forms.Button btnInvertCheck;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckmark;
		private System.Windows.Forms.DataGridViewTextBoxColumn colID;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.CheckBox chkbShowAllDocuments;
		private System.Windows.Forms.CheckBox chkbShowUncategorisedDocuments;
	}
}