using System.Drawing;

namespace RegScoreCalc
{
	partial class FormSplitNotes
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSplitNotes));
			this.splitter = new System.Windows.Forms.SplitContainer();
			this.btnSwap = new System.Windows.Forms.Button();
			this.lblRight = new System.Windows.Forms.Label();
			this.lblLeft = new System.Windows.Forms.Label();
			this.btnMakeSameSize = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
			this.splitter.Panel1.SuspendLayout();
			this.splitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitter
			// 
			this.splitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitter.IsSplitterFixed = true;
			this.splitter.Location = new System.Drawing.Point(0, 0);
			this.splitter.Name = "splitter";
			this.splitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitter.Panel1
			// 
			this.splitter.Panel1.Controls.Add(this.btnMakeSameSize);
			this.splitter.Panel1.Controls.Add(this.btnSwap);
			this.splitter.Panel1.Controls.Add(this.lblRight);
			this.splitter.Panel1.Controls.Add(this.lblLeft);
			this.splitter.Size = new System.Drawing.Size(662, 429);
			this.splitter.SplitterDistance = 45;
			this.splitter.SplitterWidth = 1;
			this.splitter.TabIndex = 0;
			// 
			// btnSwap
			// 
			this.btnSwap.Location = new System.Drawing.Point(228, 10);
			this.btnSwap.Name = "btnSwap";
			this.btnSwap.Size = new System.Drawing.Size(100, 25);
			this.btnSwap.TabIndex = 1;
			this.btnSwap.Text = "Swap";
			this.btnSwap.UseVisualStyleBackColor = true;
			this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
			// 
			// lblRight
			// 
			this.lblRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRight.AutoSize = true;
			this.lblRight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblRight.Location = new System.Drawing.Point(569, 12);
			this.lblRight.Name = "lblRight";
			this.lblRight.Size = new System.Drawing.Size(81, 20);
			this.lblRight.TabIndex = 0;
			this.lblRight.Text = "Discharge";
			// 
			// lblLeft
			// 
			this.lblLeft.AutoSize = true;
			this.lblLeft.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblLeft.Location = new System.Drawing.Point(12, 12);
			this.lblLeft.Name = "lblLeft";
			this.lblLeft.Size = new System.Drawing.Size(70, 20);
			this.lblLeft.TabIndex = 0;
			this.lblLeft.Text = "ED Note";
			// 
			// btnMakeSameSize
			// 
			this.btnMakeSameSize.Location = new System.Drawing.Point(334, 10);
			this.btnMakeSameSize.Name = "btnMakeSameSize";
			this.btnMakeSameSize.Size = new System.Drawing.Size(100, 25);
			this.btnMakeSameSize.TabIndex = 2;
			this.btnMakeSameSize.Text = "50% / 50%";
			this.btnMakeSameSize.UseVisualStyleBackColor = true;
			this.btnMakeSameSize.Click += new System.EventHandler(this.btnMakeSameSize_Click);
			// 
			// FormSplitNotes
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(662, 429);
			this.Controls.Add(this.splitter);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FormSplitNotes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Notes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSplitNotes_FormClosing);
			this.Load += new System.EventHandler(this.FormSplitNotes_Load);
			this.SizeChanged += new System.EventHandler(this.FormSplitNotes_SizeChanged);
			this.splitter.Panel1.ResumeLayout(false);
			this.splitter.Panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
			this.splitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitter;
		private System.Windows.Forms.Label lblRight;
		private System.Windows.Forms.Label lblLeft;
		private System.Windows.Forms.Button btnSwap;
		private System.Windows.Forms.Button btnMakeSameSize;
	}
}