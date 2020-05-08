namespace RegScoreCalc
{
	partial class PaneStatistics
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaneStatistics));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.txtRegExp = new System.Windows.Forms.TextBox();
			this.lvDatabases = new System.Windows.Forms.ListView();
			this.headerFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerLastModifiedDatabase = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imagesSmall = new System.Windows.Forms.ImageList(this.components);
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.regExpDataGridView = new System.Windows.Forms.DataGridView();
			this.colWord = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnSaveCurrent = new System.Windows.Forms.Button();
			this.btnSaveToDatabase = new System.Windows.Forms.Button();
			this.txtReplace = new System.Windows.Forms.TextBox();
			this.txtRegularExpressionWholeWord = new System.Windows.Forms.TextBox();
			this.btnToggleWholeWord = new System.Windows.Forms.Button();
			this.btnAddToRegExpTable = new System.Windows.Forms.Button();
			this.btnAddAllWords = new System.Windows.Forms.Button();
			this.btnCalcStatistics = new System.Windows.Forms.Button();
			this.btnAddRegExps = new System.Windows.Forms.Button();
			this.btnClearStatistics = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.lvStatistics = new System.Windows.Forms.ListView();
			this.headerException = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerWord = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerPercentage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnRemoveDatabases = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.btnGoLevelUp = new System.Windows.Forms.Button();
			this.btnAddDatabases = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.lvFileBrowser = new System.Windows.Forms.ListView();
			this.headerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.headerLastModifiedObject = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chbReplace = new System.Windows.Forms.CheckBox();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tooltip = new System.Windows.Forms.ToolTip(this.components);
			this.menuPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuitemAddToExceptions = new System.Windows.Forms.ToolStripMenuItem();
			this.paneRegExpButtons = new System.Windows.Forms.Panel();
			this.btnAnyCharacter = new RegScoreCalc.RibbonStyleButton();
			this.btnAnyDigit = new RegScoreCalc.RibbonStyleButton();
			this.btnAnyWhiteSpace = new RegScoreCalc.RibbonStyleButton();
			this.btnOptionalOnceOrNone = new RegScoreCalc.RibbonStyleButton();
			this.btnOneOrMoreTimes = new RegScoreCalc.RibbonStyleButton();
			this.btnZeroOrMultipleTimes = new RegScoreCalc.RibbonStyleButton();
			this.btnEitherOr = new RegScoreCalc.RibbonStyleButton();
			this.btnRange = new RegScoreCalc.RibbonStyleButton();
			this.btnWriteMyOwn = new RegScoreCalc.RibbonStyleButton();
			((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).BeginInit();
			this.panel1.SuspendLayout();
			this.menuPopup.SuspendLayout();
			this.paneRegExpButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtRegExp
			// 
			this.txtRegExp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRegExp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtRegExp.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtRegExp.HideSelection = false;
			this.txtRegExp.Location = new System.Drawing.Point(389, 6);
			this.txtRegExp.Name = "txtRegExp";
			this.txtRegExp.Size = new System.Drawing.Size(1405, 40);
			this.txtRegExp.TabIndex = 0;
			this.txtRegExp.Enter += new System.EventHandler(this.txtRegExp_Enter);
			this.txtRegExp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRegExp_KeyUp);
			this.txtRegExp.Leave += new System.EventHandler(this.txtRegExp_Leave);
			this.txtRegExp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtRegExp_MouseUp);
			// 
			// lvDatabases
			// 
			this.lvDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lvDatabases.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvDatabases.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerFileName,
            this.headerLastModifiedDatabase});
			this.lvDatabases.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lvDatabases.FullRowSelect = true;
			this.lvDatabases.GridLines = true;
			this.lvDatabases.HideSelection = false;
			this.lvDatabases.Location = new System.Drawing.Point(389, 176);
			this.lvDatabases.Name = "lvDatabases";
			this.lvDatabases.Size = new System.Drawing.Size(330, 808);
			this.lvDatabases.SmallImageList = this.imagesSmall;
			this.lvDatabases.TabIndex = 7;
			this.lvDatabases.UseCompatibleStateImageBehavior = false;
			this.lvDatabases.View = System.Windows.Forms.View.Details;
			this.lvDatabases.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvDatabases_KeyDown);
			// 
			// headerFileName
			// 
			this.headerFileName.Text = "File Name";
			this.headerFileName.Width = 200;
			// 
			// headerLastModifiedDatabase
			// 
			this.headerLastModifiedDatabase.Text = "Last Modified";
			this.headerLastModifiedDatabase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.headerLastModifiedDatabase.Width = 120;
			// 
			// imagesSmall
			// 
			this.imagesSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imagesSmall.ImageStream")));
			this.imagesSmall.TransparentColor = System.Drawing.Color.Transparent;
			this.imagesSmall.Images.SetKeyName(0, "Folder.ico");
			this.imagesSmall.Images.SetKeyName(1, "Database.ico");
			this.imagesSmall.Images.SetKeyName(2, "Drive.ico");
			this.imagesSmall.Images.SetKeyName(3, "Word.ico");
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(152, 14);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.MaximumSize = new System.Drawing.Size(229, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(223, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "Type regular expression:";
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(183)))));
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label2.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(389, 132);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(330, 41);
			this.label2.TabIndex = 2;
			this.label2.Text = "Database files:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// regExpDataGridView
			// 
			this.regExpDataGridView.AllowUserToAddRows = false;
			this.regExpDataGridView.AllowUserToDeleteRows = false;
			this.regExpDataGridView.AllowUserToResizeRows = false;
			this.regExpDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.regExpDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.regExpDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.regExpDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.regExpDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWord,
            this.colCount});
			this.regExpDataGridView.Location = new System.Drawing.Point(1916, 112);
			this.regExpDataGridView.Name = "regExpDataGridView";
			this.regExpDataGridView.ReadOnly = true;
			this.regExpDataGridView.RowHeadersVisible = false;
			this.regExpDataGridView.RowHeadersWidth = 20;
			this.regExpDataGridView.Size = new System.Drawing.Size(273, 808);
			this.regExpDataGridView.TabIndex = 3;
			this.regExpDataGridView.VirtualMode = true;
			// 
			// colWord
			// 
			this.colWord.HeaderText = "Word";
			this.colWord.Name = "colWord";
			this.colWord.ReadOnly = true;
			this.colWord.Width = 185;
			// 
			// colCount
			// 
			this.colCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colCount.DefaultCellStyle = dataGridViewCellStyle3;
			this.colCount.HeaderText = "Count";
			this.colCount.Name = "colCount";
			this.colCount.ReadOnly = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-40, 56);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(6669, 2);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Name";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Date";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Size";
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.btnSaveCurrent);
			this.panel1.Controls.Add(this.btnSaveToDatabase);
			this.panel1.Controls.Add(this.txtReplace);
			this.panel1.Controls.Add(this.txtRegularExpressionWholeWord);
			this.panel1.Controls.Add(this.btnToggleWholeWord);
			this.panel1.Controls.Add(this.btnAddToRegExpTable);
			this.panel1.Controls.Add(this.btnAddAllWords);
			this.panel1.Controls.Add(this.btnCalcStatistics);
			this.panel1.Controls.Add(this.btnAddRegExps);
			this.panel1.Controls.Add(this.btnClearStatistics);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.lvStatistics);
			this.panel1.Controls.Add(this.lvDatabases);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.btnRemoveDatabases);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.btnGoLevelUp);
			this.panel1.Controls.Add(this.btnAddDatabases);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.txtRegExp);
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.lvFileBrowser);
			this.panel1.Controls.Add(this.regExpDataGridView);
			this.panel1.Controls.Add(this.chbReplace);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 40);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1908, 810);
			this.panel1.TabIndex = 10;
			// 
			// btnSaveCurrent
			// 
			this.btnSaveCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSaveCurrent.BackColor = System.Drawing.Color.Transparent;
			this.btnSaveCurrent.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnSaveCurrent.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSaveCurrent.Image = global::RegScoreCalc.Properties.Resources.Save;
			this.btnSaveCurrent.Location = new System.Drawing.Point(1803, 6);
			this.btnSaveCurrent.Name = "btnSaveCurrent";
			this.btnSaveCurrent.Size = new System.Drawing.Size(46, 40);
			this.btnSaveCurrent.TabIndex = 19;
			this.tooltip.SetToolTip(this.btnSaveCurrent, "Save");
			this.btnSaveCurrent.UseVisualStyleBackColor = false;
			this.btnSaveCurrent.Click += new System.EventHandler(this.btnSaveCurrent_Click);
			// 
			// btnSaveToDatabase
			// 
			this.btnSaveToDatabase.BackColor = System.Drawing.Color.Transparent;
			this.btnSaveToDatabase.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnSaveToDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnSaveToDatabase.Image = global::RegScoreCalc.Properties.Resources.Save;
			this.btnSaveToDatabase.Location = new System.Drawing.Point(157, 68);
			this.btnSaveToDatabase.Name = "btnSaveToDatabase";
			this.btnSaveToDatabase.Size = new System.Drawing.Size(40, 35);
			this.btnSaveToDatabase.TabIndex = 18;
			this.tooltip.SetToolTip(this.btnSaveToDatabase, "Save");
			this.btnSaveToDatabase.UseVisualStyleBackColor = false;
			this.btnSaveToDatabase.Click += new System.EventHandler(this.btnSaveToDatabase_Click);
			// 
			// txtReplace
			// 
			this.txtReplace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtReplace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtReplace.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtReplace.Location = new System.Drawing.Point(389, 83);
			this.txtReplace.Name = "txtReplace";
			this.txtReplace.Size = new System.Drawing.Size(1906, 40);
			this.txtReplace.TabIndex = 16;
			// 
			// txtRegularExpressionWholeWord
			// 
			this.txtRegularExpressionWholeWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRegularExpressionWholeWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtRegularExpressionWholeWord.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtRegularExpressionWholeWord.Location = new System.Drawing.Point(389, 126);
			this.txtRegularExpressionWholeWord.Name = "txtRegularExpressionWholeWord";
			this.txtRegularExpressionWholeWord.ReadOnly = true;
			this.txtRegularExpressionWholeWord.Size = new System.Drawing.Size(1906, 40);
			this.txtRegularExpressionWholeWord.TabIndex = 15;
			this.txtRegularExpressionWholeWord.Visible = false;
			// 
			// btnToggleWholeWord
			// 
			this.btnToggleWholeWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnToggleWholeWord.Location = new System.Drawing.Point(17, 6);
			this.btnToggleWholeWord.Name = "btnToggleWholeWord";
			this.btnToggleWholeWord.Size = new System.Drawing.Size(124, 40);
			this.btnToggleWholeWord.TabIndex = 14;
			this.btnToggleWholeWord.Tag = "off";
			this.btnToggleWholeWord.Text = "Whole Word";
			this.btnToggleWholeWord.UseVisualStyleBackColor = true;
			this.btnToggleWholeWord.Click += new System.EventHandler(this.btnToggleWholeWord_Click);
			// 
			// btnAddToRegExpTable
			// 
			this.btnAddToRegExpTable.BackColor = System.Drawing.Color.Transparent;
			this.btnAddToRegExpTable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnAddToRegExpTable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddToRegExpTable.Image = global::RegScoreCalc.Properties.Resources.AddDatabase;
			this.btnAddToRegExpTable.Location = new System.Drawing.Point(199, 68);
			this.btnAddToRegExpTable.Name = "btnAddToRegExpTable";
			this.btnAddToRegExpTable.Size = new System.Drawing.Size(40, 35);
			this.btnAddToRegExpTable.TabIndex = 13;
			this.tooltip.SetToolTip(this.btnAddToRegExpTable, "Add to RegExp table with exceptions.");
			this.btnAddToRegExpTable.UseVisualStyleBackColor = false;
			this.btnAddToRegExpTable.Click += new System.EventHandler(this.btnAddToRegExpTable_Click);
			// 
			// btnAddAllWords
			// 
			this.btnAddAllWords.BackColor = System.Drawing.Color.Transparent;
			this.btnAddAllWords.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnAddAllWords.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddAllWords.Image = global::RegScoreCalc.Properties.Resources.AddAllRegExpsMedium;
			this.btnAddAllWords.Location = new System.Drawing.Point(241, 68);
			this.btnAddAllWords.Name = "btnAddAllWords";
			this.btnAddAllWords.Size = new System.Drawing.Size(40, 35);
			this.btnAddAllWords.TabIndex = 2;
			this.tooltip.SetToolTip(this.btnAddAllWords, "Insert all words to regular expressions");
			this.btnAddAllWords.UseVisualStyleBackColor = false;
			this.btnAddAllWords.Click += new System.EventHandler(this.btnAddAllWords_Click);
			// 
			// btnCalcStatistics
			// 
			this.btnCalcStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCalcStatistics.BackColor = System.Drawing.Color.Transparent;
			this.btnCalcStatistics.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnCalcStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCalcStatistics.Image = global::RegScoreCalc.Properties.Resources.CalcScores;
			this.btnCalcStatistics.Location = new System.Drawing.Point(1851, 6);
			this.btnCalcStatistics.Name = "btnCalcStatistics";
			this.btnCalcStatistics.Size = new System.Drawing.Size(44, 40);
			this.btnCalcStatistics.TabIndex = 1;
			this.tooltip.SetToolTip(this.btnCalcStatistics, "Calculate statistics");
			this.btnCalcStatistics.UseVisualStyleBackColor = false;
			this.btnCalcStatistics.Click += new System.EventHandler(this.btnCalcStatistics_Click);
			// 
			// btnAddRegExps
			// 
			this.btnAddRegExps.BackColor = System.Drawing.Color.Transparent;
			this.btnAddRegExps.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnAddRegExps.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddRegExps.Image = global::RegScoreCalc.Properties.Resources.AddRegExpsMedium;
			this.btnAddRegExps.Location = new System.Drawing.Point(283, 68);
			this.btnAddRegExps.Name = "btnAddRegExps";
			this.btnAddRegExps.Size = new System.Drawing.Size(40, 35);
			this.btnAddRegExps.TabIndex = 3;
			this.tooltip.SetToolTip(this.btnAddRegExps, "Insert selected words to regular expressions");
			this.btnAddRegExps.UseVisualStyleBackColor = false;
			this.btnAddRegExps.Click += new System.EventHandler(this.btnAddRegExps_Click);
			// 
			// btnClearStatistics
			// 
			this.btnClearStatistics.BackColor = System.Drawing.Color.Transparent;
			this.btnClearStatistics.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnClearStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnClearStatistics.Image = global::RegScoreCalc.Properties.Resources.RemoveStatistics;
			this.btnClearStatistics.Location = new System.Drawing.Point(325, 68);
			this.btnClearStatistics.Name = "btnClearStatistics";
			this.btnClearStatistics.Size = new System.Drawing.Size(40, 35);
			this.btnClearStatistics.TabIndex = 4;
			this.tooltip.SetToolTip(this.btnClearStatistics, "Remove selected words");
			this.btnClearStatistics.UseVisualStyleBackColor = false;
			this.btnClearStatistics.Click += new System.EventHandler(this.btnClearStatistics_Click);
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(183)))));
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label5.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(17, 65);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(352, 41);
			this.label5.TabIndex = 9;
			this.label5.Text = "Statistics:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lvStatistics
			// 
			this.lvStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lvStatistics.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvStatistics.CheckBoxes = true;
			this.lvStatistics.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerException,
            this.headerWord,
            this.headerCount,
            this.headerPercentage});
			this.lvStatistics.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lvStatistics.FullRowSelect = true;
			this.lvStatistics.GridLines = true;
			this.lvStatistics.HideSelection = false;
			this.lvStatistics.Location = new System.Drawing.Point(17, 109);
			this.lvStatistics.Name = "lvStatistics";
			this.lvStatistics.Size = new System.Drawing.Size(352, 808);
			this.lvStatistics.SmallImageList = this.imagesSmall;
			this.lvStatistics.TabIndex = 5;
			this.lvStatistics.UseCompatibleStateImageBehavior = false;
			this.lvStatistics.View = System.Windows.Forms.View.Details;
			this.lvStatistics.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvStatistics_ColumnClick);
			this.lvStatistics.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvStatistics_KeyDown);
			// 
			// headerException
			// 
			this.headerException.Text = "Ex";
			this.headerException.Width = 40;
			// 
			// headerWord
			// 
			this.headerWord.Text = "Word";
			this.headerWord.Width = 181;
			// 
			// headerCount
			// 
			this.headerCount.Text = "Count";
			this.headerCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// headerPercentage
			// 
			this.headerPercentage.Text = "%";
			this.headerPercentage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.headerPercentage.Width = 40;
			// 
			// btnRemoveDatabases
			// 
			this.btnRemoveDatabases.BackColor = System.Drawing.Color.Transparent;
			this.btnRemoveDatabases.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnRemoveDatabases.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRemoveDatabases.Image = global::RegScoreCalc.Properties.Resources.RemoveDatabase;
			this.btnRemoveDatabases.Location = new System.Drawing.Point(675, 135);
			this.btnRemoveDatabases.Name = "btnRemoveDatabases";
			this.btnRemoveDatabases.Size = new System.Drawing.Size(40, 35);
			this.btnRemoveDatabases.TabIndex = 6;
			this.tooltip.SetToolTip(this.btnRemoveDatabases, "Remove selected files");
			this.btnRemoveDatabases.UseVisualStyleBackColor = false;
			this.btnRemoveDatabases.Click += new System.EventHandler(this.btnRemoveDatabases_Click);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.Location = new System.Drawing.Point(1916, 68);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(273, 41);
			this.label4.TabIndex = 12;
			this.label4.Text = "Database files:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnGoLevelUp
			// 
			this.btnGoLevelUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGoLevelUp.BackColor = System.Drawing.Color.Transparent;
			this.btnGoLevelUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnGoLevelUp.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnGoLevelUp.Image = global::RegScoreCalc.Properties.Resources.GoLevelUp;
			this.btnGoLevelUp.Location = new System.Drawing.Point(1810, 135);
			this.btnGoLevelUp.Name = "btnGoLevelUp";
			this.btnGoLevelUp.Size = new System.Drawing.Size(40, 35);
			this.btnGoLevelUp.TabIndex = 8;
			this.tooltip.SetToolTip(this.btnGoLevelUp, "InvokeNavigate to parent folder");
			this.btnGoLevelUp.UseVisualStyleBackColor = false;
			this.btnGoLevelUp.Click += new System.EventHandler(this.btnGoLevelUp_Click);
			// 
			// btnAddDatabases
			// 
			this.btnAddDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddDatabases.BackColor = System.Drawing.Color.Transparent;
			this.btnAddDatabases.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Blue;
			this.btnAddDatabases.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnAddDatabases.Image = global::RegScoreCalc.Properties.Resources.AddDatabase;
			this.btnAddDatabases.Location = new System.Drawing.Point(1851, 135);
			this.btnAddDatabases.Name = "btnAddDatabases";
			this.btnAddDatabases.Size = new System.Drawing.Size(40, 35);
			this.btnAddDatabases.TabIndex = 9;
			this.tooltip.SetToolTip(this.btnAddDatabases, "Add selected databases to list");
			this.btnAddDatabases.UseVisualStyleBackColor = false;
			this.btnAddDatabases.Click += new System.EventHandler(this.btnAddDatabases_Click);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(183)))));
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Font = new System.Drawing.Font("Calibri", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(739, 132);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(1906, 41);
			this.label3.TabIndex = 5;
			this.label3.Text = "File browser:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lvFileBrowser
			// 
			this.lvFileBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvFileBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lvFileBrowser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerName,
            this.headerSize,
            this.headerLastModifiedObject});
			this.lvFileBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lvFileBrowser.FullRowSelect = true;
			this.lvFileBrowser.GridLines = true;
			this.lvFileBrowser.HideSelection = false;
			this.lvFileBrowser.Location = new System.Drawing.Point(739, 176);
			this.lvFileBrowser.Name = "lvFileBrowser";
			this.lvFileBrowser.Size = new System.Drawing.Size(1906, 808);
			this.lvFileBrowser.SmallImageList = this.imagesSmall;
			this.lvFileBrowser.TabIndex = 10;
			this.lvFileBrowser.UseCompatibleStateImageBehavior = false;
			this.lvFileBrowser.View = System.Windows.Forms.View.Details;
			this.lvFileBrowser.ItemActivate += new System.EventHandler(this.lvFileBrowser_ItemActivate);
			this.lvFileBrowser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvFileBrowser_KeyDown);
			// 
			// headerName
			// 
			this.headerName.Text = "Name";
			this.headerName.Width = 250;
			// 
			// headerSize
			// 
			this.headerSize.Text = "Size";
			this.headerSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.headerSize.Width = 100;
			// 
			// headerLastModifiedObject
			// 
			this.headerLastModifiedObject.Text = "Last Modified";
			this.headerLastModifiedObject.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.headerLastModifiedObject.Width = 120;
			// 
			// chbReplace
			// 
			this.chbReplace.AutoSize = true;
			this.chbReplace.Location = new System.Drawing.Point(390, 56);
			this.chbReplace.Name = "chbReplace";
			this.chbReplace.Size = new System.Drawing.Size(106, 28);
			this.chbReplace.TabIndex = 17;
			this.chbReplace.Text = "Replace";
			this.chbReplace.UseVisualStyleBackColor = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.HeaderText = "Word";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.Width = 185;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridViewTextBoxColumn2.HeaderText = "Count";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// menuPopup
			// 
			this.menuPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuitemAddToExceptions});
			this.menuPopup.Name = "menuPopup";
			this.menuPopup.Size = new System.Drawing.Size(266, 26);
			// 
			// menuitemAddToExceptions
			// 
			this.menuitemAddToExceptions.Name = "menuitemAddToExceptions";
			this.menuitemAddToExceptions.Size = new System.Drawing.Size(265, 22);
			this.menuitemAddToExceptions.Text = "Add to RegExp table with exceptions";
			// 
			// paneRegExpButtons
			// 
			this.paneRegExpButtons.Controls.Add(this.btnAnyCharacter);
			this.paneRegExpButtons.Controls.Add(this.btnAnyDigit);
			this.paneRegExpButtons.Controls.Add(this.btnAnyWhiteSpace);
			this.paneRegExpButtons.Controls.Add(this.btnOptionalOnceOrNone);
			this.paneRegExpButtons.Controls.Add(this.btnOneOrMoreTimes);
			this.paneRegExpButtons.Controls.Add(this.btnZeroOrMultipleTimes);
			this.paneRegExpButtons.Controls.Add(this.btnEitherOr);
			this.paneRegExpButtons.Controls.Add(this.btnRange);
			this.paneRegExpButtons.Controls.Add(this.btnWriteMyOwn);
			this.paneRegExpButtons.Dock = System.Windows.Forms.DockStyle.Top;
			this.paneRegExpButtons.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.paneRegExpButtons.Location = new System.Drawing.Point(0, 0);
			this.paneRegExpButtons.Name = "paneRegExpButtons";
			this.paneRegExpButtons.Size = new System.Drawing.Size(1908, 40);
			this.paneRegExpButtons.TabIndex = 11;
			// 
			// btnAnyCharacter
			// 
			this.btnAnyCharacter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.btnAnyCharacter.Enabled = false;
			this.btnAnyCharacter.HoverImage = null;
			this.btnAnyCharacter.Location = new System.Drawing.Point(3, 5);
			this.btnAnyCharacter.Name = "btnAnyCharacter";
			this.btnAnyCharacter.NormalImage = null;
			this.btnAnyCharacter.PressedImage = null;
			this.btnAnyCharacter.Size = new System.Drawing.Size(139, 28);
			this.btnAnyCharacter.TabIndex = 0;
			this.btnAnyCharacter.Text = "Any character";
			this.btnAnyCharacter.UseVisualStyleBackColor = true;
			this.btnAnyCharacter.Click += new System.EventHandler(this.btnAnyCharacter_Click);
			// 
			// btnAnyDigit
			// 
			this.btnAnyDigit.Enabled = false;
			this.btnAnyDigit.HoverImage = null;
			this.btnAnyDigit.Location = new System.Drawing.Point(148, 5);
			this.btnAnyDigit.Name = "btnAnyDigit";
			this.btnAnyDigit.NormalImage = null;
			this.btnAnyDigit.PressedImage = null;
			this.btnAnyDigit.Size = new System.Drawing.Size(92, 28);
			this.btnAnyDigit.TabIndex = 1;
			this.btnAnyDigit.Text = "Any digit";
			this.btnAnyDigit.UseVisualStyleBackColor = true;
			this.btnAnyDigit.Click += new System.EventHandler(this.btnAnyDigit_Click);
			// 
			// btnAnyWhiteSpace
			// 
			this.btnAnyWhiteSpace.Enabled = false;
			this.btnAnyWhiteSpace.HoverImage = null;
			this.btnAnyWhiteSpace.Location = new System.Drawing.Point(246, 5);
			this.btnAnyWhiteSpace.Name = "btnAnyWhiteSpace";
			this.btnAnyWhiteSpace.NormalImage = null;
			this.btnAnyWhiteSpace.PressedImage = null;
			this.btnAnyWhiteSpace.Size = new System.Drawing.Size(153, 28);
			this.btnAnyWhiteSpace.TabIndex = 2;
			this.btnAnyWhiteSpace.Text = "Any white space";
			this.btnAnyWhiteSpace.UseVisualStyleBackColor = true;
			this.btnAnyWhiteSpace.Click += new System.EventHandler(this.btnAnyWhiteSpace_Click);
			// 
			// btnOptionalOnceOrNone
			// 
			this.btnOptionalOnceOrNone.Enabled = false;
			this.btnOptionalOnceOrNone.HoverImage = null;
			this.btnOptionalOnceOrNone.Location = new System.Drawing.Point(405, 5);
			this.btnOptionalOnceOrNone.Name = "btnOptionalOnceOrNone";
			this.btnOptionalOnceOrNone.NormalImage = null;
			this.btnOptionalOnceOrNone.PressedImage = null;
			this.btnOptionalOnceOrNone.Size = new System.Drawing.Size(261, 28);
			this.btnOptionalOnceOrNone.TabIndex = 3;
			this.btnOptionalOnceOrNone.Text = "Optional (Either once, or none)";
			this.btnOptionalOnceOrNone.UseVisualStyleBackColor = true;
			this.btnOptionalOnceOrNone.Click += new System.EventHandler(this.btnOptionalOnceOrNone_Click);
			// 
			// btnOneOrMoreTimes
			// 
			this.btnOneOrMoreTimes.Enabled = false;
			this.btnOneOrMoreTimes.HoverImage = null;
			this.btnOneOrMoreTimes.Location = new System.Drawing.Point(672, 5);
			this.btnOneOrMoreTimes.Name = "btnOneOrMoreTimes";
			this.btnOneOrMoreTimes.NormalImage = null;
			this.btnOneOrMoreTimes.PressedImage = null;
			this.btnOneOrMoreTimes.Size = new System.Drawing.Size(162, 28);
			this.btnOneOrMoreTimes.TabIndex = 4;
			this.btnOneOrMoreTimes.Text = "One or more times";
			this.btnOneOrMoreTimes.UseVisualStyleBackColor = true;
			this.btnOneOrMoreTimes.Click += new System.EventHandler(this.btnOneOrMoreTimes_Click);
			// 
			// btnZeroOrMultipleTimes
			// 
			this.btnZeroOrMultipleTimes.Enabled = false;
			this.btnZeroOrMultipleTimes.HoverImage = null;
			this.btnZeroOrMultipleTimes.Location = new System.Drawing.Point(840, 5);
			this.btnZeroOrMultipleTimes.Name = "btnZeroOrMultipleTimes";
			this.btnZeroOrMultipleTimes.NormalImage = null;
			this.btnZeroOrMultipleTimes.PressedImage = null;
			this.btnZeroOrMultipleTimes.Size = new System.Drawing.Size(166, 28);
			this.btnZeroOrMultipleTimes.TabIndex = 5;
			this.btnZeroOrMultipleTimes.Text = "Zero or more times";
			this.btnZeroOrMultipleTimes.UseVisualStyleBackColor = true;
			this.btnZeroOrMultipleTimes.Click += new System.EventHandler(this.btnZeroOrMultipleTimes_Click);
			// 
			// btnEitherOr
			// 
			this.btnEitherOr.Enabled = false;
			this.btnEitherOr.HoverImage = null;
			this.btnEitherOr.Location = new System.Drawing.Point(1012, 5);
			this.btnEitherOr.Name = "btnEitherOr";
			this.btnEitherOr.NormalImage = null;
			this.btnEitherOr.PressedImage = null;
			this.btnEitherOr.Size = new System.Drawing.Size(85, 28);
			this.btnEitherOr.TabIndex = 6;
			this.btnEitherOr.Text = "Either or";
			this.btnEitherOr.UseVisualStyleBackColor = true;
			this.btnEitherOr.Click += new System.EventHandler(this.btnEitherOr_Click);
			// 
			// btnRange
			// 
			this.btnRange.Enabled = false;
			this.btnRange.HoverImage = null;
			this.btnRange.Location = new System.Drawing.Point(1103, 5);
			this.btnRange.Name = "btnRange";
			this.btnRange.NormalImage = null;
			this.btnRange.PressedImage = null;
			this.btnRange.Size = new System.Drawing.Size(71, 28);
			this.btnRange.TabIndex = 7;
			this.btnRange.Text = "Range";
			this.btnRange.UseVisualStyleBackColor = true;
			this.btnRange.Click += new System.EventHandler(this.btnRange_Click);
			// 
			// btnWriteMyOwn
			// 
			this.btnWriteMyOwn.HoverImage = null;
			this.btnWriteMyOwn.Location = new System.Drawing.Point(1180, 5);
			this.btnWriteMyOwn.Name = "btnWriteMyOwn";
			this.btnWriteMyOwn.NormalImage = null;
			this.btnWriteMyOwn.PressedImage = null;
			this.btnWriteMyOwn.Size = new System.Drawing.Size(153, 28);
			this.btnWriteMyOwn.TabIndex = 8;
			this.btnWriteMyOwn.Text = "Will write my own";
			this.btnWriteMyOwn.UseVisualStyleBackColor = true;
			this.btnWriteMyOwn.Click += new System.EventHandler(this.btnWriteMyOwn_Click);
			// 
			// PaneStatistics
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1908, 850);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.paneRegExpButtons);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
			this.Name = "PaneStatistics";
			this.Text = "PaneStatistics";
			this.Load += new System.EventHandler(this.PaneStatistics_Load);
			((System.ComponentModel.ISupportInitialize)(this.regExpDataGridView)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.menuPopup.ResumeLayout(false);
			this.paneRegExpButtons.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox txtRegExp;
		private System.Windows.Forms.ListView lvDatabases;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
		private System.Windows.Forms.DataGridViewTextBoxColumn colWord;
		private System.Windows.Forms.DataGridView regExpDataGridView;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Button btnAddDatabases;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnRemoveDatabases;
		private System.Windows.Forms.ListView lvStatistics;
		private System.Windows.Forms.Button btnClearStatistics;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button btnGoLevelUp;
		private System.Windows.Forms.ColumnHeader headerWord;
		private System.Windows.Forms.ColumnHeader headerCount;
		private System.Windows.Forms.ListView lvFileBrowser;
		private System.Windows.Forms.ColumnHeader headerName;
		private System.Windows.Forms.ColumnHeader headerSize;
		private System.Windows.Forms.ColumnHeader headerLastModifiedObject;
		private System.Windows.Forms.ImageList imagesSmall;
		private System.Windows.Forms.ColumnHeader headerFileName;
		private System.Windows.Forms.ColumnHeader headerLastModifiedDatabase;
		private System.Windows.Forms.Button btnCalcStatistics;
		private System.Windows.Forms.Button btnAddRegExps;
		private System.Windows.Forms.ColumnHeader headerPercentage;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.ToolTip tooltip;
		private System.Windows.Forms.Button btnAddAllWords;
		private System.Windows.Forms.ContextMenuStrip menuPopup;
		private System.Windows.Forms.ToolStripMenuItem menuitemAddToExceptions;
        private System.Windows.Forms.Button btnToggleWholeWord;
        private System.Windows.Forms.Button btnAddToRegExpTable;
        private System.Windows.Forms.TextBox txtRegularExpressionWholeWord;
        private System.Windows.Forms.ColumnHeader headerException;
        private System.Windows.Forms.TextBox txtReplace;
        private System.Windows.Forms.CheckBox chbReplace;
        private System.Windows.Forms.Button btnSaveToDatabase;
        private System.Windows.Forms.Button btnSaveCurrent;
        private System.Windows.Forms.Panel paneRegExpButtons;
        private RibbonStyleButton btnAnyCharacter;
		private RibbonStyleButton btnAnyDigit;
		private RibbonStyleButton btnAnyWhiteSpace;
		private RibbonStyleButton btnOptionalOnceOrNone;
		private RibbonStyleButton btnOneOrMoreTimes;
		private RibbonStyleButton btnZeroOrMultipleTimes;
		private RibbonStyleButton btnEitherOr;
		private RibbonStyleButton btnRange;
		private RibbonStyleButton btnWriteMyOwn;
	}
}