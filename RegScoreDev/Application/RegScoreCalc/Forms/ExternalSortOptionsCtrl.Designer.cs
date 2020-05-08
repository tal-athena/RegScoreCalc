namespace RegScoreCalc.Forms
{
	partial class ExternalSortOptionsCtrl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbColumn = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbOrder = new System.Windows.Forms.ComboBox();
			this.chkbGroupSimilar = new System.Windows.Forms.CheckBox();
			this.chkbAlternateColor = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 5;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.cmbColumn, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.label2, 3, 0);
			this.tableLayoutPanel.Controls.Add(this.cmbOrder, 4, 0);
			this.tableLayoutPanel.Controls.Add(this.chkbGroupSimilar, 1, 1);
			this.tableLayoutPanel.Controls.Add(this.chkbAlternateColor, 4, 1);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(5, 5);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 2;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel.Size = new System.Drawing.Size(457, 179);
			this.tableLayoutPanel.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 21);
			this.label1.TabIndex = 2;
			this.label1.Text = "Sort by:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cmbColumn
			// 
			this.cmbColumn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbColumn.FormattingEnabled = true;
			this.cmbColumn.Location = new System.Drawing.Point(43, 0);
			this.cmbColumn.Margin = new System.Windows.Forms.Padding(0);
			this.cmbColumn.Name = "cmbColumn";
			this.cmbColumn.Size = new System.Drawing.Size(172, 21);
			this.cmbColumn.TabIndex = 3;
			this.cmbColumn.SelectedIndexChanged += new System.EventHandler(this.cmbColumn_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(238, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 21);
			this.label2.TabIndex = 4;
			this.label2.Text = "Order:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cmbOrder
			// 
			this.cmbOrder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbOrder.FormattingEnabled = true;
			this.cmbOrder.Items.AddRange(new object[] {
            "Ascending",
            "Descending"});
			this.cmbOrder.Location = new System.Drawing.Point(284, 0);
			this.cmbOrder.Margin = new System.Windows.Forms.Padding(0);
			this.cmbOrder.Name = "cmbOrder";
			this.cmbOrder.Size = new System.Drawing.Size(173, 21);
			this.cmbOrder.TabIndex = 5;
			// 
			// chkbGroupSimilar
			// 
			this.chkbGroupSimilar.AutoSize = true;
			this.chkbGroupSimilar.Location = new System.Drawing.Point(43, 31);
			this.chkbGroupSimilar.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.chkbGroupSimilar.Name = "chkbGroupSimilar";
			this.chkbGroupSimilar.Size = new System.Drawing.Size(86, 17);
			this.chkbGroupSimilar.TabIndex = 6;
			this.chkbGroupSimilar.Text = "Group similar";
			this.chkbGroupSimilar.UseVisualStyleBackColor = true;
			this.chkbGroupSimilar.CheckedChanged += new System.EventHandler(this.chkbGroupSimilar_CheckedChanged);
			// 
			// chkbAlternateColor
			// 
			this.chkbAlternateColor.AutoSize = true;
			this.chkbAlternateColor.Location = new System.Drawing.Point(284, 31);
			this.chkbAlternateColor.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
			this.chkbAlternateColor.Name = "chkbAlternateColor";
			this.chkbAlternateColor.Size = new System.Drawing.Size(94, 17);
			this.chkbAlternateColor.TabIndex = 7;
			this.chkbAlternateColor.Text = "Alternate color";
			this.chkbAlternateColor.UseVisualStyleBackColor = true;
			// 
			// ExternalSortOptionsCtrl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.tableLayoutPanel);
			this.Name = "ExternalSortOptionsCtrl";
			this.Padding = new System.Windows.Forms.Padding(5);
			this.Size = new System.Drawing.Size(467, 189);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbColumn;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbOrder;
		private System.Windows.Forms.CheckBox chkbGroupSimilar;
		private System.Windows.Forms.CheckBox chkbAlternateColor;
	}
}
