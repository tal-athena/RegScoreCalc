namespace RegScoreCalc
{
	partial class PaneDocumentsBilling
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
            this.documentsDataGridView = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCategory = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDob = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // documentsDataGridView
            // 
            this.documentsDataGridView.AllowUserToAddRows = false;
            this.documentsDataGridView.AllowUserToDeleteRows = false;
            this.documentsDataGridView.AllowUserToResizeRows = false;
            this.documentsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.documentsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.documentsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentsDataGridView.Location = new System.Drawing.Point(0, 0);
            this.documentsDataGridView.Name = "documentsDataGridView";
            this.documentsDataGridView.RowHeadersWidth = 20;
            this.documentsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.documentsDataGridView.Size = new System.Drawing.Size(1003, 568);
            this.documentsDataGridView.TabIndex = 1;
            this.documentsDataGridView.VirtualMode = true;
            this.documentsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.documentsDataGridView_CellDoubleClick);
			this.documentsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.documentsDataGridView_KeyDown);
			this.documentsDataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.documentsDataGridView_KeyUp);
			this.documentsDataGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.documentsDataGridView_CellValueNeeded);
            this.documentsDataGridView.SelectionChanged += new System.EventHandler(this.documentsDataGridView_SelectionChanged);
            this.documentsDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.documentsDataGridView_MouseClick);
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "SNo";
            this.columnNo.MaxInputLength = 16;
            this.columnNo.Name = "columnNo";
            this.columnNo.ReadOnly = true;
            this.columnNo.Width = 42;
            // 
            // columnCategory
            // 
            this.columnCategory.DataPropertyName = "Category";
            this.columnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.columnCategory.HeaderText = "Category";
            this.columnCategory.MaxDropDownItems = 18;
            this.columnCategory.Name = "columnCategory";
            this.columnCategory.ReadOnly = true;
            this.columnCategory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnCategory.Width = 120;
            // 
            // columnID
            // 
            this.columnID.Name = "columnID";
            // 
            // columnScore
            // 
            this.columnCreateDate.Name = "columnCreateDate";
            // 
            // columnRank
            // 
            this.columnDob.Name = "columnDob";
            // 
            // columnProc1SVM
            // 
            this.columnSex.Name = "columnSex";
            // 
            // columnProc3SVM
            // 
            this.columnAge.Name = "columnAge";
            // 
            // PaneDocumentsBilling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 568);
            this.Controls.Add(this.documentsDataGridView);
            this.Name = "PaneDocumentsBilling";
            this.Text = "PaneDocuments";
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGridView)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView documentsDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
		private System.Windows.Forms.DataGridViewComboBoxColumn columnCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnReason;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDob;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCreateDate;
	}
}