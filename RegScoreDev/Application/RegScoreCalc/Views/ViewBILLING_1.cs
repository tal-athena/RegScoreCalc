using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using RegScoreCalc.Forms;
using System.Windows.Forms.Integration;
using CustomTreeView;
using System.Data;
using RegScoreCalc.Data;
using System.Windows.Media;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

using FastColoredTextBoxNS;

using Microsoft.Office.Interop.Excel;

using RegExpLib.Processing;

using Range = Microsoft.Office.Interop.Excel.Range;

namespace RegScoreCalc
{
	using Font = System.Drawing.Font;
	using Rectangle = System.Drawing.Rectangle;
	using Color = System.Drawing.Color;

	public class ViewBILLING_1 : View
	{
		#region Fields

		protected SplitContainer _splitter;
		protected SplitContainer _splitterRightSide;
		protected SplitContainer _splitterLeftSide;
		protected SplitContainer _splitterRightTree;

		protected Panel notesTreeViewPanel;

		//Custom tree views
		private CustomTreeView.TreeView _notesTreeView;
		public RibbonComboBox filtersCombobox;

		protected PaneDocumentsBilling _paneDocuments;
		protected PaneNotesBilling _paneNotesNote;
		protected PaneNotesBilling _paneNotesDisc;
		protected PaneFilterStats _paneFilterStats;
		protected PaneCodesList paneCodesList;

		protected BillingDataSet.DocumentsDataTable bindingTable;

		protected SolidColorBrush lightRedSolidColorBrush = new SolidColorBrush(System.Windows.Media.Color.FromArgb(120, 255, 0, 0));
		protected SolidColorBrush redSolidColorBrush = new SolidColorBrush(Colors.Red);
		protected SolidColorBrush orangeSolidColorBrush = new SolidColorBrush(Colors.Orange);

		protected List<List<RegScoreCalc.Forms.DialogFilter.ICD9CodeStruct>> ICD9codes;
		protected bool filterApplyed = false;


		protected int rightSplitterWidth = 0;
		protected bool allowChangeSplitterWidth = true;

		protected DialogGroup dialogGroup;
		protected DialogFilter dialogFilter;


		protected int redNoOfDocuments = 0;
		protected int greenNoOfDocuments = 0;
		protected int noOfDocuments = 0;

		protected RibbonButton btnFilterStats;

		protected ToolTip tip;

		public int _filterId = -1;

		protected FormSplitNotes _formNotes;
		protected Rectangle _initialWindowRect;
		protected FormWindowState _initialWindowState = FormWindowState.Normal;
		protected bool _isNotesDetached;

		protected bool _isSwapped;
		protected int _horizontalSplitterDistance;
		protected int _verticalSplitterDistance;
		protected bool _panel1Collapsed;
		protected bool _panel2Collapsed;

		protected TextStyle _textStyle;

		#endregion

		#region Ctors

		public ViewBILLING_1(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
			_strLayoutName = "Default Layout";

			_textStyle = new TextStyle(System.Drawing.Brushes.Black, System.Drawing.Brushes.Yellow, FontStyle.Bold | FontStyle.Underline);
		}

		#endregion

		#region Events

		private void OnDataModified(object sender, EventArgs e)
		{
			UpdateView();
		}

		private void OnMaximizeTable_Clicked(object sender, EventArgs e)
		{
			if (_isNotesDetached)
				RestoreNotesPane();

			allowChangeSplitterWidth = false;

			_splitterLeftSide.Panel1Collapsed = true;
			this.Panel1Collapsed = false;
			this.Panel2Collapsed = true;

			allowChangeSplitterWidth = true;
		}

		private void OnMaximizeNotes_Clicked(object sender, EventArgs e)
		{
			if (_isNotesDetached)
				RestoreNotesPane();

			allowChangeSplitterWidth = false;

			this.Panel1Collapsed = true;
			this.Panel2Collapsed = false;
			_splitter.Panel1Collapsed = false;
			_splitter.Panel2Collapsed = false;

			_splitterRightSide.SplitterDistance = this.Panel2.Width - rightSplitterWidth;

			allowChangeSplitterWidth = true;
		}

		private void OnResetView_Clicked(object sender, EventArgs e)
		{
			allowChangeSplitterWidth = false;
			ResetView();
			_splitterRightSide.SplitterDistance = this.Panel2.Width - rightSplitterWidth;

			allowChangeSplitterWidth = true;
		}

		private void OnLayout1_Clicked(object sender, EventArgs e)
		{
			if (_isNotesDetached)
				RestoreNotesPane();

			allowChangeSplitterWidth = false;
			//UpdateView();
			this.Panel1Collapsed = true;
			_splitter.Panel1Collapsed = false;
			_splitter.Panel2Collapsed = true;

			_splitterRightSide.SplitterDistance = this.Panel2.Width - rightSplitterWidth;

			allowChangeSplitterWidth = true;
		}

		private void OnLayout2_Clicked(object sender, EventArgs e)
		{
			if (_isNotesDetached)
				RestoreNotesPane();

			allowChangeSplitterWidth = false;

			//UpdateView();
			this.Panel1Collapsed = true;
			_splitter.Panel1Collapsed = true;
			_splitter.Panel2Collapsed = false;


			_splitterRightSide.SplitterDistance = this.Panel2.Width - rightSplitterWidth;

			allowChangeSplitterWidth = true;
		}

		private void OnNotesSeparateWindow_Clicked(object sender, EventArgs e)
		{
			DetachOrRestoreNotesPane();
		}

		private void formNotes_FormClosing(object sender, FormClosingEventArgs e)
		{
			OnNotesFormClosing(e);
		}

		//private void btnSnomedct_Click(object sender, EventArgs e)
		//{
		//    string selectedText = _paneNotesNote.GetSelectedText();
		//    if (String.IsNullOrEmpty(selectedText))
		//    {
		//        selectedText = _paneNotesDisc.GetSelectedText();
		//        if (String.IsNullOrEmpty(selectedText))
		//        {
		//            selectedText = "";
		//        }
		//    }
		//    string path = _views.SNOMEDBaseAddress;
		//    MediTermBrowser.MainForm mainForm = new MediTermBrowser.MainForm(selectedText, path);

		//    mainForm.Show();
		//}

		private void btnFilterStats_Click(object sender, EventArgs e)
		{
			//Calculate values for pane
			_paneFilterStats.InitPieChart(redNoOfDocuments, greenNoOfDocuments, noOfDocuments);


			//Show pane
			_splitterLeftSide.Panel1Collapsed = false;
		}

		private void btnClearFilter_Click(object sender, EventArgs e)
		{

			btnFilterStats.Enabled = false;
			_splitterLeftSide.Panel1Collapsed = true;

			// RemoveFilterFromGrid(true, true);
			filterApplyed = false;
			_filterId = -1;
			SetFilter("");

			_paneFilterStats.ResetData();

			var grid = _paneDocuments.GetDocumentsGrid();
			grid.Columns["columnColor"].Visible = false;
			grid.Columns["columnReason"].Visible = false;
			grid.DataSource = _views.MainForm.sourceDocumentsBilling;

			SetTreeViewData();
		}

