using System;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using RegScoreCalc.Code;

namespace RegScoreCalc
{
	public class ViewExport : View
	{
		#region Types

		public class CategoryInfo
		{
			#region Data members

			public int ID;
			public string Category;

			#endregion

			#region Ctors

			public CategoryInfo()
			{
				ID = -1;
				Category = "";
			}

			public CategoryInfo(int nID, string strCategory)
			{
				ID = nID;
				Category = strCategory;
			}

			#endregion
		}

		#endregion

		#region Data members

		protected PaneDocuments _paneDocuments;
		protected PaneNotes _paneNotes;

		protected RibbonButtonList _ribbonList;

		#endregion

		#region Ctors

		public ViewExport(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
		}

		#endregion

		#region Events

		protected void OnDataModified(object sender, EventArgs e)
		{
			UpdateView();
		}

		protected void btnCategory_Clicked(object sender, EventArgs e)
		{
			RibbonButton btn = (RibbonButton) sender;
			UpdateButtonImage(btn);
		}

		protected void btnSelectAll_Clicked(object sender, EventArgs e)
		{
			UpdateSelection(true);
		}

		protected void btnDeselectAll_Clicked(object sender, EventArgs e)
		{
			UpdateSelection(false);
		}

		protected void btnExport_Clicked(object sender, EventArgs e)
		{
			ExportDocuments(false, false);
		}

		protected void btnExportAs_Clicked(object sender, EventArgs e)
		{
			ExportDocuments(false, true);
		}

		protected void btnExportEach_Clicked(object sender, EventArgs e)
		{
			ExportDocuments(true, false);
		}

		#endregion

		#region Overrides

		protected override void InitViewCommands(RibbonPanel panel)
		{
			_ribbonList = new RibbonButtonList();
			_ribbonList.ItemsWideInMediumMode = 3;
			_ribbonList.ItemsWideInLargeMode = 3;

			FillCategoriesList();

			panel.Items.Add(_ribbonList);

			RibbonButton btnExport = new RibbonButton("Export Selected");

			RibbonButton btnExportAs = new RibbonButton("Export To...");

			RibbonButton btnExportEach = new RibbonButton("Export Each");

			RibbonButton btnSelectAll = new RibbonButton("Select All");

			panel.Items.Add(btnSelectAll);

			btnSelectAll.Click += new EventHandler(btnSelectAll_Clicked);
			btnSelectAll.MinSizeMode = RibbonElementSizeMode.Medium;
			btnSelectAll.MaxSizeMode = RibbonElementSizeMode.Medium;
			btnSelectAll.SmallImage = RegScoreCalc.Properties.Resources.SelectAll;
			btnSelectAll.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnDeselectAll = new RibbonButton("Deselect All");

			panel.Items.Add(btnDeselectAll);

			btnDeselectAll.Click += new EventHandler(btnDeselectAll_Clicked);
			btnDeselectAll.MinSizeMode = RibbonElementSizeMode.Medium;
			btnDeselectAll.MaxSizeMode = RibbonElementSizeMode.Medium;
			btnDeselectAll.SmallImage = RegScoreCalc.Properties.Resources.DeselectAll;
			btnDeselectAll.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());
			panel.Items.Add(btnExport);
			panel.Items.Add(btnExportEach);
			panel.Items.Add(btnExportAs);

			btnExport.Click += new EventHandler(btnExport_Clicked);
			btnExport.Image = RegScoreCalc.Properties.Resources.Export;
			btnExport.SmallImage = RegScoreCalc.Properties.Resources.Export;
			btnExport.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			btnExportAs.Click += new EventHandler(btnExportAs_Clicked);
			btnExportAs.Image = RegScoreCalc.Properties.Resources.Export;
			btnExportAs.SmallImage = RegScoreCalc.Properties.Resources.Export;
			btnExportAs.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			btnExportEach.Click += new EventHandler(btnExportEach_Clicked);
			btnExportEach.Image = RegScoreCalc.Properties.Resources.Export;
			btnExportEach.SmallImage = RegScoreCalc.Properties.Resources.Export;
			btnExportEach.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
		}

		protected override void InitViewPanes(RibbonTab tab)
		{
			this.Orientation = Orientation.Horizontal;

			_paneDocuments = new PaneDocuments();
			_paneDocuments.InitPane(_views, this, this.Panel1, tab);
			_paneDocuments._eventDataModified += new EventHandler(OnDataModified);

			this.Panel1.Controls.Add(_paneDocuments);
			_paneDocuments.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			_paneNotes = new PaneNotes();
			_paneNotes.InitPane(_views, this, this.Panel2, tab);
			_paneNotes._eventDataModified += new EventHandler(OnDataModified);

			this.Panel2.Controls.Add(_paneNotes);
			_paneNotes.ShowPane();

			this.SplitterDistance = 350;

			CalculateScores();

			_paneDocuments.Select();
		}

