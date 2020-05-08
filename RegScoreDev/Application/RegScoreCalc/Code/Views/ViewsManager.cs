using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using DocumentsServiceInterfaceLib;

using Newtonsoft.Json;

using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;

using RegScoreCalc.Views;

namespace RegScoreCalc
{
	public class ViewsManager
	{
		#region Delegates

		public event MatchNavigateEventHandler OnMatchNavigate;

		public delegate void MatchNavigateEventHandler(RegExpMatchResult result);

        public event FontChangeEventHandler OnFontChanged;
        public delegate void FontChangeEventHandler();

        public event LineSpacingEventHandler OnLineSpacingChanged;
        public delegate void LineSpacingEventHandler();

        public event NoteColumnsChangeEventHandler OnNoteColumnsChanged;
        public delegate void NoteColumnsChangeEventHandler();

        #endregion

        #region Fields

        protected MainForm _form;
		protected Ribbon _ribbon;

		protected Panel _panelAntiFlicker;

		protected List<ViewType> _listViewTypes;
		protected List<View> _listViews;

		protected const string _strConfigFileName = "RegScoreCalc.cfg";
		protected const string _strLayoutFileName = "Layout.xml";
		protected const string _strHotkeysFileName = "Hotkeys.xml";

		protected string _strAvailableViews;
		protected string _strAutoOpenViews;
		protected string _strHtmlAvailableViews;
		protected string _strHtmlAutoOpenViews;

		protected string _strActiveView;

		protected bool _isNavigating;

		protected string _strFontFamily;
		protected float _fFontSize;
		protected FontStyle _styleFont;
		protected int _nLineSpacing;
		protected int _nAutoCalc;
		protected bool _bSortByDocumentScore;
		protected bool _bShowOldCategoryColumn;
		protected bool _bHighlightBillingMatches;
		protected bool _bOnlyPositiveScores;
		protected bool _bDocumentsServiceDebug;
		protected int _nDocumentsServiceQueryLimit;
		protected Font _selectCategoryFont;
		protected int _pythonPort;

		protected bool _bNewDatabaseMode;
		protected bool _bCategoriesTablesExist;

        protected string _strAnacondaPath;
        protected string _strPythonEnv;
        protected int _nPythonVersion;

		protected List<HotkeyInfo> _hotkeys;

		protected string _strSNOMEDBaseAddress;

		protected SynchronizationContext _context;

		protected IDocumentsService _documentsService;
		protected NavigationContext _navigationContext;

		protected bool _isDocumentsTableLoading;

		public event BeforeViewCreateEventHandler BeforeViewCreate;

		public delegate bool BeforeViewCreateEventHandler(ViewType viewType);

        protected List<TabSetting> _tabSettings;

		#endregion

		#region Properties

		public MainForm MainForm
		{
			get { return _form; }
		}

		public Ribbon Ribbon
		{
			get { return _ribbon; }
		}

		public IDocumentsService DocumentsService
		{
			get { return _documentsService; }
		}

		public string SNOMEDBaseAddress
		{
			get { return _strSNOMEDBaseAddress; }
			set { _strSNOMEDBaseAddress = value; }
		}

		public string FontFamily
		{
			get { return _strFontFamily; }
			set { _strFontFamily = value; }
		}

		public float FontSize
		{
			get { return _fFontSize; }
			set { _fFontSize = value; }
		}

		public FontStyle FontStyle
		{
			get { return _styleFont; }
			set { _styleFont = value; }
		}

		public int LineSpacing
		{
			get { return _nLineSpacing; }
			set { _nLineSpacing = value; }
		}

		public int DocumentsServiceQueryLimit
		{
			get { return _nDocumentsServiceQueryLimit; }
			set { _nDocumentsServiceQueryLimit = value; }
		}

		public int AutoCalc
		{
			get { return _nAutoCalc; }
			set { _nAutoCalc = value; }
		}

		public bool SortByDocumentScore
		{
			get { return _bSortByDocumentScore; }
			set { _bSortByDocumentScore = value; }
		}

		public bool ShowOldCategoryColumn
		{
			get { return _bShowOldCategoryColumn; }
			set { _bShowOldCategoryColumn = value; }
		}

		public bool HighlightBillingMatches
		{
			get { return _bHighlightBillingMatches; }
			set { _bHighlightBillingMatches = value; }
		}

		public bool OnlyPositiveScores
		{
			get { return _bOnlyPositiveScores; }
			set { _bOnlyPositiveScores = value; }
		}

		public bool DocumentsServiceDebug
		{
			get { return _bDocumentsServiceDebug; }
			set { _bDocumentsServiceDebug = value; }
		}

		public Font SelectCategoryFont
		{
			get { return _selectCategoryFont; }
			set { _selectCategoryFont = value; }
		}

		public bool IsNavigating
		{
			get { return _isNavigating; }
		}

		public bool FreezeSort { get; set; }

		public int PythonPort
		{
			get { return _pythonPort; }
			set { _pythonPort = value; }
		}

		public bool IsDocumentsTableLoading
		{
			get { return _isDocumentsTableLoading; }
		}

        public string AnacondaPath
        {
            get { return _strAnacondaPath;  }
            set { _strAnacondaPath = value; }
        }
           
        public string PythonEnv
        {
            get { return String.IsNullOrEmpty(_strPythonEnv) ? "NULL" : _strPythonEnv;  }
            set { _strPythonEnv = value; }
        }

        public int PythonVersion
        {
            get { return _nPythonVersion == 3 ? 3 : 2; }
            set { _nPythonVersion = value; }
        }

        public List<TabSetting> TabSettings
        {
            get { return _tabSettings; }
            set { _tabSettings = value; }
        }
		#endregion

		#region Ctors

		public ViewsManager(MainForm form, Ribbon ribbon)
		{
			_form = form;
			_ribbon = ribbon;

			_listViewTypes = new List<ViewType>();
			_listViews = new List<View>();

			_ribbon.ActiveTabChanged += OnActiveTabChanged;

			_bNewDatabaseMode = false;

			_bSortByDocumentScore = false;

			_bShowOldCategoryColumn = true;

            _tabSettings = new List<TabSetting>();

			_context = SynchronizationContext.Current;
			_hotkeys = new List<HotkeyInfo>();

			_documentsService = new LocalDocumentsServiceLib.DocumentsServiceClient();
			_navigationContext = new NavigationContext();

			_form.Closing += OnClosing;

			_nDocumentsServiceQueryLimit = 500;

			CreateAntiFlickerPanel();

			LoadConfig();

			LoadHotkeys();
		}

		protected void CreateAntiFlickerPanel()
		{
			_panelAntiFlicker = new Panel();

			_panelAntiFlicker.Left = this.Ribbon.Left;
			_panelAntiFlicker.Top = this.Ribbon.Bottom;
			_panelAntiFlicker.Width = this.Ribbon.Width;
			_panelAntiFlicker.Height = (this.MainForm.Height - _panelAntiFlicker.Top) - 50;
			_panelAntiFlicker.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			_panelAntiFlicker.BackColor = MainForm.ColorBackground;
			_panelAntiFlicker.BorderStyle = BorderStyle.None;
			_panelAntiFlicker.Visible = false;

			var lbl = new Label
			{
				AutoSize = false,
				TextAlign = ContentAlignment.MiddleCenter,
				Font = new Font("Calibri", 20, FontStyle.Italic),
				Dock = DockStyle.Fill
			};

			_panelAntiFlicker.Controls.Add(lbl);

			this.MainForm.Controls.Add(_panelAntiFlicker);
		}

		protected void ShowAntiFlickerPanel(string message)
		{
			_panelAntiFlicker.Visible = true;
			_panelAntiFlicker.BringToFront();

			_panelAntiFlicker.Controls[0].Text = "Loading " + message + " view...";
		}

		protected void HideAntiFlickerPanel()
		{
			_panelAntiFlicker.Visible = false;
		}

		#endregion

		#region Events

