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
using System.Windows.Forms;

using Newtonsoft.Json;

using RegScoreCalc.Code;
using RegScoreCalc.Views.Models;

namespace RegScoreCalc
{
	public class ViewReviewMLOld : View
	{
		#region Fields

		bool dataInitialized = true;

		public int cutoffMin = 100;
		public int cutoffMax = -100;

		List<int> positiveCategoriesIds = new List<int>();

		int pom = 5;
		bool FastClassificationOn;

		private int ReviewMLIndex;

		protected PaneDocumentsReviewMLOld _paneDocumentsReviewMlOld;
		protected PaneNotes _paneNotes;
		Timer _timer;

		private System.Timers.Timer ClickTimer;
		private int ClickCounter;

		PaneNotes paneNotes;
		ListBox categoriesListBox;

		DataGridView gridView;
		SplitContainer _splitter;
		PaneReviewStatistics _paneReview;
		BindingSource bs;

		string openedFilePath = "";
		string filename = "";
		bool openedFromStart;

		#endregion

		#region Ctors

		public ViewReviewMLOld(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
			openedFromStart = _views.MainForm.ReviewMLOpenedFromStart;

			ClickTimer = new System.Timers.Timer();
			ClickTimer.Interval = 300;
			ClickTimer.Elapsed += ClickTimer_Elapsed;

			_paneReview.numericCutoff.ValueChanged += numericCutoff_ValueChanged;
			preCalculatedPrevelance = 0;

			GetSavedCategories();
		}

		#endregion

		#region Events

		private void OnDataModified(object sender, EventArgs e)
		{
			//UpdateView();
			UpdateTextNotePane();
		}

		private void numericCutoff_ValueChanged(object sender, EventArgs e)
		{
			RecalculateCutoff();
		}

		private void _timer_Tick(object sender, EventArgs e)
		{
			//Sort by Rank

			if (pom > 0)
			{
				pom--;
				gridView = _paneDocumentsReviewMlOld.GetDocumentsGrid();

				// string filename = Properties.Settings.Default.ProcessName;

				// if (filename[0] == '2')
				if (ReviewMLIndex == 2)
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
				else if (ReviewMLIndex == 1)
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
				else if (ReviewMLIndex == 3)
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
				pom = 5;
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

					var text = _views.DocumentsService.GetDocumentText(rowReviewDocuments.ED_ENC_NUM);
					_paneNotes.InvokeUpdateTextOnMainThread(text, true);
				}
				else
				{
					DataRowView rowview = (DataRowView) gridView.Rows[0].DataBoundItem;
					MainDataSet.ReviewMLDocumentsRow rowReviewDocuments = (MainDataSet.ReviewMLDocumentsRow) rowview.Row;

					var text = _views.DocumentsService.GetDocumentText(rowReviewDocuments.ED_ENC_NUM);
					_paneNotes.InvokeUpdateTextOnMainThread(text, true);
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
			InitCategoriesToReviewStatistic(_paneReview);

			InitCategories();
		}

		private void categoriesListBox_MouseDown(object sender, MouseEventArgs e)
		{
			ClickTimer.Stop();
			ClickCounter++;
			ClickTimer.Start();
		}

		private void ClickTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			ClickTimer.Stop();

			SetCategoryFastReview(categoriesListBox);

			CountRatios();

			ClickCounter = 0;
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
			//opf.InitialDirectory = _views.MainForm.DatabaseFolder;
			if (opf.ShowDialog() == DialogResult.OK)
			{
				ReviewMLIndex = 1;
				filename = "1";
				openedFilePath = opf.FileName;

				InitRankData();
			}
		}

		private void btnOpenSVM2_Click(object sender, EventArgs e)
		{
			OpenFileDialog opf = new OpenFileDialog();
			opf.Filter = "DAT Files|*.dat";
			opf.Multiselect = false;
			//opf.InitialDirectory = _views.MainForm.DatabaseFolder;
			if (opf.ShowDialog() == DialogResult.OK)
			{
				ReviewMLIndex = 2;
				filename = "2";
				openedFilePath = opf.FileName;

				InitRankData();
			}
		}

