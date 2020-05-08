using System.Windows.Forms;

namespace RegScoreCalc
{
	partial class PaneColumnsRegEx
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaneColumnsRegEx));
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
            this.tabControlColRegExp = new RegScoreCalc.CustomTabControl();
            this.tabPageRegExp = new RegScoreCalc.CustomTabPage();
            this.regExpDataGridView = new System.Windows.Forms.DataGridView();
            this.tabPageScript = new RegScoreCalc.CustomTabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fastColoredTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.checkBoxExtractValues = new System.Windows.Forms.CheckBox();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnRunOnNextDoc = new RegScoreCalc.RibbonStyleButton();
            this.btnRunOnCurrentDoc = new RegScoreCalc.RibbonStyleButton();
            this.tabPagePython = new RegScoreCalc.CustomTabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.comboNoteTextColumn = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenPythonFile = new RegScoreCalc.RibbonStyleButton();
            this.textBoxPythonFile = new System.Windows.Forms.TextBox();
            this.checkBoxExtractPython = new System.Windows.Forms.CheckBox();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonAddToExceptions = new System.Windows.Forms.ToolStripButton();
            this.btnHelp = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.menuOperations.SuspendLayout();
            this.menuRegExp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.SuspendLayout();
            this.tabControlColRegExp.SuspendLayout();
            this.tabPageRegExp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).BeginInit();
            this.tabPageScript.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.tabPagePython.SuspendLayout();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuOperations
            // 
            this.menuOperations.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemEditRegExp,
            this.menuitemCopyRegExp,
            this.toolStripSeparator2,
            this.menuitemDeleteRows});
            this.menuOperations.Name = "menuOperations";
            this.menuOperations.Size = new System.Drawing.Size(205, 76);
            // 
            // menuitemEditRegExp
            // 
            this.menuitemEditRegExp.Name = "menuitemEditRegExp";
            this.menuitemEditRegExp.Size = new System.Drawing.Size(204, 22);
            this.menuitemEditRegExp.Text = "Edit Regular Expression";
            // 
            // menuitemCopyRegExp
            // 
            this.menuitemCopyRegExp.Name = "menuitemCopyRegExp";
            this.menuitemCopyRegExp.Size = new System.Drawing.Size(204, 22);
            this.menuitemCopyRegExp.Text = "Copy Regular Expression";
            this.menuitemCopyRegExp.Click += new System.EventHandler(this.menuitemCopyRegExp_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(201, 6);
            // 
            // menuitemDeleteRows
            // 
            this.menuitemDeleteRows.Name = "menuitemDeleteRows";
            this.menuitemDeleteRows.Size = new System.Drawing.Size(204, 22);
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
            this.menuRegExp.Size = new System.Drawing.Size(159, 98);
            // 
            // menuitemCopy
            // 
            this.menuitemCopy.Name = "menuitemCopy";
            this.menuitemCopy.Size = new System.Drawing.Size(158, 22);
            this.menuitemCopy.Text = "Copy";
            this.menuitemCopy.Click += new System.EventHandler(this.menuitemCopy_Click);
            // 
            // menuitemCut
            // 
            this.menuitemCut.Name = "menuitemCut";
            this.menuitemCut.Size = new System.Drawing.Size(158, 22);
            this.menuitemCut.Text = "Cut";
            this.menuitemCut.Click += new System.EventHandler(this.menuitemCut_Click);
            // 
            // menuitemPaste
            // 
            this.menuitemPaste.Name = "menuitemPaste";
            this.menuitemPaste.Size = new System.Drawing.Size(158, 22);
            this.menuitemPaste.Text = "Paste";
            this.menuitemPaste.Click += new System.EventHandler(this.menuitemPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(155, 6);
            // 
            // regExpOptionsToolStripMenuItem
            // 
            this.regExpOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemWordBoundaries});
            this.regExpOptionsToolStripMenuItem.Name = "regExpOptionsToolStripMenuItem";
            this.regExpOptionsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
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
            this.splitter.Panel1.Controls.Add(this.tabControlColRegExp);
            this.splitter.Panel1.Controls.Add(this.toolStripTop);
            this.splitter.Panel1.Controls.Add(this.flowLayoutPanel);
            this.splitter.Size = new System.Drawing.Size(989, 487);
            this.splitter.SplitterDistance = 531;
            this.splitter.TabIndex = 5;
            // 
            // tabControlColRegExp
            // 
            this.tabControlColRegExp.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControlColRegExp.Controls.Add(this.tabPageRegExp);
            this.tabControlColRegExp.Controls.Add(this.tabPageScript);
            this.tabControlColRegExp.Controls.Add(this.tabPagePython);
            this.tabControlColRegExp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlColRegExp.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlColRegExp.Location = new System.Drawing.Point(0, 38);
            this.tabControlColRegExp.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlColRegExp.Multiline = true;
            this.tabControlColRegExp.Name = "tabControlColRegExp";
            this.tabControlColRegExp.SelectedIndex = 0;
            this.tabControlColRegExp.ShowIndicators = false;
            this.tabControlColRegExp.Size = new System.Drawing.Size(531, 449);
            this.tabControlColRegExp.TabIndex = 4;
            // 
            // tabPageRegExp
            // 
            this.tabPageRegExp.Controls.Add(this.regExpDataGridView);
            this.tabPageRegExp.Location = new System.Drawing.Point(4, 4);
            this.tabPageRegExp.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageRegExp.Name = "tabPageRegExp";
            this.tabPageRegExp.Size = new System.Drawing.Size(523, 420);
            this.tabPageRegExp.TabIndex = 0;
            this.tabPageRegExp.Text = "RegExp";
            this.tabPageRegExp.UseVisualStyleBackColor = true;
            // 
            // regExpDataGridView
            // 
            this.regExpDataGridView.AllowUserToAddRows = false;
            this.regExpDataGridView.AllowUserToResizeRows = false;
            this.regExpDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.regExpDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.regExpDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.regExpDataGridView.Location = new System.Drawing.Point(0, 0);
            this.regExpDataGridView.Margin = new System.Windows.Forms.Padding(0);
            this.regExpDataGridView.MultiSelect = false;
            this.regExpDataGridView.Name = "regExpDataGridView";
            this.regExpDataGridView.RowHeadersWidth = 20;
            this.regExpDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.regExpDataGridView.Size = new System.Drawing.Size(523, 420);
            this.regExpDataGridView.TabIndex = 2;
            this.regExpDataGridView.VirtualMode = true;
            this.regExpDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellClick);
            this.regExpDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellDoubleClick);
            this.regExpDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.regExpDataGridView_CellFormatting);
            this.regExpDataGridView.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellLeave);
            this.regExpDataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.regExpDataGridView_CellMouseClick);
            this.regExpDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.regExpDataGridView_CellMouseDoubleClick);
            this.regExpDataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.regExpDataGridView_CellValidated);
            this.regExpDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.regExpDataGridView_DataBindingComplete);
            this.regExpDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.regExpDataGridView_DataError);
            this.regExpDataGridView.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.regExpDataGridView_UserDeletedRow);
            this.regExpDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.regExpDataGridView_MouseClick);
            // 
            // tabPageScript
            // 
            this.tabPageScript.Controls.Add(this.panel1);
            this.tabPageScript.Controls.Add(this.checkBoxExtractValues);
            this.tabPageScript.Controls.Add(this.panelButtons);
            this.tabPageScript.Location = new System.Drawing.Point(4, 4);
            this.tabPageScript.Name = "tabPageScript";
            this.tabPageScript.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageScript.Size = new System.Drawing.Size(523, 420);
            this.tabPageScript.TabIndex = 1;
            this.tabPageScript.Text = "Script";
            this.tabPageScript.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.fastColoredTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 347);
            this.panel1.TabIndex = 3;
            // 
            // fastColoredTextBox
            // 
            this.fastColoredTextBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBox.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n^\\s*(case|default)\\s*[^:]" +
    "*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.fastColoredTextBox.AutoScrollMinSize = new System.Drawing.Size(2, 14);
            this.fastColoredTextBox.BackBrush = null;
            this.fastColoredTextBox.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.fastColoredTextBox.CharHeight = 14;
            this.fastColoredTextBox.CharWidth = 8;
            this.fastColoredTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fastColoredTextBox.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.fastColoredTextBox.IsReplaceMode = false;
            this.fastColoredTextBox.Language = FastColoredTextBoxNS.Language.CSharp;
            this.fastColoredTextBox.LeftBracket = '(';
            this.fastColoredTextBox.LeftBracket2 = '{';
            this.fastColoredTextBox.Location = new System.Drawing.Point(0, 0);
            this.fastColoredTextBox.Name = "fastColoredTextBox";
            this.fastColoredTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBox.RightBracket = ')';
            this.fastColoredTextBox.RightBracket2 = '}';
            this.fastColoredTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBox.ServiceColors")));
            this.fastColoredTextBox.Size = new System.Drawing.Size(515, 345);
            this.fastColoredTextBox.TabIndex = 2;
            this.fastColoredTextBox.Zoom = 100;
            // 
            // checkBoxExtractValues
            // 
            this.checkBoxExtractValues.AutoSize = true;
            this.checkBoxExtractValues.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.checkBoxExtractValues.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxExtractValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxExtractValues.Location = new System.Drawing.Point(3, 3);
            this.checkBoxExtractValues.Name = "checkBoxExtractValues";
            this.checkBoxExtractValues.Size = new System.Drawing.Size(517, 24);
            this.checkBoxExtractValues.TabIndex = 0;
            this.checkBoxExtractValues.Text = "Use script to extract values";
            this.checkBoxExtractValues.UseVisualStyleBackColor = false;
            // 
            // panelButtons
            // 
            this.panelButtons.BackColor = System.Drawing.Color.Gainsboro;
            this.panelButtons.Controls.Add(this.btnRunOnNextDoc);
            this.panelButtons.Controls.Add(this.btnRunOnCurrentDoc);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Location = new System.Drawing.Point(3, 374);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(517, 43);
            this.panelButtons.TabIndex = 1;
            // 
            // btnRunOnNextDoc
            // 
            this.btnRunOnNextDoc.DrawNormalBorder = false;
            this.btnRunOnNextDoc.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunOnNextDoc.HoverImage = null;
            this.btnRunOnNextDoc.IsHighlighted = false;
            this.btnRunOnNextDoc.Location = new System.Drawing.Point(235, 6);
            this.btnRunOnNextDoc.Name = "btnRunOnNextDoc";
            this.btnRunOnNextDoc.NormalImage = null;
            this.btnRunOnNextDoc.PressedImage = null;
            this.btnRunOnNextDoc.Size = new System.Drawing.Size(226, 30);
            this.btnRunOnNextDoc.TabIndex = 1;
            this.btnRunOnNextDoc.Text = "Run on next document";
            this.btnRunOnNextDoc.UseVisualStyleBackColor = true;
            this.btnRunOnNextDoc.Click += new System.EventHandler(this.btnRunOnNextDoc_Click);
            // 
            // btnRunOnCurrentDoc
            // 
            this.btnRunOnCurrentDoc.DrawNormalBorder = false;
            this.btnRunOnCurrentDoc.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunOnCurrentDoc.HoverImage = null;
            this.btnRunOnCurrentDoc.IsHighlighted = false;
            this.btnRunOnCurrentDoc.Location = new System.Drawing.Point(3, 6);
            this.btnRunOnCurrentDoc.Name = "btnRunOnCurrentDoc";
            this.btnRunOnCurrentDoc.NormalImage = null;
            this.btnRunOnCurrentDoc.PressedImage = null;
            this.btnRunOnCurrentDoc.Size = new System.Drawing.Size(226, 30);
            this.btnRunOnCurrentDoc.TabIndex = 0;
            this.btnRunOnCurrentDoc.Text = "Run on current document";
            this.btnRunOnCurrentDoc.UseVisualStyleBackColor = true;
            this.btnRunOnCurrentDoc.Click += new System.EventHandler(this.btnRunOnCurrentDoc_Click);
            // 
            // tabPagePython
            // 
            this.tabPagePython.Controls.Add(this.label2);
            this.tabPagePython.Controls.Add(this.comboNoteTextColumn);
            this.tabPagePython.Controls.Add(this.label1);
            this.tabPagePython.Controls.Add(this.btnOpenPythonFile);
            this.tabPagePython.Controls.Add(this.textBoxPythonFile);
            this.tabPagePython.Controls.Add(this.checkBoxExtractPython);
            this.tabPagePython.Location = new System.Drawing.Point(4, 4);
            this.tabPagePython.Name = "tabPagePython";
            this.tabPagePython.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePython.Size = new System.Drawing.Size(523, 420);
            this.tabPagePython.TabIndex = 2;
            this.tabPagePython.Text = "Python";
            this.tabPagePython.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(8, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Select Column :";
            // 
            // comboNoteTextColumn
            // 
            this.comboNoteTextColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboNoteTextColumn.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboNoteTextColumn.FormattingEnabled = true;
            this.comboNoteTextColumn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboNoteTextColumn.Location = new System.Drawing.Point(8, 121);
            this.comboNoteTextColumn.Name = "comboNoteTextColumn";
            this.comboNoteTextColumn.Size = new System.Drawing.Size(415, 26);
            this.comboNoteTextColumn.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Python File :";
            // 
            // btnOpenPythonFile
            // 
            this.btnOpenPythonFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenPythonFile.DrawNormalBorder = false;
            this.btnOpenPythonFile.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenPythonFile.HoverImage = null;
            this.btnOpenPythonFile.IsHighlighted = false;
            this.btnOpenPythonFile.Location = new System.Drawing.Point(442, 62);
            this.btnOpenPythonFile.Name = "btnOpenPythonFile";
            this.btnOpenPythonFile.NormalImage = null;
            this.btnOpenPythonFile.PressedImage = null;
            this.btnOpenPythonFile.Size = new System.Drawing.Size(75, 25);
            this.btnOpenPythonFile.TabIndex = 2;
            this.btnOpenPythonFile.Text = "Open File";
            this.btnOpenPythonFile.UseVisualStyleBackColor = true;
            this.btnOpenPythonFile.Click += new System.EventHandler(this.btnOpenPythonFile_Click);
            // 
            // textBoxPythonFile
            // 
            this.textBoxPythonFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPythonFile.Location = new System.Drawing.Point(8, 62);
            this.textBoxPythonFile.Name = "textBoxPythonFile";
            this.textBoxPythonFile.Size = new System.Drawing.Size(415, 25);
            this.textBoxPythonFile.TabIndex = 1;
            // 
            // checkBoxExtractPython
            // 
            this.checkBoxExtractPython.AutoSize = true;
            this.checkBoxExtractPython.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.checkBoxExtractPython.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxExtractPython.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxExtractPython.Location = new System.Drawing.Point(3, 3);
            this.checkBoxExtractPython.Name = "checkBoxExtractPython";
            this.checkBoxExtractPython.Size = new System.Drawing.Size(517, 24);
            this.checkBoxExtractPython.TabIndex = 0;
            this.checkBoxExtractPython.Text = "Use python to extract values";
            this.checkBoxExtractPython.UseVisualStyleBackColor = true;
            // 
            // toolStripTop
            // 
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonAddToExceptions,
            this.btnHelp});
            this.toolStripTop.Location = new System.Drawing.Point(0, 4);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
            this.toolStripTop.Size = new System.Drawing.Size(531, 34);
            this.toolStripTop.Stretch = true;
            this.toolStripTop.TabIndex = 6;
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
            // btnHelp
            // 
            this.btnHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnHelp.Image = ((System.Drawing.Image)(resources.GetObject("btnHelp.Image")));
            this.btnHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(36, 28);
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.AutoSize = true;
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Padding = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel.Size = new System.Drawing.Size(531, 4);
            this.flowLayoutPanel.TabIndex = 7;
            // 
            // PaneColumnsRegEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 487);
            this.Controls.Add(this.splitter);
            this.Name = "PaneColumnsRegEx";
            this.Text = "PaneColumnsRegEx";
            this.menuOperations.ResumeLayout(false);
            this.menuRegExp.ResumeLayout(false);
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.tabControlColRegExp.ResumeLayout(false);
            this.tabPageRegExp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).EndInit();
            this.tabPageScript.ResumeLayout(false);
            this.tabPageScript.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.tabPagePython.ResumeLayout(false);
            this.tabPagePython.PerformLayout();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
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
		private ToolStrip toolStripTop;
		private ToolStripButton toolStripButtonAddToExceptions;
		private FlowLayoutPanel flowLayoutPanel;
        private ToolStripButton btnHelp;
        private CustomTabPage tabPagePython;
        private CustomTabControl tabControlColRegExp;
        private CustomTabPage tabPageRegExp;
        private DataGridView regExpDataGridView;
        private CustomTabPage tabPageScript;
        private Panel panel1;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBox;
        private CheckBox checkBoxExtractValues;
        private Panel panelButtons;
        private RibbonStyleButton btnRunOnNextDoc;
        private RibbonStyleButton btnRunOnCurrentDoc;
        private CheckBox checkBoxExtractPython;
        private Label label1;
        private RibbonStyleButton btnOpenPythonFile;
        private TextBox textBoxPythonFile;
        private ComboBox comboNoteTextColumn;
        private Label label2;
    }
}