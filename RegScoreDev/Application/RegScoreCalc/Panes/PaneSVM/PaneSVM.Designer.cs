namespace RegScoreCalc
{
	partial class PaneSVM
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbSVMValidation = new System.Windows.Forms.CheckBox();
            this.lblNegativeValidation = new System.Windows.Forms.Label();
            this.lblPositiveValidation = new System.Windows.Forms.Label();
            this.numericNegativeValidation = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericPositiveValidation = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericNumberUncategorized = new System.Windows.Forms.NumericUpDown();
            this.lblRangeMax = new System.Windows.Forms.Label();
            this.lblRangeMin = new System.Windows.Forms.Label();
            this.gbPositive = new System.Windows.Forms.GroupBox();
            this.txtPositiveCounter = new System.Windows.Forms.TextBox();
            this.txtNegativeCounter = new System.Windows.Forms.TextBox();
            this.btnPositiveRemove = new System.Windows.Forms.Button();
            this.btnPositiveAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbPositiveAllSVM = new System.Windows.Forms.ListBox();
            this.lbPositiveChoosedSVM = new System.Windows.Forms.ListBox();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNegativeValidation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPositiveValidation)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNumberUncategorized)).BeginInit();
            this.gbPositive.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 95);
            this.listBox1.TabIndex = 0;
            // 
            // listBox2
            // 
            this.listBox2.Location = new System.Drawing.Point(0, 0);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(120, 95);
            this.listBox2.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1059, 720);
            this.panel2.TabIndex = 10;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chbSVMValidation);
            this.groupBox3.Controls.Add(this.lblNegativeValidation);
            this.groupBox3.Controls.Add(this.lblPositiveValidation);
            this.groupBox3.Controls.Add(this.numericNegativeValidation);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.numericPositiveValidation);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox3.Location = new System.Drawing.Point(0, 209);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(694, 144);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // chbSVMValidation
            // 
            this.chbSVMValidation.AutoSize = true;
            this.chbSVMValidation.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.chbSVMValidation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.chbSVMValidation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chbSVMValidation.Location = new System.Drawing.Point(17, 27);
            this.chbSVMValidation.Name = "chbSVMValidation";
            this.chbSVMValidation.Size = new System.Drawing.Size(521, 23);
            this.chbSVMValidation.TabIndex = 13;
            this.chbSVMValidation.Text = "Divide my categorized data into a derivation and a validation sets.";
            this.chbSVMValidation.UseVisualStyleBackColor = true;
            // 
            // lblNegativeValidation
            // 
            this.lblNegativeValidation.AutoSize = true;
            this.lblNegativeValidation.Location = new System.Drawing.Point(252, 94);
            this.lblNegativeValidation.Name = "lblNegativeValidation";
            this.lblNegativeValidation.Size = new System.Drawing.Size(259, 19);
            this.lblNegativeValidation.TabIndex = 12;
            this.lblNegativeValidation.Text = "for validation and 0 for derivation";
            // 
            // lblPositiveValidation
            // 
            this.lblPositiveValidation.AutoSize = true;
            this.lblPositiveValidation.Location = new System.Drawing.Point(252, 67);
            this.lblPositiveValidation.Name = "lblPositiveValidation";
            this.lblPositiveValidation.Size = new System.Drawing.Size(259, 19);
            this.lblPositiveValidation.TabIndex = 11;
            this.lblPositiveValidation.Text = "for validation and 0 for derivation";
            // 
            // numericNegativeValidation
            // 
            this.numericNegativeValidation.Location = new System.Drawing.Point(162, 92);
            this.numericNegativeValidation.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericNegativeValidation.Name = "numericNegativeValidation";
            this.numericNegativeValidation.Size = new System.Drawing.Size(75, 26);
            this.numericNegativeValidation.TabIndex = 10;
            this.numericNegativeValidation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericNegativeValidation.ValueChanged += new System.EventHandler(this.numericNegativeValidation_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Negative: Use";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(110, 19);
            this.label6.TabIndex = 1;
            this.label6.Text = "Positive: Use";
            // 
            // numericPositiveValidation
            // 
            this.numericPositiveValidation.Location = new System.Drawing.Point(162, 65);
            this.numericPositiveValidation.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericPositiveValidation.Name = "numericPositiveValidation";
            this.numericPositiveValidation.Size = new System.Drawing.Size(75, 26);
            this.numericPositiveValidation.TabIndex = 0;
            this.numericPositiveValidation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericPositiveValidation.ValueChanged += new System.EventHandler(this.numericPositiveValidation_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Location = new System.Drawing.Point(0, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1080, 185);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TRAINING SET";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.gbPositive);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1074, 160);
            this.panel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.numericNumberUncategorized);
            this.groupBox4.Controls.Add(this.lblRangeMax);
            this.groupBox4.Controls.Add(this.lblRangeMin);
            this.groupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox4.Location = new System.Drawing.Point(633, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(417, 157);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SCORE 0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(17, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(351, 14);
            this.label5.TabIndex = 21;
            this.label5.Text = "Add documents with score 0 as negative documents to the training set.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(172, 19);
            this.label4.TabIndex = 20;
            this.label4.Text = "NO. OF DOCUMENTS";
            // 
            // numericNumberUncategorized
            // 
            this.numericNumberUncategorized.Location = new System.Drawing.Point(224, 53);
            this.numericNumberUncategorized.Name = "numericNumberUncategorized";
            this.numericNumberUncategorized.Size = new System.Drawing.Size(110, 26);
            this.numericNumberUncategorized.TabIndex = 19;
            this.numericNumberUncategorized.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblRangeMax
            // 
            this.lblRangeMax.AutoSize = true;
            this.lblRangeMax.Location = new System.Drawing.Point(348, 55);
            this.lblRangeMax.Name = "lblRangeMax";
            this.lblRangeMax.Size = new System.Drawing.Size(18, 19);
            this.lblRangeMax.TabIndex = 18;
            this.lblRangeMax.Text = "0";
            // 
            // lblRangeMin
            // 
            this.lblRangeMin.AutoSize = true;
            this.lblRangeMin.Location = new System.Drawing.Point(336, 55);
            this.lblRangeMin.Name = "lblRangeMin";
            this.lblRangeMin.Size = new System.Drawing.Size(13, 19);
            this.lblRangeMin.TabIndex = 17;
            this.lblRangeMin.Text = "/";
            // 
            // gbPositive
            // 
            this.gbPositive.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbPositive.Controls.Add(this.txtPositiveCounter);
            this.gbPositive.Controls.Add(this.txtNegativeCounter);
            this.gbPositive.Controls.Add(this.btnPositiveRemove);
            this.gbPositive.Controls.Add(this.btnPositiveAdd);
            this.gbPositive.Controls.Add(this.label2);
            this.gbPositive.Controls.Add(this.label1);
            this.gbPositive.Controls.Add(this.lbPositiveAllSVM);
            this.gbPositive.Controls.Add(this.lbPositiveChoosedSVM);
            this.gbPositive.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbPositive.Location = new System.Drawing.Point(0, 0);
            this.gbPositive.Name = "gbPositive";
            this.gbPositive.Size = new System.Drawing.Size(627, 160);
            this.gbPositive.TabIndex = 5;
            this.gbPositive.TabStop = false;
            // 
            // txtPositiveCounter
            // 
            this.txtPositiveCounter.Location = new System.Drawing.Point(538, 20);
            this.txtPositiveCounter.Name = "txtPositiveCounter";
            this.txtPositiveCounter.Size = new System.Drawing.Size(82, 26);
            this.txtPositiveCounter.TabIndex = 10;
            this.txtPositiveCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNegativeCounter
            // 
            this.txtNegativeCounter.Location = new System.Drawing.Point(200, 20);
            this.txtNegativeCounter.Name = "txtNegativeCounter";
            this.txtNegativeCounter.Size = new System.Drawing.Size(82, 26);
            this.txtNegativeCounter.TabIndex = 9;
            this.txtNegativeCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnPositiveRemove
            // 
            this.btnPositiveRemove.Image = global::RegScoreCalc.Properties.Resources.leftArrow;
            this.btnPositiveRemove.Location = new System.Drawing.Point(288, 100);
            this.btnPositiveRemove.Name = "btnPositiveRemove";
            this.btnPositiveRemove.Size = new System.Drawing.Size(50, 47);
            this.btnPositiveRemove.TabIndex = 6;
            this.btnPositiveRemove.UseVisualStyleBackColor = true;
            this.btnPositiveRemove.Click += new System.EventHandler(this.btnPositiveRemove_Click);
            // 
            // btnPositiveAdd
            // 
            this.btnPositiveAdd.Image = global::RegScoreCalc.Properties.Resources.rightArrow;
            this.btnPositiveAdd.Location = new System.Drawing.Point(288, 46);
            this.btnPositiveAdd.Name = "btnPositiveAdd";
            this.btnPositiveAdd.Size = new System.Drawing.Size(50, 45);
            this.btnPositiveAdd.TabIndex = 5;
            this.btnPositiveAdd.UseVisualStyleBackColor = true;
            this.btnPositiveAdd.Click += new System.EventHandler(this.btnPositiveAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "NEGATIVE";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(341, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "POSITIVE";
            // 
            // lbPositiveAllSVM
            // 
            this.lbPositiveAllSVM.FormattingEnabled = true;
            this.lbPositiveAllSVM.ItemHeight = 19;
            this.lbPositiveAllSVM.Location = new System.Drawing.Point(4, 47);
            this.lbPositiveAllSVM.Name = "lbPositiveAllSVM";
            this.lbPositiveAllSVM.Size = new System.Drawing.Size(278, 99);
            this.lbPositiveAllSVM.Sorted = true;
            this.lbPositiveAllSVM.TabIndex = 1;
            // 
            // lbPositiveChoosedSVM
            // 
            this.lbPositiveChoosedSVM.FormattingEnabled = true;
            this.lbPositiveChoosedSVM.ItemHeight = 19;
            this.lbPositiveChoosedSVM.Location = new System.Drawing.Point(344, 47);
            this.lbPositiveChoosedSVM.Name = "lbPositiveChoosedSVM";
            this.lbPositiveChoosedSVM.Size = new System.Drawing.Size(277, 99);
            this.lbPositiveChoosedSVM.Sorted = true;
            this.lbPositiveChoosedSVM.TabIndex = 2;
            // 
            // PaneSVM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 720);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "PaneSVM";
            this.Text = "PaneTemplate";
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNegativeValidation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPositiveValidation)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNumberUncategorized)).EndInit();
            this.gbPositive.ResumeLayout(false);
            this.gbPositive.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.ListBox lbPositiveAllSVM;
        private System.Windows.Forms.ListBox lbPositiveChoosedSVM;
        private System.Windows.Forms.GroupBox gbPositive;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button btnPositiveRemove;
        private System.Windows.Forms.Button btnPositiveAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericPositiveValidation;
        private System.Windows.Forms.NumericUpDown numericNegativeValidation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblNegativeValidation;
        private System.Windows.Forms.Label lblPositiveValidation;
        private System.Windows.Forms.CheckBox chbSVMValidation;
        private System.Windows.Forms.TextBox txtPositiveCounter;
        private System.Windows.Forms.TextBox txtNegativeCounter;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericNumberUncategorized;
        private System.Windows.Forms.Label lblRangeMax;
        private System.Windows.Forms.Label lblRangeMin;


	}
}