using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using RegScoreCalc.Code;

namespace RegScoreCalc.Forms
{
	public partial class ExportModifiedNotesPopUp : Form
	{
		#region Fields

		private readonly ViewsManager _views;
		private bool _isSettingsLoading;
		private DefaultViewData _viewData;

		#endregion

		#region Properties

		public ExportNotesOptions Options
		{
			get { return _viewData.ExportNotesOptions; }
		}

		private List<int> SelectedCategories
		{
			get
			{
				return lbCategories.CheckedItems.Cast<DataRowView>()
				                   .Select(x => x.Row)
				                   .Cast<MainDataSet.CategoriesRow>()
				                   .Select(x => x.ID)
				                   .ToList();
			}
		}

		#endregion

		#region Ctors

		public ExportModifiedNotesPopUp(ViewsManager views, string newCsvName)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			_views = views;
			txtOutputFileName.Text = newCsvName;
		}

		#endregion

		#region Events

		private void Form_Load(object sender, EventArgs e)
		{
			try
			{
				groupOutput.Paint += PaintGroupBox;
				groupCriteria.Paint += PaintGroupBox;
				groupCategories.Paint += PaintGroupBox;
				groupNotes.Paint += PaintGroupBox;
                groupColumns.Paint += PaintGroupBox;

                FillColumns();

				FillCategories();

				LoadSettings();
				UpdateOutputFileName();

				UpdateEnabledState();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

        private void FillColumns()
        {
            var columns = _views.MainForm.adapterDocuments.GetAllColumnsList();

            listColumns.Items.Clear();
            listSelectedColumns.Items.Clear();

            foreach (var col in columns)
            {
                listColumns.Items.Add(col.Name);
            }
        }

        private void ExportModifiedNotesPopUp_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (String.IsNullOrEmpty(txtOutputFileName.Text))
					{
						e.Cancel = true;
						txtOutputFileName.Focus();
					}
					else if (String.IsNullOrEmpty(txtOutputFolder.Text))
					{
						e.Cancel = true;
						txtOutputFolder.Focus();
					}
					else if (chkbExportDocumentsWithCategory.Checked)
					{
						if (!this.SelectedCategories.Any())
						{
							e.Cancel = true;
							MessageBox.Show("Please select at least one category");
						}
					} else if (rdbJsonExport.Checked && listSelectedColumns.Items.Count <= 0)
                    {
                        e.Cancel = true;
                        MessageBox.Show("Please select at least one columns");
                    }

					if (!e.Cancel)
						SaveSettings();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);

				e.Cancel = true;
			}
		}

		private void PaintGroupBox(object sender, PaintEventArgs e)
		{
			try
			{
				var groupBox = (GroupBox) sender;

				var g = e.Graphics;
				var size = TextRenderer.MeasureText(g, groupBox.Text, groupBox.Font ?? this.Font, new Size(), TextFormatFlags.SingleLine);
				size.Width += 5;

				const int y = 7;

				var pen = new Pen(Color.FromArgb(255, 43, 133, 255), 1);

				g.DrawLine(pen, 0, y, 0, e.ClipRectangle.Height - 2);
				g.DrawLine(pen, 0, y, 6, y);
				g.DrawLine(pen, size.Width, y, e.ClipRectangle.Width - 2, y);
				g.DrawLine(pen, e.ClipRectangle.Width - 2, y, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
				g.DrawLine(pen, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
			}
			catch (Exception ex)
			{

			}
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			try
			{
				var dlg = new FolderBrowserDialog
				{
					SelectedPath = txtOutputFolder.Text
				};

				if (dlg.ShowDialog() == DialogResult.OK)
					txtOutputFolder.Text = dlg.SelectedPath;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			try
			{
				SetCategoriesChecked(true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnDeselectAll_Click(object sender, EventArgs e)
		{
			try
			{
				SetCategoriesChecked(false);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void rdbExportAll_CheckedChanged(object sender, EventArgs e)
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

		private void rdbExportPositive_CheckedChanged(object sender, EventArgs e)
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

		private void rdbExportPositiveOrNegative_CheckedChanged(object sender, EventArgs e)
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

		private void chbExportDocumentsWithCategory_CheckedChanged(object sender, EventArgs e)
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

		private void rdbXmlExport_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateEnabledState();
				UpdateOutputFileName();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void rdbExcelExport_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateEnabledState();
				UpdateOutputFileName();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void LoadSettings()
		{
			_isSettingsLoading = true;

			try
			{
				_viewData = DefaultViewData.Load(BrowserManager.GetViewData(_views, "Default"));
				var settings = _viewData.ExportNotesOptions;

				if (settings.Mode == ExportMode.Excel)
					rdbExcelExport.Checked = true;
				else
					rdbXmlExport.Checked = true;

				txtNumberOfLines.Value = settings.ExcelNumberOfLines;
				txtOutputFolder.Text = settings.OutputFolder;

				if (settings.ExportCriteria == ExportCriteria.Positive)
					rdbExportPositive.Checked = true;
				else if (settings.ExportCriteria == ExportCriteria.PositiveNegative)
					rdbExportPositiveOrNegative.Checked = true;
				else
					rdbExportAll.Checked = true;

				chkbExportDocumentsWithCategory.Checked = settings.ExportWithCategory;

				if (settings.Categories.Any())
					SetCategoriesChecked(categoryID => settings.Categories.Contains(categoryID));
				else
					SetCategoriesChecked(true);

				chkbHighlightMatches.Checked = settings.ColorMatches;
				chkbIncludePrefixSuffix.Checked = settings.ColorMatches;

				if (String.IsNullOrEmpty(txtOutputFolder.Text))
					txtOutputFolder.Text = Path.Combine(Directory.GetCurrentDirectory(), "Data");
			}
			finally
			{
				_isSettingsLoading = false;
			}
		}

		protected void SaveSettings()
		{
			_viewData.ExportNotesOptions = GetOptions();

			var json = JsonConvert.SerializeObject(_viewData);
			BrowserManager.SetViewData(_views, "Default", json);
		}

		protected ExportNotesOptions GetOptions()
		{
			var settings = new ExportNotesOptions
			               {
				               Mode = rdbXmlExport.Checked ? ExportMode.Xml : rdbExcelExport.Checked ? ExportMode.Excel: ExportMode.Json,
				               ExcelNumberOfLines = Convert.ToInt32(txtNumberOfLines.Value),
				               OutputFolder = txtOutputFolder.Text,
				               OutputFileName = txtOutputFileName.Text,
				               ExportWithCategory = chkbExportDocumentsWithCategory.Checked,
				               Categories = this.SelectedCategories,
				               AddPrefixSuffix = chkbIncludePrefixSuffix.Checked,
				               ColorMatches = chkbHighlightMatches.Checked,
                               Columns = listSelectedColumns.Items.Cast<string>().ToList()
			               };

			if (rdbExportPositive.Checked)
				settings.ExportCriteria = ExportCriteria.Positive;
			else if (rdbExportPositiveOrNegative.Checked)
				settings.ExportCriteria = ExportCriteria.PositiveNegative;
			else
				settings.ExportCriteria = ExportCriteria.AllDocuments;

			return settings;
		}

		protected void UpdateOutputFileName()
		{
			var xmlExt = ".xml";
			var excelExt = ".xlsx";
            var jsonExt = ".json";

			var fileName = txtOutputFileName.Text;
			if (rdbXmlExport.Checked)
			{
				if (!fileName.EndsWith(xmlExt, StringComparison.InvariantCultureIgnoreCase))
				{
					if (fileName.EndsWith(excelExt, StringComparison.InvariantCultureIgnoreCase))
					{
						var pos = fileName.LastIndexOf(excelExt, StringComparison.InvariantCultureIgnoreCase);
						fileName = fileName.Substring(0, pos);
					}
                    if (fileName.EndsWith(jsonExt, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var pos = fileName.LastIndexOf(jsonExt, StringComparison.InvariantCultureIgnoreCase);
                        fileName = fileName.Substring(0, pos);
                    }
                    fileName += xmlExt;
				}
			}
			else if (rdbExcelExport.Checked)
			{
				if (!fileName.EndsWith(excelExt, StringComparison.InvariantCultureIgnoreCase))
				{
					if (fileName.EndsWith(xmlExt, StringComparison.InvariantCultureIgnoreCase))
					{
						var pos = fileName.LastIndexOf(xmlExt, StringComparison.InvariantCultureIgnoreCase);
						fileName = fileName.Substring(0, pos);
					}
                    if (fileName.EndsWith(jsonExt, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var pos = fileName.LastIndexOf(jsonExt, StringComparison.InvariantCultureIgnoreCase);
                        fileName = fileName.Substring(0, pos);
                    }
                    fileName += excelExt;
				}
			} else
            {
                if (!fileName.EndsWith(jsonExt, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (fileName.EndsWith(xmlExt, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var pos = fileName.LastIndexOf(xmlExt, StringComparison.InvariantCultureIgnoreCase);
                        fileName = fileName.Substring(0, pos);
                    }

                    if (fileName.EndsWith(excelExt, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var pos = fileName.LastIndexOf(excelExt, StringComparison.InvariantCultureIgnoreCase);
                        fileName = fileName.Substring(0, pos);
                    }
                    fileName += jsonExt;
                }
            }

			txtOutputFileName.Text = fileName;
		}

		protected void FillCategories()
		{
			lbCategories.DataSource = _views.MainForm.sourceCategories;
			lbCategories.DisplayMember = "Category";
			lbCategories.ValueMember = "ID";

			SetCategoriesChecked(true);
		}

		protected void SetCategoriesChecked(bool isChecked)
		{
			for (var i = 0; i < lbCategories.Items.Count; i++)
			{
				lbCategories.SetItemChecked(i, isChecked);
			}
		}

		protected void SetCategoriesChecked(Func<int, bool> func)
		{
			for (var i = 0; i < lbCategories.Items.Count; i++)
			{
				var rowView = (DataRowView) lbCategories.Items[i];
				var row = (MainDataSet.CategoriesRow) rowView.Row;
				lbCategories.SetItemChecked(i, func(row.ID));
			}
		}

		protected void UpdateEnabledState()
		{
			if (_isSettingsLoading)
				return;

			///////////////////////////////////////////////////////////////////////////////	

			lbCategories.Enabled = chkbExportDocumentsWithCategory.Checked;
			txtNumberOfLines.Enabled = rdbExcelExport.Checked;

            groupColumns.Enabled = rdbJsonExport.Checked;

			///////////////////////////////////////////////////////////////////////////////

			chkbHighlightMatches.Enabled = !rdbXmlExport.Checked;
		}

        #endregion

        private void rdbJsonExport_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateEnabledState();
                UpdateOutputFileName();
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listColumns.SelectedItem != null)
            {                
                listSelectedColumns.Items.Add(listColumns.SelectedItem);
                listColumns.Items.Remove(listColumns.SelectedItem);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (listSelectedColumns.SelectedItem != null)
            {
                listColumns.Items.Add(listSelectedColumns.SelectedItem);
                listSelectedColumns.Items.Remove(listSelectedColumns.SelectedItem);
            }
        }
    }

    public class ExportNotesOptions
	{
		#region Fields

		public ExportMode Mode { get; set; }
		public int ExcelNumberOfLines { get; set; }

		public string OutputFolder { get; set; }
		public string OutputFileName { get; set; }

		public ExportCriteria ExportCriteria { get; set; }
		
		public bool AddPrefixSuffix { get; set; }
		public bool ColorMatches { get; set; }
		public bool ExportWithCategory { get; set; }
		public List<int> Categories { get; set; }
        public List<string> Columns { get; set; }
        
		#endregion

		#region Ctors

		public ExportNotesOptions()
		{
			this.Categories = new List<int>();
			this.ExcelNumberOfLines = 8;
		}

		#endregion
	}

	public enum ExportMode
	{
		Xml = 0,
		Excel,
        Json
	}

	public enum ExportCriteria
	{
		AllDocuments = 0,
		Positive,
		PositiveNegative
	}
}
