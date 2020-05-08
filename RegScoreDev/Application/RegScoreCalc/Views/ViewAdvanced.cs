using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Xml.Linq;

namespace RegScoreCalc
{
	public class ViewAdvanced : View
	{
        #region Fields

		protected SplitContainer _splitter;
		protected SplitContainer _splitter2;

		protected PaneAdvRegExp _paneAdvRegExp;
		protected PaneDocuments _paneDocuments;
		protected PaneAdvNotes _paneAdvNotes;
		protected PaneCodes _paneCodes;
		protected RibbonButton _btnLayout1;
		protected RibbonButton _btnLayout2;

	    protected FormNotes _formNotes;
		protected Rectangle _initialWindowRect;
		protected FormWindowState _initialWindowState = FormWindowState.Normal;
	    protected bool _isNotesDetached;
		
		#endregion

		#region Ctors

		public ViewAdvanced(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
			_strLayoutName = "Layout 1";
		}

		#endregion

		#region Events

		protected void OnDataModified(object sender, EventArgs e)
		{
			UpdateView();
		}

		protected void OnMaximizeNotes_Clicked(object sender, EventArgs e)
		{
			MaximizeNotesView();
		}

		private void OnNotesSeparateWindow_Clicked(object sender, EventArgs e)
        {
			DetachNotesPane();
        }

		private void OnResetView_Clicked(object sender, EventArgs e)
        {
            ResetView();
        }

		protected void OnLayout1_Clicked(object sender, EventArgs e)
		{
			ApplyLayout("Layout 1");

			UpdateView();
		}

		protected void OnLayout2_Clicked(object sender, EventArgs e)
		{
			ApplyLayout("Layout 2");

			UpdateView();
		}

		private void paneAdvNotes_CalcScores(object sender, EventArgs e)
		{
			_paneAdvRegExp.CalcScores();
		}

		private void paneAdvNotes_RefreshHighlights(object sender, EventArgs e)
		{
			_paneAdvRegExp.RefreshHighlights();
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
			btnMaximizeNotes.Click += new EventHandler(OnMaximizeNotes_Clicked);
			btnMaximizeNotes.Image = Properties.Resources.MaximizeNotes;
            btnMaximizeNotes.SmallImage = Properties.Resources.MaximizeNotes;

            btnMaximizeNotes.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnNotesSeparateWindow = new RibbonButton("Open Notes in Window");
			panel.Items.Add(btnNotesSeparateWindow);
			btnNotesSeparateWindow.Image = Properties.Resources.NotesSeparateWindow;
			btnNotesSeparateWindow.SmallImage = Properties.Resources.NotesSeparateWindow;
			btnNotesSeparateWindow.Click += new EventHandler(OnNotesSeparateWindow_Clicked);
			btnNotesSeparateWindow.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

            RibbonButton btnResetView = new RibbonButton("Reset Layout"); 
            panel.Items.Add(btnResetView);
			btnResetView.Click += new EventHandler(OnResetView_Clicked);
			btnResetView.Image = Properties.Resources.ResetView;
            btnResetView.SmallImage = Properties.Resources.ResetView;

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
		
		}

		protected override void InitViewPanes(RibbonTab tab)
		{
			this.Orientation = Orientation.Horizontal;

			//////////////////////////////////////////////////////////////////////////

			_splitter = new SplitContainer();
			_splitter.Name = "Splitter1";
			_splitter.Orientation = Orientation.Vertical;
			_splitter.Dock = DockStyle.Fill;
			_splitter.BorderStyle = BorderStyle.Fixed3D;
			_splitter.BackColor = MainForm.ColorBackground;
			_splitter.Panel1MinSize = 0;
			_splitter.Panel2MinSize = 0;
			this.Panel1.Controls.Add(_splitter);

			_splitter2 = new SplitContainer();
			_splitter2.Name = "Splitter2";
			_splitter2.BorderStyle = BorderStyle.Fixed3D;
			_splitter2.Orientation = Orientation.Vertical;
			_splitter2.Dock = DockStyle.Fill;
			_splitter2.BackColor = MainForm.ColorBackground;
			_splitter2.Panel1MinSize = 0;
			_splitter2.Panel2MinSize = 0;
			this.Panel2.Controls.Add(_splitter2);

			//////////////////////////////////////////////////////////////////////////

			_paneAdvRegExp = new PaneAdvRegExp();
			_paneAdvRegExp.InitPane(_views, this, this.Panel1, tab);
			_paneAdvRegExp._eventDataModified += new EventHandler(OnDataModified);

			_splitter.Panel1.Controls.Add(_paneAdvRegExp);
			_paneAdvRegExp.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			_paneDocuments = new PaneDocuments();
			_paneDocuments.InitPane(_views, this, this.Panel1, tab);
			_paneDocuments._eventDataModified += new EventHandler(OnDataModified);

			_splitter2.Panel1.Controls.Add(_paneDocuments);
			_paneDocuments.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			_paneAdvNotes = new PaneAdvNotes();
			_paneAdvNotes.InitPane(_views, this, this.Panel1, tab);
			_paneAdvNotes._eventDataModified += new EventHandler(OnDataModified);
			_paneAdvNotes.CalcScores += paneAdvNotes_CalcScores;
			_paneAdvNotes.RefreshHighlights += new EventHandler(paneAdvNotes_RefreshHighlights);

			_splitter2.Panel2.Controls.Add(_paneAdvNotes);
			_paneAdvNotes.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			_paneCodes = new PaneCodes();
			_paneCodes.InitPane(_views, this, this.Panel1, tab);
			_paneCodes._eventDataModified += new EventHandler(OnDataModified);

			_splitter.Panel2.Controls.Add(_paneCodes);
			_paneCodes.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			ResetView();

			_paneDocuments.Select();
		}

		public override void UpdateView()
		{
			if (_paneAdvRegExp != null)
				_paneAdvRegExp.UpdatePane();

			if (_paneDocuments != null)
				_paneDocuments.UpdatePane();

			if (_paneAdvNotes != null)
				_paneAdvNotes.UpdatePane();

			if (_paneCodes != null)
				_paneCodes.UpdatePane();

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
			_paneAdvNotes.DestroyPane();

			base.DestroyView();
		}

		public override bool OnHotkey(string code)
		{
			var handled = _paneDocuments.OnHotkey(code);
			if (!handled)
				handled = _paneAdvNotes.OnHotkey(code);

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

			var xPaneState = new XElement("NotesPaneState",
				new XAttribute("IsDetached", _isNotesDetached));

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
			}
		}
		#endregion

		#region Implementation

		protected void MaximizeNotesView()
		{
			RestoreNotesPane();

			this.Panel1Collapsed = true;
			_splitter2.Panel1Collapsed = true;
		}

		protected void DetachNotesPane()
		{
			if (_formNotes == null)
			{
				_formNotes = new FormNotes(_paneAdvNotes);
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

				_splitter2.Panel2Collapsed = true;

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

				_splitter2.Panel2Collapsed = false;
			}
		}

		protected void ResetView()
		{
			RestoreNotesPane();

			this.Panel1Collapsed = false;
			this.Panel2Collapsed = false;

			_splitter.Panel1Collapsed = false;
			_splitter.Panel2Collapsed = false;

			_splitter2.Panel1Collapsed = false;
			_splitter2.Panel2Collapsed = false;

			_splitter.SplitterDistance = _splitter.Width / 100 * 85;
			_splitter2.SplitterDistance = _splitter2.Width / 100 * 40;
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

		#endregion
	}
}
