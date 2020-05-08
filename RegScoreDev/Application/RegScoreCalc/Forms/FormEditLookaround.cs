using System;
using System.Windows.Forms;

using CefSharp.WinForms.Internals;

namespace RegScoreCalc.Forms
{
	public partial class FormEditLookaround : Form
	{
		#region Properties

		public string Value
		{
			get { return txtValue.Text; }
			set { txtValue.Text = value; }
		}

		#endregion

		#region Ctors

		public FormEditLookaround()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			splitter.BackColor = MainForm.ColorBackground;

			txtValue.Activate();

			LoadSize();
		}

		#endregion

		#region Events

		private void FormEditLookaround_Load(object sender, EventArgs e)
		{
			try
			{
				lvQuickActions.SetBuddy(txtValue);

				txtValue.Select(txtValue.TextLength, 0);

				txtValue.Activate();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormEditLookaround_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				SaveSize();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region Implementation

		protected void LoadSize()
		{
			if (Properties.Settings.Default.FormEditLookaround_Width > 0)
				this.Width = Properties.Settings.Default.FormEditLookaround_Width;

			if (Properties.Settings.Default.FormEditLookaround_Height > 0)
				this.Height = Properties.Settings.Default.FormEditLookaround_Height;

			if (Properties.Settings.Default.FormEditLookaround_SplitterDistance > 0)
				splitter.SplitterDistance = Properties.Settings.Default.FormEditLookaround_SplitterDistance;
		}

		protected void SaveSize()
		{
			Properties.Settings.Default.FormEditLookaround_Width = this.Width;
			Properties.Settings.Default.FormEditLookaround_Height = this.Height;
			Properties.Settings.Default.FormEditLookaround_SplitterDistance = splitter.SplitterDistance;

			Properties.Settings.Default.Save();
		}

		#endregion
	}
}