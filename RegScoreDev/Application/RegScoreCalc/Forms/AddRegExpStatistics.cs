using System;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class AddRegExpStatistics : Form
	{
		#region Fields

		public string regExp = "";
		public string replaceText = "";
		public bool replace = false;

		#endregion

		#region Ctors

		public AddRegExpStatistics(string RegExp, bool Replace, string ReplaceText)
		{
			regExp = RegExp;
			replace = Replace;
			replaceText = ReplaceText;
			InitializeComponent();

			txtReplace.Text = replaceText;
			txtRegExp.Text = regExp;
			chbReplace.Checked = replace;
		}

		#endregion

		#region Events

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (txtRegExp.Text != "")
			{
				regExp = txtRegExp.Text;
				replace = chbReplace.Checked;
				replaceText = txtReplace.Text;

				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();
			}
			else
				MessageBox.Show(this, "Enter RegExp!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		#endregion
	}
}