namespace RegScoreCalc.Forms
{
	partial class Performance
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
			this.gridResults = new System.Windows.Forms.DataGridView();
			this.colMethodName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colRecordsUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colElapsedTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colElapsedPerRecord = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnStoredProcedure = new System.Windows.Forms.Button();
			this.btnSimpleUpdate = new System.Windows.Forms.Button();
			this.txtMaxRowsToUpdate = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.chkbPrepareCommand = new System.Windows.Forms.CheckBox();
			this.btnCreateProcedure = new System.Windows.Forms.Button();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnCreateIndex = new System.Windows.Forms.Button();
			this.btnDeleteIndex = new System.Windows.Forms.Button();
			this.lblIndexName = new System.Windows.Forms.Label();
			this.textBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
			((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtMaxRowsToUpdate)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridResults
			// 
			this.gridResults.AllowUserToAddRows = false;
			this.gridResults.AllowUserToDeleteRows = false;
			this.gridResults.AllowUserToResizeRows = false;
			this.gridResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridResults.BackgroundColor = System.Drawing.Color.White;
			this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMethodName,
            this.colRecordsUpdated,
            this.colElapsedTotal,
            this.colElapsedPerRecord});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridResults.DefaultCellStyle = dataGridViewCellStyle1;
			this.gridResults.Location = new System.Drawing.Point(12, 196);
			this.gridResults.Name = "gridResults";
			this.gridResults.ReadOnly = true;
			this.gridResults.RowHeadersVisible = false;
			this.gridResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridResults.Size = new System.Drawing.Size(1100, 153);
			this.gridResults.TabIndex = 8;
			// 
			// colMethodName
			// 
			this.colMethodName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colMethodName.HeaderText = "Method Name";
			this.colMethodName.Name = "colMethodName";
			this.colMethodName.ReadOnly = true;
			this.colMethodName.Width = 91;
			// 
			// colRecordsUpdated
			// 
			this.colRecordsUpdated.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colRecordsUpdated.HeaderText = "Records Updated";
			this.colRecordsUpdated.Name = "colRecordsUpdated";
			this.colRecordsUpdated.ReadOnly = true;
			this.colRecordsUpdated.Width = 106;
			// 
			// colElapsedTotal
			// 
			this.colElapsedTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.colElapsedTotal.HeaderText = "Elapsed Total";
			this.colElapsedTotal.Name = "colElapsedTotal";
			this.colElapsedTotal.ReadOnly = true;
			this.colElapsedTotal.Width = 89;
			// 
			// colElapsedPerRecord
			// 
			this.colElapsedPerRecord.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colElapsedPerRecord.HeaderText = "Elapsed Per Record";
			this.colElapsedPerRecord.Name = "colElapsedPerRecord";
			this.colElapsedPerRecord.ReadOnly = true;
			// 
			// btnStoredProcedure
			// 
			this.btnStoredProcedure.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnStoredProcedure.Location = new System.Drawing.Point(210, 12);
			this.btnStoredProcedure.Name = "btnStoredProcedure";
			this.btnStoredProcedure.Size = new System.Drawing.Size(192, 77);
			this.btnStoredProcedure.TabIndex = 2;
			this.btnStoredProcedure.Text = "Stored Procedure";
			this.btnStoredProcedure.UseVisualStyleBackColor = true;
			this.btnStoredProcedure.Click += new System.EventHandler(this.btnStoredProcedure_Click);
			// 
			// btnSimpleUpdate
			// 
			this.btnSimpleUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnSimpleUpdate.Location = new System.Drawing.Point(12, 12);
			this.btnSimpleUpdate.Name = "btnSimpleUpdate";
			this.btnSimpleUpdate.Size = new System.Drawing.Size(192, 77);
			this.btnSimpleUpdate.TabIndex = 0;
			this.btnSimpleUpdate.Text = "Simple Update";
			this.btnSimpleUpdate.UseVisualStyleBackColor = true;
			this.btnSimpleUpdate.Click += new System.EventHandler(this.btnSimpleUpdate_Click);
			// 
			// txtMaxRowsToUpdate
			// 
			this.txtMaxRowsToUpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtMaxRowsToUpdate.Increment = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.txtMaxRowsToUpdate.Location = new System.Drawing.Point(352, 101);
			this.txtMaxRowsToUpdate.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.txtMaxRowsToUpdate.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.txtMaxRowsToUpdate.Name = "txtMaxRowsToUpdate";
			this.txtMaxRowsToUpdate.Size = new System.Drawing.Size(120, 29);
			this.txtMaxRowsToUpdate.TabIndex = 4;
			this.txtMaxRowsToUpdate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtMaxRowsToUpdate.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(12, 103);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(243, 24);
			this.label1.TabIndex = 4;
			this.label1.Text = "Maximum Rows to Process:";
			// 
			// chkbPrepareCommand
			// 
			this.chkbPrepareCommand.AutoSize = true;
			this.chkbPrepareCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.chkbPrepareCommand.Location = new System.Drawing.Point(499, 102);
			this.chkbPrepareCommand.Name = "chkbPrepareCommand";
			this.chkbPrepareCommand.Size = new System.Drawing.Size(189, 28);
			this.chkbPrepareCommand.TabIndex = 6;
			this.chkbPrepareCommand.Text = "Prepare Command";
			this.chkbPrepareCommand.UseVisualStyleBackColor = true;
			// 
			// btnCreateProcedure
			// 
			this.btnCreateProcedure.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnCreateProcedure.Location = new System.Drawing.Point(499, 143);
			this.btnCreateProcedure.Name = "btnCreateProcedure";
			this.btnCreateProcedure.Size = new System.Drawing.Size(299, 35);
			this.btnCreateProcedure.TabIndex = 7;
			this.btnCreateProcedure.Text = "Create Stored Procedure";
			this.btnCreateProcedure.UseVisualStyleBackColor = true;
			this.btnCreateProcedure.Click += new System.EventHandler(this.btnCreateProcedure_Click);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn1.HeaderText = "Method Name";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn2.HeaderText = "Records Updated";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.dataGridViewTextBoxColumn3.HeaderText = "Elapsed Total";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			// 
			// dataGridViewTextBoxColumn4
			// 
			this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.dataGridViewTextBoxColumn4.HeaderText = "Elapsed Per Record";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnClear.Location = new System.Drawing.Point(12, 693);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(108, 35);
			this.btnClear.TabIndex = 9;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnClose.Location = new System.Drawing.Point(1004, 693);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(108, 35);
			this.btnClose.TabIndex = 10;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.btnCreateIndex);
			this.groupBox1.Controls.Add(this.btnDeleteIndex);
			this.groupBox1.Controls.Add(this.lblIndexName);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.groupBox1.Location = new System.Drawing.Point(804, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(308, 166);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Index";
			// 
			// btnCreateIndex
			// 
			this.btnCreateIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnCreateIndex.Location = new System.Drawing.Point(6, 95);
			this.btnCreateIndex.Name = "btnCreateIndex";
			this.btnCreateIndex.Size = new System.Drawing.Size(296, 35);
			this.btnCreateIndex.TabIndex = 8;
			this.btnCreateIndex.Text = "Create Index";
			this.btnCreateIndex.UseVisualStyleBackColor = true;
			this.btnCreateIndex.Click += new System.EventHandler(this.btnCreateIndex_Click);
			// 
			// btnDeleteIndex
			// 
			this.btnDeleteIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnDeleteIndex.Location = new System.Drawing.Point(6, 54);
			this.btnDeleteIndex.Name = "btnDeleteIndex";
			this.btnDeleteIndex.Size = new System.Drawing.Size(296, 35);
			this.btnDeleteIndex.TabIndex = 8;
			this.btnDeleteIndex.Text = "Delete Index";
			this.btnDeleteIndex.UseVisualStyleBackColor = true;
			this.btnDeleteIndex.Click += new System.EventHandler(this.btnDeleteIndex_Click);
			// 
			// lblIndexName
			// 
			this.lblIndexName.AutoSize = true;
			this.lblIndexName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblIndexName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblIndexName.Location = new System.Drawing.Point(6, 25);
			this.lblIndexName.Name = "lblIndexName";
			this.lblIndexName.Size = new System.Drawing.Size(115, 26);
			this.lblIndexName.TabIndex = 5;
			this.lblIndexName.Text = "Index Name";
			// 
			// textBox1
			// 
			this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox1.Location = new System.Drawing.Point(12, 364);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(1100, 318);
			this.textBox1.TabIndex = 12;
			// 
			// Performance
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(1124, 740);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.chkbPrepareCommand);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnCreateProcedure);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtMaxRowsToUpdate);
			this.Controls.Add(this.gridResults);
			this.Controls.Add(this.btnStoredProcedure);
			this.Controls.Add(this.btnSimpleUpdate);
			this.Name = "Performance";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Performance";
			this.Load += new System.EventHandler(this.Performance_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtMaxRowsToUpdate)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView gridResults;
		private System.Windows.Forms.Button btnStoredProcedure;
		private System.Windows.Forms.DataGridViewTextBoxColumn colMethodName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colRecordsUpdated;
		private System.Windows.Forms.DataGridViewTextBoxColumn colElapsedTotal;
		private System.Windows.Forms.DataGridViewTextBoxColumn colElapsedPerRecord;
		private System.Windows.Forms.Button btnSimpleUpdate;
		private System.Windows.Forms.NumericUpDown txtMaxRowsToUpdate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkbPrepareCommand;
		private System.Windows.Forms.Button btnCreateProcedure;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label lblIndexName;
		private System.Windows.Forms.Button btnCreateIndex;
		private System.Windows.Forms.Button btnDeleteIndex;
		private FastColoredTextBoxNS.FastColoredTextBox textBox1;
	}
}