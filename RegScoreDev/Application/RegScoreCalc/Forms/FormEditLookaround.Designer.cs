using RegScoreCalc.Code.Controls;

namespace RegScoreCalc.Forms
{
	partial class FormEditLookaround
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
			this.components = new System.ComponentModel.Container();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnOK = new RegScoreCalc.RibbonStyleButton();
			this.txtValue = new CustomTextBox();
			this.splitter = new System.Windows.Forms.SplitContainer();
			this.lvQuickActions = new RegScoreCalc.Controls.ToolboxCtrl();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
			this.splitter.Panel1.SuspendLayout();
			this.splitter.Panel2.SuspendLayout();
			this.splitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-37, 209);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(703, 2);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(542, 221);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.HoverImage = null;
			this.btnOK.Location = new System.Drawing.Point(461, 221);
			this.btnOK.Name = "btnOK";
			this.btnOK.NormalImage = null;
			this.btnOK.PressedImage = null;
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// txtValue
			// 
			this.txtValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtValue.Location = new System.Drawing.Point(0, 0);
			this.txtValue.Multiline = true;
			this.txtValue.Name = "txtValue";
			this.txtValue.Size = new System.Drawing.Size(370, 184);
			this.txtValue.TabIndex = 0;
			// 
			// splitter
			// 
			this.splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitter.Location = new System.Drawing.Point(12, 12);
			this.splitter.Name = "splitter";
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.txtValue);
			// 
			// splitter.Panel2
			// 
			this.splitter.Panel2.Controls.Add(this.lvQuickActions);
			this.splitter.Size = new System.Drawing.Size(605, 184);
			this.splitter.SplitterDistance = 370;
			this.splitter.SplitterWidth = 10;
			this.splitter.TabIndex = 0;
			// 
			// lvQuickActions
			// 
			this.lvQuickActions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvQuickActions.Location = new System.Drawing.Point(0, 0);
			this.lvQuickActions.Margin = new System.Windows.Forms.Padding(0);
			this.lvQuickActions.Name = "lvQuickActions";
			this.lvQuickActions.Size = new System.Drawing.Size(225, 184);
			this.lvQuickActions.TabIndex = 0;
			// 
			// FormEditLookaround
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(629, 256);
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox1);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormEditLookaround";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit Lookaround";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEditLookaround_FormClosing);
			this.Load += new System.EventHandler(this.FormEditLookaround_Load);
			this.splitter.Panel1.ResumeLayout(false);
			this.splitter.Panel1.PerformLayout();
			this.splitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
			this.splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private CustomTextBox txtValue;
		private System.Windows.Forms.SplitContainer splitter;
		private Controls.ToolboxCtrl lvQuickActions;
	}
}