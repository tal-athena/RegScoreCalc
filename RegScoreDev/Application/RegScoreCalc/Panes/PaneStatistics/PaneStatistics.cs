using System;
using System.IO;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;

using RegExpLib.Core;
using RegExpLib.Processing;

using RegScoreCalc.Code;
using RegScoreCalc.Forms;

namespace RegScoreCalc
{
	public partial class PaneStatistics : Pane
	{
		#region Fields

		protected string _strBrowserPath;

		protected List<RegExpStatisticsProcessingResult> _statisticsList;

		protected ListViewColumnSorter _sorterStatistics;

		#endregion

		#region Ctors

		public PaneStatistics()
		{
			InitializeComponent();

			_strBrowserPath = "";

			_statisticsList = new List<RegExpStatisticsProcessingResult>();
		}

		#endregion

		#region Events

		private void PaneStatistics_Load(object sender, EventArgs e)
		{
			try
			{
				_strBrowserPath = Path.GetDirectoryName(_views.MainForm.RegExpsDbPath);
			}
			catch
			{
			}

			_sorterStatistics = new ListViewColumnSorter();
			lvStatistics.ListViewItemSorter = _sorterStatistics;

			UpdateFileBrowser();
		}

		private void btnCalcStatistics_Click(object sender, EventArgs e)
		{
			CalcStatistics();
		}

		private void btnRemoveDatabases_Click(object sender, EventArgs e)
		{
			RemoveDatabases();
		}

		private void btnGoLevelUp_Click(object sender, EventArgs e)
		{
			GoLevelUp();
		}

		private void lvDatabases_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				RemoveDatabases();
		}

