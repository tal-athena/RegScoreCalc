using System;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class FormGenerateSVMProgress : Form
	{
		public bool cancel;

		public FormGenerateSVMProgress()
		{
			InitializeComponent();
		}

		public void SetProgressStyle(ProgressBarStyle style)
		{
			this.progressBarGenerate.Style = style;
		}

		public void UpdateProgress(int value)
		{
			if (progressBarGenerate.InvokeRequired)
				progressBarGenerate.Invoke(new Action(() => this.progressBarGenerate.Value = value));
			else
				this.progressBarGenerate.Value = value;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			cancel = true;
		}
	}
}