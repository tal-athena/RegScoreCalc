using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using DocumentsServiceInterfaceLib;

using Helpers;
using Newtonsoft.Json;
using RegExpLib.Core;
using RegExpLib.Model;
using RegExpLib.Processing;

using RegScoreCalc.Code;
using RegScoreCalc.Properties;

namespace RegScoreCalc.Forms
{
	public partial class FormEntityStatistics : Form, IMessageFilter
	{
		#region Delegates

		public event EventHandler Modified;

		#endregion

		#region Fields

		protected BindingSource _source;

		protected RegExpBase _regExp;

		protected string _connectionString;
		protected string text;
		protected int selectionStartPosition;
		protected int selectionLength;

		protected ViewsManager _views;
		protected int _nPrevPosition;
		protected int _nMatchIndex;
		protected PaneNotes _openedNotesPane;

		protected bool _preventUpdate;
		protected bool _isDirty;


        protected string _distanceProcessOutput;

        protected List<EntityStatisticsModel> _entityStaticsData;

		#endregion

		#region Properties

		public bool EnableNavigation
		{
			get { return panelNavButtons.Visible; }
			set { panelNavButtons.Visible = value; }
		}

		#endregion

		#region Ctors

		public FormEntityStatistics(ViewsManager views, BindingSource source)
		{
			Application.AddMessageFilter(this);

			InitializeComponent();

			toolStripTop.Renderer = new CustomToolStripRenderer { RoundedEdges = false };

			_connectionString = views.MainForm.DocumentsDbPath;
			_views = views;			

			_source = source;

            _entityStaticsData = new List<EntityStatisticsModel>();
            gridStatistics.DataSource = new BindingSource(_entityStaticsData, "");

            gridStatistics.AutoGenerateColumns = false;

			///////////////////////////////////////////////////////////////////////////////

			tabControlEditRegEx.ShowIndicators = true;
			
			///////////////////////////////////////////////////////////////////////////////

			

			this.BackColor = MainForm.ColorBackground;
			tabControlEditRegEx.BackColor = MainForm.ColorBackground;
			foreach (TabPage page in tabControlEditRegEx.TabPages)
			{
				page.BackColor = MainForm.ColorBackground;
			}
		}

		#endregion

		#region Events

		private void FormRegularExpressionEditor_Load(object sender, EventArgs e)
		{
			
			///////////////////////////////////////////////////////////////////////////////
            
			_source.CurrentChanged += OnCurrentEntityChanged;
			_source.CurrentItemChanged += OnCurrentItemChanged;

			//UpdateRegExpView();
		}

		
		
		private void OnCurrentItemChanged(object sender, EventArgs e)
		{
			try
			{
				//if (!_preventUpdate)
				//	UpdateRegExpView();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnCurrentEntityChanged(object sender, EventArgs e)
		{
			try
			{
                //gridStatistics.Rows.Clear();
                _entityStaticsData.Clear();
                ((BindingSource)gridStatistics.DataSource).Clear();
				//UpdateRegExpView();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

        #endregion


		#region Events: navigation

		private void btnPrevDocument_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.PrevDocument);
		}

		private void btnNextDocument_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.NextDocument);
		}

