using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class PaneCodes : Pane
	{
		#region Data members

		private DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn colorDataGridViewTextBoxColumn;

		#endregion

		#region Ctors

		public PaneCodes()
		{
			InitializeComponent();
		}

		#endregion

		#region Events

		protected void gridCodes_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == colorDataGridViewTextBoxColumn.Index && e.RowIndex >= 0) // index of column with color
			{
				DataGridViewCell c = gridCodes.Rows[e.RowIndex].Cells[e.ColumnIndex];
				ColorDialog cd = new ColorDialog();

				cd.Color = c.Style.BackColor;
				if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					c.Style.BackColor = cd.Color;
					c.Style.ForeColor = cd.Color;
					c.Style.SelectionBackColor = cd.Color;
					c.Style.SelectionForeColor = cd.Color;

					c.Value = cd.Color.ToArgb();

					RaiseDataModifiedEvent();
				}
			}
		}

		protected void gridCodes_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == colorDataGridViewTextBoxColumn.Index && e.RowIndex >= 0) // index of column with color
			{
				DataGridViewCell c = gridCodes.Rows[e.RowIndex].Cells[e.ColumnIndex];

				if (c.Value != null)
				{
					Color color = Color.White;
					int argbColor = 0;
					try
					{
						argbColor = (int)c.Value;
					}
					catch (Exception)
					{
						// do nothing e.g. DBNull
					}


					if (argbColor != 0)
					{
						color = Color.FromArgb(argbColor);
					}

					c.Style.BackColor = color;
					c.Style.ForeColor = color;
					c.Style.SelectionBackColor = color;
					c.Style.SelectionForeColor = color;
				}
			}
		}

		private void gridCodes_RowValidated(object sender, DataGridViewCellEventArgs e)
		{
			_views.MainForm.adapterColorCodes.Update(_views.MainForm.datasetMain.ColorCodes);
			_views.MainForm.adapterColorCodes.Fill(_views.MainForm.datasetMain.ColorCodes);
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			gridCodes.AutoGenerateColumns = false;
			gridCodes.DataSource = _views.MainForm.sourceColorCodes;

			InitializeGridColumns();
		}

		#endregion

		#region Implementation

		protected void InitializeGridColumns()
		{
			// 
			// descriptionDataGridViewTextBoxColumn
			// 
			this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
			this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
			this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
			// 
			// colorDataGridViewTextBoxColumn
			// 
			this.colorDataGridViewTextBoxColumn.DataPropertyName = "Color";
			this.colorDataGridViewTextBoxColumn.HeaderText = "Color";
			this.colorDataGridViewTextBoxColumn.Name = "colorDataGridViewTextBoxColumn";
			this.colorDataGridViewTextBoxColumn.ReadOnly = true;

			//////////////////////////////////////////////////////////////////////////

			this.gridCodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descriptionDataGridViewTextBoxColumn,
            this.colorDataGridViewTextBoxColumn});
		}

		#endregion
	}
}
