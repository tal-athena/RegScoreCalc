using System;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class FormAssignRegExpToGroup : Form
	{
		#region Fields

		protected int _regExpID;
		protected ViewsManager _views;

		#endregion

		#region Ctors

		public FormAssignRegExpToGroup(ViewsManager views, int regExpID)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			listGroups.HideSelection = false;

			///////////////////////////////////////////////////////////////////////////////

			_views = views;
			_regExpID = regExpID;
		}

		#endregion

		#region Events

		private void AssignRegExpToGroupDialog_Load(object sender, EventArgs e)
		{
			try
			{
				FillGroups();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			try
			{
				var selectedGroups = listGroups.Items.Cast<ListViewItem>()
				                               .Select(x => x.Tag)
				                               .OfType<int>()
				                               .ToList();

				var formAddGroup = new FormAddGroup(_views, selectedGroups);
				if (formAddGroup.ShowDialog() == DialogResult.OK)
				{
					foreach (var groupID in formAddGroup.SelectedGroups)
					{
						var hasDuplicates = _views.MainForm.datasetBilling.RegexpToGroups.Any(x => !x.IsIDNull() && x.ID == _regExpID && !x.IsGroupIDNull() && x.GroupID == groupID);
						if (!hasDuplicates)
						{
							_views.MainForm.datasetBilling.RegexpToGroups.AddRegexpToGroupsRow(_regExpID, groupID);
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					_views.MainForm.adapterRegexpToGroupsBilling.Update(_views.MainForm.datasetBilling.RegexpToGroups);
					_views.MainForm.adapterRegexpToGroupsBilling.Fill(_views.MainForm.datasetBilling.RegexpToGroups);

					///////////////////////////////////////////////////////////////////////////////

					FillGroups();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			OleDbConnection connection = null;

			try
			{
				if (listGroups.SelectedItems.Count == 0)
				{
					MessageBox.Show(this, "Please select group first", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				var groupID = (int) listGroups.SelectedItems[0].Tag;

				///////////////////////////////////////////////////////////////////////////////

				var cmd = new OleDbCommand("DELETE FROM RegexpToGroups WHERE (RegExpID = @RegExpID AND GroupID = @GroupID)");
				cmd.Parameters.Add(new OleDbParameter("@RegExpID", _regExpID));
				cmd.Parameters.Add(new OleDbParameter("@GroupID", groupID));

				///////////////////////////////////////////////////////////////////////////////

				cmd.Connection = _views.MainForm.adapterRegexpToGroupsBilling.Connection;
				if (cmd.Connection.State != ConnectionState.Open)
				{
					connection = cmd.Connection;
					connection.Open();
				}

				///////////////////////////////////////////////////////////////////////////////

				var count = cmd.ExecuteNonQuery();
				if (count == 0)
					MessageBox.Show(this, "0 records affected", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);

				_views.MainForm.adapterRegexpToGroupsBilling.Fill(_views.MainForm.datasetBilling.RegexpToGroups);

				FillGroups();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
			finally
			{
				if (connection != null && connection.State == ConnectionState.Open)
					connection.Close();

			}
		}

		private void btnClose_Click(object sender, EventArgs e)
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

		#endregion

		#region Implementation

		protected void FillGroups()
		{
			listGroups.Items.Clear();

			//Load filters from database
			var regExpToGroups = _views.MainForm.datasetBilling.RegexpToGroups.Where(x => !x.IsIDNull() && x.ID /* RegExpID */ == _regExpID)
									   .ToList();

			foreach (var item in regExpToGroups)
			{
				var groupId = item.GroupID;

				var group = _views.MainForm.datasetBilling.ICD9Groups.FindByGroupID(groupId);
				if (group != null)
				{
					var listItem = new ListViewItem { Text = group.Name, Tag = group.GroupID };
					listGroups.Items.Add(listItem);
				}
			}

			if (listGroups.Items.Count > 0)
				listGroups.Items[0].Selected = true;
		}

		#endregion
	}
}