		public void btn_FilterDropDownSelect(object sender, EventArgs e)
		{
			RibbonButton btn = (RibbonButton)sender;
			var id = (int)btn.Tag;

			_filterId = id;

			FilterDocuments(id);

			SetFilter(btn.Text);
			btnFilterStats.Enabled = true;
		}

		private void btnGroups_Click(object sender, EventArgs e)
		{
			if (dialogGroup == null)
			{
				dialogGroup = new DialogGroup(_views);
				dialogGroup.Show();
				dialogGroup.FormClosing += dialogGroup_FormClosing;
			}
			else
			{
				dialogGroup.BringToFront();
			}
		}

		private void dialogGroup_FormClosing(object sender, FormClosingEventArgs e)
		{
			dialogGroup = null;
		}

		private void btnExportToExcel_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Excel file (*.xlsx)|*.xlsx";
			var result = sfd.ShowDialog();

			if (result == DialogResult.OK)
			{
				var formGenericProgress = new FormGenericProgress("Exporting to Microsoft Excel", ExportToExcel, sfd.FileName, true);
				formGenericProgress.ShowDialog();
			}
		}

		private void btnFilters_Click(object sender, EventArgs e)
		{
			if (dialogFilter == null)
			{
				dialogFilter = new DialogFilter(_views, this);
				dialogFilter.Show();
				dialogFilter.FormClosing += dialogFilter_FormClosing;
			}
			else
			{
				dialogFilter.BringToFront();
			}
		}

		private void dialogFilter_FormClosing(object sender, FormClosingEventArgs e)
		{
			//Update filter combobox order
			SetComboboxFilterItems();

			dialogFilter = null;
		}

		private void _splitterRightSide_SplitterMoved(object sender, SplitterEventArgs e)
		{
			SetTreePaneWidth();

		}
		
		private void notesTreeViewPanelSizeChange(object sender, EventArgs e)
		{
			TreeViewResize();
		}

		private void TreeViewResize()
		{
			//get width and change the width in the TreeView
			if (_splitterRightSide != null)
			{
				var newWidth = _splitterRightSide.Panel2.Width;
				if (_notesTreeView != null)
				{

					_notesTreeView.SetWidth(newWidth);


					//Change splitter distance
					if (paneCodesList.Expanded)
					{
						_splitterRightTree.SplitterDistance = 224;
					}
					else
					{
						_splitterRightTree.SplitterDistance = 30;
					}
				}
			}

		}

		private void _notesTreeView_OnSelectionChange(object sender, CustomTreeView.TreeView.CustomTreeViewEventArgs args)
		{
			//Get currently selected item
			CustomTreeViewItem item = args.Item;

			//Highlight regExp
			_paneNotesNote.UpdateText();

			//Only highlight for green and orange tree items
			if (item.Background != redSolidColorBrush && item.Background != lightRedSolidColorBrush)
			{


				//Find in text ICD9 and Diagnosis
				var icd9 = args.Item.ICD9;
				var diagnosis = args.Item.Diagnosis;

				var richTextBox = _paneNotesNote.textBox;

				////Clear all highlight
				//RichTextExtensions.ClearAllFormatting(richTextBox, richTextBox.Font);

				int scrollToIndex = 0;
				//IDC9
				var icd9Indexes = AllIndexesOf(richTextBox.Text, icd9);
				var icd9Length = icd9.Length;
				foreach (var index in icd9Indexes)
				{
					if (scrollToIndex == 0)
					{
						scrollToIndex = index;
					}

					richTextBox.GetRange(index, index + icd9Length).SetStyle(_textStyle);
				}

				//Diagnosis
				var diagnosisIndexes = AllIndexesOf(richTextBox.Text, diagnosis);
				var diagnosisLength = diagnosis.Length;
				foreach (var index in diagnosisIndexes)
				{
					if (scrollToIndex == 0)
					{
						scrollToIndex = index;
					}

					richTextBox.GetRange(index, index + diagnosisLength).SetStyle(_textStyle);

				}

				try
				{
					richTextBox.Navigate(richTextBox.GetRange(scrollToIndex, scrollToIndex).FromLine);
				}
				catch (Exception ex)
				{
					MainForm.ShowExceptionMessage(ex);
				}
			}
		}
		
		private void _notesTreeView_OnAddToGroup(object sender, CustomTreeView.TreeView.CustomTreeViewEventArgs args)
		{
			//Get currently selected item
			CustomTreeView.CustomTreeViewItem item = args.Item;

			//Open add to group dialog
			AddTreeViewItemToGroup addTreeView = new AddTreeViewItemToGroup(_views, item.ICD9, item.Diagnosis);
			var dialogResult = addTreeView.ShowDialog();
			if (dialogResult == System.Windows.Forms.DialogResult.OK)
			{
				//Change that name
				var icd = addTreeView.icd;
				var description = addTreeView.description;
				var regExp = addTreeView.regExp;
				var groupId = addTreeView.groupId;
				if (!String.IsNullOrEmpty(icd))
				{
					var newCode = _views.MainForm.datasetBilling.ICDCodes.NewICDCodesRow();
					newCode.ICD9Code = icd;
					newCode.Description = description;
					newCode.RegExp = regExp;
					newCode.GroupID = groupId;

					_views.MainForm.datasetBilling.ICDCodes.Rows.Add(newCode);

					_views.MainForm.adapterICDCodesBilling.Update(_views.MainForm.datasetBilling.ICDCodes);

					if (dialogGroup != null)
					{
						dialogGroup.UpdateGroupList(groupId, icd, description, regExp, newCode.ID);
					}
				}
			}
		}

		private void paneCodesList_ChangedState(object sender, EventArgs e)
		{
			if (_splitterRightTree != null)
			{
				//Change splitter distance
				if (paneCodesList.Expanded)
				{
					_splitterRightTree.SplitterDistance = 224;
				}
				else
				{
					_splitterRightTree.SplitterDistance = 30;
				}

				TreeViewResize();
			}
		}

		private void OnCurrentDocumentChanged(object sender, EventArgs e)
		{
			SetTreeViewData();

			var newWidth = _splitterRightSide.Panel2.Width;

			_notesTreeView.SetWidth(newWidth);

			//Color current grid document

		}

		private void btnCalculateAll_Click(object sender, EventArgs e)
		{
			using (DialogFilter filter = new DialogFilter(_views, this))
			{
				filter.CalculateAll();
			}
		}

		private void btnRegExpOnOff_Click(object sender, EventArgs e)
		{
			_views.HighlightBillingMatches = !_views.HighlightBillingMatches;

			UpdateHighlightBillingMatchesButton((RibbonButton) sender);

			_paneNotesNote.UpdateText();
			_paneNotesDisc.UpdateText();
		}

		#endregion

		#region Overrides

