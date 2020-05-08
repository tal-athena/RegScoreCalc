using System;
using System.Data;
using System.Windows.Forms;

using RegExpLib.Core;

using RegScoreCalc.Code;

namespace RegScoreCalc
{
	public partial class MatchEditingControl : UserControl, IDataGridViewEditingControl
	{
		#region Fields

		protected DataGridView grid;
		protected bool valueChanged = false;
		protected int rowIndex;

		#endregion

		#region Ctors

		public MatchEditingControl()
		{
			InitializeComponent();
		}

		#endregion

		#region Events

		private void btnEdit_Click(object sender, EventArgs e)
		{
			if (grid.SelectedRows.Count == 1)
			{
				var rowView = (DataRowView)grid.SelectedRows[0].DataBoundItem;

				var regExp = RegExpFactory.Create_RegExp(rowView, false);
				if (regExp != null)
				{
					FormEditPrefixSuffix formEditPrefixSuffix = new FormEditPrefixSuffix(regExp);
					if (formEditPrefixSuffix.ShowDialog() == DialogResult.OK)
						regExp.SafeSave(rowView, true);
				}
			}
		}

		private void MatchEditingControl_Load(object sender, EventArgs e)
		{
			try
			{
				btnEdit.Left = (this.ClientSize.Width / 2) - (btnEdit.Width / 2);
				btnEdit.Top = (this.ClientSize.Height / 2) - (btnEdit.Height / 2);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		#endregion

		#region Operations

		public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
		{
			return EditingControlFormattedValue;
		}

		public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
		{
			
		}

		public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
		{
			return false;
		}

		public void PrepareEditingControlForEdit(bool selectAll)
		{

		}

		#endregion

		#region Implementation: IDataGridViewEditingControl

		public DataGridView EditingControlDataGridView
		{
			get
			{
				return grid;
			}
			set
			{
				grid = value;
			}
		}

		public object EditingControlFormattedValue
		{
			get
			{
				return "";
			}
			set
			{
				
			}
		}

		public int EditingControlRowIndex
		{
			get
			{
				return rowIndex;
			}
			set
			{
				rowIndex = value;
			}
		}

		public bool EditingControlValueChanged
		{
			get
			{
				return false;
			}
			set
			{
				
			}
		}

		public Cursor EditingPanelCursor
		{
			get { return Cursors.Arrow; }
		}

		public bool RepositionEditingControlOnValueChange
		{
			get { return true; }
		}

		#endregion
	}
}
