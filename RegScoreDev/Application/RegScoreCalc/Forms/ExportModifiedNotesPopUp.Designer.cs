namespace RegScoreCalc.Forms
{
    partial class ExportModifiedNotesPopUp
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
            this.chkbExportDocumentsWithCategory = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutputFileName = new System.Windows.Forms.TextBox();
            this.btnCancel = new RegScoreCalc.RibbonStyleButton();
            this.btnOk = new RegScoreCalc.RibbonStyleButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new RegScoreCalc.RibbonStyleButton();
            this.lbCategories = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new RegScoreCalc.RibbonStyleButton();
            this.btnDeselectAll = new RegScoreCalc.RibbonStyleButton();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdbXmlExport = new System.Windows.Forms.RadioButton();
            this.rdbExcelExport = new System.Windows.Forms.RadioButton();
            this.groupCriteria = new System.Windows.Forms.GroupBox();
            this.rdbExportPositiveOrNegative = new System.Windows.Forms.RadioButton();
            this.rdbExportPositive = new System.Windows.Forms.RadioButton();
            this.rdbExportAll = new System.Windows.Forms.RadioButton();
            this.groupCategories = new System.Windows.Forms.GroupBox();
            this.groupOutput = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtNumberOfLines = new System.Windows.Forms.NumericUpDown();
            this.chkbHighlightMatches = new System.Windows.Forms.CheckBox();
            this.chkbIncludePrefixSuffix = new System.Windows.Forms.CheckBox();
            this.groupNotes = new System.Windows.Forms.GroupBox();
            this.rdbJsonExport = new System.Windows.Forms.RadioButton();
            this.listSelectedColumns = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.groupColumns = new System.Windows.Forms.GroupBox();
            this.listColumns = new System.Windows.Forms.ListBox();
            this.groupCriteria.SuspendLayout();
            this.groupCategories.SuspendLayout();
            this.groupOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumberOfLines)).BeginInit();
            this.groupNotes.SuspendLayout();
            this.groupColumns.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkbExportDocumentsWithCategory
            // 
            this.chkbExportDocumentsWithCategory.AutoSize = true;
            this.chkbExportDocumentsWithCategory.Location = new System.Drawing.Point(12, 0);
            this.chkbExportDocumentsWithCategory.Name = "chkbExportDocumentsWithCategory";
            this.chkbExportDocumentsWithCategory.Size = new System.Drawing.Size(226, 17);
            this.chkbExportDocumentsWithCategory.TabIndex = 0;
            this.chkbExportDocumentsWithCategory.Text = " Export documents with assigned Category";
            this.chkbExportDocumentsWithCategory.UseVisualStyleBackColor = true;
            this.chkbExportDocumentsWithCategory.CheckedChanged += new System.EventHandler(this.chbExportDocumentsWithCategory_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "File name:";
            // 
            // txtOutputFileName
            // 
            this.txtOutputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFileName.Location = new System.Drawing.Point(70, 47);
            this.txtOutputFileName.Name = "txtOutputFileName";
            this.txtOutputFileName.Size = new System.Drawing.Size(308, 20);
            this.txtOutputFileName.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.DrawNormalBorder = false;
            this.btnCancel.HoverImage = null;
            this.btnCancel.IsHighlighted = false;
            this.btnCancel.Location = new System.Drawing.Point(403, 728);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormalImage = null;
            this.btnCancel.PressedImage = null;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.DrawNormalBorder = false;
            this.btnOk.HoverImage = null;
            this.btnOk.IsHighlighted = false;
            this.btnOk.Location = new System.Drawing.Point(322, 728);
            this.btnOk.Name = "btnOk";
            this.btnOk.NormalImage = null;
            this.btnOk.PressedImage = null;
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(-34, 718);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(558, 2);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.DrawNormalBorder = false;
            this.btnBrowse.HoverImage = null;
            this.btnBrowse.IsHighlighted = false;
            this.btnBrowse.Location = new System.Drawing.Point(384, 19);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.NormalImage = null;
            this.btnBrowse.PressedImage = null;
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lbCategories
            // 
            this.lbCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCategories.FormattingEnabled = true;
            this.lbCategories.IntegralHeight = false;
            this.lbCategories.Location = new System.Drawing.Point(9, 22);
            this.lbCategories.Name = "lbCategories";
            this.lbCategories.Size = new System.Drawing.Size(449, 161);
            this.lbCategories.TabIndex = 1;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectAll.DrawNormalBorder = false;
            this.btnSelectAll.HoverImage = null;
            this.btnSelectAll.IsHighlighted = false;
            this.btnSelectAll.Location = new System.Drawing.Point(9, 189);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.NormalImage = null;
            this.btnSelectAll.PressedImage = null;
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeselectAll.DrawNormalBorder = false;
            this.btnDeselectAll.HoverImage = null;
            this.btnDeselectAll.IsHighlighted = false;
            this.btnDeselectAll.Location = new System.Drawing.Point(90, 189);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.NormalImage = null;
            this.btnDeselectAll.PressedImage = null;
            this.btnDeselectAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeselectAll.TabIndex = 3;
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(70, 21);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(308, 20);
            this.txtOutputFolder.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Folder:";
            // 
            // rdbXmlExport
            // 
            this.rdbXmlExport.AutoSize = true;
            this.rdbXmlExport.Checked = true;
            this.rdbXmlExport.Location = new System.Drawing.Point(12, 12);
            this.rdbXmlExport.Name = "rdbXmlExport";
            this.rdbXmlExport.Size = new System.Drawing.Size(92, 17);
            this.rdbXmlExport.TabIndex = 0;
            this.rdbXmlExport.TabStop = true;
            this.rdbXmlExport.Text = "Export to XML";
            this.rdbXmlExport.UseVisualStyleBackColor = true;
            this.rdbXmlExport.CheckedChanged += new System.EventHandler(this.rdbXmlExport_CheckedChanged);
            // 
            // rdbExcelExport
            // 
            this.rdbExcelExport.AutoSize = true;
            this.rdbExcelExport.Location = new System.Drawing.Point(110, 12);
            this.rdbExcelExport.Name = "rdbExcelExport";
            this.rdbExcelExport.Size = new System.Drawing.Size(96, 17);
            this.rdbExcelExport.TabIndex = 1;
            this.rdbExcelExport.Text = "Export to Excel";
            this.rdbExcelExport.UseVisualStyleBackColor = true;
            this.rdbExcelExport.CheckedChanged += new System.EventHandler(this.rdbExcelExport_CheckedChanged);
            // 
            // groupCriteria
            // 
            this.groupCriteria.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCriteria.Controls.Add(this.rdbExportPositiveOrNegative);
            this.groupCriteria.Controls.Add(this.rdbExportPositive);
            this.groupCriteria.Controls.Add(this.rdbExportAll);
            this.groupCriteria.Location = new System.Drawing.Point(12, 143);
            this.groupCriteria.Name = "groupCriteria";
            this.groupCriteria.Size = new System.Drawing.Size(466, 94);
            this.groupCriteria.TabIndex = 5;
            this.groupCriteria.TabStop = false;
            this.groupCriteria.Text = "Export criteria";
            // 
            // rdbExportPositiveOrNegative
            // 
            this.rdbExportPositiveOrNegative.AutoSize = true;
            this.rdbExportPositiveOrNegative.Location = new System.Drawing.Point(12, 66);
            this.rdbExportPositiveOrNegative.Name = "rdbExportPositiveOrNegative";
            this.rdbExportPositiveOrNegative.Size = new System.Drawing.Size(239, 17);
            this.rdbExportPositiveOrNegative.TabIndex = 2;
            this.rdbExportPositiveOrNegative.TabStop = true;
            this.rdbExportPositiveOrNegative.Text = "Export documents positive or negative scores";
            this.rdbExportPositiveOrNegative.UseVisualStyleBackColor = true;
            this.rdbExportPositiveOrNegative.CheckedChanged += new System.EventHandler(this.rdbExportPositiveOrNegative_CheckedChanged);
            // 
            // rdbExportPositive
            // 
            this.rdbExportPositive.AutoSize = true;
            this.rdbExportPositive.Location = new System.Drawing.Point(12, 43);
            this.rdbExportPositive.Name = "rdbExportPositive";
            this.rdbExportPositive.Size = new System.Drawing.Size(183, 17);
            this.rdbExportPositive.TabIndex = 1;
            this.rdbExportPositive.TabStop = true;
            this.rdbExportPositive.Text = "Export documents positive scores";
            this.rdbExportPositive.UseVisualStyleBackColor = true;
            this.rdbExportPositive.CheckedChanged += new System.EventHandler(this.rdbExportPositive_CheckedChanged);
            // 
            // rdbExportAll
            // 
            this.rdbExportAll.AutoSize = true;
            this.rdbExportAll.Location = new System.Drawing.Point(12, 20);
            this.rdbExportAll.Name = "rdbExportAll";
            this.rdbExportAll.Size = new System.Drawing.Size(123, 17);
            this.rdbExportAll.TabIndex = 0;
            this.rdbExportAll.TabStop = true;
            this.rdbExportAll.Text = "Export all documents";
            this.rdbExportAll.UseVisualStyleBackColor = true;
            this.rdbExportAll.CheckedChanged += new System.EventHandler(this.rdbExportAll_CheckedChanged);
            // 
            // groupCategories
            // 
            this.groupCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCategories.Controls.Add(this.chkbExportDocumentsWithCategory);
            this.groupCategories.Controls.Add(this.lbCategories);
            this.groupCategories.Controls.Add(this.btnSelectAll);
            this.groupCategories.Controls.Add(this.btnDeselectAll);
            this.groupCategories.Location = new System.Drawing.Point(12, 249);
            this.groupCategories.Name = "groupCategories";
            this.groupCategories.Size = new System.Drawing.Size(466, 224);
            this.groupCategories.TabIndex = 6;
            this.groupCategories.TabStop = false;
            // 
            // groupOutput
            // 
            this.groupOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupOutput.Controls.Add(this.label2);
            this.groupOutput.Controls.Add(this.label1);
            this.groupOutput.Controls.Add(this.txtOutputFileName);
            this.groupOutput.Controls.Add(this.btnBrowse);
            this.groupOutput.Controls.Add(this.txtOutputFolder);
            this.groupOutput.Location = new System.Drawing.Point(12, 52);
            this.groupOutput.Name = "groupOutput";
            this.groupOutput.Size = new System.Drawing.Size(466, 79);
            this.groupOutput.TabIndex = 4;
            this.groupOutput.TabStop = false;
            this.groupOutput.Text = "Output";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(331, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Number of lines:";
            // 
            // txtNumberOfLines
            // 
            this.txtNumberOfLines.Location = new System.Drawing.Point(420, 12);
            this.txtNumberOfLines.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.txtNumberOfLines.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtNumberOfLines.Name = "txtNumberOfLines";
            this.txtNumberOfLines.Size = new System.Drawing.Size(54, 20);
            this.txtNumberOfLines.TabIndex = 3;
            this.txtNumberOfLines.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // chkbHighlightMatches
            // 
            this.chkbHighlightMatches.AutoSize = true;
            this.chkbHighlightMatches.Location = new System.Drawing.Point(12, 43);
            this.chkbHighlightMatches.Name = "chkbHighlightMatches";
            this.chkbHighlightMatches.Size = new System.Drawing.Size(93, 17);
            this.chkbHighlightMatches.TabIndex = 1;
            this.chkbHighlightMatches.Text = "Color matches";
            this.chkbHighlightMatches.UseVisualStyleBackColor = true;
            // 
            // chkbIncludePrefixSuffix
            // 
            this.chkbIncludePrefixSuffix.AutoSize = true;
            this.chkbIncludePrefixSuffix.Location = new System.Drawing.Point(12, 20);
            this.chkbIncludePrefixSuffix.Name = "chkbIncludePrefixSuffix";
            this.chkbIncludePrefixSuffix.Size = new System.Drawing.Size(176, 17);
            this.chkbIncludePrefixSuffix.TabIndex = 0;
            this.chkbIncludePrefixSuffix.Text = "Add prefix and suffix to matches";
            this.chkbIncludePrefixSuffix.UseVisualStyleBackColor = true;
            // 
            // groupNotes
            // 
            this.groupNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupNotes.Controls.Add(this.chkbIncludePrefixSuffix);
            this.groupNotes.Controls.Add(this.chkbHighlightMatches);
            this.groupNotes.Location = new System.Drawing.Point(12, 636);
            this.groupNotes.Name = "groupNotes";
            this.groupNotes.Size = new System.Drawing.Size(466, 68);
            this.groupNotes.TabIndex = 7;
            this.groupNotes.TabStop = false;
            this.groupNotes.Text = "Notes options";
            // 
            // rdbJsonExport
            // 
            this.rdbJsonExport.AutoSize = true;
            this.rdbJsonExport.Location = new System.Drawing.Point(212, 12);
            this.rdbJsonExport.Name = "rdbJsonExport";
            this.rdbJsonExport.Size = new System.Drawing.Size(92, 17);
            this.rdbJsonExport.TabIndex = 11;
            this.rdbJsonExport.Text = "Export to Json";
            this.rdbJsonExport.UseVisualStyleBackColor = true;
            this.rdbJsonExport.CheckedChanged += new System.EventHandler(this.rdbJsonExport_CheckedChanged);
            // 
            // listSelectedColumns
            // 
            this.listSelectedColumns.FormattingEnabled = true;
            this.listSelectedColumns.Location = new System.Drawing.Point(281, 19);
            this.listSelectedColumns.Name = "listSelectedColumns";
            this.listSelectedColumns.Size = new System.Drawing.Size(167, 121);
            this.listSelectedColumns.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(191, 34);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = ">>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(191, 109);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "<<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // groupColumns
            // 
            this.groupColumns.Controls.Add(this.listColumns);
            this.groupColumns.Controls.Add(this.btnRemove);
            this.groupColumns.Controls.Add(this.btnAdd);
            this.groupColumns.Controls.Add(this.listSelectedColumns);
            this.groupColumns.Location = new System.Drawing.Point(12, 479);
            this.groupColumns.Name = "groupColumns";
            this.groupColumns.Size = new System.Drawing.Size(466, 151);
            this.groupColumns.TabIndex = 12;
            this.groupColumns.TabStop = false;
            this.groupColumns.Text = "Columns";
            // 
            // listColumns
            // 
            this.listColumns.FormattingEnabled = true;
            this.listColumns.Location = new System.Drawing.Point(9, 19);
            this.listColumns.Name = "listColumns";
            this.listColumns.Size = new System.Drawing.Size(167, 121);
            this.listColumns.TabIndex = 4;
            // 
            // ExportModifiedNotesPopUp
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(490, 763);
            this.Controls.Add(this.groupColumns);
            this.Controls.Add(this.rdbJsonExport);
            this.Controls.Add(this.groupNotes);
            this.Controls.Add(this.txtNumberOfLines);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupOutput);
            this.Controls.Add(this.groupCategories);
            this.Controls.Add(this.groupCriteria);
            this.Controls.Add(this.rdbExcelExport);
            this.Controls.Add(this.rdbXmlExport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(432, 627);
            this.Name = "ExportModifiedNotesPopUp";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Export Modified Notes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportModifiedNotesPopUp_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.groupCriteria.ResumeLayout(false);
            this.groupCriteria.PerformLayout();
            this.groupCategories.ResumeLayout(false);
            this.groupCategories.PerformLayout();
            this.groupOutput.ResumeLayout(false);
            this.groupOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNumberOfLines)).EndInit();
            this.groupNotes.ResumeLayout(false);
            this.groupNotes.PerformLayout();
            this.groupColumns.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private RibbonStyleButton btnCancel;
        private RibbonStyleButton btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutputFileName;
        private System.Windows.Forms.CheckBox chkbExportDocumentsWithCategory;
		private System.Windows.Forms.GroupBox groupBox1;
		private RibbonStyleButton btnBrowse;
		private System.Windows.Forms.CheckedListBox lbCategories;
		private RibbonStyleButton btnSelectAll;
		private RibbonStyleButton btnDeselectAll;
		private System.Windows.Forms.TextBox txtOutputFolder;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton rdbXmlExport;
		private System.Windows.Forms.RadioButton rdbExcelExport;
		private System.Windows.Forms.GroupBox groupCriteria;
		private System.Windows.Forms.GroupBox groupCategories;
		private System.Windows.Forms.GroupBox groupOutput;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown txtNumberOfLines;
		private System.Windows.Forms.RadioButton rdbExportPositiveOrNegative;
		private System.Windows.Forms.RadioButton rdbExportPositive;
		private System.Windows.Forms.RadioButton rdbExportAll;
		private System.Windows.Forms.CheckBox chkbHighlightMatches;
		private System.Windows.Forms.CheckBox chkbIncludePrefixSuffix;
		private System.Windows.Forms.GroupBox groupNotes;
        private System.Windows.Forms.RadioButton rdbJsonExport;
        private System.Windows.Forms.ListBox listSelectedColumns;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.GroupBox groupColumns;
        private System.Windows.Forms.ListBox listColumns;
    }
}