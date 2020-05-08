using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;

using CefSharp.WinForms.Internals;

using RegScoreCalc.Code;
using RegScoreCalc.Data;

namespace RegScoreCalc
{
	public partial class FormCopyNotebook : Form
	{
		#region Properties

		public string FileName
		{
			get { return GetValidFileName(); }
		}

		#endregion

		#region Ctors

		public FormCopyNotebook(string fileName)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			txtFileName.Text = fileName;
		}

		#endregion

		#region Events

		private void FormCopyNotebook_Load(object sender, EventArgs e)
		{
			try
			{
				txtFileName.SelectionStart = txtFileName.TextLength;
				txtFileName.Activate();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormCopyNotebook_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (String.IsNullOrEmpty(this.FileName))
					{
						MessageBox.Show("Please input a valid file name", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						e.Cancel = true;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void txtFileName_TextChanged(object sender, EventArgs e)
		{
			try
			{
				btnOK.Enabled = GetValidFileName() != null;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected string GetValidFileName()
		{
			var fileName = txtFileName.Text;
			if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
				return null;

			return fileName;
		}

		#endregion
	}
}