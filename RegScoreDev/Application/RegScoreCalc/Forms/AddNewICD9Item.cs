using System;
using System.Windows.Forms;

using CefSharp.WinForms.Internals;

namespace RegScoreCalc.Forms
{
	public partial class AddNewICD9Item : Form
	{
		#region Fields

		public string icd;
		public string description;
		public bool regExp;
		public bool oneWay;
		public bool combination;

		#endregion

		#region Ctors

		public AddNewICD9Item(string Icd9, string Description, bool RegExp, bool OneWay, bool Combination, string title = "Add")
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
			tabControl1.BackColor = MainForm.ColorBackground;
			tabPage1.BackColor = MainForm.ColorBackground;
			tabPage2.BackColor = MainForm.ColorBackground;

			icd = Icd9;
			description = Description;
			regExp = RegExp;
			oneWay = OneWay;
			combination = Combination;

			txtCode.Text = icd;
			txtDiagnosis.Text = description;
			chbRegExp.Checked = regExp;
			chbOneWay.Checked = oneWay;
			this.Text = title;

			if (combination)
			{
				string[] separator = new string[] { " + " };
				var icdCodes = icd.Split(separator, StringSplitOptions.None);
				var descriptionArray = description.Split(separator, StringSplitOptions.None);
				if (icdCodes.Length >= 0)
				{
					try
					{
						txtICD1.Text = icdCodes[0];
						txtDiagnosis1.Text = descriptionArray[0];
					}
					catch
					{
					}
				}
				if (icdCodes.Length >= 1)
				{
					try
					{
						txtICD2.Text = icdCodes[1];
						txtDiagnosis2.Text = descriptionArray[1];
					}
					catch
					{
					}
				}
				if (icdCodes.Length >= 2)
				{
					try
					{
						txtICD3.Text = icdCodes[2];
						txtDiagnosis3.Text = descriptionArray[2];
					}
					catch
					{
					}
				}

				tabControl1.SelectTab(1);
			}
		}

		#endregion

		#region Events

		private void AddNewICD9Item_Load(object sender, EventArgs e)
		{
			try
			{
				if (String.IsNullOrEmpty(txtICD1.Text))
					txtCode.Activate();
				else
					txtICD1.Activate();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtDiagnosis.Text != "" && txtCode.Text != "")
				{
					icd = txtCode.Text;
					description = txtDiagnosis.Text;
					regExp = chbRegExp.Checked;
					oneWay = chbOneWay.Checked;
					combination = false;
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
					this.Close();
				}
				else
					MessageBox.Show(this, "Please provide all data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnSaveCombination_Click(object sender, EventArgs e)
		{
			try
			{
				if (txtDiagnosis1.Text != "" && txtICD1.Text != ""
				|| txtDiagnosis2.Text != "" && txtICD2.Text != ""
				|| txtDiagnosis3.Text != "" && txtICD3.Text != "")
				{
					icd = "";
					description = "";
					if (txtICD1.Text != "")
					{
						if (icd.Length > 0)
							icd += " + " + txtICD1.Text;
						else
							icd += txtICD1.Text;
					}
					if (txtICD2.Text != "")
					{
						if (icd.Length > 0)
							icd += " + " + txtICD2.Text;
						else
							icd += txtICD2.Text;
					}
					if (txtICD3.Text != "")
					{
						if (icd.Length > 0)
							icd += " + " + txtICD3.Text;
						else
							icd += txtICD3.Text;
					}

					//Description
					if (txtDiagnosis1.Text != "")
					{
						if (description.Length > 0)
							description += " + " + txtDiagnosis1.Text;
						else
							description += txtDiagnosis1.Text;
					}
					if (txtDiagnosis2.Text != "")
					{
						if (description.Length > 0)
							description += " + " + txtDiagnosis2.Text;
						else
							description += txtDiagnosis2.Text;
					}
					if (txtDiagnosis3.Text != "")
					{
						if (description.Length > 0)
							description += " + " + txtDiagnosis3.Text;
						else
							description += txtDiagnosis3.Text;
					}

					oneWay = true;
					combination = true;
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
					this.Close();
				}
				else
					MessageBox.Show(this, "Please provide all data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
				this.Close();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCancelCombination_Click(object sender, EventArgs e)
		{
			try
			{
				this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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