using System.Windows.Forms;

namespace RegScoreCalc
{
	partial class PaneNoteHtmlView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaneNoteHtmlView));
            this.menuPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuitemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuitemAddToLookBehind = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemAddToNegLookBehind = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemAddToLookAhead = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemAddToNegLookAhead = new System.Windows.Forms.ToolStripMenuItem();
            this.menuitemAddToExceptions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.btnAddViewTab = new System.Windows.Forms.ToolStripButton();
            this.btnShowText = new System.Windows.Forms.ToolStripButton();
            this.htmlPanel = new System.Windows.Forms.Panel();
            this.paneSplitter = new System.Windows.Forms.SplitContainer();
            this.toolStripTop.SuspendLayout();
            this.htmlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paneSplitter)).BeginInit();
            this.paneSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPopup
            // 
            this.menuPopup.Name = "menuPopup";
            this.menuPopup.Size = new System.Drawing.Size(61, 4);
            // 
            // menuitemCopy
            // 
            this.menuitemCopy.Name = "menuitemCopy";
            this.menuitemCopy.Size = new System.Drawing.Size(32, 19);
            // 
            // menuitemSeparator
            // 
            this.menuitemSeparator.Name = "menuitemSeparator";
            this.menuitemSeparator.Size = new System.Drawing.Size(6, 6);
            // 
            // menuitemAddToLookBehind
            // 
            this.menuitemAddToLookBehind.Name = "menuitemAddToLookBehind";
            this.menuitemAddToLookBehind.Size = new System.Drawing.Size(32, 19);
            // 
            // menuitemAddToNegLookBehind
            // 
            this.menuitemAddToNegLookBehind.Name = "menuitemAddToNegLookBehind";
            this.menuitemAddToNegLookBehind.Size = new System.Drawing.Size(32, 19);
            // 
            // menuitemAddToLookAhead
            // 
            this.menuitemAddToLookAhead.Name = "menuitemAddToLookAhead";
            this.menuitemAddToLookAhead.Size = new System.Drawing.Size(32, 19);
            // 
            // menuitemAddToNegLookAhead
            // 
            this.menuitemAddToNegLookAhead.Name = "menuitemAddToNegLookAhead";
            this.menuitemAddToNegLookAhead.Size = new System.Drawing.Size(32, 19);
            // 
            // menuitemAddToExceptions
            // 
            this.menuitemAddToExceptions.Name = "menuitemAddToExceptions";
            this.menuitemAddToExceptions.Size = new System.Drawing.Size(32, 19);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.AutoSize = false;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 28);
            this.toolStripButton1.Text = "TABs";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(98, 28);
            this.toolStripButton2.Text = "Add browser tab";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripTop
            // 
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.btnAddViewTab,
            this.btnShowText});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(10, 0, 0, 3);
            this.toolStripTop.Size = new System.Drawing.Size(1011, 34);
            this.toolStripTop.Stretch = true;
            this.toolStripTop.TabIndex = 2;
            this.toolStripTop.Text = "toolStrip";
            // 
            // btnAddViewTab
            // 
            this.btnAddViewTab.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddViewTab.Image = ((System.Drawing.Image)(resources.GetObject("btnAddViewTab.Image")));
            this.btnAddViewTab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddViewTab.Name = "btnAddViewTab";
            this.btnAddViewTab.Size = new System.Drawing.Size(80, 28);
            this.btnAddViewTab.Text = "Add view tab";
            this.btnAddViewTab.Click += new System.EventHandler(this.btnAddViewTab_Click);
            // 
            // btnShowText
            // 
            this.btnShowText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnShowText.CheckOnClick = true;
            this.btnShowText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnShowText.Image = ((System.Drawing.Image)(resources.GetObject("btnShowText.Image")));
            this.btnShowText.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowText.Name = "btnShowText";
            this.btnShowText.Size = new System.Drawing.Size(64, 28);
            this.btnShowText.Text = "Show Text";
            this.btnShowText.CheckedChanged += new System.EventHandler(this.btnShowText_Click);
            // 
            // htmlPanel
            // 
            this.htmlPanel.Controls.Add(this.paneSplitter);
            this.htmlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlPanel.Location = new System.Drawing.Point(0, 34);
            this.htmlPanel.Name = "htmlPanel";
            this.htmlPanel.Size = new System.Drawing.Size(1011, 497);
            this.htmlPanel.TabIndex = 3;
            // 
            // paneSplitter
            // 
            this.paneSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.paneSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paneSplitter.Location = new System.Drawing.Point(0, 0);
            this.paneSplitter.Name = "paneSplitter";
            this.paneSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.paneSplitter.Panel1Collapsed = true;
            this.paneSplitter.Panel1MinSize = 0;
            this.paneSplitter.Panel2MinSize = 0;
            this.paneSplitter.Size = new System.Drawing.Size(1011, 497);
            this.paneSplitter.SplitterDistance = 25;
            this.paneSplitter.TabIndex = 0;
            // 
            // PaneNoteHtmlView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 531);
            this.Controls.Add(this.htmlPanel);
            this.Controls.Add(this.toolStripTop);
            this.Name = "PaneNoteHtmlView";
            this.Text = "PaneNotes";
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.htmlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paneSplitter)).EndInit();
            this.paneSplitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ContextMenuStrip menuPopup;
		private System.Windows.Forms.ToolStripMenuItem menuitemCopy;
		private System.Windows.Forms.ToolStripSeparator menuitemSeparator;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToLookBehind;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToNegLookBehind;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToLookAhead;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToNegLookAhead;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToExceptions;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStrip toolStripTop;
        private System.Windows.Forms.ToolStripButton btnAddViewTab;
        private System.Windows.Forms.Panel htmlPanel;
        private System.Windows.Forms.ToolStripButton btnShowText;
        private System.Windows.Forms.SplitContainer paneSplitter;
    }
}