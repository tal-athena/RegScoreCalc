namespace RegScoreCalc
{
	partial class PaneCodes
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
            this.gridCodes = new System.Windows.Forms.DataGridView();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridCodes)).BeginInit();
            this.SuspendLayout();
            // 
            // gridCodes
            // 
            this.gridCodes.AllowUserToResizeRows = false;
            this.gridCodes.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCodes.Location = new System.Drawing.Point(0, 0);
            this.gridCodes.Name = "gridCodes";
            this.gridCodes.RowHeadersWidth = 20;
            this.gridCodes.Size = new System.Drawing.Size(1021, 568);
            this.gridCodes.TabIndex = 3;
            this.gridCodes.VirtualMode = true;
            this.gridCodes.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCodes_CellClick);
            this.gridCodes.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCodes_CellFormatting);
            this.gridCodes.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCodes_RowValidated);
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // colorDataGridViewTextBoxColumn
            // 
            this.colorDataGridViewTextBoxColumn.Name = "colorDataGridViewTextBoxColumn";
            // 
            // PaneCodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1021, 568);
            this.Controls.Add(this.gridCodes);
            this.Name = "PaneCodes";
            this.Text = "PaneCodes";
            ((System.ComponentModel.ISupportInitialize)(this.gridCodes)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridCodes;
	}
}