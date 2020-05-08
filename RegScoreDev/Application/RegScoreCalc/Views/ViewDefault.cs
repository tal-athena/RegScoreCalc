using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;

using Newtonsoft.Json;

using RegExpLib.Core;

using RegScoreCalc.Forms;

namespace RegScoreCalc
{
	public class ViewDefault : View
	{
		#region Fields

		protected SplitContainer _splitter;

		protected PaneRegExp _paneRegExp;
		protected PaneDocuments _paneDocuments;
        //protected PaneNotes _paneNotes;
        protected PaneTabNotes _paneTabNotes;
		protected RibbonButton _btnLayout1;
		protected RibbonButton _btnLayout2;

		protected RibbonButton _btnToggleNotesBottom;

		protected FormNotes _formNotes;
		protected Rectangle _initialWindowRect;
		protected FormWindowState _initialWindowState = FormWindowState.Normal;
		protected bool _isNotesDetached;

		protected ToolTip tip;

		#endregion

		#region Properties

		public PaneDocuments PaneDocuments
		{
			get { return _paneDocuments; }
		}

		#endregion

		#region Ctors

		public ViewDefault(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
			_strLayoutName = "Layout 1";
		}

		#endregion

		#region Events

		private void OnDataModified(object sender, EventArgs e)
		{
			UpdateView();
		}

		private void OnMaximizeNotes_Clicked(object sender, EventArgs e)
		{
			MaximizeNotesView();
		}

		private void OnNotesSeparateWindow_Clicked(object sender, EventArgs e)
		{
			DetachNotesPane();
		}

