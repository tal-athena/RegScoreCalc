using System;
using System.Data;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class FormManualEditRegExp : Form
	{
		protected readonly ViewsManager _views;
		protected readonly DataRow _row;

		#region Ctors

		public FormManualEditRegExp(ViewsManager views, DataRow row)
		{
			InitializeComponent();

			_views = views;
			_row = row;
		}

		#endregion

		#region Events

		private void FormManualEditRegExp_Load(object sender, EventArgs e)
		{
			try
			{
				var table = _row.Table;

				for (var i = 0; i < _row.ItemArray.Length; i++)
				{
					var column = table.Columns[i];

					var name = column.ColumnName;
					var type = column.DataType.Name;
					var value = _row.ItemArray[i];

					gridRegExp.Rows.Add(name, type, value);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormManualEditRegExp_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					SaveRow();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);

				e.Cancel = true;
			}
		}

		#endregion

		#region Implementation

		protected void SaveRow()
		{
			for (var i = 0; i < gridRegExp.Rows.Count; i++)
			{
				var row = gridRegExp.Rows[i];

				var value = row.Cells[colValue.Index].Value;

				if (value == null)
					_row[i] = DBNull.Value;
				else
					_row[i] = value;
			}
		}

		#endregion
	}
}