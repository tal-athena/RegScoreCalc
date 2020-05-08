namespace Bootstrapper.Forms
{
	partial class FormMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.worker = new System.ComponentModel.BackgroundWorker();
			this.gridRequirements = new System.Windows.Forms.DataGridView();
			this.colImage = new System.Windows.Forms.DataGridViewImageColumn();
			this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colLink = new System.Windows.Forms.DataGridViewLinkColumn();
			this.btnRetry = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.gridRequirements)).BeginInit();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(207, 266);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(100, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-22, 255);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(452, 2);
			this.groupBox1.TabIndex = 9;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "groupBox1";
			// 
			// worker
			// 
			this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
			this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
			// 
			// gridRequirements
			// 
			this.gridRequirements.AllowUserToAddRows = false;
			this.gridRequirements.AllowUserToDeleteRows = false;
			this.gridRequirements.AllowUserToResizeColumns = false;
			this.gridRequirements.AllowUserToResizeRows = false;
			this.gridRequirements.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridRequirements.BackgroundColor = System.Drawing.Color.White;
			this.gridRequirements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridRequirements.ColumnHeadersVisible = false;
			this.gridRequirements.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colImage,
            this.colMessage,
            this.colLink});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridRequirements.DefaultCellStyle = dataGridViewCellStyle1;
			this.gridRequirements.Location = new System.Drawing.Point(12, 12);
			this.gridRequirements.MultiSelect = false;
			this.gridRequirements.Name = "gridRequirements";
			this.gridRequirements.ReadOnly = true;
			this.gridRequirements.RowHeadersVisible = false;
			this.gridRequirements.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridRequirements.Size = new System.Drawing.Size(385, 230);
			this.gridRequirements.TabIndex = 0;
			this.gridRequirements.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRequirements_CellContentClick);
			// 
			// colImage
			// 
			this.colImage.HeaderText = "";
			this.colImage.Name = "colImage";
			this.colImage.ReadOnly = true;
			this.colImage.Width = 5;
			// 
			// colMessage
			// 
			this.colMessage.HeaderText = "";
			this.colMessage.Name = "colMessage";
			this.colMessage.ReadOnly = true;
			this.colMessage.Width = 5;
			// 
			// colLink
			// 
			this.colLink.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colLink.HeaderText = "";
			this.colLink.Name = "colLink";
			this.colLink.ReadOnly = true;
			// 
			// btnRetry
			// 
			this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnRetry.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnRetry.Location = new System.Drawing.Point(101, 266);
			this.btnRetry.Name = "btnRetry";
			this.btnRetry.Size = new System.Drawing.Size(100, 23);
			this.btnRetry.TabIndex = 1;
			this.btnRetry.Text = "&Retry";
			this.btnRetry.UseVisualStyleBackColor = true;
			this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
			// 
			// FormMain
			// 
			this.AcceptButton = this.btnRetry;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(409, 301);
			this.Controls.Add(this.btnRetry);
			this.Controls.Add(this.gridRequirements);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(425, 242);
			this.Name = "FormMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "RegScoreCalc Bootstrapper";
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
			((System.ComponentModel.ISupportInitialize)(this.gridRequirements)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.ComponentModel.BackgroundWorker worker;
		private System.Windows.Forms.DataGridView gridRequirements;
		private System.Windows.Forms.DataGridViewImageColumn colImage;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
		private System.Windows.Forms.DataGridViewLinkColumn colLink;
		private System.Windows.Forms.Button btnRetry;
	}
}

