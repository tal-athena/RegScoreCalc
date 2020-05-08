using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEditColumnDateTime : Form
	{
		#region Fields

		protected DateTimeColumnProperties _props;

		#endregion

		#region Properties

		public DateTime Value
		{
			get { return dtpValue.Value; }
			set { dtpValue.Value = value; }
		}

		#endregion

		#region Ctors

		public FormEditColumnDateTime(DateTimeColumnProperties props)
		{
			InitializeComponent();

			_props = props;

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormEditColumnDateTime_Load(object sender, EventArgs e)
		{
			try
			{
				dtpValue.CustomFormat = _props.GetFormatString();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormEditColumnDateTime_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (dtpValue.Value == DateTime.MinValue || dtpValue.Value == DateTime.MaxValue)
					{
						MessageBox.Show("Incorrect value", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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