namespace RegScoreCalc
{
	partial class FormColumns
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnOK = new RegScoreCalc.RibbonStyleButton();
			this.gridColumns = new System.Windows.Forms.DataGridView();
			this.colCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnMoveDown = new RegScoreCalc.RibbonStyleButton();
			this.btnMoveUp = new RegScoreCalc.RibbonStyleButton();
			this.btnSelectAll = new RegScoreCalc.RibbonStyleButton();
			this.btnDeselectAll = new RegScoreCalc.RibbonStyleButton();
			this.btnInvert = new RegScoreCalc.RibbonStyleButton();
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-16, 335);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(413, 2);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(294, 348);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.HoverImage = null;
			this.btnOK.Location = new System.Drawing.Point(213, 348);
			this.btnOK.Name = "btnOK";
			this.btnOK.NormalImage = null;
			this.btnOK.PressedImage = null;
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// gridColumns
			// 
			this.gridColumns.AllowUserToAddRows = false;
			this.gridColumns.AllowUserToDeleteRows = false;
			this.gridColumns.AllowUserToOrderColumns = true;
			this.gridColumns.AllowUserToResizeRows = false;
			this.gridColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridColumns.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridColumns.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCheckbox,
            this.colName});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridColumns.DefaultCellStyle = dataGridViewCellStyle2;
			this.gridColumns.Location = new System.Drawing.Point(12, 41);
			this.gridColumns.MultiSelect = false;
			this.gridColumns.Name = "gridColumns";
			this.gridColumns.RowHeadersVisible = false;
			this.gridColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridColumns.Size = new System.Drawing.Size(305, 282);
			this.gridColumns.TabIndex = 3;
			// 
			// colCheckbox
			// 
			this.colCheckbox.HeaderText = "";
			this.colCheckbox.Name = "colCheckbox";
			this.colCheckbox.Width = 40;
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colName.DataPropertyName = "Category";
			this.colName.HeaderText = "Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn1.DataPropertyName = "ID";
			this.dataGridViewTextBoxColumn1.HeaderText = "ID";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Category";
			this.dataGridViewTextBoxColumn2.HeaderText = "Name";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// btnMoveDown
			// 
			this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveDown.HoverImage = null;
			this.btnMoveDown.Image = global::RegScoreCalc.Properties.Resources.down_icon;
			this.btnMoveDown.Location = new System.Drawing.Point(323, 280);
			this.btnMoveDown.Name = "btnMoveDown";
			this.btnMoveDown.NormalImage = null;
			this.btnMoveDown.PressedImage = null;
			this.btnMoveDown.Size = new System.Drawing.Size(46, 43);
			this.btnMoveDown.TabIndex = 5;
			this.btnMoveDown.UseVisualStyleBackColor = true;
			this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
			// 
			// btnMoveUp
			// 
			this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveUp.HoverImage = null;
			this.btnMoveUp.Image = global::RegScoreCalc.Properties.Resources.up_icon;
			this.btnMoveUp.Location = new System.Drawing.Point(323, 41);
			this.btnMoveUp.Name = "btnMoveUp";
			this.btnMoveUp.NormalImage = null;
			this.btnMoveUp.PressedImage = null;
			this.btnMoveUp.Size = new System.Drawing.Size(46, 43);
			this.btnMoveUp.TabIndex = 4;
			this.btnMoveUp.UseVisualStyleBackColor = true;
			this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.HoverImage = null;
			this.btnSelectAll.Location = new System.Drawing.Point(12, 12);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.NormalImage = null;
			this.btnSelectAll.PressedImage = null;
			this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
			this.btnSelectAll.TabIndex = 0;
			this.btnSelectAll.Text = "Select All";
			this.btnSelectAll.UseVisualStyleBackColor = true;
			this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// btnDeselectAll
			// 
			this.btnDeselectAll.HoverImage = null;
			this.btnDeselectAll.Location = new System.Drawing.Point(93, 12);
			this.btnDeselectAll.Name = "btnDeselectAll";
			this.btnDeselectAll.NormalImage = null;
			this.btnDeselectAll.PressedImage = null;
			this.btnDeselectAll.Size = new System.Drawing.Size(75, 23);
			this.btnDeselectAll.TabIndex = 1;
			this.btnDeselectAll.Text = "Deselect All";
			this.btnDeselectAll.UseVisualStyleBackColor = true;
			this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
			// 
			// btnInvert
			// 
			this.btnInvert.HoverImage = null;
			this.btnInvert.Location = new System.Drawing.Point(174, 12);
			this.btnInvert.Name = "btnInvert";
			this.btnInvert.NormalImage = null;
			this.btnInvert.PressedImage = null;
			this.btnInvert.Size = new System.Drawing.Size(75, 23);
			this.btnInvert.TabIndex = 2;
			this.btnInvert.Text = "Invert";
			this.btnInvert.UseVisualStyleBackColor = true;
			this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
			// 
			// FormColumns
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(381, 383);
			this.Controls.Add(this.btnInvert);
			this.Controls.Add(this.btnDeselectAll);
			this.Controls.Add(this.btnSelectAll);
			this.Controls.Add(this.btnMoveUp);
			this.Controls.Add(this.btnMoveDown);
			this.Controls.Add(this.gridColumns);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormColumns";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Columns";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormColumns_FormClosing);
			this.Load += new System.EventHandler(this.FormColumns_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private System.Windows.Forms.DataGridView gridColumns;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private RibbonStyleButton btnMoveDown;
		private RibbonStyleButton btnMoveUp;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCheckbox;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private RibbonStyleButton btnSelectAll;
		private RibbonStyleButton btnDeselectAll;
		private RibbonStyleButton btnInvert;
	}
}