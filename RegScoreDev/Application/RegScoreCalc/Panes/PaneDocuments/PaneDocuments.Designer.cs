using System;
using System.Windows.Forms;

namespace RegScoreCalc
{
	partial class PaneDocuments
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaneDocuments));
            this.columnCategory = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.documentsDataGridView = new System.Windows.Forms.DataGridView();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAutoAssignCategory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonShowMLScores = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHighlightCategories = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonConvertToDynamic = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonColumns = new System.Windows.Forms.ToolStripButton();
            this.btnTextHighlight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSortByCategory = new System.Windows.Forms.ToolStripButton();
            this.btnExternalSort = new System.Windows.Forms.ToolStripButton();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProc1SVM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnProc3SVM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGridView)).BeginInit();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
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
            // toolStripContainer
            // 
            this.toolStripContainer.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.documentsDataGridView);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1134, 492);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.LeftToolStripPanelVisible = false;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.RightToolStripPanelVisible = false;
            this.toolStripContainer.Size = new System.Drawing.Size(1134, 526);
            this.toolStripContainer.TabIndex = 2;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStripTop);
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
            this.documentsDataGridView.Size = new System.Drawing.Size(1134, 492);
            this.documentsDataGridView.TabIndex = 1;
            this.documentsDataGridView.VirtualMode = true;
            this.documentsDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.documentsDataGridView_CellDoubleClick);
            this.documentsDataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.documentsDataGridView_CellPainting);
            this.documentsDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.documentsDataGridView_CellValidating);
            this.documentsDataGridView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.documentsDataGridView_CellValueNeeded);
            this.documentsDataGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.documentsDataGridView_ColumnHeaderMouseClick);
            this.documentsDataGridView.CurrentCellChanged += new System.EventHandler(this.documentsDataGridView_CurrentCellChanged);
            this.documentsDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.documentsDataGridView_DataError);
            this.documentsDataGridView.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.documentsDataGridView_EditingControlShowing);
            this.documentsDataGridView.SelectionChanged += new System.EventHandler(this.documentsDataGridView_SelectionChanged);
            this.documentsDataGridView.Sorted += new System.EventHandler(this.documentsDataGridView_Sorted);
            this.documentsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.documentsDataGridView_KeyDown);
            this.documentsDataGridView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.documentsDataGridView_KeyUp);
            this.documentsDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.documentsDataGridView_MouseClick);
            // 
            // toolStripTop
            // 
            this.toolStripTop.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAutoAssignCategory,
            this.toolStripSeparator1,
            this.toolStripButtonShowMLScores,
            this.toolStripButtonHighlightCategories,
            this.toolStripSeparator2,
            this.toolStripButtonConvertToDynamic,
            this.toolStripButtonColumns,
            this.btnTextHighlight,
            this.toolStripSeparator3,
            this.btnSortByCategory,
            this.btnExternalSort});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(10, 0, 0, 3);
            this.toolStripTop.Size = new System.Drawing.Size(1134, 34);
            this.toolStripTop.Stretch = true;
            this.toolStripTop.TabIndex = 0;
            this.toolStripTop.Text = "toolStrip";
            // 
            // toolStripButtonAutoAssignCategory
            // 
            this.toolStripButtonAutoAssignCategory.Image = global::RegScoreCalc.Properties.Resources.CategoryClasses;
            this.toolStripButtonAutoAssignCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoAssignCategory.Name = "toolStripButtonAutoAssignCategory";
            this.toolStripButtonAutoAssignCategory.Size = new System.Drawing.Size(124, 28);
            this.toolStripButtonAutoAssignCategory.Text = "Assign Cagegory";
            this.toolStripButtonAutoAssignCategory.Click += new System.EventHandler(this.toolStripButtonAutoAssignCategory_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButtonShowMLScores
            // 
            this.toolStripButtonShowMLScores.CheckOnClick = true;
            this.toolStripButtonShowMLScores.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonShowMLScores.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowMLScores.Name = "toolStripButtonShowMLScores";
            this.toolStripButtonShowMLScores.Size = new System.Drawing.Size(97, 28);
            this.toolStripButtonShowMLScores.Text = "Show ML Scores";
            this.toolStripButtonShowMLScores.Click += new System.EventHandler(this.toolStripButtonShowMLScores_Click);
            // 
            // toolStripButtonHighlightCategories
            // 
            this.toolStripButtonHighlightCategories.CheckOnClick = true;
            this.toolStripButtonHighlightCategories.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonHighlightCategories.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHighlightCategories.Image")));
            this.toolStripButtonHighlightCategories.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHighlightCategories.Name = "toolStripButtonHighlightCategories";
            this.toolStripButtonHighlightCategories.Size = new System.Drawing.Size(111, 28);
            this.toolStripButtonHighlightCategories.Text = "Colorize categories";
            this.toolStripButtonHighlightCategories.Click += new System.EventHandler(this.toolStripButtonHighlightCategories_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButtonConvertToDynamic
            // 
            this.toolStripButtonConvertToDynamic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonConvertToDynamic.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonConvertToDynamic.Image")));
            this.toolStripButtonConvertToDynamic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonConvertToDynamic.Name = "toolStripButtonConvertToDynamic";
            this.toolStripButtonConvertToDynamic.Size = new System.Drawing.Size(117, 28);
            this.toolStripButtonConvertToDynamic.Text = "Convert to Dynamic";
            this.toolStripButtonConvertToDynamic.Click += new System.EventHandler(this.toolStripButtonConvertToDynamic_Click);
            // 
            // toolStripButtonColumns
            // 
            this.toolStripButtonColumns.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonColumns.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonColumns.Image")));
            this.toolStripButtonColumns.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColumns.Name = "toolStripButtonColumns";
            this.toolStripButtonColumns.Size = new System.Drawing.Size(59, 28);
            this.toolStripButtonColumns.Text = "Columns";
            this.toolStripButtonColumns.Click += new System.EventHandler(this.toolStripButtonColumns_Click);
            // 
            // btnTextHighlight
            // 
            this.btnTextHighlight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnTextHighlight.Image = ((System.Drawing.Image)(resources.GetObject("btnTextHighlight.Image")));
            this.btnTextHighlight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTextHighlight.Name = "btnTextHighlight";
            this.btnTextHighlight.Size = new System.Drawing.Size(85, 28);
            this.btnTextHighlight.Text = "Text Highlight";
            this.btnTextHighlight.Visible = false;
            this.btnTextHighlight.Click += new System.EventHandler(this.btnTextHighlight_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // btnSortByCategory
            // 
            this.btnSortByCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSortByCategory.Image = ((System.Drawing.Image)(resources.GetObject("btnSortByCategory.Image")));
            this.btnSortByCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSortByCategory.Name = "btnSortByCategory";
            this.btnSortByCategory.Size = new System.Drawing.Size(130, 28);
            this.btnSortByCategory.Text = "Sort by Category Value";
            this.btnSortByCategory.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnSortByCategory.Click += new System.EventHandler(this.btnSortByCategory_Click);
            // 
            // btnExternalSort
            // 
            this.btnExternalSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnExternalSort.Image = ((System.Drawing.Image)(resources.GetObject("btnExternalSort.Image")));
            this.btnExternalSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExternalSort.Name = "btnExternalSort";
            this.btnExternalSort.Size = new System.Drawing.Size(73, 28);
            this.btnExternalSort.Text = "Sort Groups";
            this.btnExternalSort.Click += new System.EventHandler(this.btnExternalSort_Click);
            // 
            // columnNo
            // 
            this.columnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.columnNo.HeaderText = "SNo";
            this.columnNo.MaxInputLength = 16;
            this.columnNo.Name = "columnNo";
            this.columnNo.ReadOnly = true;
            this.columnNo.Width = 80;
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
            // PaneDocuments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 526);
            this.Controls.Add(this.toolStripContainer);
            this.Name = "PaneDocuments";
            this.Text = "PaneDocuments";
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentsDataGridView)).EndInit();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);

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
		private System.Windows.Forms.ToolStripContainer toolStripContainer;
		private System.Windows.Forms.ToolStrip toolStripTop;
		private System.Windows.Forms.ToolStripButton toolStripButtonAutoAssignCategory;
		private System.Windows.Forms.ToolStripButton toolStripButtonShowMLScores;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButtonHighlightCategories;
		private ToolStripButton toolStripButtonConvertToDynamic;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripButton toolStripButtonColumns;
		private ToolStripButton btnTextHighlight;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripButton btnSortByCategory;
		private ToolStripButton btnExternalSort;
	}
}