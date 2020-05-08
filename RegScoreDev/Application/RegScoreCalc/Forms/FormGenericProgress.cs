using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace RegScoreCalc
{
	public partial class FormGenericProgress : Form
	{
		#region Fields

		protected LengthyOperation _operation;

		protected object _objArgument;

		protected DateTime _dtStart;

		protected bool _bResult;

		#endregion

		#region Properties

		public bool Result
		{
			get { return _bResult; }
		}

		public bool CancellationEnabled { get; set; }

		public bool CancelWithoutConfirmation { get; set; }

		#endregion

		#region Ctors

		public FormGenericProgress(string strOperation, LengthyOperation operaton, object objArgument, bool bDeterminated)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			//////////////////////////////////////////////////////////////////////////

			this.Text = strOperation;

			_operation = operaton;

			_objArgument = objArgument;

			progress.Style = bDeterminated ? ProgressBarStyle.Blocks : ProgressBarStyle.Marquee;

			this.CancellationEnabled = true;
		}

		#endregion

		#region Events

		private void FormGenericProgress_Load(object sender, System.EventArgs e)
		{
			btnStop.Enabled = this.CancellationEnabled;

			progressInd.StartProgress();

			if (_operation != null)
			{
				_dtStart = DateTime.Now;

				worker.RunWorkerAsync(_operation);
			}
		}

		private void FormGenericProgress_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (worker.IsBusy)
				e.Cancel = true;
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			//Check if it is finished?

			if (worker.IsBusy)
			{
				this.DialogResult = DialogResult.None;

				if (!this.CancelWithoutConfirmation)
				{
					DialogResult dlgres = MessageBox.Show("Do you wish to stop the operation?", MainForm.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
					if (dlgres == DialogResult.Yes)
						worker.CancelAsync();
				}
				else
					worker.CancelAsync();
			}
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				e.Result = _operation(worker, _objArgument);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			UpdateTime(e);
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Result is bool)
				_bResult = (bool) e.Result;

			if (groupLog.Visible)
			{
				this.Text = "Operation completed";

				btnStop.Text = "Close";
				btnStop.Top = lblProgress.Top;

				progress.Visible = false;
				progressInd.Visible = false;
				groupLog.Top = lblProgress.Top;
				groupLog.Height = this.Height - (groupLog.Top + 55);
				groupLog.Width -= (btnStop.Width + 20);
				groupLog.BringToFront();

				this.MinimumSize = this.Size;

				btnStop.Enabled = true;

				this.CenterToScreen();
			}
			else
				this.Close();
		}

		#endregion

		#region Implementation

		public void HideRemainingTime()
		{
			label4.Visible = false;
			lblLeft.Visible = false;
		}

		protected void UpdateTime(ProgressChangedEventArgs e)
		{
			try
			{
				TimeSpan span = DateTime.Now - _dtStart;

				lblElapsed.Text = String.Format("{0:hh\\:mm\\:ss}", span);

				if (e.ProgressPercentage != -1)
				{
					if (!progress.Visible)
					{
						progress.Visible = true;
						progressInd.Visible = false;

						progressInd.StopProgress();
					}

					if (progress.Value != e.ProgressPercentage || progress.Value == 0)
					{
						double dElapsed = span.TotalSeconds / e.ProgressPercentage;
						double dLeft = dElapsed * (100 - e.ProgressPercentage);

						span = new TimeSpan(0, 0, (int) dLeft);

						lblLeft.Text = String.Format("{0:hh\\:mm\\:ss}", span);
					}

					if (e.UserState != null)
						lblProgress.Text = e.ProgressPercentage.ToString() + "%    (" + (string) e.UserState + ")";
					else
						lblProgress.Text = e.ProgressPercentage.ToString() + "%";

					progress.Value = (e.ProgressPercentage < 100) ? e.ProgressPercentage : 100;
				}
				else
				{
					var errorMessage = e.UserState as string;
					if (!String.IsNullOrEmpty(errorMessage))
					{
						gridLog.Rows.Add(errorMessage);

						ShowLog();

						return;
					}
					else
					{
						progressInd.Top = lblProgress.Top;
						progressInd.Width = btnStop.Right - progressInd.Left;

						if (!groupLog.Visible)
						{
							Size size = this.ClientSize;
							size.Height = progressInd.Top + progressInd.Bottom;

							this.ClientSize = size;

							btnStop.Visible = false;
						}

						lblProgress.Visible = false;
						label2.Visible = false;
						progress.Visible = false;

						progressInd.Visible = true;

						//lblProgress.Text = progress.Value.ToString() + "%    (saving processed records...)";
						lblProgress.Text = "Saving processed records...";

						progressInd.StartProgress();
					}
				}

				lblProgress.Update();
				lblElapsed.Update();
				lblLeft.Update();

				label2.Update();
				label3.Update();
				label4.Update();

				btnStop.Update();
			}
			catch
			{
			}
		}

		protected void ShowLog()
		{
			if (!groupLog.Visible)
			{
				groupLog.Visible = true;
				this.Height += 200;
				groupLog.Height = (this.Height - groupLog.Top) - 55;
				this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
				this.SizeGripStyle = SizeGripStyle.Show;
				this.CenterToScreen();
			}
		}

		#endregion
	}

	#region Delegates

	public delegate bool LengthyOperation(BackgroundWorker worker, object objArgument);

	#endregion
}