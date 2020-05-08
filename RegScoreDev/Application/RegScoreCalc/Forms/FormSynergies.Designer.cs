namespace RegScoreCalc.Forms
{
	partial class FormSynergies
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
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnOK = new RegScoreCalc.RibbonStyleButton();
			this.grid = new System.Windows.Forms.DataGridView();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.btnClearAll = new RegScoreCalc.RibbonStyleButton();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(719, 430);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.HoverImage = null;
			this.btnOK.Location = new System.Drawing.Point(638, 430);
			this.btnOK.Name = "btnOK";
			this.btnOK.NormalImage = null;
			this.btnOK.PressedImage = null;
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			// 
			// grid
			// 
			this.grid.AllowUserToAddRows = false;
			this.grid.AllowUserToDeleteRows = false;
			this.grid.AllowUserToResizeColumns = false;
			this.grid.AllowUserToResizeRows = false;
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.grid.BackgroundColor = System.Drawing.Color.White;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Location = new System.Drawing.Point(12, 12);
			this.grid.MultiSelect = false;
			this.grid.Name = "grid";
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.grid.Size = new System.Drawing.Size(782, 407);
			this.grid.TabIndex = 0;
			this.grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellClick);
			this.grid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
			this.grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_CellFormatting);
			this.grid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grid_CellPainting);
			this.grid.CurrentCellChanged += new System.EventHandler(this.grid_CurrentCellChanged);
			this.grid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.grid_EditingControlShowing);
			this.grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grid_KeyDown);
			// 
			// timer
			// 
			this.timer.Interval = 20;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// btnClearAll
			// 
			this.btnClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClearAll.HoverImage = null;
			this.btnClearAll.Location = new System.Drawing.Point(12, 430);
			this.btnClearAll.Name = "btnClearAll";
			this.btnClearAll.NormalImage = null;
			this.btnClearAll.PressedImage = null;
			this.btnClearAll.Size = new System.Drawing.Size(135, 23);
			this.btnClearAll.TabIndex = 3;
			this.btnClearAll.Text = "Reset All";
			this.btnClearAll.UseVisualStyleBackColor = true;
			this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
			// 
			// FormSynergies
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(806, 465);
			this.Controls.Add(this.btnClearAll);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Name = "FormSynergies";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Synergies";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSynergies_FormClosing);
			this.Load += new System.EventHandler(this.FormSynergies_Load);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private RegScoreCalc.RibbonStyleButton btnCancel;
		private RegScoreCalc.RibbonStyleButton btnOK;
		private System.Windows.Forms.DataGridView grid;
		private System.Windows.Forms.Timer timer;
		private RibbonStyleButton btnClearAll;
	}
}