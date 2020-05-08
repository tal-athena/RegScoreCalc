using Helpers;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormScriptHelp : Form
	{
        #region Fields

        protected List<Tuple<string, string, string>> _helpList;

		#endregion

		#region Ctors

		public FormScriptHelp(List<Tuple<string, string, string>> helpList)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

            ///////////////////////////////////////////////////////////////////////////////

            _helpList = helpList;

            foreach (var help in _helpList)
                listFunctions.Items.Add(help.Item1);
		}

        #endregion

        #region events


        private void FormScriptHelp_Closing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK && this.txtExample.Text == "")
            {
                MessageBox.Show("Please select a function", "Help Script");

                e.Cancel = true;
            }
        }


        private void lvDynamicColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listFunctions.SelectedItems.Count == 1)
            {
                txtDescription.Text = _helpList[listFunctions.SelectedItems[0].Index].Item2;
                txtExample.Text = _helpList[listFunctions.SelectedItems[0].Index].Item3;
            }
        }


        private void onUse(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
        #endregion

        #region implements
        public string GetExampleCode()
        {
            return txtExample.Text;
        }
        #endregion
    }

}