using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Data.OleDb;

using RegExpLib.Core;

using RegScoreCalc.Code;
using RegScoreCalc.Forms;

namespace RegScoreCalc
{
	public partial class PaneAdvRegExp : Pane
	{
		#region Fields

		private DataGridViewTextBoxColumn regExpDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn scoreDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn arithmeticFactorDataGridViewTextBoxColumn;
		private DataGridViewCustomColumn regExpColorDataGridViewTextBoxColumn;
		private DataGridViewTextBoxColumn colTotalMatches;
		private DataGridViewTextBoxColumn colTotalDocuments;
		private MatchColumn matchDataGridViewTextBoxColumn;

		protected RibbonComboBox _cmbCalcScores;

		protected FormRegularExpressionEditor _editor;

		protected int _nRegExpRow;
		protected int _nMatchIndex;

		protected int _nPrevPosition;

		#endregion

		#region Ctors

		public PaneAdvRegExp()
		{
			InitializeComponent();

			menuitemEditRegExp.Click += new EventHandler(menuitemEditRegExp_Clicked);
			menuitemDeleteRows.Click += new EventHandler(menuitemDeleteRows_Clicked);
		}

		#endregion

		#region Events

		protected void OnCalcScores_Click(object sender, EventArgs e)
		{
			CalcScores();
		}

		protected void regExpDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex < 0)
				return;