		private void btnPrevMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.PrevMatch);
		}

		private void btnNextMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.NextMatch);
		}

		private void btnPrevUniqueMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.PrevUniqueMatch);
		}

		private void btnNextUniqueMatch_Click(object sender, EventArgs e)
		{
			InvokeNavigate(MatchNavigationMode.NextUniqueMatch);
		}

		#endregion

		#region Events: statistics

		private void btnCalculate_Click(object sender, EventArgs e)
		{
			try
			{
				CalculateStatistics();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		
		#endregion

		#region Implementation

		protected void InvokeNavigate(MatchNavigationMode mode)
		{
			try
			{
				if (_isDirty)
				{
					
				}

				var formProgress = new FormGenericProgress("Navigating...", DoNavigate, mode, true)
				                   {
					                   CancellationEnabled = true
				                   };

				formProgress.ShowDialog();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected bool DoNavigate(BackgroundWorker worker, object objArgument)
		{
			try
			{
				var mode = (MatchNavigationMode) objArgument;

				_views.InvokeOnMatchNavigate(mode, _regExp, worker);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			return true;
		}

		protected void CalculateStatistics()
		{
            //SaveRegExpToDatabase();

            ///////////////////////////////////////////////////////////////////////////////

            var entityLabel = (_source.Current as DataRowView).Row["Entity"];

            ///////////////////////////////////////////////////////////////////////////////

            var formProgress = new FormGenericProgress("Calculating statistics...", DoCalculateStatistics, (string)entityLabel , true);
			formProgress.ShowDialog();
            ((BindingSource)gridStatistics.DataSource).ResetBindings(false);
        }

		protected bool DoCalculateStatistics(BackgroundWorker worker, object objArgument)
		{
			try
			{
                worker.ReportProgress(0);

                var totalMatches = new Dictionary<string, int>();
                var totalRecords = new Dictionary<string, int>();
                var totalDocuments = new Dictionary<string, int>();

                var label = (string)objArgument;

                int current = 0;
                int step = _views.MainForm.datasetMain.Documents.Count / 100 + 1;

                int totalDocCount = _views.MainForm.datasetMain.Documents.Count;

                foreach (DataRow row in _views.MainForm.datasetMain.Documents.Rows)
                {

                    if (current % step == 0)
                        worker.ReportProgress((int)(1.0 * current / totalDocCount * 100));
                    try
                    {
                        var note_entities = _views.DocumentsService.GetDocumentText((double)row["ED_ENC_NUM"], "NOTE_ENTITIES");
                        //List<EntityInstance> entities = JsonConvert.DeserializeObject<List<EntityInstance>>((string)row["NOTE_ENTITIES"]);
                        List<EntityInstance> entities = JsonConvert.DeserializeObject<List<EntityInstance>>(note_entities);
                        var instances = entities.Where(t => t.label == label).ToList();
                        foreach (var instance in instances)
                        {
                            if (!totalMatches.ContainsKey(instance.text))
                                totalMatches.Add(instance.text, 1);
                            else totalMatches[instance.text]++;
                        }
                        var docInstances = instances.GroupBy(t => t.text).Select(t => t.Key).ToList();
                        foreach (var instance in docInstances)
                        {
                            if (!totalRecords.ContainsKey(instance))
                                totalRecords.Add(instance, 1);
                            else totalRecords[instance]++;

                            if (!totalDocuments.ContainsKey(instance))
                                totalDocuments.Add(instance, 1);
                            else totalDocuments[instance]++;
                        }
                    } catch (Exception e)
                    {
                        continue;
                    }
                }

                var topInstances = totalMatches.OrderBy(x => -x.Value).ToList();

                int sum = totalMatches.Sum(x => x.Value);

                for (int i = 0; i < 50 && i < topInstances.Count; i ++)
                {
                    var word = topInstances[i].Key;
                    //gridStatistics.Rows.Add(word, topInstances[i].Value, 100.0 * topInstances[i].Value / sum, totalDocuments[word], totalRecords[word], 100.0 * totalDocuments[word] / totalDocCount, 100.0 * totalRecords[word] / totalDocCount);
                    _entityStaticsData.Add(new EntityStatisticsModel
                    {
                        Word = word,
                        MatchesCount = topInstances[i].Value,
                        MatchesCountPercentage = (float)100.0 * topInstances[i].Value / sum,
                        MatchingDocsCount = totalDocuments[word],
                        MatchingRecordsCount = totalRecords[word],
                        MatchingDocsPercentage = (float)100.0 * totalDocuments[word] / totalDocCount,
                        MatchingRecordsPercentage = (float)100.0 * totalRecords[word] / totalDocCount
                    });                    
                }                
                //gridStatistics.AutoResizeColumns();

                return true;
				
			}
			catch (Exception ex)
			{
				

				MainForm.ShowExceptionMessage(ex);
			}

			return false;
		}


		protected void RaiseModifiedEvent(RegExpBase regExp)
		{
			if (this.Modified != null)
				this.Modified(regExp, EventArgs.Empty);
		}

		#endregion

		#region Examples

		#endregion

		#region Methods

	
		protected void SaveSettings()
		{
		
			Settings.Default.Save();
		}


		#endregion

		#region Interop

		[DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pt);

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public bool PreFilterMessage(ref Message m)
        {
            return false;
        }

        #endregion

    }

    [Serializable]
    public class EntityInstance
    {
        public string text { get; set; }
        public string label { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }
    public class EntityStatisticsModel
    {
        #region Fields

        public string Word { get; set; }
        public int MatchesCount { get; set; }
        public float MatchesCountPercentage { get; set; }
        public int MatchingDocsCount { get; set; }
        public float MatchingDocsPercentage { get; set; }
        public float MatchingRecordsPercentage { get; set; }
        public int MatchingRecordsCount { get; set; }

        #endregion
    }
}