namespace RegScoreCalc
{
	partial class FormDynamicColumns
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
            this.btnCancel = new RegScoreCalc.RibbonStyleButton();
            this.btnAddClass = new RegScoreCalc.RibbonStyleButton();
            this.btnRenameClass = new RegScoreCalc.RibbonStyleButton();
            this.btnDeleteClass = new RegScoreCalc.RibbonStyleButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnChangeType = new RegScoreCalc.RibbonStyleButton();
            this.btnCloneColumn = new RegScoreCalc.RibbonStyleButton();
            this.btnColumnDown = new System.Windows.Forms.Button();
            this.btnColumnUp = new System.Windows.Forms.Button();
            this.lvDynamicColumns = new System.Windows.Forms.ListView();
            this.headerColumnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnOK = new RegScoreCalc.RibbonStyleButton();
            this.groupProperties = new System.Windows.Forms.GroupBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.DrawNormalBorder = false;
            this.btnCancel.HoverImage = null;
            this.btnCancel.IsHighlighted = false;
            this.btnCancel.Location = new System.Drawing.Point(465, 346);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormalImage = null;
            this.btnCancel.PressedImage = null;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnAddClass
            // 
            this.btnAddClass.DrawNormalBorder = false;
            this.btnAddClass.HoverImage = null;
            this.btnAddClass.IsHighlighted = false;
            this.btnAddClass.Location = new System.Drawing.Point(11, 24);
            this.btnAddClass.Name = "btnAddClass";
            this.btnAddClass.NormalImage = null;
            this.btnAddClass.PressedImage = null;
            this.btnAddClass.Size = new System.Drawing.Size(75, 23);
            this.btnAddClass.TabIndex = 0;
            this.btnAddClass.Text = "Add";
            this.btnAddClass.UseVisualStyleBackColor = true;
            this.btnAddClass.Click += new System.EventHandler(this.btnAddColumn_Click);
            // 
            // btnRenameClass
            // 
            this.btnRenameClass.DrawNormalBorder = false;
            this.btnRenameClass.HoverImage = null;
            this.btnRenameClass.IsHighlighted = false;
            this.btnRenameClass.Location = new System.Drawing.Point(92, 24);
            this.btnRenameClass.Name = "btnRenameClass";
            this.btnRenameClass.NormalImage = null;
            this.btnRenameClass.PressedImage = null;
            this.btnRenameClass.Size = new System.Drawing.Size(75, 23);
            this.btnRenameClass.TabIndex = 1;
            this.btnRenameClass.Text = "Rename";
            this.btnRenameClass.UseVisualStyleBackColor = true;
            this.btnRenameClass.Click += new System.EventHandler(this.btnRenameColumn_Click);
            // 
            // btnDeleteClass
            // 
            this.btnDeleteClass.DrawNormalBorder = false;
            this.btnDeleteClass.HoverImage = null;
            this.btnDeleteClass.IsHighlighted = false;
            this.btnDeleteClass.Location = new System.Drawing.Point(173, 24);
            this.btnDeleteClass.Name = "btnDeleteClass";
            this.btnDeleteClass.NormalImage = null;
            this.btnDeleteClass.PressedImage = null;
            this.btnDeleteClass.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteClass.TabIndex = 2;
            this.btnDeleteClass.Text = "Delete";
            this.btnDeleteClass.UseVisualStyleBackColor = true;
            this.btnDeleteClass.Click += new System.EventHandler(this.btnDeleteColumn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.btnChangeType);
            this.groupBox2.Controls.Add(this.btnCloneColumn);
            this.groupBox2.Controls.Add(this.btnColumnDown);
            this.groupBox2.Controls.Add(this.btnColumnUp);
            this.groupBox2.Controls.Add(this.lvDynamicColumns);
            this.groupBox2.Controls.Add(this.btnAddClass);
            this.groupBox2.Controls.Add(this.btnRenameClass);
            this.groupBox2.Controls.Add(this.btnDeleteClass);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 324);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "List";
            // 
            // btnChangeType
            // 
            this.btnChangeType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChangeType.DrawNormalBorder = false;
            this.btnChangeType.HoverImage = null;
            this.btnChangeType.IsHighlighted = false;
            this.btnChangeType.Location = new System.Drawing.Point(92, 291);
            this.btnChangeType.Name = "btnChangeType";
            this.btnChangeType.NormalImage = null;
            this.btnChangeType.PressedImage = null;
            this.btnChangeType.Size = new System.Drawing.Size(75, 23);
            this.btnChangeType.TabIndex = 5;
            this.btnChangeType.Text = "Change type";
            this.btnChangeType.UseVisualStyleBackColor = true;
            this.btnChangeType.Click += new System.EventHandler(this.btnChangeType_Click);
            // 
            // btnCloneColumn
            // 
            this.btnCloneColumn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloneColumn.DrawNormalBorder = false;
            this.btnCloneColumn.HoverImage = null;
            this.btnCloneColumn.IsHighlighted = false;
            this.btnCloneColumn.Location = new System.Drawing.Point(11, 291);
            this.btnCloneColumn.Name = "btnCloneColumn";
            this.btnCloneColumn.NormalImage = null;
            this.btnCloneColumn.PressedImage = null;
            this.btnCloneColumn.Size = new System.Drawing.Size(75, 23);
            this.btnCloneColumn.TabIndex = 4;
            this.btnCloneColumn.Text = "Clone";
            this.btnCloneColumn.UseVisualStyleBackColor = true;
            this.btnCloneColumn.Click += new System.EventHandler(this.btnCloneColumn_Click);
            // 
            // btnColumnDown
            // 
            this.btnColumnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColumnDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.btnColumnDown.Image = global::RegScoreCalc.Properties.Resources.down_icon;
            this.btnColumnDown.Location = new System.Drawing.Point(215, 254);
            this.btnColumnDown.Name = "btnColumnDown";
            this.btnColumnDown.Size = new System.Drawing.Size(33, 31);
            this.btnColumnDown.TabIndex = 4;
            this.btnColumnDown.UseVisualStyleBackColor = true;
            this.btnColumnDown.Visible = false;
            this.btnColumnDown.Click += new System.EventHandler(this.btnColumnDown_Click);
            // 
            // btnColumnUp
            // 
            this.btnColumnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColumnUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.btnColumnUp.Image = global::RegScoreCalc.Properties.Resources.up_icon;
            this.btnColumnUp.Location = new System.Drawing.Point(215, 55);
            this.btnColumnUp.Name = "btnColumnUp";
            this.btnColumnUp.Size = new System.Drawing.Size(33, 31);
            this.btnColumnUp.TabIndex = 4;
            this.btnColumnUp.UseVisualStyleBackColor = true;
            this.btnColumnUp.Visible = false;
            this.btnColumnUp.Click += new System.EventHandler(this.btnColumnUp_Click);
            // 
            // lvDynamicColumns
            // 
            this.lvDynamicColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvDynamicColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.headerColumnTitle});
            this.lvDynamicColumns.FullRowSelect = true;
            this.lvDynamicColumns.GridLines = true;
            this.lvDynamicColumns.HideSelection = false;
            this.lvDynamicColumns.Location = new System.Drawing.Point(11, 55);
            this.lvDynamicColumns.MultiSelect = false;
            this.lvDynamicColumns.Name = "lvDynamicColumns";
            this.lvDynamicColumns.Size = new System.Drawing.Size(237, 230);
            this.lvDynamicColumns.TabIndex = 3;
            this.lvDynamicColumns.UseCompatibleStateImageBehavior = false;
            this.lvDynamicColumns.View = System.Windows.Forms.View.Details;
            this.lvDynamicColumns.ItemActivate += new System.EventHandler(this.lvColumns_ItemActivate);
            this.lvDynamicColumns.SelectedIndexChanged += new System.EventHandler(this.lvColumns_SelectedIndexChanged);
            // 
            // headerColumnTitle
            // 
            this.headerColumnTitle.Text = "Name";
            this.headerColumnTitle.Width = 190;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.DrawNormalBorder = false;
            this.btnOK.HoverImage = null;
            this.btnOK.IsHighlighted = false;
            this.btnOK.Location = new System.Drawing.Point(384, 346);
            this.btnOK.Name = "btnOK";
            this.btnOK.NormalImage = null;
            this.btnOK.PressedImage = null;
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // groupProperties
            // 
            this.groupProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupProperties.Controls.Add(this.lblHint);
            this.groupProperties.Location = new System.Drawing.Point(281, 12);
            this.groupProperties.Name = "groupProperties";
            this.groupProperties.Size = new System.Drawing.Size(259, 324);
            this.groupProperties.TabIndex = 0;
            this.groupProperties.TabStop = false;
            this.groupProperties.Text = "Properties";
            // 
            // lblHint
            // 
            this.lblHint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblHint.Location = new System.Drawing.Point(52, 156);
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(155, 25);
            this.lblHint.TabIndex = 1;
            this.lblHint.Text = "Select column to edit properties";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormDynamicColumns
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(552, 381);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupProperties);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(568, 419);
            this.Name = "FormDynamicColumns";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Columns";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDynamicColumns_FormClosing);
            this.Load += new System.EventHandler(this.FormDynamicColumns_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupProperties.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnAddClass;
		private RibbonStyleButton btnRenameClass;
		private RibbonStyleButton btnDeleteClass;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListView lvDynamicColumns;
		private System.Windows.Forms.ColumnHeader headerColumnTitle;
		private RibbonStyleButton btnOK;
		private System.Windows.Forms.Button btnColumnUp;
		private System.Windows.Forms.Button btnColumnDown;
		private System.Windows.Forms.GroupBox groupProperties;
		private System.Windows.Forms.Label lblHint;
		private RibbonStyleButton btnCloneColumn;
		private RibbonStyleButton btnChangeType;
    }
}