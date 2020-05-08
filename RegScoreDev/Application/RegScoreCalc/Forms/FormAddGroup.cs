using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class FormAddGroup : Form
	{
		#region Fields

		protected ViewsManager _views;
		protected readonly List<int> _selectedGroups;

		#endregion

		#region Properties

		public List<int> SelectedGroups
		{
			get { return Prop_GetSelectedGroups(); }
		}

		#endregion

		#region Ctors

		public FormAddGroup(ViewsManager views, List<int> selectedGroups)
		{
			InitializeComponent();

			lbGroups.CheckOnClick = true;

			this.BackColor = MainForm.ColorBackground;

			///////////////////////////////////////////////////////////////////////////////

			_views = views;
			_selectedGroups = selectedGroups;
		}

		#endregion

		#region Events

		private void FormAddGroup_Load(object sender, EventArgs e)
		{
			try
			{
				//Load all existing groups to the combobox
				var groups = _views.MainForm.datasetBilling.ICD9Groups;
				foreach (var item in groups)
				{
					if (!_selectedGroups.Contains(item.GroupID))
						lbGroups.Items.Add(new AddGroupComboboxItem { Text = item.Name, GroupID = item.GroupID });
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
				this.DialogResult = DialogResult.OK;
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
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCheckAll_Click(object sender, EventArgs e)
		{
			try
			{
				SetCheck(CheckState.Checked);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnUncheckAll_Click(object sender, EventArgs e)
		{
			try
			{
				SetCheck(CheckState.Unchecked);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnInvertCheck_Click(object sender, EventArgs e)
		{
			try
			{
				SetCheck(CheckState.Indeterminate);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void SetCheck(CheckState state)
		{
			for (var i = 0; i < lbGroups.Items.Count; i++)
			{
				if (state == CheckState.Indeterminate)
					lbGroups.SetItemCheckState(i, lbGroups.GetItemCheckState(i) == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
				else
					lbGroups.SetItemCheckState(i, state);
			}
		}

		protected List<int> Prop_GetSelectedGroups()
		{
			return lbGroups.CheckedItems.Cast<AddGroupComboboxItem>()
			               .Select(x => x.GroupID)
			               .ToList();
		}

		#endregion
	}

	public class AddGroupComboboxItem
	{
		#region Fields

		public string Text { get; set; }
		public int GroupID { get; set; }

		#endregion

		#region Overrides

		public override string ToString()
		{
			return Text;
		}

		#endregion
	}
}