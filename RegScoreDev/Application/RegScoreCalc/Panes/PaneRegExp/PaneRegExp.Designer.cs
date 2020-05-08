using System;
using RegScoreCalc.Code.Controls;

namespace RegScoreCalc
{
	partial class PaneRegExp
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaneRegExp));
            this.regExpDataGridView = new System.Windows.Forms.DataGridView();
            this.menuOperations = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuitemEditRegExp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemCopyRegExp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuitemDeleteRows = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRegExp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuitemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.regExpOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemWordBoundaries = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.tabControl = new RegScoreCalc.CustomTabControl();
            this.tabRegularExpression = new RegScoreCalc.CustomTabPage();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddToExceptions = new System.Windows.Forms.ToolStripButton();
            this.btnSynergies = new System.Windows.Forms.ToolStripButton();
            this.tabEntities = new RegScoreCalc.CustomTabPage();
            this.entityDataGridView = new System.Windows.Forms.DataGridView();
            this.toolStripAutoSizeEntityGrid = new System.Windows.Forms.ToolStrip();
            this.toolAutosizeEntity = new System.Windows.Forms.ToolStripButton();
            this.toolStripRefreshEntity = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).BeginInit();
            this.menuOperations.SuspendLayout();
            this.menuRegExp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabRegularExpression.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.tabEntities.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityDataGridView)).BeginInit();
            this.toolStripAutoSizeEntityGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // regExpDataGridView
            // 
            this.regExpDataGridView.AllowUserToResizeRows = false;
            this.regExpDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.regExpDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.regExpDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.regExpDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regExpDataGridView.Location = new System.Drawing.Point(3, 37);
            this.regExpDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.regExpDataGridView.Name = "regExpDataGridView";
            this.regExpDataGridView.RowHeadersWidth = 20;
            this.regExpDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.regExpDataGridView.Size = new System.Drawing.Size(557, 475);
            this.regExpDataGridView.TabIndex = 2;
            this.regExpDataGridView.VirtualMode = true;
            this.regExpDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellClick);
            this.regExpDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellDoubleClick);
            this.regExpDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.regExpDataGridView_CellFormatting);
            this.regExpDataGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellLeave);
            this.regExpDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.regExpDataGridView_CellMouseClick);
            this.regExpDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.regExpDataGridView_CellMouseDoubleClick);
            this.regExpDataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellValidated);
            this.regExpDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.regExpDataGridView_DataError);
            this.regExpDataGridView.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.regExpDataGridView_UserDeletedRow);
            this.regExpDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.regExpDataGridView_MouseClick);
            // 
            // menuOperations
            // 
            this.menuOperations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemEditRegExp,
            this.menuitemCopyRegExp,
            this.toolStripSeparator2,
            this.menuitemDeleteRows});
            this.menuOperations.Name = "menuOperations";
            this.menuOperations.Size = new System.Drawing.Size(204, 76);
            // 
            // menuitemEditRegExp
            // 
            this.menuitemEditRegExp.Name = "menuitemEditRegExp";
            this.menuitemEditRegExp.Size = new System.Drawing.Size(203, 22);
            this.menuitemEditRegExp.Text = "Edit Regular Expression";
            // 
            // menuitemCopyRegExp
            // 
            this.menuitemCopyRegExp.Name = "menuitemCopyRegExp";
            this.menuitemCopyRegExp.Size = new System.Drawing.Size(203, 22);
            this.menuitemCopyRegExp.Text = "Copy Regular Expression";
            this.menuitemCopyRegExp.Click += new System.EventHandler(this.menuitemCopyRegExp_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(200, 6);
            // 
            // menuitemDeleteRows
            // 
            this.menuitemDeleteRows.Name = "menuitemDeleteRows";
            this.menuitemDeleteRows.Size = new System.Drawing.Size(203, 22);
            this.menuitemDeleteRows.Text = "Delete Selected Rows";
            // 
            // menuRegExp
            // 
            this.menuRegExp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemCopy,
            this.menuitemCut,
            this.menuitemPaste,
            this.toolStripSeparator1,
            this.regExpOptionsToolStripMenuItem});
            this.menuRegExp.Name = "menuRegExp";
            this.menuRegExp.Size = new System.Drawing.Size(158, 98);
            // 
            // menuitemCopy
            // 
            this.menuitemCopy.Name = "menuitemCopy";
            this.menuitemCopy.Size = new System.Drawing.Size(157, 22);
            this.menuitemCopy.Text = "Copy";
            this.menuitemCopy.Click += new System.EventHandler(this.menuitemCopy_Click);
            // 
            // menuitemCut
            // 
            this.menuitemCut.Name = "menuitemCut";
            this.menuitemCut.Size = new System.Drawing.Size(157, 22);
            this.menuitemCut.Text = "Cut";
            this.menuitemCut.Click += new System.EventHandler(this.menuitemCut_Click);
            // 
            // menuitemPaste
            // 
            this.menuitemPaste.Name = "menuitemPaste";
            this.menuitemPaste.Size = new System.Drawing.Size(157, 22);
            this.menuitemPaste.Text = "Paste";
            this.menuitemPaste.Click += new System.EventHandler(this.menuitemPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(154, 6);
            // 
            // regExpOptionsToolStripMenuItem
            // 
            this.regExpOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemWordBoundaries});
            this.regExpOptionsToolStripMenuItem.Name = "regExpOptionsToolStripMenuItem";
            this.regExpOptionsToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.regExpOptionsToolStripMenuItem.Text = "RegExp Options";
            // 
            // menuitemWordBoundaries
            // 
            this.menuitemWordBoundaries.Name = "menuitemWordBoundaries";
            this.menuitemWordBoundaries.Size = new System.Drawing.Size(165, 22);
            this.menuitemWordBoundaries.Text = "Word Boundaries";
            this.menuitemWordBoundaries.Click += new System.EventHandler(this.menuitemWordBoundaries_Click);
            // 
            // splitter
            // 
            this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitter.Location = new System.Drawing.Point(0, 0);
            this.splitter.Margin = new System.Windows.Forms.Padding(0);
            this.splitter.Name = "splitter";
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.tabControl);
            this.splitter.Size = new System.Drawing.Size(1242, 544);
            this.splitter.SplitterDistance = 571;
            this.splitter.TabIndex = 3;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabRegularExpression);
            this.tabControl.Controls.Add(this.tabEntities);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowIndicators = false;
            this.tabControl.Size = new System.Drawing.Size(571, 544);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabRegularExpression
            // 
            this.tabRegularExpression.Controls.Add(this.regExpDataGridView);
            this.tabRegularExpression.Controls.Add(this.toolStripTop);
            this.tabRegularExpression.Location = new System.Drawing.Point(4, 25);
            this.tabRegularExpression.Name = "tabRegularExpression";
            this.tabRegularExpression.Padding = new System.Windows.Forms.Padding(3);
            this.tabRegularExpression.Size = new System.Drawing.Size(563, 515);
            this.tabRegularExpression.TabIndex = 0;
            this.tabRegularExpression.Text = "Regular Expressions";
            this.tabRegularExpression.UseVisualStyleBackColor = true;
            // 
            // toolStripTop
            // 
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddToExceptions,
            this.btnSynergies});
            this.toolStripTop.Location = new System.Drawing.Point(3, 3);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(10, 0, 0, 3);
            this.toolStripTop.Size = new System.Drawing.Size(557, 34);
            this.toolStripTop.Stretch = true;
            this.toolStripTop.TabIndex = 3;
            this.toolStripTop.Text = "toolStrip";
            // 
            // toolStripButtonAddToExceptions
            // 
            this.toolStripButtonAddToExceptions.Image = global::RegScoreCalc.Properties.Resources.search;
            this.toolStripButtonAddToExceptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAddToExceptions.Name = "toolStripButtonAddToExceptions";
            this.toolStripButtonAddToExceptions.Size = new System.Drawing.Size(136, 28);
            this.toolStripButtonAddToExceptions.Text = "Auto-size Columns";
            this.toolStripButtonAddToExceptions.Click += new System.EventHandler(this.toolStripAutoSizeColumns_Click);
            // 
            // btnSynergies
            // 
            this.btnSynergies.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSynergies.Image = ((System.Drawing.Image)(resources.GetObject("btnSynergies.Image")));
            this.btnSynergies.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSynergies.Name = "btnSynergies";
            this.btnSynergies.Size = new System.Drawing.Size(61, 28);
            this.btnSynergies.Text = "Synergies";
            this.btnSynergies.Click += new System.EventHandler(this.btnSynergies_Click);
            // 
            // tabEntities
            // 
            this.tabEntities.Controls.Add(this.entityDataGridView);
            this.tabEntities.Controls.Add(this.toolStripAutoSizeEntityGrid);
            this.tabEntities.Location = new System.Drawing.Point(4, 25);
            this.tabEntities.Name = "tabEntities";
            this.tabEntities.Padding = new System.Windows.Forms.Padding(3);
            this.tabEntities.Size = new System.Drawing.Size(563, 515);
            this.tabEntities.TabIndex = 1;
            this.tabEntities.Text = "Entities";
            this.tabEntities.UseVisualStyleBackColor = true;
            // 
            // entityDataGridView
            // 
            this.entityDataGridView.AllowUserToResizeRows = false;
            this.entityDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.entityDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.entityDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.entityDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityDataGridView.Location = new System.Drawing.Point(3, 37);
            this.entityDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.entityDataGridView.Name = "entityDataGridView";
            this.entityDataGridView.RowHeadersWidth = 20;
            this.entityDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.entityDataGridView.Size = new System.Drawing.Size(557, 475);
            this.entityDataGridView.TabIndex = 9;
            this.entityDataGridView.VirtualMode = true;
            this.entityDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.entityDataGridView_CellClick);
            this.entityDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.entityDataGridView_CellFormatting);
            // 
            // toolStripAutoSizeEntityGrid
            // 
            this.toolStripAutoSizeEntityGrid.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripAutoSizeEntityGrid.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripAutoSizeEntityGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolAutosizeEntity,
            this.toolStripRefreshEntity});
            this.toolStripAutoSizeEntityGrid.Location = new System.Drawing.Point(3, 3);
            this.toolStripAutoSizeEntityGrid.Name = "toolStripAutoSizeEntityGrid";
            this.toolStripAutoSizeEntityGrid.Padding = new System.Windows.Forms.Padding(10, 0, 0, 3);
            this.toolStripAutoSizeEntityGrid.Size = new System.Drawing.Size(557, 34);
            this.toolStripAutoSizeEntityGrid.Stretch = true;
            this.toolStripAutoSizeEntityGrid.TabIndex = 8;
            this.toolStripAutoSizeEntityGrid.Text = "toolStrip";
            // 
            // toolAutosizeEntity
            // 
            this.toolAutosizeEntity.Image = global::RegScoreCalc.Properties.Resources.search;
            this.toolAutosizeEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolAutosizeEntity.Name = "toolAutosizeEntity";
            this.toolAutosizeEntity.Size = new System.Drawing.Size(136, 28);
            this.toolAutosizeEntity.Text = "Auto-size Columns";
            this.toolAutosizeEntity.Click += new System.EventHandler(this.toolAutosizeEntity_Click);
            // 
            // toolStripRefreshEntity
            // 
            this.toolStripRefreshEntity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripRefreshEntity.Image = ((System.Drawing.Image)(resources.GetObject("toolStripRefreshEntity.Image")));
            this.toolStripRefreshEntity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripRefreshEntity.Name = "toolStripRefreshEntity";
            this.toolStripRefreshEntity.Size = new System.Drawing.Size(50, 28);
            this.toolStripRefreshEntity.Text = "Refresh";
            this.toolStripRefreshEntity.Click += new System.EventHandler(this.toolStripRefreshEntity_Click);
            // 
            // PaneRegExp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1242, 544);
            this.Controls.Add(this.splitter);
            this.Name = "PaneRegExp";
            this.Text = "PaneRegExp";
            ((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).EndInit();
            this.menuOperations.ResumeLayout(false);
            this.menuRegExp.ResumeLayout(false);
            this.splitter.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabRegularExpression.ResumeLayout(false);
            this.tabRegularExpression.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.tabEntities.ResumeLayout(false);
            this.tabEntities.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.entityDataGridView)).EndInit();
            this.toolStripAutoSizeEntityGrid.ResumeLayout(false);
            this.toolStripAutoSizeEntityGrid.PerformLayout();
            this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.ContextMenuStrip menuOperations;
		private System.Windows.Forms.ToolStripMenuItem menuitemDeleteRows;
		private System.Windows.Forms.ToolStripMenuItem menuitemEditRegExp;
		private System.Windows.Forms.ContextMenuStrip menuRegExp;
		private System.Windows.Forms.ToolStripMenuItem menuitemCopy;
		private System.Windows.Forms.ToolStripMenuItem menuitemCut;
		private System.Windows.Forms.ToolStripMenuItem menuitemPaste;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem regExpOptionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuitemWordBoundaries;
		private System.Windows.Forms.ToolStripMenuItem menuitemCopyRegExp;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.SplitContainer splitter;
        private CustomTabControl tabControl;
        private CustomTabPage tabRegularExpression;
        private CustomTabPage tabEntities;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripButton toolStripButtonAddToExceptions;
        private System.Windows.Forms.ToolStripButton btnSynergies;
        private System.Windows.Forms.DataGridView regExpDataGridView;
        private System.Windows.Forms.DataGridView entityDataGridView;
        private System.Windows.Forms.ToolStrip toolStripAutoSizeEntityGrid;
        private System.Windows.Forms.ToolStripButton toolStripRefreshEntity;
        private System.Windows.Forms.ToolStripButton toolAutosizeEntity;
    }
}