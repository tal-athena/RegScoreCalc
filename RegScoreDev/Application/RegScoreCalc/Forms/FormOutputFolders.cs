using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using RegScoreCalc.Views.Models;

namespace RegScoreCalc
{
	public partial class FormOutputFolders : Form
	{
		#region Ctors

		public FormOutputFolders(List<FolderInfo> folders)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			grid.EnableHeadersVisualStyles = false;
			grid.ColumnHeadersDefaultCellStyle.BackColor = MainForm.ColorBackground;
			grid.AutoGenerateColumns = false;

			FillGrid(folders);
		}

		#endregion

		#region Events

		private void FormOutputFolders_Load(object sender, EventArgs e)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void FormOutputFolders_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
					OpenInExplorer();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnExplore_Click(object sender, EventArgs e)
		{
			try
			{
				OpenInExplorer();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnCopyToClipboard_Click(object sender, EventArgs e)
		{
			try
			{
				if (grid.SelectedRows.Count == 1)
				{
					var folderInfo = (FolderInfo) grid.SelectedRows[0].Tag;
					Clipboard.SetText(folderInfo.fullPath);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion

		#region Implementation

		private void FillGrid(List<FolderInfo> folders)
		{
			grid.Rows.Clear();

			foreach (var fi in folders.OrderByDescending(x => x.createdTime))
			{
				var index = grid.Rows.Add(fi.createdTime.ToShortTimeString() + "   " + fi.createdTime.ToShortDateString(), Path.GetFileName(fi.fullPath), Path.GetDirectoryName(fi.fullPath));
				grid.Rows[index].Tag = fi;
			}
		}

		private void OpenInExplorer()
		{
			if (grid.SelectedRows.Count == 1)
			{
				var folderInfo = (FolderInfo)grid.SelectedRows[0].Tag;

				Process.Start("explorer.exe", "\"" + folderInfo.fullPath + "\"");
			}
		}

		#endregion
	}
}