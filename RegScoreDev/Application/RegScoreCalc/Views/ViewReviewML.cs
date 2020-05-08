using RegScoreCalc.Forms;
using RegScoreCalc.Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

using Newtonsoft.Json;

using RegScoreCalc.Code;
using RegScoreCalc.Views.Models;

namespace RegScoreCalc
{
	public class ViewReviewML : View
	{
		#region Fields

		bool _dataInitialized = true;

		public int _cutoffMin = 100;
		public int _cutoffMax = -100;

		List<int> _positiveCategoriesIds = new List<int>();

		protected int _pom = 5;
		protected bool _fastClassificationOn;

		protected int preCalculatedPrevelance { get; set; }

		protected int _reviewMLIndex;

		protected PaneDocumentsReviewML _paneDocumentsReviewML;
		protected PaneTabNotes _paneNotes;
		protected Timer _timer;

		protected System.Timers.Timer ClickTimer;

		protected PaneNotes paneNotes;
		protected ListBox categoriesListBox;

		protected DataGridView gridView;
		protected SplitContainer _splitter;
		protected PaneReviewStatistics _paneReview;
		protected BindingSource bs;

		protected string _openedFilePath = "";
		protected string _filename = "";
		protected bool _openedFromStart;

		protected int _dynamicCategoryColumnID;
		protected ColumnInfo _svmColumnInfo;

		protected bool _messageShown;

        protected string _noteTextColumnName;

        #endregion

        #region Ctors

        public ViewReviewML(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
			_openedFromStart = _views.MainForm.ReviewMLOpenedFromStart;

			ClickTimer = new System.Timers.Timer();
			ClickTimer.Interval = 300;
			ClickTimer.Elapsed += ClickTimer_Elapsed;

			_paneReview.numericCutoff.ValueChanged += numericCutoff_ValueChanged;
			preCalculatedPrevelance = 0;

			_dynamicCategoryColumnID = 0;
			_svmColumnInfo = SvmColumnService.GetSvmColumnInfo(_views, _dynamicCategoryColumnID, false);

            _noteTextColumnName = "NOTE_TEXT";


            GetSavedCategories();
		}

		#endregion

		#region Events

		private void OnDataModified(object sender, EventArgs e)
		{
			UpdateTextNotePane();
		}

		private void numericCutoff_ValueChanged(object sender, EventArgs e)
		{
			RecalculateCutoff();
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			//Sort by Rank

			if (_pom > 0)
			{
				_pom--;
				gridView = _paneDocumentsReviewML.GetDocumentsGrid();

				if (_reviewMLIndex == 2)
				{
					try
					{
						gridView.Sort(gridView.Columns["columnRank"], ListSortDirection.Ascending);
					}
					catch
					{
					}

					gridView.Columns["columnProc1SVM"].Visible = false;
					gridView.Columns["columnProc3SVM"].Visible = false;
				}
				//else if (filename[0] == '1')
				else if (_reviewMLIndex == 1)
				{
					try
					{
						gridView.Sort(gridView.Columns["columnProc1SVM"], ListSortDirection.Descending);
					}
					catch
					{
					}
					gridView.Columns["columnProc3SVM"].Visible = false;
					gridView.Columns["columnRank"].Visible = false;
				}
				//else if (filename[0] == '3')
				else if (_reviewMLIndex == 3)
				{
					try
					{
						gridView.Sort(gridView.Columns["columnProc3SVM"], ListSortDirection.Descending);
					}
					catch
					{
					}
					gridView.Columns["columnProc1SVM"].Visible = false;
					gridView.Columns["columnRank"].Visible = false;
				}

				//check what process is finished, and then sort

				_timer.Stop();
				_timer.Start();
			}
			else
			{
				_pom = 5;
				_timer.Stop();
			}
		}

		private void UpdateTextNotePane()
		{
			if (gridView.Rows.Count > 0 && _paneNotes != null)
			{
				//get current text and update text pane
				if (gridView.SelectedRows.Count > 0)
				{
					DataRowView rowview = (DataRowView) gridView.SelectedRows[0].DataBoundItem;
					MainDataSet.ReviewMLDocumentsRow rowReviewDocuments = (MainDataSet.ReviewMLDocumentsRow) rowview.Row;

					//var text = _views.DocumentsService.GetDocumentText(rowReviewDocuments.ED_ENC_NUM);                    
					_paneNotes.InvokeUpdateTextOnMainThread(rowReviewDocuments.ED_ENC_NUM, true);
				}
				else
				{
					DataRowView rowview = (DataRowView) gridView.Rows[0].DataBoundItem;
					MainDataSet.ReviewMLDocumentsRow rowReviewDocuments = (MainDataSet.ReviewMLDocumentsRow) rowview.Row;

					//var text = _views.DocumentsService.GetDocumentText(rowReviewDocuments.ED_ENC_NUM);
					_paneNotes.InvokeUpdateTextOnMainThread(rowReviewDocuments.ED_ENC_NUM, true);
				}
			}
		}

		private void OnFastClassification_Clicked(object sender, EventArgs e)
		{
			FastClassification();
		}

		private void OnResetViews_Clicked(object sender, EventArgs e)
		{
			ResetView();
		}

		private void sourceCategories_ListChanged(object sender, ListChangedEventArgs e)
		{
			InitCategoriesToReviewStatistic();

			InitCategoriesListBox();
		}

		private void categoriesListBox_MouseDown(object sender, MouseEventArgs e)
		{
			ClickTimer.Stop();

			ClickTimer.Start();
		}

		private void ClickTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			ClickTimer.Stop();

			SetCategoryFastReview();