		private void btnOpenSVM3_Click(object sender, EventArgs e)
		{
			OpenFileDialog opf = new OpenFileDialog();
			opf.Filter = "CSV Files|*.csv";
			opf.Multiselect = false;
			//opf.InitialDirectory = _views.MainForm.DatabaseFolder;
			if (opf.ShowDialog() == DialogResult.OK)
			{
				ReviewMLIndex = 3;
				filename = "3";
				openedFilePath = opf.FileName;

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

		protected void InitCategoriesToReviewStatistic(PaneReviewStatistics _paneReview)
		{
			//Add categories to cmbCategories
			DataTable dt = ((MainDataSet) _views.MainForm.sourceCategories.DataSource).Categories.DataSet.Tables["Categories"];

			_paneReview.cmbCategories.Items.Clear();
			foreach (DataRow item in dt.Rows)
			{
				var id = (int) item[0];
				var name = item[1].ToString();
				_paneReview.cmbCategories.Items.Add(new CategoryListItem(id, name));
			}
			_paneReview.cmbCategories.ValueMember = "ID";
			_paneReview.cmbCategories.DisplayMember = "Category";
		}

		#endregion

		#region Overrides

		protected override void InitViewPanes(RibbonTab tab)
		{
			_views.MainForm.LoadReviewMLDocuments();

			openedFromStart = _views.MainForm.ReviewMLOpenedFromStart;
			//Check what type of process is file
			filename = Properties.Settings.Default.ProcessName;
			ReviewMLIndex = 1;
			if (!String.IsNullOrEmpty(filename))
			{
				if (filename[0] == '1')
					ReviewMLIndex = 1;
				else if (filename[1] == '2')
					ReviewMLIndex = 2;
				else
					ReviewMLIndex = 3;
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

			InitCategoriesToReviewStatistic(_paneReview);

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

			_paneDocumentsReviewMlOld = new PaneDocumentsReviewMLOld();
			_paneDocumentsReviewMlOld.InitPane(_views, this, _splitter.Panel1, tab);
			_paneDocumentsReviewMlOld._eventDataModified += new EventHandler(OnDataModified);

			////Remove not needed button
			//_paneDocumentsReviewMlOld._panel.Items.Remove(_paneDocumentsReviewMlOld._btnSortScores);

			gridView = _paneDocumentsReviewMlOld.GetDocumentsGrid();
			gridView.CellClick += gridView_CellClick;

			//gridView.Columns["columnNo"].Visible = false;
			//gridView.Columns["dataGridViewTextBoxColumn9"].Visible = false;

			//gridView.Columns["columnRank"].Visible = true;
			//gridView.Columns["columnProc1SVM"].Visible = true;
			//gridView.Columns["columnProc3SVM"].Visible = true;

			//string filename = Properties.Settings.Default.ProcessName;
			//try
			//{
			//    ReviewMLIndex = Int32.Parse(filename[0].ToString());
			//}
			//catch
			//{
			//    ReviewMLIndex = 0;
			//}

			//if (ReviewMLIndex == 2)
			//{
			//    ranksFilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer", "ranks.dat");
			//}

			if (openedFromStart)
			{
				//Do not load data
				dataInitialized = false;
			}
			else
			{
				//_paneDocumentsReviewMlOld.GetDocumentsGrid().DataSource = _views.MainForm.sourceReviewMLDocuments;

				var ranksFilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer", "output.csv");
				if (ReviewMLIndex == 2)
					ranksFilePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer", "ranks.dat");

				if (File.Exists(ranksFilePath))
				{
					//Init of the data from the ranks.dat
					InitRankData();

					//Delete ranks.dat
					//File.Delete(ranksFilePath);

					//Remove not needed rows
					FilterBindingSource();
				}
			}

			_splitter.Panel1.Controls.Add(_paneDocumentsReviewMlOld);
			_paneDocumentsReviewMlOld.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			_paneNotes = new PaneNotes();
			_paneNotes.InitPane(_views, this, _splitter.Panel2, tab);
			_paneNotes.ShowToolbar = false;

			//Remove not needed button
			//_paneNotes.panel.Items.Remove(_paneNotes.btnWriteNotes);

			_paneNotes._eventDataModified += new EventHandler(OnDataModified);

			_splitter.Panel2.Controls.Add(_paneNotes);
			_paneNotes.ShowPane();

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
			_paneDocumentsReviewMlOld.DestroyPane();
			_paneNotes.DestroyPane();

			base.DestroyView();
		}

		public override bool OnHotkey(string code)
		{
			var handled = _paneDocumentsReviewMlOld.OnHotkey(code);
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
			if (_paneDocumentsReviewMlOld != null)
			{
				_paneDocumentsReviewMlOld.UpdatePane();
				try
				{
					if (dataInitialized)
					{
						FilterBindingSource();

						//   StartTimer();
						UpdateTextNotePane();
					}
					else
					{
						//Hide all data
						gridView = _paneDocumentsReviewMlOld.GetDocumentsGrid();

						bs = ((BindingSource) gridView.DataSource);
						bs.Filter = "ED_ENC_NUM is null";
					}
				}
				catch
				{
				}
			}
			//if (_paneNotes != null)
			//    _paneNotes.UpdatePane();
		}

		protected bool messageShown;

		private void FilterBindingSource()
		{
			gridView = _paneDocumentsReviewMlOld.GetDocumentsGrid();

			bs = ((BindingSource) gridView.DataSource);
			//  bs.RemoveFilter();

			if (ReviewMLIndex == 1)
			{
				bs.Filter = "Proc1SVM is not null";
				if (gridView.Rows.Count == 0)
				{
					if (!messageShown)
					{
						MessageBox.Show(this, "Proc1SVM is not initialized, please do process at server to get ranks!", "Proc1SVM don't exist.");
						messageShown = true;
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
			else if (ReviewMLIndex == 2)
			{
				bs.Filter = "Rank is not null";
				if (gridView.Rows.Count == 0)
				{
					if (!messageShown)
					{
						MessageBox.Show(this, "Ranks not initialized, please do process at server to get ranks!", "Ranks don't exist.");
						messageShown = true;
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
			else if (ReviewMLIndex == 3)
			{
				bs.Filter = "Proc3SVM is not null";
				if (gridView.Rows.Count == 0)
				{
					if (!messageShown)
					{
						MessageBox.Show(this, "Proc3SVM is not initialized, please do process at server to get ranks!", "Proc3SVM don't exist.");
						messageShown = true;
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

		#endregion

		#region Implementation

		private int preCalculatedPrevelance { get; set; }

		private void RecalculateCutoff()
		{
			//Recalculate cutoff
			//int rank;

			int tp = 0;
			int fp = 0;
			int fn = 0;
			int tn = 0;

			int curentCutoff = (int) _paneReview.numericCutoff.Value;

			//BindingSource bs = (BindingSource)gridView.DataSource;
			MainDataSet mds = _views.MainForm.datasetMain;

			// if (filename[0] == '1')
			if (ReviewMLIndex == 1)
			{
				//foreach (RegScoreCalc.MainDataSet.DocumentsRow row in mds.Documents.Rows)
				foreach (RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row in mds.ReviewMLDocuments.Rows)
				{
					try
					{
						if (!row.IsProc1SVMNull())
						{
							CutoffOneRowCalculation(ref tp, ref fp, ref fn, ref tn,
								curentCutoff, row, row.Proc1SVM);
						}
					}
					catch
					{
					}
				}
			}
			//else if (filename[0] == '3')
			else if (ReviewMLIndex == 3)
			{
				// foreach (RegScoreCalc.MainDataSet.DocumentsRow row in mds.Documents.Rows)
				foreach (RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row in mds.ReviewMLDocuments.Rows)
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
				_paneReview.numericPrevalence.Value = 0;
			_paneReview.lblPosPredValue.Text = "0 %";
			_paneReview.lblNegPredValue.Text = "0 %";

			double manual_prevalence = (_paneReview.checkBox_ManualPrev.Checked && _paneReview.numericPrevalence.Value > 0) ? (double) ((double)_paneReview.numericPrevalence.Value / 100.0) : 0;
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

		private void CutoffOneRowCalculation(ref int tp, ref int fp, ref int fn, ref int tn, int curentCutoff, RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row, int Rank)
		{
			int rank = Rank;

			//true pos - no of all documents that have positive category and score >= then cutoff
			bool isInPositive = (!row.IsCategoryNull()) && positiveCategoriesIds.Contains(row.Category);
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

				if (String.IsNullOrEmpty(json))
				{
					json = BrowserManager.GetViewData(_views, "Binary Classification");
					var parameters = JsonConvert.DeserializeObject<ReviewMLParameters>(json);

					positiveCategoriesIds = parameters.PositiveCategories;
				}
				else
				{
					var parameters = JsonConvert.DeserializeObject<ReviewMLParameters>(json);

					positiveCategoriesIds = parameters.PositiveCategories;

					ReviewMLIndex = 1;
					filename = "1";
					openedFilePath = parameters.PathToCSV;
					openedFromStart = true;

					InitRankData();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void CountRatios()
		{
			try
			{
				var selectedCategory = ((CategoryListItem) _paneReview.cmbCategories.SelectedItem).ID;

				//Count again the ratio
				double ratioItems = 0;
				for (int i = 0; i <= gridView.CurrentRow.Index; i++)
				{
					try
					{
						if ((int) gridView.Rows[i].Cells[4].Value == selectedCategory)
							ratioItems++;
					}
					catch
					{
					}
				}
				if (ratioItems > 0)
					_paneReview.lblRatio.Text = (Math.Round((ratioItems / (gridView.CurrentRow.Index + 1)) * 100)).ToString() + " %";
				else
					_paneReview.lblRatio.Text = "0 %";

				//Count last 10
				//if (gridView.CurrentRow.Index < 10)
				//{
				//    _paneReview.lblRatioLast10.Text = _paneReview.lblRatio.Text;
				//}
				//else
				//{
				ratioItems = 0;
				var loopCounter = gridView.CurrentRow.Index + 1;
				var loopEnd = (loopCounter - 10 < 0) ? 0 : loopCounter - 10;
				for (int i = loopCounter; i >= loopEnd; i--)
				{
					try
					{
						if ((int) gridView.Rows[i].Cells[4].Value == selectedCategory)
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
				//}
			}
			catch
			{
			}
		}

		protected void MaximizeNotesView()
		{
			this.Panel1Collapsed = true;
		}

		private void FastClassification()
		{
			if (!FastClassificationOn)
			{
				FastClassificationOn = true;

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

				////add notes to the left
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

				InitCategories();

				groupBox.Controls.Add(categoriesListBox);
				splitter.Panel2.Controls.Add(groupBox);
			}
		}

		private void InitCategories()
		{
			try
			{
				DataTable dt = ((MainDataSet) _views.MainForm.sourceCategories.DataSource).Categories.DataSet.Tables["Categories"];
				categoriesListBox.Items.Clear();
				foreach (DataRow item in dt.Rows)
				{
					var id = (int) item[0];
					var name = item[1].ToString();
					categoriesListBox.Items.Add(new CategoryListItem(id, name));
				}
				categoriesListBox.ValueMember = "ID";
				categoriesListBox.DisplayMember = "Category";

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

		private void SetCategoryFastReview(object sender)
		{
			ListBox listbox = (ListBox) sender;
			var selectedItem = (CategoryListItem) listbox.SelectedItem;
			var categoryId = selectedItem.ID;

			DataRowView row = _views.MainForm.sourceReviewMLDocuments.Current as DataRowView;
			if (row != null)
			{
				MainDataSet.ReviewMLDocumentsRow dataRow = row.Row as MainDataSet.ReviewMLDocumentsRow;

				MainDataSet.DocumentsRow dataRowDocuments = _views.MainForm.datasetMain.Documents.FindByPrimaryKey(dataRow.ED_ENC_NUM);

				//Update current row and go to the next one
				dataRow["Category"] = categoryId;
				dataRowDocuments["Category"] = categoryId;

				_paneDocumentsReviewMlOld.UpdateDocumentsRow(dataRowDocuments);

				//Go to next document
				//_views.MainForm.sourceDocuments.MoveNext();

				//var grid = _paneDocumentsReviewMlOld.GetDocumentsGrid();
				//int nRow = grid.CurrentCell.RowIndex;
				//if (nRow < grid.RowCount)
				//{
				//    grid.Rows[nRow].Selected = false;
				//    grid.Rows[++nRow].Selected = true;
				//}

				_views.MainForm.sourceReviewMLDocuments.MoveNext();
			}
		}

		protected void ResetView()
		{
			if (FastClassificationOn)
			{
				FastClassificationOn = false;

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

		private void StartTimer()
		{
			if (_timer == null)
			{
				_timer = new Timer();
				_timer.Interval = 300;
				_timer.Tick += _timer_Tick;

				_timer.Start();
			}
		}

		#endregion

		#region Implementation: init rank data

		private void InitRankData()
		{
			try
			{
				HideMLScoreColumn();

				///////////////////////////////////////////////////////////////////////////////

				BindingSource bindingSource;

				if (dataInitialized)
				{
					//Change the dataGrid
					gridView = _paneDocumentsReviewMlOld.GetDocumentsGrid();
					bindingSource = (BindingSource) gridView.DataSource;

					bindingSource.RaiseListChangedEvents = false;
					bindingSource.RemoveFilter();
					bindingSource.SuspendBinding();
				}

				///////////////////////////////////////////////////////////////////////////////

				/*if (ReviewMLIndex == 1)
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;*/

				var formProgress = new FormGenericProgress("Loading values...", DoInitRankData, null, true)
				                   {
					                   CancellationEnabled = false
				                   };

				formProgress.ShowDialog();

				///////////////////////////////////////////////////////////////////////////////

				dataInitialized = true;

				gridView = _paneDocumentsReviewMlOld.GetDocumentsGrid();

				bindingSource = (BindingSource) gridView.DataSource;
				bindingSource.RaiseListChangedEvents = true;
				bindingSource.ResumeBinding();

				FilterBindingSource();
				StartTimer();

				///////////////////////////////////////////////////////////////////////////////

				/*if (ReviewMLIndex == 1)
				{
					_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;
					_views.MainForm.sourceDocuments.ResetBindings(false);
				}*/
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
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

		protected bool DoInitRankData(BackgroundWorker worker, object argument)
		{
			try
			{
				MainDataSet mds = _views.MainForm.datasetMain;

				//CrateDataTable();
				_views.MainForm.adapterReviewMLDocuments.Fill(_views.MainForm.datasetMain.ReviewMLDocuments);

				//Delete previous Ranks

				//if (filename[0] == '1')
				if (ReviewMLIndex == 1)
				{
					//foreach (RegScoreCalc.MainDataSet.DocumentsRow row in mds.Documents.Rows)
					foreach (RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row in mds.ReviewMLDocuments.Rows)
					{
						try
						{
							if (!row.IsProc1SVMNull())
							{
								row.SetProc1SVMNull();
								row.AcceptChanges();
							}
						}
						catch (Exception ex)
						{
							worker.ReportProgress(-1, ex.Message);
						}
					}
					InitProcess1Data(mds, true, openedFilePath, worker);
				}
				//else if (filename[0] == '2')
				else if (ReviewMLIndex == 2)
				{
					//  foreach (RegScoreCalc.MainDataSet.DocumentsRow row in mds.Documents.Rows)
					foreach (RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row in mds.ReviewMLDocuments.Rows)
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
					InitProcess2Data(mds, openedFilePath, worker);
				}
				//else if (filename[0] == '3')
				else if (ReviewMLIndex == 3)
				{
					// foreach (RegScoreCalc.MainDataSet.DocumentsRow row in mds.Documents.Rows)
					foreach (RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row in mds.ReviewMLDocuments.Rows)
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
					InitProcess1Data(mds, false, openedFilePath, worker);
				}

				return true;
			}
			catch (Exception ex)
			{
				worker.ReportProgress(-1, ex.Message);
			}

			return false;
		}

		protected void InitProcess1Data(MainDataSet mds, bool isFirstProcess, string filePath, BackgroundWorker worker)
		{
			if (Path.GetFileNameWithoutExtension(filePath).EndsWith("_all"))
				this.Panel1Collapsed = true;

			//Read ranks.dat file and take values from there
			//var filePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer", "output.csv");
			using (StreamReader reader = new StreamReader(filePath))
			{
				//Load data
				double docID = 0;
				int rank = 0;
				string line = "";
				if (isFirstProcess)
				{
					var scores = new List<MLScore>();

					while ((line = reader.ReadLine()) != null)
					{
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

								if (score < cutoffMin)
									cutoffMin = score;
								if (score > cutoffMax)
									cutoffMax = score;
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

						var joinReviewDocuments = from rowDoc in mds.ReviewMLDocuments
						                          join s in scores
							                          on rowDoc.ED_ENC_NUM equals s.DocID
							                          into tmp
						                          from t in tmp.DefaultIfEmpty()
						                          select new { rowDoc, score = t };

						foreach (var item in joinReviewDocuments)
						{
							if (item.score != null)
								item.rowDoc.Proc1SVM = item.score.Score;
							else
								item.rowDoc.SetProc1SVMNull();

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

						SetNullProc1SVM();

						var cmd = CreateUpdateCommand();

						///////////////////////////////////////////////////////////////////////////////

						foreach (var item in joinDocuments)
						{
							item.rowDoc.Proc1SVM = item.score.Score;

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
					while ((line = reader.ReadLine()) != null)
					{
						var dataFromLine = line.Split(',');
						try
						{
							//Get ed_inc
							docID = Double.Parse(dataFromLine[0]);
							//set rank
							//RegScoreCalc.MainDataSet.DocumentsRow row = mds.Documents.FindByED_ENC_NUM(ed_inc_number);
							//rank = Int32.Parse(dataFromLine[1]);
							//row.Proc3SVM = rank;

							RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row = mds.ReviewMLDocuments.FindByED_ENC_NUM(docID);
							rank = Int32.Parse(dataFromLine[1]);
							row.Proc3SVM = rank;

							row.AcceptChanges();

							if (rank < cutoffMin)
								cutoffMin = rank;
							if (rank > cutoffMax)
								cutoffMax = rank;
						}
						catch (Exception ex)
						{
							worker.ReportProgress(-1, ex.Message);
						}
					}
				}
			}

			_paneReview.numericCutoff.Minimum = cutoffMin;
			_paneReview.numericCutoff.Maximum = cutoffMax;
		}

		protected OleDbCommand CreateUpdateCommand()
		{
			var connection = _views.MainForm.adapterDocuments.Connection;
			if (connection.State != ConnectionState.Open)
				connection.Open();

			var query = "UPDATE Documents SET Proc1SVM = @Proc1SVM WHERE ED_ENC_NUM = @ED_ENC_NUM";

			var cmd = new OleDbCommand(query, connection);

			cmd.Parameters.AddWithValue("@Proc1SVM", 0)
			   .DbType = DbType.Int32;
			cmd.Parameters.AddWithValue("@ED_ENC_NUM", 0)
			   .DbType = DbType.Double;

			cmd.Prepare();

			return cmd;
		}

		protected void SetNullProc1SVM()
		{
			var connection = _views.MainForm.adapterDocuments.Connection;
			if (connection.State != ConnectionState.Open)
				connection.Open();

			var query = "UPDATE Documents SET Proc1SVM = NULL";

			var cmd = new OleDbCommand(query, connection);

			cmd.ExecuteNonQuery();
		}

		protected void InitProcess2Data(MainDataSet mds, string filePath, BackgroundWorker worker)
		{
			//Read ranks.dat file and take values from there
			//var filePath = Path.Combine(Directory.GetCurrentDirectory(), "DataFromServer", "ranks.dat");
			using (var reader = new StreamReader(filePath))
			{
				//Load data
				double ed_inc_number = 0;

				string line = "";
				int rank = 1;
				while ((line = reader.ReadLine()) != null)
				{
					try
					{
						//Get ed_inc
						ed_inc_number = Double.Parse(line);
						//set rank
						//  RegScoreCalc.MainDataSet.DocumentsRow row = mds.Documents.FindByED_ENC_NUM(ed_inc_number);
						RegScoreCalc.MainDataSet.ReviewMLDocumentsRow row = mds.ReviewMLDocuments.FindByED_ENC_NUM(ed_inc_number);

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

		#endregion
	}
}