using System;
using System.Windows.Forms;

namespace RegScoreCalc
{
	partial class FormScriptHelp
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
            System.Windows.Forms.ColumnHeader columnHeader1;
            this.btnCancel = new RegScoreCalc.RibbonStyleButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listFunctions = new System.Windows.Forms.ListView();
            this.btnOK = new RegScoreCalc.RibbonStyleButton();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.groupProperties = new System.Windows.Forms.GroupBox();
            this.txtExample = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2.SuspendLayout();
            this.groupProperties.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Name";
            columnHeader1.Width = 200;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.DrawNormalBorder = false;
            this.btnCancel.HoverImage = null;
            this.btnCancel.IsHighlighted = false;
            this.btnCancel.Location = new System.Drawing.Point(465, 373);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormalImage = null;
            this.btnCancel.PressedImage = null;
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.listFunctions);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 351);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Function List";
            // 
            // listFunctions
            // 
            this.listFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listFunctions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            columnHeader1});
            this.listFunctions.FullRowSelect = true;
            this.listFunctions.GridLines = true;
            this.listFunctions.HideSelection = false;
            this.listFunctions.Location = new System.Drawing.Point(11, 19);
            this.listFunctions.MultiSelect = false;
            this.listFunctions.Name = "listFunctions";
            this.listFunctions.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listFunctions.Size = new System.Drawing.Size(237, 326);
            this.listFunctions.TabIndex = 1;
            this.listFunctions.UseCompatibleStateImageBehavior = false;
            this.listFunctions.View = System.Windows.Forms.View.Details;
            this.listFunctions.SelectedIndexChanged += new System.EventHandler(this.lvDynamicColumns_SelectedIndexChanged);
            this.listFunctions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.onUse);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.DrawNormalBorder = false;
            this.btnOK.HoverImage = null;
            this.btnOK.IsHighlighted = false;
            this.btnOK.Location = new System.Drawing.Point(384, 373);
            this.btnOK.Name = "btnOK";
            this.btnOK.NormalImage = null;
            this.btnOK.PressedImage = null;
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Use";
            this.btnOK.UseVisualStyleBackColor = true;              
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(6, 19);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(247, 192);
            this.txtDescription.TabIndex = 3;
            // 
            // groupProperties
            // 
            this.groupProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupProperties.Controls.Add(this.txtDescription);
            this.groupProperties.Location = new System.Drawing.Point(277, 12);
            this.groupProperties.Name = "groupProperties";
            this.groupProperties.Size = new System.Drawing.Size(259, 222);
            this.groupProperties.TabIndex = 0;
            this.groupProperties.TabStop = false;
            this.groupProperties.Text = "Description";
            // 
            // txtExample
            // 
            this.txtExample.Location = new System.Drawing.Point(6, 19);
            this.txtExample.Multiline = true;
            this.txtExample.Name = "txtExample";
            this.txtExample.ReadOnly = true;
            this.txtExample.Size = new System.Drawing.Size(247, 98);
            this.txtExample.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtExample);
            this.groupBox1.Location = new System.Drawing.Point(274, 240);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 123);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Example";
            // 
            // FormScriptHelp
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(552, 408);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupProperties);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(568, 419);
            this.Name = "FormScriptHelp";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Help";
            this.groupBox2.ResumeLayout(false);
            this.groupProperties.ResumeLayout(false);
            this.groupProperties.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormScriptHelp_Closing);
        }

        #endregion

        private RibbonStyleButton btnCancel;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ListView listFunctions;
		private RibbonStyleButton btnOK;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.GroupBox groupProperties;
        private System.Windows.Forms.TextBox txtExample;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}