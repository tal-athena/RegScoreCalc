namespace RegScoreCalc
{
	partial class FormOutputFolders
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnClose = new RegScoreCalc.RibbonStyleButton();
			this.btnCopyToClipboard = new RegScoreCalc.RibbonStyleButton();
			this.grid = new System.Windows.Forms.DataGridView();
			this.btnExplore = new RegScoreCalc.RibbonStyleButton();
			this.colCreationTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colFullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-16, 320);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(916, 2);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.HoverImage = null;
			this.btnClose.Location = new System.Drawing.Point(797, 333);
			this.btnClose.Name = "btnClose";
			this.btnClose.NormalImage = null;
			this.btnClose.PressedImage = null;
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnCopyToClipboard
			// 
			this.btnCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCopyToClipboard.HoverImage = null;
			this.btnCopyToClipboard.Location = new System.Drawing.Point(674, 333);
			this.btnCopyToClipboard.Name = "btnCopyToClipboard";
			this.btnCopyToClipboard.NormalImage = null;
			this.btnCopyToClipboard.PressedImage = null;
			this.btnCopyToClipboard.Size = new System.Drawing.Size(117, 23);
			this.btnCopyToClipboard.TabIndex = 1;
			this.btnCopyToClipboard.Text = "Copy to Clipboard";
			this.btnCopyToClipboard.UseVisualStyleBackColor = true;
			this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.AllowUserToResizeRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.BackgroundColor = System.Drawing.Color.White;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCreationTime,
            this.colName,
            this.colFullPath});
			this.grid.Location = new System.Drawing.Point(12, 12);
			this.grid.MultiSelect = false;
			this.grid.Name = "grid";
			this.grid.ReadOnly = true;
			this.grid.RowHeadersVisible = false;
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid.Size = new System.Drawing.Size(860, 297);
			this.grid.TabIndex = 0;
			this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
			// 
			// btnExplore
			// 
			this.btnExplore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnExplore.HoverImage = null;
			this.btnExplore.Location = new System.Drawing.Point(12, 333);
			this.btnExplore.Name = "btnExplore";
			this.btnExplore.NormalImage = null;
			this.btnExplore.PressedImage = null;
			this.btnExplore.Size = new System.Drawing.Size(117, 23);
			this.btnExplore.TabIndex = 3;
			this.btnExplore.Text = "Open in Explorer";
			this.btnExplore.UseVisualStyleBackColor = true;
			this.btnExplore.Click += new System.EventHandler(this.btnExplore_Click);
			// 
			// colCreationTime
			// 
			this.colCreationTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.colCreationTime.DefaultCellStyle = dataGridViewCellStyle1;
			this.colCreationTime.HeaderText = "Created";
			this.colCreationTime.Name = "colCreationTime";
			this.colCreationTime.ReadOnly = true;
			this.colCreationTime.Width = 69;
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colName.HeaderText = "Folder Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			this.colName.Width = 92;
			// 
			// colFullPath
			// 
			this.colFullPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colFullPath.HeaderText = "Full Path";
			this.colFullPath.MinimumWidth = 400;
			this.colFullPath.Name = "colFullPath";
			this.colFullPath.ReadOnly = true;
			// 
			// FormOutputFolders
			// 
			this.AcceptButton = this.btnCopyToClipboard;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(884, 368);
			this.Controls.Add(this.btnExplore);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnCopyToClipboard);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormOutputFolders";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Output Folders";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormOutputFolders_FormClosing);
			this.Load += new System.EventHandler(this.FormOutputFolders_Load);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private RibbonStyleButton btnClose;
		private RibbonStyleButton btnCopyToClipboard;
		private System.Windows.Forms.DataGridView grid;
		private RibbonStyleButton btnExplore;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCreationTime;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colFullPath;
	}
}