		private void OnToggleNotesBottom_Clicked(object sender, EventArgs e)
		{
			try
			{
				SetNotesBottom(!_btnToggleNotesBottom.Checked, true);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnResetView_Clicked(object sender, EventArgs e)
		{
			ResetView();
		}

		private void OnLayout1_Clicked(object sender, EventArgs e)
		{
			ApplyLayout("Layout 1");

			UpdateView();
		}

		private void OnLayout2_Clicked(object sender, EventArgs e)
		{
			ApplyLayout("Layout 2");

			UpdateView();
		}

		private void paneNotes_CalcScores(object sender, EventArgs e)
		{
			_paneRegExp.CalcScores();
		}

		private void paneNotes_RefreshHighlights(object sender, EventArgs e)
		{
			_views.InvokeRefreshHighlights();
		}

		private void btnSnomedct_Click(object sender, EventArgs e)
		{
            //string selectedText = _paneNotes.GetSelectedText();
            string selectedText = _paneTabNotes.GetSelectedText();

            if (String.IsNullOrEmpty(selectedText))
				selectedText = "";
			string path = _views.SNOMEDBaseAddress;
			MediTermBrowser.MainForm mainForm = new MediTermBrowser.MainForm(selectedText, path);

			mainForm.Show();
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			var path = System.IO.Directory.GetCurrentDirectory();
			var filepath = path + "\\Resources\\HelpHtml.html";

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

		private void formNotes_FormClosing(object sender, FormClosingEventArgs e)
		{
			OnNotesFormClosing(e);
		}

		#endregion

		#region Overrides

		protected override void InitViewCommands(RibbonPanel panel)
		{
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

			_btnToggleNotesBottom = new RibbonButton("Notes at Bottom");
			panel.Items.Add(_btnToggleNotesBottom);
			_btnToggleNotesBottom.Image = Properties.Resources.notes_down;
			_btnToggleNotesBottom.SmallImage = Properties.Resources.notes_down;
			_btnToggleNotesBottom.Click += new EventHandler(OnToggleNotesBottom_Clicked);
			_btnToggleNotesBottom.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnResetView = new RibbonButton("Reset View");
			panel.Items.Add(btnResetView);
			btnResetView.Image = Properties.Resources.ResetView;
			btnResetView.SmallImage = Properties.Resources.ResetView;
			btnResetView.Click += new EventHandler(OnResetView_Clicked);
			btnResetView.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());

			_btnLayout1 = new RibbonButton("Layout 1");
			panel.Items.Add(_btnLayout1);
			_btnLayout1.Image = Properties.Resources.ResetView;
			_btnLayout1.SmallImage = Properties.Resources.ResetView;
			_btnLayout1.Click += new EventHandler(OnLayout1_Clicked);
			_btnLayout1.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			_btnLayout2 = new RibbonButton("Layout 2");
			panel.Items.Add(_btnLayout2);
			_btnLayout2.Image = Properties.Resources.ResetView;
			_btnLayout2.SmallImage = Properties.Resources.ResetView;
			_btnLayout2.Click += new EventHandler(OnLayout2_Clicked);
			_btnLayout2.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());

			RibbonButton btnSnomedct = new RibbonButton("Snomedct");
			panel.Items.Add(btnSnomedct);
			btnSnomedct.Image = Properties.Resources.View;
			btnSnomedct.SmallImage = Properties.Resources.View;
			btnSnomedct.Click += btnSnomedct_Click;
			btnSnomedct.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			panel.Items.Add(new RibbonSeparator());

			RibbonButton btnHelp = new RibbonButton("RegExp Help");
			panel.Items.Add(btnHelp);
			btnHelp.Image = Properties.Resources.HelpIcon;
			btnHelp.SmallImage = Properties.Resources.HelpIcon;
			btnHelp.Click += btnHelp_Click;
			btnHelp.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			///////////////////////////////////////////////////////////////////////////////

			/*RibbonButton btnPerformance = new RibbonButton("TEST UPDATE");
			_panel.Items.Add(btnPerformance);
			btnPerformance.Click += (sender, args) => { new Forms.Performance(_views).ShowDialog(); };
			btnPerformance.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;*/
		}

		protected override void InitViewPanes(RibbonTab tab)
		{
			this.Orientation = Orientation.Horizontal;

			_paneRegExp = new PaneRegExp(_views);
			_paneRegExp.InitPane(_views, this, this.Panel1, tab);
			_paneRegExp._eventDataModified += new EventHandler(OnDataModified);

			this.Panel1.Controls.Add(_paneRegExp);
			_paneRegExp.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			_splitter = new SplitContainer();
			_splitter.Name = "Splitter";
			_splitter.BorderStyle = BorderStyle.Fixed3D;
			_splitter.Orientation = Orientation.Vertical;
			_splitter.Dock = DockStyle.Fill;
			_splitter.BackColor = MainForm.ColorBackground;
			_splitter.Panel1MinSize = 0;
			_splitter.Panel2MinSize = 0;
			_splitter.Tag = new SplitterInfo();

			this.Panel2.Controls.Add(_splitter);

			_paneDocuments = new PaneDocuments();
			_paneDocuments.InitPane(_views, this, this.Panel1, tab);
			_paneDocuments._eventDataModified += new EventHandler(OnDataModified);

			_splitter.Panel1.Controls.Add(_paneDocuments);
			_paneDocuments.ShowPane();

            //////////////////////////////////////////////////////////////////////////
            _paneTabNotes = new PaneTabNotes();
            _paneTabNotes.InitPane(_views, this, this.Panel1, tab);
            _paneTabNotes._eventDataModified += new EventHandler(OnDataModified);
            _paneTabNotes.PaneTab_CalcScores += paneNotes_CalcScores;
            _paneTabNotes.PaneTab_RefreshHighlights += new EventHandler(paneNotes_RefreshHighlights);

            _splitter.Panel2.Controls.Add(_paneTabNotes);
            _paneTabNotes.ShowPane();

            /*
			_paneNotes = new PaneNotes();
			_paneNotes.InitPane(_views, this, this.Panel1, tab);
			_paneNotes._eventDataModified += new EventHandler(OnDataModified);
			_paneNotes.CalcScores += paneNotes_CalcScores;
			_paneNotes.RefreshHighlights += new EventHandler(paneNotes_RefreshHighlights);

			_splitter.Panel2.Controls.Add(_paneNotes);
			_paneNotes.ShowPane();
            */
			ResetView();

			_paneDocuments.Select();
		}

		public override void UpdateView()
		{
			if (_paneRegExp != null)
				_paneRegExp.UpdatePane();

			if (_paneDocuments != null)
				_paneDocuments.UpdatePane();
            /*
			if (_paneNotes != null)
				_paneNotes.UpdatePane();
            */
			if (this.LayoutName == "Layout 1")
			{
				_btnLayout1.Checked = true;
				_btnLayout2.Checked = false;
			}
			else if (this.LayoutName == "Layout 2")
			{
				_btnLayout1.Checked = false;
				_btnLayout2.Checked = true;
			}

			_views.Ribbon.Refresh();
		}

		public override void DestroyView()
		{
			RestoreNotesPane();

			_paneDocuments.DestroyPane();
            _paneRegExp.DestroyPane();
            //_paneNotes.DestroyPane();
            _paneTabNotes.DestroyPane();
			base.DestroyView();
		}

		public override bool OnHotkey(string code)
		{
			var handled = _paneDocuments.OnHotkey(code);
			if (!handled)
            {
                //handled = _paneNotes.OnHotkey(code);
                handled = _paneTabNotes.OnHotkey(code);
            }


            ///////////////////////////////////////////////////////////////////////////////

            if (!handled)
				handled = base.OnHotkey(code);

			///////////////////////////////////////////////////////////////////////////////

			return handled;
		}

		public override void BeforeDocumentsTableLoad(bool removeDynamicColumns)
		{
			_paneDocuments.BeforeDocumentsTableLoad(removeDynamicColumns);

			base.BeforeDocumentsTableLoad(removeDynamicColumns);
		}

		public override void AfterDocumentsTableLoad(bool addDynamicColumns)
		{
			_paneDocuments.AfterDocumentsTableLoad(addDynamicColumns);

			base.AfterDocumentsTableLoad(addDynamicColumns);
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
			}

			///////////////////////////////////////////////////////////////////

			var splitterInfo = (SplitterInfo) _splitter.Tag;
			UpdateSplitterInfo(splitterInfo);

			var isNotesBottom = _splitter.Orientation == Orientation.Horizontal;

			var xPaneState = new XElement("NotesPaneState",
				new XAttribute("IsDetached", _isNotesDetached),
				new XAttribute("IsBottom", isNotesBottom),
				new XAttribute("HorizontalSplitterDistance", splitterInfo.HorizontalPercentage),
				new XAttribute("VerticalSplitterDistance", splitterInfo.VerticalPercentage));

			listCustomLayout.Add(xPaneState);

			var xFormState = new XElement("NotesFormState",
				new XAttribute("Left", _initialWindowRect.Left),
				new XAttribute("Top", _initialWindowRect.Top),
				new XAttribute("Width", _initialWindowRect.Width),
				new XAttribute("Height", _initialWindowRect.Height),
				new XAttribute("WindowState", (int) _initialWindowState));

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

				ExtractIntValue(xFormState, "WindowState", x => _initialWindowState = (FormWindowState) x);
			}

