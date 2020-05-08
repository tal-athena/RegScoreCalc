using System;
using System.Windows.Forms;

namespace Deploy
{
    partial class Form1
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
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnDeploy = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.textOutputPane = new System.Windows.Forms.TextBox();
            this.chkX86 = new System.Windows.Forms.CheckBox();
            this.chkX64 = new System.Windows.Forms.CheckBox();
            this.lblSolution = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.textVSPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(27, 127);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(86, 31);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open solution";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnDeploy
            // 
            this.btnDeploy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeploy.Location = new System.Drawing.Point(696, 128);
            this.btnDeploy.Name = "btnDeploy";
            this.btnDeploy.Size = new System.Drawing.Size(88, 30);
            this.btnDeploy.TabIndex = 1;
            this.btnDeploy.Text = "Deploy";
            this.btnDeploy.UseVisualStyleBackColor = true;
            this.btnDeploy.Click += new System.EventHandler(this.btnDeploy_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOutput.Location = new System.Drawing.Point(578, 130);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(102, 28);
            this.btnOutput.TabIndex = 2;
            this.btnOutput.Text = "Output Folder";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // textOutputPane
            // 
            this.textOutputPane.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textOutputPane.Location = new System.Drawing.Point(12, 187);
            this.textOutputPane.Multiline = true;
            this.textOutputPane.Name = "textOutputPane";
            this.textOutputPane.ReadOnly = true;
            this.textOutputPane.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textOutputPane.Size = new System.Drawing.Size(792, 299);
            this.textOutputPane.TabIndex = 3;
            // 
            // chkX86
            // 
            this.chkX86.AutoSize = true;
            this.chkX86.Location = new System.Drawing.Point(149, 130);
            this.chkX86.Name = "chkX86";
            this.chkX86.Size = new System.Drawing.Size(43, 17);
            this.chkX86.TabIndex = 4;
            this.chkX86.Text = "x86";
            this.chkX86.UseVisualStyleBackColor = true;
            // 
            // chkX64
            // 
            this.chkX64.AutoSize = true;
            this.chkX64.Location = new System.Drawing.Point(149, 153);
            this.chkX64.Name = "chkX64";
            this.chkX64.Size = new System.Drawing.Size(43, 17);
            this.chkX64.TabIndex = 5;
            this.chkX64.Text = "x64";
            this.chkX64.UseVisualStyleBackColor = true;
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSolution.Location = new System.Drawing.Point(24, 72);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(62, 16);
            this.lblSolution.TabIndex = 6;
            this.lblSolution.Text = "Solution: ";
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutput.Location = new System.Drawing.Point(24, 99);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(49, 16);
            this.lblOutput.TabIndex = 7;
            this.lblOutput.Text = "Output:";
            // 
            // textVSPath
            // 
            this.textVSPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textVSPath.Location = new System.Drawing.Point(27, 12);
            this.textVSPath.Name = "textVSPath";
            this.textVSPath.Size = new System.Drawing.Size(782, 20);
            this.textVSPath.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(336, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Open developer command prompt for Visual Studio and copy the Path";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 498);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textVSPath);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.lblSolution);
            this.Controls.Add(this.chkX64);
            this.Controls.Add(this.chkX86);
            this.Controls.Add(this.textOutputPane);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.btnDeploy);
            this.Controls.Add(this.btnOpen);
            this.Name = "Form1";
            this.Text = "Deploy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClose);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnDeploy;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.TextBox textOutputPane;
        private System.Windows.Forms.CheckBox chkX86;
        private System.Windows.Forms.CheckBox chkX64;
        private System.Windows.Forms.Label lblSolution;
        private Label lblOutput;
        private TextBox textVSPath;
        private Label label1;
    }
}

