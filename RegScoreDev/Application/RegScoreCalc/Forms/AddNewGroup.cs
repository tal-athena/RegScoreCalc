using System;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class AddNewGroup : Form
	{
		#region Fields

		public string GroupName
		{
			get { return txtGroupName.Text; }
			set { txtGroupName.Text = value; }
		}

		#endregion

		#region Ctors

		public AddNewGroup(string currentName, string title = "Add")
		{
			InitializeComponent();

			this.GroupName = currentName;

			this.Text = title;

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtGroupName.Text != "")
				{
					this.DialogResult = DialogResult.OK;
					this.Close();
				}
				else
					MessageBox.Show(this, "Name cannot be empty", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				this.DialogResult = DialogResult.Cancel;
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion
	}
}