namespace RegScoreCalc
{
	partial class FormRecentDatasets
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRecentDatasets));
			this.lvFiles = new System.Windows.Forms.ListView();
			this.colFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colLastModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCreated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.images = new System.Windows.Forms.ImageList(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new RibbonStyleButton();
			this.btnOpen = new RibbonStyleButton();
			this.btnOpenFolder = new RibbonStyleButton();
			this.btnClearList = new RibbonStyleButton();
			this.btnOpenFromDisk = new RibbonStyleButton();
			this.SuspendLayout();
			// 
			// lvFiles
			// 
			this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileName,
            this.colLastModified,
            this.colCreated,
            this.colFolder});
			this.lvFiles.FullRowSelect = true;
			this.lvFiles.GridLines = true;
			this.lvFiles.HideSelection = false;
			this.lvFiles.Location = new System.Drawing.Point(12, 12);
			this.lvFiles.MultiSelect = false;
			this.lvFiles.Name = "lvFiles";
			this.lvFiles.Size = new System.Drawing.Size(580, 243);
			this.lvFiles.SmallImageList = this.images;
			this.lvFiles.TabIndex = 0;
			this.lvFiles.UseCompatibleStateImageBehavior = false;
			this.lvFiles.View = System.Windows.Forms.View.Details;
			this.lvFiles.ItemActivate += new System.EventHandler(this.lvFiles_ItemActivate);
			// 
			// colFileName
			// 
			this.colFileName.Text = "File Name";
			this.colFileName.Width = 200;
			// 
			// colLastModified
			// 
			this.colLastModified.Text = "Last Modified";
			this.colLastModified.Width = 130;
			// 
			// colCreated
			// 
			this.colCreated.Text = "Created";
			this.colCreated.Width = 130;
			// 
			// colFolder
			// 
			this.colFolder.Text = "Containing Folder";
			this.colFolder.Width = 500;
			// 
			// images
			// 
			this.images.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("images.ImageStream")));
			this.images.TransparentColor = System.Drawing.Color.Transparent;
			this.images.Images.SetKeyName(0, "Database.ico");
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-31, 267);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(666, 2);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(517, 277);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOpen
			// 
			this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOpen.Location = new System.Drawing.Point(436, 277);
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(75, 23);
			this.btnOpen.TabIndex = 3;
			this.btnOpen.Text = "Open";
			this.btnOpen.UseVisualStyleBackColor = true;
			// 
			// btnOpenFolder
			// 
			this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOpenFolder.Location = new System.Drawing.Point(93, 277);
			this.btnOpenFolder.Name = "btnOpenFolder";
			this.btnOpenFolder.Size = new System.Drawing.Size(145, 23);
			this.btnOpenFolder.TabIndex = 2;
			this.btnOpenFolder.Text = "Open Containing Folder";
			this.btnOpenFolder.UseVisualStyleBackColor = true;
			this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
			// 
			// btnClearList
			// 
			this.btnClearList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClearList.Location = new System.Drawing.Point(12, 277);
			this.btnClearList.Name = "btnClearList";
			this.btnClearList.Size = new System.Drawing.Size(75, 23);
			this.btnClearList.TabIndex = 1;
			this.btnClearList.Text = "Clear List";
			this.btnClearList.UseVisualStyleBackColor = true;
			this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
			// 
			// btnOpenFromDisk
			// 
			this.btnOpenFromDisk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnOpenFromDisk.Location = new System.Drawing.Point(244, 277);
			this.btnOpenFromDisk.Name = "btnOpenFromDisk";
			this.btnOpenFromDisk.Size = new System.Drawing.Size(145, 23);
			this.btnOpenFromDisk.TabIndex = 5;
			this.btnOpenFromDisk.Text = "Open Another Database";
			this.btnOpenFromDisk.UseVisualStyleBackColor = true;
			this.btnOpenFromDisk.Click += new System.EventHandler(this.btnOpenFromDisk_Click);
			// 
			// FormRecentDatasets
			// 
			this.AcceptButton = this.btnOpen;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(604, 312);
			this.Controls.Add(this.btnOpenFromDisk);
			this.Controls.Add(this.btnClearList);
			this.Controls.Add(this.btnOpenFolder);
			this.Controls.Add(this.btnOpen);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lvFiles);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormRecentDatasets";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Recent Datasets";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormRecentDatasets_FormClosing);
			this.Load += new System.EventHandler(this.FormRecentDatasets_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvFiles;
		private System.Windows.Forms.GroupBox groupBox1;
		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOpen;
		private System.Windows.Forms.ImageList images;
		private System.Windows.Forms.ColumnHeader colFileName;
		private System.Windows.Forms.ColumnHeader colLastModified;
		private System.Windows.Forms.ColumnHeader colCreated;
		private System.Windows.Forms.ColumnHeader colFolder;
		private RibbonStyleButton btnOpenFolder;
		private RibbonStyleButton btnClearList;
		private RibbonStyleButton btnOpenFromDisk;

	}
}