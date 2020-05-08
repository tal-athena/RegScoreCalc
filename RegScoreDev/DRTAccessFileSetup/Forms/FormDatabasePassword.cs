using System;
using System.Windows.Forms;

namespace DRTAccessFileSetup.Forms
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
		}

		#endregion

		#region Events

		private void FormDatabasePassword_Load(object sender, EventArgs e)
		{
			
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
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				e.Cancel = true;
			}
		}

		#endregion
	}
}