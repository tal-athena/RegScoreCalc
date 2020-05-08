namespace RegScoreCalc.Forms
{
    partial class DialogFilter
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnCalculate = new RegScoreCalc.RibbonStyleButton();
			this.btnCalculateAll = new RegScoreCalc.RibbonStyleButton();
			this.btnDelete = new RegScoreCalc.RibbonStyleButton();
			this.btnAdd = new RegScoreCalc.RibbonStyleButton();
			this.btnEdit = new RegScoreCalc.RibbonStyleButton();
			this.btnMoveDown = new RegScoreCalc.RibbonStyleButton();
			this.btnMoveUp = new RegScoreCalc.RibbonStyleButton();
			this.groupBoxFilters = new System.Windows.Forms.GroupBox();
			this.listFilters = new System.Windows.Forms.ListView();
			this.groupBoxGroups = new System.Windows.Forms.GroupBox();
			this.listGroups = new System.Windows.Forms.ListView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnGroupRemove = new RegScoreCalc.RibbonStyleButton();
			this.btnGroupAdd = new RegScoreCalc.RibbonStyleButton();
			this.panel3 = new System.Windows.Forms.Panel();
			this.btnClose = new RegScoreCalc.RibbonStyleButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.listExcludeCategories = new System.Windows.Forms.ListView();
			this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.panel4 = new System.Windows.Forms.Panel();
			this.btnDeleteExcludeCategory = new RegScoreCalc.RibbonStyleButton();
			this.btnAddExcludeCategory = new RegScoreCalc.RibbonStyleButton();
			this.panel1.SuspendLayout();
			this.groupBoxFilters.SuspendLayout();
			this.groupBoxGroups.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnCalculate);
			this.panel1.Controls.Add(this.btnCalculateAll);
			this.panel1.Controls.Add(this.btnMoveUp);
			this.panel1.Controls.Add(this.btnDelete);
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Controls.Add(this.btnEdit);
			this.panel1.Controls.Add(this.btnMoveDown);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(534, 20);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(139, 282);
			this.panel1.TabIndex = 1;
			// 
			// btnCalculate
			// 
			this.btnCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCalculate.HoverImage = null;
			this.btnCalculate.Image = global::RegScoreCalc.Properties.Resources.CalcScores;
			this.btnCalculate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnCalculate.Location = new System.Drawing.Point(8, 206);
			this.btnCalculate.Name = "btnCalculate";
			this.btnCalculate.NormalImage = null;
			this.btnCalculate.PressedImage = null;
			this.btnCalculate.Size = new System.Drawing.Size(127, 34);
			this.btnCalculate.TabIndex = 9;
			this.btnCalculate.Text = "  Calculate";
			this.btnCalculate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCalculate.UseVisualStyleBackColor = true;
			this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
			// 
			// btnCalculateAll
			// 
			this.btnCalculateAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCalculateAll.HoverImage = null;
			this.btnCalculateAll.Image = global::RegScoreCalc.Properties.Resources.CalcScores;
			this.btnCalculateAll.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnCalculateAll.Location = new System.Drawing.Point(8, 246);
			this.btnCalculateAll.Name = "btnCalculateAll";
			this.btnCalculateAll.NormalImage = null;
			this.btnCalculateAll.PressedImage = null;
			this.btnCalculateAll.Size = new System.Drawing.Size(127, 34);
			this.btnCalculateAll.TabIndex = 8;
			this.btnCalculateAll.Text = "Calculate All";
			this.btnCalculateAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCalculateAll.UseVisualStyleBackColor = true;
			this.btnCalculateAll.Click += new System.EventHandler(this.btnCalculateAll_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDelete.HoverImage = null;
			this.btnDelete.Image = global::RegScoreCalc.Properties.Resources.delete_remove_icon;
			this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDelete.Location = new System.Drawing.Point(8, 80);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.NormalImage = null;
			this.btnDelete.PressedImage = null;
			this.btnDelete.Size = new System.Drawing.Size(127, 34);
			this.btnDelete.TabIndex = 7;
			this.btnDelete.Text = "Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAdd
			// 
			this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAdd.HoverImage = null;
			this.btnAdd.Image = global::RegScoreCalc.Properties.Resources.add_icon;
			this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAdd.Location = new System.Drawing.Point(8, 0);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.NormalImage = null;
			this.btnAdd.PressedImage = null;
			this.btnAdd.Size = new System.Drawing.Size(127, 34);
			this.btnAdd.TabIndex = 6;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnEdit.HoverImage = null;
			this.btnEdit.Image = global::RegScoreCalc.Properties.Resources.update_icon;
			this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnEdit.Location = new System.Drawing.Point(8, 40);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.NormalImage = null;
			this.btnEdit.PressedImage = null;
			this.btnEdit.Size = new System.Drawing.Size(127, 34);
			this.btnEdit.TabIndex = 5;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnMoveDown
			// 
			this.btnMoveDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnMoveDown.HoverImage = null;
			this.btnMoveDown.Image = global::RegScoreCalc.Properties.Resources.down_icon;
			this.btnMoveDown.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnMoveDown.Location = new System.Drawing.Point(7, 162);
			this.btnMoveDown.Name = "btnMoveDown";
			this.btnMoveDown.NormalImage = null;
			this.btnMoveDown.PressedImage = null;
			this.btnMoveDown.Size = new System.Drawing.Size(127, 34);
			this.btnMoveDown.TabIndex = 4;
			this.btnMoveDown.Text = "Move Down";
			this.btnMoveDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnMoveDown.UseVisualStyleBackColor = true;
			this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
			// 
			// btnMoveUp
			// 
			this.btnMoveUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnMoveUp.HoverImage = null;
			this.btnMoveUp.Image = global::RegScoreCalc.Properties.Resources.up_icon;
			this.btnMoveUp.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnMoveUp.Location = new System.Drawing.Point(8, 122);
			this.btnMoveUp.Name = "btnMoveUp";
			this.btnMoveUp.NormalImage = null;
			this.btnMoveUp.PressedImage = null;
			this.btnMoveUp.Size = new System.Drawing.Size(127, 34);
			this.btnMoveUp.TabIndex = 0;
			this.btnMoveUp.Text = "   Move Up";
			this.btnMoveUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnMoveUp.UseVisualStyleBackColor = true;
			this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
			// 
			// groupBoxFilters
			// 
			this.groupBoxFilters.Controls.Add(this.listFilters);
			this.groupBoxFilters.Controls.Add(this.panel1);
			this.groupBoxFilters.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBoxFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBoxFilters.Location = new System.Drawing.Point(0, 0);
			this.groupBoxFilters.Name = "groupBoxFilters";
			this.groupBoxFilters.Size = new System.Drawing.Size(676, 305);
			this.groupBoxFilters.TabIndex = 1;
			this.groupBoxFilters.TabStop = false;
			this.groupBoxFilters.Text = "Filters";
			// 
			// listFilters
			// 
			this.listFilters.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listFilters.FullRowSelect = true;
			this.listFilters.HideSelection = false;
			this.listFilters.Location = new System.Drawing.Point(3, 20);
			this.listFilters.MultiSelect = false;
			this.listFilters.Name = "listFilters";
			this.listFilters.Size = new System.Drawing.Size(531, 282);
			this.listFilters.TabIndex = 2;
			this.listFilters.UseCompatibleStateImageBehavior = false;
			this.listFilters.View = System.Windows.Forms.View.List;
			this.listFilters.SelectedIndexChanged += new System.EventHandler(this.listFilters_SelectedIndexChanged);
			// 
			// groupBoxGroups
			// 
			this.groupBoxGroups.Controls.Add(this.listGroups);
			this.groupBoxGroups.Controls.Add(this.panel2);
			this.groupBoxGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBoxGroups.Location = new System.Drawing.Point(0, 305);
			this.groupBoxGroups.Name = "groupBoxGroups";
			this.groupBoxGroups.Size = new System.Drawing.Size(676, 168);
			this.groupBoxGroups.TabIndex = 2;
			this.groupBoxGroups.TabStop = false;
			this.groupBoxGroups.Text = "Groups";
			// 
			// listGroups
			// 
			this.listGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listGroups.FullRowSelect = true;
			this.listGroups.HideSelection = false;
			this.listGroups.Location = new System.Drawing.Point(3, 20);
			this.listGroups.MultiSelect = false;
			this.listGroups.Name = "listGroups";
			this.listGroups.Size = new System.Drawing.Size(531, 145);
			this.listGroups.TabIndex = 1;
			this.listGroups.UseCompatibleStateImageBehavior = false;
			this.listGroups.View = System.Windows.Forms.View.List;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnGroupRemove);
			this.panel2.Controls.Add(this.btnGroupAdd);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(534, 20);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(139, 145);
			this.panel2.TabIndex = 0;
			// 
			// btnGroupRemove
			// 
			this.btnGroupRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGroupRemove.HoverImage = null;
			this.btnGroupRemove.Image = global::RegScoreCalc.Properties.Resources.delete_remove_icon;
			this.btnGroupRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnGroupRemove.Location = new System.Drawing.Point(7, 82);
			this.btnGroupRemove.Name = "btnGroupRemove";
			this.btnGroupRemove.NormalImage = null;
			this.btnGroupRemove.PressedImage = null;
			this.btnGroupRemove.Size = new System.Drawing.Size(127, 34);
			this.btnGroupRemove.TabIndex = 6;
			this.btnGroupRemove.Text = "Delete";
			this.btnGroupRemove.UseVisualStyleBackColor = true;
			this.btnGroupRemove.Click += new System.EventHandler(this.btnGroupRemove_Click);
			// 
			// btnGroupAdd
			// 
			this.btnGroupAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGroupAdd.HoverImage = null;
			this.btnGroupAdd.Image = global::RegScoreCalc.Properties.Resources.add_icon;
			this.btnGroupAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnGroupAdd.Location = new System.Drawing.Point(7, 42);
			this.btnGroupAdd.Name = "btnGroupAdd";
			this.btnGroupAdd.NormalImage = null;
			this.btnGroupAdd.PressedImage = null;
			this.btnGroupAdd.Size = new System.Drawing.Size(127, 34);
			this.btnGroupAdd.TabIndex = 5;
			this.btnGroupAdd.Text = "Add";
			this.btnGroupAdd.UseVisualStyleBackColor = true;
			this.btnGroupAdd.Click += new System.EventHandler(this.btnGroupAdd_Click);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.btnClose);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 636);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(676, 48);
			this.panel3.TabIndex = 3;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.HoverImage = null;
			this.btnClose.Location = new System.Drawing.Point(305, 13);
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImage = null;
			this.btnClose.PressedImage = null;
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.listExcludeCategories);
			this.groupBox1.Controls.Add(this.panel4);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(0, 473);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(676, 163);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Exclude categories";
			// 
			// listExcludeCategories
			// 
			this.listExcludeCategories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnType});
			this.listExcludeCategories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listExcludeCategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listExcludeCategories.FullRowSelect = true;
			this.listExcludeCategories.HideSelection = false;
			this.listExcludeCategories.Location = new System.Drawing.Point(3, 20);
			this.listExcludeCategories.MultiSelect = false;
			this.listExcludeCategories.Name = "listExcludeCategories";
			this.listExcludeCategories.Size = new System.Drawing.Size(531, 140);
			this.listExcludeCategories.TabIndex = 1;
			this.listExcludeCategories.UseCompatibleStateImageBehavior = false;
			this.listExcludeCategories.View = System.Windows.Forms.View.Details;
			// 
			// columnName
			// 
			this.columnName.Text = "Name";
			this.columnName.Width = 300;
			// 
			// columnType
			// 
			this.columnType.Text = "Type";
			this.columnType.Width = 150;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.btnDeleteExcludeCategory);
			this.panel4.Controls.Add(this.btnAddExcludeCategory);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel4.Location = new System.Drawing.Point(534, 20);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(139, 140);
			this.panel4.TabIndex = 0;
			// 
			// btnDeleteExcludeCategory
			// 
			this.btnDeleteExcludeCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDeleteExcludeCategory.HoverImage = null;
			this.btnDeleteExcludeCategory.Image = global::RegScoreCalc.Properties.Resources.delete_remove_icon;
			this.btnDeleteExcludeCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDeleteExcludeCategory.Location = new System.Drawing.Point(7, 80);
			this.btnDeleteExcludeCategory.Name = "btnDeleteExcludeCategory";
			this.btnDeleteExcludeCategory.NormalImage = null;
			this.btnDeleteExcludeCategory.PressedImage = null;
			this.btnDeleteExcludeCategory.Size = new System.Drawing.Size(127, 34);
			this.btnDeleteExcludeCategory.TabIndex = 6;
			this.btnDeleteExcludeCategory.Text = "Delete";
			this.btnDeleteExcludeCategory.UseVisualStyleBackColor = true;
			this.btnDeleteExcludeCategory.Click += new System.EventHandler(this.btnDeleteExcludeCategory_Click);
			// 
			// btnAddExcludeCategory
			// 
			this.btnAddExcludeCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAddExcludeCategory.HoverImage = null;
			this.btnAddExcludeCategory.Image = global::RegScoreCalc.Properties.Resources.add_icon;
			this.btnAddExcludeCategory.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnAddExcludeCategory.Location = new System.Drawing.Point(7, 40);
			this.btnAddExcludeCategory.Name = "btnAddExcludeCategory";
			this.btnAddExcludeCategory.NormalImage = null;
			this.btnAddExcludeCategory.PressedImage = null;
			this.btnAddExcludeCategory.Size = new System.Drawing.Size(127, 34);
			this.btnAddExcludeCategory.TabIndex = 5;
			this.btnAddExcludeCategory.Text = "Add";
			this.btnAddExcludeCategory.UseVisualStyleBackColor = true;
			this.btnAddExcludeCategory.Click += new System.EventHandler(this.btnAddExcludeCategory_Click);
			// 
			// DialogFilter
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(676, 684);
			this.Controls.Add(this.groupBoxGroups);
			this.Controls.Add(this.groupBoxFilters);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.panel3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogFilter";
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Filters";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogFilter_FormClosing);
			this.Load += new System.EventHandler(this.DialogFilter_Load);
			this.panel1.ResumeLayout(false);
			this.groupBoxFilters.ResumeLayout(false);
			this.groupBoxGroups.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBoxFilters;
		private RibbonStyleButton btnMoveUp;
        private System.Windows.Forms.GroupBox groupBoxGroups;
        private System.Windows.Forms.ListView listFilters;

        private System.Windows.Forms.Panel panel3;

        private System.Windows.Forms.ListView listGroups;
        private System.Windows.Forms.Panel panel2;
		private RibbonStyleButton btnMoveDown;
		private RibbonStyleButton btnGroupAdd;
		private RibbonStyleButton btnGroupRemove;
		private RibbonStyleButton btnDelete;
		private RibbonStyleButton btnAdd;
		private RibbonStyleButton btnEdit;
		private RibbonStyleButton btnCalculate;
		private RibbonStyleButton btnCalculateAll;
		private RibbonStyleButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listExcludeCategories;
        private System.Windows.Forms.Panel panel4;
		private RibbonStyleButton btnDeleteExcludeCategory;
		private RibbonStyleButton btnAddExcludeCategory;
        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnType;
    }
}