using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RegScoreCalc.Code;

namespace RegScoreCalc.Forms
{
	public partial class ExternalSortOptionsCtrl : UserControl
	{
		#region Constants

		private const string LabelNone = "<none>";

		#endregion

		#region Fields

		protected bool _ignoreSelectedIndexChanged;
		protected bool _ignoreGroupSimilarChecked;

		#endregion

		#region Properties

		public bool GroupSimilar => chkbGroupSimilar.Checked;

		public SortByColumn SortByColumn
		{
			get
			{
				if (cmbColumn.SelectedIndex > 0)
				{
					return new SortByColumn
					{
						ColumnName = cmbColumn.Text,
						SortOrder = cmbOrder.SelectedIndex == 0 ? SortOrder.Ascending : SortOrder.Descending,
						GroupSimilar = chkbGroupSimilar.Checked,
						AlternateColor = chkbAlternateColor.Checked
					};
				}

				return null;
			}
			set
			{
				_ignoreSelectedIndexChanged = true;
				_ignoreGroupSimilarChecked = true;

				try
				{
					var opt = value;
					if (opt == null)
						return;

					var index = cmbColumn.FindString(opt.ColumnName);
					cmbColumn.SelectedIndex = index != -1 ? index : 0;

					if (opt.SortOrder == SortOrder.Ascending)
						cmbOrder.SelectedIndex = 0;
					else if (opt.SortOrder == SortOrder.Descending)
						cmbOrder.SelectedIndex = 1;

					chkbGroupSimilar.Checked = opt.GroupSimilar;
					chkbAlternateColor.Checked = opt.AlternateColor;
				}
				finally
				{
					_ignoreSelectedIndexChanged = false;
					_ignoreGroupSimilarChecked = false;

					UpdateEnabledState();
				}
			}
		}

		public string SelectedColumnName => cmbColumn.Text;

		public event EventHandler ColumnSelected;
		public event EventHandler GroupSimilarClicked;

		#endregion

		#region Ctors

		public ExternalSortOptionsCtrl()
		{
			InitializeComponent();

			UpdateEnabledState();
		}

		#endregion

		#region Events

		private void cmbColumn_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (_ignoreSelectedIndexChanged)
					return;

				UpdateEnabledState();

				this.ColumnSelected?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void chkbGroupSimilar_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (!_ignoreGroupSimilarChecked)
					this.GroupSimilarClicked?.Invoke(this, EventArgs.Empty);

				var isEnabled = chkbGroupSimilar.Checked;

				chkbAlternateColor.Enabled = isEnabled;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public void SetColumnsList(List<string> columnsList)
		{
			cmbColumn.Items.Clear();
			cmbColumn.Items.Add(LabelNone);
			cmbColumn.Items.AddRange(columnsList.Select(x => (object)x).ToArray());

			cmbColumn.SelectedIndex = 0;
		}

		public void SetGroupSimilar(bool value)
		{
			_ignoreGroupSimilarChecked = true;

			try
			{
				chkbGroupSimilar.Checked = value;
			}
			finally
			{
				_ignoreGroupSimilarChecked = false;
			}
		}

		public void DeselectColumn(string columnName)
		{
			if (columnName != LabelNone && cmbColumn.Text == columnName)
			{
				_ignoreSelectedIndexChanged = true;
				_ignoreGroupSimilarChecked = true;

				try
				{
					cmbColumn.SelectedIndex = 0;

					chkbGroupSimilar.Checked = false;

					UpdateEnabledState();
				}
				finally
				{
					_ignoreSelectedIndexChanged = false;
					_ignoreGroupSimilarChecked = false;
				}
			}
		}

		#endregion

		#region Implementation

		protected void UpdateEnabledState()
		{
			var isEnabled = cmbColumn.SelectedIndex > 0;

			cmbOrder.Enabled = isEnabled;
			chkbGroupSimilar.Enabled = isEnabled;
			chkbAlternateColor.Enabled = chkbGroupSimilar.Checked;

			if (isEnabled)
			{
				if (cmbOrder.SelectedIndex == -1)
					cmbOrder.SelectedIndex = 0;
			}
			else
				cmbOrder.SelectedIndex = -1;
		}

		#endregion
	}
}