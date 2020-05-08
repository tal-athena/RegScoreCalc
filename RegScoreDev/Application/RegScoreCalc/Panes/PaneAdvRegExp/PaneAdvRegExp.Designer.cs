namespace RegScoreCalc
{
	partial class PaneAdvRegExp
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
			((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).BeginInit();
			this.menuOperations.SuspendLayout();
			this.menuRegExp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
			this.splitter.Panel1.SuspendLayout();
			this.splitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// regExpDataGridView
			// 
			this.regExpDataGridView.AllowUserToResizeRows = false;
			this.regExpDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.regExpDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.regExpDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.regExpDataGridView.Location = new System.Drawing.Point(0, 0);
			this.regExpDataGridView.Name = "regExpDataGridView";
			this.regExpDataGridView.RowHeadersWidth = 20;
			this.regExpDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.regExpDataGridView.Size = new System.Drawing.Size(1500, 1879);
			this.regExpDataGridView.TabIndex = 2;
			this.regExpDataGridView.VirtualMode = true;
			this.regExpDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellClick);
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
			this.splitter.Name = "splitter";
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.regExpDataGridView);
			this.splitter.Size = new System.Drawing.Size(2936, 1879);
			this.splitter.SplitterDistance = 1500;
			this.splitter.TabIndex = 3;
			// 
			// PaneAdvRegExp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(2936, 1879);
			this.Controls.Add(this.splitter);
			this.Name = "PaneAdvRegExp";
			this.Text = "PaneAdvRegExp";
			((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).EndInit();
			this.menuOperations.ResumeLayout(false);
			this.menuRegExp.ResumeLayout(false);
			this.splitter.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
			this.splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView regExpDataGridView;
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
	}
}