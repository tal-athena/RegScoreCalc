using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Helpers;
using RegScoreCalc.Code;
using RegScoreCalc.Forms;

namespace RegScoreCalc
{
	public partial class FormExternalSort : Form
	{
		#region Fields

		protected ViewsManager _views;

		#endregion

		#region Properties

		#endregion

		#region Ctors

		public FormExternalSort(ViewsManager views, ExternalSortOptions sortOptions)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			_views = views;

			InitSortOptionControls();
			InitSortOptionValues(sortOptions);

			UpdateEnabledState();
		}

		#endregion

		#region Events

		private void FormExternalSort_Load(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormExternalSort_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
					GetSortOptions(true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
				e.Cancel = true;
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
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

		private void sortOptions_ColumnSelected(object sender, EventArgs eventArgs)
		{
			try
			{
				var senderCtrl = (ExternalSortOptionsCtrl)sender;
				var column = senderCtrl.SortByColumn;

				if (column != null)
				{
					foreach (var ctrl in this.Controls.OfType<ExternalSortOptionsCtrl>().Where(x => x != senderCtrl))
					{
						ctrl.DeselectColumn(column.ColumnName);
					}
				}

				if (chkbSortGroupsOnClick.Checked)
				{
					if (column == null && !senderCtrl.GroupSimilar)
						chkbSortGroupsOnClick.Checked = false;
				}

				UpdateEnabledState();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void sortOptions_GroupSimilarClicked(object sender, EventArgs eventArgs)
		{
			try
			{
				var senderCtrl = (ExternalSortOptionsCtrl)sender;

				foreach (var ctrl in this.Controls.OfType<ExternalSortOptionsCtrl>().Where(x => x != senderCtrl))
				{
					ctrl.SetGroupSimilar(false);
				}

				if (!senderCtrl.GroupSimilar)
					chkbSortGroupsOnClick.Checked = false;

				UpdateEnabledState();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void cmbLeaveOneDocumentColumn_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				FixFilterColumnName();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void chkbSortGroupsOnClick_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateEnabledState();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void chkbShowGroupMinDocuments_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateEnabledState();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void chkbLeaveOneDocument_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateEnabledState();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void chkbApplyEditsToGroup_CheckedChanged(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public ExternalSortOptions GetSortOptions(bool throwExceptionOnError)
		{
			var list = new List<SortByColumn>
			{
				sortOptions1.SortByColumn,
				sortOptions2.SortByColumn,
				sortOptions3.SortByColumn
			};

			list = list.Where(x => x != null).ToList();
			if (!list.Any())
			{
				if (throwExceptionOnError)
					throw new Exception("Select at least one column for sorting");
				else
					return null;
			}

			if (chkbLeaveOneDocument.Checked)
			{
				if (String.IsNullOrEmpty(cmbLeaveOneDocumentColumn.Text))
				{
					if (throwExceptionOnError)
						throw new Exception("Select a column for 'Leave one document for each group with' option");
					else
						return null;
				}

				if (cmbLeaveOneDocumentColumn.Text == list.FirstOrDefault(x => x.GroupSimilar)?.ColumnName)
				{
					if (throwExceptionOnError)
						throw new Exception("Select a different column for 'Leave one document for each group with' option or change grouping column");
					else
						return null;
				}
			}

			return new ExternalSortOptions
			{
				SortByColumns = list,
				SortGroupsByCriteria = GetSortGroupsByCriteria(),
				ShowGroupsMinDocuments = chkbShowGroupMinDocuments.Enabled && chkbShowGroupMinDocuments.Checked && txtMinDocuments.Value > 0,
				MinDocuments = Convert.ToInt32(txtMinDocuments.Value),
				ApplyEditsToGroup = chkbApplyEditsToGroup.Enabled && chkbApplyEditsToGroup.Checked,
				LeaveOneDocument = chkbLeaveOneDocument.Checked,
				LeaveOneDocumentCriteria = (LeaveOneDocumentCriteria)cmbLeaveOneDocumentCriteria.SelectedIndex,
				LeaveOneDocumentColumn = cmbLeaveOneDocumentColumn.Text
			};
		}

		#endregion

		#region Implementation

		protected List<string> GetColumnsList()
		{
			var join = from actualCol in _views.MainForm.adapterDocuments.GetActualColumnsList()
					   join columnOptions in _views.MainForm.datasetMain.Columns.Cast<MainDataSet.ColumnsRow>()
						   on actualCol.Name equals columnOptions.Name
						   into tmp
					   from t in tmp.DefaultIfEmpty()
					   where t == null || t.IsVisible
					   orderby actualCol.Name
					   select actualCol.Name;

			return join.ToList();
		}

		protected void InitSortOptionControls()
		{
			sortOptions1.ColumnSelected += sortOptions_ColumnSelected;
			sortOptions2.ColumnSelected += sortOptions_ColumnSelected;
			sortOptions3.ColumnSelected += sortOptions_ColumnSelected;

			sortOptions1.GroupSimilarClicked += sortOptions_GroupSimilarClicked;
			sortOptions2.GroupSimilarClicked += sortOptions_GroupSimilarClicked;
			sortOptions3.GroupSimilarClicked += sortOptions_GroupSimilarClicked;

			var columnsList = GetColumnsList();

			sortOptions1.SetColumnsList(columnsList);
			sortOptions2.SetColumnsList(columnsList);
			sortOptions3.SetColumnsList(columnsList);

			cmbLeaveOneDocumentColumn.Items.Clear();
			cmbLeaveOneDocumentColumn.Items.AddRange(columnsList.Select(x => (object)x).ToArray());
			cmbLeaveOneDocumentColumn.SelectedIndex = 0;
		}

		protected void InitSortOptionValues(ExternalSortOptions sortOptions)
		{
			if (sortOptions == null)
				return;

			chkbApplyEditsToGroup.Checked = sortOptions.ApplyEditsToGroup;

			chkbLeaveOneDocument.Checked = sortOptions.LeaveOneDocument;
			cmbLeaveOneDocumentCriteria.SelectedIndex = (int)sortOptions.LeaveOneDocumentCriteria;

			var columnIndex = !String.IsNullOrEmpty(sortOptions.LeaveOneDocumentColumn) ? cmbLeaveOneDocumentColumn.Items.IndexOf(sortOptions.LeaveOneDocumentColumn) : -1;
			if (columnIndex != -1)
				cmbLeaveOneDocumentColumn.SelectedIndex = columnIndex;

			///////////////////////////////////////////////////////////////////////////////

			chkbSortGroupsOnClick.Checked = sortOptions.SortGroupsByCriteria != SortGroupsBy.None;
			rdbMin.Checked = sortOptions.SortGroupsByCriteria == SortGroupsBy.Min;
			rdbMax.Checked = sortOptions.SortGroupsByCriteria == SortGroupsBy.Max;
			rdbSum.Checked = sortOptions.SortGroupsByCriteria == SortGroupsBy.Sum || sortOptions.SortGroupsByCriteria == SortGroupsBy.None;

			///////////////////////////////////////////////////////////////////////////////

			chkbShowGroupMinDocuments.Checked = sortOptions.ShowGroupsMinDocuments;
			txtMinDocuments.Value = sortOptions.MinDocuments;

			///////////////////////////////////////////////////////////////////////////////

			var controls = this.Controls.OfType<ExternalSortOptionsCtrl>().OrderBy(x => x.TabIndex).ToList();

			for (var index = 0; index < sortOptions.SortByColumns.Count; index++)
			{
				if (index >= controls.Count)
					break;

				var opt = sortOptions.SortByColumns[index];
				var ctrl = controls[index];
				ctrl.SortByColumn = opt;
			}
		}

		protected void UpdateEnabledState()
		{
			FixFilterColumnName();

			var sortOptions = GetSortOptions(false);
			var isGroupingEnabled = sortOptions != null && sortOptions.SortByColumns.Any(x => x.GroupSimilar);

			chkbLeaveOneDocument.Enabled = isGroupingEnabled;
			var isLeaveOneDocument = isGroupingEnabled && chkbLeaveOneDocument.Checked;

			cmbLeaveOneDocumentCriteria.Enabled = isLeaveOneDocument;
			cmbLeaveOneDocumentColumn.Enabled = isLeaveOneDocument;

			if (isLeaveOneDocument)
				isGroupingEnabled = false;

			///////////////////////////////////////////////////////////////////////////////

			chkbSortGroupsOnClick.Enabled = isGroupingEnabled;

			var isEnabled = isGroupingEnabled && chkbSortGroupsOnClick.Checked;

			rdbMin.Enabled = isEnabled;
			rdbMax.Enabled = isEnabled;
			rdbSum.Enabled = isEnabled;

			chkbShowGroupMinDocuments.Enabled = isGroupingEnabled;

			txtMinDocuments.Enabled = isGroupingEnabled && chkbShowGroupMinDocuments.Checked;

			chkbApplyEditsToGroup.Enabled = isGroupingEnabled;
		}

		protected SortGroupsBy GetSortGroupsByCriteria()
		{
			if (chkbSortGroupsOnClick.Enabled && chkbSortGroupsOnClick.Checked)
			{
				if (rdbMin.Checked)
					return SortGroupsBy.Min;

				if (rdbMax.Checked)
					return SortGroupsBy.Max;

				if (rdbSum.Checked)
					return SortGroupsBy.Sum;
			}

			return SortGroupsBy.None;
		}

		protected void FixFilterColumnName()
		{
			if (this.Controls.OfType<ExternalSortOptionsCtrl>().Any(x => !String.IsNullOrEmpty(x.SelectedColumnName) && x.GroupSimilar && x.SelectedColumnName == cmbLeaveOneDocumentColumn.Text))
			{
				var index = cmbLeaveOneDocumentColumn.Items.IndexOf(cmbLeaveOneDocumentColumn.Text);
				if (index > 0)
					index = 0;
				else if (cmbLeaveOneDocumentColumn.Items.Count > 0)
					index = 1;

				cmbLeaveOneDocumentColumn.SelectedIndex = index;
			}
		}

		#endregion
	}
}