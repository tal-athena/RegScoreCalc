namespace RegScoreCalc
{
	partial class PaneNotesBilling
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
			textBox = new FastColoredTextBoxNS.FastColoredTextBox();
			this.menuPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuitemCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.menuitemSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.menuitemAddToLookBehind = new System.Windows.Forms.ToolStripMenuItem();
			this.menuitemAddToNegLookBehind = new System.Windows.Forms.ToolStripMenuItem();
			this.menuitemAddToLookAhead = new System.Windows.Forms.ToolStripMenuItem();
			this.menuitemAddToNegLookAhead = new System.Windows.Forms.ToolStripMenuItem();
			this.menuitemAddToExceptions = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripTop = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonAddToExceptions = new System.Windows.Forms.ToolStripButton();
			this.btnScrollToFirstMatch = new System.Windows.Forms.ToolStripButton();
			this.btnScrollToLastMatch = new System.Windows.Forms.ToolStripButton();
			this.menuPopup.SuspendLayout();
			this.toolStripTop.SuspendLayout();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.BackColor = System.Drawing.Color.White;
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Location = new System.Drawing.Point(0, 34);
			this.textBox.Margin = new System.Windows.Forms.Padding(10);
			this.textBox.Name = "richTextBox";
			this.textBox.ReadOnly = true;
			this.textBox.Size = new System.Drawing.Size(2954, 1827);
			this.textBox.TabIndex = 1;
			this.textBox.Text = "";
			this.textBox.SelectionChanged += new System.EventHandler(this.textBox_SelectionChanged);
			// 
			// menuPopup
			// 
			this.menuPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemCopy,
            this.menuitemSeparator,
            this.menuitemAddToLookBehind,
            this.menuitemAddToNegLookBehind,
            this.menuitemAddToLookAhead,
            this.menuitemAddToNegLookAhead,
            this.menuitemAddToExceptions});
            this.menuPopup.Name = "menuPopup";
            this.menuPopup.Size = new System.Drawing.Size(208, 142);
            // 
            // menuitemCopy
            // 
            this.menuitemCopy.Name = "menuitemCopy";
            this.menuitemCopy.Size = new System.Drawing.Size(207, 22);
            this.menuitemCopy.Text = "Copy";
            this.menuitemCopy.Click += new System.EventHandler(this.menuitemCopy_Click);
            // 
            // menuitemSeparator
            // 
            this.menuitemSeparator.Name = "menuitemSeparator";
            this.menuitemSeparator.Size = new System.Drawing.Size(204, 6);
            // 
            // menuitemAddToLookBehind
            // 
            this.menuitemAddToLookBehind.Name = "menuitemAddToLookBehind";
            this.menuitemAddToLookBehind.Size = new System.Drawing.Size(207, 22);
            this.menuitemAddToLookBehind.Text = "Add to \'Look behind\'";
            this.menuitemAddToLookBehind.Click += new System.EventHandler(this.menuitemAddToLookBehind_Click);
            // 
            // menuitemAddToNegLookBehind
            // 
            this.menuitemAddToNegLookBehind.Name = "menuitemAddToNegLookBehind";
            this.menuitemAddToNegLookBehind.Size = new System.Drawing.Size(207, 22);
            this.menuitemAddToNegLookBehind.Text = "Add to \'Neg look behind\'";
            this.menuitemAddToNegLookBehind.Click += new System.EventHandler(this.menuitemAddToNegLookBehind_Click);
            // 
            // menuitemAddToLookAhead
            // 
            this.menuitemAddToLookAhead.Name = "menuitemAddToLookAhead";
            this.menuitemAddToLookAhead.Size = new System.Drawing.Size(207, 22);
            this.menuitemAddToLookAhead.Text = "Add to \'Look ahead\'";
            this.menuitemAddToLookAhead.Click += new System.EventHandler(this.menuitemAddToLookAhead_Click);
            // 
            // menuitemAddToNegLookAhead
            // 
            this.menuitemAddToNegLookAhead.Name = "menuitemAddToNegLookAhead";
            this.menuitemAddToNegLookAhead.Size = new System.Drawing.Size(207, 22);
            this.menuitemAddToNegLookAhead.Text = "Add to \'Neg look ahead\'";
            this.menuitemAddToNegLookAhead.Click += new System.EventHandler(this.menuitemAddToNegLookAhead_Click);
            // 
            // menuitemAddToExceptions
            // 
            this.menuitemAddToExceptions.Name = "menuitemAddToExceptions";
            this.menuitemAddToExceptions.Size = new System.Drawing.Size(207, 22);
            this.menuitemAddToExceptions.Text = "Add to Exceptions";
            this.menuitemAddToExceptions.Click += new System.EventHandler(this.menuitemAddToExceptions_Click);
			// 
			// toolStripTop
			// 
			this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripTop.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddToExceptions,
            this.btnScrollToFirstMatch,
            this.btnScrollToLastMatch});
			this.toolStripTop.Location = new System.Drawing.Point(0, 0);
			this.toolStripTop.Name = "toolStripTop";
			this.toolStripTop.Padding = new System.Windows.Forms.Padding(10, 0, 0, 3);
			this.toolStripTop.Size = new System.Drawing.Size(2954, 34);
			this.toolStripTop.Stretch = true;
			this.toolStripTop.TabIndex = 2;
			this.toolStripTop.Text = "toolStrip";
			// 
			// toolStripButtonAddToExceptions
			// 
			this.toolStripButtonAddToExceptions.Image = global::RegScoreCalc.Properties.Resources.CategoryClasses;
			this.toolStripButtonAddToExceptions.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonAddToExceptions.Name = "toolStripButtonAddToExceptions";
			this.toolStripButtonAddToExceptions.Size = new System.Drawing.Size(130, 28);
			this.toolStripButtonAddToExceptions.Text = "Add to Exceptions";
			this.toolStripButtonAddToExceptions.Click += new System.EventHandler(this.toolStripButtonAddToExceptions_Click);
			// 
			// btnScrollToFirstMatch
			// 
			this.btnScrollToFirstMatch.Image = global::RegScoreCalc.Properties.Resources.ScrollToFirstMatch;
			this.btnScrollToFirstMatch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnScrollToFirstMatch.Name = "btnScrollToFirstMatch";
			this.btnScrollToFirstMatch.Size = new System.Drawing.Size(126, 28);
			this.btnScrollToFirstMatch.Text = "Go to First Match";
			this.btnScrollToFirstMatch.Click += new System.EventHandler(this.btnScrollToFirstMatch_Click);
			// 
			// btnScrollToLastMatch
			// 
			this.btnScrollToLastMatch.Image = global::RegScoreCalc.Properties.Resources.ScrollToLastMatch;
			this.btnScrollToLastMatch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnScrollToLastMatch.Name = "btnScrollToLastMatch";
			this.btnScrollToLastMatch.Size = new System.Drawing.Size(125, 28);
			this.btnScrollToLastMatch.Text = "Go to Last Match";
			this.btnScrollToLastMatch.Click += new System.EventHandler(this.btnScrollToLastMatch_Click);
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(2954, 1861);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.toolStripTop);
            this.Name = "PaneNotesBilling";
            this.Text = "PaneNotesBilling";
            this.menuPopup.ResumeLayout(false);
			this.toolStripTop.ResumeLayout(false);
			this.toolStripTop.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public FastColoredTextBoxNS.FastColoredTextBox textBox;
		private System.Windows.Forms.ContextMenuStrip menuPopup;
		private System.Windows.Forms.ToolStripMenuItem menuitemCopy;
		private System.Windows.Forms.ToolStripSeparator menuitemSeparator;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToLookBehind;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToNegLookBehind;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToLookAhead;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToNegLookAhead;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToExceptions;
		private System.Windows.Forms.ToolStrip toolStripTop;
		private System.Windows.Forms.ToolStripButton toolStripButtonAddToExceptions;
		private System.Windows.Forms.ToolStripButton btnScrollToFirstMatch;
		private System.Windows.Forms.ToolStripButton btnScrollToLastMatch;
	}
}