using RegScoreCalc.Data;

using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace RegScoreCalc
{
	public partial class PaneDocumentsBilling : Pane
	{
		#region Fields

		private int _currentMouseOverRow;
		private bool _isCtrlDown;

		#endregion

		#region Ctors

		public PaneDocumentsBilling()
		{
			InitializeComponent();
		}

		public string Filter { get; set; }

		#endregion

		#region Events

		private void documentsDataGridView_DataSourceChanged(object sender, EventArgs e)
		{
			UpdateGridColor();
		}

		private void documentsDataGridView_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				ContextMenu m = new ContextMenu();

				_currentMouseOverRow = documentsDataGridView.HitTest(e.X, e.Y)
															.RowIndex;

				if (_currentMouseOverRow >= 0)
				{
					MenuItem selectCategory = new MenuItem("Select Category");
					selectCategory.Click += selectCategory_Click;
					m.MenuItems.Add(selectCategory);
				}

				m.Show(documentsDataGridView, new Point(e.X, e.Y));
			}
		}

		protected void documentsDataGridView_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateCtrlDownState();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void documentsDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex == this.columnNo.Index)
					e.Value = e.RowIndex + 1;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.RowIndex >= 0 && e.ColumnIndex == columnCategory.Index)
				{
					var row = documentsDataGridView.Rows[e.RowIndex];

					BeginEditStaticCategoryColumn(row, false);
				}
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void documentsDataGridView_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.ControlKey)
				{
					if (_isCtrlDown)
						return;

					var currentCell = documentsDataGridView.CurrentCell;
					if (currentCell != null)
					{
						if (currentCell.ColumnIndex == columnCategory.Index)
						{
							BeginEditStaticCategoryColumn(currentCell.OwningRow, true);

							UpdateCtrlDownState();

							e.Handled = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void documentsDataGridView_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode == Keys.ControlKey)
					_isCtrlDown = false;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void addCategory_Click(object sender, EventArgs e)
		{
			var addCategoryForm = new AddCategory(_views);
			if (addCategoryForm.ShowDialog() == DialogResult.OK)
				RaiseDataModifiedEvent();
		}

		private void selectCategory_Click(object sender, EventArgs e)
		{
			try
			{
				if (documentsDataGridView.SelectedRows.Count > 0)
					BeginEditStaticCategoryColumn(documentsDataGridView.Rows[_currentMouseOverRow], false);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public DataGridView GetDocumentsGrid()
		{
			return documentsDataGridView;
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			base.InitPane(views, ownerView, panel, tab);

			documentsDataGridView.AutoGenerateColumns = false;
			documentsDataGridView.DataSource = _views.MainForm.sourceDocumentsBilling;

			this.columnColor.DataPropertyName = "Color";
			this.columnColor.HeaderText = "";
			this.columnColor.Name = "columnColor";
			this.columnColor.ReadOnly = true;
			this.columnColor.Width = 10;

			this.columnReason.DataPropertyName = "Reason";
			this.columnReason.HeaderText = "Matching";
			this.columnReason.Name = "columnReason";
			this.columnReason.ReadOnly = true;
			this.columnReason.Width = 110;

			this.columnID.DataPropertyName = "ED_ENC_NUM";
			this.columnID.HeaderText = "ED_ENC_NUM";
			this.columnID.Name = "dataGridViewTextBoxColumn7";
			this.columnID.ReadOnly = true;
			this.columnID.Width = 110;

			this.columnCreateDate.DataPropertyName = "DOCUMENT_CREATE_DT";
			this.columnCreateDate.HeaderText = "Create Date";
			this.columnCreateDate.MaxInputLength = 100;
			this.columnCreateDate.Name = "columnCreateDate";
			this.columnCreateDate.ReadOnly = true;
			this.columnCreateDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnCreateDate.Width = 110;

			this.columnSex.DataPropertyName = "SEX";
			this.columnSex.HeaderText = "SEX";
			this.columnSex.MaxInputLength = 5;
			this.columnSex.Name = "columnSex";
			this.columnSex.ReadOnly = true;
			this.columnSex.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnSex.Width = 40;

			this.columnDob.DataPropertyName = "DOB";
			this.columnDob.HeaderText = "DOB";
			this.columnDob.MaxInputLength = 50;
			this.columnDob.Name = "columnDob";
			this.columnDob.ReadOnly = true;
			this.columnDob.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnDob.Width = 100;

			this.columnAge.DataPropertyName = "AGE";
			this.columnAge.HeaderText = "AGE";
			this.columnAge.MaxInputLength = 10;
			this.columnAge.Name = "columnAge";
			this.columnAge.ReadOnly = true;
			this.columnAge.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnAge.Width = 80;

			this.columnCategory.DataPropertyName = "Category";
			this.columnCategory.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
			this.columnCategory.DataSource = _views.MainForm.sourceCategoriesBilling;
			this.columnCategory.DisplayMember = "Category";
			this.columnCategory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.columnCategory.HeaderText = "Category";
			this.columnCategory.MaxDropDownItems = 18;
			this.columnCategory.Name = "Category";
			this.columnCategory.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.columnCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.columnCategory.ValueMember = "ID";
			this.columnCategory.Width = 130;

			this.documentsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
														{
															this.columnNo,
															this.columnColor,
															this.columnReason,
															this.columnID,
															this.columnCategory,
															this.columnCreateDate,
															this.columnSex,
															this.columnDob,
															this.columnAge
														});

			this.columnColor.Visible = false;
			this.columnReason.Visible = false;

			foreach (DataGridViewColumn column in this.documentsDataGridView.Columns)
			{
				column.HeaderCell.Style.Font = new Font("Arial", 11);
				column.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10);
			}

			documentsDataGridView.DataSourceChanged += documentsDataGridView_DataSourceChanged;
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
		}

		public override void UpdatePane()
		{
			base.UpdatePane();

			documentsDataGridView.Refresh();
		}

		#endregion

		#region Implementation

		protected void BeginEditStaticCategoryColumn(DataGridViewRow row, bool triggeredByCtrl)
		{
			var cell = row.Cells[columnCategory.Index];

			var formSelectCategory = new FormSelectCategory(_views, FormSelectCategory.DisplayMode.Billing, cell, triggeredByCtrl, this.Filter);

			var rowView = (DataRowView) row.DataBoundItem;
			var rowDocuments = (BillingDataSet.DocumentsRow) rowView.Row;
			if (!rowDocuments.IsCategoryNull())
				formSelectCategory.CategoryID = rowDocuments.Category;

			//////////////////////////////////////////////////////////////////////////

			if (formSelectCategory.ShowDialog() == DialogResult.OK)
			{
				if (formSelectCategory.CategoryID != -1)
					rowDocuments.Category = formSelectCategory.CategoryID;
				else
					rowDocuments.SetCategoryNull();

				UpdateDocumentsRow(rowDocuments);
			}
		}

		protected void UpdateCtrlDownState()
		{
			try
			{
				if (!documentsDataGridView.InvokeRequired)
					_isCtrlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
			}
			catch { }
		}

		protected void UpdateGridColor()
		{
			try
			{
				foreach (DataGridViewRow item in documentsDataGridView.Rows)
				{
					SetRowCollor(item);
				}
			}
			catch
			{
			}
		}

		protected void SetRowCollor(DataGridViewRow item)
		{
			if (item.Cells[1]
					.Value as string == "RED")
			{
				item.Cells[1]
					.Style.BackColor = Color.Red;
				item.Cells[1]
					.Style.ForeColor = Color.Red;
				item.Cells[1]
					.Style.SelectionBackColor = Color.Red;
				item.Cells[1]
					.Style.SelectionForeColor = Color.Red;
			}
			else if (item.Cells[1]
						 .Value as string == "GREEN")
			{
				item.Cells[1]
					.Style.BackColor = Color.Green;
				item.Cells[1]
					.Style.ForeColor = Color.Green;
				item.Cells[1]
					.Style.SelectionBackColor = Color.Green;
				item.Cells[1]
					.Style.SelectionForeColor = Color.Green;
			}
		}

		protected void UpdateDocumentsRow(BillingDataSet.DocumentsRow rowDocument)
		{
            
			OleDbConnection conn = _views.MainForm.adapterDocumentsBilling.Connection;
			if (conn.State != ConnectionState.Open)
				conn.Open();

			OleDbCommand cmd = conn.CreateCommand();
			cmd.CommandText = "UPDATE Documents SET Category = @Category WHERE ED_ENC_NUM = @ED_ENC_NUM";

			if (!rowDocument.IsCategoryNull())
				cmd.Parameters.Add(new OleDbParameter("@Category", rowDocument.Category));
			else
				cmd.Parameters.Add(new OleDbParameter("@Category", DBNull.Value));

			cmd.Parameters.Add(new OleDbParameter("@ED_ENC_NUM", rowDocument.ED_ENC_NUM));

			cmd.ExecuteNonQuery();            
            
            rowDocument.AcceptChanges();

			documentsDataGridView.Refresh();
		}

		#endregion
	}
}