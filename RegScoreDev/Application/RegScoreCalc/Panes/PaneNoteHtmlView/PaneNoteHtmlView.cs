using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

using System.Text;

using Excel = Microsoft.Office.Interop.Excel;

using FastColoredTextBoxNS;

using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;

using RegScoreCalc.Code;
using RegScoreCalc.Forms;
using CefSharp.WinForms;
using System.Reflection;
using CefSharp;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Linq;
using DocumentsServiceInterfaceLib;
using System.Threading.Tasks;

namespace RegScoreCalc
{
	public partial class PaneNoteHtmlView : Pane
	{
		#region Delegates

        public event EventHandler SortTabs;
        public event EventHandler AddBrowserTab;
        public event EventHandler AddViewTab;
        #endregion

        #region Fields

        private ChromiumWebBrowser _browser;

        protected DocumentViewType _viewType;
		protected bool _preventUpdate;

        protected string _strNoteColumnName;

        protected string _noteText;

        protected PaneColumnNotes _paneColumnNotes;
        protected PaneNotes _paneNotes;

        #endregion

        #region Properties

        public bool ShowToolbar
		{
			get { return toolStripTop.Visible; }
			set { toolStripTop.Visible = value; }
		}

        public int SelectedColumnID
        {
            get { return _paneColumnNotes.SelectedColumnID; }
            set
            {
                if (_paneColumnNotes.SelectedColumnID != value)
                {
                    _paneColumnNotes.SelectedColumnID = value;                   
                }
            }
        }

        public string NoteColumnName
        {
            get { return _strNoteColumnName; }
            set { _strNoteColumnName = value; }
        }

        #endregion

        #region Ctors

        public PaneNoteHtmlView(string noteColumnName = "NOTE_TEXT", DocumentViewType viewType = DocumentViewType.Html_ListView)
		{

            NoteColumnName = noteColumnName;
            _viewType = viewType;


			InitializeComponent();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };			
		
		}

        #endregion