		protected override void InitViewPanes(RibbonTab tab)
		{
			_splitterLeftSide = new SplitContainer();
			_splitterLeftSide.Name = "Splitter";
			_splitterLeftSide.BorderStyle = BorderStyle.Fixed3D;
			_splitterLeftSide.Orientation = Orientation.Horizontal;
			_splitterLeftSide.Dock = DockStyle.Fill;
			_splitterLeftSide.BackColor = MainForm.ColorBackground;
			_splitterLeftSide.Panel1MinSize = 0;
			_splitterLeftSide.Panel2MinSize = 0;


			this.Panel1.Controls.Add(_splitterLeftSide);

			//Add pie chart
			_paneFilterStats = new PaneFilterStats(this);
			_paneFilterStats.InitPane(_views, this, this.Panel1, tab);
			_splitterLeftSide.Panel1.Controls.Add(_paneFilterStats);
			_paneFilterStats.ShowPane();

			_splitterLeftSide.Panel1Collapsed = true;



			this.Orientation = Orientation.Vertical;

			_paneDocuments = new PaneDocumentsBilling();
			_paneDocuments.InitPane(_views, this, this.Panel1, tab);
			_paneDocuments._eventDataModified += new EventHandler(OnDataModified);



			var dataGrid = _paneDocuments.GetDocumentsGrid();

			_splitterLeftSide.Panel2.Controls.Add(_paneDocuments);
			_paneDocuments.ShowPane();

			//////////////////////////////////////////////////////////////////////////


			_splitterRightSide = new SplitContainer();
			_splitterRightSide.Name = "Splitter";
			_splitterRightSide.BorderStyle = BorderStyle.Fixed3D;
			_splitterRightSide.Orientation = Orientation.Vertical;
			_splitterRightSide.Dock = DockStyle.Fill;
			_splitterRightSide.BackColor = MainForm.ColorBackground;
			_splitterRightSide.Panel1MinSize = 0;
			_splitterRightSide.Panel2MinSize = 0;


			_splitter = new SplitContainer();
			_splitter.Name = "Splitter";
			_splitter.Orientation = Orientation.Horizontal;
			_splitter.Dock = DockStyle.Fill;
			_splitter.BorderStyle = BorderStyle.Fixed3D;
			_splitter.BackColor = MainForm.ColorBackground;
			_splitter.Panel1MinSize = 0;
			_splitter.Panel2MinSize = 0;

			_paneNotesNote = new PaneNotesBilling(NoteType.TEXTNOTE, this);
			_paneNotesNote.InitPane(_views, this, this.Panel1, tab);
			_paneNotesNote._eventDataModified += new EventHandler(OnDataModified);


			_splitter.Panel1.Controls.Add(_paneNotesNote);


			_paneNotesNote.ShowPane();

			_paneDocuments.GetDocumentsGrid().SelectionChanged += new EventHandler(OnCurrentDocumentChanged);
			//_views.MainForm.sourceDocumentsBilling.CurrentChanged += new EventHandler(OnCurrentDocumentChanged);

			//////////////////////////////////////////////////////////////////////////

			_paneNotesDisc = new PaneNotesBilling(NoteType.DISCNOTE, this);
			_paneNotesDisc.InitPane(_views, this, this.Panel1, tab);
			_paneNotesDisc._eventDataModified += new EventHandler(OnDataModified);

			_splitter.Panel2.Controls.Add(_paneNotesDisc);
			_paneNotesDisc.ShowPane();



			_splitterRightSide.Panel1.Controls.Add(_splitter);


			//TREE VIEW
			//CustomTreeView
			notesTreeViewPanel = new Panel();
			notesTreeViewPanel.Dock = DockStyle.Fill;


			ElementHost notesHost = new ElementHost();
			notesHost.Dock = DockStyle.Fill;
			notesTreeViewPanel.Controls.Add(notesHost);

			_notesTreeView = new CustomTreeView.TreeView();
			_notesTreeView.InitializeComponent();
			_notesTreeView.OnSelectionChange += _notesTreeView_OnSelectionChange;
			_notesTreeView.OnAddToGroup += _notesTreeView_OnAddToGroup;

			notesHost.Child = _notesTreeView;


			_splitterRightSide.Panel2.SizeChanged += notesTreeViewPanelSizeChange;


			_splitterRightTree = new SplitContainer();
			_splitterRightTree.Name = "Splitter";
			_splitterRightTree.BorderStyle = BorderStyle.Fixed3D;
			_splitterRightTree.Orientation = Orientation.Horizontal;
			_splitterRightTree.Dock = DockStyle.Fill;
			_splitterRightTree.BackColor = MainForm.ColorBackground;
			_splitterRightTree.Panel1MinSize = 0;
			_splitterRightTree.Panel2MinSize = 0;



			//Codes list
			paneCodesList = new PaneCodesList();
			paneCodesList.InitPane(_views, this, _splitterRightTree.Panel1, tab);
			paneCodesList.ChangedState += paneCodesList_ChangedState;

			_splitterRightTree.Panel1.Controls.Add(paneCodesList);
			paneCodesList.ShowPane();

			_splitterRightTree.Panel2.Controls.Add(notesTreeViewPanel);

			//Disable horizontal scroll
			_splitterRightTree.Panel2.AutoScroll = true;
			_splitterRightTree.Panel2.HorizontalScroll.Enabled = false;
			_splitterRightTree.Panel2.HorizontalScroll.Visible = false;



			_splitterRightSide.Panel2.Controls.Add(_splitterRightTree);

			this.Panel2.Controls.Add(_splitterRightSide);

			//Set height to show only button on start
			_splitterRightTree.Show();
			_splitterRightTree.SplitterDistance = 30;
			_splitterRightTree.IsSplitterFixed = true;

			//Add Ribbon Notes
			var panel = new RibbonPanel("Notes");
			tab.Panels.Add(panel);

			var _commands = new PaneNotesCommandsBilling(_views, panel, _paneNotesNote.textBox, _paneNotesDisc.textBox);
			_commands._eventDataModified += new EventHandler(OnDataModified);

			_splitterRightSide.SplitterMoved += _splitterRightSide_SplitterMoved;

			_splitter.SplitterDistance = this.Panel2.Height / 2;

			ResetView();

			SetTreeViewData();

			_paneDocuments.Select();

		}

		protected override void InitViewCommands(RibbonPanel panel)
		{

			btnFilterStats = new RibbonButton("Filter Stats");
			panel.Items.Add(btnFilterStats);
			btnFilterStats.Image = Properties.Resources.View;
			btnFilterStats.SmallImage = Properties.Resources.View;
			btnFilterStats.Click += btnFilterStats_Click;
			btnFilterStats.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
			btnFilterStats.Enabled = true;

			panel.Items.Add(new RibbonSeparator());

			RibbonButton btnMaximizeNotes = new RibbonButton("Maximize Notes View");
			panel.Items.Add(btnMaximizeNotes);
			btnMaximizeNotes.Image = Properties.Resources.MaximizeNotes;
			btnMaximizeNotes.SmallImage = Properties.Resources.MaximizeNotes;
			btnMaximizeNotes.Click += new EventHandler(OnMaximizeNotes_Clicked);
			btnMaximizeNotes.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnNotesSeparateWindow = new RibbonButton("Open Notes in Window");
			panel.Items.Add(btnNotesSeparateWindow);
			btnNotesSeparateWindow.Image = Properties.Resources.NotesSeparateWindow;
			btnNotesSeparateWindow.SmallImage = Properties.Resources.NotesSeparateWindow;
			btnNotesSeparateWindow.Click += new EventHandler(OnNotesSeparateWindow_Clicked);
			btnNotesSeparateWindow.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());

