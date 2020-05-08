namespace RegScoreCalc.Forms
{
    partial class DialogGroup
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
			this.btnGroupDelete = new RegScoreCalc.RibbonStyleButton();
			this.btnGroupAdd = new RegScoreCalc.RibbonStyleButton();
			this.btnGroupEdit = new RegScoreCalc.RibbonStyleButton();
			this.groupBoxFilters = new System.Windows.Forms.GroupBox();
			this.listGroups = new System.Windows.Forms.ListView();
			this.groupBoxGroups = new System.Windows.Forms.GroupBox();
			this.listCodes = new System.Windows.Forms.ListView();
			this.code = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.regExp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.oneWay = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnCodesEdit = new RegScoreCalc.RibbonStyleButton();
			this.btnCodesDelete = new RegScoreCalc.RibbonStyleButton();
			this.btnCodesAdd = new RegScoreCalc.RibbonStyleButton();
			this.panel3 = new System.Windows.Forms.Panel();
			this.btnClose = new RegScoreCalc.RibbonStyleButton();
			this.panel1.SuspendLayout();
			this.groupBoxFilters.SuspendLayout();
			this.groupBoxGroups.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnGroupDelete);
			this.panel1.Controls.Add(this.btnGroupAdd);
			this.panel1.Controls.Add(this.btnGroupEdit);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(566, 20);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(107, 205);
			this.panel1.TabIndex = 1;
			// 
			// btnGroupDelete
			// 
			this.btnGroupDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGroupDelete.HoverImage = null;
			this.btnGroupDelete.Image = global::RegScoreCalc.Properties.Resources.delete_remove_icon;
			this.btnGroupDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnGroupDelete.Location = new System.Drawing.Point(6, 124);
			this.btnGroupDelete.Name = "btnGroupDelete";
			this.btnGroupDelete.NormalImage = null;
			this.btnGroupDelete.PressedImage = null;
			this.btnGroupDelete.Size = new System.Drawing.Size(92, 34);
			this.btnGroupDelete.TabIndex = 2;
			this.btnGroupDelete.Text = "Delete";
			this.btnGroupDelete.UseVisualStyleBackColor = true;
			this.btnGroupDelete.Click += new System.EventHandler(this.btnGroupDelete_Click);
			// 
			// btnGroupAdd
			// 
			this.btnGroupAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGroupAdd.HoverImage = null;
			this.btnGroupAdd.Image = global::RegScoreCalc.Properties.Resources.add_icon;
			this.btnGroupAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnGroupAdd.Location = new System.Drawing.Point(6, 44);
			this.btnGroupAdd.Name = "btnGroupAdd";
			this.btnGroupAdd.NormalImage = null;
			this.btnGroupAdd.PressedImage = null;
			this.btnGroupAdd.Size = new System.Drawing.Size(92, 34);
			this.btnGroupAdd.TabIndex = 0;
			this.btnGroupAdd.Text = "Add";
			this.btnGroupAdd.UseVisualStyleBackColor = true;
			this.btnGroupAdd.Click += new System.EventHandler(this.btnGroupAdd_Click);
			// 
			// btnGroupEdit
			// 
			this.btnGroupEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGroupEdit.HoverImage = null;
			this.btnGroupEdit.Image = global::RegScoreCalc.Properties.Resources.update_icon;
			this.btnGroupEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnGroupEdit.Location = new System.Drawing.Point(6, 84);
			this.btnGroupEdit.Name = "btnGroupEdit";
			this.btnGroupEdit.NormalImage = null;
			this.btnGroupEdit.PressedImage = null;
			this.btnGroupEdit.Size = new System.Drawing.Size(92, 34);
			this.btnGroupEdit.TabIndex = 1;
			this.btnGroupEdit.Text = "Edit";
			this.btnGroupEdit.UseVisualStyleBackColor = true;
			this.btnGroupEdit.Click += new System.EventHandler(this.btnGroupEdit_Click);
			// 
			// groupBoxFilters
			// 
			this.groupBoxFilters.Controls.Add(this.listGroups);
			this.groupBoxFilters.Controls.Add(this.panel1);
			this.groupBoxFilters.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBoxFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBoxFilters.Location = new System.Drawing.Point(0, 0);
			this.groupBoxFilters.Name = "groupBoxFilters";
			this.groupBoxFilters.Size = new System.Drawing.Size(676, 228);
			this.groupBoxFilters.TabIndex = 1;
			this.groupBoxFilters.TabStop = false;
			this.groupBoxFilters.Text = "ICD9 Concordance Groups";
			// 
			// listGroups
			// 
			this.listGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listGroups.FullRowSelect = true;
			this.listGroups.HideSelection = false;
			this.listGroups.Location = new System.Drawing.Point(3, 20);
			this.listGroups.MultiSelect = false;
			this.listGroups.Name = "listGroups";
			this.listGroups.Size = new System.Drawing.Size(563, 205);
			this.listGroups.TabIndex = 0;
			this.listGroups.UseCompatibleStateImageBehavior = false;
			this.listGroups.View = System.Windows.Forms.View.List;
			this.listGroups.ItemActivate += new System.EventHandler(this.listGroups_ItemActivate);
			this.listGroups.SelectedIndexChanged += new System.EventHandler(this.listGroups_SelectedIndexChanged);
			// 
			// groupBoxGroups
			// 
			this.groupBoxGroups.Controls.Add(this.listCodes);
			this.groupBoxGroups.Controls.Add(this.panel2);
			this.groupBoxGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxGroups.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBoxGroups.Location = new System.Drawing.Point(0, 228);
			this.groupBoxGroups.Name = "groupBoxGroups";
			this.groupBoxGroups.Size = new System.Drawing.Size(676, 225);
			this.groupBoxGroups.TabIndex = 2;
			this.groupBoxGroups.TabStop = false;
			this.groupBoxGroups.Text = "ICD9 Codes";
			// 
			// listCodes
			// 
			this.listCodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.code,
            this.description,
            this.regExp,
            this.oneWay});
			this.listCodes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listCodes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listCodes.FullRowSelect = true;
			this.listCodes.HideSelection = false;
			this.listCodes.Location = new System.Drawing.Point(3, 20);
			this.listCodes.MultiSelect = false;
			this.listCodes.Name = "listCodes";
			this.listCodes.Size = new System.Drawing.Size(563, 202);
			this.listCodes.TabIndex = 0;
			this.listCodes.UseCompatibleStateImageBehavior = false;
			this.listCodes.View = System.Windows.Forms.View.Details;
			this.listCodes.ItemActivate += new System.EventHandler(this.listCodes_ItemActivate);
			// 
			// code
			// 
			this.code.Text = "Code";
			this.code.Width = 100;
			// 
			// description
			// 
			this.description.Text = "Description";
			this.description.Width = 233;
			// 
			// regExp
			// 
			this.regExp.Text = "RegExp";
			this.regExp.Width = 70;
			// 
			// oneWay
			// 
			this.oneWay.Text = "OneWay";
			this.oneWay.Width = 106;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnCodesEdit);
			this.panel2.Controls.Add(this.btnCodesDelete);
			this.panel2.Controls.Add(this.btnCodesAdd);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(566, 20);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(107, 202);
			this.panel2.TabIndex = 0;
			// 
			// btnCodesEdit
			// 
			this.btnCodesEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCodesEdit.HoverImage = null;
			this.btnCodesEdit.Image = global::RegScoreCalc.Properties.Resources.update_icon;
			this.btnCodesEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCodesEdit.Location = new System.Drawing.Point(6, 75);
			this.btnCodesEdit.Name = "btnCodesEdit";
			this.btnCodesEdit.NormalImage = null;
			this.btnCodesEdit.PressedImage = null;
			this.btnCodesEdit.Size = new System.Drawing.Size(92, 34);
			this.btnCodesEdit.TabIndex = 1;
			this.btnCodesEdit.Text = "Edit";
			this.btnCodesEdit.UseVisualStyleBackColor = true;
			this.btnCodesEdit.Click += new System.EventHandler(this.btnCodesEdit_Click);
			// 
			// btnCodesDelete
			// 
			this.btnCodesDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCodesDelete.HoverImage = null;
			this.btnCodesDelete.Image = global::RegScoreCalc.Properties.Resources.delete_remove_icon;
			this.btnCodesDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCodesDelete.Location = new System.Drawing.Point(6, 115);
			this.btnCodesDelete.Name = "btnCodesDelete";
			this.btnCodesDelete.NormalImage = null;
			this.btnCodesDelete.PressedImage = null;
			this.btnCodesDelete.Size = new System.Drawing.Size(92, 34);
			this.btnCodesDelete.TabIndex = 2;
			this.btnCodesDelete.Text = "Delete";
			this.btnCodesDelete.UseVisualStyleBackColor = true;
			this.btnCodesDelete.Click += new System.EventHandler(this.btnCodesDelete_Click);
			// 
			// btnCodesAdd
			// 
			this.btnCodesAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCodesAdd.HoverImage = null;
			this.btnCodesAdd.Image = global::RegScoreCalc.Properties.Resources.add_icon;
			this.btnCodesAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnCodesAdd.Location = new System.Drawing.Point(6, 35);
			this.btnCodesAdd.Name = "btnCodesAdd";
			this.btnCodesAdd.NormalImage = null;
			this.btnCodesAdd.PressedImage = null;
			this.btnCodesAdd.Size = new System.Drawing.Size(92, 34);
			this.btnCodesAdd.TabIndex = 0;
			this.btnCodesAdd.Text = "Add";
			this.btnCodesAdd.UseVisualStyleBackColor = true;
			this.btnCodesAdd.Click += new System.EventHandler(this.btnCodesAdd_Click);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.btnClose);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 453);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(676, 39);
			this.panel3.TabIndex = 3;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.HoverImage = null;
			this.btnClose.Location = new System.Drawing.Point(304, 6);
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImage = null;
			this.btnClose.PressedImage = null;
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// DialogGroup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(676, 492);
			this.Controls.Add(this.groupBoxGroups);
			this.Controls.Add(this.groupBoxFilters);
			this.Controls.Add(this.panel3);
			this.Name = "DialogGroup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Groups";
			this.Load += new System.EventHandler(this.DialogGroup_Load);
			this.panel1.ResumeLayout(false);
			this.groupBoxFilters.ResumeLayout(false);
			this.groupBoxGroups.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBoxFilters;
        private System.Windows.Forms.GroupBox groupBoxGroups;
        private System.Windows.Forms.ListView listGroups;
        private System.Windows.Forms.ListView listCodes;
        private System.Windows.Forms.Panel panel2;
        private RibbonStyleButton btnGroupDelete;
		private RibbonStyleButton btnGroupAdd;
		private RibbonStyleButton btnGroupEdit;
		private RibbonStyleButton btnCodesDelete;
		private RibbonStyleButton btnCodesAdd;
		private RibbonStyleButton btnCodesEdit;
        private System.Windows.Forms.ColumnHeader code;
        private System.Windows.Forms.ColumnHeader description;
        private System.Windows.Forms.Panel panel3;
		private RibbonStyleButton btnClose;
        private System.Windows.Forms.ColumnHeader regExp;
        private System.Windows.Forms.ColumnHeader oneWay;
    }
}