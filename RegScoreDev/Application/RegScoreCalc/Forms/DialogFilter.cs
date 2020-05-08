using RegScoreCalc.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Office.Interop.Excel;

using RegScoreCalc.Data;

namespace RegScoreCalc.Forms
{
	public partial class DialogFilter : Form
	{
		protected ViewsManager _views;
		private bool positionsChanged;
		private FormGenerateSVMProgress progress;
		private BackgroundWorker worker;
		private BackgroundWorker workerAll;
		private int lastFiltersIndex = 0;
		private Color defaultListBackgroundColor;
		private ViewBILLING_1 _billingView;

		string[] separator = new string[] { " + " };

		public DialogFilter(ViewsManager views, ViewBILLING_1 billingView)
		{
			InitializeComponent();

			_billingView = billingView;
			_views = views;

			this.BackColor = MainForm.ColorBackground;

			listFilters.SelectedIndexChanged += listFilters_SelectedIndexChanged;
			positionsChanged = false;
			defaultListBackgroundColor = listFilters.BackColor;
		}

		void listFilters_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listFilters.SelectedItems.Count > 0)
			{
				////Set color
				//var newIndex = listFilters.SelectedItems[0].Index;
				//if (lastFiltersIndex != newIndex)
				//{
				//    listFilters.Items[lastFiltersIndex].BackColor = defaultListBackgroundColor;
				//    listFilters.Items[newIndex].BackColor = Color.Blue;
				//    lastFiltersIndex = newIndex;
				//}

				lastFiltersIndex = listFilters.SelectedItems[0].Index;

				//Load groups
				FillGroupsList();

				FillExcludedCategoriesList();
			}
		}

		private void FillExcludedCategoriesList()
		{
			listExcludeCategories.Items.Clear();

			var filterID = (int) listFilters.SelectedItems[0].Tag;

			var categories = _views.MainForm.datasetBilling.CategoryToFilterExclusion.Select("FilterID = " + filterID);

			foreach (Data.BillingDataSet.CategoryToFilterExclusionRow item in categories)
			{
				var category = _views.MainForm.datasetBilling.Categories.FindByID(item.CategoryID);
				if (category != null)
				{
					var mainListItem = new ListViewItem(new[] { category.Category, item.Type });
					mainListItem.Tag = category.ID;
					listExcludeCategories.Items.Add(mainListItem);
				}
				else
				{
					//Delete from database
				}
			}
		}

		public void FillGroupsList()
		{
			if (listFilters.SelectedItems.Count == 1)
			{
				var selectedFilter = listFilters.SelectedItems[0];
				var filterRow = _views.MainForm.datasetBilling.ICDFilters.FindByFilterID((int) selectedFilter.Tag);

				listGroups.Items.Clear();

				var groupsArray = !filterRow.IsGroupIDsNull() ? filterRow.GroupIDs : String.Empty;

				if (groupsArray != "")
				{
					var groups = _views.MainForm.datasetBilling.ICD9Groups.Select("GroupID IN (" + groupsArray + ")");

					foreach (Data.BillingDataSet.ICD9GroupsRow group in groups)
					{
						var listItem = new ListViewItem();
						listItem.Text = group.Name;
						listItem.Tag = group.GroupID;
						listGroups.Items.Add(listItem);
					}
				}
			}
		}

		public void FillFiltersList()
		{
			listFilters.Items.Clear();

			//Load filters from database
			var filters = _views.MainForm.datasetBilling.ICDFilters.OrderBy(p => p.Position);
			foreach (var filter in filters)
			{
				//Add to list
				var id = filter.FilterID;
				var name = filter.Name;
				var position = filter.Position;

				var item = new ListViewItem();
				item.Tag = id;
				item.Text = name;
				if (listFilters.Items.Count > position)
					listFilters.Items.Insert(position, item);
				else
					listFilters.Items.Add(item);
			}
		}

		#region "Events"

		private void DialogFilter_Load(object sender, EventArgs e)
		{
			try
			{
				FillFiltersList();
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
				AddNewFilter newFilterDialog = new AddNewFilter("");
				var dialogResult = newFilterDialog.ShowDialog();
				if (dialogResult == System.Windows.Forms.DialogResult.OK)
				{
					//Change that name
					var newName = newFilterDialog.name;
					if (!String.IsNullOrEmpty(newName))
					{
						var newFilter = _views.MainForm.datasetBilling.ICDFilters.NewICDFiltersRow();
						newFilter.Name = newName;
						newFilter.GroupIDs = "";
						newFilter.Position = (short) listFilters.Items.Count;

						_views.MainForm.datasetBilling.ICDFilters.Rows.Add(newFilter);

						_views.MainForm.adapterICDFiltersBilling.Update(_views.MainForm.datasetBilling.ICDFilters);

						var id = MainForm.GetLastInsertedID(_views.MainForm.adapterICDFiltersBilling.Connection);

						_views.MainForm.adapterICDFiltersBilling.Fill(_views.MainForm.datasetBilling.ICDFilters);

						FillFiltersList();

						//Add to combobox
						var btn = new RibbonButton(newName);
						btn.Tag = id;
						btn.Click += _billingView.btn_FilterDropDownSelect;
						_billingView.filtersCombobox.DropDownItems.Add(btn);
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			try
			{
				//Open edit dialog
				if (listFilters.SelectedItems.Count > 0)
				{
					var name = listFilters.SelectedItems[0].Text;
					AddNewFilter newFilterDialog = new AddNewFilter(name, "Edit");
					var dialogResult = newFilterDialog.ShowDialog();
					if (dialogResult == System.Windows.Forms.DialogResult.OK)
					{
						//Change that name
						var newName = newFilterDialog.name;
						if (!String.IsNullOrEmpty(newName))
						{
							var newFilter = _views.MainForm.datasetBilling.ICDFilters.FindByFilterID((int) listFilters.SelectedItems[0].Tag);
							newFilter.Name = newName;

							_views.MainForm.adapterICDFiltersBilling.Update(_views.MainForm.datasetBilling.ICDFilters);
							_views.MainForm.adapterICDFiltersBilling.Fill(_views.MainForm.datasetBilling.ICDFilters);

							FillFiltersList();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (listFilters.SelectedItems.Count > 0)
				{
					var filterID = (int) listFilters.SelectedItems[0].Tag;
					_views.MainForm.datasetBilling.ICDFilters.FindByFilterID(filterID)
					      .Delete();

					_views.MainForm.adapterICDFiltersBilling.Update(_views.MainForm.datasetBilling.ICDFilters);
					_views.MainForm.adapterICDFiltersBilling.Fill(_views.MainForm.datasetBilling.ICDFilters);

					FillFiltersList();

					_billingView.filtersCombobox.DropDownItems.RemoveAll(p => p.Tag == (object) filterID);

					//Remove from database the data from FilterToDocs
					CalculateFilter calcualteFilter = new CalculateFilter(_views);
					calcualteFilter.DeleteFromDocsToFilters(filterID);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnMoveUp_Click(object sender, EventArgs e)
		{
			if (listFilters.SelectedItems.Count > 0)
			{
				var selectedItem = listFilters.SelectedItems[0];
				if (selectedItem.Index > 0)
				{
					int newPosition = listFilters.SelectedItems[0].Index - 1;
					listFilters.Items.RemoveAt(selectedItem.Index);
					listFilters.Items.Insert(newPosition, selectedItem);

					positionsChanged = true;
				}
			}
		}

		private void btnMoveDown_Click(object sender, EventArgs e)
		{
			if (listFilters.SelectedItems.Count > 0)
			{
				var selectedItem = listFilters.SelectedItems[0];
				if (selectedItem.Index < listFilters.Items.Count - 1)
				{
					int newPosition = listFilters.SelectedItems[0].Index + 1;
					listFilters.Items.RemoveAt(selectedItem.Index);
					listFilters.Items.Insert(newPosition, selectedItem);

					positionsChanged = true;
				}
			}
		}

		#endregion

		#region "Groups"

		private void btnGroupAdd_Click(object sender, EventArgs e)
		{
			try
			{
				if (listFilters.SelectedItems.Count > 0)
				{
					var selectedGroups = listGroups.Items.Cast<ListViewItem>()
					                               .Select(x => x.Tag)
					                               .OfType<int>()
					                               .ToList();

					var formAddGroup = new FormAddGroup(_views, selectedGroups);
					if (formAddGroup.ShowDialog() == DialogResult.OK)
					{
						var selectedFilterID = (int) listFilters.SelectedItems[0].Tag;

						///////////////////////////////////////////////////////////////////////////////

						var filterRow = _views.MainForm.datasetBilling.ICDFilters.FirstOrDefault(x => x.FilterID == selectedFilterID);
						if (filterRow == null)
							filterRow = _views.MainForm.datasetBilling.ICDFilters.NewICDFiltersRow();

						///////////////////////////////////////////////////////////////////////////////

						List<int> groupIDs;

						if (!filterRow.IsGroupIDsNull())
						{
							groupIDs = filterRow.GroupIDs.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
							                    .Select(x => Convert.ToInt32(x))
							                    .ToList();
						}
						else
							groupIDs = new List<int>();

						///////////////////////////////////////////////////////////////////////////////

						foreach (var groupID in formAddGroup.SelectedGroups)
						{
							if (groupIDs.Contains(groupID))
								continue;

							groupIDs.Add(groupID);

							///////////////////////////////////////////////////////////////////////////////

							var listItem = new ListViewItem
							               {
								               Text = _views.MainForm.datasetBilling.ICD9Groups.Single(x => x.GroupID == groupID)
								                            .Name,
								               Tag = groupID
							               };

							listGroups.Items.Add(listItem);
						}

						///////////////////////////////////////////////////////////////////////////////

						filterRow.GroupIDs = String.Join(",", groupIDs);

						if (filterRow.RowState == DataRowState.Detached)
							_views.MainForm.datasetBilling.ICDFilters.AddICDFiltersRow(filterRow);

						///////////////////////////////////////////////////////////////////////////////

						_views.MainForm.adapterRegexpToGroupsBilling.Update(_views.MainForm.datasetBilling.RegexpToGroups);
						_views.MainForm.adapterRegexpToGroupsBilling.Fill(_views.MainForm.datasetBilling.RegexpToGroups);

						_views.MainForm.adapterICDFiltersBilling.Update(_views.MainForm.datasetBilling.ICDFilters);
						_views.MainForm.adapterICDFiltersBilling.Fill(_views.MainForm.datasetBilling.ICDFilters);

						FillGroupsList();
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnGroupRemove_Click(object sender, EventArgs e)
		{
			try
			{
				//Add value
				if (listGroups.SelectedItems.Count > 0)
				{
					var selectedID = (int) listGroups.SelectedItems[0].Tag;
					listGroups.Items.Remove(listGroups.SelectedItems[0]);

					string groups = "";
					foreach (ListViewItem item in listGroups.Items)
					{
						groups += item.Tag.ToString() + ",";
					}
					//Remove last ,
					if (groups != "")
						groups = groups.Substring(0, groups.Length - 1);

					//Save to database
					int filterId = (int) listFilters.SelectedItems[0].Tag;
					_views.MainForm.datasetBilling.ICDFilters.FindByFilterID(filterId)
					      .GroupIDs = groups;

					_views.MainForm.adapterICDFiltersBilling.Update(_views.MainForm.datasetBilling.ICDFilters);
					_views.MainForm.adapterICDFiltersBilling.Fill(_views.MainForm.datasetBilling.ICDFilters);

					FillGroupsList();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region "Categories"

		private void btnAddExcludeCategory_Click(object sender, EventArgs e)
		{
			try
			{
				if (listFilters.SelectedItems.Count > 0)
				{
					AddCategoryToFilterExclude groupDialog = new AddCategoryToFilterExclude(_views);
					DialogResult result = groupDialog.ShowDialog();
					if (result == System.Windows.Forms.DialogResult.OK)
					{
						var filterId = (int) listFilters.SelectedItems[0].Tag;
						var exclusionType = groupDialog.Type;

						var newRow = _views.MainForm.datasetBilling.CategoryToFilterExclusion.NewCategoryToFilterExclusionRow();
						newRow.CategoryID = groupDialog.SelectedCategoryID;
						newRow.FilterID = filterId;
						newRow.Type = exclusionType;

						_views.MainForm.datasetBilling.CategoryToFilterExclusion.Rows.Add(newRow);
						_views.MainForm.datasetBilling.CategoryToFilterExclusion.AcceptChanges();

						_views.MainForm.adapterCategoryToFilterExclusionBilling.Insert(groupDialog.SelectedCategoryID, filterId, exclusionType);
						_views.MainForm.adapterCategoryToFilterExclusionBilling.Fill(_views.MainForm.datasetBilling.CategoryToFilterExclusion);

						FillExcludedCategoriesList();
					}
				}
				else
					MessageBox.Show(this, "Please select filter first.", "No filter selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDeleteExcludeCategory_Click(object sender, EventArgs e)
		{
			try
			{
				if (listFilters.SelectedItems.Count > 0)
				{
					//Add value
					if (listExcludeCategories.SelectedItems.Count > 0)
					{
						var filterId = (int) listFilters.SelectedItems[0].Tag;

						var selectedID = (int) listExcludeCategories.SelectedItems[0].Tag;

						//Delete from database
						var connection = _views.MainForm.adapterCategoryToFilterExclusionBilling.Connection;
						if (connection.State != ConnectionState.Open)
							connection.Open();

						var cmd = connection.CreateCommand();
						cmd.CommandText = "DELETE FROM CategoryToFilterExclusion WHERE FilterID = @filterID AND CategoryID = @categoryID";
						cmd.Parameters.Clear();
						cmd.Parameters.Add(new OleDbParameter("@filterID", filterId.ToString()));
						cmd.Parameters.Add(new OleDbParameter("@categoryID", selectedID.ToString()));
						cmd.ExecuteNonQuery();

						_views.MainForm.adapterCategoryToFilterExclusionBilling.Fill(_views.MainForm.datasetBilling.CategoryToFilterExclusion);

						FillExcludedCategoriesList();
					}
					else
						MessageBox.Show(this, "Please select category first.", "No category selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
				else
					MessageBox.Show(this, "Please select filter first.", "No filter selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region "Calculate"

		private int selectedFilterIDForCalculation = -1;

		private void btnCalculate_Click(object sender, EventArgs e)
		{
			//Calculate for this filter and show progress bar
			if (listFilters.SelectedItems.Count > 0)
			{
                _views.MainForm.adapterDocumentsBilling.Fill(_views.MainForm.datasetBilling.Documents);

                worker = new BackgroundWorker();
				worker.WorkerReportsProgress = true;
				worker.DoWork += worker_DoWork;
				worker.ProgressChanged += worker_ProgressChanged;
				worker.RunWorkerCompleted += worker_RunWorkerCompleted;

				worker.RunWorkerAsync();

				progress = new FormGenerateSVMProgress();
				progress.ShowDialog();
			}
		}

		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
            if (_billingView._filterId == selectedFilterIDForCalculation)
            {                
                _billingView.FilterDocuments(_billingView._filterId);
            }

			MessageBox.Show(this, "Filter calculation completed", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
			progress.Close();
		}

		void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (e.ProgressPercentage <= 100)
				progress.UpdateProgress(e.ProgressPercentage);
		}

		void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			double step = 100.0 / _views.MainForm.datasetBilling.Documents.Count;
			var selectedItemId = (int) listFilters.SelectedItems[0].Tag;
			selectedFilterIDForCalculation = selectedItemId;

			CalculateFilter2(selectedItemId, worker, step / 2);
		}

		private void btnCalculateAll_Click(object sender, EventArgs e)
		{
			CalculateAll();
		}

		public void CalculateAll()
		{
            _views.MainForm.adapterDocumentsBilling.Fill(_views.MainForm.datasetBilling.Documents);

            workerAll = new BackgroundWorker();
			workerAll.DoWork += workerAll_DoWork;
			workerAll.WorkerReportsProgress = true;
			workerAll.ProgressChanged += workerAll_ProgressChanged;
			workerAll.RunWorkerCompleted += workerAll_RunWorkerCompleted;

			workerAll.RunWorkerAsync();

			progress = new FormGenerateSVMProgress();
			progress.ShowDialog();
		}

		void workerAll_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
            if (_billingView._filterId != -1)            {
                
                _billingView.FilterDocuments(_billingView._filterId);
            }

            MessageBox.Show("Filter calculation completed", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
			progress.Close();
		}

		void workerAll_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			var percentage = e.ProgressPercentage / filterCount;
			if (percentage + currentLoopProgress <= 100)
				progress.UpdateProgress(percentage + currentLoopProgress);
		}

		private int currentLoopProgress = 0;
		private int filterCount;

		void workerAll_DoWork(object sender, DoWorkEventArgs e)
		{
			filterCount = _views.MainForm.datasetBilling.ICDFilters.Count;
			double loopStep = (int) (100.0 / filterCount);
			double step = loopStep / _views.MainForm.datasetBilling.Documents.Count;

			int i = 0;
			foreach (RegScoreCalc.Data.BillingDataSet.ICDFiltersRow item in _views.MainForm.datasetBilling.ICDFilters)
			{
				CalculateFilter2(item.FilterID, workerAll, step / 2, currentLoopProgress);

				//Update progress
				currentLoopProgress += (int) loopStep;
				workerAll.ReportProgress(currentLoopProgress);
				i++;
			}

			workerAll.ReportProgress(100);

			//double loopStep = 100.0 / _views.MainForm.datasetBilling.ICDFilters.Count;
			//double step = loopStep / _views.MainForm.datasetBilling.Documents.Count;

			//int i = 0;
			//foreach (RegScoreCalc.Data.BillingDataSet.ICDFiltersRow item in _views.MainForm.datasetBilling.ICDFilters)
			//{
			//    CalculateFilter(item.FilterID, workerAll, step / 2, i * loopStep);

			//    //Update progress
			//    workerAll.ReportProgress((int)((i + 1) * loopStep));
			//    i++;
			//}

			//workerAll.ReportProgress(100);
		}

		public struct ICD9CodeStruct
		{
			public string Text;
			public bool RegExp;
			public bool OneWay;
			public bool Combination;
		}

		//NEW WAY

		private void DeleteFromDocsToFilters(int filterId)
		{
			var connection = _views.MainForm.adapterDocsToFiltersBilling.Connection;
			if (connection.State != ConnectionState.Open)
				connection.Open();
			OleDbCommand cmd = connection.CreateCommand();
			cmd.CommandText = "DELETE FROM DocsToFilters WHERE FilterID = @filterID";
			cmd.Parameters.Clear();
			cmd.Parameters.Add(new OleDbParameter("@filterID", filterId.ToString()));
			cmd.ExecuteNonQuery();

			_views.MainForm.adapterDocsToFiltersBilling.Fill(_views.MainForm.datasetBilling.DocsToFilters);
		}

		private void CalculateFilter2(int filterId, BackgroundWorker currentWorker, double progressBarStep, double currentProgress = 0)
		{
			try
			{
				CalculateFilter filter = new CalculateFilter(_views);
				filter.Calculate(filterId, currentWorker);

            }
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}
		
		#endregion

		#region "Close"

		private void DialogFilter_FormClosing(object sender, FormClosingEventArgs e)
		{
			//Save all new positions
			if (positionsChanged)
			{
				short position = 0;
				foreach (ListViewItem item in listFilters.Items)
				{
					var id = (int) item.Tag;
					_views.MainForm.datasetBilling.ICDFilters.FindByFilterID(id)
					      .Position = position;

					position++;
				}
				_views.MainForm.adapterICDFiltersBilling.Update(_views.MainForm.datasetBilling.ICDFilters);
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion
	}
}