			CountRatios();
		}

		private void gridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (_paneReview.cmbCategories.SelectedIndex > -1)
				CountRatios();
		}

		private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
		{
			CountRatios();
		}

		private void btnOpenSVM1_Click(object sender, EventArgs e)
		{
			OpenFileDialog opf = new OpenFileDialog();
			opf.Filter = "CSV Files|*.csv";
			opf.Multiselect = false;
			if (opf.ShowDialog() == DialogResult.OK)
			{
				_reviewMLIndex = 1;
				_filename = "1";
				_openedFilePath = opf.FileName;


				InitRankData();
			}
		}

		private void btnOpenSVM2_Click(object sender, EventArgs e)
		{
			OpenFileDialog opf = new OpenFileDialog();
			opf.Filter = "DAT Files|*.dat";
			opf.Multiselect = false;
			if (opf.ShowDialog() == DialogResult.OK)
			{
				_reviewMLIndex = 2;
				_filename = "2";
				_openedFilePath = opf.FileName;

				InitRankData();
			}
		}

		private void btnOpenSVM3_Click(object sender, EventArgs e)
		{
			OpenFileDialog opf = new OpenFileDialog();
			opf.Filter = "CSV Files|*.csv";
			opf.Multiselect = false;
			if (opf.ShowDialog() == DialogResult.OK)
			{
				_reviewMLIndex = 3;
				_filename = "3";
				_openedFilePath = opf.FileName;

				InitRankData();
			}
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			//Open help file
			var path = System.IO.Directory.GetCurrentDirectory();
			var filepath = path + "\\Resources\\ReviewMLHelpHtml.html";

			if (_helpOpened)
			{
				help.NavigateToUrl(filepath);
				help.Focus();
				help.BringToFront();
			}
			else
			{
				help = new HelpForm(filepath);

				Rectangle screenSize = Screen.PrimaryScreen.Bounds;

				help.Width = 600;
				help.Height = screenSize.Height - 50;

				int x = screenSize.Right - 600;
				int y = Screen.PrimaryScreen.WorkingArea.Top;
				help.Location = new Point(x, y);
				help.FormClosed += help_FormClosed;

				help.Show();

				_helpOpened = true;
			}
		}

		private void help_FormClosed(object sender, FormClosedEventArgs e)
		{
			_helpOpened = false;
		}

		private void btnPerformance_Click(object sender, EventArgs e)
		{
			RecalculateCutoff();
		}

		private void btnToggleStatistics_Click(object sender, EventArgs e)
		{
			try
			{
				this.Panel1Collapsed = !this.Panel1Collapsed;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Overrides

		protected override void InitViewPanes(RibbonTab tab)
		{
			_views.MainForm.LoadReviewMLDocumentsNew();

			SetDynamicCategoryColumn(0);

			_openedFromStart = _views.MainForm.ReviewMLOpenedFromStart;
			//Check what type of process is file
			_filename = Properties.Settings.Default.ProcessName;
			_reviewMLIndex = 1;
			if (!String.IsNullOrEmpty(_filename))
			{
				if (_filename[0] == '1')
					_reviewMLIndex = 1;
				else if (_filename[1] == '2')
					_reviewMLIndex = 2;
				else
					_reviewMLIndex = 3;
			}

			//Event that catches changes in categories
			_views.MainForm.sourceCategories.ListChanged += sourceCategories_ListChanged;

			this.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.SplitterDistance = 240;

			//1.	Review Statistics pane:  

			_paneReview = new PaneReviewStatistics();

			_paneReview.InitPane(_views, this, this.Panel1, tab);
			_paneReview.ShowPane();

			this.Panel1.Controls.Add(_paneReview);

			_paneReview.cmbCategories.SelectedIndexChanged += cmbCategories_SelectedIndexChanged;

			InitCategoriesToReviewStatistic();

			//Old panes

			_splitter = new SplitContainer();
			_splitter.Name = "Splitter";
			_splitter.Orientation = Orientation.Vertical;
			_splitter.BorderStyle = BorderStyle.Fixed3D;
			_splitter.Dock = DockStyle.Fill;
			_splitter.BackColor = MainForm.ColorBackground;
			_splitter.Panel1MinSize = 0;
			_splitter.Panel2MinSize = 0;

			this.Panel2.Controls.Add(_splitter);

			_paneDocumentsReviewML = new PaneDocumentsReviewML(_dynamicCategoryColumnID, _svmColumnInfo.Name);
			_paneDocumentsReviewML.InitPane(_views, this, _splitter.Panel1, tab);
			_paneDocumentsReviewML._eventDataModified += new EventHandler(OnDataModified);

			gridView = _paneDocumentsReviewML.GetDocumentsGrid();
			gridView.CellClick += gridView_CellClick;

			if (_openedFromStart)
			{
				//Do not load data
				_dataInitialized = false;
			}
			else
			{
				var ranksFilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer", "output.csv");
				if (_reviewMLIndex == 2)
					ranksFilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer", "ranks.dat");

				if (File.Exists(ranksFilePath))
				{
					//Init of the data from the ranks.dat
					InitRankData();

					//Remove not needed rows
					FilterBindingSource();
				}
			}

			_splitter.Panel1.Controls.Add(_paneDocumentsReviewML);
			_paneDocumentsReviewML.ShowPane();

            //////////////////////////////////////////////////////////////////////////
            /*
			_paneNotes = new PaneNotes();
            _paneNotes.ShowFontPane = true;
            _paneNotes.InitPane(_views, this, _splitter.Panel2, tab);
			_paneNotes.ShowToolbar = false;            

			//Remove not needed button
			//_paneNotes.panel.Items.Remove(_paneNotes.btnWriteNotes);

			_paneNotes._eventDataModified += new EventHandler(OnDataModified);

			_splitter.Panel2.Controls.Add(_paneNotes);
			_paneNotes.ShowPane();
            */

            _paneNotes = new PaneTabNotes();
            _paneNotes.InitPane(_views, this, _splitter.Panel2, tab);
            _paneNotes._eventDataModified += new EventHandler(OnDataModified);
            _splitter.Panel2.Controls.Add(_paneNotes);
            _paneNotes.ShowPane();

            //Remove not needed button
            _paneNotes.panel.Items.Remove(_paneNotes.btnWriteNotes);
            //_paneNote.ReviewNoteTextColumn =


            ResetView();

			UpdateView();
		}

		protected override void InitViewCommands(RibbonPanel panel)
		{
			RibbonButton btnFastClassification = new RibbonButton("Fast Classification");
			panel.Items.Add(btnFastClassification);
			btnFastClassification.Image = Properties.Resources.ResetView;
			btnFastClassification.SmallImage = Properties.Resources.ResetView;
			btnFastClassification.Click += new EventHandler(OnFastClassification_Clicked);
			btnFastClassification.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnResetView = new RibbonButton("Reset view");
			panel.Items.Add(btnResetView);
			btnResetView.Image = Properties.Resources.ResetView;
			btnResetView.SmallImage = Properties.Resources.ResetView;
			btnResetView.Click += new EventHandler(OnResetViews_Clicked);
			btnResetView.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnToggleStatistics = new RibbonButton("Toggle Statistics");
			panel.Items.Add(btnToggleStatistics);
			btnToggleStatistics.Image = Properties.Resources.statistics;
			btnToggleStatistics.SmallImage = Properties.Resources.statistics;
			btnToggleStatistics.Click += btnToggleStatistics_Click;
			btnToggleStatistics.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnPerformance = new RibbonButton("Performance");
			panel.Items.Add(btnPerformance);
			btnPerformance.Image = Properties.Resources.CalcScores;
			btnPerformance.SmallImage = Properties.Resources.CalcScores;
			btnPerformance.Click += btnPerformance_Click;
			btnPerformance.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			//Open Svm data
			RibbonButton btnOpenSVM1 = new RibbonButton("Open SVM 1 Output");
			panel.Items.Add(btnOpenSVM1);
			btnOpenSVM1.Image = Properties.Resources.Open;
			btnOpenSVM1.SmallImage = Properties.Resources.Open;
			btnOpenSVM1.Click += btnOpenSVM1_Click;
			btnOpenSVM1.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnOpenSVM2 = new RibbonButton("Open SVM 2 Output");
			panel.Items.Add(btnOpenSVM2);
			btnOpenSVM2.Image = Properties.Resources.Open;
			btnOpenSVM2.SmallImage = Properties.Resources.Open;
			btnOpenSVM2.Click += btnOpenSVM2_Click;
			btnOpenSVM2.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnOpenSVM3 = new RibbonButton("Open SVM 3 Output");
			panel.Items.Add(btnOpenSVM3);
			btnOpenSVM3.Image = Properties.Resources.Open;
			btnOpenSVM3.SmallImage = Properties.Resources.Open;
			btnOpenSVM3.Click += btnOpenSVM3_Click;
			btnOpenSVM3.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnHelp = new RibbonButton("Help");
			panel.Items.Add(btnHelp);
			btnHelp.Image = Properties.Resources.HelpIcon;
			btnHelp.SmallImage = Properties.Resources.HelpIcon;
			btnHelp.Click += btnHelp_Click;
			btnHelp.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
		}

		public override void DestroyView()
		{
			_paneDocumentsReviewML.DestroyPane();
			_paneNotes.DestroyPane();

			base.DestroyView();
		}

		public override bool OnHotkey(string code)
		{
			var handled = _paneDocumentsReviewML.OnHotkey(code);
			if (!handled)
				handled = _paneNotes.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			if (!handled)
				handled = base.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			return handled;
		}

		public override void UpdateView()
		{
			if (_paneDocumentsReviewML != null)
			{
				_paneDocumentsReviewML.UpdatePane();
				try
				{
					if (_dataInitialized)
					{
						FilterBindingSource();

						//   StartTimer();
						UpdateTextNotePane();
					}
					else
					{
						//Hide all data
						gridView = _paneDocumentsReviewML.GetDocumentsGrid();

						bs = ((BindingSource) gridView.DataSource);
						bs.Filter = "ED_ENC_NUM is null";
					}
				}
				catch
				{
				}
			}
		}

		#endregion

		#region Implementation

		protected void LoadDynamicCategories()
		{
			if (_dynamicCategoryColumnID <= 0)
				return;

			var dynamicColumn = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == _dynamicCategoryColumnID);
			if (dynamicColumn != null)
			{
				var dynamicCategories = _views.MainForm.datasetMain.DynamicColumnCategories.Where(x => !x.IsDynamicColumnIDNull() && x.DynamicColumnID == _dynamicCategoryColumnID);
				var documents = _views.MainForm.datasetMain.Documents;
				var reviewDocuments = (MainDataSet.ReviewMLDocumentsDataTable) _views.MainForm.adapterReviewMLDocumentsNew.Table;

				var join = from doc in documents
				           join reviewDoc in reviewDocuments
					           on doc.ED_ENC_NUM equals reviewDoc.ED_ENC_NUM
				           join cat in dynamicCategories
					           on doc.Field<string>(dynamicColumn.Title) equals cat.Title
					           into tmp
				           from t in tmp.DefaultIfEmpty()
				           select new { reviewDoc, ID = t != null ? (int?) t.ID : null };

				foreach (var j in join)
				{
					if (j.ID != null)
						j.reviewDoc.Category = j.ID.Value;
					else
						j.reviewDoc.SetCategoryNull();
				}

				reviewDocuments.AcceptChanges();
			}
		}

		protected List<CategoryInfo> GetCategories()
		{
			var categories = _dynamicCategoryColumnID > 0
				? _views.MainForm.datasetMain.DynamicColumnCategories.Where(x => !x.IsDynamicColumnIDNull() && x.DynamicColumnID == _dynamicCategoryColumnID)
						.Select(x => new CategoryInfo { ID = x.ID, Title = x.Title })
						.ToList()
				: _views.MainForm.datasetMain.Categories.Select(x => new CategoryInfo { ID = x.ID, Title = x.Category })
						.ToList();

			return categories;
		}

		protected void InitCategoriesToReviewStatistic()
		{
			_paneReview.cmbCategories.Items.Clear();

			foreach (var cat in GetCategories())
			{
				_paneReview.cmbCategories.Items.Add(cat);
			}

			_paneReview.cmbCategories.ValueMember = "ID";
			_paneReview.cmbCategories.DisplayMember = "Title";
		}

		protected void FilterBindingSource()
		{
			gridView = _paneDocumentsReviewML.GetDocumentsGrid();

			bs = ((BindingSource)gridView.DataSource);

            // TODO: Revise these message boxes, currently commented out, they seem to be not needed for this view

			if (_reviewMLIndex == 1)
			{
				bs.Filter = "[" + _svmColumnInfo.Name + "] is not null";
				if (gridView.Rows.Count == 0)
				{
					if (!_messageShown)
					{
						//MessageBox.Show(this, "Proc1SVM is not initialized, please do process at server to get ranks!", "Proc1SVM don't exist.");
						_messageShown = true;
					}
				}
				else
				{
					try
					{
						gridView.Sort(gridView.Columns["columnProc1SVM"], ListSortDirection.Descending);
					}
					catch
					{
					}
				}
			}
			else if (_reviewMLIndex == 2)
			{
				bs.Filter = "Rank is not null";
				if (gridView.Rows.Count == 0)
				{
					if (!_messageShown)
					{
						//MessageBox.Show(this, "Ranks not initialized, please do process at server to get ranks!", "Ranks don't exist.");
						_messageShown = true;
					}
				}
				else
				{
					try
					{
						gridView.Sort(gridView.Columns["columnRank"], ListSortDirection.Ascending);
					}
					catch
					{
					}
				}
			}
			else if (_reviewMLIndex == 3)
			{
				bs.Filter = "Proc3SVM is not null";
				if (gridView.Rows.Count == 0)
				{
					if (!_messageShown)
					{
						//MessageBox.Show(this, "Proc3SVM is not initialized, please do process at server to get ranks!", "Proc3SVM don't exist.");
						_messageShown = true;
					}
				}
				else
				{
					try
					{
						gridView.Sort(gridView.Columns["columnProc3SVM"], ListSortDirection.Descending);
					}
					catch
					{
					}
				}
			}
		}

		protected void RecalculateCutoff()
		{
			//Recalculate cutoff
			//int rank;

			int tp = 0;
			int fp = 0;
			int fn = 0;
			int tn = 0;

			int curentCutoff = (int) _paneReview.numericCutoff.Value;

			var reviewMLDocumentsTable = _views.MainForm.adapterReviewMLDocumentsNew.Table;

			if (_reviewMLIndex == 1)
			{
				var svmColumnIndex = GetSvmColumnIndex();
				foreach (MainDataSet.ReviewMLDocumentsRow row in reviewMLDocumentsTable.Rows)
				{
					try
					{
						if (!row.IsNull(svmColumnIndex))
						{
							CutoffOneRowCalculation(ref tp, ref fp, ref fn, ref tn, curentCutoff, row, (int) row[svmColumnIndex]);
						}
					}
					catch
					{
					}
				}
			}
			//else if (filename[0] == '3')
			else if (_reviewMLIndex == 3)
			{
				foreach (MainDataSet.ReviewMLDocumentsRow row in reviewMLDocumentsTable.Rows)
				{
					try
					{
						if (!row.IsProc3SVMNull())
						{
							CutoffOneRowCalculation(ref tp, ref fp, ref fn, ref tn,
								curentCutoff, row, row.Proc3SVM);
						}
					}
					catch
					{
					}
				}
			}

			_paneReview.lblNegFalse.Text = tn.ToString();
			_paneReview.lblNegTrue.Text = fn.ToString();
			_paneReview.lblPosFalse.Text = fp.ToString();
			_paneReview.lblPosTrue.Text = tp.ToString();
			_paneReview.lblSensitivity.Text = "0 %";
			_paneReview.lblSpecificity.Text = "0 %";
			//_paneReview.lblAccuracy.Text = "0 %";
			_paneReview.lblPrelevance.Text = "0 %";
			if (!_paneReview.checkBox_ManualPrev.Checked)
				_paneReview.numericPrevalence.Value = 0.0M;
			_paneReview.lblPosPredValue.Text = "0 %";
			_paneReview.lblNegPredValue.Text = "0 %";

			double manual_prevalence = (_paneReview.checkBox_ManualPrev.Checked && _paneReview.numericPrevalence.Value > 0) ? (double)((double)_paneReview.numericPrevalence.Value / 100.0) : 0;
			var stat = new TwoByTwoStat(tp, fp, fn, tn, manual_prevalence);

			// Accuracy
			var acc = stat.accuracy;
			_paneReview.lblAccuracy.Text = acc.str_value;
			_paneReview.lblAccuracy95.Text = acc.str_interval;

			//Sensitivity
			var sensi = stat.sensitivity;
			_paneReview.lblSensitivity.Text = sensi.str_value;
			_paneReview.lblSensitivity95.Text = sensi.str_interval;

			//Specificity
			var speci = stat.specificity;
			_paneReview.lblSpecificity.Text = speci.str_value;
			_paneReview.lblSpecificity95.Text = speci.str_interval;

			//Prelevance
			var pre = stat.prevalence;
			_paneReview.lblPrelevance.Text = pre.str_value;
			_paneReview.numericPrevalence.Value = decimal.Parse(pre.str_numvalue);
			_paneReview.lblPrelevance95.Text = pre.str_interval;

			//PosPredValue
			var pospre = stat.positive_predictive_value;
			_paneReview.lblPosPredValue.Text = pospre.str_value;
			_paneReview.lblPosPredVal95.Text = pospre.str_interval;

			//NegPredValue
			var negpre = stat.negative_predictive_value;
			_paneReview.lblNegPredValue.Text = negpre.str_value;
			_paneReview.lblNegPredVal95.Text = negpre.str_interval;

			//Kappa
			var kappa = stat.kappa;
			_paneReview.lblKappa.Text = kappa.str_value;
			_paneReview.lblKappa95.Text = kappa.str_interval;
		}

		protected void CutoffOneRowCalculation(ref int tp, ref int fp, ref int fn, ref int tn, int curentCutoff, MainDataSet.ReviewMLDocumentsRow row, int rank)
		{
			//true pos - no of all documents that have positive category and score >= then cutoff
			bool isInPositive = (!row.IsCategoryNull()) && _positiveCategoriesIds.Contains(row.Category);
			bool isInNegative = !isInPositive;

			if (rank >= curentCutoff && isInPositive)
				tp++;
			else if (isInPositive)
				fn++;
			else if (rank < curentCutoff && isInNegative)
				tn++;
			else
				fp++;
		}

		protected void GetSavedCategories()
		{
			try
			{
				var json = this.Argument as string;
				if (!String.IsNullOrEmpty(json))
				{
					var parameters = JsonConvert.DeserializeObject<ReviewMLParameters>(json);

					_positiveCategoriesIds = parameters.PositiveCategories;

					_reviewMLIndex = 1;
					_filename = "1";
					_openedFilePath = parameters.PathToCSV;
					_openedFromStart = true;

					InitRankData();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected int LoadDynamicColumnFromFile()
		{
			var columnID = 0;

			try
			{
				if (File.Exists(_openedFilePath))
				{
					using (var streamReader = new StreamReader(_openedFilePath))
					{
						var firstLine = streamReader.ReadLine();
						if (!String.IsNullOrEmpty(firstLine) && !firstLine.Contains(","))
							columnID = Convert.ToInt32(firstLine);
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return columnID;
		}
        protected string LoadNoteTextColumnIndex()
        {
            var columnID = "NOTE_TEXT";

            try
            {
                var file = _openedFilePath.Replace(Path.GetFileName(_openedFilePath), "") + "NoteTextColumnName.csv";
                
                if (File.Exists(file))
                {
                    using (var streamReader = new StreamReader(file))
                    {
                        var firstLine = streamReader.ReadLine();
                        if (!String.IsNullOrEmpty(firstLine) && !firstLine.Contains(","))
                            columnID = firstLine;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }

            return columnID;
        }
        protected void CountRatios()
		{
			try
			{
				var selectedCategory = ((CategoryInfo) _paneReview.cmbCategories.SelectedItem).ID;

				//Count again the ratio
				double ratioItems = 0;
				for (int i = 0; i <= gridView.CurrentRow.Index; i++)
				{
					try
					{
						var value = gridView.Rows[i].Cells[4].Value;
						if (value is int && (int) value == selectedCategory)
							ratioItems++;
					}
					catch { }
				}
				if (ratioItems > 0)
					_paneReview.lblRatio.Text = (Math.Round((ratioItems / (gridView.CurrentRow.Index + 1)) * 100)).ToString() + " %";
				else
					_paneReview.lblRatio.Text = "0 %";

				//Count last 10
				ratioItems = 0;
				var loopCounter = gridView.CurrentRow.Index + 1;
				var loopEnd = (loopCounter - 10 < 0) ? 0 : loopCounter - 10;
				for (int i = loopCounter; i >= loopEnd; i--)
				{
					try
					{
						var value = gridView.Rows[i].Cells[4].Value;
						if (value is int && (int) value == selectedCategory)
							ratioItems++;
					}
					catch
					{
					}
				}
				if (ratioItems > 0)
				{
					double divider = 10.0;
					if (loopCounter - 10 < 0)
					{
						if (loopCounter == 0)
							divider = 1;
						else
							divider = loopCounter;
					}
					_paneReview.lblRatioLast10.Text = (Math.Round((ratioItems / divider) * 100)).ToString() + " %";
				}
				else
					_paneReview.lblRatioLast10.Text = "0 %";
			}
			catch
			{
			}
		}

		protected void MaximizeNotesView()
		{
			this.Panel1Collapsed = true;
		}

		protected void FastClassification()
		{
			if (!_fastClassificationOn)
			{
				_fastClassificationOn = true;

				this.Panel1Collapsed = true;

				paneNotes = (PaneNotes) _splitter.Panel2.Controls[0];

				//Create new splitter
				var splitter = new SplitContainer();
				splitter.Name = "Splitter2";
				splitter.BorderStyle = BorderStyle.Fixed3D;
				splitter.Orientation = Orientation.Vertical;
				splitter.Dock = DockStyle.Fill;
				splitter.BackColor = MainForm.ColorBackground;

				splitter.Width = this.Width;
				splitter.SplitterDistance = this.Width - 205;
				splitter.Panel1MinSize = 0;
				splitter.Panel2MinSize = 200;

				this.Panel2.Controls.Clear();
				this.Panel2.Controls.Add(splitter);

				//add notes to the left
				splitter.Panel1.Controls.Add(paneNotes);

				GroupBox groupBox = new GroupBox();
				groupBox.Dock = DockStyle.Fill;

				//Add categories to the right

				categoriesListBox = new ListBox();

				float fontSize = 14;
				categoriesListBox.Font = new System.Drawing.Font(DefaultFont.FontFamily.Name, fontSize);

				categoriesListBox.Width = 200;
				categoriesListBox.Anchor = AnchorStyles.Bottom;
				categoriesListBox.Anchor = AnchorStyles.Top;

				categoriesListBox.MouseDown += categoriesListBox_MouseDown;

				InitCategoriesListBox();

				groupBox.Controls.Add(categoriesListBox);
				splitter.Panel2.Controls.Add(groupBox);
			}
		}

		protected void InitCategoriesListBox()
		{
			try
			{
				categoriesListBox.Items.Clear();

				foreach (var cat in GetCategories())
				{
					categoriesListBox.Items.Add(cat);
				}

				categoriesListBox.ValueMember = "ID";
				categoriesListBox.DisplayMember = "Title";

				//Count hight for position
				int sizeOfOneRow = 28;
				categoriesListBox.Height = categoriesListBox.Items.Count * sizeOfOneRow;

				if (categoriesListBox.Height > this.Height)
					categoriesListBox.Height = this.Height;
				else
					categoriesListBox.Top = (this.Height - categoriesListBox.Height) / 2;
			}
			catch
			{
			}
		}

		protected void SetCategoryFastReview()
		{
			var rowView = _views.MainForm.sourceReviewMLDocumentsNew.Current as DataRowView;
			if (rowView != null)
			{
				var reviewDocumentRow = (MainDataSet.ReviewMLDocumentsRow)rowView.Row;

				var documentRow = _views.MainForm.datasetMain.Documents.FindByPrimaryKey(reviewDocumentRow.ED_ENC_NUM);

				//Update current row and go to the next one
				var selectedCategory = (CategoryInfo) categoriesListBox.SelectedItem;
				reviewDocumentRow["Category"] = selectedCategory.ID;

				if (_dynamicCategoryColumnID > 0)
				{
					var dynamicColumn = _views.MainForm.datasetMain.DynamicColumns.FirstOrDefault(x => x.ID == _dynamicCategoryColumnID);
					if (dynamicColumn != null)
					{
						documentRow[dynamicColumn.Title] = selectedCategory.ID;

						_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey(dynamicColumn.Title, selectedCategory.Title, reviewDocumentRow.ED_ENC_NUM);
					}
				}
				else
				{
					documentRow.Category = selectedCategory.ID;

					_views.MainForm.adapterDocuments.SqlSetColumnValueByPrimaryKey("Category", selectedCategory.Title, reviewDocumentRow.ED_ENC_NUM);
				}

				reviewDocumentRow.AcceptChanges();
				documentRow.AcceptChanges();

				_views.MainForm.sourceReviewMLDocumentsNew.MoveNext();
			}
		}

		protected void ResetView()
		{
			if (_fastClassificationOn)
			{
				_fastClassificationOn = false;

				//Change _panel 2
				if (paneNotes != null)
				{
					this.Panel1Collapsed = false;
					this.Panel2.Controls.Clear();
					_splitter.Panel2.Controls.Add(paneNotes);
					this.Panel2.Controls.Add(_splitter);
				}
			}
		}

		protected void StartTimer()
		{
			if (_timer == null)
			{
				_timer = new Timer();
				_timer.Interval = 300;
				_timer.Tick += _timer_Tick;

				_timer.Start();
			}
		}

		protected void HideMLScoreColumn()
		{
			try
			{
				var views = _views.GetOpenedViewsByType<ViewDefault>();
				views.ForEach(x => x.PaneDocuments.ShowMLScoreColumns(false));
			}
			catch
			{
			}
		}

		#endregion

		#region Implementation: init rank data

		protected void InitRankData()
		{
			try
			{
				HideMLScoreColumn();

				///////////////////////////////////////////////////////////////////////////////

				BindingSource bindingSource;

				if (_dataInitialized)
				{
					//Change the dataGrid
					gridView = _paneDocumentsReviewML.GetDocumentsGrid();
					bindingSource = (BindingSource) gridView.DataSource;

					bindingSource.RaiseListChangedEvents = false;
					bindingSource.RemoveFilter();
					bindingSource.SuspendBinding();
				}

                ///////////////////////////////////////////////////////////////////////////////

                /*if (_reviewMLIndex == 1)
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;*/

                _noteTextColumnName = LoadNoteTextColumnIndex();

                _paneNotes.ReviewNoteTextColumn = _noteTextColumnName ;

                var nColumnindex = Int32.Parse(_noteTextColumnName.Replace("NOTE_TEXT", "0"));

                _paneDocumentsReviewML.NoteColumnIndex = nColumnindex;

                var columnID = LoadDynamicColumnFromFile();
				BeforeSetDynamicCategoryColumn(columnID);

				var formProgress = new FormGenericProgress("Loading values...", DoInitRankData, null, true)
				                   {
					                   CancellationEnabled = false
				                   };

				formProgress.ShowDialog();

				///////////////////////////////////////////////////////////////////////////////

				_dataInitialized = true;

				AfterSetDynamicCategoryColumn();

				gridView = _paneDocumentsReviewML.GetDocumentsGrid();

				bindingSource = (BindingSource) gridView.DataSource;
				bindingSource.RaiseListChangedEvents = true;
				bindingSource.ResumeBinding();

				FilterBindingSource();
				StartTimer();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected bool DoInitRankData(BackgroundWorker worker, object argument)
		{
			try
			{
				_views.MainForm.adapterReviewMLDocumentsNew.Fill();

				var reviewMLDocumentsTable = (MainDataSet.ReviewMLDocumentsDataTable)_views.MainForm.adapterReviewMLDocumentsNew.Table;

				//Delete previous Ranks
				if (_reviewMLIndex == 1)
				{
					var svmColumnIndex = GetSvmColumnIndex();
					foreach (MainDataSet.ReviewMLDocumentsRow row in reviewMLDocumentsTable.Rows)
					{
						try
						{
							if (!row.IsNull(svmColumnIndex))
							{
								row[svmColumnIndex] = DBNull.Value;
								row.AcceptChanges();
							}
						}
						catch (Exception ex)
						{
							worker.ReportProgress(-1, ex.Message);
						}
					}
					InitProcess1Data(reviewMLDocumentsTable, true, _openedFilePath, worker);
				}
				else if (_reviewMLIndex == 2)
				{
					foreach (MainDataSet.ReviewMLDocumentsRow row in reviewMLDocumentsTable.Rows)
					{
						try
						{
							if (!row.IsRankNull())
							{
								row.SetRankNull();
								row.AcceptChanges();
							}
						}
						catch (Exception ex)
						{
							worker.ReportProgress(-1, ex.Message);
						}
					}
					InitProcess2Data(reviewMLDocumentsTable, _openedFilePath, worker);
				}
				else if (_reviewMLIndex == 3)
				{
					foreach (MainDataSet.ReviewMLDocumentsRow row in reviewMLDocumentsTable.Rows)
					{
						try
						{
							if (!row.IsProc3SVMNull())
							{
								row.SetProc3SVMNull();
								row.AcceptChanges();
							}
						}
						catch (Exception ex)
						{
							worker.ReportProgress(-1, ex.Message);
						}
					}
					InitProcess1Data(reviewMLDocumentsTable, false, _openedFilePath, worker);
				}

				return true;
			}
			catch (Exception ex)
			{
				worker.ReportProgress(-1, ex.Message);
			}

			return false;
		}

		protected void InitProcess1Data(MainDataSet.ReviewMLDocumentsDataTable reviewMLDocumentsTable, bool isFirstProcess, string filePath, BackgroundWorker worker)
		{
			if (Path.GetFileNameWithoutExtension(filePath).EndsWith("_all"))
				this.Panel1Collapsed = true;

			//Read ranks.dat file and take values from there
			using (var reader = new StreamReader(filePath))
			{
				//Load data
				double docID = 0;
				string line = "";
				if (isFirstProcess)
				{
					var scores = new List<MLScore>();

					var lineIndex = 0;
					while ((line = reader.ReadLine()) != null)
					{
						lineIndex++;

						if (lineIndex == 1 && !line.Contains(","))
						{
							continue;
						}

						var dataFromLine = line.Split(',');

						try
						{
							int score;

							if (Double.TryParse(dataFromLine[0], out docID)
							    && Int32.TryParse(dataFromLine[1], out score))
							{
								scores.Add(new MLScore
								           {
									           DocID = docID,
									           Score = score
								           });

								///////////////////////////////////////////////////////////////////////////////

								if (score < _cutoffMin)
									_cutoffMin = score;
								if (score > _cutoffMax)
									_cutoffMax = score;
							}
						}
						catch (Exception ex)
						{
							worker.ReportProgress(-1, ex.Message);
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					if (scores.Any())
					{
						_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;

						///////////////////////////////////////////////////////////////////////////////

						var joinReviewDocuments = from rowDoc in reviewMLDocumentsTable
												  join s in scores
							                          on rowDoc.ED_ENC_NUM equals s.DocID
							                          into tmp
						                          from t in tmp.DefaultIfEmpty()
						                          select new { rowDoc, score = t };

						var svmColumnIndex = GetSvmColumnIndex();

						foreach (var item in joinReviewDocuments)
						{
							if (item.score != null)
								item.rowDoc[svmColumnIndex] = item.score.Score;
							else
								item.rowDoc[svmColumnIndex] = DBNull.Value;

							item.rowDoc.AcceptChanges();
						}

						///////////////////////////////////////////////////////////////////////////////

						var joinDocuments = (from rowDoc in _views.MainForm.datasetMain.Documents
						                     join s in scores
							                     on rowDoc.ED_ENC_NUM equals s.DocID
						                     select new { rowDoc, score = s })
							.ToList();

						///////////////////////////////////////////////////////////////////////////////

						double progressMax = joinDocuments.Count;
						var progressValue = 0;

						///////////////////////////////////////////////////////////////////////////////

						SetSvmColumnNull();

						var cmd = CreateUpdateCommand();

						///////////////////////////////////////////////////////////////////////////////

						foreach (var item in joinDocuments)
						{
							item.rowDoc[svmColumnIndex] = item.score.Score;

							///////////////////////////////////////////////////////////////////////////////

							cmd.Parameters[0].Value = item.score.Score;
							cmd.Parameters[1].Value = item.rowDoc.ED_ENC_NUM;

							cmd.ExecuteNonQuery();

							item.rowDoc.AcceptChanges();

							///////////////////////////////////////////////////////////////////////////////

							progressValue++;
							if (progressValue % 50 == 0)
							{
								var progressPercentage = (int) ((progressValue / progressMax) * 100d);

								worker.ReportProgress(progressPercentage);
							}
						}

						///////////////////////////////////////////////////////////////////////////////

						_views.MainForm.adapterDocuments.Fill();

						///////////////////////////////////////////////////////////////////////////////

						_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
						_views.MainForm.sourceDocuments.ResetBindings(false);
					}
				}
				else
				{
					var lineIndex = 0;

					while ((line = reader.ReadLine()) != null)
					{
						lineIndex++;

						if (lineIndex == 1 && !line.Contains(","))
						{
							continue;
						}

						var dataFromLine = line.Split(',');
						try
						{
							//Get ed_inc
							docID = Double.Parse(dataFromLine[0]);
							
							//set rank
							var row = reviewMLDocumentsTable.FindByED_ENC_NUM(docID);
							if (row == null)
								continue;

							var rank = Int32.Parse(dataFromLine[1]);
							row.Proc3SVM = rank;

							row.AcceptChanges();

							if (rank < _cutoffMin)
								_cutoffMin = rank;
							if (rank > _cutoffMax)
								_cutoffMax = rank;
						}
						catch (Exception ex)
						{
							worker.ReportProgress(-1, ex.Message);
						}
					}
				}
			}

			_paneReview.numericCutoff.Minimum = _cutoffMin;
			_paneReview.numericCutoff.Maximum = _cutoffMax;
		}

		protected void InitProcess2Data(MainDataSet.ReviewMLDocumentsDataTable reviewMLDocumentsTable, string filePath, BackgroundWorker worker)
		{
			//Read ranks.dat file and take values from there
			using (var reader = new StreamReader(filePath))
			{
				//Load data
				double ed_inc_number = 0;

				string line = "";
				int rank = 1;

				var lineIndex = 0;

				while ((line = reader.ReadLine()) != null)
				{
					lineIndex++;

					if (lineIndex == 1 && !line.Contains(","))
					{
						continue;
					}

					try
					{
						//Get ed_inc
						ed_inc_number = Double.Parse(line);
						//set rank
						var row = reviewMLDocumentsTable.FindByED_ENC_NUM(ed_inc_number);
						if (row == null)
							continue;

						row.Rank = rank;

						row.AcceptChanges();

						rank++;
					}
					catch (Exception ex)
					{
						worker.ReportProgress(-1, ex.Message);
					}
				}
			}
		}

		protected void SetSvmColumnNull()
		{
			_views.MainForm.adapterReviewMLDocumentsNew.SqlClearColumnValues(_svmColumnInfo.Name, null, null, ExpressionType.Equal);
		}

		protected OleDbCommand CreateUpdateCommand()
		{
			var connection = _views.MainForm.adapterDocuments.Connection;
			if (connection.State != ConnectionState.Open)
				connection.Open();

			var query = "UPDATE Documents SET [" + _svmColumnInfo.Name + "] = @Proc1SVM WHERE ED_ENC_NUM = @ED_ENC_NUM";

			var cmd = new OleDbCommand(query, connection);

			cmd.Parameters.AddWithValue("@Proc1SVM", 0)
			   .DbType = DbType.Int32;
			cmd.Parameters.AddWithValue("@ED_ENC_NUM", 0)
			   .DbType = DbType.Double;

			cmd.Prepare();

			return cmd;
		}

		protected void BeforeSetDynamicCategoryColumn(int columnID)
		{
			if (_paneDocumentsReviewML != null)
				_paneDocumentsReviewML.DisableDynamicColumns();

			_dynamicCategoryColumnID = columnID;
			_svmColumnInfo = SvmColumnService.GetSvmColumnInfo(_views, _dynamicCategoryColumnID, true);
			if (_svmColumnInfo == null)
				throw new Exception("Failed to add SVM column");
		}

		protected void AfterSetDynamicCategoryColumn()
		{
			if (_paneReview != null)
				InitCategoriesToReviewStatistic();

			InitCategoriesListBox();
			LoadDynamicCategories();
			LoadPositiveCategories();

			///////////////////////////////////////////////////////////////////////////////

			if (_paneDocumentsReviewML != null)
				_paneDocumentsReviewML.SetDynamicCategoryColumn(_dynamicCategoryColumnID, _svmColumnInfo.Name);
		}

		protected void SetDynamicCategoryColumn(int columnID)
		{
			BeforeSetDynamicCategoryColumn(columnID);
			AfterSetDynamicCategoryColumn();
		}

		protected int GetSvmColumnIndex()
		{
			return _views.MainForm.adapterReviewMLDocumentsNew.Table.Columns[_svmColumnInfo.Name].Ordinal;
		}

		protected void LoadPositiveCategories()
		{
			var json = BrowserManager.GetViewData(_views, "Binary Classification");
			var binaryClassificationViewModel = BinaryClassificationViewModel.FromJSON(json);
			if (binaryClassificationViewModel != null)
			{
				var columnInfo = binaryClassificationViewModel.columns.FirstOrDefault(x => x.ID == _dynamicCategoryColumnID);
				if (columnInfo != null && columnInfo.positiveCategories != null)
					_positiveCategoriesIds = columnInfo.positiveCategories.Select(x => x.ID).ToList();
				else
					MessageBox.Show("Failed to load positive categories", _views.MainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		#endregion
	}
}