			try
			{
				if (e.ColumnIndex == regExpColorDataGridViewTextBoxColumn.Index && e.RowIndex >= 0) // index of column with color
				{
					var rowView = (DataRowView) regExpDataGridView.Rows[e.RowIndex].DataBoundItem;
					var row = (MainDataSet.RegExpRow) rowView.Row;

					ArrayList arrColors = GetRegExpColors(row.ID, _views, false);

					FormColors formColors = new FormColors();
					formColors.Colors = arrColors;

					if (formColors.ShowDialog() == DialogResult.OK)
					{
						SetRegExpColors(row.ID, _views, formColors.Colors);

						RaiseDataModifiedEvent();
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void regExpDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			RaiseDataModifiedEvent();
		}

		private void regExpDataGridView_MouseClick(object sender, MouseEventArgs e)
		{
			if (menuOperations.Visible)
				menuOperations.Hide();

			if (e.Button == MouseButtons.Right && !regExpDataGridView.IsCurrentCellInEditMode)
			{
				menuitemEditRegExp.Visible = false;
				menuitemDeleteRows.Visible = false;

				bool bShow = false;

				if (regExpDataGridView.CurrentCell != null)
				{
					if (IsStringColumn(regExpDataGridView.CurrentCell.ColumnIndex))
					{
						menuitemEditRegExp.Visible = true;
						bShow = true;
					}
				}

				if (regExpDataGridView.SelectedCells.Count > 0)
				{
					menuitemDeleteRows.Visible = true;
					bShow = true;
				}

				if (bShow)
					menuOperations.Show(Cursor.Position);
			}
		}

		protected void menuitemDeleteRows_Clicked(object sender, EventArgs e)
		{
			DeleteRows();
		}

		protected void menuitemEditRegExp_Clicked(object sender, EventArgs e)
		{
			EditRegExp();
		}

		private void menuitemCopyRegExp_Click(object sender, EventArgs e)
		{
			try
			{
				if (regExpDataGridView.CurrentCell != null)
				{
					DataGridViewCell cell = regExpDataGridView.CurrentCell;
					if (cell.ColumnIndex == regExpDataGridViewTextBoxColumn.Index)
					{
						regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

						if (cell.Value != DBNull.Value)
						{
							DataRowView rowview = (DataRowView)cell.OwningRow.DataBoundItem;

							var regExp = RegExpFactory.Create_RegExp(rowview, true);

							Clipboard.SetText(regExp.BuiltExpression);
						}
					}
				}
			}
			catch
			{
			}
		}

		private void regExpDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Left)
				{

					if (Control.ModifierKeys == Keys.Control)
						EditRegExp();
					else
						regExpDataGridView.BeginEdit(true);
				}
			}
			catch { }
		}

		private void regExpDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			try
			{
				DataGridViewCell cell = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
				if (cell.Value is string)
					cell.ToolTipText = (string)cell.Value;
			}
			catch
			{
			}
		}

		private void regExpDataGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
			RaiseDataModifiedEvent();
		}

		private void menuitemCopy_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = (string)cell.Value;

					Clipboard.SetText(strValue);
				}
			}
			catch
			{
			}
		}

		private void menuitemCut_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = (string)cell.Value;

					Clipboard.SetText(strValue);

					cell.Value = "";
				}
			}
			catch
			{
			}
		}

		private void menuitemPaste_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = Clipboard.GetText();

					cell.Value = strValue;
				}
			}
			catch
			{
			}
		}

		private void menuitemWordBoundaries_Click(object sender, EventArgs e)
		{
			try
			{
				DataGridViewCell cell = (DataGridViewCell)menuRegExp.Tag;
				if (cell.Value != null)
				{
					string strValue = (string)cell.Value;

					cell.Value = @"\b" + strValue + @"\b";
				}
			}
			catch
			{
			}
		}

		private void regExpDataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				if (e.Button == MouseButtons.Right)
				{
					if (e.ColumnIndex == regExpDataGridViewTextBoxColumn.Index)
					{
						if (menuOperations.Visible)
							menuOperations.Hide();

						if (menuRegExp.Visible)
							menuRegExp.Hide();

						DataGridViewCell cell = regExpDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
						regExpDataGridView.CurrentCell = cell;

						menuRegExp.Tag = cell;

						menuRegExp.Show(Cursor.Position);
					}
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void regExpDataGridView_CellLeave(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				BindingSource source = (BindingSource)regExpDataGridView.DataSource;
				source.EndEdit();

				_nPrevPosition = -1;
			}
			catch
			{
			}
		}

		private void regExpDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			regExpDataGridView.AutoGenerateColumns = false;
			regExpDataGridView.DataSource = _views.MainForm.sourceRegExp;
			regExpDataGridView.Tag = _views;

			splitter.BorderStyle = BorderStyle.Fixed3D;

			InitializeGridColumns();

			InitializeRegExpEditor(true);
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
			RibbonPanel panel = new RibbonPanel("Regular Expressions");
			tab.Panels.Add(panel);

			RibbonButton btnCalcScores = new RibbonButton("Calculate Scores");
			panel.Items.Add(btnCalcScores);
			btnCalcScores.Image = Properties.Resources.CalcScores;
			btnCalcScores.SmallImage = Properties.Resources.CalcScores;
			btnCalcScores.Click += new EventHandler(OnCalcScores_Click);
			btnCalcScores.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			//////////////////////////////////////////////////////////////////////////

			CreateAutoCalcScoresComboBox();

			//////////////////////////////////////////////////////////////////////////

			panel.Items.Add(new RibbonSeparator());
			panel.Items.Add(_cmbCalcScores);
		}

		protected void CreateAutoCalcScoresComboBox()
		{
			_cmbCalcScores = new RibbonComboBox();
			_cmbCalcScores.AllowTextEdit = false;
			_cmbCalcScores.Text = "Auto-calc Scores: ";

			//////////////////////////////////////////////////////////////////////////

			RibbonButton btn;

			btn = new RibbonButton("never");
			btn.Tag = 0;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			btn = new RibbonButton("1 change");
			btn.Tag = 1;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			btn = new RibbonButton("5 changes");
			btn.Tag = 5;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			btn = new RibbonButton("10 changes");
			btn.Tag = 10;
			btn.Click += btnAutoCalc_Click;
			_cmbCalcScores.DropDownItems.Add(btn);

			//////////////////////////////////////////////////////////////////////////

			switch (_views.AutoCalc)
			{
				case 0:
					_cmbCalcScores.TextBoxText = "never";
					break;

				case 1:
					_cmbCalcScores.TextBoxText = "1 change";
					break;

				case 5:
					_cmbCalcScores.TextBoxText = "5 changes";
					break;

				case 10:
					_cmbCalcScores.TextBoxText = "10 changes";
					break;
			}
		}

		private void btnAutoCalc_Click(object sender, EventArgs e)
		{
			if (sender is RibbonButton)
			{
				RibbonButton btn = (RibbonButton)sender;
				if (btn.Tag is int)
				{
					int nTimes = (int)btn.Tag;
					_views.AutoCalc = nTimes;

					_cmbCalcScores.TextBoxText = btn.Text;
				}
			}
		}

		#endregion

		#region Implementation

		protected void InitializeGridColumns()
		{
			this.regExpDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.scoreDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.arithmeticFactorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.matchDataGridViewTextBoxColumn = new MatchColumn();
			this.regExpColorDataGridViewTextBoxColumn = new DataGridViewCustomColumn();
			this.colTotalMatches = new DataGridViewTextBoxColumn();
			this.colTotalDocuments = new DataGridViewTextBoxColumn();

			// 
			// regExpDataGridViewTextBoxColumn
			// 
			this.regExpDataGridViewTextBoxColumn.DataPropertyName = "RegExp";
			this.regExpDataGridViewTextBoxColumn.HeaderText = "RegExp";
			this.regExpDataGridViewTextBoxColumn.Name = "regExpDataGridViewTextBoxColumn";
			// 
			// scoreDataGridViewTextBoxColumn
			// 
			this.scoreDataGridViewTextBoxColumn.DataPropertyName = "score";
			this.scoreDataGridViewTextBoxColumn.HeaderText = "score";
			this.scoreDataGridViewTextBoxColumn.Name = "scoreDataGridViewTextBoxColumn";
			// 
			// arithmeticFactorDataGridViewTextBoxColumn
			// 
			this.arithmeticFactorDataGridViewTextBoxColumn.DataPropertyName = "Arithmetic factor";
			this.arithmeticFactorDataGridViewTextBoxColumn.HeaderText = "Arithmetic factor";
			this.arithmeticFactorDataGridViewTextBoxColumn.Name = "arithmeticFactorDataGridViewTextBoxColumn";

			// matchDataGridViewTextBoxColumn
			// 
			this.matchDataGridViewTextBoxColumn.HeaderText = "Match";
			this.matchDataGridViewTextBoxColumn.Name = "matchDataGridViewTextBoxColumn";
			this.matchDataGridViewTextBoxColumn.Width = 40;
			this.matchDataGridViewTextBoxColumn.MinimumWidth = 40;
			this.matchDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			// 
			// regExpColorDataGridViewTextBoxColumn
			// 
			this.regExpColorDataGridViewTextBoxColumn.HeaderText = "RegExpColor";
			this.regExpColorDataGridViewTextBoxColumn.Name = "regExpColorDataGridViewTextBoxColumn";
			this.regExpColorDataGridViewTextBoxColumn.ReadOnly = true;

			// 
			// colTotalMatches
			// 
			this.colTotalMatches.DataPropertyName = "TotalMatches";
			this.colTotalMatches.HeaderText = "#";
			this.colTotalMatches.Name = "colTotalMatches";
			this.colTotalMatches.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			// 
			// colTotalDocuments
			// 
			this.colTotalDocuments.DataPropertyName = "TotalDocuments";
			this.colTotalDocuments.HeaderText = "docs";
			this.colTotalDocuments.Name = "colTotalDocuments";
			this.colTotalDocuments.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			//////////////////////////////////////////////////////////////////////////

			this.regExpDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
			                                         {
				                                         this.regExpDataGridViewTextBoxColumn,
				                                         this.colTotalMatches,
				                                         this.colTotalDocuments,
				                                         this.scoreDataGridViewTextBoxColumn,
				                                         this.arithmeticFactorDataGridViewTextBoxColumn,
				                                         this.matchDataGridViewTextBoxColumn,
				                                         this.regExpColorDataGridViewTextBoxColumn
			                                         });
		}

		protected void InitializeRegExpEditor(bool enableNavigation)
		{
			_editor = new FormRegularExpressionEditor(_views, _views.MainForm.sourceRegExp, x => RegExpFactory.Create_RegExp(x, false))
			{
				Dock = DockStyle.Fill,
				TopLevel = false,
				Parent = splitter.Panel2
			};

			_editor.Modified += (sender, args) => RaiseDataModifiedEvent();

			///////////////////////////////////////////////////////////////////////////////

			var panel = new Panel
			{
				Dock = DockStyle.Fill,
				BorderStyle = BorderStyle.None,
				Visible = true
			};

			panel.Controls.Add(_editor);

			splitter.Panel2.Controls.Add(panel);

			_editor.Show();
		}

		public void CalcScores()
		{
			regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

			_views.MainForm.sourceDocuments.RaiseListChangedEvents = false;
			_views.MainForm.sourceRegExp.RaiseListChangedEvents = false;

			FormGenericProgress formGenericProgress = new FormGenericProgress("Calculating scores, please wait...", new LengthyOperation(DoCalcScores), null, true);
			formGenericProgress.ShowDialog();

			if (formGenericProgress.Result)
				MessageBox.Show("Calculation finished successfully", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

			_views.MainForm.sourceRegExp.RaiseListChangedEvents = true;
			_views.MainForm.sourceDocuments.RaiseListChangedEvents = true;

			_views.MainForm.sourceDocuments.ResetBindings(false);
		}

		protected void DeleteRows()
		{
			try
			{
				foreach (DataGridViewCell cell in regExpDataGridView.SelectedCells)
				{
					try
					{
						regExpDataGridView.Rows.Remove(cell.OwningRow);
					}
					catch
					{
					}
				}
			}
			catch
			{
			}

			RaiseDataModifiedEvent();
		}

		protected void EditRegExp()
		{
			try
			{
				if (regExpDataGridView.CurrentCell != null)
				{
					DataGridViewCell cell = regExpDataGridView.CurrentCell;
					if (IsStringColumn(cell.ColumnIndex))
					{
						regExpDataGridView.EndEdit(DataGridViewDataErrorContexts.Commit);

						FormEditRegExp formEditRegExp = new FormEditRegExp();

						if (cell.Value == DBNull.Value)
							formEditRegExp.RegExpValue = "";
						else
							formEditRegExp.RegExpValue = (string)cell.Value;

						if (formEditRegExp.ShowDialog() == DialogResult.OK)
						{
							cell.Value = formEditRegExp.RegExpValue;
							regExpDataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
							regExpDataGridView.Refresh();

							RaiseDataModifiedEvent();
						}
					}
				}
			}
			catch
			{
			}
		}

		protected bool IsStringColumn(int nColumn)
		{
			if (nColumn == 0 || nColumn == 3 || nColumn == 5 || nColumn == 6 || nColumn == 7 || nColumn == 8 || nColumn == 9)
				return true;

			return false;
		}

		public static ArrayList GetRegExpColors(int regExpID, object objViews, bool bOnlySelected)
		{
			ArrayList arrColors = new ArrayList();

			try
			{
				if (objViews is ViewsManager)
				{
					ViewsManager views = (ViewsManager)objViews;

					ColorInfo ci;
					bool bSelected;

					foreach (MainDataSet.ColorCodesRow rowColorCodes in views.MainForm.datasetMain.ColorCodes)
					{
						bSelected = false;

						foreach (MainDataSet.RelationsRow rowRelations in views.MainForm.datasetMain._Relations)
						{
							if (rowRelations.RegExpID == regExpID && rowColorCodes.ID == rowRelations.ColorCodeID)
							{
								bSelected = true;
								break;
							}
						}

						ci = new ColorInfo(rowColorCodes.Description, rowColorCodes.ID, rowColorCodes.Color, bSelected);

						if (bOnlySelected)
						{
							if (bSelected)
								arrColors.Add(ci);
						}
						else
							arrColors.Add(ci);
					}
				}
			}
			catch
			{
			}

			return arrColors;
		}

		public static bool SetRegExpColors(int regExpID, object objViews, ArrayList arrColors)
		{
			bool bResult = false;

			try
			{
				if (arrColors != null)
				{
					if (objViews is ViewsManager)
					{
						ViewsManager views = (ViewsManager)objViews;

						MainDataSet.RelationsRow rowRelations;
						for (int i = 0; i < views.MainForm.datasetMain._Relations.Count; i++)
						{
							rowRelations = (MainDataSet.RelationsRow)views.MainForm.datasetMain._Relations[i];
							if (rowRelations.RegExpID == regExpID)
							{
								views.MainForm.adapterRelations.Delete(rowRelations.ID, rowRelations.RegExpID, rowRelations.ColorCodeID);
								views.MainForm.adapterRelations.Fill(views.MainForm.datasetMain._Relations);

								i--;
							}
						}

						int nCount = views.MainForm.adapterRelations.Fill(views.MainForm.datasetMain._Relations);

						foreach (ColorInfo ci in arrColors)
						{
							rowRelations = views.MainForm.datasetMain._Relations.NewRelationsRow();
							rowRelations.RegExpID = regExpID;
							rowRelations.ColorCodeID = ci.ID;

							views.MainForm.datasetMain._Relations.Rows.Add(rowRelations);
						}

						views.MainForm.adapterRelations.Update(views.MainForm.datasetMain._Relations);
					}
				}
			}
			catch (System.Exception e)
			{
				e.ToString();
			}

			return bResult;
		}

		#endregion

		#region Implementation: matches

		public void RefreshHighlights()
		{
			try
			{
				_views.InvokeRefreshHighlights();
			}
			catch
			{
			}
		}

		#endregion

		#region Implementation: score calcumation

		protected bool DoCalcScores(BackgroundWorker worker, object objArgument)
		{
			using (var processor = new ExternalRegExpToolWrapper(worker))
			{
				return processor.RegExp_CalcScores(_views);
			}
		}

		#endregion
	}

	public class DataGridViewCustomCell : DataGridViewTextBoxCell
	{
		#region Overrides

		protected override void Paint(Graphics g, Rectangle rectClip, Rectangle rectCell, int nRowIndex, DataGridViewElementStates state, object objValue, object objFormattedValue, string strErrorText, DataGridViewCellStyle styleCell, DataGridViewAdvancedBorderStyle styleBorder,
			DataGridViewPaintParts paintParts)
		{
			try
			{
				g.FillRectangle(Brushes.White, rectCell);

				Pen pen = new Pen(this.DataGridView.GridColor, 1);
				g.DrawLine(pen, new Point(rectCell.Right - 1, rectCell.Top), new Point(rectCell.Right - 1, rectCell.Bottom - 1));
				g.DrawLine(pen, new Point(rectCell.Left, rectCell.Bottom - 1), new Point(rectCell.Right, rectCell.Bottom - 1));

				if (this.Value is ArrayList)
				{
					ArrayList arrColors = (ArrayList)this.Value;
					if (arrColors.Count > 0)
					{
						int nItemWidth = rectCell.Width / arrColors.Count;
						Rectangle rcItem = rectCell;
						rcItem.Width = nItemWidth;
						rcItem.Height -= 1;

						foreach (ColorInfo ci in arrColors)
						{
							g.FillRectangle(new SolidBrush(Color.FromArgb(ci.RGB)), rcItem);

							rcItem.Offset(nItemWidth, 0);
							if (rcItem.Right > rectCell.Right - 1)
								rcItem.Width -= rcItem.Right - (rectCell.Right - 2);
						}
					}
				}
			}
			catch
			{
			}
		}

		#endregion
	}

	public class DataGridViewCustomColumn : DataGridViewColumn
	{
		#region Ctors

		public DataGridViewCustomColumn()
		{
			this.CellTemplate = new DataGridViewCustomCell();
		}

		#endregion
	}
}