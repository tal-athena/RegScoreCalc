using Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormDynamicColumns : Form
	{
		#region Fields

		protected ViewsManager _views;

		protected int _currentColumnID;
		protected DynamicColumnType _currentColumnType;
		protected Form _formColumnPropsEditor;

		protected bool _needReloadDocuments;
		protected bool _refreshColRegExps;

		#endregion

		#region Ctors

		public FormDynamicColumns(ViewsManager views)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			///////////////////////////////////////////////////////////////////////////////

			_views = views;

			_currentColumnType = DynamicColumnType.None;
		}

		#endregion

		#region Properties

		public bool NeedNeedReloadDocuments
		{
			get { return _needReloadDocuments; }
		}

		#endregion

		#region Events

		private void FormDynamicColumns_Load(object sender, EventArgs e)
		{
			headerColumnTitle.Width = lvDynamicColumns.ClientSize.Width;

			var hiddenColumnsList = FillColumnsList(0);
			if (hiddenColumnsList.Any())
			{
				var message = String.Format("Following column(s) not found in actual table and therefore not displayed in the list: {0}{0}{1}", Environment.NewLine, String.Join(", ", hiddenColumnsList));

				MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void FormDynamicColumns_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					SaveColumnProps();

					_views.MainForm.adapterDynamicColumns.Update(_views.MainForm.datasetMain.DynamicColumns);
					_views.MainForm.adapterDynamicColumnCategories.Update(_views.MainForm.datasetMain.DynamicColumnCategories);
				}

				_views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);
				_views.MainForm.adapterDynamicColumnCategories.Fill(_views.MainForm.datasetMain.DynamicColumnCategories);

				if (_refreshColRegExps)
				{
					_views.MainForm.sourceColRegExp.ResetBindings(false);
					_views.MainForm.sourceColScript.ResetBindings(false);
                    _views.MainForm.sourceColPython.ResetBindings(false);
                }
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void lvColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				SaveColumnProps();

				if (lvDynamicColumns.SelectedItems.Count == 1)
				{
					var dynamicColumnRow = (MainDataSet.DynamicColumnsRow) lvDynamicColumns.SelectedItems[0].Tag;

					ShowColumnPropsEditor(dynamicColumnRow, false);
				}
				else
					HideColumnPropsEditor();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnAddColumn_Click(object sender, EventArgs e)
		{
			try
			{
				AddColumn();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnRenameColumn_Click(object sender, EventArgs e)
		{
			try
			{
				RenameColumn();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDeleteColumn_Click(object sender, EventArgs e)
		{
			try
			{
				DeleteColumn();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void lvColumns_ItemActivate(object sender, EventArgs e)
		{
			try
			{
				RenameColumn();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnColumnUp_Click(object sender, EventArgs e)
		{
			try
			{
				MoveColumnUp();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnColumnDown_Click(object sender, EventArgs e)
		{
			try
			{
				MoveColumnDown();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCloneColumn_Click(object sender, EventArgs e)
		{
			try
			{
				CloneColumn();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnChangeType_Click(object sender, EventArgs e)
		{
			try
			{
				ChangeColumnType();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation: general

		protected List<string> FillColumnsList(int selectedColumnID)
		{
			lvDynamicColumns.Items.Clear();

			HideColumnPropsEditor();

			var selectFirstItem = true;

			///////////////////////////////////////////////////////////////////

			var actualDynamicColumnsList = _views.MainForm.adapterDocuments.GetDynamicColumnsCollection().ToList();
			var hiddenColumnsList = new List<string>();

			foreach (var row in _views.MainForm.datasetMain.DynamicColumns.Where(x => x.RowState != DataRowState.Deleted && !x.Title.StartsWith("NOTE_TEXT"))
			                          .OrderBy(x => x.Order))
			{
				if (actualDynamicColumnsList.FirstOrDefault(x => String.Equals(x.Name, row.Title, StringComparison.InvariantCultureIgnoreCase)) != null)
				{
					var item = lvDynamicColumns.Items.Add(row.Title);

					item.Tag = row;

					if (row.ID == selectedColumnID)
					{
						item.Selected = true;

						selectFirstItem = false;
					}
				}
				else
					hiddenColumnsList.Add(row.Title);
			}

			///////////////////////////////////////////////////////////////////

			if (selectFirstItem && lvDynamicColumns.Items.Count > 0)
				lvDynamicColumns.Items[0].Selected = true;

			///////////////////////////////////////////////////////////////////

			return hiddenColumnsList;
		}

		protected void MoveColumnUp()
		{
			if (lvDynamicColumns.SelectedItems.Count == 1)
			{
				var lvi = lvDynamicColumns.SelectedItems[0];
				var index = lvi.Index;
				if (index > 0)
				{
					lvDynamicColumns.Items.Remove(lvi);
					lvDynamicColumns.Items.Insert(index - 1, lvi);

					UpdateColumnOrder();
				}
			}
		}

		protected void MoveColumnDown()
		{
			if (lvDynamicColumns.SelectedItems.Count == 1)
			{
				var lvi = lvDynamicColumns.SelectedItems[0];
				var index = lvi.Index;
				if (index < lvDynamicColumns.Items.Count - 1)
				{
					lvDynamicColumns.Items.Remove(lvi);
					lvDynamicColumns.Items.Insert(index + 1, lvi);

					UpdateColumnOrder();
				}
			}
		}

		protected void UpdateColumnOrder()
		{
			foreach (ListViewItem lvi in lvDynamicColumns.Items)
			{
				var row = (MainDataSet.DynamicColumnsRow) lvi.Tag;
				row.Order = lvi.Index;
			}
		}

		#endregion

		#region Implementation: column properties

		protected void SaveColumnProps()
		{
			if (_currentColumnType != DynamicColumnType.None && _formColumnPropsEditor != null)
			{
				var dynamicColumnRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.RowState != DataRowState.Deleted && x.ID == _currentColumnID);
				if (dynamicColumnRow != null)
				{
					switch (_currentColumnType)
					{
						case DynamicColumnType.Numeric:
							var formNumeric = (FormColumnPropsNumeric) _formColumnPropsEditor;
							dynamicColumnRow.Properties = formNumeric.SaveProps();
							break;

						case DynamicColumnType.DateTime:
							var formDateTime = (FormColumnPropsDateTime) _formColumnPropsEditor;
							dynamicColumnRow.Properties = formDateTime.SaveProps();
							break;
					}
				}
			}
		}

		protected void HideColumnPropsEditor()
		{
			ShowColumnPropsEditor(null, false);
		}

		protected void ShowColumnPropsEditor(MainDataSet.DynamicColumnsRow dynamicColumnRow, bool forceRefresh)
		{
			if (dynamicColumnRow == null)
			{
				groupProperties.Visible = true;
				_currentColumnID = -1;
				_currentColumnType = DynamicColumnType.None;
				return;
			}

			///////////////////////////////////////////////////////////////////////////////

			var dynamicColumnID = dynamicColumnRow.ID;
			var type = (DynamicColumnType) dynamicColumnRow.Type;

			if (!forceRefresh && dynamicColumnID == _currentColumnID && _formColumnPropsEditor != null)
				return;

			if (_formColumnPropsEditor != null)
			{
				_formColumnPropsEditor.Close();
				_formColumnPropsEditor = null;
			}

			groupProperties.Visible = false;

			switch (type)
			{
				case DynamicColumnType.Category:
					_formColumnPropsEditor = new FormColumnPropsCategories(_views, dynamicColumnID, dynamicColumnRow.Title);
					break;

				case DynamicColumnType.FreeText:
					_formColumnPropsEditor = new FormColumnPropsFreeText();
					break;

				case DynamicColumnType.Numeric:
					_formColumnPropsEditor = new FormColumnPropsNumeric(_views, dynamicColumnRow);
					break;

				case DynamicColumnType.DateTime:
					_formColumnPropsEditor = new FormColumnPropsDateTime(_views, dynamicColumnRow);
					break;
			}

			if (_formColumnPropsEditor != null)
			{
				_formColumnPropsEditor.TopLevel = false;
				_formColumnPropsEditor.Parent = this;
				_formColumnPropsEditor.Left = groupProperties.Left;
				_formColumnPropsEditor.Top = groupProperties.Top;
				_formColumnPropsEditor.Show();
			}

			_currentColumnID = dynamicColumnID;
			_currentColumnType = type;
		}

		#endregion

		#region Implementation: classes

		protected void AddColumn()
		{
			var formClass = new FormEditDynamicColumn();
			formClass.ViewsManager = _views;
			formClass.Title = "New Column";

			if (formClass.ShowDialog() == DialogResult.OK)
			{
				var newColumn = _views.MainForm.adapterDocuments.AddDynamicColumn(formClass.Title, formClass.DynamicColumnType, true);
				var newColumnReviewML = _views.MainForm.adapterReviewMLDocumentsNew.AddDynamicColumn(formClass.Title, formClass.DynamicColumnType, false);

				if (formClass.DynamicColumnType == DynamicColumnType.Category)
				{
					_views.MainForm.adapterDocuments.AddExtraColumn(formClass.Title + " (SVM)", "INTEGER", true);
					_views.MainForm.adapterReviewMLDocumentsNew.AddExtraColumn(formClass.Title + " (SVM)", "INTEGER", false);
				}

				///////////////////////////////////////////////////////////////////////////////

				var order = _views.MainForm.datasetMain.DynamicColumns.Count(x => x.RowState != DataRowState.Deleted) + 1;

				_views.MainForm.adapterDynamicColumns.Insert(formClass.Title, (int)formClass.DynamicColumnType, order, String.Empty);
				_views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);

				var newRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.Title == formClass.Title);
				if (newRow != null)
				{
					newColumn.DynamicColumnID = newRow.ID;
					newColumnReviewML.DynamicColumnID = newRow.ID;

					FillColumnsList(newRow.ID);
				}
				else
					throw new Exception("Failed to add column");
			}
		}

		protected void RenameColumn()
		{
			if (lvDynamicColumns.SelectedItems.Count == 1)
			{
				var dynamicColumnRow = (MainDataSet.DynamicColumnsRow) lvDynamicColumns.SelectedItems[0].Tag;

				var oldTitle = dynamicColumnRow.Title;

				var formClass = new FormEditDynamicColumn
				                {
					                ViewsManager = _views,
					                ID = dynamicColumnRow.ID,
					                Title = dynamicColumnRow.Title,
					                DynamicColumnType = (DynamicColumnType) dynamicColumnRow.Type,
									DisableColumnTypeSelection = true,
					                lblType = { Visible = false },
					                cmbType = { Visible = false }
				                };

				if (formClass.ShowDialog() == DialogResult.OK)
				{
					_views.MainForm.adapterDocuments.RenameDynamicColumn(dynamicColumnRow.Title, formClass.Title, dynamicColumnRow.ID, true);
					_views.MainForm.adapterReviewMLDocumentsNew.RenameDynamicColumn(dynamicColumnRow.Title, formClass.Title, dynamicColumnRow.ID, false);

					dynamicColumnRow.Title = formClass.Title;

					_views.MainForm.adapterDynamicColumns.Update(dynamicColumnRow);
					_views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);

					///////////////////////////////////////////////////////////////////////////////

					if (formClass.DynamicColumnType == DynamicColumnType.Category)
					{
						var svmColumnName = oldTitle + " (SVM)";
						var svmColumn = _views.MainForm.adapterDocuments.GetExtraColumnsCollection().FirstOrDefault(x => String.Compare(x.Name, svmColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);
						if (svmColumn != null)
						{
							_views.MainForm.adapterDocuments.RenameExtraColumn(svmColumn.Name, formClass.Title + " (SVM)", true);
							_views.MainForm.adapterReviewMLDocumentsNew.RenameExtraColumn(svmColumn.Name, formClass.Title + " (SVM)", false);
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					FillColumnsList(formClass.ID);

					_needReloadDocuments = true;
				}
			}
		}

		protected void DeleteColumn()
		{
			if (lvDynamicColumns.SelectedItems.Count == 1)
			{
				var dynamicColumnRow = (MainDataSet.DynamicColumnsRow) lvDynamicColumns.SelectedItems[0].Tag;

				string message = "Do you wish to delete this column and all related values?";

				var dlgres = MessageBox.Show(message, MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
				if (dlgres == DialogResult.Yes)
				{
					var columnType = (DynamicColumnType) dynamicColumnRow.Type;
					if (columnType == DynamicColumnType.Category)
					{
						var categories = _views.MainForm.datasetMain.DynamicColumnCategories.Where(
							x => x.RowState != DataRowState.Deleted && x.DynamicColumnID == dynamicColumnRow.ID)
						                       .ToList();

						if (categories.Any())
							categories.ForEach(x => x.Delete());

						///////////////////////////////////////////////////////////////////////////////

						var svmColumnName = dynamicColumnRow.Title + " (SVM)";
						var svmColumn = _views.MainForm.adapterDocuments.GetExtraColumnsCollection()
											  .FirstOrDefault(x => String.Compare(x.Name, svmColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);
						if (svmColumn != null)
						{
							_views.MainForm.adapterDocuments.DeleteColumn(svmColumnName, true);
							_views.MainForm.adapterReviewMLDocumentsNew.DeleteColumn(svmColumnName, false);
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					_views.MainForm.adapterDocuments.DeleteColumn(dynamicColumnRow.Title, true);
					_views.MainForm.adapterReviewMLDocumentsNew.DeleteColumn(dynamicColumnRow.Title, false);

					///////////////////////////////////////////////////////////////////////////////

					var dynamicColumnID = dynamicColumnRow.ID;

					var rowScript = _views.MainForm.datasetMain.ColScript.FirstOrDefault(x => x.ColumnID == dynamicColumnID);
					if (rowScript != null)
					{
						rowScript.Delete();
						_views.MainForm.adapterColScript.Update(dynamicColumnRow);
						_views.MainForm.adapterColScript.Fill(_views.MainForm.datasetMain.ColScript);
					}

                    var rowPython = _views.MainForm.datasetMain.ColPython.FirstOrDefault(x => x.ColumnID == dynamicColumnID);
                    if (rowPython != null)
                    {
                        rowPython.Delete();
                        _views.MainForm.adapterColPython.Update(dynamicColumnRow);
                        _views.MainForm.adapterColPython.Fill(_views.MainForm.datasetMain.ColPython);
                    }

                    dynamicColumnRow.Delete();

					_views.MainForm.adapterDynamicColumns.Update(dynamicColumnRow);
					_views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);

					///////////////////////////////////////////////////////////////////////////////

					//Delete all ColRegExp's when deleting column class
					var regExp = _views.MainForm.datasetMain.ColRegExp.Where(x => x.RowState != DataRowState.Deleted && x.ColumnID == dynamicColumnID)
					                   .ToList();

					if (regExp.Any())
					{
						regExp.ForEach(x => x.Delete());

						_views.MainForm.adapterColRegExp.Update(_views.MainForm.datasetMain.ColRegExp);
						_views.MainForm.adapterColRegExp.Fill(_views.MainForm.datasetMain.ColRegExp);
					}

					///////////////////////////////////////////////////////////////////////////////

					HideColumnPropsEditor();

					_currentColumnID = 0;
					_currentColumnType = DynamicColumnType.None;

					FillColumnsList(-1);
				}
			}
		}

		protected void CloneColumn()
		{
			if (lvDynamicColumns.SelectedItems.Count != 1)
				return;

			var sourceDynamicColumnRow = (MainDataSet.DynamicColumnsRow) lvDynamicColumns.SelectedItems[0].Tag;
            var sourceColumnTitle = sourceDynamicColumnRow.Title;
			var sourceColumnID = sourceDynamicColumnRow.ID;

			var formClass = new FormEditDynamicColumn
			{
				ViewsManager = _views,
				Title = sourceDynamicColumnRow.Title + " copy",
				DynamicColumnType = (DynamicColumnType) sourceDynamicColumnRow.Type,
				DisableColumnTypeSelection = true
			};

			if (formClass.ShowDialog() == DialogResult.OK)
			{
				var newColumn = _views.MainForm.adapterDocuments.AddDynamicColumn(formClass.Title, formClass.DynamicColumnType, true);
				var newColumnReviewML = _views.MainForm.adapterReviewMLDocumentsNew.AddDynamicColumn(formClass.Title, formClass.DynamicColumnType, false);

				///////////////////////////////////////////////////////////////////////////////

				var order = _views.MainForm.datasetMain.DynamicColumns.Count(x => x.RowState != DataRowState.Deleted) + 1;

				var colProps = sourceDynamicColumnRow.IsPropertiesNull() ? String.Empty : sourceDynamicColumnRow.Properties;
				_views.MainForm.adapterDynamicColumns.Insert(formClass.Title, (int) formClass.DynamicColumnType, order, colProps);
				_views.MainForm.adapterDynamicColumns.Fill(_views.MainForm.datasetMain.DynamicColumns);

				var newRow = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.Title == formClass.Title);
				if (newRow != null)
				{
					newColumn.DynamicColumnID = newRow.ID;
					newColumnReviewML.DynamicColumnID = newRow.ID;

					///////////////////////////////////////////////////////////////////////////////

					if (formClass.DynamicColumnType == DynamicColumnType.Category)
						CloneCategories(sourceColumnID, sourceColumnTitle, newRow.ID, formClass.Title);

					CloneColumnRegExps(sourceColumnID, newRow.ID);
					CloneColumnScript(sourceColumnID, newRow.ID);
                    CloneColumnPython(sourceColumnID, newRow.ID);

                    ///////////////////////////////////////////////////////////////////////////////

                    FillColumnsList(newRow.ID);

					_needReloadDocuments = true;
				}
				else
					throw new Exception("Failed to clone column");
			}
		}

		protected void CloneColumnRegExps(int sourceColumnID, int newColumnID)
		{
			var regExps = _views.MainForm.datasetMain.ColRegExp.Where(x => x.ColumnID == sourceColumnID).ToList();
			if (regExps.Any())
			{
				_refreshColRegExps = true;
				_views.MainForm.sourceColRegExp.RaiseListChangedEvents = false;

				try
				{
					foreach (var sourceRow in regExps)
					{
						var newRow = _views.MainForm.datasetMain.ColRegExp.NewColRegExpRow();
						newRow.GUID = Guid.NewGuid().ToString();
						newRow.ColumnID = newColumnID;

						if (!sourceRow.IsRegExpNull())
							newRow.RegExp = sourceRow.RegExp;

						if (!sourceRow.IsExtractNull())
							newRow.Extract = sourceRow.Extract;

						if (!sourceRow.IsMatchNull())
							newRow.Match = sourceRow.Match;

						if (!sourceRow.IsRegExpColorNull())
							newRow.RegExpColor = sourceRow.RegExpColor;

						if (!sourceRow.IslookbehindNull())
							newRow.lookbehind = sourceRow.lookbehind;

						if (!sourceRow.Isneg_lookbehindNull())
							newRow.neg_lookbehind = sourceRow.neg_lookbehind;

						if (!sourceRow.IslookaheadNull())
							newRow.lookahead = sourceRow.lookahead;

						if (!sourceRow.Isneg_lookaheadNull())
							newRow.neg_lookahead = sourceRow.neg_lookahead;

						if (!sourceRow.IsExceptionsNull())
							newRow.Exceptions = sourceRow.Exceptions;

						if (!sourceRow.IsDescriptionNull())
							newRow.Description = sourceRow.Description;

						_views.MainForm.datasetMain.ColRegExp.AddColRegExpRow(newRow);
					}

					_views.MainForm.adapterColRegExp.Update(_views.MainForm.datasetMain.ColRegExp);
					_views.MainForm.adapterColRegExp.Fill(_views.MainForm.datasetMain.ColRegExp);
				}
				finally
				{
					_views.MainForm.sourceColRegExp.RaiseListChangedEvents = true;
				}
			}
		}

		protected void CloneColumnScript(int sourceColumnID, int newColumnID)
		{
			var rowScript = _views.MainForm.datasetMain.ColScript.FirstOrDefault(x => x.ColumnID == sourceColumnID && !x.IsDataNull());
			if (rowScript != null)
			{
				try
				{
					_refreshColRegExps = true;
					_views.MainForm.sourceColScript.RaiseListChangedEvents = false;

					_views.MainForm.datasetMain.ColScript.AddColScriptRow(newColumnID, rowScript.Data);

					_views.MainForm.adapterColScript.Update(_views.MainForm.datasetMain.ColScript);
					_views.MainForm.adapterColScript.Fill(_views.MainForm.datasetMain.ColScript);
				}
				finally
				{
					_views.MainForm.sourceColScript.RaiseListChangedEvents = true;
				}
			}
		}

        protected void CloneColumnPython(int sourceColumnID, int newColumnID)
        {
            var rowPython = _views.MainForm.datasetMain.ColPython.FirstOrDefault(x => x.ColumnID == sourceColumnID && !x.IsDataNull());
            if (rowPython != null)
            {
                try
                {
                    _refreshColRegExps = true;
                    _views.MainForm.sourceColPython.RaiseListChangedEvents = false;

                    _views.MainForm.datasetMain.ColPython.AddColPythonRow(newColumnID, rowPython.Data);

                    _views.MainForm.adapterColPython.Update(_views.MainForm.datasetMain.ColPython);
                    _views.MainForm.adapterColPython.Fill(_views.MainForm.datasetMain.ColPython);
                }
                finally
                {
                    _views.MainForm.sourceColPython.RaiseListChangedEvents = true;
                }
            }
        }

        protected void CloneCategories(int sourceColumnID, string sourceTitle, int newColumnID, string newTitle)
		{
            var svmColumnName = sourceTitle + " (SVM)";
            var svmColumn = _views.MainForm.adapterDocuments.GetExtraColumnsCollection()
                                  .FirstOrDefault(x => String.Compare(x.Name, svmColumnName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if( svmColumn != null)
            {
                _views.MainForm.adapterDocuments.AddExtraColumn(newTitle + " (SVM)", "INTEGER", true);
                _views.MainForm.adapterReviewMLDocumentsNew.AddExtraColumn(newTitle + " (SVM)", "INTEGER", false);
            }

            foreach (var category in _views.MainForm.datasetMain.DynamicColumnCategories.Where(x => x.DynamicColumnID == sourceColumnID).ToList())
			{
				var catProps = category.IsPropertiesNull() ? String.Empty : category.Properties;
				_views.MainForm.datasetMain.DynamicColumnCategories.AddDynamicColumnCategoriesRow(newColumnID, category.Title, category.Number, catProps);
			}

			_views.MainForm.adapterDynamicColumnCategories.Update(_views.MainForm.datasetMain.DynamicColumnCategories);
			_views.MainForm.adapterDynamicColumnCategories.Fill(_views.MainForm.datasetMain.DynamicColumnCategories);
		}

		protected void ChangeColumnType()
		{
			if (lvDynamicColumns.SelectedItems.Count != 1)
				return;

			var sourceDynamicColumnRow = (MainDataSet.DynamicColumnsRow) lvDynamicColumns.SelectedItems[0].Tag;
			if (sourceDynamicColumnRow.Type == (int) DynamicColumnType.FreeText)
			{
				MessageBox.Show("Free text column cannot be converted", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var dlgres = MessageBox.Show("Do you wish to convert this column to free text?", MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
			if (dlgres == DialogResult.Yes)
			{
				this.Enabled = false;

				var formProgress = new FormGenericProgress("Converting column...", DoChangeColumnType, sourceDynamicColumnRow, true);
				formProgress.ShowDialog();

				_views.MainForm.sourceDocuments.ResetBindings(true);

				ShowColumnPropsEditor(sourceDynamicColumnRow, true);

				this.Enabled = true;
			}
		}

		protected bool DoChangeColumnType(BackgroundWorker worker, object arg)
		{
			try
			{
				_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

				worker.ReportProgress(0);

				var sourceDynamicColumnRow = (MainDataSet.DynamicColumnsRow) arg;
				var sourceDynamicColumnID = sourceDynamicColumnRow.ID;
				var sourceColumnTitle = sourceDynamicColumnRow.Title;
				var sourceColumnType = (DynamicColumnType) sourceDynamicColumnRow.Type;
				var sourceColumnIndex = _views.MainForm.datasetMain.Documents.Columns[sourceDynamicColumnRow.Title].Ordinal;

				var tempColumnName = GetTempColumnName(sourceDynamicColumnRow.Title);
				var tempColumnInfo = _views.MainForm.adapterDocuments.AddDynamicColumn(tempColumnName, DynamicColumnType.FreeText, true);
				var tempColumnIndex = _views.MainForm.datasetMain.Documents.Columns[tempColumnName].Ordinal;

				var primaryKeyColumnIndex = _views.MainForm.datasetMain.Documents.Columns[_views.MainForm.adapterDocuments.PrimaryKeyColumnName].Ordinal;

				var numericFormat = String.Empty;
				var dateFormat = String.Empty;
				if (sourceColumnType == DynamicColumnType.Numeric)
				{
					var props = NumericColumnProperties.Load(sourceDynamicColumnRow);
					numericFormat = props.GetFormatString();
				}
				else if (sourceColumnType == DynamicColumnType.DateTime)
				{
					var props = DateTimeColumnProperties.Load(sourceDynamicColumnRow);
					dateFormat = props.GetFormatString();
				}

				var totalCount = (double) _views.MainForm.datasetMain.Documents.Count;
				var counter = 0;

				var documentsUpdated = false;
				foreach (var row in _views.MainForm.datasetMain.Documents)
				{
					if (!row.IsNull(sourceColumnIndex))
					{
						var primaryKey = row[primaryKeyColumnIndex];
						var formattedValue = String.Empty;

						if (sourceColumnType == DynamicColumnType.Numeric)
						{
							var value = row[sourceColumnIndex];
							if (value is double)
								formattedValue = ((double) value).ToString(numericFormat);
							else if (value is decimal)
								formattedValue = ((decimal) value).ToString(numericFormat);
							else if (value is int)
								formattedValue = ((int) value).ToString(numericFormat);
						}
						else if (sourceColumnType == DynamicColumnType.DateTime)
						{
							var value = (DateTime) row[sourceColumnIndex];
							formattedValue = value.ToString(dateFormat);
						}
						else
							formattedValue = Convert.ToString(row[sourceColumnIndex]);

						_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey(tempColumnName, formattedValue, primaryKey);
						row[tempColumnIndex] = formattedValue;
						documentsUpdated = true;
					}

					counter++;
					if (counter % 50 == 0)
					{
						var progress = (int) ((counter / totalCount) * 100d);
						worker.ReportProgress(progress);
					}
				}

				if (documentsUpdated)
				{
					_views.MainForm.datasetMain.Documents.AcceptChanges();
					_needReloadDocuments = true;
				}

				///////////////////////////////////////////////////////////////////////////////

				_views.MainForm.adapterDocuments.DeleteColumn(sourceColumnTitle, true);
				_views.MainForm.adapterDocuments.RenameDynamicColumn(tempColumnName, sourceColumnTitle, sourceDynamicColumnID, true);

				sourceDynamicColumnRow.Type = (int) DynamicColumnType.FreeText;
				sourceDynamicColumnRow.SetPropertiesNull();
				_views.MainForm.adapterDynamicColumns.Update(sourceDynamicColumnRow);

				if (sourceColumnType == DynamicColumnType.Category)
				{
					_views.MainForm.datasetMain.DynamicColumnCategories.Where(x => x.DynamicColumnID == sourceDynamicColumnID).ToList().ForEach(x => x.Delete());
					_views.MainForm.adapterDynamicColumnCategories.Update(_views.MainForm.datasetMain.DynamicColumnCategories);
					_views.MainForm.adapterDynamicColumnCategories.Fill(_views.MainForm.datasetMain.DynamicColumnCategories);
				}

				return true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
			finally
			{
				_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
			}

			return false;
		}

		protected string GetTempColumnName(string title)
		{
			var existingColumnNames = _views.MainForm.adapterDocuments.GetActualColumnsList().Select(x => x.Name).ToList();
			var counter = 1;
			string resultTitle;

			do
			{
				resultTitle = "temp" + title + counter;
				counter++;
			}
			while (existingColumnNames.Contains(resultTitle));

			return resultTitle;
		}

        #endregion

        private void chkSVMColumn_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public enum DynamicColumnType
	{
		#region Constants

		Category = 0,
		FreeText,
		Numeric,
		DateTime,
		None

		#endregion
	}
}