		private void btnAddDatabases_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem lvi in lvFileBrowser.SelectedItems)
			{
				AddDatabase((string) lvi.Tag);
			}
		}

		private void btnClearStatistics_Click(object sender, EventArgs e)
		{
			RemoveStatistics();
		}

		private void btnAddRegExps_Click(object sender, EventArgs e)
		{
			AddRegExps(false);
		}

		private void btnAddAllWords_Click(object sender, EventArgs e)
		{
			AddRegExps(true);
		}

		private void lvFileBrowser_ItemActivate(object sender, EventArgs e)
		{
			NavigateSelection();
		}

		protected void OnCalculateStatistics_Clicked(object sender, EventArgs e)
		{
			CalcStatistics();
		}

		protected void OnAddRegExps_Clicked(object sender, EventArgs e)
		{
			AddRegExps(true);
		}

		private void lvFileBrowser_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Back)
			{
				GoLevelUp();
				e.Handled = true;
			}
			else if (e.KeyCode == Keys.Return)
			{
				NavigateSelection();
				e.Handled = true;
			}
		}

		private void lvStatistics_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				RemoveStatistics();
		}

		private void lvStatistics_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column == _sorterStatistics.SortColumn)
			{
				if (_sorterStatistics.Order == SortOrder.Ascending)
					_sorterStatistics.Order = SortOrder.Descending;
				else
					_sorterStatistics.Order = SortOrder.Ascending;
			}
			else
			{
				_sorterStatistics.SortColumn = e.Column;
				_sorterStatistics.Order = SortOrder.Ascending;
			}

			lvStatistics.Sort();
		}

		#endregion

		#region Operations

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
			RibbonPanel panel = new RibbonPanel("Statistics");
			tab.Panels.Add(panel);

			RibbonButton btnCalculateStatistics = new RibbonButton("Calculate Statistics");
			panel.Items.Add(btnCalculateStatistics);

			btnCalculateStatistics.Image = Properties.Resources.CalcScores;
			btnCalculateStatistics.SmallImage = Properties.Resources.CalcScores;
			btnCalculateStatistics.Click += new EventHandler(OnCalculateStatistics_Clicked);
			btnCalculateStatistics.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnAddAlllWords = new RibbonButton("Add All Words");
			panel.Items.Add(btnAddAlllWords);
			btnAddAlllWords.Image = Properties.Resources.AddAllRegExpsMedium;
			btnAddAlllWords.SmallImage = Properties.Resources.AddAllRegExpsMedium;
			btnAddAlllWords.Click += new EventHandler(OnAddRegExps_Clicked);
			btnAddAlllWords.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			RibbonButton btnRegExp = new RibbonButton("regexp");
			panel.Items.Add(btnRegExp);
			btnRegExp.Image = Properties.Resources.AddRegExps;
			btnRegExp.SmallImage = Properties.Resources.AddRegExps;
			btnRegExp.Click += btnRegExp_Click;
			btnRegExp.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
		}

		public override void UpdatePane()
		{
			UpdateFileBrowser();
		}

		#endregion

		#region Implementation

		protected void UpdateFileBrowser()
		{
			try
			{
				lvFileBrowser.Items.Clear();

				if (Directory.Exists(_strBrowserPath))
				{
					DirectoryInfo diCurrent = new DirectoryInfo(_strBrowserPath);
					if (diCurrent != null)
					{
						ListViewItem lvi;
						string strTemp;

						NameComparer comparer = new NameComparer();

						if (!String.IsNullOrEmpty(_strBrowserPath))
						{
							if (Path.GetPathRoot(_strBrowserPath) != _strBrowserPath)
							{
								lvi = lvFileBrowser.Items.Add("..");
								if (lvi != null)
								{
									lvi.ImageIndex = 0;
									lvi.Tag = Path.GetDirectoryName(_strBrowserPath);
								}
							}
						}

						DirectoryInfo[] arrDirectories = diCurrent.GetDirectories();
						Array.Sort(arrDirectories, comparer);

						foreach (DirectoryInfo di in arrDirectories)
						{
							if (di.Name != ".")
							{
								lvi = lvFileBrowser.Items.Add(di.Name);
								if (lvi != null)
								{
									lvi.Tag = di.FullName;
									lvi.ImageIndex = 0;

									lvi.SubItems.Add("");

									strTemp = di.LastWriteTime.ToShortTimeString() + "   " + di.LastWriteTime.ToShortDateString();
									lvi.SubItems.Add(strTemp);
								}
							}
						}

						FileInfo[] arrFiles = diCurrent.GetFiles();
						Array.Sort(arrFiles, comparer);

						foreach (FileInfo fi in arrFiles)
						{
							if (string.Compare(fi.Extension, ".mdb", true) == 0 || string.Compare(fi.Extension, ".accdb", true) == 0)
							{
								lvi = lvFileBrowser.Items.Add(fi.Name);
								if (lvi != null)
								{
									lvi.Tag = fi.FullName;
									lvi.ImageIndex = 1;

									strTemp = (fi.Length / 1024).ToString() + " KBytes";
									lvi.SubItems.Add(strTemp);

									strTemp = fi.LastWriteTime.ToShortTimeString() + "   " + fi.LastWriteTime.ToShortDateString();
									lvi.SubItems.Add(strTemp);
								}
							}
						}
					}

					if (lvFileBrowser.Columns.Count >= 3)
						lvFileBrowser.Columns[2].Width = 150;
				}
				else
				{
					ListViewItem lvi;
					DriveInfo di;
					string strDriveSize;

					string[] arrDrives = Environment.GetLogicalDrives();
					foreach (string strDrive in arrDrives)
					{
						lvi = lvFileBrowser.Items.Add(strDrive.Replace(@"\", ""));
						if (lvi != null)
						{
							lvi.Tag = strDrive;
							lvi.ImageIndex = 2;

							try
							{
								di = new DriveInfo(strDrive);
								strDriveSize = (((di.TotalSize / 1024) / 1024) / 1024).ToString() + " GBytes";
								lvi.SubItems.Add(strDriveSize);
							}
							catch
							{
							}
						}
					}

					lvFileBrowser.Columns[2].Width = 0;
				}

				if (lvFileBrowser.Items.Count > 0)
				{
					lvFileBrowser.Items[0].Selected = true;
					lvFileBrowser.Items[0].Focused = true;
				}
			}
			catch
			{
			}
		}

		protected void RelativeNavigateFileBrowser(string strRelativePath)
		{
			try
			{
				if (strRelativePath != "..")
				{
					string strBrowserPath = Path.Combine(_strBrowserPath, strRelativePath);
					if (Directory.Exists(strBrowserPath))
						_strBrowserPath = strBrowserPath;
					else if (File.Exists(strBrowserPath))
						AddDatabase(strBrowserPath);
				}
				else
				{
					if (!String.IsNullOrEmpty(_strBrowserPath))
					{
						if (Path.GetPathRoot(_strBrowserPath) != _strBrowserPath)
							_strBrowserPath = Path.GetDirectoryName(_strBrowserPath);
						else
							_strBrowserPath = "";
					}
				}
			}
			catch
			{
			}

			UpdateFileBrowser();
		}

		protected void NavigateSelection()
		{
			try
			{
				if (lvFileBrowser.SelectedItems.Count > 0)
					RelativeNavigateFileBrowser((string) lvFileBrowser.SelectedItems[0].Tag);
			}
			catch
			{
			}
		}

		protected void AddDatabase(string strDatabase)
		{
			try
			{
				ListViewItem lvi;

				DirectoryInfo di;
				string strTemp;

				FileInfo fi = new FileInfo(strDatabase);
				if (fi != null)
				{
					if (fi != null && (string.Compare(fi.Extension, ".mdb", true) == 0 || string.Compare(fi.Extension, ".accdb", true) == 0))
					{
						bool bExists = false;
						for (int i = 0; i < lvDatabases.Items.Count; i++)
						{
							lvi = lvDatabases.Items[i];
							if (lvi != null)
							{
								lvi.Selected = false;
								if (String.Compare((string) lvi.Tag, strDatabase, true) == 0)
								{
									bExists = true;
									break;
								}
							}
						}

						if (!bExists)
						{
							lvi = lvDatabases.Items.Add(Path.GetFileName(strDatabase));
							if (lvi != null)
							{
								lvi.Tag = strDatabase;
								lvi.ImageIndex = 1;

								try
								{
									di = new DirectoryInfo(strDatabase);
									strTemp = di.LastWriteTime.ToShortTimeString() + "   " + di.LastWriteTime.ToShortDateString();
									lvi.SubItems.Add(strTemp);
								}
								catch
								{
								}
							}
						}
					}
				}

				if (lvDatabases.Items.Count > 0)
				{
					lvDatabases.Items[lvDatabases.Items.Count - 1].Selected = true;
					lvDatabases.Items[lvDatabases.Items.Count - 1].Focused = true;
				}
			}
			catch
			{
			}
		}

		protected void RemoveDatabases()
		{
			try
			{
				ListViewItem lvi;
				for (int i = 0; i < lvDatabases.SelectedItems.Count; i++)
				{
					lvi = lvDatabases.SelectedItems[i];
					lvi.Remove();
					i--;
				}
			}
			catch
			{
			}
		}

		protected void RemoveStatistics()
		{
			try
			{
				foreach (ListViewItem lvi in lvStatistics.Items)
				{
					if (lvi.Selected)
						_statisticsList.Remove((RegExpStatisticsProcessingResult) lvi.Tag);
				}

				UpdateStatistics();
			}
			catch
			{
			}
		}

		protected void CalcStatistics()
		{
			string textFromRegExp = "";
			if (btnToggleWholeWord.Tag as string == "off")
				textFromRegExp = txtRegExp.Text;
			else
				textFromRegExp = txtRegularExpressionWholeWord.Text;

			if (!String.IsNullOrEmpty(textFromRegExp) && lvDatabases.Items.Count > 0)
			{
				try
				{
					lvStatistics.Items.Clear();

					ArrayList arrDatabases = new ArrayList();
					foreach (ListViewItem lvi in lvDatabases.Items)
					{
						arrDatabases.Add((string) lvi.Tag);
					}

					bool replace = chbReplace.Checked;
					var txtReplaceExp = txtReplace.Text;
					FormProgress formProgess = new FormProgress(_views, arrDatabases, textFromRegExp, txtReplaceExp, replace);
					formProgess.ShowDialog();

					_statisticsList = formProgess.Statistics;
					UpdateStatistics();
				}
				catch
				{
				}

				MessageBox.Show("Operation finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
				MessageBox.Show("This operation requires regular expression and at least one target database file", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected void UpdateStatistics()
		{
			try
			{
				lvStatistics.Items.Clear();

				float fTotalCount = 0;
				foreach (var wc in _statisticsList)
				{
					fTotalCount += wc.Count;
				}

				float fPercentage;

				foreach (var wc in _statisticsList)
				{
					//lviWord = lvStatistics.Items.Add(wc.Word);

					var lviWord = lvStatistics.Items.Add("");
					lviWord.SubItems.Add(wc.Word);

					lviWord.ImageIndex = 3;
					lviWord.Tag = wc;

					lviWord.SubItems.Add(wc.Count.ToString());

					fPercentage = (float) (wc.Count * 100) / fTotalCount;
					lviWord.SubItems.Add(fPercentage.ToString("0.00"));
				}

				if (lvStatistics.Items.Count > 0)
				{
					lvStatistics.Items[0].Selected = true;
					lvStatistics.Items[0].Focused = true;
				}
			}
			catch
			{
			}
		}

		protected void GoLevelUp()
		{
			RelativeNavigateFileBrowser("..");
		}

		protected void AddRegExps(bool bAll)
		{
			if (lvStatistics.Items.Count > 0)
			{
				try
				{
					int nSuccess = 0;

					string strCommand = "INSERT INTO RegExp (RegExp) VALUES (?)";

					OleDbCommand command;

					bool bProcess;

					_views.MainForm.adapterRegExp.Connection.Open();
					string regExpText = "";
					foreach (ListViewItem lvi in lvStatistics.Items)
					{
						try
						{
							bProcess = false;

							if (!bAll)
							{
								if (lvi.Selected)
									bProcess = true;
							}
							else
								bProcess = true;

							if (bProcess)
							{
								command = new OleDbCommand(strCommand, _views.MainForm.adapterRegExp.Connection);
								regExpText = lvi.Text;
								if (String.IsNullOrEmpty(regExpText))
								{
									var regExpTag = (RegExpStatisticsProcessingResult)lvi.Tag;
									regExpText = regExpTag.Word;
								}
								command.Parameters.Add("RegExp", OleDbType.Char)
								       .Value = regExpText.ToLower();

								////Check if the regExp already exists
								//bool alreadyExists = false;
								//var regExpAlreadyExists = mds.RegExp.Select("RegExp = '" + regExpText + "'");
								//if (regExpAlreadyExists.Length <1)
								//{
								//    alreadyExists = true;
								//}

								//var commandCheck = new OleDbCommand("SELECT * FROM RegExp WHERE ", _views.MainForm.adapterRegExp.Connection);
								//if (!alreadyExists)
								//{
								try
								{
									if (command.ExecuteNonQuery() == 1)
										nSuccess++;
								}
								catch
								{
								}
								//}
							}
						}
						catch (System.Exception ex)
						{
							MessageBox.Show("Failed to add the keyword: " + regExpText + Environment.NewLine + Environment.NewLine + ex.Message, MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						}
					}

					_views.MainForm.adapterRegExp.Connection.Close();
				}
				catch
				{
				}
			}

			_views.MainForm.adapterRegExp.Fill(_views.MainForm.datasetMain.RegExp);

			RaiseDataModifiedEvent();

			MessageBox.Show("Operation finished", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected void AddToExceptions(MainDataSet.RegExpRow rowRegExp, RegExpStatisticsProcessingResult wordcount)
		{
			var regExp = RegExpFactory.Create_RegExp(rowRegExp, false);
			regExp.AddException(wordcount.Word);

			regExp.SafeSave(rowRegExp, true);
		}

		#endregion

		private void btnToggleWholeWord_Click(object sender, EventArgs e)
		{
			Button btn = (Button) sender;
			if (btn.Tag.ToString() == "off") //if was off now turn it on
			{
				btnToggleWholeWord.BackColor = Color.Green;
				btnToggleWholeWord.Tag = "on";

				var oldText = txtRegExp.Text;
				txtRegularExpressionWholeWord.Text = "\\b\\w*" + oldText + "\\w*\\b";

				this.txtRegularExpressionWholeWord.Visible = true;

				//Change size of the tables

				ChangePositionOnWholeWord(30);
			}
			else
			{
				btnToggleWholeWord.BackColor = Color.Gray;
				btnToggleWholeWord.Tag = "off";

				//var oldText = txtRegExp.Text.Substring(5, txtRegExp.Text.Length - 10);
				txtRegularExpressionWholeWord.Text = "";

				this.txtRegularExpressionWholeWord.Visible = false;

				ChangePositionOnWholeWord(-30);
			}
		}

		private void ChangePositionOnWholeWord(int change)
		{
			lvDatabases.Top = lvDatabases.Top + change;
			lvDatabases.Height = lvDatabases.Height - change;

			lvFileBrowser.Top = lvFileBrowser.Top + change;
			lvFileBrowser.Height = lvFileBrowser.Height - change;

			label2.Top = label2.Top + change;
			btnRemoveDatabases.Top += change;
			label3.Top += change;
			btnGoLevelUp.Top += change;
			btnAddDatabases.Top += change;

			if (change > 0)
				groupBox1.Visible = false;
			else
				groupBox1.Visible = true;
		}

		private void btnAddToRegExpTable_Click(object sender, EventArgs e)
		{
			try
			{
				string regExpText = txtRegExp.Text.ToLower();
				if (btnToggleWholeWord.Tag.ToString() == "on")
					regExpText = txtRegularExpressionWholeWord.Text;

				List<MainDataSet.RegExpRow> listRegExps = new List<MainDataSet.RegExpRow>();

				var varRegExps = from x in MainForm.ViewsManager.MainForm.datasetMain.RegExp
				                 where x.RegExp == regExpText && x.RowState != DataRowState.Deleted
				                 select x;

				MainDataSet.RegExpRow row;

				if (varRegExps.Count() == 0)
				{
					row = MainForm.ViewsManager.MainForm.datasetMain.RegExp.NewRegExpRow();
					row.RegExp = regExpText;
					try
					{
						MainForm.ViewsManager.MainForm.datasetMain.RegExp.AddRegExpRow(row);
					}
					catch
					{
					}
				}
				else
					row = varRegExps.First();

				//////////////////////////////////////////////////////////////////////////

				RegExpStatisticsProcessingResult wordcount;

				if (lvStatistics.CheckedItems.Count > lvStatistics.SelectedItems.Count)
				{
					foreach (ListViewItem lvi in lvStatistics.CheckedItems)
					{
						wordcount = (RegExpStatisticsProcessingResult) lvi.Tag;

						AddToExceptions(row, wordcount);
					}
				}
				else
				{
					foreach (ListViewItem lvi in lvStatistics.SelectedItems)
					{
						wordcount = (RegExpStatisticsProcessingResult)lvi.Tag;

						AddToExceptions(row, wordcount);
					}
				}

				RaiseDataModifiedEvent();
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSaveToDatabase_Click(object sender, EventArgs e)
		{
			//Show dialog for the name of the new table
			PopUpStatisticsNewTableName newNamePopUp = new PopUpStatisticsNewTableName(_views.MainForm.adapterDocuments.Connection);
			if (newNamePopUp.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				var newTableName = newNamePopUp.newName;

				//Create table
				try
				{
					string cmdText = "CREATE TABLE [" + newTableName + "] ([Word] CHAR(255),[Count] INTEGER, [Persentage] CHAR(10));";
					OleDbCommand cmd = new OleDbCommand(cmdText, _views.MainForm.adapterDocuments.Connection);
					cmd.ExecuteNonQuery();

					//Populate table
					foreach (ListViewItem item in lvStatistics.Items)
					{
						var word = item.SubItems[1].Text;
						var count = Int32.Parse(item.SubItems[2].Text);
						var persentage = item.SubItems[3].Text;

						OleDbCommand insertCmd = new OleDbCommand("INSERT INTO [" + newTableName + "] ([Word],[Count],[Persentage]) VALUES (?,?,?);", _views.MainForm.adapterDocuments.Connection);
						insertCmd.Parameters.Add("word", OleDbType.Char)
						         .Value = word;
						insertCmd.Parameters.Add("count", OleDbType.Integer)
						         .Value = count;
						insertCmd.Parameters.Add("persentage", OleDbType.Char)
						         .Value = persentage;
						insertCmd.ExecuteNonQuery();
					}
				}
				catch
				{
				}
			}
		}

		private void btnRegExp_Click(object sender, EventArgs e)
		{
			DialogRegExpStatistics dialog = new DialogRegExpStatistics(_views);
			if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				txtRegExp.Text = dialog.selectedRegExp;
				chbReplace.Checked = dialog.replace;
				txtReplace.Text = dialog.replaceRegExp;
			}
		}

		private void btnSaveCurrent_Click(object sender, EventArgs e)
		{
			var regExp = txtRegExp.Text;
			var replace = chbReplace.Checked;
			var replaceText = txtReplace.Text;

			//Insert to database
			string cmdText = "INSERT INTO [RegExpStatistics] ([Regexp],[IsReplace], [ReplacementText]) VALUES (?,?,?);";
			OleDbCommand cmd = new OleDbCommand(cmdText, _views.MainForm.adapterDocuments.Connection);
			cmd.Parameters.Add("RegExpStatistics", OleDbType.Char)
			   .Value = regExp;
			cmd.Parameters.Add("IsReplace", OleDbType.Boolean)
			   .Value = replace;
			cmd.Parameters.Add("ReplacementText", OleDbType.Char)
			   .Value = replaceText;
			cmd.ExecuteNonQuery();
		}

		int selectionStartPosition;
		int selectionLength;

		private void txtRegExp_Leave(object sender, EventArgs e)
		{
			//Save cursor position
			selectionStartPosition = txtRegExp.SelectionStart;
			selectionLength = txtRegExp.SelectionLength;

			var RegExpButtonFocused = CheckIfRegExpButtonFocused();
			if (!RegExpButtonFocused)
			{
				//Lost focus - Disable all buttons
				btnAddAllWords.Enabled = false;
				btnAnyCharacter.Enabled = false;
				btnAnyDigit.Enabled = false;
				btnAnyWhiteSpace.Enabled = false;
				btnEitherOr.Enabled = false;
				btnOneOrMoreTimes.Enabled = false;
				btnRange.Enabled = false;
				btnZeroOrMultipleTimes.Enabled = false;
			}
		}

		private void txtRegExp_MouseUp(object sender, MouseEventArgs e)
		{
			CheckIfTextIsSelected();
		}

		private void CheckIfTextIsSelected()
		{
			//Disable all buttons
			btnEitherOr.Enabled = false;
			btnOptionalOnceOrNone.Enabled = false;
			btnOneOrMoreTimes.Enabled = false;
			btnRange.Enabled = false;
			btnZeroOrMultipleTimes.Enabled = false;

			if (txtRegExp.SelectedText != "")
			{
				var count = txtRegExp.SelectionLength;

				if (count > 0)
				{
					//Enable Optional, One or more times, Zero or more times
					btnOptionalOnceOrNone.Enabled = true;
					btnOneOrMoreTimes.Enabled = true;
					btnZeroOrMultipleTimes.Enabled = true;
				}
				if (count == 2)
				{
					//Enable Range
					btnRange.Enabled = true;
				}
				if (count > 1)
				{
					//Enable Either or
					btnEitherOr.Enabled = true;
				}
			}
		}

		private void txtRegExp_KeyUp(object sender, KeyEventArgs e)
		{
			if (btnToggleWholeWord.Tag.ToString() == "on")
			{
				var oldText = txtRegExp.Text;
				txtRegularExpressionWholeWord.Text = "\\b\\w*" + oldText + "\\w*\\b";
			}

			CheckIfTextIsSelected();
		}

		private bool CheckIfRegExpButtonFocused()
		{
			if (btnAddAllWords.Focused)
				return true;
			if (btnAnyCharacter.Focused)
				return true;
			if (btnAnyDigit.Focused)
				return true;
			if (btnAnyWhiteSpace.Focused)
				return true;
			if (btnEitherOr.Focused)
				return true;
			if (btnOneOrMoreTimes.Focused)
				return true;
			if (btnRange.Focused)
				return true;
			if (btnZeroOrMultipleTimes.Focused)
				return true;
			return false;
		}

		private void txtRegExp_Enter(object sender, EventArgs e)
		{
			//Enable first 3
			btnAnyCharacter.Enabled = true;
			btnAnyDigit.Enabled = true;
			btnAnyWhiteSpace.Enabled = true;
		}

		private void btnWriteMyOwn_Click(object sender, EventArgs e)
		{
			var url = "http://regexlib.com/CheatSheet.aspx?AspxAutoDetectCookieSupport=1";
			System.Diagnostics.Process.Start(url);
		}

		private void btnAnyCharacter_Click(object sender, EventArgs e)
		{
			var regExp = "\\w";

			var text = txtRegExp.Text.Insert(selectionStartPosition, regExp);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + regExp.Length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}

		private void btnAnyDigit_Click(object sender, EventArgs e)
		{
			var regExp = "\\d";

			var text = txtRegExp.Text.Insert(selectionStartPosition, regExp);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + regExp.Length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}

		private void btnAnyWhiteSpace_Click(object sender, EventArgs e)
		{
			var regExp = "\\s";

			var text = txtRegExp.Text.Insert(selectionStartPosition, regExp);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + regExp.Length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}

		private void btnOptionalOnceOrNone_Click(object sender, EventArgs e)
		{
			var regExp = "?";

			var text = txtRegExp.Text.Insert(selectionStartPosition + selectionLength, regExp);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + regExp.Length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}

		private void btnOneOrMoreTimes_Click(object sender, EventArgs e)
		{
			var regExp = "+";

			var text = txtRegExp.Text.Insert(selectionStartPosition + selectionLength, regExp);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + regExp.Length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}

		private void btnZeroOrMultipleTimes_Click(object sender, EventArgs e)
		{
			var regExp = "*";

			var text = txtRegExp.Text.Insert(selectionStartPosition + selectionLength, regExp);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + regExp.Length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}

		private void btnEitherOr_Click(object sender, EventArgs e)
		{
			var subText = txtRegExp.Text.Substring(selectionStartPosition, selectionLength);
			var text = txtRegExp.Text.Remove(selectionStartPosition, selectionLength);

			subText = String.Join<char>("|", subText);
			subText = "[" + subText + "]";
			var length = subText.Length;

			text = text.Insert(selectionStartPosition, subText);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}

		private void btnRange_Click(object sender, EventArgs e)
		{
			var subText = txtRegExp.Text.Substring(selectionStartPosition, selectionLength);
			var text = txtRegExp.Text.Remove(selectionStartPosition, selectionLength);

			subText = String.Join<char>("-", subText);
			subText = "[" + subText + "]";
			var length = subText.Length;

			text = text.Insert(selectionStartPosition, subText);

			txtRegExp.Text = text;

			//Select the new added characters
			txtRegExp.Select(selectionStartPosition + length, 0);
			txtRegExp.Select();

			CheckIfTextIsSelected();
		}
	}

	#region Helper types

	public class NameComparer : IComparer
	{
		#region Operations

		public int Compare(object x, object y)
		{
			int nResult = 0;

			try
			{
				if (x is DirectoryInfo && y is DirectoryInfo)
				{
					DirectoryInfo di1 = (DirectoryInfo) x;
					DirectoryInfo di2 = (DirectoryInfo) y;

					return String.Compare(di1.Name, di2.Name, false);
				}
			}
			catch
			{
			}

			return nResult;
		}

		#endregion
	}

	public class ListViewColumnSorter : IComparer
	{
		#region Data members

		protected int _nColumn;
		protected SortOrder _order;
		protected CaseInsensitiveComparer _comparer;

		#endregion

		#region Properties

		public int SortColumn
		{
			set { _nColumn = value; }
			get { return _nColumn; }
		}

		public SortOrder Order
		{
			set { _order = value; }
			get { return _order; }
		}

		#endregion

		#region Ctors

		public ListViewColumnSorter()
		{
			_nColumn = 0;

			_order = SortOrder.None;

			_comparer = new CaseInsensitiveComparer();
		}

		#endregion

		#region Operations

		public int Compare(object x, object y)
		{
			int nResult = 0;

			try
			{
				ListViewItem lv1, lv2;

				lv1 = (ListViewItem) x;
				lv2 = (ListViewItem) y;

				if (_nColumn > 0)
				{
					float f1 = Convert.ToSingle(lv1.SubItems[_nColumn].Text);
					float f2 = Convert.ToSingle(lv2.SubItems[_nColumn].Text);

					if (f1 > f2)
						nResult = 1;
					else if (f1 < f2)
						nResult = -1;
				}
				else
					nResult = _comparer.Compare(lv1.SubItems[_nColumn].Text, lv2.SubItems[_nColumn].Text);

				if (_order == SortOrder.Ascending)
					return nResult;

				if (_order == SortOrder.Descending)
					return (-nResult);
			}
			catch
			{
			}

			return nResult;
		}

		#endregion
	}

	#endregion
}