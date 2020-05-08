using System.Windows.Forms;
using RegScoreCalc.Forms;

namespace RegScoreCalc
{
    partial class FormExternalSort
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
			this.button_OK = new RegScoreCalc.RibbonStyleButton();
			this.button_Cancel = new RegScoreCalc.RibbonStyleButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkbSortGroupsOnClick = new System.Windows.Forms.CheckBox();
			this.rdbSum = new System.Windows.Forms.RadioButton();
			this.rdbMax = new System.Windows.Forms.RadioButton();
			this.rdbMin = new System.Windows.Forms.RadioButton();
			this.sortOptions1 = new RegScoreCalc.Forms.ExternalSortOptionsCtrl();
			this.sortOptions2 = new RegScoreCalc.Forms.ExternalSortOptionsCtrl();
			this.sortOptions3 = new RegScoreCalc.Forms.ExternalSortOptionsCtrl();
			this.btnClear = new RegScoreCalc.RibbonStyleButton();
			this.chkbShowGroupMinDocuments = new System.Windows.Forms.CheckBox();
			this.txtMinDocuments = new System.Windows.Forms.NumericUpDown();
			this.chkbLeaveOneDocument = new System.Windows.Forms.CheckBox();
			this.cmbLeaveOneDocumentCriteria = new System.Windows.Forms.ComboBox();
			this.cmbLeaveOneDocumentColumn = new System.Windows.Forms.ComboBox();
			this.chkbApplyEditsToGroup = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.txtMinDocuments)).BeginInit();
			this.SuspendLayout();
			// 
			// button_OK
			// 
			this.button_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button_OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button_OK.HoverImage = null;
			this.button_OK.Location = new System.Drawing.Point(194, 457);
			this.button_OK.Name = "button_OK";
			this.button_OK.NormalImage = null;
			this.button_OK.PressedImage = null;
			this.button_OK.Size = new System.Drawing.Size(135, 23);
			this.button_OK.TabIndex = 14;
			this.button_OK.Text = "Apply Sort Groups";
			this.button_OK.UseVisualStyleBackColor = true;
			// 
			// button_Cancel
			// 
			this.button_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button_Cancel.HoverImage = null;
			this.button_Cancel.Location = new System.Drawing.Point(335, 457);
			this.button_Cancel.Name = "button_Cancel";
			this.button_Cancel.NormalImage = null;
			this.button_Cancel.PressedImage = null;
			this.button_Cancel.Size = new System.Drawing.Size(75, 23);
			this.button_Cancel.TabIndex = 15;
			this.button_Cancel.Text = "Cancel";
			this.button_Cancel.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-18, 445);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(457, 2);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			// 
			// chkbSortGroupsOnClick
			// 
			this.chkbSortGroupsOnClick.AutoSize = true;
			this.chkbSortGroupsOnClick.Location = new System.Drawing.Point(12, 278);
			this.chkbSortGroupsOnClick.Name = "chkbSortGroupsOnClick";
			this.chkbSortGroupsOnClick.Size = new System.Drawing.Size(158, 17);
			this.chkbSortGroupsOnClick.TabIndex = 6;
			this.chkbSortGroupsOnClick.Text = "Column click sort groups by:";
			this.chkbSortGroupsOnClick.UseVisualStyleBackColor = true;
			this.chkbSortGroupsOnClick.CheckedChanged += new System.EventHandler(this.chkbSortGroupsOnClick_CheckedChanged);
			// 
			// rdbSum
			// 
			this.rdbSum.AutoSize = true;
			this.rdbSum.Checked = true;
			this.rdbSum.Location = new System.Drawing.Point(31, 301);
			this.rdbSum.Name = "rdbSum";
			this.rdbSum.Size = new System.Drawing.Size(46, 17);
			this.rdbSum.TabIndex = 7;
			this.rdbSum.TabStop = true;
			this.rdbSum.Text = "Sum";
			this.rdbSum.UseVisualStyleBackColor = true;
			// 
			// rdbMax
			// 
			this.rdbMax.AutoSize = true;
			this.rdbMax.Location = new System.Drawing.Point(31, 324);
			this.rdbMax.Name = "rdbMax";
			this.rdbMax.Size = new System.Drawing.Size(45, 17);
			this.rdbMax.TabIndex = 8;
			this.rdbMax.TabStop = true;
			this.rdbMax.Text = "Max";
			this.rdbMax.UseVisualStyleBackColor = true;
			// 
			// rdbMin
			// 
			this.rdbMin.AutoSize = true;
			this.rdbMin.Location = new System.Drawing.Point(31, 347);
			this.rdbMin.Name = "rdbMin";
			this.rdbMin.Size = new System.Drawing.Size(42, 17);
			this.rdbMin.TabIndex = 9;
			this.rdbMin.TabStop = true;
			this.rdbMin.Text = "Min";
			this.rdbMin.UseVisualStyleBackColor = true;
			// 
			// sortOptions1
			// 
			this.sortOptions1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sortOptions1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.sortOptions1.Location = new System.Drawing.Point(12, 12);
			this.sortOptions1.Name = "sortOptions1";
			this.sortOptions1.Padding = new System.Windows.Forms.Padding(5);
			this.sortOptions1.Size = new System.Drawing.Size(398, 60);
			this.sortOptions1.SortByColumn = null;
			this.sortOptions1.TabIndex = 0;
			// 
			// sortOptions2
			// 
			this.sortOptions2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sortOptions2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.sortOptions2.Location = new System.Drawing.Point(12, 78);
			this.sortOptions2.Name = "sortOptions2";
			this.sortOptions2.Padding = new System.Windows.Forms.Padding(5);
			this.sortOptions2.Size = new System.Drawing.Size(398, 60);
			this.sortOptions2.SortByColumn = null;
			this.sortOptions2.TabIndex = 1;
			// 
			// sortOptions3
			// 
			this.sortOptions3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sortOptions3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.sortOptions3.Location = new System.Drawing.Point(12, 144);
			this.sortOptions3.Name = "sortOptions3";
			this.sortOptions3.Padding = new System.Windows.Forms.Padding(5);
			this.sortOptions3.Size = new System.Drawing.Size(398, 60);
			this.sortOptions3.SortByColumn = null;
			this.sortOptions3.TabIndex = 2;
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btnClear.HoverImage = null;
			this.btnClear.Location = new System.Drawing.Point(53, 457);
			this.btnClear.Name = "btnClear";
			this.btnClear.NormalImage = null;
			this.btnClear.PressedImage = null;
			this.btnClear.Size = new System.Drawing.Size(135, 23);
			this.btnClear.TabIndex = 13;
			this.btnClear.Text = "Switch to Default Sorting";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// chkbShowGroupMinDocuments
			// 
			this.chkbShowGroupMinDocuments.AutoSize = true;
			this.chkbShowGroupMinDocuments.Location = new System.Drawing.Point(12, 385);
			this.chkbShowGroupMinDocuments.Name = "chkbShowGroupMinDocuments";
			this.chkbShowGroupMinDocuments.Size = new System.Drawing.Size(282, 17);
			this.chkbShowGroupMinDocuments.TabIndex = 10;
			this.chkbShowGroupMinDocuments.Text = "Show groups with number of documents more or equal";
			this.chkbShowGroupMinDocuments.UseVisualStyleBackColor = true;
			this.chkbShowGroupMinDocuments.CheckedChanged += new System.EventHandler(this.chkbShowGroupMinDocuments_CheckedChanged);
			// 
			// txtMinDocuments
			// 
			this.txtMinDocuments.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.txtMinDocuments.Location = new System.Drawing.Point(300, 382);
			this.txtMinDocuments.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
			this.txtMinDocuments.Name = "txtMinDocuments";
			this.txtMinDocuments.Size = new System.Drawing.Size(73, 20);
			this.txtMinDocuments.TabIndex = 11;
			this.txtMinDocuments.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// chkbLeaveOneDocument
			// 
			this.chkbLeaveOneDocument.AutoSize = true;
			this.chkbLeaveOneDocument.Location = new System.Drawing.Point(12, 213);
			this.chkbLeaveOneDocument.Name = "chkbLeaveOneDocument";
			this.chkbLeaveOneDocument.Size = new System.Drawing.Size(221, 17);
			this.chkbLeaveOneDocument.TabIndex = 3;
			this.chkbLeaveOneDocument.Text = "Leave one document for each group with";
			this.chkbLeaveOneDocument.UseVisualStyleBackColor = true;
			this.chkbLeaveOneDocument.CheckedChanged += new System.EventHandler(this.chkbLeaveOneDocument_CheckedChanged);
			// 
			// cmbLeaveOneDocumentCriteria
			// 
			this.cmbLeaveOneDocumentCriteria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLeaveOneDocumentCriteria.FormattingEnabled = true;
			this.cmbLeaveOneDocumentCriteria.Items.AddRange(new object[] {
            "Minimum",
            "Maximum",
            "Absolute minimum",
            "Absolute maximum"});
			this.cmbLeaveOneDocumentCriteria.Location = new System.Drawing.Point(63, 236);
			this.cmbLeaveOneDocumentCriteria.Name = "cmbLeaveOneDocumentCriteria";
			this.cmbLeaveOneDocumentCriteria.Size = new System.Drawing.Size(138, 21);
			this.cmbLeaveOneDocumentCriteria.TabIndex = 4;
			// 
			// cmbLeaveOneDocumentColumn
			// 
			this.cmbLeaveOneDocumentColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbLeaveOneDocumentColumn.FormattingEnabled = true;
			this.cmbLeaveOneDocumentColumn.Items.AddRange(new object[] {
            "Minimum",
            "Maximum"});
			this.cmbLeaveOneDocumentColumn.Location = new System.Drawing.Point(207, 236);
			this.cmbLeaveOneDocumentColumn.Name = "cmbLeaveOneDocumentColumn";
			this.cmbLeaveOneDocumentColumn.Size = new System.Drawing.Size(138, 21);
			this.cmbLeaveOneDocumentColumn.TabIndex = 5;
			this.cmbLeaveOneDocumentColumn.SelectedIndexChanged += new System.EventHandler(this.cmbLeaveOneDocumentColumn_SelectedIndexChanged);
			// 
			// chkbApplyEditsToGroup
			// 
			this.chkbApplyEditsToGroup.AutoSize = true;
			this.chkbApplyEditsToGroup.Location = new System.Drawing.Point(12, 419);
			this.chkbApplyEditsToGroup.Name = "chkbApplyEditsToGroup";
			this.chkbApplyEditsToGroup.Size = new System.Drawing.Size(119, 17);
			this.chkbApplyEditsToGroup.TabIndex = 12;
			this.chkbApplyEditsToGroup.Text = "Apply edits to group";
			this.chkbApplyEditsToGroup.UseVisualStyleBackColor = true;
			this.chkbApplyEditsToGroup.CheckedChanged += new System.EventHandler(this.chkbApplyEditsToGroup_CheckedChanged);
			// 
			// FormExternalSort
			// 
			this.AcceptButton = this.button_OK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button_Cancel;
			this.ClientSize = new System.Drawing.Size(422, 492);
			this.Controls.Add(this.chkbApplyEditsToGroup);
			this.Controls.Add(this.cmbLeaveOneDocumentColumn);
			this.Controls.Add(this.cmbLeaveOneDocumentCriteria);
			this.Controls.Add(this.chkbLeaveOneDocument);
			this.Controls.Add(this.txtMinDocuments);
			this.Controls.Add(this.chkbShowGroupMinDocuments);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.sortOptions3);
			this.Controls.Add(this.sortOptions2);
			this.Controls.Add(this.rdbMin);
			this.Controls.Add(this.rdbMax);
			this.Controls.Add(this.rdbSum);
			this.Controls.Add(this.chkbSortGroupsOnClick);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button_Cancel);
			this.Controls.Add(this.button_OK);
			this.Controls.Add(this.sortOptions1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(438, 405);
			this.Name = "FormExternalSort";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Sort Groups";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormExternalSort_FormClosing);
			this.Load += new System.EventHandler(this.FormExternalSort_Load);
			((System.ComponentModel.ISupportInitialize)(this.txtMinDocuments)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private RibbonStyleButton button_OK;
		private RibbonStyleButton button_Cancel;
		private System.Windows.Forms.GroupBox groupBox1;
		private CheckBox chkbSortGroupsOnClick;
		private RadioButton rdbSum;
		private RadioButton rdbMax;
		private RadioButton rdbMin;
	    private ExternalSortOptionsCtrl sortOptions1;
		private ExternalSortOptionsCtrl sortOptions2;
		private ExternalSortOptionsCtrl sortOptions3;
		private RibbonStyleButton btnClear;
		private CheckBox chkbShowGroupMinDocuments;
		private NumericUpDown txtMinDocuments;
		private CheckBox chkbLeaveOneDocument;
		private ComboBox cmbLeaveOneDocumentCriteria;
		private ComboBox cmbLeaveOneDocumentColumn;
		private CheckBox chkbApplyEditsToGroup;
	}
}