using System;
using System.Linq;
using System.Windows.Forms;

using RegExpLib.Core;
using RegExpLib.Processing;

namespace RegScoreCalc
{
	public partial class FormEditPrefixSuffix : Form
	{
		#region Fields

		protected RegExp _regExp;

		#endregion

		#region Ctors

		public FormEditPrefixSuffix(RegExp regExp)
		{
			InitializeComponent();

			_regExp = regExp;
		}

		#endregion

		#region Events

		private void FormEditPrefixSuffix_Load(object sender, EventArgs e)
		{
			try
			{
				txtPrefixValue.Text = _regExp.PrefixMatch;
				txtSuffixValue.Text = _regExp.SuffixMatch;

				FillValues();

				txtPrefixValue.Focus();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormEditPrefixSuffix_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					_regExp.PrefixMatch = txtPrefixValue.Text;
					_regExp.SuffixMatch= txtSuffixValue.Text;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void lbPrefixValues_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (lbPrefixValues.SelectedItem is string)
					txtPrefixValue.Text = (string)lbPrefixValues.SelectedItem;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void lbSuffixValues_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				if (lbSuffixValues.SelectedItem is string)
					txtSuffixValue.Text = (string)lbSuffixValues.SelectedItem;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCopyPrefixValue_Click(object sender, EventArgs e)
		{
			try
			{
				if (lbPrefixValues.SelectedItem is string)
					Clipboard.SetText((string)lbPrefixValues.SelectedItem);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCopySuffixValue_Click(object sender, EventArgs e)
		{
			try
			{
				if (lbPrefixValues.SelectedItem is string)
					Clipboard.SetText((string)lbPrefixValues.SelectedItem);
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

		#region Implementation

		protected void FillValues()
		{
			FillValues(lbPrefixValues, "prefix match");
			FillValues(lbSuffixValues, "suffix match");
		}

		protected void FillValues(ListBox lbValues, string strColumn)
		{
			var processor = new RegExpProcessor(MainForm.ViewsManager.MainForm.datasetMain.RegExp);

			lbPrefixValues.Items.Clear();
			lbPrefixValues.Items.AddRange(processor.Items.Select(x => x.PrefixMatch)
			                                 .Distinct()
			                                 .Cast<object>()
			                                 .ToArray());

			lbSuffixValues.Items.Clear();
			lbSuffixValues.Items.AddRange(processor.Items.Select(x => x.SuffixMatch)
											 .Distinct()
											 .Cast<object>()
											 .ToArray());
		}

		#endregion
	}
}
