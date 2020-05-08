using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEditColumnCategory : Form
	{
		#region Properties

		public ViewsManager ViewsManager { get; set; }

		public int DynamicColumnID { get; set; }
		public int ID { get; set; }

		public string Title
		{
			get { return txtName.Text; }
			set { txtName.Text = value; }
		}

		public int Number
		{
			get { return (int)txtNumber.Value; }
			set { txtNumber.Value = value; }
		}

		#endregion

		#region Ctors

		public FormEditColumnCategory()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormEditColumnCategory_Load(object sender, System.EventArgs e)
		{
			if (!String.IsNullOrEmpty(txtName.Text))
			{
				txtName.SelectionStart = 0;
				txtName.SelectionLength = txtName.TextLength;
			}
		}

		private void FormEditColumnCategory_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					var duplicates =
						this.ViewsManager.MainForm.datasetMain.DynamicColumnCategories.Where(
							x =>
								x.RowState != DataRowState.Deleted && x.DynamicColumnID == this.DynamicColumnID && x.ID != this.ID
								&& (x.Title == this.Title || x.Number == this.Number));

					if (duplicates.Any())
					{
						MessageBox.Show("Category with same property already exist", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

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