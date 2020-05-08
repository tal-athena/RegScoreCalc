using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Newtonsoft.Json;

using RegExpLib.Core;

using RegScoreCalc.Code;

namespace RegScoreCalc.Forms
{
	public partial class FormSynergies : Form
	{
		#region	Constants

		protected const string _factorFormat = "F1";
		protected const double _defaultValue = 0.0d;
		protected const double _min = 0.1d;
		protected const double _max = 100.0d;
		protected const double _highlightThreshold = 1.0d;

		#endregion

		#region Fields

		private readonly ViewsManager _views;
		private readonly Pen _pen;
		private DataGridViewCell _lastSelectedCell;

		protected DefaultViewData _viewData;

		#endregion

		#region Ctors

		public FormSynergies(ViewsManager views)
		{
			_views = views;

			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			grid.EnableHeadersVisualStyles = false;
			grid.ShowEditingIcon = false;

			var type = grid.GetType();
			var pi = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
			pi.SetValue(grid, true, null);

			_pen = new Pen(grid.GridColor);
		}

		#endregion

		#region Events

		private void FormSynergies_Load(object sender, EventArgs e)
		{
			try
			{
				LoadData();

				FillGrid();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormSynergies_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
					SaveData();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				timer.Stop();
				grid.CurrentCell = _lastSelectedCell;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnClearAll_Click(object sender, EventArgs e)
		{
			try
			{
				_viewData.RegExpSynergies.Clear();

				FillGrid();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.Delete && e.Modifiers == Keys.None)
				{
					var cell = grid.CurrentCell;
					if (cell != null && !cell.ReadOnly)
						cell.Value = SetFactor(_views.MainForm.datasetMain.RegExp[cell.RowIndex].ID, _views.MainForm.datasetMain.RegExp[cell.ColumnIndex].ID, _defaultValue.ToString(_factorFormat));
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					var cell = grid[e.ColumnIndex, e.RowIndex];
					if (!cell.ReadOnly)
					{
						grid.CurrentCell = cell;
						grid.BeginEdit(true);
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					var cell = grid[e.ColumnIndex, e.RowIndex];
					if (!cell.ReadOnly)
					{
						var bkColor = Color.White;
						var fgColor = Color.Black;

						double factor;
						if (TryParseDouble(cell.Value as string, out factor))
						{
							if (!factor.Equals(_defaultValue))
							{
								if (factor < _highlightThreshold)
									bkColor = Color.OrangeRed;
								else if (factor > _highlightThreshold)
									bkColor = Color.LightGreen;
							}
							else
								fgColor = Color.White;
						}

						cell.Style.ForeColor = fgColor;
						cell.Style.BackColor = bkColor;
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					var cell = grid[e.ColumnIndex, e.RowIndex];
					if (cell.ReadOnly)
					{
						e.Graphics.FillRectangle(Brushes.White, e.CellBounds);

						if (e.RowIndex < grid.Rows.Count - 1)
						{
							var cellBelow = grid[e.ColumnIndex, e.RowIndex + 1];
							if (!cellBelow.ReadOnly)
								e.Graphics.DrawLine(_pen, e.CellBounds.X, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
						}

						e.Handled = true;
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_CurrentCellChanged(object sender, EventArgs e)
		{
			try
			{
				if (grid.CurrentCell != _lastSelectedCell)
				{
					if (grid.CurrentCell != null)
					{
						if (grid.CurrentCell.ReadOnly)
						{
							grid.ClearSelection();
							if (!timer.Enabled)
								timer.Start();

							return;
						}
					}

					_lastSelectedCell = grid.CurrentCell;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			try
			{
				var textBox = e.Control as TextBox;
				if (textBox != null)
				{
					textBox.TextAlign = HorizontalAlignment.Right;

					e.CellStyle.ForeColor = Color.Black;

					textBox.KeyPress -= cell_KeyPress;
					textBox.KeyPress += cell_KeyPress;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void cell_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				var textBox = (TextBox)sender;

				var separators = new char[] { ',', '.' };

				if (!Char.IsControl(e.KeyChar) && !(Char.IsDigit(e.KeyChar) | separators.Contains(e.KeyChar)))
					e.Handled = true;

				if (separators.Contains(e.KeyChar) && textBox.Text.IndexOfAny(separators) > -1)
					e.Handled = true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void grid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					var cell = grid[e.ColumnIndex, e.RowIndex];
					if (!cell.ReadOnly)
						cell.Value = SetFactor(_views.MainForm.datasetMain.RegExp[e.RowIndex].ID, _views.MainForm.datasetMain.RegExp[e.ColumnIndex].ID, cell.Value as string);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void LoadData()
		{
			var json = BrowserManager.GetViewData(_views, "Default");
			_viewData = DefaultViewData.Load(json);
		}

		protected void SaveData()
		{
			var json = JsonConvert.SerializeObject(_viewData);
			BrowserManager.SetViewData(_views, "Default", json);
		}

		protected double GetFactor(int rowRegExpID, int colRegExpID)
		{
			var regExpSynergy = _viewData.RegExpSynergies.FirstOrDefault(x => x.RegExpID == rowRegExpID);
			if (regExpSynergy == null)
				return _defaultValue;

			var synergy = regExpSynergy.Synergies.FirstOrDefault(x => x.MatchingRegExpId == colRegExpID);
			if (synergy == null)
				return _defaultValue;

			return synergy.Factor;
		}

		protected string SetFactor(int rowRegExpID, int colRegExpID, string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				double factor;
				if (TryParseDouble(value, out factor))
				{
					var isZero = factor.Equals(_defaultValue);
					if (!isZero)
					{
						if (factor < _min)
							factor = _min;

						if (factor > _max)
							factor = _max;
					}

					value = factor.ToString(_factorFormat);

					///////////////////////////////////////////////////////////////////////////////

					var exists = false;
					var regExpSynergy = _viewData.RegExpSynergies.FirstOrDefault(x => x.RegExpID == rowRegExpID);
					if (regExpSynergy == null)
					{
						if (isZero)
						{
							CleanupRegExpSynergies();
							return value;
						}

						regExpSynergy = new RegExpSynergy
						                {
							                RegExpID = rowRegExpID
						                };
					}
					else
						exists = true;

					///////////////////////////////////////////////////////////////////////////////

					var matchingSynergy = regExpSynergy.Synergies.FirstOrDefault(x => x.MatchingRegExpId == colRegExpID);
					if (matchingSynergy == null)
					{
						if (isZero)
						{
							CleanupRegExpSynergies();
							return value;
						}

						matchingSynergy = new Synergy
						                  {
							                  MatchingRegExpId = colRegExpID
						                  };

						regExpSynergy.Synergies.Add(matchingSynergy);
					}
					else
					{
						if (isZero)
						{
							regExpSynergy.Synergies.Remove(matchingSynergy);
							CleanupRegExpSynergies();
							return value;
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					matchingSynergy.Factor = factor;

					if (!exists)
						_viewData.RegExpSynergies.Add(regExpSynergy);
				}
			}
			else
				value = _defaultValue.ToString(_factorFormat);

			return value;
		}

		protected bool TryParseDouble(string value, out double doubleVal)
		{
			doubleVal = _defaultValue;

			if (String.IsNullOrEmpty(value))
				return false;

			value = value.Replace(",", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);
			value = value.Replace(".", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator);

			return Double.TryParse(value, out doubleVal);
		}

		protected void FillGrid()
		{
			var oldRowIndex = grid.CurrentCell?.RowIndex;
			var oldColumnIndex = grid.CurrentCell?.ColumnIndex;

			grid.Rows.Clear();
			grid.Columns.Clear();

			grid.RowHeadersWidth = TextRenderer.MeasureText(new String('w', 25), grid.Font).Width;

			///////////////////////////////////////////////////////////////////////////////

			var counter = 1;
			foreach (var regExp in _views.MainForm.datasetMain.RegExp)
			{
				var col = new DataGridViewTextBoxColumn
				{
					HeaderText = counter.ToString(),
					SortMode = DataGridViewColumnSortMode.NotSortable
				};

				col.HeaderCell.Style.BackColor = MainForm.ColorBackground;
				col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

				col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				col.MinimumWidth = 40;

				grid.Columns.Add(col);

				///////////////////////////////////////////////////////////////////////////////

				var index = grid.Rows.Add();
				var row = grid.Rows[index];
				row.HeaderCell.Value = counter + ". " + regExp.RegExp;
				row.HeaderCell.Style.BackColor = MainForm.ColorBackground;
				row.HeaderCell.Style.WrapMode = DataGridViewTriState.False;

				///////////////////////////////////////////////////////////////////////////////

				counter++;
			}

			///////////////////////////////////////////////////////////////////////////////

			for (var rowIndex = 0; rowIndex < grid.Rows.Count; rowIndex++)
			{
				var row = grid.Rows[rowIndex];
				for (var columnIndex = 0; columnIndex < row.Cells.Count; columnIndex++)
				{
					var cell = row.Cells[columnIndex];
					if (columnIndex >= rowIndex)
						cell.ReadOnly = true;
					else
						cell.Value = GetFactor(_views.MainForm.datasetMain.RegExp[rowIndex].ID, _views.MainForm.datasetMain.RegExp[columnIndex].ID).ToString(_factorFormat);
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			if (oldRowIndex >= 0 && oldRowIndex < grid.RowCount && oldColumnIndex >= 0 && oldColumnIndex < grid.ColumnCount)
				grid.CurrentCell = grid[oldColumnIndex.Value, oldRowIndex.Value];

			grid.Select();
		}

		protected void CleanupRegExpSynergies()
		{
			_viewData.RegExpSynergies.Where(x => !x.Synergies.Any())
			         .ToList()
			         .ForEach(x => _viewData.RegExpSynergies.Remove(x));
		}

		#endregion
	}
}