			///////////////////////////////////////////////////////////////////

			var xPaneState = listCustomLayout.FirstOrDefault(x => x.Name == "NotesPaneState");
			if (xPaneState != null)
			{
				var xIsDetached = xPaneState.Attributes().FirstOrDefault(x => x.Name == "IsDetached");
				if (xIsDetached != null && Convert.ToBoolean(xIsDetached.Value))
					DetachNotesPane();

				var splitterInfo = (SplitterInfo) _splitter.Tag;

				ExtractDoubleValue(xPaneState, "HorizontalSplitterDistance", x => splitterInfo.HorizontalPercentage = x);
				ExtractDoubleValue(xPaneState, "VerticalSplitterDistance", x => splitterInfo.VerticalPercentage = x);

				var xIsNotesBottom = xPaneState.Attributes().FirstOrDefault(x => x.Name == "IsBottom");
				SetNotesBottom(xIsNotesBottom != null && Convert.ToBoolean(xIsNotesBottom.Value), false);
			}
		}

		#endregion

		#region Implementation

		protected void MaximizeNotesView()
		{
			RestoreNotesPane();

			this.Panel1Collapsed = true;
			_splitter.Panel1Collapsed = true;
		}

		protected void DetachNotesPane()
		{
			if (_formNotes == null)
			{
                //_formNotes = new FormNotes(_paneNotes);
                _formNotes = new FormNotes(_paneTabNotes);
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

				_splitter.Panel2Collapsed = true;

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

				_formNotes = null;

				_splitter.Panel2Collapsed = false;
			}
		}

