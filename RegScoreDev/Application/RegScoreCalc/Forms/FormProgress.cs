using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using RegExpLib.Processing;

using RegScoreCalc.Code;

namespace RegScoreCalc
{
	public partial class FormProgress : Form
	{
		#region Fields

		protected ViewsManager _views;
		protected ArrayList _arrDatabases;

		protected List<RegExpStatisticsProcessingResult> _statistics;
		protected string _strExpression;

		protected bool _replace;
		protected string _strReplacement;

		#endregion

		#region Properties

		public List<RegExpStatisticsProcessingResult> Statistics
		{
			get { return _statistics; }
		}

		#endregion

		#region Ctors

		public FormProgress(ViewsManager views, ArrayList arrDatabases, string strExpression, string strReplacement, bool replace)
		{
			InitializeComponent();

			_views = views;

			_arrDatabases = arrDatabases;
			_statistics = new List<RegExpStatisticsProcessingResult>();
			_strExpression = strExpression;
			_strReplacement = strReplacement;
			_replace = replace;
		}

		#endregion

		#region Events

		private void FormProgress_Load(object sender, EventArgs e)
		{
			progOperation.Maximum = _arrDatabases.Count;

			worker.RunWorkerAsync();
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				CalcStatistics();

				///////////////////////////////////////////////////////////////////////////////

				if (_replace)
					_views.MainForm.adapterDocuments.Fill();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
                if (e.ProgressPercentage < 0)
                    return;
				progOperation.Value = e.ProgressPercentage;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			worker.CancelAsync();
		}

		#endregion

		#region Implementation

		protected void CalcStatistics()
		{
			var progress = 0;

			foreach (string strDatabase in _arrDatabases)
			{
				ProcessDatabase(strDatabase);

				///////////////////////////////////////////////////////////////////////////////

				if (worker.CancellationPending)
					break;

				progress++;

				worker.ReportProgress(progress);
			}
		}

		protected bool ProcessDatabase(string strDatabase)
		{
			using (var processor = new ExternalRegExpToolWrapper(worker, false))
			{
				var results = processor.RegExp_CalcStatistics(_strExpression, _replace, _strReplacement, strDatabase, _views.MainForm.DbPassword);
				if (results != null)
				{
					var union = _statistics.Union(results.Items);

					_statistics = union.GroupBy(x => x.Word)
									   .Select(g => new RegExpStatisticsProcessingResult

									   {
										   Word = g.Key,
										   Count = g.Sum(x => x.Count)
									   })
									   .ToList();

					///////////////////////////////////////////////////////////////////////////////

					return true;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			return false;
		}

		#endregion
	}
}