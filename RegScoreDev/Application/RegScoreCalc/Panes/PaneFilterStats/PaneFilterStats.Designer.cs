namespace RegScoreCalc
{
	partial class PaneFilterStats
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chartFilterStats = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chbShowGreen = new System.Windows.Forms.CheckBox();
            this.chbShowRed = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblGreenPercent = new System.Windows.Forms.Label();
            this.lblRedPercent = new System.Windows.Forms.Label();
            this.lblTotalDocuments = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFilterStats)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chartFilterStats);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 0);
            this.panel1.TabIndex = 0;
            // 
            // chartFilterStats
            // 
            chartArea1.Name = "ChartAreaFilterStats";
            this.chartFilterStats.ChartAreas.Add(chartArea1);
            this.chartFilterStats.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Legend1";
            this.chartFilterStats.Legends.Add(legend1);
            this.chartFilterStats.Location = new System.Drawing.Point(0, 0);
            this.chartFilterStats.Name = "chartFilterStats";
            this.chartFilterStats.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            series1.ChartArea = "ChartAreaFilterStats";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "SeriesFilterStats";
            this.chartFilterStats.Series.Add(series1);
            this.chartFilterStats.Size = new System.Drawing.Size(0, 0);
            this.chartFilterStats.TabIndex = 0;
            this.chartFilterStats.Text = "Filter Stats";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chbShowGreen);
            this.panel2.Controls.Add(this.chbShowRed);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, -37);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 37);
            this.panel2.TabIndex = 1;
            // 
            // chbShowGreen
            // 
            this.chbShowGreen.AutoSize = true;
            this.chbShowGreen.Location = new System.Drawing.Point(236, 6);
            this.chbShowGreen.Name = "chbShowGreen";
            this.chbShowGreen.Size = new System.Drawing.Size(195, 28);
            this.chbShowGreen.TabIndex = 1;
            this.chbShowGreen.Text = "Show Concordant";
            this.chbShowGreen.UseVisualStyleBackColor = true;
            this.chbShowGreen.CheckedChanged += new System.EventHandler(this.chbShowGreen_CheckedChanged);
            // 
            // chbShowRed
            // 
            this.chbShowRed.AutoSize = true;
            this.chbShowRed.Checked = true;
            this.chbShowRed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbShowRed.Location = new System.Drawing.Point(32, 6);
            this.chbShowRed.Name = "chbShowRed";
            this.chbShowRed.Size = new System.Drawing.Size(186, 28);
            this.chbShowRed.TabIndex = 0;
            this.chbShowRed.Text = "Show Discordant";
            this.chbShowRed.UseVisualStyleBackColor = true;
            this.chbShowRed.CheckedChanged += new System.EventHandler(this.chbShowRed_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblGreenPercent);
            this.panel3.Controls.Add(this.lblRedPercent);
            this.panel3.Controls.Add(this.lblTotalDocuments);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(0, 34);
            this.panel3.TabIndex = 1;
            // 
            // lblGreenPercent
            // 
            this.lblGreenPercent.AutoSize = true;
            this.lblGreenPercent.Location = new System.Drawing.Point(202, 4);
            this.lblGreenPercent.Name = "lblGreenPercent";
            this.lblGreenPercent.Size = new System.Drawing.Size(146, 24);
            this.lblGreenPercent.TabIndex = 2;
            this.lblGreenPercent.Text = "Concordant %:";
            // 
            // lblRedPercent
            // 
            this.lblRedPercent.AutoSize = true;
            this.lblRedPercent.Location = new System.Drawing.Point(12, 4);
            this.lblRedPercent.Name = "lblRedPercent";
            this.lblRedPercent.Size = new System.Drawing.Size(137, 24);
            this.lblRedPercent.TabIndex = 1;
            this.lblRedPercent.Text = "Discordant %:";
            // 
            // lblTotalDocuments
            // 
            this.lblTotalDocuments.AutoSize = true;
            this.lblTotalDocuments.Location = new System.Drawing.Point(417, 4);
            this.lblTotalDocuments.Name = "lblTotalDocuments";
            this.lblTotalDocuments.Size = new System.Drawing.Size(171, 24);
            this.lblTotalDocuments.TabIndex = 0;
            this.lblTotalDocuments.Text = "Total documents:";
            // 
            // PaneFilterStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "PaneFilterStats";
            this.Text = "PaneTemplate";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartFilterStats)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chbShowGreen;
        private System.Windows.Forms.CheckBox chbShowRed;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFilterStats;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblGreenPercent;
        private System.Windows.Forms.Label lblRedPercent;
        private System.Windows.Forms.Label lblTotalDocuments;



    }
}