			RibbonButton btnMaximizeTable = new RibbonButton("Maximize Table");
			panel.Items.Add(btnMaximizeTable);
			btnMaximizeTable.Image = Properties.Resources.ResetView;
			btnMaximizeTable.SmallImage = Properties.Resources.ResetView;
			btnMaximizeTable.Click += new EventHandler(OnMaximizeTable_Clicked);
			btnMaximizeTable.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnMaximizeLeft = new RibbonButton("Maximize Top");
			panel.Items.Add(btnMaximizeLeft);
			btnMaximizeLeft.Image = Properties.Resources.ResetView;
			btnMaximizeLeft.SmallImage = Properties.Resources.ResetView;
			btnMaximizeLeft.Click += new EventHandler(OnLayout1_Clicked);
			btnMaximizeLeft.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;


			RibbonButton btnMaximizeRight = new RibbonButton("Maximize Bottom");
			panel.Items.Add(btnMaximizeRight);
			btnMaximizeRight.Image = Properties.Resources.ResetView;
			btnMaximizeRight.SmallImage = Properties.Resources.ResetView;
			btnMaximizeRight.Click += new EventHandler(OnLayout2_Clicked);
			btnMaximizeRight.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());

			RibbonButton btnResetView = new RibbonButton("Reset View");
			panel.Items.Add(btnResetView);
			btnResetView.Image = Properties.Resources.ResetView;
			btnResetView.SmallImage = Properties.Resources.ResetView;
			btnResetView.Click += new EventHandler(OnResetView_Clicked);
			btnResetView.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;



			panel.Items.Add(new RibbonSeparator());


			//RibbonButton btnSnomedct = new RibbonButton("Snomedct");
			//_panel.Items.Add(btnSnomedct);
			//btnSnomedct.Image = Properties.Resources.View;
			//btnSnomedct.SmallImage = Properties.Resources.View;
			//btnSnomedct.Click += btnSnomedct_Click;
			//btnSnomedct.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnRegExpOnOff = new RibbonButton("RegExp On");
			panel.Items.Add(btnRegExpOnOff);
			btnRegExpOnOff.Image = Properties.Resources.View;
			btnRegExpOnOff.SmallImage = Properties.Resources.View;
			btnRegExpOnOff.Click += btnRegExpOnOff_Click;
			btnRegExpOnOff.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			UpdateHighlightBillingMatchesButton(btnRegExpOnOff);

			panel.Items.Add(new RibbonSeparator());


			RibbonButton btnCalculateAll = new RibbonButton("Calculate All");
			panel.Items.Add(btnCalculateAll);

			btnCalculateAll.Image = Properties.Resources.CalcScores;
			btnCalculateAll.SmallImage = Properties.Resources.CalcScores;
			btnCalculateAll.Click += btnCalculateAll_Click;
			btnCalculateAll.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;



			//Filters combobox
			filtersCombobox = new RibbonComboBox();
			panel.Items.Add(filtersCombobox);
			filtersCombobox.AllowTextEdit = false;
			filtersCombobox.AltKey = null;
			filtersCombobox.Image = null;
			filtersCombobox.Tag = null;
			filtersCombobox.Text = "Select filter:";
			filtersCombobox.TextBoxWidth = 170;
			filtersCombobox.ToolTip = null;
			filtersCombobox.ToolTipImage = null;
			filtersCombobox.ToolTipTitle = null;

			SetFilter("");

			SetComboboxFilterItems();



			RibbonButton btnClearFilter = new RibbonButton("Clear Filter");
			panel.Items.Add(btnClearFilter);

			btnClearFilter.Image = Properties.Resources.ResetView;
			btnClearFilter.SmallImage = Properties.Resources.ResetView;
			btnClearFilter.Click += btnClearFilter_Click;
			btnClearFilter.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());


			RibbonButton btnFilters = new RibbonButton("Filters");
			panel.Items.Add(btnFilters);
			btnFilters.Image = Properties.Resources.View;
			btnFilters.SmallImage = Properties.Resources.View;
			btnFilters.Click += btnFilters_Click;
			btnFilters.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnGroups = new RibbonButton("Groups");
			panel.Items.Add(btnGroups);
			btnGroups.Image = Properties.Resources.View;
			btnGroups.SmallImage = Properties.Resources.View;
			btnGroups.Click += btnGroups_Click;
			btnGroups.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());

			RibbonButton btnExportToExcel = new RibbonButton("Export to Excel");
			panel.Items.Add(btnExportToExcel);
			btnExportToExcel.Image = Properties.Resources.ExportToExcel;
			btnExportToExcel.SmallImage = Properties.Resources.ExportToExcel;
			btnExportToExcel.Click += btnExportToExcel_Click;
			btnExportToExcel.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

		}

		public override void UpdateView()
		{


			if (_paneDocuments != null)
			{
				_paneDocuments.UpdatePane();
				//Remove filter
				try
				{
					var gridView = _paneDocuments.GetDocumentsGrid();
					if (gridView != null)
					{
						var dataSource = gridView.DataSource as BindingSource;
						if (dataSource != null)
							dataSource.RemoveFilter();
					}
				}
				catch { }
			}

			if (_paneNotesNote != null)
				_paneNotesNote.UpdatePane();
			if (_paneNotesDisc != null)
				_paneNotesDisc.UpdatePane();

			//if (_splitterRightSide != null)
			//{
			//    TreeViewResize();
			//}


			_views.Ribbon.Refresh();
		}

		public override void DestroyView()
		{
			_paneDocuments.DestroyPane();
			_paneNotesNote.DestroyPane();
			_paneNotesDisc.DestroyPane();

			base.DestroyView();
		}

		public override bool OnHotkey(string code)
		{
			var handled = _paneDocuments.OnHotkey(code);
			if (!handled)
				handled = _paneNotesNote.OnHotkey(code);

			if (!handled)
				handled = _paneNotesDisc.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			if (!handled)
				handled = base.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			return handled;
		}

		public override List<XElement> SaveCustomLayout()
		{
			var listCustomLayout = new List<XElement>();

			_isNotesDetached = false;

			if (_formNotes != null && !_formNotes.IsDisposed)
			{
				_initialWindowRect = new Rectangle(_formNotes.Left, _formNotes.Top, _formNotes.Width, _formNotes.Height);
				_initialWindowState = _formNotes.WindowState;

				_isNotesDetached = _formNotes.Visible;
				_isSwapped = _formNotes.IsSwapped;

				_verticalSplitterDistance = _splitter.SplitterDistance;
			}

			///////////////////////////////////////////////////////////////////

			var xPaneState = new XElement("NotesPaneState",
				new XAttribute("IsDetached", _isNotesDetached),
				new XAttribute("IsSwapped", _isSwapped),
				new XAttribute("HorizontalSplitterDistance", _horizontalSplitterDistance),
				new XAttribute("VerticalSplitterDistance", _verticalSplitterDistance),
				new XAttribute("Panel1Collapsed", _panel1Collapsed),
				new XAttribute("Panel2Collapsed", _panel2Collapsed));

			listCustomLayout.Add(xPaneState);

			var xFormState = new XElement("NotesFormState",
				new XAttribute("Left", _initialWindowRect.Left),
				new XAttribute("Top", _initialWindowRect.Top),
				new XAttribute("Width", _initialWindowRect.Width),
				new XAttribute("Height", _initialWindowRect.Height),
				new XAttribute("WindowState", (int)_initialWindowState));

			listCustomLayout.Add(xFormState);

			return listCustomLayout;
		}

		public override void LoadCustomLayout(List<XElement> listCustomLayout)
		{
			RestoreNotesPane();

			var xFormState = listCustomLayout.FirstOrDefault(x => x.Name == "NotesFormState");
			if (xFormState != null)
			{
				ExtractIntValue(xFormState, "Left", x => _initialWindowRect.X = x);
				ExtractIntValue(xFormState, "Top", x => _initialWindowRect.Y = x);
				ExtractIntValue(xFormState, "Width", x => _initialWindowRect.Width = x);
				ExtractIntValue(xFormState, "Height", x => _initialWindowRect.Height = x);

				ExtractIntValue(xFormState, "WindowState", x => _initialWindowState = (FormWindowState)x);
			}

			///////////////////////////////////////////////////////////////////

			var xPaneState = listCustomLayout.FirstOrDefault(x => x.Name == "NotesPaneState");
			if (xPaneState != null)
			{
				ExtractIntValue(xPaneState, "HorizontalSplitterDistance", x => _horizontalSplitterDistance = x);
				if (_horizontalSplitterDistance > 0)
					_splitter.SplitterDistance = _horizontalSplitterDistance;

				ExtractIntValue(xPaneState, "VerticalSplitterDistance", x => _verticalSplitterDistance = x);

				ExtractBoolValue(xPaneState, "IsSwapped", x => _isSwapped = x );
				ExtractBoolValue(xPaneState, "IsDetached", x => { if (x) DetachOrRestoreNotesPane(); });
				ExtractBoolValue(xPaneState, "Panel1Collapsed", x => _panel1Collapsed = x);
				ExtractBoolValue(xPaneState, "Panel2Collapsed", x => _panel2Collapsed = x);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			TreeViewResize();
		}

		#endregion

		#region Implementation

		public void FilterDocuments(int id)
		{
			//Get the filter and filter documents
			SaveFilterICD9Codes(id);


			//Reset visible documents
			//Set all documents to visible=false
			foreach (var document in _views.MainForm.datasetBilling.Documents)
			{
				document.Visible = false;
			}


			FilterDocuments(id, "RED");
			filterApplyed = true;
			_paneDocuments.GetDocumentsGrid().Columns["columnColor"].Visible = true;
			_paneDocuments.GetDocumentsGrid().Columns["columnReason"].Visible = true;

			//greenDocumentsShown = false;
			_paneFilterStats.ResetData();

			_paneFilterStats.InitPieChart(redNoOfDocuments, greenNoOfDocuments, noOfDocuments);
			SetTreeViewData();

			btnFilterStats.Enabled = true;
		}

		public void FilterDocuments(int id, string color)
		{
			//Apply filter

			var documentsId = _views.MainForm.datasetBilling
									.DocsToFilters.Select("FilterID = " + id.ToString() + "AND Type = 'RED'");

			var greenDocumentsId = _views.MainForm.datasetBilling
								   .DocsToFilters.Select("FilterID = " + id.ToString() + "AND Type = 'GREEN'");

			var allDocuments = _views.MainForm.datasetBilling.Documents;

			noOfDocuments = allDocuments.Count;
			redNoOfDocuments = documentsId.Length;
			greenNoOfDocuments = greenDocumentsId.Length;

			var showDocuments = _views.MainForm.datasetBilling
									 .DocsToFilters.Select("FilterID = " + id.ToString() + "AND Type = '" + color + "'");

            //_views.MainForm.adapterDocumentsBilling.Update(_views.MainForm.datasetBilling.Documents);
            //_views.MainForm.adapterDocumentsBilling.Fill(_views.MainForm.datasetBilling.Documents);

            foreach (RegScoreCalc.Data.BillingDataSet.DocsToFiltersRow item in showDocuments)
			{                
				var row = _views.MainForm.datasetBilling.Documents.FindByPrimaryKey(item.ED_ENC_NUM);
				row.Visible = true;
				row.Color = color;
				row.Reason = item.Reason;
                /*
                try
                {   
                    BillingDataSet.DocumentsDataTable dt = (BillingDataSet.DocumentsDataTable)_paneDocuments.GetDocumentsGrid().DataSource;
                    row.Category = dt.FindByED_ENC_NUM(item.ED_ENC_NUM).Category;
                }
                catch (Exception e)
                {

                } 
                */
            }
			//Save to database
			_views.MainForm.datasetBilling.AcceptChanges();

			//Show all rows that are visible
			ShowVisibleRows();

		}

		public DataGridView GetDocumentsGrid()
		{
			return _paneDocuments.GetDocumentsGrid();
		}

		protected void UpdateHighlightBillingMatchesButton(RibbonButton btn)
		{
			btn.Text = _views.HighlightBillingMatches ? "RegExp Off" : "RegExp On";
		}

		protected void SetFilter(string filter)
		{
			filtersCombobox.TextBoxText = filter;
			if (_paneDocuments != null)
				_paneDocuments.Filter = filter;
		}

		protected void SetComboboxFilterItems()
		{
			filtersCombobox.DropDownItems.Clear();

			//Reset filter dropdown item
			RibbonButton btnResetFilterDropDown = new RibbonButton("");
			btnResetFilterDropDown.Click += btnClearFilter_Click;
			filtersCombobox.DropDownItems.Add(btnResetFilterDropDown);

			//Load filters from database
			var filters = _views.MainForm.datasetBilling.ICDFilters.OrderBy(p => p.Position);
			foreach (var item in filters)
			{
				RibbonButton btn = new RibbonButton(item.Name);
				btn.Tag = item.FilterID;
				btn.Click += btn_FilterDropDownSelect;
				filtersCombobox.DropDownItems.Add(btn);
			}
		}

		public void RemoveFilterFromGrid(string color)
		{
			if (filterApplyed)
			{
				var id = _filterId;

				foreach (var item in _views.MainForm.datasetBilling.Documents)
				{
					if (!item.IsColorNull())
					{
						if (item.Color == color)
						{
							item.Visible = false;
						}
					}

				}

				_views.MainForm.datasetBilling.AcceptChanges();

				ShowVisibleRows();

			}
		}

		protected static IEnumerable<int> AllIndexesOf(string str, string value)
		{
			if (!String.IsNullOrEmpty(value))
			{
				for (int index = 0; ; index += value.Length)
				{
					index = str.IndexOf(value, index);
					if (index == -1)
						break;
					yield return index;
				}
			}
		}

		protected void SetTreeViewData()
		{
			if (_paneDocuments.GetDocumentsGrid().SelectedRows.Count > 0)
			{
				var gridRow = _paneDocuments.GetDocumentsGrid().SelectedRows[0];
				var edEnc = (double) gridRow.Cells[3].Value;
				
				//BillingDataSet.DocumentsRow dataRow = row.Row as BillingDataSet.DocumentsRow;
				BillingDataSet.DocumentsRow dataRow = _views.MainForm.datasetBilling.Documents.FindByPrimaryKey(edEnc);
				if (dataRow != null)
				{
					try
					{

						List<CustomTreeViewItem> source = CustomListViewSource(dataRow);
						_notesTreeView.SetData(source);
					}
					catch { }
				}
			}
		}

		protected List<CustomTreeViewItem> CustomListViewSource(BillingDataSet.DocumentsRow dataRow)
		{
			//Change the items in the TreeView
			List<CustomTreeViewItem> source = new List<CustomTreeViewItem>();
			List<CustomTreeViewItem> icdFromReason = new List<CustomTreeViewItem>();

			if (!String.IsNullOrEmpty(dataRow.ICD9_1))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_1,
					ICD9 = dataRow.ICD9_1,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_2))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_2,
					ICD9 = dataRow.ICD9_2,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_3))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_3,
					ICD9 = dataRow.ICD9_3,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_4))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_4,
					ICD9 = dataRow.ICD9_4,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_5))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_5,
					ICD9 = dataRow.ICD9_5,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_6))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_6,
					ICD9 = dataRow.ICD9_6,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_7))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_7,
					ICD9 = dataRow.ICD9_7,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_8))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_8,
					ICD9 = dataRow.ICD9_8,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_9))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_9,
					ICD9 = dataRow.ICD9_9,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_10))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_10,
					ICD9 = dataRow.ICD9_10,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_11))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_11,
					ICD9 = dataRow.ICD9_11,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_12))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_12,
					ICD9 = dataRow.ICD9_12,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_13))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_13,
					ICD9 = dataRow.ICD9_13,
					Background = orangeSolidColorBrush
				});
			}
			if (!String.IsNullOrEmpty(dataRow.ICD9_14))
			{
				source.Add(new CustomTreeViewItem()
				{
					Diagnosis = dataRow.Diagnosis_14,
					ICD9 = dataRow.ICD9_14,
					Background = orangeSolidColorBrush
				});
			}

			//Get all items with ED_ENC_NUM from Billing



			var billingData = _views.MainForm.adapterBillingBilling.GetData().Select("ED_ENC_NUM = " + dataRow.ED_ENC_NUM.ToString());
			//_views.MainForm.
			foreach (RegScoreCalc.Data.BillingDataSet.BillingRow item in billingData)
			{
				var newItem = new CustomTreeViewItem();
				newItem.Diagnosis = item.Diagnosis;
				newItem.ICD9 = item.ICD9;
				newItem.Background = orangeSolidColorBrush;
				var index = source.FindIndex(0, p => p.ICD9 == newItem.ICD9);
				if (index > -1)
				{
					//Change that items color to GREEN
					source[index].Background = new SolidColorBrush(Colors.Green);
				}
				else
				{
					if (filterApplyed)
					{
						//Check if this code has caused
						if (CodeInFilter(item.ICD9))
						{
							newItem.Background = redSolidColorBrush;
						}
						else
						{
							newItem.Background = lightRedSolidColorBrush;
						}
					}
					else
					{
						newItem.Background = redSolidColorBrush;
					}
					source.Add(newItem);
				}
			}

			//Sort by ICD9
			SortCustomTreeViewItemList(source);


			//Now take ICDs from Reason column and put them to the new list
			//icdFromReason.Add(new CustomTreeViewItem());

			//Check if reason exists
			if (!dataRow.IsReasonNull())
			{
				//Get reason value
				var reason = dataRow.Reason;

				var reasonICDs = new List<string>();
				Regex numberTest = new Regex(@"[0-9]{1,5}([.][0-9]{1,5})?");
				foreach (Match match in numberTest.Matches(reason))
				{
					reasonICDs.Add(match.Value);
				}

				//Remove all of the found reasonICDs from source list
				foreach (var icd in reasonICDs)
				{
					var itemInSource = source.Find(p => p.ICD9 == icd);
					if (itemInSource != null)
					{
						source.Remove(itemInSource);

						itemInSource.FrameBackgroundColor = new SolidColorBrush(Colors.LightBlue);
						icdFromReason.Add(itemInSource);
					}
				}


				//Sort icdFromReasonList
				SortCustomTreeViewItemList(icdFromReason);

				//Start looping over list from behind and insert items in first position
				//Also change color of the items
				for (int i = icdFromReason.Count - 1; i >= 0; i--)
				{
					source.Insert(0, icdFromReason[i]);
				}


				//Change the margin of the last tree view item from reason
				if (icdFromReason.Count > 0)
				{
					if (icdFromReason.Count == 1)
					{
						source[icdFromReason.Count - 1].Margin = new System.Windows.Thickness(5, 15, 5, 15);
					}
					else
					{
						source[0].Margin = new System.Windows.Thickness(5, 15, 5, 5);
						source[icdFromReason.Count - 1].Margin = new System.Windows.Thickness(5, 5, 5, 15);
					}
				}

			}




			var width = _splitterRightSide.Panel2.Width - 20;
			foreach (var item in source)
			{
				//item.SubList = new List<SubList>();
				//for (int i = 0; i < 3; i++)
				//{
				//    item.SubList.Add(new SubList() { Name = "sub_" + i.ToString() });
				//}


				item.Width = width;
			}

			return source;
		}

		protected static void SortCustomTreeViewItemList(List<CustomTreeViewItem> list)
		{
			list.Sort(delegate(CustomTreeViewItem item1, CustomTreeViewItem item2)
			{
				double icd1, icd2;
				bool icd1Success = Double.TryParse(item1.ICD9, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out icd1);
				if (!icd1Success)
				{
					return 1;
				}
				bool icd2Success = Double.TryParse(item2.ICD9, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out icd2);
				if (!icd2Success)
				{
					return -1;
				}
				int compareICD9 = icd1.CompareTo(icd2);

				return compareICD9;
			});
		}

		protected bool CodeInFilter(string code)
		{
			foreach (var group in ICD9codes)
			{
				foreach (var icdCode in group)
				{
					if (icdCode.RegExp)
					{
						Regex regExp = new Regex(icdCode.Text);
						if (regExp.Match(code).Success)
						{
							return true;
						}
					}
					else
					{
						if (code == icdCode.Text)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		protected void SetTreePaneWidth()
		{
			if (allowChangeSplitterWidth)
			{
				rightSplitterWidth = _splitterRightSide.Panel2.Width;
			}
		}

		protected void ShowVisibleRows()
		{
			try
			{
				//Filter dataGridView
				var grid = _paneDocuments.GetDocumentsGrid();

				var visibleRows = _views.MainForm.datasetBilling.Documents.Select("Visible = TRUE");
				bindingTable = new BillingDataSet.DocumentsDataTable();
				foreach (var item in visibleRows)
				{
					bindingTable.ImportRow(item);
				}
				grid.DataSource = bindingTable;
				grid.Update();
			}
			catch { }
		}

		protected void SaveFilterICD9Codes(int id)
		{
			try
			{
				//Save filter codes
				ICD9codes = new List<List<RegScoreCalc.Forms.DialogFilter.ICD9CodeStruct>>();
				var filter = _views.MainForm.datasetBilling.ICDFilters.FindByFilterID(id);
				if (!filter.IsGroupIDsNull())
				{
					var groups = filter.GroupIDs.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var item in groups)
					{
						//Get all ICD9 codes from group
						var codes = _views.MainForm.datasetBilling.ICDCodes.Select("GroupID = " + item);
						if (codes.Length > 0)
						{
							List<RegScoreCalc.Forms.DialogFilter.ICD9CodeStruct> tempList = new List<RegScoreCalc.Forms.DialogFilter.ICD9CodeStruct>();

							foreach (RegScoreCalc.Data.BillingDataSet.ICDCodesRow code in codes)
							{
								tempList.Add(new RegScoreCalc.Forms.DialogFilter.ICD9CodeStruct() { Text = code.ICD9Code, RegExp = code.RegExp });
							}
							ICD9codes.Add(tempList);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected bool ExportToExcel(BackgroundWorker worker, object objArgument)
		{
			bool bResult = false;

			var filePath = objArgument as string;

			Microsoft.Office.Interop.Excel.Application excel = null;
			_Workbook book = null;
			_Worksheet sheet = null;

			try
			{
				//Start Excel and get Application object.

				try
				{
					excel = new Microsoft.Office.Interop.Excel.Application();
				}
				catch
				{
					MessageBox.Show("Failed to export. Please make sure that Excel is installed", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}
				
				excel.Visible = false;
				excel.UserControl = false;

				//Get a new workbook.
				book = excel.Workbooks.Add(System.Type.Missing);
				sheet = (_Worksheet)book.ActiveSheet;

				excel.Rows.EntireRow.RowHeight = 20;

				int xPosition = 1;
				int yPosition = 1;

				//Save all names of cells in grid
				List<string> cellNames = new List<string>();
				//Create column headers??
				var gridView = _paneDocuments.GetDocumentsGrid();
				foreach (DataGridViewColumn column in gridView.Columns)
				{
					if (column.Visible)
					{
						sheet.Cells[xPosition, yPosition] = column.HeaderText;
						cellNames.Add(column.Name);
						yPosition++;
					}
				}

				sheet.Cells[xPosition, yPosition++] = "ED_NOTE";
				sheet.Cells[xPosition, yPosition++] = "DISC_NOTE";
				sheet.Cells[xPosition, yPosition++] = "Matched_Dx";
				sheet.Cells[xPosition, yPosition++] = "Unmatched_Ed";
				sheet.Cells[xPosition, yPosition++] = "Unmatched_Discharge";
				sheet.Cells[xPosition, yPosition++] = "Unmatched_Do_not_miss";

				var rowCount = gridView.Rows.Count + 1;
				var currentRow = 0;
				//Add data to Excel
				//Get all rows from the dataGrid
				foreach (DataGridViewRow row in gridView.Rows)
				{
					//Increment xPosition to store to next row
					xPosition++;

					//Restart yPosition to start for beginning
					yPosition = 1;
					foreach (var cellName in cellNames)
					{
						sheet.Cells[xPosition, yPosition++] = row.Cells[cellName].FormattedValue.ToString();
					}

					//Get 2 text values from paneNotes
					BillingDataSet.DocumentsRow item = (BillingDataSet.DocumentsRow)(row.DataBoundItem as DataRowView).Row;

					var noteCell = (Range)sheet.Cells[xPosition, yPosition++];
					noteCell.Value2 = item.NOTE_TEXT;

					if (_views.HighlightBillingMatches)
						FormatCellText(noteCell, _paneNotesNote.GetRegExpMatchResults(item.NOTE_TEXT));

					var discCell = (Range)sheet.Cells[xPosition, yPosition++];
					discCell.Value2 = item.DISC_TEXT;

					if (_views.HighlightBillingMatches)
						FormatCellText(discCell, _paneNotesDisc.GetRegExpMatchResults(item.DISC_TEXT));

					//Colors

					string redColor = "";
					string lightRedColor = "";
					string orangeColor = "";
					string greenColor = "";
					List<CustomTreeViewItem> colorItemsSource = CustomListViewSource(item);
					foreach (var colorItem in colorItemsSource)
					{
						if (colorItem.Background == redSolidColorBrush)
						{
							redColor += colorItem.ICD9 + ", " + colorItem.Diagnosis + Environment.NewLine;
						}
						else if (colorItem.Background == lightRedSolidColorBrush)
						{
							lightRedColor += colorItem.ICD9 + ", " + colorItem.Diagnosis + Environment.NewLine;
						}
						else if (colorItem.Background == orangeSolidColorBrush)
						{
							orangeColor += colorItem.ICD9 + ", " + colorItem.Diagnosis + Environment.NewLine;
						}
						else //Green
						{
							greenColor += colorItem.ICD9 + ", " + colorItem.Diagnosis + Environment.NewLine;
						}
					}
					//Matched_Dx
					sheet.Cells[xPosition, yPosition++] = greenColor;
					//Unmatched_Ed
					sheet.Cells[xPosition, yPosition++] = lightRedColor;
					//Unmatched_Discharge
					sheet.Cells[xPosition, yPosition++] = orangeColor;
					//Unmatched_Do_not_miss
					sheet.Cells[xPosition, yPosition++] = redColor;

					if (worker.CancellationPending)
						break;

					currentRow++;

					System.Threading.Thread.Sleep(3000);

					var progress = (currentRow / (double)rowCount) * 100.0;

					worker.ReportProgress((int)progress);
				}

				if (!worker.CancellationPending)
				{
					var cellLeftTop = (Range)sheet.Cells[1, 1];
					var cellBottomRight = (Range)sheet.Cells[xPosition, yPosition - 1];
					var range = sheet.Range[cellLeftTop, cellBottomRight];

					var color = ColorTranslator.ToOle(Color.FromArgb(0x5B, 0x9B, 0xD5));

					range = sheet.Range[cellLeftTop, cellBottomRight];
					range.Font.Size = 14;
					range.Rows.RowHeight = 60;
					range.HorizontalAlignment = XlHAlign.xlHAlignLeft;
					range.VerticalAlignment = XlVAlign.xlVAlignTop;
					range.Borders.LineStyle = XlLineStyle.xlContinuous;
					range.Borders.Weight = XlBorderWeight.xlThin;
					range.Borders.Color = color;

					///////////////////////////////////////////////////////////////////////////////

					cellLeftTop = (Range)sheet.Cells[1, 1];
					cellBottomRight = (Range)sheet.Cells[xPosition, 7];

					range = sheet.Range[cellLeftTop, cellBottomRight];
					range.Columns.AutoFit();

					///////////////////////////////////////////////////////////////////////////////

					cellLeftTop = (Range)sheet.Cells[1, 1];
					cellBottomRight = (Range)sheet.Cells[xPosition, yPosition - 1];

					range = sheet.Range[cellLeftTop, cellBottomRight];

					var header = (Range)range.Rows[1];
					header.Font.Bold = true;
					header.Font.Color = ColorTranslator.ToOle(Color.White);
					header.Interior.Color = color;
					header.Rows.RowHeight = 25;
					range.Borders.LineStyle = XlLineStyle.xlContinuous;
					range.Borders.Weight = XlBorderWeight.xlThin;
					range.Borders.Color = color;

					///////////////////////////////////////////////////////////////////////////////

					cellLeftTop = (Range)sheet.Cells[1, 8];
					cellBottomRight = (Range)sheet.Cells[xPosition, yPosition - 1];

					range = sheet.Range[cellLeftTop, cellBottomRight];
					range.Columns.ColumnWidth = 65;

					///////////////////////////////////////////////////////////////////////////////

					worker.ReportProgress(100);

					excel.Visible = true;

					book.SaveAs(filePath, XlFileFormat.xlWorkbookDefault, System.Type.Missing, System.Type.Missing,
						false, false, XlSaveAsAccessMode.xlNoChange,
						System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);

					bResult = true;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
			finally
			{
				try
				{
					if (!bResult)
					{
						if (book != null)
						{
							object objFalse = false;
							book.Close(false);
						}

						if (excel != null)
							excel.Quit();
					}

					if (book != null)
					{
						Marshal.FinalReleaseComObject(book);
						book = null;
					}

					if (sheet != null)
					{
						Marshal.FinalReleaseComObject(sheet);
						sheet = null;
					}

					if (excel != null)
					{
						Marshal.FinalReleaseComObject(excel);
						excel = null;
					}
				}
				catch { }

				GC.Collect();
				GC.WaitForPendingFinalizers();
			}

			return bResult;
		}

		protected void FormatCellText(Range cell, List<RegExpMatchResult> resultsList)
		{
			foreach (var item in resultsList)
			{
				try
				{
					cell.Characters[item.Match.Index + 1, item.Match.Length].Font.Color = ColorTranslator.ToOle(item.RegExp.Color);
					cell.Characters[item.Match.Index + 1, item.Match.Length].Font.Bold = true;
				}
				catch
				{
					break;
				}
			}
		}

		protected void ResetView()
		{
			RestoreNotesPane();

			this.Panel1Collapsed = false;
			this.Panel2Collapsed = false;
			_splitter.Panel1Collapsed = false;
			_splitter.Panel2Collapsed = false;

			_splitterLeftSide.Panel1Collapsed = true;
			//_splitter.SplitterDistance = this.Panel2.Height / 2;
		}

		protected void DetachOrRestoreNotesPane()
		{
			if (_formNotes == null)
			{
				_formNotes = new FormSplitNotes(_splitter);
				_formNotes.FormClosing += formNotes_FormClosing;
				_formNotes.Show(_views.MainForm);

				if (!_initialWindowRect.IsEmpty)
				{
					_formNotes.Left = _initialWindowRect.Left;
					_formNotes.Top = _initialWindowRect.Top;

					if (_initialWindowState != FormWindowState.Maximized)
					{
						_formNotes.Width = _initialWindowRect.Width;
						_formNotes.Height = _initialWindowRect.Height;
					}
				}

				_formNotes.WindowState = _initialWindowState;

				///////////////////////////////////////////////////////////////

				_horizontalSplitterDistance = _splitter.SplitterDistance;
				_panel1Collapsed = _splitter.Panel1Collapsed;
				_panel2Collapsed = _splitter.Panel2Collapsed;

				_formNotes.IsSwapped = _isSwapped;
				_formNotes.DetachControlFromParent();

				_splitter.Panel1Collapsed = false;
				_splitter.Panel2Collapsed = false;

				_splitter.Orientation = Orientation.Vertical;
				
				if (_verticalSplitterDistance > 0)
					_splitter.SplitterDistance = _verticalSplitterDistance;
				else
					_splitter.SplitterDistance = _splitter.Width / 2;

				///////////////////////////////////////////////////////////////

				_splitterRightSide.Panel1Collapsed = true;

				_isNotesDetached = true;
			}
			else
			{
				RestoreNotesPane();

				_isNotesDetached = false;
			}
		}

		protected void RestoreNotesPane()
		{
			if (_formNotes != null && !_formNotes.IsDisposed)
				_formNotes.Close();
		}

		protected void OnNotesFormClosing(FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				_initialWindowRect = new Rectangle(_formNotes.Left, _formNotes.Top, _formNotes.Width, _formNotes.Height);
				_initialWindowState = _formNotes.WindowState;

				///////////////////////////////////////////////////////////////////////////////

				_isSwapped = _formNotes.IsSwapped;
				_verticalSplitterDistance = _splitter.SplitterDistance;

				_formNotes.AttachControlToParent();
				_formNotes = null;

				///////////////////////////////////////////////////////////////////////////////

				_splitter.Orientation = Orientation.Horizontal;
				_splitter.Panel1Collapsed = _panel1Collapsed;
				_splitter.Panel2Collapsed = _panel2Collapsed;

				if (_horizontalSplitterDistance > 0)
					_splitter.SplitterDistance = _horizontalSplitterDistance;
				else
					_splitter.SplitterDistance = _splitter.Width / 2;

				///////////////////////////////////////////////////////////////////////////////

				_splitterRightSide.Panel1Collapsed = false;
			}
		}

		protected void ExtractIntValue(XElement xElement, string strAttribute, Action<int> predicate)
		{
			XAttribute xAttribute = xElement.Attribute(strAttribute);
			if (xAttribute != null)
			{
				int nValue = Convert.ToInt32(xAttribute.Value);
				predicate(nValue);
			}
		}

		protected void ExtractBoolValue(XElement xElement, string strAttribute, Action<bool> predicate)
		{
			XAttribute xAttribute = xElement.Attribute(strAttribute);
			if (xAttribute != null)
			{
				bool bValue = Convert.ToBoolean(xAttribute.Value);
				predicate(bValue);
			}
		}

		#endregion
	}
}
