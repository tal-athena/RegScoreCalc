using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

using Plumbing2.Code;

namespace Plumbing2.Forms
{
	public partial class FormMain : Form
	{
		#region Fields

		protected readonly PlumbingCore _plumbingCore;

		protected bool _completed;

		protected int _result;

		#endregion

		#region Properties

		public int ProcessingResult
		{
			get { return _result; }
		}

		#endregion

		#region Ctors

		public FormMain(PlumbingCore plumbingCore)
		{
			InitializeComponent();

			_result = 3;

			_plumbingCore = plumbingCore;
		}

		#endregion

		#region Events

		private void FormMain_Load(object sender, EventArgs e)
		{
			try
			{
				txtOutputFolder.Text = _plumbingCore.OutputFolder;
				txtInputFileName.Text = _plumbingCore.DatabaseFileName;				
				txtLogFileName.Text = _plumbingCore.LogFileName;

				txtJSON.Text = File.ReadAllText(Path.Combine(txtOutputFolder.Text, txtParametersFileName.Text));

				progress.Show();

				worker.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				HandleException(ex);

				this.Close();
			}
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (!_completed)
					e.Cancel = true;
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
				_plumbingCore.Logger.StdEvent += plumbing_StdEvent;

				_result = _plumbingCore.StartProcessing();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			_completed = true;
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				progress.Visible = false;

				btnExit.Enabled = true;

				_completed = true;

				if (_result == 0)
				{
					if (!chkbKeepOpen.Checked)
						this.Close();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void plumbing_StdEvent(object sender, StdEventArgs e)
		{
			try
			{
				if (txtOutput.InvokeRequired)
				{
					var stdevent = new Logger.StdEventHandler(plumbing_StdEvent);

					txtOutput.Invoke(stdevent, new object[] { sender, e });
				}
				else
				{
					txtOutput.AppendText(e.Message + Environment.NewLine);
					txtOutput.SelectionStart = txtOutput.TextLength;
					txtOutput.ScrollToCaret();
				}
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

		#endregion

		#region Helpers

		protected void HandleException(Exception ex)
		{
			_plumbingCore.Logger.LogError(ex.Message);
		}

		#endregion
	}
}
