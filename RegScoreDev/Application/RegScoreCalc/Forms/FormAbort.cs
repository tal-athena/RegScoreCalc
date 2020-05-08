using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormAbort : Form
	{
		#region Properties

		public string Message
		{
			set { lblMessage.Text = value; }
		}

		#endregion

		#region Ctors

		public FormAbort()
		{
			InitializeComponent();
		}

		#endregion

		#region Events

		private void btnContinue_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnAbort_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		#endregion
	}
}
