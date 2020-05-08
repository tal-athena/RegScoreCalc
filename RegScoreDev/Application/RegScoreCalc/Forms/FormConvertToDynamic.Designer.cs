namespace RegScoreCalc.Forms
{
	partial class FormConvertToDynamic
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormConvertToDynamic));
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnConvert = new RegScoreCalc.RibbonStyleButton();
			this.gridColumns = new System.Windows.Forms.DataGridView();
			this.colColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDbType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDynamicType = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.gridCategories = new System.Windows.Forms.DataGridView();
			this.colEmpty = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colConvertTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.btnSelectAll = new System.Windows.Forms.ToolStripButton();
			this.btnSelectNone = new System.Windows.Forms.ToolStripButton();
			this.btnInvert = new System.Windows.Forms.ToolStripButton();
			this.loadingImage = new System.Windows.Forms.PictureBox();
			this.worker = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridCategories)).BeginInit();
			this.toolStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(556, 83);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "Close";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnConvert
			// 
			this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConvert.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnConvert.HoverImage = null;
			this.btnConvert.Location = new System.Drawing.Point(556, 11);
			this.btnConvert.Name = "btnConvert";
			this.btnConvert.NormalImage = null;
			this.btnConvert.PressedImage = null;
			this.btnConvert.Size = new System.Drawing.Size(75, 66);
			this.btnConvert.TabIndex = 0;
			this.btnConvert.Text = "Convert Selected Column";
			this.btnConvert.UseVisualStyleBackColor = true;
			this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
			// 
			// gridColumns
			// 
			this.gridColumns.AllowUserToAddRows = false;
			this.gridColumns.AllowUserToDeleteRows = false;
			this.gridColumns.AllowUserToOrderColumns = true;
			this.gridColumns.AllowUserToResizeRows = false;
			this.gridColumns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.gridColumns.BackgroundColor = System.Drawing.Color.White;
			this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colColumnName,
            this.colDbType,
            this.colDynamicType});
			this.gridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridColumns.Location = new System.Drawing.Point(0, 0);
			this.gridColumns.MultiSelect = false;
			this.gridColumns.Name = "gridColumns";
			this.gridColumns.RowHeadersVisible = false;
			this.gridColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridColumns.Size = new System.Drawing.Size(538, 132);
			this.gridColumns.TabIndex = 2;
			this.gridColumns.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridColumns_CellClick);
			this.gridColumns.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridColumns_CellFormatting);
			this.gridColumns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridColumns_CellValueChanged);
			this.gridColumns.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridColumns_CurrentCellDirtyStateChanged);
			this.gridColumns.SelectionChanged += new System.EventHandler(this.gridColumns_SelectionChanged);
			// 
			// colColumnName
			// 
			this.colColumnName.DataPropertyName = "Name";
			this.colColumnName.HeaderText = "Column Name";
			this.colColumnName.Name = "colColumnName";
			this.colColumnName.ReadOnly = true;
			// 
			// colDbType
			// 
			this.colDbType.DataPropertyName = "Type";
			this.colDbType.HeaderText = "DB Data Type";
			this.colDbType.Name = "colDbType";
			this.colDbType.ReadOnly = true;
			// 
			// colDynamicType
			// 
			this.colDynamicType.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
			this.colDynamicType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.colDynamicType.HeaderText = "Convert To";
			this.colDynamicType.Name = "colDynamicType";
			this.colDynamicType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.colDynamicType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.Location = new System.Drawing.Point(12, 12);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.gridColumns);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.gridCategories);
			this.splitContainer.Panel2.Controls.Add(this.toolStrip);
			this.splitContainer.Panel2.Controls.Add(this.loadingImage);
			this.splitContainer.Size = new System.Drawing.Size(538, 267);
			this.splitContainer.SplitterDistance = 132;
			this.splitContainer.TabIndex = 3;
			// 
			// gridCategories
			// 
			this.gridCategories.AllowUserToAddRows = false;
			this.gridCategories.AllowUserToDeleteRows = false;
			this.gridCategories.AllowUserToOrderColumns = true;
			this.gridCategories.AllowUserToResizeRows = false;
			this.gridCategories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.gridCategories.BackgroundColor = System.Drawing.Color.White;
			this.gridCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEmpty,
            this.colPercentage,
            this.colValue,
            this.colConvertTo,
            this.colNumber});
			this.gridCategories.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridCategories.Location = new System.Drawing.Point(0, 25);
			this.gridCategories.MultiSelect = false;
			this.gridCategories.Name = "gridCategories";
			this.gridCategories.RowHeadersVisible = false;
			this.gridCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridCategories.Size = new System.Drawing.Size(538, 106);
			this.gridCategories.TabIndex = 3;
			this.gridCategories.Visible = false;
			this.gridCategories.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCategories_CellClick);
			this.gridCategories.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCategories_CellContentClick);
			this.gridCategories.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCategories_CellFormatting);
			this.gridCategories.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCategories_CellValueChanged);
			this.gridCategories.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridCategories_CurrentCellDirtyStateChanged);
			this.gridCategories.SelectionChanged += new System.EventHandler(this.gridCategories_SelectionChanged);
			// 
			// colEmpty
			// 
			this.colEmpty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colEmpty.DataPropertyName = "Empty";
			this.colEmpty.HeaderText = "Empty";
			this.colEmpty.Name = "colEmpty";
			this.colEmpty.ReadOnly = true;
			this.colEmpty.Width = 42;
			// 
			// colPercentage
			// 
			this.colPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colPercentage.DataPropertyName = "Percentage";
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle1.Format = "F2";
			this.colPercentage.DefaultCellStyle = dataGridViewCellStyle1;
			this.colPercentage.HeaderText = "%";
			this.colPercentage.Name = "colPercentage";
			this.colPercentage.ReadOnly = true;
			this.colPercentage.Width = 40;
			// 
			// colValue
			// 
			this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colValue.DataPropertyName = "Value";
			this.colValue.HeaderText = "Value";
			this.colValue.Name = "colValue";
			this.colValue.ReadOnly = true;
			// 
			// colConvertTo
			// 
			this.colConvertTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colConvertTo.DataPropertyName = "ConvertTo";
			this.colConvertTo.FillWeight = 104.3147F;
			this.colConvertTo.HeaderText = "Convert To";
			this.colConvertTo.Name = "colConvertTo";
			// 
			// colNumber
			// 
			this.colNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colNumber.DataPropertyName = "Number";
			this.colNumber.FillWeight = 104.3147F;
			this.colNumber.HeaderText = "Number";
			this.colNumber.Name = "colNumber";
			this.colNumber.Width = 69;
			// 
			// toolStrip
			// 
			this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectAll,
            this.btnSelectNone,
            this.btnInvert});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(538, 25);
			this.toolStrip.TabIndex = 5;
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectAll.Image")));
			this.btnSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(25, 22);
			this.btnSelectAll.Text = "All";
			this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// btnSelectNone
			// 
			this.btnSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnSelectNone.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectNone.Image")));
			this.btnSelectNone.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSelectNone.Name = "btnSelectNone";
			this.btnSelectNone.Size = new System.Drawing.Size(40, 22);
			this.btnSelectNone.Text = "None";
			this.btnSelectNone.Click += new System.EventHandler(this.btnSelectNone_Click);
			// 
			// btnInvert
			// 
			this.btnInvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
			this.btnInvert.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnInvert.Image = ((System.Drawing.Image)(resources.GetObject("btnInvert.Image")));
			this.btnInvert.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnInvert.Name = "btnInvert";
			this.btnInvert.Size = new System.Drawing.Size(41, 22);
			this.btnInvert.Text = "Invert";
			this.btnInvert.Click += new System.EventHandler(this.btnInvert_Click);
			// 
			// loadingImage
			// 
			this.loadingImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
			this.loadingImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.loadingImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.loadingImage.Image = global::RegScoreCalc.Properties.Resources.Loading_small;
			this.loadingImage.Location = new System.Drawing.Point(0, 0);
			this.loadingImage.Name = "loadingImage";
			this.loadingImage.Size = new System.Drawing.Size(538, 131);
			this.loadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.loadingImage.TabIndex = 4;
			this.loadingImage.TabStop = false;
			// 
			// worker
			// 
			this.worker.WorkerReportsProgress = true;
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
			this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
			// 
			// FormConvertToDynamic
			// 
			this.AcceptButton = this.btnConvert;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(643, 291);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.btnConvert);
			this.Controls.Add(this.btnCancel);
			this.MinimizeBox = false;
			this.Name = "FormConvertToDynamic";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Convert Column To Dynamic Column";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormConvertToDynamic_FormClosing);
			this.Load += new System.EventHandler(this.FormConvertToDynamic_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridCategories)).EndInit();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnConvert;
		private System.Windows.Forms.DataGridView gridColumns;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.DataGridView gridCategories;
		private System.Windows.Forms.DataGridViewTextBoxColumn colColumnName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDbType;
		private System.Windows.Forms.DataGridViewComboBoxColumn colDynamicType;
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.PictureBox loadingImage;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton btnSelectAll;
		private System.Windows.Forms.ToolStripButton btnSelectNone;
		private System.Windows.Forms.ToolStripButton btnInvert;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colEmpty;
		private System.Windows.Forms.DataGridViewTextBoxColumn colPercentage;
		private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
		private System.Windows.Forms.DataGridViewTextBoxColumn colConvertTo;
		private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
	}
}