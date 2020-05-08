using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class PaneBrowser : Pane
	{
        #region Data members

        private string _columnName;
        private ChromiumWebBrowser _browser;

        #endregion

        #region delegate
        public event EventHandler SortTabs;
        public event EventHandler AddBrowserTab;
        public event EventHandler AddViewTab;
        #endregion

        #region Ctors

        public PaneBrowser(string columnName = "")
		{
			InitializeComponent();

            toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

            _columnName = columnName;
		}

		#endregion

		#region Events
        
        private void OnCurrentDocumentChanged(object sender, EventArgs e)
        {
            if (_ownerView.Visible)
                LoadPage();          
        }

        private void OnRegExpChanged(object sender, EventArgs e)
        {
            var rowView = _views.MainForm.sourceRegExp.Current as DataRowView;            

            if (rowView != null)
                this.txtboxSearch.Text = (string)rowView["RegExp"];
        }

        private void OnColRegExpChanged(object sender, EventArgs e)
        {
            var rowView = _views.MainForm.sourceColRegExp.Current as DataRowView;

            if (rowView != null)
                this.txtboxSearch.Text = (string)rowView["RegExp"];
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {            
            Find(false);          
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Find(true);
        }

        #endregion

        #region Overrides

        public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
            base.InitPane(views, ownerView, panel, tab);

            _views.MainForm.sourceDocuments.CurrentChanged += OnCurrentDocumentChanged;

            _views.MainForm.sourceRegExp.CurrentItemChanged += OnRegExpChanged;
            _views.MainForm.sourceColRegExp.CurrentItemChanged += OnColRegExpChanged;

            _browser = new ChromiumWebBrowser();
            _browser.Dock = DockStyle.Fill;

            this.htmlPanel.Controls.Add(_browser);              
		}

        public override void UpdatePane()
        {
            base.UpdatePane();

            LoadPage();
        }

        public override void DestroyPane()
        {
            _views.MainForm.sourceDocuments.CurrentChanged -= OnCurrentDocumentChanged;

            _views.MainForm.sourceRegExp.CurrentItemChanged -= OnRegExpChanged;
            _views.MainForm.sourceColRegExp.CurrentItemChanged -= OnColRegExpChanged;

            base.DestroyPane();            
        }
        #endregion

        #region Implementation

        public void LoadPage()
        {
            var documentUrl = String.Empty;

            
            DataRowView rowView = (DataRowView)_views.MainForm.sourceDocuments.Current;

            if (rowView != null && rowView.Row != null)
            {
                try
                {
                    documentUrl = (string)rowView.Row[_columnName];
                } catch (Exception e)
                {
                    documentUrl = "";
                }                
                _browser.Load(documentUrl);
            }            
        }
        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.SortTabs != null)
            {
                this.SortTabs(this, EventArgs.Empty);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.AddBrowserTab != null)
            {
                this.AddBrowserTab(this, EventArgs.Empty);
            } 
        }

        private void Find(bool next)
        {
            if (!string.IsNullOrEmpty(this.txtboxSearch.Text))
            {
                _browser.Find(0, this.txtboxSearch.Text, next, false, false);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (this.AddViewTab != null)
            {
                this.AddViewTab(this, EventArgs.Empty);
            }
        }
    }
}
