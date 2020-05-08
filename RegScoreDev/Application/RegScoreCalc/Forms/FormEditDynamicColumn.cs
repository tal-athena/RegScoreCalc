using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEditDynamicColumn : Form
	{
		#region Properties

		public ViewsManager ViewsManager { get; set; }

		public int ID { get; set; }

		public string Title
		{
			get { return txtName.Text; }
			set { txtName.Text = value; }
		}

		public bool DisableTitleEdit { get; set; }
		public bool DisableColumnTypeSelection { get; set; }

		public DynamicColumnType DynamicColumnType
		{
			get { return (DynamicColumnType) cmbType.SelectedIndex; }
			set { cmbType.SelectedIndex = (int) value; }
		}

		#endregion

		#region Ctors

		public FormEditDynamicColumn()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormEditDynamicColumn_Load(object sender, System.EventArgs e)
		{
			if (!String.IsNullOrEmpty(txtName.Text))
				txtName.SelectionStart = txtName.TextLength;

			lblType.Visible = !this.DisableColumnTypeSelection;
			cmbType.Visible = !this.DisableColumnTypeSelection;

			txtName.Enabled = !this.DisableTitleEdit;
		}

		private void FormEditCategoryClass_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					var duplicates =
						this.ViewsManager.MainForm.datasetMain.DynamicColumns.Where(
							x => x.RowState != DataRowState.Deleted && x.ID != this.ID && x.Title == this.Title);

					if (duplicates.Any())
					{
						MessageBox.Show("Column with same name already exist", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

						e.Cancel = true;
					}
					else if (cmbType.SelectedIndex < 0)
					{
						MessageBox.Show("Please select column type", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

						e.Cancel = true;
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion
	}
}