using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using DRTAccessFileSetup.Code;

namespace DRTAccessFileSetup.Forms
{
	public partial class FormSelectDatabase : Form
	{
		#region Constants

		protected const string _historyFile = "History.txt";

		#endregion

		#region Properties

		public string SelectedFile
		{
			get { return lbHistory.SelectedItem as string; }
			private set {}
		}

		#endregion

		#region Ctors

		public FormSelectDatabase()
		{
			InitializeComponent();
		}

		#endregion

		#region Events

		private void FormSelectDatabase_Load(object sender, EventArgs e)
		{
			try
			{
				LoadHistory();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void FormSelectDatabase_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (String.IsNullOrEmpty(this.SelectedFile))
					{
						Program.ShowInfoMessage("Please select a database file");
						e.Cancel = true;
					}
					else
						SaveHistory();
				}
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnOpenSelected_Click(object sender, EventArgs e)
		{
			try
			{
				OpenFromHistory(lbHistory.SelectedIndex);
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void lbHistory_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				OpenFromHistory(lbHistory.IndexFromPoint(e.Location));
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnSelectFile_Click(object sender, EventArgs e)
		{
			try
			{
				SelectFile();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			try
			{
				lbHistory.Items.Clear();

				SaveHistory();
			}
			catch (Exception ex)
			{
				Program.HandleException(ex);
			}
		}

		#endregion

		#region Implementation

		protected void LoadHistory()
		{
			lbHistory.Items.Clear();

			string fullPath = GetHistoryFilePath();
			if (File.Exists(fullPath))
			{
				var lines = File.ReadAllLines(fullPath);
				if (lines.Length > 0)
				{
					lbHistory.Items.AddRange(lines.ToArray<object>());

					lbHistory.SelectedIndex = 0;
				}
			}
		}

		protected void SaveHistory()
		{
			var history = lbHistory.Items.OfType<string>();
			File.WriteAllLines(GetHistoryFilePath(), history);
		}

		protected string GetHistoryFilePath()
		{
			string fullPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _historyFile);

			return fullPath;
		}

		protected void SelectFile()
		{
			OpenFileDialog openFile = new OpenFileDialog();
			openFile.Filter = "MS Access database (*.accdb;*.mdb)|*.accdb;*.mdb|All files (*.*)|*.*";
			if (openFile.ShowDialog(this) == DialogResult.OK)
			{
				for (var i = 0; i < lbHistory.Items.Count; i++)
				{
					var item = lbHistory.Items[i] as string;
					if (String.Compare(item, openFile.FileName, StringComparison.OrdinalIgnoreCase) == 0)
					{
						lbHistory.Items.RemoveAt(i);
						i--;
					}
				}
				lbHistory.Items.Insert(0, openFile.FileName);
				lbHistory.SelectedIndex = 0;

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		protected void OpenFromHistory(int index)
		{
			if (index >= 0 && index < lbHistory.Items.Count)
			{
				if (index > 0)
				{
					var item = lbHistory.Items[index];
					lbHistory.Items.RemoveAt(index);

					lbHistory.Items.Insert(0, item);
				}

				lbHistory.SelectedIndex = 0;

				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		#endregion
	}
}
