using System;
using System.Windows.Forms;

namespace RegScoreCalc
{
	partial class PaneDocumentsReviewMLOld
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaneDocumentsReviewMLOld));
            this.documentsDataGridView = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCategory = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProc1SVM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProc3SVM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonHighlightCategories = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGridView)).BeginInit();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // documentsDataGridView
            // 
            this.documentsDataGridView.AllowUserToAddRows = false;
            this.documentsDataGridView.AllowUserToDeleteRows = false;
            this.documentsDataGridView.AllowUserToResizeRows = false;
            this.documentsDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.documentsDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.documentsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.documentsDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.documentsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentsDataGridView.Location = new System.Drawing.Point(0, 25);
            this.documentsDataGridView.Name = "documentsDataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.documentsDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.documentsDataGridView.RowHeadersWidth = 20;
            this.documentsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.documentsDataGridView.Size = new System.Drawing.Size(0, 0);
            this.documentsDataGridView.TabIndex = 1;
            this.documentsDataGridView.VirtualMode = true;
            this.documentsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.documentsDataGridView_CellDoubleClick);
            this.documentsDataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.documentsDataGridView_CellPainting);
            this.documentsDataGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.documentsDataGridView_CellValueNeeded);
            this.documentsDataGridView.SelectionChanged += new System.EventHandler(this.documentsDataGridView_SelectionChanged);
            this.documentsDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.documentsDataGridView_MouseClick);
			this.documentsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.documentsDataGridView_KeyDown);
			this.documentsDataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.documentsDataGridView_KeyUp);
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
            this.columnScore.Name = "columnScore";
            // 
            // columnRank
            // 
            this.columnRank.Name = "columnRank";
            // 
            // columnProc1SVM
            // 
            this.columnProc1SVM.Name = "columnProc1SVM";
            // 
            // columnProc3SVM
            // 
            this.columnProc3SVM.Name = "columnProc3SVM";
            // 
            // toolStripTop
            // 
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonHighlightCategories});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(10, 0, 0, 3);
            this.toolStripTop.Size = new System.Drawing.Size(0, 25);
            this.toolStripTop.Stretch = true;
            this.toolStripTop.TabIndex = 0;
            this.toolStripTop.Text = "toolStrip";
            // 
            // toolStripButtonHighlightCategories
            // 
            this.toolStripButtonHighlightCategories.CheckOnClick = true;
            this.toolStripButtonHighlightCategories.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonHighlightCategories.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHighlightCategories.Image")));
            this.toolStripButtonHighlightCategories.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHighlightCategories.Name = "toolStripButtonHighlightCategories";
            this.toolStripButtonHighlightCategories.Size = new System.Drawing.Size(102, 19);
            this.toolStripButtonHighlightCategories.Text = "Colorize categories";
            this.toolStripButtonHighlightCategories.Click += new System.EventHandler(this.toolStripButtonHighlightCategories_Click);
            // 
            // PaneDocumentsReviewMLOld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.Controls.Add(this.documentsDataGridView);
            this.Controls.Add(this.toolStripTop);
            this.Name = "PaneDocumentsReviewMLOld";
            this.Text = "PaneDocuments";
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGridView)).EndInit();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView documentsDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
		private System.Windows.Forms.DataGridViewComboBoxColumn columnCategory;
		private System.Windows.Forms.DataGridViewTextBoxColumn columnID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnProc3SVM;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnProc1SVM;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnRank;
		private System.Windows.Forms.ToolStrip toolStripTop;
		private System.Windows.Forms.ToolStripButton toolStripButtonHighlightCategories;
	}
}