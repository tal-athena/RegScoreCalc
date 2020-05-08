using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace RegScoreCalc
{
	public partial class FormRecentDatasets : Form
	{
		#region Fields

		protected List<string> _listFiles;
		protected string _selectedFile;

		#endregion

		#region Properties

		public string SelectedDataset
		{
			get
			{
				if (!String.IsNullOrEmpty(_selectedFile))
					return _selectedFile;

				return (string) lvFiles.SelectedItems[0].Tag;
			}
		}

		#endregion

		#region Ctors

		public FormRecentDatasets(List<string> listFiles)
		{
			InitializeComponent();

			_listFiles = listFiles;

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormRecentDatasets_Load(object sender, EventArgs e)
		{
			try
			{
				FillFilesList();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormRecentDatasets_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (String.IsNullOrEmpty(_selectedFile) && lvFiles.SelectedItems.Count != 1)
						e.Cancel = true;
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnOpenFolder_Click(object sender, EventArgs e)
		{
			try
			{
				if (lvFiles.SelectedItems.Count == 1)
				{
					string strFullPath = lvFiles.SelectedItems[0].Tag as string;
					if (!String.IsNullOrEmpty(strFullPath))
						Process.Start("explorer.exe", @"/select, " + strFullPath);
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnClearList_Click(object sender, EventArgs e)
		{
			try
			{
				_listFiles.Clear();

				FillFilesList();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnOpenFromDisk_Click(object sender, EventArgs e)
		{
			try
			{
				var openFileDialog = new OpenFileDialog
				                     {
					                     Filter = "MS Access database (*.accdb;*.mdb)|*.accdb;*.mdb|All files (*.*)|*.*"
				                     };

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					_selectedFile = openFileDialog.FileName;
					this.DialogResult = DialogResult.OK;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void lvFiles_ItemActivate(object sender, EventArgs e)
		{
			try
			{
				this.DialogResult = DialogResult.OK;
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

		#region Implementation

		protected void FillFilesList()
		{
			lvFiles.Items.Clear();

			FileInfo fi;

			ListViewItem lvi;
			foreach (string strFullPath in _listFiles)
			{
				lvi = lvFiles.Items.Add(Path.GetFileName(strFullPath), 0);
				lvi.Tag = strFullPath;

				fi = new FileInfo(strFullPath);

				lvi.SubItems.Add(FormatFileTime(fi.LastWriteTime));
				lvi.SubItems.Add(FormatFileTime(fi.CreationTime));
				lvi.SubItems.Add(Path.GetDirectoryName(strFullPath));
			}

			if (lvFiles.Items.Count > 0)
			{
				lvFiles.Items[0].Selected = true;

				lvFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			}
			else
				lvFiles.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		protected string FormatFileTime(DateTime dt)
		{
			return String.Format("{0}   {1}", dt.ToShortTimeString(), dt.ToShortDateString());
		}

		protected bool IsFileRemote(string strPath)
		{
			try
			{
				Uri uri = new Uri(strPath);
				if (uri.IsUnc)
					return true;

				DriveInfo di = new DriveInfo(strPath);
				if (di.DriveType == DriveType.Network)
					return true;
			}
			catch { }

			return false;
		}

		#endregion
	}
}
