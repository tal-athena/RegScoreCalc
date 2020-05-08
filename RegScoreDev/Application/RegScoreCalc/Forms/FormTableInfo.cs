using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormTableInfo : Form
	{
		protected ArrayList _arrTableList;

		public FormTableInfo()
		{
			InitializeComponent();

			//_arrTableList = arrTableList;
		}

		public string TableName
		{
			get { return cmbTables.Text; }
			set { cmbTables.Text = value; }
		}

		private void FormTableInfo_Load(object sender, EventArgs e)
		{
			/*foreach (string strTableName in _arrTableList)
			{
				cmbTables.Items.Add(strTableName);
			}*/
		}

		private void FormTableInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.DialogResult == DialogResult.OK)
			{
				string strTableName = cmbTables.Text;

				if (String.IsNullOrEmpty(strTableName) || strTableName.IndexOf(" ") != -1)
				{
					e.Cancel = true;
					MessageBox.Show("Invalid table name", "Score Calculator", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
			}
		}
	}
}