		protected void OnClosing(object sender, CancelEventArgs args)
		{
			try
			{
				_documentsService.Exit();
			}
			catch (System.Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		public void OnCloseView_Clicked(object sender, EventArgs e)
		{
			try
			{
				if (sender is RibbonButton)
				{
					RibbonButton btnCloseView = (RibbonButton)sender;

					RibbonTab tab;
					for (int i = 0; i < _ribbon.Tabs.Count; i++)
					{
						tab = _ribbon.Tabs[i];
						if (tab == btnCloseView.OwnerTab)
						{
							DestroyView(i - 1);
							break;
						}
					}
				}
			}
			catch
			{
			}
		}

		protected void OnOrbItemClick(object sender, EventArgs e)
		{
			try
			{
				if (sender is RibbonItem)
				{
					var item = (RibbonItem)sender;
					CreateView(item.Text, true, false);
				}
			}
			catch
			{
			}
		}

		protected void OnComboBoxButtonClick(object sender, EventArgs e)
		{
			try
			{
				if (sender is RibbonButton)
				{
					RibbonButton btn = (RibbonButton)sender;
					RibbonComboBox combobox = GetTypesComboBox();
					if (combobox != null)
						combobox.TextBoxText = btn.Text;
				}
			}
			catch
			{
			}
		}

		protected void OnActiveTabChanged(object sender, EventArgs e)
		{
			if (_ribbon.ActiveTab != null)
			{
				foreach (View view in _listViews)
				{
					if (_ribbon.ActiveTab.Tag == view)
					{
						if (!view.Visible)
						{
							ShowAntiFlickerPanel(view.Name);

							HideAllViews();

							view.ShowView();

							HideAntiFlickerPanel();
						}

						break;
					}
				}
			}
		}

		#endregion

		#region Operations

		public bool AddViewType(string viewName, Type type, bool bNewDatabaseOnly, bool bDocumentsRequired, bool bRegExpsRequired, bool allowMultipleOpenViews = true, bool htmlView = false)
		{
			bool bResult = false;

			var viewType = FindViewType(viewName);
			if (viewType == null)
			{
				try
				{
					if (htmlView)
					{
						var htmlViewInfoList = GetHtmlViewInfoList();
						foreach (var viewInfo in htmlViewInfoList)
						{
							if (IsViewTypeAvailable(viewInfo.Name, true))
							{
								viewType = new ViewType(viewInfo.Name, type, bNewDatabaseOnly, bDocumentsRequired, bRegExpsRequired, allowMultipleOpenViews, viewInfo);
								_listViewTypes.Add(viewType);

								bResult = true;
							}
						}
					}
					else
					{
						if (IsViewTypeAvailable(viewName, false))
						{
							viewType = new ViewType(viewName, type, bNewDatabaseOnly, bDocumentsRequired, bRegExpsRequired, allowMultipleOpenViews, null);
							_listViewTypes.Add(viewType);

							return true;
						}
						else
							return false;
					}
				}
				catch
				{
				}
			}

			return bResult;
		}

		public int GetViewIndex(string name)
		{
			for (int i = 0; i < _listViews.Count; i++)
			{
				if (_listViews[i].ViewType.Name == name)
					return i;
			}
			return -1;
		}

		public ViewSVM GetSVMView(int position)
		{
			return (ViewSVM)_listViews[position];
		}

		public string[] GetViewTypesList()
		{
			string[] arrNames = null;

			try
			{
				if (_listViewTypes.Count > 0)
				{
					arrNames = new string[_listViewTypes.Count];

					ViewType viewtype;
					for (int i = 0; i < _listViewTypes.Count; i++)
					{
						viewtype = (ViewType)_listViewTypes[i];
						arrNames.SetValue(viewtype.Name, i);
					}
				}
				else
					arrNames = new string[0];
			}
			catch
			{
			}

			return arrNames;
		}

		public bool CreateView(string strViewName, bool bActivate, bool silent, object objArgument = null, string tabTitle = null)
		{
			bool bResult = false;

			if (!String.IsNullOrEmpty(strViewName))
			{
				var viewType = FindViewType(strViewName);
				if (viewType != null)
				{
					if (!CloseViewsIfNeeded(viewType, silent))
						return false;

					ShowAntiFlickerPanel(strViewName);

					if (this.BeforeViewCreate != null)
					{
						if (!this.BeforeViewCreate(viewType))
							return false;
					}

					try
					{
						if (String.IsNullOrEmpty(tabTitle))
							tabTitle = strViewName;

						string strTitle = FormatViewTitle(tabTitle);

						object[] args = new object[] { viewType, strTitle, this, objArgument };

						object objView = Activator.CreateInstance(viewType.Type, args);

						View view;

						var htmlView = objView as ViewHtmlBase;
						if (htmlView != null)
						{
							var viewInfo = GetHtmlViewInfo(strViewName);
							if (!viewInfo.IsValid)
								throw new Exception("Cannot open " + strViewName + " view: required files are missing.");

							viewInfo.View = htmlView;

							htmlView.HtmlViewInfo = viewInfo;
							view = htmlView;
						}
						else
							view = objView as View;

						if (view != null)
						{
							view.Text = strTitle;

							HideAllViews();

							view.ShowView();

							_listViews.Add(view);

							//////////////////////////////////////////////////////////////////////////

							string strLayoutName = GetViewLayoutName(view);
							if (!String.IsNullOrEmpty(strLayoutName))
							{
								view.LayoutName = strLayoutName;
								LoadLayout(view);

								view.UpdateView();
							}
							else
								view.ResetView();

							//////////////////////////////////////////////////////////////////////////

							if (bActivate)
								bResult = ActivateView(_listViews.Count - 1);
							else
								bResult = true;

							SetDoubleBufferedGrids(view);
						}
					}
					catch (Exception ex)
					{
						if (ex.InnerException != null)
							MessageBox.Show(ex.InnerException.Message);
						else
							MainForm.ShowExceptionMessage(ex);
					}
				}

				HideAntiFlickerPanel();
			}

			return bResult;
		}

		public string[] GetAutoOpenViews()
		{
			if (!String.IsNullOrEmpty(_strAutoOpenViews))
				return _strAutoOpenViews.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

			return new string[0];
		}

		public bool CreateSelectedView()
		{
			bool bResult = false;

			try
			{
				RibbonComboBox combobox = GetTypesComboBox();
				if (combobox != null)
					bResult = CreateView(combobox.TextBoxText, true, false);
			}
			catch
			{
			}

			return bResult;
		}

		protected bool CloseViewsIfNeeded(ViewType viewType, bool silent)
		{
			if (viewType.AllowMultipleViews)
				return true;

			var openedViews = _listViews.Where(x => x.ViewType.Name == viewType.Name)
										.ToList();

			if (!openedViews.Any())
				return true;

			///////////////////////////////////////////////////////////////////////////////

			var closeViews = silent;

			if (!closeViews)
			{
				var dlgres = MessageBox.Show("Only one " + viewType.Name + " view can be opened at a time." + Environment.NewLine + Environment.NewLine + "Do you wish to close the existing view and open new one?", MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button1);
				if (dlgres == DialogResult.Yes)
					closeViews = true;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (closeViews)
			{
				openedViews.ForEach(x => DestroyView(x, false));

				GC.Collect();
				GC.WaitForPendingFinalizers();

				return true;
			}

			return false;
		}

		public string GetSelectedViewName()
		{
			RibbonComboBox combobox = GetTypesComboBox();
			if (combobox != null)
				return combobox.TextBoxText;
			return "";
		}

		public bool ActivateView(int nIndex)
		{
			bool bResult = false;

			try
			{
				if (nIndex >= 0 && nIndex < _listViews.Count)
				{
					if (nIndex < this.Ribbon.Tabs.Count + 1)
					{
						var ribbonTab = this.Ribbon.Tabs[nIndex + 1];
						var view = (View)ribbonTab.Tag;

						ShowAntiFlickerPanel(view.Name);

						HideAllViews();
						this.Ribbon.ActiveTab = ribbonTab;

						HideAntiFlickerPanel();

						bResult = true;
					}
				}
			}
			catch
			{
			}

			return bResult;
		}

		public View GetActiveView()
		{
			if (_ribbon.ActiveTab != null)
				return _ribbon.ActiveTab.Tag as View;

			return null;
		}

		public List<View> GetAllOpenedViews()
		{
			return _listViews;
		}

		public List<T> GetOpenedViewsByType<T>() where T : View
		{
			var type = typeof(T);

			return _listViews.Where(x => x.ViewType.Type == type)
							 .Cast<T>()
							 .ToList();
		}

		public void UpdateViews()
		{
			foreach (View view in _listViews)
			{
				view.UpdateView();
			}
		}

		public bool DestroyView(int nIndex, bool collect = true)
		{
			bool bResult = false;

			try
			{
				if (nIndex >= 0 && nIndex < _listViews.Count)
				{
					View view = (View)_listViews[nIndex];
					if (view != null)
					{
						DestroyView(view);

						bResult = true;
					}
				}
			}
			catch
			{
			}

			return bResult;
		}

		protected void DestroyView(View view, bool collect = true)
		{
			SaveLayout(view);

			view.DestroyView();

			_listViews.Remove(view);

			if (collect)
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}
		}

		public void HideAllViews()
		{
			try
			{
				foreach (View view in _listViews)
				{
					view.HideView();
				}
			}
			catch
			{
			}
		}

		public void DestroyAllViews()
		{
			try
			{
				foreach (View view in _listViews)
				{
					SaveLayout(view);

					view.DestroyView();
				}

				_listViews.Clear();
			}
			catch
			{
			}
		}

		public void AutoOpenViews(bool bNewVersion, bool bDocumentsLoaded, bool bRegExpsLoaded)
		{
			try
			{
				int nViewCounter = 0;
				int nActiveViewIndex = -1;

				bool bAlreadyOpened;

				ViewType viewtypeActive = null;
				foreach (ViewType viewtype in _listViewTypes)
				{
					if (IsViewTypeAutoOpen(viewtype))
					{
						if (IsDatabaseSupported(viewtype, bNewVersion, bDocumentsLoaded, bRegExpsLoaded))
						{
							bAlreadyOpened = false;
							foreach (View view in _listViews)
							{
								if (view.ViewType.Type == viewtype.Type)
								{
									view.UpdateView();

									bAlreadyOpened = true;
									break;
								}
							}

							if (!bAlreadyOpened)
							{
								if (CreateView(viewtype.Name, false, true))
								{
									if (IsViewTypeActive(viewtype))
									{
										viewtypeActive = viewtype;
										nActiveViewIndex = nViewCounter;
									}

									nViewCounter++;
								}
							}
						}
					}

					if (viewtypeActive != null && nActiveViewIndex != -1)
						ActivateView(nActiveViewIndex);
				}
			}
			catch
			{
			}
		}

		public void HandleDatabaseLoad(bool bNewVersion, bool bDocumentsLoaded, bool bRegExpsLoaded)
		{
			bool bClosed = false;

			View view;
			for (int i = 0; i < _listViews.Count; i++)
			{
				view = (View)_listViews[i];
				if (view != null)
				{
					if (!IsDatabaseSupported(view.ViewType, bNewVersion, bDocumentsLoaded, bRegExpsLoaded))
					{
						view.DestroyView();
						_listViews.RemoveAt(i);
						i--;

						bClosed = true;
					}
					else
						view.UpdateView();
				}
			}

			UpdateViewsListUI(bNewVersion, bDocumentsLoaded, bRegExpsLoaded);

			AutoOpenViews(bNewVersion, bDocumentsLoaded, bRegExpsLoaded);

			if (bClosed)
				MessageBox.Show("Some open views have to be close", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void BeforeDocumentsTableLoad(bool removeColumns)
		{
			_isDocumentsTableLoading = true;

			foreach (View view in _listViews)
			{
				view.BeforeDocumentsTableLoad(removeColumns);
			}
		}

		public void AfterDocumentsTableLoad(bool addColumns)
		{
			foreach (View view in _listViews)
			{
				view.AfterDocumentsTableLoad(addColumns);
			}

			_isDocumentsTableLoading = false;
		}

		public void InvokeRefreshHighlights()
		{
			InvokeOnMatchNavigate(MatchNavigationMode.Refresh, null, null);
		}

		public void InvokeOnMatchNavigate(RegExpMatchResult result)
		{
			try
			{
				InvokeOnMatchNavigateOnMainThread(result);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		public void InvokeOnMatchNavigate(MatchNavigationMode mode, RegExpBase regExp, BackgroundWorker worker)
		{
			try
			{
				if (this.OnMatchNavigate == null)
					return;

				if (mode != MatchNavigationMode.Refresh)
				{
					if (regExp == null)
						return;

					if (regExp.Regex == null)
					{
						regExp.Build();
					}

					if (String.IsNullOrEmpty(regExp.BuiltExpression))
						throw new Exception("Regular expression is empty");

					///////////////////////////////////////////////////////////////////////////////

					var source = this.MainForm.sourceDocuments;
					source.RaiseListChangedEvents = false;

					var blockStartPosition = source.Position;

					var navigationContextID = _navigationContext.GetNavigationContextID(source.SortProperty != null ? source.SortProperty.Name : String.Empty,
						source.SortDirection == ListSortDirection.Ascending ? "ASC" : "DESC",
						source.Filter);

					var totalDocumentsCount = source.Count;

					var forward = IsForwardNavigation(mode);

					var result = RegExpMatchResult.NeedMoreDataResult();
					while (result.NeedMoreData)
					{
						if (worker.CancellationPending)
							return;

						///////////////////////////////////////////////////////////////////////////////

						var documentsBlock = GetNextDocumentsBlock(mode, blockStartPosition);
						if (documentsBlock != null)
						{
							result = _documentsService.Navigate(navigationContextID, regExp, mode, totalDocumentsCount, documentsBlock, blockStartPosition);

							blockStartPosition += forward ? documentsBlock.Length : -documentsBlock.Length;

							var progress = ((forward ? blockStartPosition : totalDocumentsCount - blockStartPosition) / (double) totalDocumentsCount) * 100;
							worker.ReportProgress((int) progress);
						}
						else
							result = null;

						if (result == null || (result.IsEmpty && !result.NeedMoreData))
						{
							if (!worker.CancellationPending)
								MessageBox.Show("No matches found", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

							source.RaiseListChangedEvents = true;

							return;
						}
					}

					///////////////////////////////////////////////////////////////////////////////

					_isNavigating = true;

					_context.Send(InvokeOnMatchNavigateOnMainThread, result);
				}
				else
					_context.Send(InvokeRefreshHighlightsOnMainThread, null);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
			finally
			{
				_isNavigating = false;
				this.MainForm.sourceDocuments.RaiseListChangedEvents = true;
			}
		}

        #endregion

        #region Implementation
        
        protected bool IsForwardNavigation(MatchNavigationMode mode)
		{
			bool forward;

			switch (mode)
			{
				case MatchNavigationMode.NextDocument:
				case MatchNavigationMode.NextMatch:
				case MatchNavigationMode.NextUniqueMatch:
					forward = true;
					break;

				case MatchNavigationMode.PrevDocument:
				case MatchNavigationMode.PrevMatch:
				case MatchNavigationMode.PrevUniqueMatch:
					forward = false;
					break;

				case MatchNavigationMode.Invalidate:
				case MatchNavigationMode.Refresh:
				default:
					throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
			}

			return forward;
		}

		protected double[] GetNextDocumentsBlock(MatchNavigationMode mode, int currentPosition)
		{
			var source = this.MainForm.sourceDocuments;
			var count = source.Count;
			if (currentPosition < 0 || currentPosition >= count)
				return null;

			///////////////////////////////////////////////////////////////////////////////

			var forward = IsForwardNavigation(mode);

			var documents = new List<double>(_nDocumentsServiceQueryLimit);

			for (var i = 0; i < _nDocumentsServiceQueryLimit; i++)
			{
				if (currentPosition >= 0 && currentPosition < count)
				{
					var rowView = (DataRowView) source[currentPosition];
					var documentID = (double) rowView.Row["ED_ENC_NUM"];

					documents.Add(documentID);
				}
				else
				{
					break;
				}

				currentPosition += forward ? 1 : -1;
			}

			///////////////////////////////////////////////////////////////////////////////

			return documents.ToArray();
		}

        public void InvokeRefreshFont()
        {
            if (this.OnFontChanged != null)
                this.OnFontChanged();
        }

        public void InvokeRefreshTabSettings()
        {
            _tabSettings = DocumentsService.GetDocumentColumnSettings();
                       

            if (this.OnNoteColumnsChanged != null)
            {
                this.OnNoteColumnsChanged();
            }
        }
        public void SaveTabSettings(List<TabSetting> tabSettings)
        {
            DocumentsService.SetDocumentColumnSettings(tabSettings);
        }
        protected void InvokeOnMatchNavigateOnMainThread(object state)
		{
			try
			{
				var result = (RegExpMatchResult)state;

				this.MainForm.sourceDocuments.RaiseListChangedEvents = true;
				this.MainForm.sourceDocuments.Position = result.Position;

				_isNavigating = false;

				this.OnMatchNavigate(result);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void InvokeRefreshHighlightsOnMainThread(object state)
		{
			try
			{
				this.OnMatchNavigate(RegExpMatchResult.RefreshResult());
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

        internal void InvokeRefreshLineSpacing()
        {
            if (this.OnLineSpacingChanged != null)
                this.OnLineSpacingChanged();
        }
        
        protected ViewType FindViewType(string strName)
		{
			ViewType viewtypeResult = null;

			try
			{
				if (!String.IsNullOrEmpty(strName))
				{
					foreach (ViewType viewtype in _listViewTypes)
					{
						if (String.Compare(viewtype.Name, strName, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							viewtypeResult = viewtype;
							break;
						}
					}
				}
			}
			catch
			{
			}

			return viewtypeResult;
		}

		protected string FormatViewTitle(string viewTitle)
		{
			try
			{
				int nCounter = 0;
				foreach (View view in _listViews)
				{
					if (String.Compare(view.Text, viewTitle, StringComparison.OrdinalIgnoreCase) == 0)
						nCounter++;
				}

				if (nCounter > 0)
				{
					nCounter++;

					viewTitle += " (" + nCounter.ToString() + ")";
				}
			}
			catch
			{
			}

			return viewTitle;
		}

		public bool IsViewTypeAvailable(string viewName, bool htmlView)
		{
			bool bResult = false;

			try
			{
				var availableViews = htmlView ? _strHtmlAvailableViews : _strAvailableViews;

				if (!String.IsNullOrEmpty(availableViews))
				{
					if (!htmlView)
					{
						if (viewName.StartsWith("ReviewML NEW"))
						{
							if (viewName != "ReviewML NEW")
								viewName = viewName.Substring(0, viewName.Length - 1);
						}
						else if (viewName.StartsWith("ReviewML"))
						{
							if (viewName != "ReviewML")
								viewName = viewName.Substring(0, viewName.Length - 1);
						}
					}

					var arrAutoOpenViews = availableViews.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
					if (arrAutoOpenViews.Any(strView => String.Compare(strView, viewName, StringComparison.InvariantCultureIgnoreCase) == 0))
						bResult = true;
				}
			}
			catch
			{
			}

			return bResult;
		}

		protected bool IsViewTypeAutoOpen(ViewType viewType)
		{
			bool bResult = false;

			try
			{
				var autoOpenViews = viewType.HtmlViewInfo != null ? _strHtmlAutoOpenViews : _strAutoOpenViews;

				if (!String.IsNullOrEmpty(autoOpenViews))
				{
					var arrAutoOpenViews = autoOpenViews.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
					if (arrAutoOpenViews.Any(strView => String.Compare(strView, viewType.Name, StringComparison.InvariantCultureIgnoreCase) == 0))
						bResult = true;
				}
			}
			catch
			{
			}

			return bResult;
		}

		protected bool IsViewTypeActive(ViewType viewtype)
		{
			bool bResult = false;

			try
			{
				if (String.Compare(viewtype.Name, _strActiveView, true) == 0)
					bResult = true;
			}
			catch
			{
			}

			return bResult;
		}

		protected bool LoadConfig()
		{
			bool bResult = false;

			try
			{
				string strConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
																				  .Location), _strConfigFileName);
				if (File.Exists(strConfigPath))
				{
					XmlTextReader xmlReader = new XmlTextReader(strConfigPath);

					xmlReader.Read();
					xmlReader.Read();
					xmlReader.Read();

					if (xmlReader.Name == "Configuration")
					{
						string strOption;

						while (xmlReader.Read() && xmlReader.Read())
						{
							if (xmlReader.Name == "option")
							{
								strOption = xmlReader.GetAttribute("name");

								xmlReader.Read();

								if (strOption == "AvailableViews")
									_strAvailableViews = xmlReader.Value;
								else if (strOption == "AutoOpenViews")
									_strAutoOpenViews = xmlReader.Value;
								else if (strOption == "HTMLAvailableViews")
									_strHtmlAvailableViews = xmlReader.Value;
								else if (strOption == "HTMLAutoOpenViews")
									_strHtmlAutoOpenViews = xmlReader.Value;
								else if (strOption == "ActiveView")
									_strActiveView = xmlReader.Value;
								else if (strOption == "SNOMEDBaseAddress")
									_strSNOMEDBaseAddress = xmlReader.Value;
								else if (strOption == "FontFamily")
									_strFontFamily = xmlReader.Value;
								else if (strOption == "FontSize")
								{
									try
									{
										_fFontSize = (float)Convert.ToDouble(xmlReader.Value);
										if (_fFontSize <= 0 || _fFontSize > 72)
											_fFontSize = 14;
									}
									catch
									{
										_fFontSize = 14;
									}
								}
								else if (strOption == "FontStyle")
								{
									try
									{
										_styleFont = (FontStyle)Convert.ToInt32(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "LineSpacing")
								{
									try
									{
										_nLineSpacing = Convert.ToInt32(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "DocumentsServiceQueryLimit")
								{
									try
									{
										_nDocumentsServiceQueryLimit = Convert.ToInt32(xmlReader.Value);
										if (_nDocumentsServiceQueryLimit <= 0 || _nDocumentsServiceQueryLimit > 10000)
											_nDocumentsServiceQueryLimit = 500;
									}
									catch
									{
									}
								}
								else if (strOption == "AutoCalc")
								{
									try
									{
										_nAutoCalc = Convert.ToInt32(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "AutoSort")
								{
									try
									{
										_bSortByDocumentScore = Convert.ToBoolean(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "ShowOldCategoryColumn")
								{
									try
									{
										_bShowOldCategoryColumn = Convert.ToBoolean(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "HighlightBillingMatches")
								{
									try
									{
										_bHighlightBillingMatches = Convert.ToBoolean(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "OnlyPositiveScores")
								{
									try
									{
										_bOnlyPositiveScores = Convert.ToBoolean(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "DocumentsServiceDebug")
								{
									try
									{
										_bDocumentsServiceDebug = Convert.ToBoolean(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "SelectCategoryFont")
								{
									try
									{
										_selectCategoryFont = FontFromString(xmlReader.Value);
									}
									catch
									{
									}
								}
								else if (strOption == "PythonPort")
								{
									try
									{
										_pythonPort = Convert.ToInt32(xmlReader.Value);
									}
									catch { }
								}
                                else if (strOption == "AnacondaPath")
                                {
                                    _strAnacondaPath = xmlReader.Value;
                                }
                                else if (strOption == "PythonEnv")
                                {
                                    _strPythonEnv = xmlReader.Value;
                                }
                                else if (strOption == "PythonVersion")
                                {
                                    _nPythonVersion = Convert.ToInt32(xmlReader.Value);
                                }

								bResult = true;
							}

							if (!String.IsNullOrEmpty(xmlReader.Value))
								xmlReader.Read();
						}
					}

					xmlReader.Close();
				}
				else
					MessageBox.Show("Cannot read configuration. Functionality is disabled.", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			catch
			{
			}

			if (_selectCategoryFont == null)
				_selectCategoryFont = new Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);

			if (_pythonPort <= 0)
				_pythonPort = 8888;

			return bResult;
		}

		public bool SaveConfig()
		{
			bool bResult = false;

			try
			{
				XmlWriterSettings xmlSettings = new XmlWriterSettings();
				xmlSettings.Encoding = Encoding.UTF8;
				xmlSettings.Indent = true;
				xmlSettings.IndentChars = "  ";
				xmlSettings.NewLineChars = Environment.NewLine;

				string strConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
																				  .Location), _strConfigFileName);

				XmlWriter xmlWriter = XmlWriter.Create(strConfigPath, xmlSettings);
				if (xmlWriter != null)
				{
					xmlWriter.WriteStartDocument();
					xmlWriter.WriteStartElement("Configuration");

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "AvailableViews");
					xmlWriter.WriteValue(_strAvailableViews);
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "AutoOpenViews");
					xmlWriter.WriteValue(_strAutoOpenViews);
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "HTMLAvailableViews");
					xmlWriter.WriteValue(_strHtmlAvailableViews);
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "HTMLAutoOpenViews");
					xmlWriter.WriteValue(_strHtmlAutoOpenViews);
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "ActiveView");
					xmlWriter.WriteValue(_strActiveView);
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "ShowOldCategoryColumn");
					xmlWriter.WriteValue(_bShowOldCategoryColumn.ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "FontFamily");
					xmlWriter.WriteValue(_strFontFamily);
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "FontSize");
					xmlWriter.WriteValue(_fFontSize.ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "FontStyle");
					xmlWriter.WriteValue(((int)_styleFont).ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "LineSpacing");
					xmlWriter.WriteValue(((int)_nLineSpacing).ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "DocumentsServiceQueryLimit");
					xmlWriter.WriteValue(((int)_nDocumentsServiceQueryLimit).ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "AutoCalc");
					xmlWriter.WriteValue(((int)AutoCalc).ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "AutoSort");
					xmlWriter.WriteValue(_bSortByDocumentScore.ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "HighlightBillingMatches");
					xmlWriter.WriteValue(_bHighlightBillingMatches.ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "OnlyPositiveScores");
					xmlWriter.WriteValue(_bOnlyPositiveScores.ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "DocumentsServiceDebug");
					xmlWriter.WriteValue(_bDocumentsServiceDebug.ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "SelectCategoryFont");
					xmlWriter.WriteValue(FontToString(_selectCategoryFont));
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "PythonPort");
					xmlWriter.WriteValue(_pythonPort.ToString());
					xmlWriter.WriteFullEndElement();

					xmlWriter.WriteStartElement("option");
					xmlWriter.WriteAttributeString("name", "SNOMEDBaseAddress");
					xmlWriter.WriteValue(_strSNOMEDBaseAddress);
					xmlWriter.WriteFullEndElement();

                    xmlWriter.WriteStartElement("option");
                    xmlWriter.WriteAttributeString("name", "AnacondaPath");
                    xmlWriter.WriteValue(_strAnacondaPath);
                    xmlWriter.WriteFullEndElement();

                    xmlWriter.WriteStartElement("option");
                    xmlWriter.WriteAttributeString("name", "PythonEnv");
                    xmlWriter.WriteValue(_strPythonEnv);
                    xmlWriter.WriteFullEndElement();

                    xmlWriter.WriteStartElement("option");
                    xmlWriter.WriteAttributeString("name", "PythonVersion");
                    xmlWriter.WriteValue(_nPythonVersion.ToString());
                    xmlWriter.WriteFullEndElement();

                    xmlWriter.WriteFullEndElement();
					xmlWriter.WriteEndDocument();

					xmlWriter.Flush();
					xmlWriter.Close();

					bResult = true;
				}
			}
			catch
			{
			}

			return bResult;
		}

		protected bool IsDatabaseSupported(ViewType viewtype, bool bNewVersion, bool bDocumentsLoaded, bool bRegExpsLoaded)
		{
			if (viewtype != null)
			{
				if (viewtype.NewVersionOnly && !bNewVersion)
					return false;

				if (viewtype.DocumentsRequired && !bDocumentsLoaded)
					return false;

				if (viewtype.RegExpsRequired && !bRegExpsLoaded)
					return false;
			}

			return true;
		}

		protected void UpdateViewsListUI(bool bNewVersion, bool bDocumentsLoaded, bool bRegExpsLoaded)
		{
			try
			{
				var orbmenuitem = GetViewsListOrb();

				var combobox = GetTypesComboBox();
				if (orbmenuitem != null && combobox != null)
				{
					combobox.DropDownItems.Clear();

					ViewType viewtypeSelected = null;

                    orbmenuitem.DropDownItems.Clear();


                    foreach (ViewType viewtype in _listViewTypes)
					{
						if (viewtype.HtmlViewInfo != null && viewtype.HtmlViewInfo.Type != HtmlViewType.Default)
							continue;

						if (IsDatabaseSupported(viewtype, bNewVersion, bDocumentsLoaded, bRegExpsLoaded))
						{
							if (viewtypeSelected == null)
								viewtypeSelected = viewtype;

							var button = new RibbonButton(viewtype.Name);
							button.Click += new EventHandler(OnComboBoxButtonClick);

							combobox.DropDownItems.Add(button);

							var orbitem = new RibbonOrbRecentItem(viewtype.Name);
							orbitem.Click += new EventHandler(OnOrbItemClick);

							orbmenuitem.DropDownItems.Add(orbitem);
						}
					}

					if (viewtypeSelected != null)
						combobox.TextBoxText = viewtypeSelected.Name;

					if (combobox.DropDownItems.Count > 0)
						combobox.Enabled = true;
					else
						combobox.Enabled = false;
				}
			}
			catch
			{
			}
		}

		protected void SetDoubleBufferedGrids(Control ctrlParent)
		{
			foreach (Control ctrl in ctrlParent.Controls)
			{
				SetDoubleBufferedGrids(ctrl);

				if (ctrl is DataGridView)
				{
					DataGridView grid = (DataGridView)ctrl;

					Type type = grid.GetType();
					PropertyInfo pi = type.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
					pi.SetValue(grid, true, null);

					grid.EnableHeadersVisualStyles = false;
					grid.RowHeadersDefaultCellStyle.BackColor = MainForm.ColorBackground;
					grid.ColumnHeadersDefaultCellStyle.BackColor = MainForm.ColorBackground;
				}
			}
		}

		protected RibbonComboBox GetTypesComboBox()
		{
			RibbonComboBox combobox = null;

			try
			{
				combobox = (RibbonComboBox)_ribbon.Tabs[0].Panels[4].Items[0];
			}
			catch
			{
			}

			return combobox;
		}

		protected RibbonOrbMenuItem GetViewsListOrb()
		{
			RibbonOrbMenuItem menuitem = null;

			try
			{
				menuitem = (RibbonOrbMenuItem) _ribbon.OrbDropDown.MenuItems[0];
			}
			catch
			{
			}

			return menuitem;
		}

		protected List<HtmlViewInfo> GetHtmlViewInfoList()
		{
			var list = new List<HtmlViewInfo>();

			try
			{
				var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
														 .Location);
				path = Path.Combine(path, "Views");
				if (Directory.Exists(path))
				{
					var di = new DirectoryInfo(path);
					foreach (var dir in di.GetDirectories())
					{
						try
						{
							var indexJsonPath = Path.Combine(dir.FullName, "index.json");
							if (File.Exists(indexJsonPath))
							{
								var viewInfo = DeserializeHtmlViewInfo(dir.FullName, indexJsonPath);
								if (viewInfo.Type == HtmlViewType.Default)
								{
									var indexHtmlPath = Path.Combine(dir.FullName, "index.html");
									if (!File.Exists(indexHtmlPath))
										indexHtmlPath = Path.Combine(dir.FullName, "index.htm");

									if (File.Exists(indexHtmlPath))
										viewInfo.IndexHtmlPath = indexHtmlPath;
								}

								list.Add(viewInfo);
							}
						}
						catch (Exception ex)
						{
							MainForm.ShowExceptionMessage(ex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return list;
		}

		protected HtmlViewInfo GetHtmlViewInfo(string name)
		{
			try
			{
				var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
														 .Location);
				path = Path.Combine(path, @"Views\" + name);
				if (Directory.Exists(path))
				{
					var indexJsonPath = Path.Combine(path, "index.json");
					if (File.Exists(indexJsonPath))
					{
						var viewInfo = DeserializeHtmlViewInfo(path, indexJsonPath);

						if (viewInfo.Type == HtmlViewType.Default)
						{
							var indexHtmlPath = Path.Combine(path, "index.html");
							if (!File.Exists(indexHtmlPath))
								indexHtmlPath = Path.Combine(path, "index.htm");

							if (File.Exists(indexHtmlPath))
								viewInfo.IndexHtmlPath = indexHtmlPath;
						}

						return viewInfo;
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return null;
		}

		protected HtmlViewInfo DeserializeHtmlViewInfo(string viewFolderPath, string indexJsonPath)
		{
			var json = File.ReadAllText(indexJsonPath);

			var viewInfo = JsonConvert.DeserializeObject<HtmlViewInfo>(json);
			viewInfo.ViewFolderPath = viewFolderPath;
			viewInfo.IndexJsonPath = indexJsonPath;

			return viewInfo;
		}

		public void CreateSampleHtmlView()
		{
			try
			{
				var info = new HtmlViewInfo
				{
					Name = "Sample HTML View",
					ButtonGroups = new List<HtmlRibbonButtonGroup>
											  {
												  new HtmlRibbonButtonGroup
												  {
													  Title = "Sample Buttons",
													  Buttons = new List<HtmlRIbbonButton>
																{
																	new HtmlRIbbonButton
																	{
																		Text = "Button 1",
																		Icon = "Button 1.png",
																		TooltipText = "Button 1 Tooltip"
																	},
																	new HtmlRIbbonButton
																	{
																		Text = "Button 2",
																		Icon = "Button 2.png",
																		TooltipText = "Button 2 Tooltip"
																	}
																}
												  }
											  }
				};

				var json = JsonConvert.SerializeObject(info);
			}
			catch (Exception ex)
			{
				ex.ToString();
			}
		}

		protected string FontToString(Font font)
		{
			return String.Format("{0};{1};{2}", font.FontFamily.Name, font.Size, (int)font.Style);
		}

		protected Font FontFromString(string value)
		{
			if (String.IsNullOrEmpty(value))
				return null;

			var values = value.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			if (values.Length != 3)
				return null;

			return new Font(values[0], Convert.ToSingle(values[1]), (FontStyle)Convert.ToInt32(values[2]));
		}

		#endregion

		#region Implementation: layout

		#region General

		protected XElement LoadLayoutRoot()
		{
			XDocument xDoc = null;

			string strLayoutFilePath = GetLayoutFilePath();
			if (File.Exists(strLayoutFilePath))
			{
				try
				{
					xDoc = XDocument.Load(strLayoutFilePath);
				}
				catch
				{
				}
			}

			if (xDoc == null)
				xDoc = new XDocument();

			XElement xRoot = null;

			if (xDoc != null)
				xRoot = xDoc.Element("Layouts");

			if (xRoot == null)
			{
				xRoot = new XElement("Layouts");
				xDoc.Add(xRoot);
			}

			return xRoot;
		}

		protected string GetViewLayoutName(View view)
		{
			string strLayoutName = String.Empty;

			XElement xRoot = LoadLayoutRoot();
			var varView = xRoot.Elements()
							   .Where(x => x.Name == "View" && x.Attribute("Name")
																.Value == view.ViewType.Name);
			if (varView.Count() > 0)
			{
				XElement xView = varView.First();

				XAttribute xAttribute = xView.Attribute("Layout");
				if (xAttribute != null)
					strLayoutName = xAttribute.Value;
			}

			return strLayoutName;
		}

		protected string GetLayoutFilePath()
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
															  .Location), _strLayoutFileName);
		}

		protected double ConvertToDouble(string strValue)
		{
			double dResult = 0;

			if (!String.IsNullOrEmpty(strValue))
			{
				strValue = strValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
				strValue = strValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

				dResult = Convert.ToDouble(strValue);
			}

			return dResult;
		}

		#endregion

		#region Save

		public void SaveLayout(View view)
		{
			if (!String.IsNullOrEmpty(view.LayoutName))
			{
				XElement xRoot = LoadLayoutRoot();

				//////////////////////////////////////////////////////////////////////////

				xRoot.SetAttributeValue("Width", _form.Width);
				xRoot.SetAttributeValue("Height", _form.Height);
				xRoot.SetAttributeValue("WindowState", (int)_form.WindowState);

				//////////////////////////////////////////////////////////////////////////

				XElement xView;

				var varView = xRoot.Elements()
								   .Where(x => x.Name == "View" && x.Attribute("Name")
																	.Value == view.ViewType.Name)
								   .ToList();
				if (!varView.Any())
				{
					xView = new XElement("View");
					xView.SetAttributeValue("Name", view.ViewType.Name);
					xRoot.Add(xView);
				}
				else
					xView = varView.First();

				xView.SetAttributeValue("Layout", view.LayoutName);

				//////////////////////////////////////////////////////////////////////////

				XElement xLayout;

				var varLayout = xView.Elements()
									 .Where(x => x.Name == "Layout" && x.Attribute("Name")
																		.Value == view.LayoutName)
									 .ToList();
				if (!varLayout.Any())
				{
					xLayout = new XElement("Layout");
					xLayout.SetAttributeValue("Name", view.LayoutName);
					xView.Add(xLayout);
				}
				else
				{
					xLayout = varLayout.First();
					xLayout.RemoveNodes();
				}

				//////////////////////////////////////////////////////////////////////////

				RecurseSaveLayout(view, xLayout);

				var xCustomLayout = view.SaveCustomLayout();
				if (xCustomLayout != null)
				{
					var xCustomLayoutRoot = new XElement("CustomLayout");
					xCustomLayoutRoot.Add(xCustomLayout);

					xLayout.Add(xCustomLayoutRoot);
				}

				//////////////////////////////////////////////////////////////////////////

				File.WriteAllText(GetLayoutFilePath(), xRoot.Document.ToString());
			}
		}

		protected void RecurseSaveLayout(Control ctrl, XElement xParent)
		{
			XElement xElement = SaveControlLayout(ctrl, xParent);

			foreach (Control ctrlChild in ctrl.Controls)
			{
				RecurseSaveLayout(ctrlChild, xElement);
			}
		}

		protected XElement SaveControlLayout(Control ctrl, XElement xParent)
		{
			string strName = String.IsNullOrEmpty(ctrl.Name)
				? ctrl.GetType()
					  .Name
				: ctrl.Name;

			XElement xElement = new XElement("Control");
			xElement.SetAttributeValue("Name", strName);
			xElement.SetAttributeValue("Width", GetChildPercentageFromDimension(ctrl.Parent.DisplayRectangle.Width, ctrl.Width)
				.ToString("0.00"));
			xElement.SetAttributeValue("Height", GetChildPercentageFromDimension(ctrl.Parent.DisplayRectangle.Height, ctrl.Height)
				.ToString("0.00"));

			xParent.Add(xElement);

			if (ctrl is SplitContainer)
			{
				SplitContainer container = (SplitContainer)ctrl;

				int nParentDimension = container.Orientation != Orientation.Horizontal ? container.Width : container.Height;

				xElement.SetAttributeValue("SplitterDistance", GetChildPercentageFromDimension(nParentDimension, container.SplitterDistance)
					.ToString("0.00"));
				xElement.SetAttributeValue("Panel1Collapsed", container.Panel1Collapsed);
				xElement.SetAttributeValue("Panel2Collapsed", container.Panel2Collapsed);
			}
			else if (ctrl is DataGridView)
			{
				DataGridView grid = (DataGridView)ctrl;
				foreach (DataGridViewColumn col in grid.Columns)
				{
					SaveColumnLayout(col, xElement);
				}
			}

			return xElement;
		}

		protected void SaveColumnLayout(DataGridViewColumn col, XElement xParent)
		{
			XElement xElement = new XElement("Column");
			xElement.SetAttributeValue("Name", col.Name);
			xElement.SetAttributeValue("Width", GetChildPercentageFromDimension(col.DataGridView.Width, col.Width).ToString("0.00"));
			xElement.SetAttributeValue("Mode", (int)col.AutoSizeMode);

			xParent.Add(xElement);
		}

		public static double GetChildPercentageFromDimension(int nParentValue, int nValue)
		{
			double dResult = (nParentValue != 0) ? ((double)nValue / (double)nParentValue) * (double)100 : 0;

			return dResult;
		}

		#endregion

		#region Load

		public void LoadFormState()
		{
			XElement xRoot = LoadLayoutRoot();

			//////////////////////////////////////////////////////////////////////////

			bool bCenterScreen = false;

			XAttribute xAttribute = xRoot.Attribute("WindowState");
			if (xAttribute != null)
			{
				try
				{
					FormWindowState state = (FormWindowState)Convert.ToInt32(xAttribute.Value);
					_form.WindowState = state;

					if (state == FormWindowState.Normal)
						bCenterScreen = true;
				}
				catch
				{
				}
			}
			else
				bCenterScreen = true;

			AssignIntValue(xRoot, "Width", x => _form.Width = x);
			AssignIntValue(xRoot, "Height", x => _form.Height = x);

			//////////////////////////////////////////////////////////////////////////

			if (bCenterScreen)
			{
				Screen screen = Screen.FromControl(_form);

				_form.Location = new Point((screen.WorkingArea.Width - _form.Width) / 2, (screen.WorkingArea.Height - _form.Height) / 2);
			}
		}

		public void LoadLayout(View view)
		{
			if (!String.IsNullOrEmpty(view.LayoutName))
			{
				XElement xRoot = LoadLayoutRoot();

				//////////////////////////////////////////////////////////////////////////

				XAttribute xAttribute = xRoot.Attribute("WindowState");
				if (xAttribute != null)
				{
					try
					{
						FormWindowState state = (FormWindowState)Convert.ToInt32(xAttribute.Value);
						_form.WindowState = state;
					}
					catch
					{
					}
				}

				//////////////////////////////////////////////////////////////////////////

				var varView = xRoot.Elements()
								   .Where(x => x.Name == "View" && x.Attribute("Name")
																	.Value == view.ViewType.Name)
								   .ToList();
				if (varView.Any())
				{
					XElement xView = varView.First();

					var varLayout = varView.Elements()
										   .Where(x => x.Name == "Layout" && x.Attribute("Name")
																			  .Value == view.LayoutName)
										   .ToList();
					if (varLayout.Any())
					{
						XElement xLayout = varLayout.First();

						RecurseLoadLayout(view, xLayout);

						var xCustomLayout = xLayout.Elements()
												   .FirstOrDefault(x => x.Name == "CustomLayout");
						if (xCustomLayout != null)
						{
							view.LoadCustomLayout(xCustomLayout.Elements()
								.ToList());
						}
						else
							view.ResetView();
					}
					else
						view.ResetView();
				}
				else
					view.ResetView();
			}
		}

		protected void RecurseLoadLayout(Control ctrl, XElement xParent)
		{
			XElement xElement = LoadControlLayout(ctrl, xParent);
			if (xElement != null)
			{
				foreach (Control ctrlChild in ctrl.Controls)
				{
					RecurseLoadLayout(ctrlChild, xElement);
				}

				xElement.Remove();
			}
		}

		protected XElement LoadControlLayout(Control ctrl, XElement xParent)
		{
			var varElement = xParent.Elements()
									.Where(x => x.Name == "Control" && x.Attribute("Name")
																		.Value == ctrl.Name);
			if (varElement.Count() == 0)
			{
				varElement = xParent.Elements()
									.Where(x => x.Name == "Control" && x.Attribute("Name")
																		.Value == ctrl.GetType()
																					  .Name);
			}

			if (varElement.Count() > 0)
			{
				XElement xElement = varElement.First();

				AssignDoubleValue(xElement, "Width", x => GetChildDimensionFromPercentage(ctrl.Parent.DisplayRectangle.Width, x));
				AssignDoubleValue(xElement, "Height", x => GetChildDimensionFromPercentage(ctrl.Parent.DisplayRectangle.Height, x));

				if (ctrl is SplitContainer)
				{
					SplitContainer container = (SplitContainer)ctrl;

					double dSplitterDistance;
					XAttribute xAttribute = xElement.Attribute("SplitterDistance");
					if (xAttribute != null)
						dSplitterDistance = ConvertToDouble(xAttribute.Value);
					else
						dSplitterDistance = 50;

					var nParentDimension = container.Orientation != Orientation.Horizontal ? container.Width : container.Height;
					var splitterDistance = GetChildDimensionFromPercentage(nParentDimension, dSplitterDistance);

					if (splitterDistance < container.Panel1MinSize || splitterDistance > nParentDimension - container.Panel2MinSize)
						splitterDistance = container.Panel1MinSize;

					try
					{
						container.SplitterDistance = splitterDistance;
					}
					catch
					{
						container.SplitterDistance = container.Panel1MinSize;
					}

					AssignBoolValue(xElement, "Panel1Collapsed", x => container.Panel1Collapsed = x);
					AssignBoolValue(xElement, "Panel2Collapsed", x => container.Panel2Collapsed = x);
				}
				else if (ctrl is DataGridView)
				{
					DataGridView grid = (DataGridView)ctrl;
					if (grid.Name != "documentsDataGridView")
					{
						foreach (DataGridViewColumn col in grid.Columns)
						{
							LoadColumnLayout(col, xElement);
						}
					}
				}

				return xElement;
			}

			return null;
		}

		protected void LoadColumnLayout(DataGridViewColumn col, XElement xParent)
		{
			var varColumn = xParent.Elements()
								   .Where(x => x.Name == "Column" && x.Attribute("Name")
																	  .Value == col.Name);
			if (varColumn.Count() > 0)
			{
				XElement xColumn = varColumn.First();

				AssignDoubleValue(xColumn, "Width", x => col.Width = GetChildDimensionFromPercentage(col.DataGridView.DisplayRectangle.Width, x));

				XAttribute xAttribute = xColumn.Attribute("Mode");
				if (xAttribute != null)
				{
					col.AutoSizeMode = (DataGridViewAutoSizeColumnMode)Convert.ToInt32(xAttribute.Value);
				}
			}
		}

		protected void AssignDoubleValue(XElement xElement, string strAttribute, Action<double> predicate)
		{
			XAttribute xAttribute = xElement.Attribute(strAttribute);
			if (xAttribute != null)
			{
				double dValue = ConvertToDouble(xAttribute.Value);
				predicate(dValue);
			}
		}

		protected void AssignIntValue(XElement xElement, string strAttribute, Action<int> predicate)
		{
			XAttribute xAttribute = xElement.Attribute(strAttribute);
			if (xAttribute != null)
			{
				int nValue = Convert.ToInt32(xAttribute.Value);
				predicate(nValue);
			}
		}

		protected void AssignBoolValue(XElement xElement, string strAttribute, Action<bool> predicate)
		{
			XAttribute xAttribute = xElement.Attribute(strAttribute);
			if (xAttribute != null)
			{
				bool bValue = Convert.ToBoolean(xAttribute.Value);
				predicate(bValue);
			}
		}

		public static int GetChildDimensionFromPercentage(int nParentValue, double dValue)
		{
			int nResult = (int)(((double)nParentValue / (double)100) * (double)dValue);

			return nResult;
		}

		#endregion

		#endregion

		#region Implementation: hotkeys

		public void ProcessHotkey(KeyEventArgs keyEventArgs)
		{
			if (!_listViews.Any())
				return;

			///////////////////////////////////////////////////////////////////////////////

			var hotkey = _hotkeys.FirstOrDefault(x => IsMatchingHotkey(x, keyEventArgs));
			if (hotkey != null)
			{
				var activeView = GetActiveView();
				if (activeView != null)
					keyEventArgs.Handled = activeView.OnHotkey(hotkey.Code);
			}
		}

		public HotkeyInfo GetHotkeyByCode(string code)
		{
			return _hotkeys.FirstOrDefault(x => x.Code == code);
		}

		public void SetHotkeyByCode(string code, HotkeyInfo newHotkey)
		{
			var oldHotkey = GetHotkeyByCode(code);
			if (oldHotkey != null)
			{
				oldHotkey.Hotkey = newHotkey.Hotkey;
				oldHotkey.Modifiers = newHotkey.Modifiers;
			}
			else
			{
				newHotkey.Code = code;
				_hotkeys.Add(newHotkey);
			}
		}

		public void SaveHotkeys()
		{
			try
			{
				using (var file = File.Create(GetHotkeysFilePath()))
				{
					var writer = new XmlTextWriter(file, Encoding.UTF8);

					var serializer = new XmlSerializer(_hotkeys.GetType());
					serializer.Serialize(writer, _hotkeys);

					file.Flush();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void LoadHotkeys()
		{
			try
			{
				var filePath = GetHotkeysFilePath();
				if (File.Exists(filePath))
				{
					using (var file = File.OpenRead(filePath))
					{
						var serializer = new XmlSerializer(_hotkeys.GetType());
						_hotkeys = (List<HotkeyInfo>)serializer.Deserialize(file);
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			///////////////////////////////////////////////////////////////////////////////

			if (GetHotkeyByCode(PaneDocuments.HotkeyCode_SelectCategory) == null)
				SetHotkeyByCode(PaneDocuments.HotkeyCode_SelectCategory, new HotkeyInfo { Hotkey = Keys.Return, Modifiers = Keys.Control });

			///////////////////////////////////////////////////////////////////////////////

			if (GetHotkeyByCode(PaneDocuments.HotkeyCode_EditValue) == null)
				SetHotkeyByCode(PaneDocuments.HotkeyCode_EditValue, new HotkeyInfo { Hotkey = Keys.Space, Modifiers = Keys.Control });
		}

		protected string GetHotkeysFilePath()
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
															  .Location), _strHotkeysFileName);
		}

		protected bool IsMatchingHotkey(HotkeyInfo hotkey, KeyEventArgs e)
		{
			var isControlPressNeeded = (hotkey.Modifiers & Keys.Control) == Keys.Control;
			var isAltPressNeeded = (hotkey.Modifiers & Keys.Alt) == Keys.Alt;
			var isShiftPressNeeded = (hotkey.Modifiers & Keys.Shift) == Keys.Shift;

			///////////////////////////////////////////////////////////////////////////////

			if (isControlPressNeeded != e.Control)
				return false;

			if (isAltPressNeeded != e.Alt)
				return false;

			if (isShiftPressNeeded != e.Shift)
				return false;

			///////////////////////////////////////////////////////////////////////////////

			if (e.KeyCode != hotkey.Hotkey)
				return false;

			///////////////////////////////////////////////////////////////////////////////

			return true;
		}

		#endregion
	}

	#region Helper types

	public class ViewType
	{
		#region Fields

		protected string _strName;
		protected Type _type;
		protected bool _bNewDatabaseOnly;

		protected bool _bDocumentsRequired;
		protected bool _bRegExpsRequired;
		protected bool _allowMultipleViews;
		protected HtmlViewInfo _htmlViewInfo;

		#endregion

		#region Properties

		public string Name
		{
			get { return _strName; }
		}

		public Type Type
		{
			get { return _type; }
		}

		public bool NewVersionOnly
		{
			get { return _bNewDatabaseOnly; }
		}

		public bool DocumentsRequired
		{
			get { return _bDocumentsRequired; }
		}

		public bool RegExpsRequired
		{
			get { return _bRegExpsRequired; }
		}

		public HtmlViewInfo HtmlViewInfo
		{
			get { return _htmlViewInfo; }
		}

		public bool AllowMultipleViews
		{
			get { return _allowMultipleViews; }
		}

		#endregion

		#region Ctors

		public ViewType(string strName, Type type, bool bNewDatabaseOnly, bool bDocumentsRequired, bool bRegExpsRequired, bool allowMultipleViews, HtmlViewInfo htmlViewInfo)
		{
			_type = type;
			_strName = strName;
			_bNewDatabaseOnly = bNewDatabaseOnly;
			_bDocumentsRequired = bDocumentsRequired;
			_bRegExpsRequired = bRegExpsRequired;
			_allowMultipleViews = allowMultipleViews;

			_htmlViewInfo = htmlViewInfo;
		}

		#endregion
	}

	public class HtmlViewInfo
	{
		#region Fields

		public string Name { get; set; }
		public HtmlViewType Type { get; set; }
		public List<HtmlRibbonButtonGroup> ButtonGroups { get; set; }

		[JsonIgnore]
		public View View { get; set; }

		[JsonIgnore]
		public string ViewFolderPath { get; set; }

		[JsonIgnore]
		public string IndexHtmlPath { get; set; }

		[JsonIgnore]
		public string IndexJsonPath { get; set; }

		#endregion

		#region Properties

		[JsonIgnore]
		public bool IsValid
		{
			get { return Directory.Exists(this.ViewFolderPath) && File.Exists(this.IndexJsonPath) && (this.Type == HtmlViewType.Python || File.Exists(this.IndexHtmlPath)); }
		}

		#endregion
	}

	public enum HtmlViewType
	{
		#region Constants

		Default,
		Python

		#endregion
	}

	public class HtmlRibbonButtonGroup
	{
		#region Fields

		public string Title { get; set; }
		public List<HtmlRIbbonButton> Buttons { get; set; }

		#endregion
	}

	public class HtmlRIbbonButton
	{
		#region Fields

		public string Text { get; set; }
		public string Icon { get; set; }
		public string TooltipText { get; set; }

		#endregion
	}

	public class HotkeyInfo
	{
		#region Fields

		public string Code { get; set; }
		public Keys Hotkey { get; set; }
		public Keys Modifiers { get; set; }

		#endregion
	}

	public class NavigationContext
	{
		#region Fields

		protected long _navigationContextID;
		protected string _sortColumn;
		protected string _sortOrder;
		protected string _filter;

		#endregion

		#region Ctors

		public long GetNavigationContextID(string sortColumn, string sortOrder, string filter)
		{
			if (_navigationContextID < 0 || sortColumn != _sortColumn || sortOrder != _sortOrder || filter != _filter)
			{
				_sortColumn = sortColumn;
				_sortOrder = sortOrder;
				_filter = filter;

				_navigationContextID = DateTime.Now.Ticks;
			}

			return _navigationContextID;
		}

		#endregion
	}

	public class SplitterInfo
	{
		#region Fields

		public double HorizontalPercentage { get; set; }
		public double VerticalPercentage { get; set; }

		#endregion

		#region Ctors

		public SplitterInfo()
		{
			this.HorizontalPercentage = 50;
			this.VerticalPercentage = 50;
		}

		#endregion
	}

	#endregion
}