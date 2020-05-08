using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ExportDocuments.Forms
{
	public partial class FormMain : Form
	{
		#region Types

		protected class WorkerArgs
		{
			public string InputDatabasePath { get; set; }
			public string OutputDatabasePath { get; set; }
			public bool UseTransaction { get; set; }
			public bool ReportWarnings { get; set; }
			public bool DeleteDocuments { get; set; }
			public string Password { get; set; }
		}

		#endregion

		#region Fields

		protected Code.Exporter _exporter;

		protected bool _aborted;

		#endregion

		#region Ctor

		public FormMain()
		{
			InitializeComponent();

			gridLog.Height += 60;
			_decraseLogHeight = true;
		}

		#endregion

		#region Fields

		protected Stopwatch _sw;

		protected bool _decraseLogHeight;

		#endregion

		#region Events

		private void FormMain_Load(object sender, EventArgs e)
		{
			try
			{
				LoadSettings();

				UpdateState(false);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (worker.IsBusy)
				{
					e.Cancel = true;

					///////////////////////////////////////////////////////////////////////////////

					if (worker.CancellationPending)
						return;

					///////////////////////////////////////////////////////////////////////////////

					//if (ShowConfirmationMessage("Do you wish to abort export?"))

					_aborted = true;
					worker.CancelAsync();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnBrowseInput_Click(object sender, EventArgs e)
		{
			try
			{
				if (BrowseForFile(txtInput))
					SaveSettings();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnBrowseOutput_Click(object sender, EventArgs e)
		{
			try
			{
				if (BrowseForFile(txtOutput))
					SaveSettings();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void chkbShowWarnings_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (_exporter != null)
					_exporter.ReportWarnings = chkbShowWarnings.Checked;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			try
			{
				if (worker.IsBusy)
				{
					//if (ShowConfirmationMessage("Do you wish to abort export?"))

					_aborted = true;
					worker.CancelAsync();

					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				if (!File.Exists(txtInput.Text))
					throw new Exception("Input database file does not exist");

				if (!File.Exists(txtOutput.Text))
					throw new Exception("Output database file does not exist");

				///////////////////////////////////////////////////////////////////////////////

				_sw = Stopwatch.StartNew();

				///////////////////////////////////////////////////////////////////////////////

				var args = new WorkerArgs
					        {
								UseTransaction = chkbUseTransaction.Checked,
								DeleteDocuments = chkbDeleteDocuments.Checked,
								ReportWarnings = chkbShowWarnings.Checked,
						        InputDatabasePath = txtInput.Text,
						        OutputDatabasePath = txtOutput.Text,
								Password = txtPassword.Text
					        };

				worker.RunWorkerAsync(args);

				///////////////////////////////////////////////////////////////////////////////

				UpdateState(true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				var args = (WorkerArgs) e.Argument;

				using (_exporter = new Code.Exporter(args.UseTransaction, args.DeleteDocuments, args.ReportWarnings, worker))
				{
					e.Result = _exporter.ExportDocuments(args.InputDatabasePath, args.OutputDatabasePath, args.Password);
				}
			}
			catch (Exception ex)
			{
				worker.ReportProgress(-1, ex.Message);

				HandleException(ex);
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			try
			{
				if (e.ProgressPercentage != -1)
				{
					progressBar.Value = e.ProgressPercentage;

					///////////////////////////////////////////////////////////////////////////////

					var totalRowsCopied = (int) e.UserState;

					lblTimeElapsed.Text = _sw.Elapsed.ToString("hh\\:mm\\:ss");
					lblTotalRowsProcessed.Text = totalRowsCopied.ToString();

					///////////////////////////////////////////////////////////////////////////////

					var speed = totalRowsCopied / _sw.Elapsed.TotalSeconds;
					lblProcessingSpeed.Text = speed.ToString("N0");

					///////////////////////////////////////////////////////////////////////////////

					if (_sw.Elapsed.TotalSeconds > 0 && e.ProgressPercentage > 0)
					{
						double percentageRemaining = 100 - e.ProgressPercentage;

						int remainingSeconds = (int) (_sw.Elapsed.TotalSeconds * (percentageRemaining / e.ProgressPercentage));
						var span = new TimeSpan(0, 0, remainingSeconds);

						lblTimeRemaining.Text = span.ToString("hh\\:mm\\:ss");
					}
				}
				else
				{
					if (gridLog.Rows.Count > 100)
						gridLog.Rows.RemoveAt(0);

					gridLog.Rows.Add(e.UserState as string);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				_exporter = null;

				///////////////////////////////////////////////////////////////////////////////

				UpdateState(false);

				///////////////////////////////////////////////////////////////////////////////

				if (e.Result is int)
				{
					var totalRowsCopied = (int) e.Result;

					///////////////////////////////////////////////////////////////////////////////

					if (!_aborted)
					{
						var message = String.Format("{0}{1}{1}Total rows copied: {2}{1}{1}Do you wish to open output database", "Export finished successfully.", Environment.NewLine, totalRowsCopied);

						if (ShowConfirmationMessage(message))
							OpenDatabaseInAccess(txtOutput.Text);
					}
					else
						ShowInfoMessage("Export aborted by user.");
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		protected void OpenDatabaseInAccess(string filePath)
		{
			var process = new Process
			              {
				              StartInfo = new ProcessStartInfo
				                          {
											  FileName = filePath,
					                          UseShellExecute = true
				                          }
			              };

			process.Start();
		}

		#endregion

		#region Implementation: general

		protected void LoadSettings()
		{
			txtInput.Text = Properties.Settings.Default.InputDatabasePath;
			txtOutput.Text = Properties.Settings.Default.OutputDatabasePath;
		}

		protected void SaveSettings()
		{
			Properties.Settings.Default.InputDatabasePath = txtInput.Text;
			Properties.Settings.Default.OutputDatabasePath = txtOutput.Text;
			Properties.Settings.Default.Save();
		}

		protected bool BrowseForFile(TextBox txt)
		{
			var filter = "Access files|*.mdb;*.accdb|All files|*.*";

			var dlg = new OpenFileDialog
			          {
				          Filter = filter,
				          FileName = txt.Text
			          };

			///////////////////////////////////////////////////////////////////////////////

			if (dlg.ShowDialog(this) == DialogResult.OK)
			{
				txt.Text = dlg.FileName;

				return true;
			}

			///////////////////////////////////////////////////////////////////////////////

			return false;
		}

		protected void UpdateState(bool busy)
		{
			if (busy)
			{
				_aborted = false;

				gridLog.Rows.Clear();

				progressBar.Value = 0;
			}

			///////////////////////////////////////////////////////////////////////////////

			chkbUseTransaction.Visible = !busy;
			chkbDeleteDocuments.Visible = !busy;

			progressBar.Visible = busy;

			btnExit.Enabled = !busy;

			txtInput.Enabled = !busy;
			txtOutput.Enabled = !busy;
			btnBrowseInput.Enabled = !busy;
			btnBrowseOutput.Enabled = !busy;

			///////////////////////////////////////////////////////////////////////////////

			if (busy)
			{
				this.AcceptButton = btnExit;
				this.CancelButton = btnStart;
			}
			else
			{
				this.AcceptButton = btnStart;
				this.CancelButton = btnExit;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (busy && _decraseLogHeight)
			{
				gridLog.Height -= 60;
				_decraseLogHeight = false;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (busy)
			{
				lblTimeElapsedLabel.Visible = true;
				lblTimeElapsed.Visible = true;
				lblTimeElapsed.Text = String.Empty;

				lblTimeRemainingLabel.Visible = true;
				lblTimeRemaining.Visible = true;
				lblTimeRemaining.Text = String.Empty;

				lblTotalRowsProcessedLabel.Visible = true;
				lblTotalRowsProcessed.Visible = true;
				lblTotalRowsProcessed.Text = String.Empty;

				lblProcessingSpeedLabel.Visible = true;
				lblProcessingSpeed.Visible = true;
				lblProcessingSpeed.Text = String.Empty;
			}

			///////////////////////////////////////////////////////////////////////////////

			btnStart.Text = busy ? "Abort" : "Start";
		}

		#endregion

		#region Helpers

		protected bool ShowConfirmationMessage(string message)
		{
			return MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes;
		}

		protected void ShowInfoMessage(string message)
		{
			MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected void HandleException(Exception ex)
		{
			MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		#endregion
	}
}