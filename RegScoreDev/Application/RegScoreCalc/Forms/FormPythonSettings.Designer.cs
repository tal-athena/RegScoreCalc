using System;

namespace RegScoreCalc
{
	partial class FormPythonSettings
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new RegScoreCalc.RibbonStyleButton();
            this.btnOK = new RegScoreCalc.RibbonStyleButton();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radioDefault = new System.Windows.Forms.RadioButton();
            this.radioSpecific = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listEnvironment = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new RegScoreCalc.RibbonStyleButton();
            this.textAnacondaPath = new System.Windows.Forms.TextBox();
            this.lblPythonVersion = new System.Windows.Forms.Label();
            this.lblAnacondaVersion = new System.Windows.Forms.Label();
            this.btnOpenConda = new RegScoreCalc.RibbonStyleButton();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(-16, 340);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(778, 2);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.DrawNormalBorder = false;
            this.btnCancel.HoverImage = null;
            this.btnCancel.IsHighlighted = false;
            this.btnCancel.Location = new System.Drawing.Point(659, 353);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormalImage = null;
            this.btnCancel.PressedImage = null;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.DrawNormalBorder = false;
            this.btnOK.HoverImage = null;
            this.btnOK.IsHighlighted = false;
            this.btnOK.Location = new System.Drawing.Point(578, 353);
            this.btnOK.Name = "btnOK";
            this.btnOK.NormalImage = null;
            this.btnOK.PressedImage = null;
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ID";
            this.dataGridViewTextBoxColumn1.HeaderText = "ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Category";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // radioDefault
            // 
            this.radioDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioDefault.AutoSize = true;
            this.radioDefault.Location = new System.Drawing.Point(12, 35);
            this.radioDefault.Name = "radioDefault";
            this.radioDefault.Size = new System.Drawing.Size(117, 17);
            this.radioDefault.TabIndex = 9;
            this.radioDefault.TabStop = true;
            this.radioDefault.Text = "Use Default Python";
            this.radioDefault.UseVisualStyleBackColor = true;
            this.radioDefault.CheckedChanged += new System.EventHandler(this.radioDefault_CheckedChanged);
            // 
            // radioSpecific
            // 
            this.radioSpecific.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioSpecific.AutoSize = true;
            this.radioSpecific.Location = new System.Drawing.Point(12, 59);
            this.radioSpecific.Name = "radioSpecific";
            this.radioSpecific.Size = new System.Drawing.Size(137, 17);
            this.radioSpecific.TabIndex = 10;
            this.radioSpecific.TabStop = true;
            this.radioSpecific.Text = "Use Specific Anaconda";
            this.radioSpecific.UseVisualStyleBackColor = true;
            this.radioSpecific.CheckedChanged += new System.EventHandler(this.radioSpecific_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listEnvironment);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnBrowse);
            this.groupBox2.Controls.Add(this.textAnacondaPath);
            this.groupBox2.Location = new System.Drawing.Point(12, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(722, 193);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // listEnvironment
            // 
            this.listEnvironment.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listEnvironment.FormattingEnabled = true;
            this.listEnvironment.Location = new System.Drawing.Point(113, 64);
            this.listEnvironment.Name = "listEnvironment";
            this.listEnvironment.Size = new System.Drawing.Size(486, 121);
            this.listEnvironment.TabIndex = 7;
            this.listEnvironment.SelectedIndexChanged += new System.EventHandler(this.VirtualEnvironmentChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Environment :";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Path to anaconda : ";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowse.DrawNormalBorder = false;
            this.btnBrowse.HoverImage = null;
            this.btnBrowse.IsHighlighted = false;
            this.btnBrowse.Location = new System.Drawing.Point(603, 29);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.NormalImage = null;
            this.btnBrowse.PressedImage = null;
            this.btnBrowse.Size = new System.Drawing.Size(111, 23);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // textAnacondaPath
            // 
            this.textAnacondaPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textAnacondaPath.Location = new System.Drawing.Point(113, 30);
            this.textAnacondaPath.Name = "textAnacondaPath";
            this.textAnacondaPath.Size = new System.Drawing.Size(486, 20);
            this.textAnacondaPath.TabIndex = 1;
            this.textAnacondaPath.TextChanged += new System.EventHandler(this.textAnacondaPath_TextChanged);
            // 
            // lblPythonVersion
            // 
            this.lblPythonVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPythonVersion.AutoSize = true;
            this.lblPythonVersion.Location = new System.Drawing.Point(19, 292);
            this.lblPythonVersion.Name = "lblPythonVersion";
            this.lblPythonVersion.Size = new System.Drawing.Size(87, 13);
            this.lblPythonVersion.TabIndex = 12;
            this.lblPythonVersion.Text = "Python Version : ";
            // 
            // lblAnacondaVersion
            // 
            this.lblAnacondaVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAnacondaVersion.AutoSize = true;
            this.lblAnacondaVersion.Location = new System.Drawing.Point(19, 312);
            this.lblAnacondaVersion.Name = "lblAnacondaVersion";
            this.lblAnacondaVersion.Size = new System.Drawing.Size(102, 13);
            this.lblAnacondaVersion.TabIndex = 13;
            this.lblAnacondaVersion.Text = "Anaconda version : ";
            // 
            // btnOpenConda
            // 
            this.btnOpenConda.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOpenConda.DrawNormalBorder = false;
            this.btnOpenConda.HoverImage = null;
            this.btnOpenConda.IsHighlighted = false;
            this.btnOpenConda.Location = new System.Drawing.Point(402, 353);
            this.btnOpenConda.Name = "btnOpenConda";
            this.btnOpenConda.NormalImage = null;
            this.btnOpenConda.PressedImage = null;
            this.btnOpenConda.Size = new System.Drawing.Size(149, 23);
            this.btnOpenConda.TabIndex = 8;
            this.btnOpenConda.Text = "Open Anaconda Prompt";
            this.btnOpenConda.UseVisualStyleBackColor = true;
            this.btnOpenConda.Click += new System.EventHandler(this.btnOpenConda_Click);
            // 
            // FormPythonSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(746, 388);
            this.Controls.Add(this.btnOpenConda);
            this.Controls.Add(this.lblAnacondaVersion);
            this.Controls.Add(this.lblPythonVersion);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.radioSpecific);
            this.Controls.Add(this.radioDefault);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(762, 427);
            this.Name = "FormPythonSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Python";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormColumns_FormClosing);
            this.Load += new System.EventHandler(this.Form_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.RadioButton radioDefault;
        private System.Windows.Forms.RadioButton radioSpecific;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textAnacondaPath;
        private RibbonStyleButton btnBrowse;
        private System.Windows.Forms.ListBox listEnvironment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblPythonVersion;
        private System.Windows.Forms.Label lblAnacondaVersion;
        private RibbonStyleButton btnOpenConda;
    }
}