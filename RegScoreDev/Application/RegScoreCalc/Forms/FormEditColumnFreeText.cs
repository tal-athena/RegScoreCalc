using System;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEditColumnFreeText : Form
	{
		#region Properties

		public string Value
		{
			get { return txtValue.Text; }
			set { txtValue.Text = value; }
		}

		#endregion

		#region Ctors

		public FormEditColumnFreeText()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormEditColumnFreeText_Load(object sender, System.EventArgs e)
		{
			if (!String.IsNullOrEmpty(txtValue.Text))
				txtValue.SelectionStart = txtValue.TextLength;
		}

		private void FormEditColumnFreeText_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion
	}
}