using System;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormDatabasePassword : Form
	{
		#region Properties

		public string Password
		{
			get { return txtPassword.Text; }
		}

		#endregion

		#region Ctors

		public FormDatabasePassword()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormDatabasePassword_Load(object sender, EventArgs e)
		{
			try
			{

			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormDatabasePassword_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (String.IsNullOrEmpty(txtPassword.Text))
					{
						MessageBox.Show("Password cannot be empty");
						e.Cancel = true;
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
				e.Cancel = true;
			}
		}

		#endregion
	}
}