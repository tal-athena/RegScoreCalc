using System;
using System.Windows.Forms;
using System.ComponentModel;

namespace RegScoreCalc
{
	public partial class FormLoadingProgress : Form
	{
		#region Fields

		protected LengthyOperation _operation;

		protected object _objArgument;

		#endregion

		#region Ctors

		public FormLoadingProgress(LengthyOperation operaton, object objArgument)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			//////////////////////////////////////////////////////////////////////////

			_operation = operaton;

			_objArgument = objArgument;
		}

		#endregion

		#region Events

		private void FormLoadingProgress_Load(object sender, EventArgs e)
		{
			try
			{
				if (_operation != null)
					worker.RunWorkerAsync(_operation);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormLoadingProgress_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (worker.IsBusy)
					e.Cancel = true;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
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
			try
			{
				UpdateProgress(e);
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

		#endregion

		#region Implementation

		protected void UpdateProgress(ProgressChangedEventArgs e)
		{
			var state = e.UserState as string;
			if (e.ProgressPercentage > 0)
				lblDescription.Text = String.Format("Loading {0} table...", state);
			else
				lblTitle.Text = String.Format("Loading {0} database...", state);
		}

		#endregion
	}
}