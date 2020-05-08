namespace RegScoreCalc
{
    partial class PaneReviewStatistics
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.lblRatioLast10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblRatio = new System.Windows.Forms.Label();
            this.panelCutoff = new System.Windows.Forms.Panel();
            this.lblKappa95 = new System.Windows.Forms.Label();
            this.lblKappa = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.checkBox_ManualPrev = new System.Windows.Forms.CheckBox();
            this.numericPrevalence = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.lblAccuracy95 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblNegPredVal95 = new System.Windows.Forms.Label();
            this.lblPosPredVal95 = new System.Windows.Forms.Label();
            this.lblPrelevance95 = new System.Windows.Forms.Label();
            this.lblSpecificity95 = new System.Windows.Forms.Label();
            this.lblSensitivity95 = new System.Windows.Forms.Label();
            this.lblNegPredValue = new System.Windows.Forms.Label();
            this.lblPrelevance = new System.Windows.Forms.Label();
            this.lblPosPredValue = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblSpecificity = new System.Windows.Forms.Label();
            this.lblSensitivity = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBoxCutoff = new System.Windows.Forms.GroupBox();
            this.lblNegTrue = new System.Windows.Forms.Label();
            this.lblPosFalse = new System.Windows.Forms.Label();
            this.lblNegFalse = new System.Windows.Forms.Label();
            this.lblPosTrue = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericCutoff = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelCutoff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPrevalence)).BeginInit();
            this.groupBoxCutoff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCutoff)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbCategories);
            this.panel1.Controls.Add(this.lblRatioLast10);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.lblRatio);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(0, 56);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Category:";
            // 
            // cmbCategories
            // 
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(110, 12);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(232, 28);
            this.cmbCategories.TabIndex = 0;
            // 
            // lblRatioLast10
            // 
            this.lblRatioLast10.AutoSize = true;
            this.lblRatioLast10.Location = new System.Drawing.Point(656, 18);
            this.lblRatioLast10.Name = "lblRatioLast10";
            this.lblRatioLast10.Size = new System.Drawing.Size(18, 20);
            this.lblRatioLast10.TabIndex = 5;
            this.lblRatioLast10.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(372, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Ratio:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(509, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Ratio in last 10:";
            // 
            // lblRatio
            // 
            this.lblRatio.AutoSize = true;
            this.lblRatio.Location = new System.Drawing.Point(434, 18);
            this.lblRatio.Name = "lblRatio";
            this.lblRatio.Size = new System.Drawing.Size(18, 20);
            this.lblRatio.TabIndex = 3;
            this.lblRatio.Text = "0";
            // 
            // panelCutoff
            // 
            this.panelCutoff.BackColor = System.Drawing.Color.Transparent;
            this.panelCutoff.Controls.Add(this.lblKappa95);
            this.panelCutoff.Controls.Add(this.lblKappa);
            this.panelCutoff.Controls.Add(this.label20);
            this.panelCutoff.Controls.Add(this.checkBox_ManualPrev);
            this.panelCutoff.Controls.Add(this.numericPrevalence);
            this.panelCutoff.Controls.Add(this.label13);
            this.panelCutoff.Controls.Add(this.lblAccuracy95);
            this.panelCutoff.Controls.Add(this.label14);
            this.panelCutoff.Controls.Add(this.lblAccuracy);
            this.panelCutoff.Controls.Add(this.label12);
            this.panelCutoff.Controls.Add(this.groupBox1);
            this.panelCutoff.Controls.Add(this.lblNegPredVal95);
            this.panelCutoff.Controls.Add(this.lblPosPredVal95);
            this.panelCutoff.Controls.Add(this.lblPrelevance95);
            this.panelCutoff.Controls.Add(this.lblSpecificity95);
            this.panelCutoff.Controls.Add(this.lblSensitivity95);
            this.panelCutoff.Controls.Add(this.lblNegPredValue);
            this.panelCutoff.Controls.Add(this.lblPrelevance);
            this.panelCutoff.Controls.Add(this.lblPosPredValue);
            this.panelCutoff.Controls.Add(this.label16);
            this.panelCutoff.Controls.Add(this.label17);
            this.panelCutoff.Controls.Add(this.label18);
            this.panelCutoff.Controls.Add(this.lblSpecificity);
            this.panelCutoff.Controls.Add(this.lblSensitivity);
            this.panelCutoff.Controls.Add(this.label11);
            this.panelCutoff.Controls.Add(this.label10);
            this.panelCutoff.Controls.Add(this.groupBoxCutoff);
            this.panelCutoff.Controls.Add(this.label5);
            this.panelCutoff.Controls.Add(this.numericCutoff);
            this.panelCutoff.Controls.Add(this.label3);
            this.panelCutoff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCutoff.Location = new System.Drawing.Point(0, 0);
            this.panelCutoff.Name = "panelCutoff";
            this.panelCutoff.Size = new System.Drawing.Size(0, 0);
            this.panelCutoff.TabIndex = 6;
            // 
            // lblKappa95
            // 
            this.lblKappa95.AutoSize = true;
            this.lblKappa95.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblKappa95.Location = new System.Drawing.Point(741, 221);
            this.lblKappa95.Name = "lblKappa95";
            this.lblKappa95.Size = new System.Drawing.Size(15, 16);
            this.lblKappa95.TabIndex = 39;
            this.lblKappa95.Text = "0";
            // 
            // lblKappa
            // 
            this.lblKappa.AutoSize = true;
            this.lblKappa.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblKappa.Location = new System.Drawing.Point(613, 220);
            this.lblKappa.Name = "lblKappa";
            this.lblKappa.Size = new System.Drawing.Size(30, 16);
            this.lblKappa.TabIndex = 38;
            this.lblKappa.Text = "0 %";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label20.Location = new System.Drawing.Point(484, 222);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(51, 16);
            this.label20.TabIndex = 37;
            this.label20.Text = "Kappa:";
            // 
            // checkBox_ManualPrev
            // 
            this.checkBox_ManualPrev.AutoSize = true;
            this.checkBox_ManualPrev.Location = new System.Drawing.Point(469, 151);
            this.checkBox_ManualPrev.Name = "checkBox_ManualPrev";
            this.checkBox_ManualPrev.Size = new System.Drawing.Size(15, 14);
            this.checkBox_ManualPrev.TabIndex = 36;
            this.checkBox_ManualPrev.UseVisualStyleBackColor = true;
            this.checkBox_ManualPrev.CheckedChanged += new System.EventHandler(this.checkBox_ManualPrev_CheckedChanged);
            // 
            // numericPrevalence
            // 
            this.numericPrevalence.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericPrevalence.DecimalPlaces = 2;
            this.numericPrevalence.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numericPrevalence.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericPrevalence.Location = new System.Drawing.Point(612, 148);
            this.numericPrevalence.Name = "numericPrevalence";
            this.numericPrevalence.ReadOnly = true;
            this.numericPrevalence.Size = new System.Drawing.Size(62, 20);
            this.numericPrevalence.TabIndex = 35;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(676, 149);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(20, 16);
            this.label13.TabIndex = 34;
            this.label13.Text = "%";
            this.label13.Click += new System.EventHandler(this.label13_Click_1);
            // 
            // lblAccuracy95
            // 
            this.lblAccuracy95.AutoSize = true;
            this.lblAccuracy95.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccuracy95.Location = new System.Drawing.Point(741, 82);
            this.lblAccuracy95.Name = "lblAccuracy95";
            this.lblAccuracy95.Size = new System.Drawing.Size(15, 16);
            this.lblAccuracy95.TabIndex = 32;
            this.lblAccuracy95.Text = "0";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(484, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 16);
            this.label14.TabIndex = 31;
            this.label14.Text = "Accuracy: ";
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.AutoSize = true;
            this.lblAccuracy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblAccuracy.Location = new System.Drawing.Point(613, 81);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(16, 16);
            this.lblAccuracy.TabIndex = 16;
            this.lblAccuracy.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(697, 57);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(122, 13);
            this.label12.TabIndex = 30;
            this.label12.Text = "95% Confidence Interval";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(0, 232);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox1.Size = new System.Drawing.Size(0, 20);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // lblNegPredVal95
            // 
            this.lblNegPredVal95.AutoSize = true;
            this.lblNegPredVal95.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNegPredVal95.Location = new System.Drawing.Point(741, 197);
            this.lblNegPredVal95.Name = "lblNegPredVal95";
            this.lblNegPredVal95.Size = new System.Drawing.Size(15, 16);
            this.lblNegPredVal95.TabIndex = 28;
            this.lblNegPredVal95.Text = "0";
            // 
            // lblPosPredVal95
            // 
            this.lblPosPredVal95.AutoSize = true;
            this.lblPosPredVal95.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPosPredVal95.Location = new System.Drawing.Point(741, 174);
            this.lblPosPredVal95.Name = "lblPosPredVal95";
            this.lblPosPredVal95.Size = new System.Drawing.Size(15, 16);
            this.lblPosPredVal95.TabIndex = 27;
            this.lblPosPredVal95.Text = "0";
            // 
            // lblPrelevance95
            // 
            this.lblPrelevance95.AutoSize = true;
            this.lblPrelevance95.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPrelevance95.Location = new System.Drawing.Point(741, 150);
            this.lblPrelevance95.Name = "lblPrelevance95";
            this.lblPrelevance95.Size = new System.Drawing.Size(15, 16);
            this.lblPrelevance95.TabIndex = 26;
            this.lblPrelevance95.Text = "0";
            // 
            // lblSpecificity95
            // 
            this.lblSpecificity95.AutoSize = true;
            this.lblSpecificity95.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpecificity95.Location = new System.Drawing.Point(741, 127);
            this.lblSpecificity95.Name = "lblSpecificity95";
            this.lblSpecificity95.Size = new System.Drawing.Size(15, 16);
            this.lblSpecificity95.TabIndex = 25;
            this.lblSpecificity95.Text = "0";
            // 
            // lblSensitivity95
            // 
            this.lblSensitivity95.AutoSize = true;
            this.lblSensitivity95.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSensitivity95.Location = new System.Drawing.Point(741, 105);
            this.lblSensitivity95.Name = "lblSensitivity95";
            this.lblSensitivity95.Size = new System.Drawing.Size(15, 16);
            this.lblSensitivity95.TabIndex = 24;
            this.lblSensitivity95.Text = "0";
            // 
            // lblNegPredValue
            // 
            this.lblNegPredValue.AutoSize = true;
            this.lblNegPredValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNegPredValue.Location = new System.Drawing.Point(613, 196);
            this.lblNegPredValue.Name = "lblNegPredValue";
            this.lblNegPredValue.Size = new System.Drawing.Size(30, 16);
            this.lblNegPredValue.TabIndex = 22;
            this.lblNegPredValue.Text = "0 %";
            // 
            // lblPrelevance
            // 
            this.lblPrelevance.AutoSize = true;
            this.lblPrelevance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPrelevance.Location = new System.Drawing.Point(613, 149);
            this.lblPrelevance.Name = "lblPrelevance";
            this.lblPrelevance.Size = new System.Drawing.Size(30, 16);
            this.lblPrelevance.TabIndex = 21;
            this.lblPrelevance.Text = "0 %";
            this.lblPrelevance.Visible = false;
            // 
            // lblPosPredValue
            // 
            this.lblPosPredValue.AutoSize = true;
            this.lblPosPredValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPosPredValue.Location = new System.Drawing.Point(613, 173);
            this.lblPosPredValue.Name = "lblPosPredValue";
            this.lblPosPredValue.Size = new System.Drawing.Size(30, 16);
            this.lblPosPredValue.TabIndex = 20;
            this.lblPosPredValue.Text = "0 %";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label16.Location = new System.Drawing.Point(484, 150);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 16);
            this.label16.TabIndex = 19;
            this.label16.Text = "Prevalence:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.Location = new System.Drawing.Point(484, 198);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(104, 16);
            this.label17.TabIndex = 18;
            this.label17.Text = "Neg pred value:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label18.Location = new System.Drawing.Point(484, 174);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(102, 16);
            this.label18.TabIndex = 17;
            this.label18.Text = "Pos pred value:";
            // 
            // lblSpecificity
            // 
            this.lblSpecificity.AutoSize = true;
            this.lblSpecificity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSpecificity.Location = new System.Drawing.Point(613, 127);
            this.lblSpecificity.Name = "lblSpecificity";
            this.lblSpecificity.Size = new System.Drawing.Size(30, 16);
            this.lblSpecificity.TabIndex = 16;
            this.lblSpecificity.Text = "0 %";
            // 
            // lblSensitivity
            // 
            this.lblSensitivity.AutoSize = true;
            this.lblSensitivity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSensitivity.Location = new System.Drawing.Point(613, 104);
            this.lblSensitivity.Name = "lblSensitivity";
            this.lblSensitivity.Size = new System.Drawing.Size(30, 16);
            this.lblSensitivity.TabIndex = 14;
            this.lblSensitivity.Text = "0 %";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(484, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 16);
            this.label11.TabIndex = 12;
            this.label11.Text = "Specificity:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(484, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 16);
            this.label10.TabIndex = 11;
            this.label10.Text = "Sensitivity:";
            // 
            // groupBoxCutoff
            // 
            this.groupBoxCutoff.BackColor = System.Drawing.Color.Azure;
            this.groupBoxCutoff.Controls.Add(this.lblNegTrue);
            this.groupBoxCutoff.Controls.Add(this.lblPosFalse);
            this.groupBoxCutoff.Controls.Add(this.lblNegFalse);
            this.groupBoxCutoff.Controls.Add(this.lblPosTrue);
            this.groupBoxCutoff.Controls.Add(this.label9);
            this.groupBoxCutoff.Controls.Add(this.label8);
            this.groupBoxCutoff.Controls.Add(this.label6);
            this.groupBoxCutoff.Controls.Add(this.label7);
            this.groupBoxCutoff.Location = new System.Drawing.Point(215, 65);
            this.groupBoxCutoff.Name = "groupBoxCutoff";
            this.groupBoxCutoff.Size = new System.Drawing.Size(248, 143);
            this.groupBoxCutoff.TabIndex = 6;
            this.groupBoxCutoff.TabStop = false;
            this.groupBoxCutoff.Text = "2 x 2";
            this.groupBoxCutoff.Enter += new System.EventHandler(this.groupBoxCutoff_Enter);
            // 
            // lblNegTrue
            // 
            this.lblNegTrue.AutoSize = true;
            this.lblNegTrue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNegTrue.Location = new System.Drawing.Point(109, 95);
            this.lblNegTrue.Name = "lblNegTrue";
            this.lblNegTrue.Size = new System.Drawing.Size(13, 13);
            this.lblNegTrue.TabIndex = 14;
            this.lblNegTrue.Text = "0";
            // 
            // lblPosFalse
            // 
            this.lblPosFalse.AutoSize = true;
            this.lblPosFalse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPosFalse.Location = new System.Drawing.Point(185, 65);
            this.lblPosFalse.Name = "lblPosFalse";
            this.lblPosFalse.Size = new System.Drawing.Size(13, 13);
            this.lblPosFalse.TabIndex = 13;
            this.lblPosFalse.Text = "0";
            // 
            // lblNegFalse
            // 
            this.lblNegFalse.AutoSize = true;
            this.lblNegFalse.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblNegFalse.Location = new System.Drawing.Point(185, 94);
            this.lblNegFalse.Name = "lblNegFalse";
            this.lblNegFalse.Size = new System.Drawing.Size(13, 13);
            this.lblNegFalse.TabIndex = 12;
            this.lblNegFalse.Text = "0";
            // 
            // lblPosTrue
            // 
            this.lblPosTrue.AutoSize = true;
            this.lblPosTrue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblPosTrue.Location = new System.Drawing.Point(109, 65);
            this.lblPosTrue.Name = "lblPosTrue";
            this.lblPosTrue.Size = new System.Drawing.Size(13, 13);
            this.lblPosTrue.TabIndex = 11;
            this.lblPosTrue.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(6, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 15);
            this.label9.TabIndex = 10;
            this.label9.Text = "SVM model [+]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(6, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 15);
            this.label8.TabIndex = 9;
            this.label8.Text = "SVM model [-]";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(162, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "classified [-]";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(73, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 16);
            this.label7.TabIndex = 7;
            this.label7.Text = "classified [+]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(23, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(162, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "(grater than of equal are positive)";
            // 
            // numericCutoff
            // 
            this.numericCutoff.Location = new System.Drawing.Point(78, 80);
            this.numericCutoff.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numericCutoff.Name = "numericCutoff";
            this.numericCutoff.Size = new System.Drawing.Size(120, 26);
            this.numericCutoff.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Cutoff:";
            // 
            // PaneReviewStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelCutoff);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "PaneReviewStatistics";
            this.Text = "PaneTemplate";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelCutoff.ResumeLayout(false);
            this.panelCutoff.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericPrevalence)).EndInit();
            this.groupBoxCutoff.ResumeLayout(false);
            this.groupBoxCutoff.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCutoff)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        public System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label lblRatio;
        public System.Windows.Forms.Label lblRatioLast10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelCutoff;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label lblSpecificity;
        public System.Windows.Forms.Label lblSensitivity;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBoxCutoff;
       public System.Windows.Forms.Label lblNegTrue;
       public System.Windows.Forms.Label lblPosFalse;
       public System.Windows.Forms.Label lblNegFalse;
       public System.Windows.Forms.Label lblPosTrue;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.NumericUpDown numericCutoff;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label lblNegPredValue;
        public System.Windows.Forms.Label lblPrelevance;
        public System.Windows.Forms.Label lblPosPredValue;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        public System.Windows.Forms.Label lblNegPredVal95;
        public System.Windows.Forms.Label lblPosPredVal95;
        public System.Windows.Forms.Label lblPrelevance95;
        public System.Windows.Forms.Label lblSpecificity95;
        public System.Windows.Forms.Label lblSensitivity95;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Label lblAccuracy;
        public System.Windows.Forms.Label lblAccuracy95;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.Label label13;
        public System.Windows.Forms.NumericUpDown numericPrevalence;
        public System.Windows.Forms.CheckBox checkBox_ManualPrev;
        public System.Windows.Forms.Label lblKappa95;
        public System.Windows.Forms.Label lblKappa;
        private System.Windows.Forms.Label label20;
    }
}