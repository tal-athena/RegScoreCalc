using System;
using System.Windows.Forms;

using RegScoreCalc.Data;

namespace RegScoreCalc.Forms
{
	public partial class DialogGroup : Form
	{
		#region Fields

		protected ViewsManager _views;
		protected int _lastSelectedGroupID = -1;

		#endregion

		#region Ctors

		public DialogGroup(ViewsManager views)
		{
			InitializeComponent();

			_views = views;

			this.BackColor = MainForm.ColorBackground;

			listGroups.SelectedIndexChanged += listGroups_SelectedIndexChanged;
		}

		#endregion

		#region Events

		private void DialogGroup_Load(object sender, EventArgs e)
		{
			try
			{
				FillGroupsList();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnGroupAdd_Click(object sender, EventArgs e)
		{
			try
			{
				//Popup dialog for new Group
				AddNewGroup newGroupDialog = new AddNewGroup("");
				var dialogResult = newGroupDialog.ShowDialog();
				if (dialogResult == System.Windows.Forms.DialogResult.OK)
				{
					//Change that name
					var newName = newGroupDialog.GroupName;
					if (!String.IsNullOrEmpty(newName))
					{
						var newGroup = _views.MainForm.datasetBilling.ICD9Groups.NewICD9GroupsRow();
						newGroup.Name = newName;

						_views.MainForm.datasetBilling.ICD9Groups.Rows.Add(newGroup);

						_views.MainForm.adapterICD9GroupsBilling.Update(_views.MainForm.datasetBilling.ICD9Groups);
						_views.MainForm.adapterICD9GroupsBilling.Fill(_views.MainForm.datasetBilling.ICD9Groups);

						FillGroupsList();
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnGroupEdit_Click(object sender, EventArgs e)
		{
			try
			{
				//Popup dialog to edit the Name
				//Open edit dialog
				if (listGroups.SelectedItems.Count > 0)
				{
					var name = listGroups.SelectedItems[0].Text;
					AddNewGroup newGroupDialog = new AddNewGroup(name, "Edit");
					var dialogResult = newGroupDialog.ShowDialog();
					if (dialogResult == System.Windows.Forms.DialogResult.OK)
					{
						//Change that name
						var newName = newGroupDialog.GroupName;
						if (!String.IsNullOrEmpty(newName))
						{
							var newFilter = _views.MainForm.datasetBilling.ICD9Groups.FindByGroupID((int)listGroups.SelectedItems[0].Tag);
							newFilter.Name = newName;

							_views.MainForm.adapterICD9GroupsBilling.Update(_views.MainForm.datasetBilling.ICD9Groups);
							_views.MainForm.adapterICD9GroupsBilling.Fill(_views.MainForm.datasetBilling.ICD9Groups);

							FillGroupsList();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnGroupDelete_Click(object sender, EventArgs e)
		{
			try
			{
				//Delete selected
				if (listGroups.SelectedItems.Count > 0)
				{
					_views.MainForm.datasetBilling.ICD9Groups.FindByGroupID((int)listGroups.SelectedItems[0].Tag).Delete();

					_views.MainForm.adapterICD9GroupsBilling.Update(_views.MainForm.datasetBilling.ICD9Groups);
					_views.MainForm.adapterICD9GroupsBilling.Fill(_views.MainForm.datasetBilling.ICD9Groups);

					FillGroupsList();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCodesAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if (_lastSelectedGroupID > 0)
				{
					//Popup dialog for new Group
					AddNewICD9Item newGroupDialog = new AddNewICD9Item("", "", false, false, false);
					var dialogResult = newGroupDialog.ShowDialog();
					if (dialogResult == System.Windows.Forms.DialogResult.OK)
					{
						//Change that name
						var icd = newGroupDialog.icd;
						var description = newGroupDialog.description;
						var regExp = newGroupDialog.regExp;
						var oneWay = newGroupDialog.oneWay;
						var combination = newGroupDialog.combination;

						if (!String.IsNullOrEmpty(icd))
						{
							var newCode = _views.MainForm.datasetBilling.ICDCodes.NewICDCodesRow();
							newCode.ICD9Code = icd;
							newCode.Description = description;
							newCode.RegExp = regExp;
							newCode.GroupID = _lastSelectedGroupID;
							newCode.OneWay = oneWay;
							newCode.Combination = combination;

							_views.MainForm.datasetBilling.ICDCodes.Rows.Add(newCode);

							_views.MainForm.adapterICDCodesBilling.Update(_views.MainForm.datasetBilling.ICDCodes);

							var id = MainForm.GetLastInsertedID(_views.MainForm.adapterICDCodesBilling.Connection);

							_views.MainForm.adapterICDCodesBilling.Fill(_views.MainForm.datasetBilling.ICDCodes);

							FillCodesList(id);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCodesEdit_Click(object sender, EventArgs e)
		{
			try
			{
				if (listCodes.SelectedItems.Count > 0)
				{
					var selectedRow = _views.MainForm.datasetBilling.ICDCodes.FindByID((int)listCodes.SelectedItems[0].Tag);

					AddNewICD9Item newGroupDialog = new AddNewICD9Item(selectedRow.ICD9Code, selectedRow.Description, selectedRow.RegExp, selectedRow.OneWay, selectedRow.Combination, "Edit");
					var dialogResult = newGroupDialog.ShowDialog();
					if (dialogResult == System.Windows.Forms.DialogResult.OK)
					{
						//Change that name
						var newIcd = newGroupDialog.icd;
						var newDescription = newGroupDialog.description;
						var regExp = newGroupDialog.regExp;
						var oneWay = newGroupDialog.oneWay;
						var combination = newGroupDialog.combination;

						if (!String.IsNullOrEmpty(newIcd) && !String.IsNullOrEmpty(newDescription))
						{
							//Change value in listView
							listCodes.SelectedItems[0].SubItems[0].Text = newIcd;
							listCodes.SelectedItems[0].SubItems[1].Text = newDescription;
							var regExpString = regExp ? "Yes" : "";
							listCodes.SelectedItems[0].SubItems[2].Text = regExpString;
							var oneWayString = oneWay ? "Yes" : "";
							listCodes.SelectedItems[0].SubItems[3].Text = oneWayString;

							var id = selectedRow.ID;

							selectedRow.ICD9Code = newIcd;
							selectedRow.Description = newDescription;
							selectedRow.RegExp = regExp;
							selectedRow.OneWay = oneWay;
							selectedRow.Combination = combination;

							_views.MainForm.adapterICDCodesBilling.Update(_views.MainForm.datasetBilling.ICDCodes);
							_views.MainForm.adapterICDCodesBilling.Fill(_views.MainForm.datasetBilling.ICDCodes);

							FillCodesList(id);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCodesDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (listCodes.SelectedItems.Count > 0)
				{
					_views.MainForm.datasetBilling.ICDCodes.FindByID((int)listCodes.SelectedItems[0].Tag).Delete();

					_views.MainForm.adapterICDCodesBilling.Update(_views.MainForm.datasetBilling.ICDCodes);
					_views.MainForm.adapterICDCodesBilling.Fill(_views.MainForm.datasetBilling.ICDCodes);

					FillCodesList();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
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

		private void listGroups_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				//Load ICD codes for selected group
				if (listGroups.SelectedItems.Count > 0)
				{
					_lastSelectedGroupID = (int)listGroups.SelectedItems[0].Tag;

					//Load all Codes
					FillCodesList(_lastSelectedGroupID);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void listGroups_ItemActivate(object sender, EventArgs e)
		{
			try
			{
				btnGroupEdit_Click(listGroups, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void listCodes_ItemActivate(object sender, EventArgs e)
		{
			try
			{
				btnCodesEdit_Click(listCodes, EventArgs.Empty);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public void UpdateGroupList(int groupID, string ICD, string Diagnosis, bool regExp, int ID)
		{
			if (groupID == _lastSelectedGroupID)
			{
				//Add new item to list
				var regExpString = regExp ? "Yes" : "";
				ListViewItem listItem = new ListViewItem(new[] { ICD, Diagnosis, regExpString });
				listItem.Tag = ID;
				listCodes.Items.Add(listItem);
			}
		}

		#endregion

		#region Implementation

		protected void FillGroupsList()
		{
			listGroups.Items.Clear();

			foreach (var item in _views.MainForm.datasetBilling.ICD9Groups)
			{
				var listItem = new ListViewItem
				               {
					               Tag = item.GroupID,
					               Text = item.Name
				               };

				listGroups.Items.Add(listItem);
			}

			///////////////////////////////////////////////////////////////////////////////

			if (listGroups.Items.Count > 0)
				listGroups.Items[0].Selected = true;
		}

		protected void FillCodesList(int selectedRowID = -1)
		{
			listCodes.Items.Clear();

			var i = 0;

			var codes = _views.MainForm.datasetBilling.ICDCodes.Select("GroupID = " + _lastSelectedGroupID);
			foreach (var dataRow in codes)
			{
				var item = (BillingDataSet.ICDCodesRow) dataRow;
				var val = item.RegExp ? "Yes" : "";
				var oneWayString = item.OneWay ? "Yes" : "";
				var listItem = new ListViewItem(new[] { item.ICD9Code, item.Description, val, oneWayString })
				               {
					               Tag = item.ID
				               };

				listCodes.Items.Add(listItem);

				if (item.ID == selectedRowID)
					listItem.Selected = true;

				///////////////////////////////////////////////////////////////////////////////

				i++;
			}
		}

		#endregion
	}
}