using System;
using System.Linq;

namespace RegScoreCalc
{
	partial class FormColumnPropsCategories
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
        private void InitializeCheckBox()
        {
            var svmColumnName = _dynamicColumnTitle + " (SVM)";
            var svmColumn = _views.MainForm.adapterDocuments.GetExtraColumnsCollection()
                                  .FirstOrDefault(x => String.Compare(x.Name, svmColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (svmColumn != null)
            {
                chkSVMColumn.Checked = true;
            }
            else {
                chkSVMColumn.Checked = false;
            }
        }
        private void InitializeComponent()
		{
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSVMColumn = new System.Windows.Forms.CheckBox();
            this.lvCategories = new System.Windows.Forms.ListView();
            this.headerCategoryName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.headerCategoryNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddCategory = new RegScoreCalc.RibbonStyleButton();
            this.btnDeleteCategory = new RegScoreCalc.RibbonStyleButton();
            this.btnRenameCategory = new RegScoreCalc.RibbonStyleButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkSVMColumn);
            this.groupBox1.Controls.Add(this.lvCategories);
            this.groupBox1.Controls.Add(this.btnAddCategory);
            this.groupBox1.Controls.Add(this.btnDeleteCategory);
            this.groupBox1.Controls.Add(this.btnRenameCategory);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 321);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // chkSVMColumn
            // 
            this.chkSVMColumn.AutoSize = true;
            this.chkSVMColumn.Checked = true;
            this.chkSVMColumn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSVMColumn.Location = new System.Drawing.Point(12, 273);
            this.chkSVMColumn.Name = "chkSVMColumn";
            this.chkSVMColumn.Size = new System.Drawing.Size(118, 17);
            this.chkSVMColumn.TabIndex = 7;
            this.chkSVMColumn.Text = "SVM Score Column";
            this.chkSVMColumn.UseVisualStyleBackColor = true;
            this.chkSVMColumn.CheckedChanged += new System.EventHandler(this.chkSVMColumn_CheckedChanged);
            // 
            // lvCategories
            // 
            this.lvCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCategories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerCategoryName,
            this.headerCategoryNumber});
            this.lvCategories.FullRowSelect = true;
            this.lvCategories.GridLines = true;
            this.lvCategories.HideSelection = false;
            this.lvCategories.Location = new System.Drawing.Point(11, 55);
            this.lvCategories.MultiSelect = false;
            this.lvCategories.Name = "lvCategories";
            this.lvCategories.Size = new System.Drawing.Size(237, 212);
            this.lvCategories.TabIndex = 3;
            this.lvCategories.UseCompatibleStateImageBehavior = false;
            this.lvCategories.View = System.Windows.Forms.View.Details;
            this.lvCategories.ItemActivate += new System.EventHandler(this.lvCategories_ItemActivate);
            // 
            // headerCategoryName
            // 
            this.headerCategoryName.Text = "Category Name";
            this.headerCategoryName.Width = 173;
            // 
            // headerCategoryNumber
            // 
            this.headerCategoryNumber.Text = "Number";
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.DrawNormalBorder = false;
            this.btnAddCategory.HoverImage = null;
            this.btnAddCategory.IsHighlighted = false;
            this.btnAddCategory.Location = new System.Drawing.Point(11, 24);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.NormalImage = null;
            this.btnAddCategory.PressedImage = null;
            this.btnAddCategory.Size = new System.Drawing.Size(75, 23);
            this.btnAddCategory.TabIndex = 0;
            this.btnAddCategory.Text = "Add";
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // btnDeleteCategory
            // 
            this.btnDeleteCategory.DrawNormalBorder = false;
            this.btnDeleteCategory.HoverImage = null;
            this.btnDeleteCategory.IsHighlighted = false;
            this.btnDeleteCategory.Location = new System.Drawing.Point(173, 24);
            this.btnDeleteCategory.Name = "btnDeleteCategory";
            this.btnDeleteCategory.NormalImage = null;
            this.btnDeleteCategory.PressedImage = null;
            this.btnDeleteCategory.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteCategory.TabIndex = 2;
            this.btnDeleteCategory.Text = "Delete";
            this.btnDeleteCategory.UseVisualStyleBackColor = true;
            this.btnDeleteCategory.Click += new System.EventHandler(this.btnDeleteCategory_Click);
            // 
            // btnRenameCategory
            // 
            this.btnRenameCategory.DrawNormalBorder = false;
            this.btnRenameCategory.HoverImage = null;
            this.btnRenameCategory.IsHighlighted = false;
            this.btnRenameCategory.Location = new System.Drawing.Point(92, 24);
            this.btnRenameCategory.Name = "btnRenameCategory";
            this.btnRenameCategory.NormalImage = null;
            this.btnRenameCategory.PressedImage = null;
            this.btnRenameCategory.Size = new System.Drawing.Size(75, 23);
            this.btnRenameCategory.TabIndex = 1;
            this.btnRenameCategory.Text = "Rename";
            this.btnRenameCategory.UseVisualStyleBackColor = true;
            this.btnRenameCategory.Click += new System.EventHandler(this.btnRenameCategory_Click);
            // 
            // FormColumnPropsCategories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 321);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormColumnPropsCategories";
            this.Text = "FormColumnPropsCategories";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView lvCategories;
		private System.Windows.Forms.ColumnHeader headerCategoryName;
		private System.Windows.Forms.ColumnHeader headerCategoryNumber;
		private RibbonStyleButton btnAddCategory;
		private RibbonStyleButton btnDeleteCategory;
		private RibbonStyleButton btnRenameCategory;
        private System.Windows.Forms.CheckBox chkSVMColumn;
    }
}