		public override void UpdateView()
		{
			FillCategoriesList();

			if (_paneDocuments != null)
				_paneDocuments.UpdatePane();

			if (_paneNotes != null)
				_paneNotes.UpdatePane();
		}

		public override void DestroyView()
		{
			_paneDocuments.DestroyPane();
			_paneNotes.DestroyPane();

			base.DestroyView();
		}

		public override bool OnHotkey(string code)
		{
			var handled = _paneDocuments.OnHotkey(code);
			if (!handled)
				handled = _paneNotes.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			if (!handled)
				handled = base.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			return handled;
		}

		#endregion

		#region Implementation

		protected void UpdateButtonImage(RibbonButton btn)
		{
			if (btn.Checked)
				btn.SmallImage = RegScoreCalc.Properties.Resources.Checked;
			else
				btn.SmallImage = RegScoreCalc.Properties.Resources.Unchecked;
		}

		protected void FillCategoriesList()
		{
			int nCounter = 0;

			_ribbonList.DropDownItems.Clear();
			_ribbonList.Buttons.Clear();

			MainDataSet.CategoriesDataTable tableCategories = new MainDataSet.CategoriesDataTable();
			_views.MainForm.adapterCategories.Fill(tableCategories);

			RibbonButton btn;

			foreach (MainDataSet.CategoriesRow row in tableCategories)
			{
				if (!String.IsNullOrEmpty(row.Category))
				{
					btn = new RibbonButton(row.Category);
					btn.MaxSizeMode = RibbonElementSizeMode.Medium;
					btn.MinSizeMode = RibbonElementSizeMode.Medium;
					btn.Checked = true;
					btn.CheckOnClick = true;
					btn.Click += new EventHandler(btnCategory_Clicked);
					btn.Tag = row.ID;

					UpdateButtonImage(btn);

					if (nCounter >= 10)
						_ribbonList.DropDownItems.Add(btn);
					else
						_ribbonList.Buttons.Add(btn);

					nCounter++;
				}
			}
		}