        #region Events

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.AddBrowserTab != null)
            {
                this.AddBrowserTab(this, EventArgs.Empty);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.SortTabs != null)
            {
                this.SortTabs(this, EventArgs.Empty);
            }
        }

        private void btnAddViewTab_Click(object sender, EventArgs e)
        {
            if (this.AddViewTab != null)
            {
                this.AddViewTab(this, EventArgs.Empty);
            }
        }

        private void toolStripButtonAddToExceptions_Click(object sender, EventArgs e)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnCurrentDocumentChanged(object sender, EventArgs e)
		{
			if (_ownerView.Visible)
				UpdateText();
		}

        #endregion

		#region Operations


		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			_views.MainForm.sourceDocuments.CurrentChanged += OnCurrentDocumentChanged;

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                         .Location);
            path = Path.Combine(path, "HtmlViewer");
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            if (_viewType == DocumentViewType.Html_ListView)
            {

                _browser = new ChromiumWebBrowser(Path.Combine(path, "listview.html"));
                                
                _browser.RegisterAsyncJsObject("boundAsync", new AsyncBoundListObject(this));
            }
            else if (_viewType == DocumentViewType.Html_CalenderView)
            {
                _browser = new ChromiumWebBrowser(Path.Combine(path, "calenderview.html"));

                _browser.RegisterAsyncJsObject("boundAsync", new AsyncBoundCalenderObject(this));
            } else if (_viewType == DocumentViewType.Html_MedaCyEntityView)
            {
                _browser = new ChromiumWebBrowser(Path.Combine(path, "entities.html"));
                _browser.RegisterAsyncJsObject("boundAsync", new AsyncBoundEntitiesObject(this));
            }
            else if (_viewType == DocumentViewType.Html_SpaCyEntityView)
            {
                _browser = new ChromiumWebBrowser(Path.Combine(path, "spacy_entities.html"));
                _browser.RegisterAsyncJsObject("boundAsync", new AsyncBoundSpacyEntitiesObject(this));
            }

            _browser.Dock = DockStyle.Fill;

            {
                if (ownerView.Name == "Columns RegEx")
                {
                    _paneColumnNotes = new PaneColumnNotes(NoteColumnName);

                    _paneColumnNotes.InitPane(views, ownerView, panel, tab);
                    _paneColumnNotes._eventDataModified += new EventHandler(OnDataModified);

                    _paneColumnNotes.ShowToolbar = false;

                    _paneColumnNotes.Dock = DockStyle.Fill;
                    _paneColumnNotes.ShowPane();

                    this.paneSplitter.Panel1.Controls.Add(_paneColumnNotes);

                } else if (ownerView.Name == "Default" || ownerView.Name == "ReviewML")
                {
                    _paneNotes = new PaneNotes(NoteColumnName);


                    _paneNotes.InitPane(views, ownerView, panel, tab);
                    _paneNotes._eventDataModified += new EventHandler(OnDataModified);

                    _paneNotes.ShowToolbar = false;

                    _paneNotes.Dock = DockStyle.Fill;
                    _paneNotes.ShowPane();

                    this.paneSplitter.Panel1.Controls.Add(_paneNotes);

                }

                this.paneSplitter.Panel1Collapsed = true;                
                this.paneSplitter.Panel1.Hide();
            }           

            this.paneSplitter.Panel2.Controls.Add(_browser);            
            
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
            
		}

		public override void UpdatePane()
		{
			UpdateText();
		}

		public override void DestroyPane()
		{
			_views.MainForm.sourceDocuments.CurrentChanged -= OnCurrentDocumentChanged;

            if (_paneColumnNotes != null)
            {
                _paneColumnNotes.DestroyPane();

            }
            if (_paneNotes != null)
            {
                _paneNotes.DestroyPane();
            }

            this.paneSplitter.Panel1Collapsed = true;

			base.DestroyPane();
		}

		#endregion

		#region Implementation

		protected string GetDocumentText(DataRowView rowView, string specificNoteColumnName = null)
		{
			var documentText = String.Empty;
            try
            {            
			    if (rowView != null && rowView.Row != null)
			    {
				    var documentID = (double) rowView.Row["ED_ENC_NUM"];
                    if (specificNoteColumnName == null)
				        documentText = _views.DocumentsService.GetDocumentText(documentID, NoteColumnName);
                    else
                        documentText = _views.DocumentsService.GetDocumentText(documentID, specificNoteColumnName);
                }
            } catch (Exception e)
            {
                MainForm.ShowErrorToolTip(e.Message);
            }

            return documentText;
		}


		public void UpdateText()
		{
            /*
            Task.Run(() =>
            {
            */
                try
                {
                    _preventUpdate = true;

                    _noteText = GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView);
                    if (_browser.IsBrowserInitialized)
                    {
                        _browser.Reload();
                    }

                }
                catch (Exception ex)
                {
                    MainForm.ShowErrorToolTip(ex.Message);
                }
                finally
                {
                    _preventUpdate = false;
                }
            /*
            });			
            */
		}

        public PaneNotes GetDebugPaneNotes()
        {
            return _paneNotes;
        }

        public PaneColumnNotes GetDebugPaneColumnNotes()
        {
            return _paneColumnNotes;
        }
        #endregion


        #region API
        public string[] GetSentences()
        {
            return _noteText.Split('\n').ToArray();
        }

        public List<CalendarEvent> GetCalenderEvents()
        {
            string[] germanMonth = new string[] { "Januar", "Februar", "März", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember" };
            string[] englishMonth = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };


            List<CalendarEvent> retVal = new List<CalendarEvent>();

            string text = _noteText;
            text = text.Replace("\r", "");
             List<string> lst = text.Split(new char[] {','}).ToList();

            var i = 0;
            while (i < lst.Count)
            {                
                CalendarEvent calender = new CalendarEvent();

                var dateString = lst[i];
                
                for (var j = 0; j < 12; j++)
                    dateString = dateString.Replace(germanMonth[j], englishMonth[j]);
                    
                try
                {                    
                    CultureInfo culture = new CultureInfo("en-GB");
                    
                    DateTime dt = Convert.ToDateTime(dateString, culture);
                    
                    calender.start = dt.Year + "-" + (dt.Month < 10 ? "0" + dt.Month : dt.Month.ToString()) + "-" + (dt.Day < 10 ? "0" + dt.Day : dt.Day.ToString());
                    calender.description = lst[i + 1];

                    calender.title = lst[i + 1].Split('\n')[0];
                    retVal.Add(calender);

                } catch (Exception ex)
                {
                    i++;
                    continue;
                }
                i += 2;
            }
            retVal.Sort((x, y) => x.start.CompareTo(y.start));
            return retVal;
        }

        public Tuple<object, string> GetEntities()
        {
            try
            {
                JObject result = JObject.Parse(_noteText);


                string noteTextColumnName = (string)result["Item2"];

                var text = GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView, noteTextColumnName).Replace("\r", "");
              
                return new Tuple<object, string>(result["Item1"], text);
            } catch (Exception e)
            {
                return new Tuple<object, string>(new object(), GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView, "NOTE_TEXT").Replace("\r", ""));
                //return GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView, 0);
            }            
        }
        public string GetMainNoteDocument()
        {
            return GetDocumentText(_views.MainForm.sourceDocuments.Current as DataRowView, "NOTE_TEXT").Replace("\r", "");
        }
        public static string toRgbText(uint rgb)
        {
            return "#" + rgb.ToString("X"); //$NON-NLS-1$ 
        }
        public string GetLabelColors()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            //List<EntityColor> entityColors = new List<EntityColor>();
            
            foreach (DataRow row in _views.MainForm.datasetMain.Entities.Rows)
            {
                if (row["EntityColor"] == DBNull.Value)
                {
                    dict.Add((string)row["Entity"], string.Format("rgba({0}, {1}, {2}, {3}", RegExpBase.DefaultHighlightColor.R, RegExpBase.DefaultHighlightColor.G, RegExpBase.DefaultHighlightColor.B, RegExpBase.DefaultHighlightColor.A));
                } else
                {
                    var color = Color.FromArgb(Int32.Parse(row["EntityColor"].ToString()));
                    dict.Add((string)row["Entity"], string.Format("rgba({0}, {1}, {2}, {3}", color.R, color.G, color.B, color.A));
                }

                
            }
            return JsonConvert.SerializeObject(dict);
        }
        public string GetSpacyEntities()
        {
            if (string.IsNullOrEmpty(_noteText))
                return "[]";
            return _noteText;
        }
        #endregion

        #region Implementation: context menu


        #endregion

        private void btnShowText_Click(object sender, EventArgs e)
        {
            //this.btnShowText.Checked = !this.btnShowText.Checked;
            
            if (this.btnShowText.Checked)
            {
                if (this.paneSplitter.SplitterDistance < 10)
                {
                    this.paneSplitter.SplitterDistance = this.paneSplitter.Height / 2;
                }

                this.paneSplitter.Panel1Collapsed = false;
                this.paneSplitter.Panel1.Show();

            } else
            {
                
                //this.paneSplitter.SplitterDistance = 0;
                this.paneSplitter.Panel1Collapsed = true;
                this.paneSplitter.Panel1.Hide();               
            }
        }

        public void InvokeUpdateTextOnMainThread(string text, bool v)
        {
            if (_paneNotes != null)
            {
                _paneNotes.InvokeUpdateTextOnMainThread(text, v);
            }
            if (_paneColumnNotes != null)
            {
                _paneColumnNotes.InvokeUpdateTextOnMainThread(text, v);
            }

            try
            {
                _preventUpdate = true;

                _noteText = text;
                if (_browser.IsBrowserInitialized)
                {
                    _browser.Reload();
                }

            }
            catch (Exception ex)
            {
                MainForm.ShowErrorToolTip(ex.Message);
            }
            finally
            {
                _preventUpdate = false;
            }
        }
    }
    public class AsyncBoundListObject
    {
        protected PaneNoteHtmlView _htmlViewer;

        public AsyncBoundListObject(PaneNoteHtmlView viewer)
        {
            _htmlViewer = viewer;
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public void Error(string err)
        {
            //throw new Exception("This is an exception coming from C#");
            MessageBox.Show(err, "List View");
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public string GetSentences()
        {   
            return JsonConvert.SerializeObject(_htmlViewer.GetSentences());
        }

    }

    public class AsyncBoundCalenderObject
    {
        protected PaneNoteHtmlView _htmlViewer;

        public AsyncBoundCalenderObject(PaneNoteHtmlView viewer)
        {
            _htmlViewer = viewer;
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public void Error(string err)
        {
            //throw new Exception("This is an exception coming from C#");
            MessageBox.Show(err, "List View");
        }

        
        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public string GetCalenderEvents()
        {               
            return JsonConvert.SerializeObject(_htmlViewer.GetCalenderEvents());
        }
    }

    public class AsyncBoundEntitiesObject
    {
        protected PaneNoteHtmlView _htmlViewer;

        public AsyncBoundEntitiesObject(PaneNoteHtmlView viewer)
        {
            _htmlViewer = viewer;
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public void Error(string err)
        {
            //throw new Exception("This is an exception coming from C#");
            MessageBox.Show(err, "List View");
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public string GetEntities()
        {           
            return JsonConvert.SerializeObject(_htmlViewer.GetEntities());
        }
    }

    public class AsyncBoundSpacyEntitiesObject
    {
        protected PaneNoteHtmlView _htmlViewer;

        public AsyncBoundSpacyEntitiesObject(PaneNoteHtmlView viewer)
        {
            _htmlViewer = viewer;
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public void Error(string err)
        {
            //throw new Exception("This is an exception coming from C#");
            MessageBox.Show(err, "spaCyEntityView");
        }

        //We expect an exception here, so tell VS to ignore
        [DebuggerHidden]
        public string GetSpacyEntities()
        {
            return _htmlViewer.GetSpacyEntities();
        }
        [DebuggerHidden]
        public string GetDocument()
        {
            return _htmlViewer.GetMainNoteDocument();
        }
        [DebuggerHidden]
        public string GetLabelColors()
        {
            return _htmlViewer.GetLabelColors();
        }
    }
    public class CalendarEvent
    {
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public string description { get; set; }
    }
    public class EntityColor
    {
        public string Label { get; set; }
        public int Color { get; set; }
    }
}  

