namespace RegScoreCalc
{
	partial class FormSelectCategory
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnOK = new RegScoreCalc.RibbonStyleButton();
			this.btnAdd = new RegScoreCalc.RibbonStyleButton();
			this.btnRemove = new RegScoreCalc.RibbonStyleButton();
			this.gridCategories = new System.Windows.Forms.DataGridView();
			this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnNone = new RegScoreCalc.RibbonStyleButton();
			this.btnSelectFont = new RegScoreCalc.RibbonStyleButton();
			this.txtFilter = new RegScoreCalc.Code.Controls.HintTextBox();
			((System.ComponentModel.ISupportInitialize)(this.gridCategories)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Location = new System.Drawing.Point(-16, 276);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(418, 2);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(299, 289);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			this.btnCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_KeyDown);
			this.btnCancel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_KeyUp);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.HoverImage = null;
			this.btnOK.Location = new System.Drawing.Point(218, 289);
			this.btnOK.Name = "btnOK";
			this.btnOK.NormalImage = null;
			this.btnOK.PressedImage = null;
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.btnOK.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_KeyDown);
			this.btnOK.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_KeyUp);
			// 
			// btnAdd
			// 
			this.btnAdd.HoverImage = null;
			this.btnAdd.Location = new System.Drawing.Point(12, 12);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.NormalImage = null;
			this.btnAdd.PressedImage = null;
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 0;
			this.btnAdd.Text = "Add...";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			this.btnAdd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_KeyDown);
			this.btnAdd.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_KeyUp);
			// 
			// btnRemove
			// 
			this.btnRemove.HoverImage = null;
			this.btnRemove.Location = new System.Drawing.Point(93, 12);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.NormalImage = null;
			this.btnRemove.PressedImage = null;
			this.btnRemove.Size = new System.Drawing.Size(75, 23);
			this.btnRemove.TabIndex = 1;
			this.btnRemove.Text = "Remove";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			this.btnRemove.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_KeyDown);
			this.btnRemove.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_KeyUp);
			// 
			// gridCategories
			// 
			this.gridCategories.AllowUserToAddRows = false;
			this.gridCategories.AllowUserToDeleteRows = false;
			this.gridCategories.AllowUserToOrderColumns = true;
			this.gridCategories.AllowUserToResizeRows = false;
			this.gridCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridCategories.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.gridCategories.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridCategories.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.gridCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colName,
            this.colColor});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.gridCategories.DefaultCellStyle = dataGridViewCellStyle2;
			this.gridCategories.Location = new System.Drawing.Point(12, 45);
			this.gridCategories.MultiSelect = false;
			this.gridCategories.Name = "gridCategories";
			this.gridCategories.ReadOnly = true;
			this.gridCategories.RowHeadersVisible = false;
			this.gridCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridCategories.Size = new System.Drawing.Size(362, 219);
			this.gridCategories.TabIndex = 4;
			this.gridCategories.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCategories_CellClick);
			this.gridCategories.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCategories_CellDoubleClick);
			this.gridCategories.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridCategories_CellFormatting);
			this.gridCategories.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gridCategories_CellPainting);
			this.gridCategories.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridCategories_KeyDown);
			this.gridCategories.KeyUp += new System.Windows.Forms.KeyEventHandler(this.gridCategories_KeyUp);
			// 
			// colID
			// 
			this.colID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colID.DataPropertyName = "ID";
			this.colID.HeaderText = "ID";
			this.colID.Name = "colID";
			this.colID.ReadOnly = true;
			this.colID.Width = 65;
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colName.DataPropertyName = "Category";
			this.colName.HeaderText = "Name";
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colColor
			// 
			this.colColor.HeaderText = "Color";
			this.colColor.Name = "colColor";
			this.colColor.ReadOnly = true;
			this.colColor.Width = 60;
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
			// btnNone
			// 
			this.btnNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnNone.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this.btnNone.HoverImage = null;
			this.btnNone.Location = new System.Drawing.Point(12, 289);
			this.btnNone.Name = "btnNone";
			this.btnNone.NormalImage = null;
			this.btnNone.PressedImage = null;
			this.btnNone.Size = new System.Drawing.Size(75, 23);
			this.btnNone.TabIndex = 5;
			this.btnNone.Text = "None";
			this.btnNone.UseVisualStyleBackColor = true;
			this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
			this.btnNone.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_KeyDown);
			this.btnNone.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_KeyUp);
			// 
			// btnSelectFont
			// 
			this.btnSelectFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectFont.HoverImage = null;
			this.btnSelectFont.Location = new System.Drawing.Point(299, 12);
			this.btnSelectFont.Name = "btnSelectFont";
			this.btnSelectFont.NormalImage = null;
			this.btnSelectFont.PressedImage = null;
			this.btnSelectFont.Size = new System.Drawing.Size(75, 23);
			this.btnSelectFont.TabIndex = 3;
			this.btnSelectFont.Text = "Set Font...";
			this.btnSelectFont.UseVisualStyleBackColor = true;
			this.btnSelectFont.Click += new System.EventHandler(this.btnSelectFont_Click);
			this.btnSelectFont.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btn_KeyDown);
			this.btnSelectFont.KeyUp += new System.Windows.Forms.KeyEventHandler(this.btn_KeyUp);
			// 
			// txtFilter
			// 
			this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilter.Location = new System.Drawing.Point(174, 14);
			this.txtFilter.Name = "txtFilter";
			this.txtFilter.Size = new System.Drawing.Size(119, 20);
			this.txtFilter.TabIndex = 2;
			this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
			// 
			// FormSelectCategory
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(386, 324);
			this.Controls.Add(this.txtFilter);
			this.Controls.Add(this.btnSelectFont);
			this.Controls.Add(this.btnNone);
			this.Controls.Add(this.gridCategories);
			this.Controls.Add(this.btnRemove);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(325, 263);
			this.Name = "FormSelectCategory";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Select Сategory";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSelectCategory_FormClosing);
			this.Load += new System.EventHandler(this.FormSelectCategory_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridCategories)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private RibbonStyleButton btnCancel;
		private RibbonStyleButton btnOK;
		private RibbonStyleButton btnAdd;
		private RibbonStyleButton btnRemove;
		private System.Windows.Forms.DataGridView gridCategories;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private RibbonStyleButton btnNone;
		private RibbonStyleButton btnSelectFont;
		private System.Windows.Forms.DataGridViewTextBoxColumn colID;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colColor;
		private RegScoreCalc.Code.Controls.HintTextBox txtFilter;
	}
}