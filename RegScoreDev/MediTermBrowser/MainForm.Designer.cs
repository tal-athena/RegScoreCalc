namespace MediTermBrowser
{
    partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.treeViewNavigation = new System.Windows.Forms.TreeView();
			this.panelSearch = new System.Windows.Forms.Panel();
			this.btnSearch = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.lblSearch = new System.Windows.Forms.Label();
			this.panelDetails = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.lblDetailsCode = new System.Windows.Forms.Label();
			this.txtDetailsCode = new System.Windows.Forms.TextBox();
			this.lblDetailsTerm = new System.Windows.Forms.Label();
			this.txtDetailsTerm = new System.Windows.Forms.TextBox();
			this.groupBoxSynonims = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.listBoxDetailsSynonims = new System.Windows.Forms.ListBox();
			this.tabElements = new System.Windows.Forms.TabControl();
			this.tabPageParents = new System.Windows.Forms.TabPage();
			this.listViewParents = new System.Windows.Forms.ListView();
			this.toolStrip2 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.tabChildren = new System.Windows.Forms.TabPage();
			this.listViewChildren = new System.Windows.Forms.ListView();
			this.tabPagePartOf = new System.Windows.Forms.TabPage();
			this.listViewPartOf = new System.Windows.Forms.ListView();
			this.tabPageInversePartOf = new System.Windows.Forms.TabPage();
			this.listViewInversePartOf = new System.Windows.Forms.ListView();
			this.tabPageAncestorParts = new System.Windows.Forms.TabPage();
			this.listViewAncestorParts = new System.Windows.Forms.ListView();
			this.tabPageDescendantParts = new System.Windows.Forms.TabPage();
			this.listViewDescendantParts = new System.Windows.Forms.ListView();
			this.tabPageOtherRelations = new System.Windows.Forms.TabPage();
			this.listViewOtherRelations = new System.Windows.Forms.ListView();
			this.panelSearch.SuspendLayout();
			this.panelDetails.SuspendLayout();
			this.panel1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.groupBoxSynonims.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabElements.SuspendLayout();
			this.tabPageParents.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			this.tabChildren.SuspendLayout();
			this.tabPagePartOf.SuspendLayout();
			this.tabPageInversePartOf.SuspendLayout();
			this.tabPageAncestorParts.SuspendLayout();
			this.tabPageDescendantParts.SuspendLayout();
			this.tabPageOtherRelations.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeViewNavigation
			// 
			this.treeViewNavigation.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeViewNavigation.Location = new System.Drawing.Point(0, 0);
			this.treeViewNavigation.Name = "treeViewNavigation";
			this.treeViewNavigation.Size = new System.Drawing.Size(431, 473);
			this.treeViewNavigation.TabIndex = 0;
			this.treeViewNavigation.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewNavigation_BeforeSelect);
			this.treeViewNavigation.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewNavigation_AfterSelect);
			// 
			// panelSearch
			// 
			this.panelSearch.Controls.Add(this.btnSearch);
			this.panelSearch.Controls.Add(this.txtSearch);
			this.panelSearch.Controls.Add(this.lblSearch);
			this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelSearch.Location = new System.Drawing.Point(431, 0);
			this.panelSearch.Name = "panelSearch";
			this.panelSearch.Size = new System.Drawing.Size(541, 55);
			this.panelSearch.TabIndex = 1;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Location = new System.Drawing.Point(454, 11);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 2;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.Location = new System.Drawing.Point(57, 13);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(391, 20);
			this.txtSearch.TabIndex = 1;
			// 
			// lblSearch
			// 
			this.lblSearch.AutoSize = true;
			this.lblSearch.Location = new System.Drawing.Point(6, 16);
			this.lblSearch.Name = "lblSearch";
			this.lblSearch.Size = new System.Drawing.Size(44, 13);
			this.lblSearch.TabIndex = 0;
			this.lblSearch.Text = "Search:";
			// 
			// panelDetails
			// 
			this.panelDetails.Controls.Add(this.panel1);
			this.panelDetails.Controls.Add(this.groupBoxSynonims);
			this.panelDetails.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelDetails.Location = new System.Drawing.Point(431, 55);
			this.panelDetails.Name = "panelDetails";
			this.panelDetails.Size = new System.Drawing.Size(541, 224);
			this.panelDetails.TabIndex = 2;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.toolStrip1);
			this.panel1.Controls.Add(this.lblDetailsCode);
			this.panel1.Controls.Add(this.txtDetailsCode);
			this.panel1.Controls.Add(this.lblDetailsTerm);
			this.panel1.Controls.Add(this.txtDetailsTerm);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(541, 83);
			this.panel1.TabIndex = 6;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton5});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(541, 25);
			this.toolStrip1.TabIndex = 6;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton4.Text = "Copy Term to clipboard";
			this.toolStripButton4.Click += new System.EventHandler(this.btnCopyTermToClipboard_Click);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
			this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton5.Text = "Copy synonims to clipboard";
			this.toolStripButton5.Click += new System.EventHandler(this.btnCopySynonimsToClipboard_Click);
			// 
			// lblDetailsCode
			// 
			this.lblDetailsCode.AutoSize = true;
			this.lblDetailsCode.Location = new System.Drawing.Point(16, 36);
			this.lblDetailsCode.Name = "lblDetailsCode";
			this.lblDetailsCode.Size = new System.Drawing.Size(35, 13);
			this.lblDetailsCode.TabIndex = 4;
			this.lblDetailsCode.Text = "Code:";
			// 
			// txtDetailsCode
			// 
			this.txtDetailsCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDetailsCode.Location = new System.Drawing.Point(57, 33);
			this.txtDetailsCode.Name = "txtDetailsCode";
			this.txtDetailsCode.Size = new System.Drawing.Size(472, 20);
			this.txtDetailsCode.TabIndex = 5;
			// 
			// lblDetailsTerm
			// 
			this.lblDetailsTerm.AutoSize = true;
			this.lblDetailsTerm.Location = new System.Drawing.Point(16, 60);
			this.lblDetailsTerm.Name = "lblDetailsTerm";
			this.lblDetailsTerm.Size = new System.Drawing.Size(34, 13);
			this.lblDetailsTerm.TabIndex = 2;
			this.lblDetailsTerm.Text = "Term:";
			// 
			// txtDetailsTerm
			// 
			this.txtDetailsTerm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDetailsTerm.Location = new System.Drawing.Point(57, 57);
			this.txtDetailsTerm.Name = "txtDetailsTerm";
			this.txtDetailsTerm.Size = new System.Drawing.Size(472, 20);
			this.txtDetailsTerm.TabIndex = 3;
			// 
			// groupBoxSynonims
			// 
			this.groupBoxSynonims.Controls.Add(this.groupBox1);
			this.groupBoxSynonims.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxSynonims.Location = new System.Drawing.Point(0, 0);
			this.groupBoxSynonims.Name = "groupBoxSynonims";
			this.groupBoxSynonims.Size = new System.Drawing.Size(541, 224);
			this.groupBoxSynonims.TabIndex = 7;
			this.groupBoxSynonims.TabStop = false;
			this.groupBoxSynonims.Text = "Synonims";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.listBoxDetailsSynonims);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox1.Location = new System.Drawing.Point(3, 93);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(535, 128);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Synonims";
			// 
			// listBoxDetailsSynonims
			// 
			this.listBoxDetailsSynonims.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBoxDetailsSynonims.FormattingEnabled = true;
			this.listBoxDetailsSynonims.Location = new System.Drawing.Point(3, 16);
			this.listBoxDetailsSynonims.Margin = new System.Windows.Forms.Padding(33, 3, 3, 3);
			this.listBoxDetailsSynonims.Name = "listBoxDetailsSynonims";
			this.listBoxDetailsSynonims.Size = new System.Drawing.Size(529, 109);
			this.listBoxDetailsSynonims.TabIndex = 0;
			// 
			// tabElements
			// 
			this.tabElements.Controls.Add(this.tabPageParents);
			this.tabElements.Controls.Add(this.tabChildren);
			this.tabElements.Controls.Add(this.tabPagePartOf);
			this.tabElements.Controls.Add(this.tabPageInversePartOf);
			this.tabElements.Controls.Add(this.tabPageAncestorParts);
			this.tabElements.Controls.Add(this.tabPageDescendantParts);
			this.tabElements.Controls.Add(this.tabPageOtherRelations);
			this.tabElements.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabElements.Location = new System.Drawing.Point(431, 279);
			this.tabElements.Name = "tabElements";
			this.tabElements.SelectedIndex = 0;
			this.tabElements.Size = new System.Drawing.Size(541, 194);
			this.tabElements.TabIndex = 3;
			this.tabElements.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabElements_Selecting);
			// 
			// tabPageParents
			// 
			this.tabPageParents.Controls.Add(this.listViewParents);
			this.tabPageParents.Controls.Add(this.toolStrip2);
			this.tabPageParents.Location = new System.Drawing.Point(4, 22);
			this.tabPageParents.Name = "tabPageParents";
			this.tabPageParents.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageParents.Size = new System.Drawing.Size(533, 168);
			this.tabPageParents.TabIndex = 0;
			this.tabPageParents.Text = "Parents";
			this.tabPageParents.UseVisualStyleBackColor = true;
			// 
			// listViewParents
			// 
			this.listViewParents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewParents.FullRowSelect = true;
			this.listViewParents.Location = new System.Drawing.Point(3, 28);
			this.listViewParents.MultiSelect = false;
			this.listViewParents.Name = "listViewParents";
			this.listViewParents.Size = new System.Drawing.Size(527, 137);
			this.listViewParents.TabIndex = 1;
			this.listViewParents.UseCompatibleStateImageBehavior = false;
			this.listViewParents.View = System.Windows.Forms.View.List;
			// 
			// toolStrip2
			// 
			this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3});
			this.toolStrip2.Location = new System.Drawing.Point(3, 3);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new System.Drawing.Size(527, 25);
			this.toolStrip2.TabIndex = 2;
			this.toolStrip2.Text = "toolStrip2";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "Copy Selected Term to clipboard";
			this.toolStripButton1.Click += new System.EventHandler(this.btnCopySelectedTermToClipboard_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "Copy all terms to clipboard (comma separated)";
			this.toolStripButton2.Click += new System.EventHandler(this.btnCopyAllTermsToClipboard_Click);
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "Select in the navigation tree";
			this.toolStripButton3.Click += new System.EventHandler(this.btnSelectInTheNavigationTree_Click);
			// 
			// tabChildren
			// 
			this.tabChildren.Controls.Add(this.listViewChildren);
			this.tabChildren.Location = new System.Drawing.Point(4, 22);
			this.tabChildren.Name = "tabChildren";
			this.tabChildren.Padding = new System.Windows.Forms.Padding(3);
			this.tabChildren.Size = new System.Drawing.Size(533, 168);
			this.tabChildren.TabIndex = 1;
			this.tabChildren.Text = "Children";
			this.tabChildren.UseVisualStyleBackColor = true;
			// 
			// listViewChildren
			// 
			this.listViewChildren.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewChildren.FullRowSelect = true;
			this.listViewChildren.Location = new System.Drawing.Point(3, 3);
			this.listViewChildren.MultiSelect = false;
			this.listViewChildren.Name = "listViewChildren";
			this.listViewChildren.Size = new System.Drawing.Size(527, 162);
			this.listViewChildren.TabIndex = 2;
			this.listViewChildren.UseCompatibleStateImageBehavior = false;
			this.listViewChildren.View = System.Windows.Forms.View.List;
			// 
			// tabPagePartOf
			// 
			this.tabPagePartOf.Controls.Add(this.listViewPartOf);
			this.tabPagePartOf.Location = new System.Drawing.Point(4, 22);
			this.tabPagePartOf.Name = "tabPagePartOf";
			this.tabPagePartOf.Padding = new System.Windows.Forms.Padding(3);
			this.tabPagePartOf.Size = new System.Drawing.Size(533, 168);
			this.tabPagePartOf.TabIndex = 2;
			this.tabPagePartOf.Text = "part_of";
			this.tabPagePartOf.UseVisualStyleBackColor = true;
			// 
			// listViewPartOf
			// 
			this.listViewPartOf.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewPartOf.FullRowSelect = true;
			this.listViewPartOf.Location = new System.Drawing.Point(3, 3);
			this.listViewPartOf.MultiSelect = false;
			this.listViewPartOf.Name = "listViewPartOf";
			this.listViewPartOf.Size = new System.Drawing.Size(527, 162);
			this.listViewPartOf.TabIndex = 3;
			this.listViewPartOf.UseCompatibleStateImageBehavior = false;
			this.listViewPartOf.View = System.Windows.Forms.View.List;
			// 
			// tabPageInversePartOf
			// 
			this.tabPageInversePartOf.Controls.Add(this.listViewInversePartOf);
			this.tabPageInversePartOf.Location = new System.Drawing.Point(4, 22);
			this.tabPageInversePartOf.Name = "tabPageInversePartOf";
			this.tabPageInversePartOf.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageInversePartOf.Size = new System.Drawing.Size(533, 168);
			this.tabPageInversePartOf.TabIndex = 3;
			this.tabPageInversePartOf.Text = "INVERSE_part_of";
			this.tabPageInversePartOf.UseVisualStyleBackColor = true;
			// 
			// listViewInversePartOf
			// 
			this.listViewInversePartOf.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewInversePartOf.FullRowSelect = true;
			this.listViewInversePartOf.Location = new System.Drawing.Point(3, 3);
			this.listViewInversePartOf.MultiSelect = false;
			this.listViewInversePartOf.Name = "listViewInversePartOf";
			this.listViewInversePartOf.Size = new System.Drawing.Size(527, 162);
			this.listViewInversePartOf.TabIndex = 3;
			this.listViewInversePartOf.UseCompatibleStateImageBehavior = false;
			this.listViewInversePartOf.View = System.Windows.Forms.View.List;
			// 
			// tabPageAncestorParts
			// 
			this.tabPageAncestorParts.Controls.Add(this.listViewAncestorParts);
			this.tabPageAncestorParts.Location = new System.Drawing.Point(4, 22);
			this.tabPageAncestorParts.Name = "tabPageAncestorParts";
			this.tabPageAncestorParts.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageAncestorParts.Size = new System.Drawing.Size(533, 168);
			this.tabPageAncestorParts.TabIndex = 4;
			this.tabPageAncestorParts.Text = "ancestor_parts";
			this.tabPageAncestorParts.UseVisualStyleBackColor = true;
			// 
			// listViewAncestorParts
			// 
			this.listViewAncestorParts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewAncestorParts.FullRowSelect = true;
			this.listViewAncestorParts.Location = new System.Drawing.Point(3, 3);
			this.listViewAncestorParts.MultiSelect = false;
			this.listViewAncestorParts.Name = "listViewAncestorParts";
			this.listViewAncestorParts.Size = new System.Drawing.Size(527, 162);
			this.listViewAncestorParts.TabIndex = 3;
			this.listViewAncestorParts.UseCompatibleStateImageBehavior = false;
			this.listViewAncestorParts.View = System.Windows.Forms.View.List;
			// 
			// tabPageDescendantParts
			// 
			this.tabPageDescendantParts.Controls.Add(this.listViewDescendantParts);
			this.tabPageDescendantParts.Location = new System.Drawing.Point(4, 22);
			this.tabPageDescendantParts.Name = "tabPageDescendantParts";
			this.tabPageDescendantParts.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageDescendantParts.Size = new System.Drawing.Size(533, 168);
			this.tabPageDescendantParts.TabIndex = 5;
			this.tabPageDescendantParts.Text = "descendant_parts";
			this.tabPageDescendantParts.UseVisualStyleBackColor = true;
			// 
			// listViewDescendantParts
			// 
			this.listViewDescendantParts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewDescendantParts.FullRowSelect = true;
			this.listViewDescendantParts.Location = new System.Drawing.Point(3, 3);
			this.listViewDescendantParts.MultiSelect = false;
			this.listViewDescendantParts.Name = "listViewDescendantParts";
			this.listViewDescendantParts.Size = new System.Drawing.Size(527, 162);
			this.listViewDescendantParts.TabIndex = 3;
			this.listViewDescendantParts.UseCompatibleStateImageBehavior = false;
			this.listViewDescendantParts.View = System.Windows.Forms.View.List;
			// 
			// tabPageOtherRelations
			// 
			this.tabPageOtherRelations.Controls.Add(this.listViewOtherRelations);
			this.tabPageOtherRelations.Location = new System.Drawing.Point(4, 22);
			this.tabPageOtherRelations.Name = "tabPageOtherRelations";
			this.tabPageOtherRelations.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageOtherRelations.Size = new System.Drawing.Size(533, 168);
			this.tabPageOtherRelations.TabIndex = 6;
			this.tabPageOtherRelations.Text = "otherrelations";
			this.tabPageOtherRelations.UseVisualStyleBackColor = true;
			// 
			// listViewOtherRelations
			// 
			this.listViewOtherRelations.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listViewOtherRelations.FullRowSelect = true;
			this.listViewOtherRelations.Location = new System.Drawing.Point(3, 3);
			this.listViewOtherRelations.MultiSelect = false;
			this.listViewOtherRelations.Name = "listViewOtherRelations";
			this.listViewOtherRelations.Size = new System.Drawing.Size(527, 162);
			this.listViewOtherRelations.TabIndex = 3;
			this.listViewOtherRelations.UseCompatibleStateImageBehavior = false;
			this.listViewOtherRelations.View = System.Windows.Forms.View.List;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(972, 473);
			this.Controls.Add(this.tabElements);
			this.Controls.Add(this.panelDetails);
			this.Controls.Add(this.panelSearch);
			this.Controls.Add(this.treeViewNavigation);
			this.Name = "MainForm";
			this.Text = "Medi Term Browser";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.panelSearch.ResumeLayout(false);
			this.panelSearch.PerformLayout();
			this.panelDetails.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.groupBoxSynonims.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.tabElements.ResumeLayout(false);
			this.tabPageParents.ResumeLayout(false);
			this.tabPageParents.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			this.tabChildren.ResumeLayout(false);
			this.tabPagePartOf.ResumeLayout(false);
			this.tabPageInversePartOf.ResumeLayout(false);
			this.tabPageAncestorParts.ResumeLayout(false);
			this.tabPageDescendantParts.ResumeLayout(false);
			this.tabPageOtherRelations.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewNavigation;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.TextBox txtDetailsCode;
        private System.Windows.Forms.Label lblDetailsCode;
        private System.Windows.Forms.TextBox txtDetailsTerm;
        private System.Windows.Forms.Label lblDetailsTerm;
        private System.Windows.Forms.GroupBox groupBoxSynonims;
        private System.Windows.Forms.ListBox listBoxDetailsSynonims;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabElements;
        private System.Windows.Forms.TabPage tabPageParents;
        private System.Windows.Forms.TabPage tabChildren;
        private System.Windows.Forms.ListView listViewParents;
        private System.Windows.Forms.TabPage tabPagePartOf;
        private System.Windows.Forms.TabPage tabPageInversePartOf;
        private System.Windows.Forms.TabPage tabPageAncestorParts;
        private System.Windows.Forms.TabPage tabPageDescendantParts;
        private System.Windows.Forms.TabPage tabPageOtherRelations;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ListView listViewChildren;
        private System.Windows.Forms.ListView listViewPartOf;
        private System.Windows.Forms.ListView listViewInversePartOf;
        private System.Windows.Forms.ListView listViewAncestorParts;
        private System.Windows.Forms.ListView listViewDescendantParts;
        private System.Windows.Forms.ListView listViewOtherRelations;
    }
}