		protected bool ExportDocuments(bool bEach, bool bConfirmTableName)
		{
			bool bResult = false;

			_views.MainForm.SaveAll();

			ArrayList arrCategories;
			if (bEach)
			{
				arrCategories = new ArrayList();

				CategoryInfo ci;
				foreach (RibbonButton btn in _ribbonList.Buttons)
				{
					if (btn.Checked)
					{
						ci = new CategoryInfo((int) btn.Tag, btn.Text);
						arrCategories.Add(ci);

						ProcessCategories(arrCategories, false);
						arrCategories.Clear();
					}
				}

				foreach (RibbonButton btn in _ribbonList.DropDownItems)
				{
					if (btn.Checked)
					{
						ci = new CategoryInfo((int) btn.Tag, btn.Text);
						arrCategories.Add(ci);

						ProcessCategories(arrCategories, false);
						arrCategories.Clear();
					}
				}

				MessageBox.Show("Operation finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				arrCategories = GetSelectedCategories();
				bResult = ProcessCategories(arrCategories, bConfirmTableName);
				if (bResult)
					MessageBox.Show("Operation finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			return bResult;
		}

		protected bool ProcessCategories(ArrayList arrCategories, bool bConfirmTableName)
		{
			bool bResult = false;

			if (arrCategories.Count > 0)
			{
				string strTableName = "Documents";

				foreach (CategoryInfo ci in arrCategories)
				{
					strTableName += "_" + ci.Category.Replace(" ", "_");
				}

				bool bCreate = false;

				if (bConfirmTableName)
				{
					FormTableInfo formTableInfo = new FormTableInfo();
					formTableInfo.TableName = strTableName;
					if (formTableInfo.ShowDialog() == DialogResult.OK)
					{
						strTableName = formTableInfo.TableName;
						bCreate = true;
					}
				}
				else
					bCreate = true;

				if (bCreate)
				{
					if (CreateTable(strTableName))
					{
						var tableDocuments = new MainDataSet.DocumentsDataTable();
						var adapter = new MainDataSetTableAdapters.DocumentsTableAdapter();
						adapter.Fill(tableDocuments);

						int nNullRows = 0;

						foreach (CategoryInfo ci in arrCategories)
						{
							nNullRows += InsertDocuments(strTableName, ci, tableDocuments);
						}

						bResult = true;
					}
				}
			}
			else
				MessageBox.Show("No categories selected", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

			return bResult;
		}

		protected int InsertDocuments(string strTableName, CategoryInfo ci, MainDataSet.DocumentsDataTable tableDocuments)
		{
			int nNullRows = 0;

			foreach (MainDataSet.DocumentsRow row in tableDocuments)
			{
				if (!row.IsCategoryNull())
				{
					if (row.Category == ci.ID)
						InsertDocument(strTableName, row, ci);
				}
				else
					nNullRows++;
			}

			return nNullRows;
		}

		protected bool InsertDocument(string strTableName, MainDataSet.DocumentsRow row, CategoryInfo ci)
		{
			bool bResult = false;

			try
			{
				OleDbConnection connection = _views.MainForm.adapterDocuments.Connection;
				if (connection != null)
				{
					if (connection.State == ConnectionState.Closed)
						connection.Open();

					OleDbCommand command;

					try
					{
						string strCommand = "INSERT INTO " + strTableName + "(ED_ENC_NUM, NoteText, Score, Category, CategoryID) "
						                    + "VALUES (?, ?, ?, ?, ?)";

						command = new OleDbCommand(strCommand, connection);

						command.Parameters.Add("ED_ENC_NUM", OleDbType.Double)
						       .Value = row.ED_ENC_NUM;
						command.Parameters.Add("NoteText", OleDbType.Char)
						       .Value = _views.DocumentsService.GetDocumentText(row.ED_ENC_NUM); // TODO
						command.Parameters.Add("Score", OleDbType.Integer)
						       .Value = row.Score;
						command.Parameters.Add("Category", OleDbType.Char)
						       .Value = ci.Category;
						command.Parameters.Add("CategoryID", OleDbType.Char)
						       .Value = ci.ID;

						int nCount = command.ExecuteNonQuery();
						if (nCount > 0)
							bResult = true;
					}
					catch
					{
					}

					connection.Close();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return bResult;
		}

		protected bool CreateTable(string strTableName)
		{
			bool bResult = false;

			try
			{
				OleDbConnection connection = _views.MainForm.adapterDocuments.Connection;
				if (connection != null)
				{
					if (connection.State == ConnectionState.Closed)
						connection.Open();

					OleDbCommand command;

					try
					{
						string strCommand = "SELECT * FROM " + strTableName;

						command = new OleDbCommand(strCommand, connection);
						OleDbDataAdapter adapter = new OleDbDataAdapter(command);

						DataSet ds = new DataSet();

						adapter.Fill(ds, strTableName);

						bResult = true;
					}
					catch
					{
					}

					try
					{
						if (!bResult)
						{
							string strCommand = "CREATE TABLE " + strTableName + " (ED_ENC_NUM double, NoteText ntext, Score int, Category ntext, CategoryID int)";

							command = new OleDbCommand(strCommand, connection);
							command.ExecuteNonQuery();

							bResult = true;
						}
					}
					catch (System.Exception e)
					{
						MessageBox.Show(e.Message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}

					connection.Close();
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return bResult;
		}

		protected ArrayList GetSelectedCategories()
		{
			ArrayList arrCategories = new ArrayList();

			CategoryInfo ci;

			foreach (RibbonButton btn in _ribbonList.Buttons)
			{
				if (btn.Checked)
				{
					ci = new CategoryInfo((int) btn.Tag, btn.Text);
					arrCategories.Add(ci);
				}
			}

			foreach (RibbonButton btn in _ribbonList.DropDownItems)
			{
				if (btn.Checked)
				{
					ci = new CategoryInfo((int) btn.Tag, btn.Text);
					arrCategories.Add(ci);
				}
			}

			return arrCategories;
		}

		protected void UpdateSelection(bool bSelected)
		{
			foreach (RibbonButton btn in _ribbonList.Buttons)
			{
				btn.Checked = bSelected;
				UpdateButtonImage(btn);
			}

			foreach (RibbonButton btn in _ribbonList.DropDownItems)
			{
				btn.Checked = bSelected;
				UpdateButtonImage(btn);
			}
		}

		protected void CalculateScores()
		{
			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;
			_views.MainForm.sourceRegExp.RaiseListChangedEvents = false;

			///////////////////////////////////////////////////////////////////////////////

			var formGenericProgress = new FormGenericProgress("Calculating scores, please wait...", DoCalcScores, null, true);
			formGenericProgress.ShowDialog();

			if (formGenericProgress.Result)
				MessageBox.Show("Calculation finished successfully", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

			///////////////////////////////////////////////////////////////////////////////

			_views.MainForm.sourceRegExp.RaiseListChangedEvents = true;
			_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
		}

		protected bool DoCalcScores(BackgroundWorker worker, object objArgument)
		{
			using (var processor = new ExternalRegExpToolWrapper(worker))
			{
				return processor.RegExp_CalcScores(_views);
			}
		}

		#endregion
	}
}