		protected void SetNotesBottom(bool isBottom, bool updateSplitterInfo)
		{
			var splitterInfo = (SplitterInfo) _splitter.Tag;
			if (updateSplitterInfo)
				UpdateSplitterInfo(splitterInfo);

			var oldOrientation = _splitter.Orientation;

			_btnToggleNotesBottom.Checked = isBottom;
			_splitter.Orientation = isBottom ? Orientation.Horizontal : Orientation.Vertical;

			if (_splitter.Orientation != oldOrientation)
				UpdateSplitterDistance(splitterInfo);
		}

		public override void ResetView()
		{
			RestoreNotesPane();

			this.Panel1Collapsed = false;
			_splitter.Panel1Collapsed = false;
			_splitter.Panel2Collapsed = false;

			_splitter.SplitterDistance = _splitter.Width / 100 * 45;

			SetNotesBottom(false, false);
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

		protected void ExtractDoubleValue(XElement xElement, string strAttribute, Action<double> predicate)
		{
			XAttribute xAttribute = xElement.Attribute(strAttribute);
			if (xAttribute != null)
			{
				var valueStr = xAttribute.Value;
				if (!String.IsNullOrEmpty(valueStr))
				{
					valueStr = valueStr.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
					valueStr = valueStr.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

					var value = Convert.ToDouble(valueStr);
					predicate(value);
				}
			}
		}

		private void UpdateSplitterInfo(SplitterInfo splitterInfo)
		{
			if (_splitter.Orientation == Orientation.Horizontal)
				splitterInfo.HorizontalPercentage = ViewsManager.GetChildPercentageFromDimension(_splitter.Height, _splitter.SplitterDistance);
			else
				splitterInfo.VerticalPercentage = ViewsManager.GetChildPercentageFromDimension(_splitter.Width, _splitter.SplitterDistance);
		}

		private void UpdateSplitterDistance(SplitterInfo splitterInfo)
		{
			if (_splitter.Orientation == Orientation.Horizontal)
				_splitter.SplitterDistance = ViewsManager.GetChildDimensionFromPercentage(_splitter.Height, splitterInfo.HorizontalPercentage);
			else
				_splitter.SplitterDistance = ViewsManager.GetChildDimensionFromPercentage(_splitter.Width, splitterInfo.VerticalPercentage);
		}

		#endregion
	}

	[DebuggerDisplay("Synegies = {RegExpSynergies.Count}")]
	public class DefaultViewData
	{
		#region Fields

		public List<RegExpSynergy> RegExpSynergies { get; set; }
		public ExportNotesOptions ExportNotesOptions { get; set; }

		#endregion

		#region Ctors

		public DefaultViewData()
		{
			this.RegExpSynergies = new List<RegExpSynergy>();
			this.ExportNotesOptions = new ExportNotesOptions();
		}

		#endregion

		#region Static operations

		public static DefaultViewData Load(string json)
		{
			if (!String.IsNullOrEmpty(json))
				return JsonConvert.DeserializeObject<DefaultViewData>(json);

			return new DefaultViewData();
		}

		#endregion
	}
}