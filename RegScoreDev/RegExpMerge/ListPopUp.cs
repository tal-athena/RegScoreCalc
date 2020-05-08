using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegExpMerge
{
    public partial class ListPopUp : Form
    {
        MainForm _mainForm;
        RegExpMerge.Data.DataSetMain.RegExpRow currentRow;
        int currentPosition;
        public ListPopUp(RegExpMerge.Data.DataSetMain.RegExpRow row, int position, MainForm mainForm)
        {
            InitializeComponent();
            _mainForm = mainForm;
            currentRow = row;
            currentPosition = position;

            string strValues = "";
            if (row[position] != null )
            {
                strValues = row[position].ToString();
            }
            FillGrid(strValues);
        }

        #region Implementation

        protected void FillGrid(string strValues)
        {
            gridCriteria.Rows.Clear();

            if (!String.IsNullOrEmpty(strValues))
            {
                string[] arrValues = strValues.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string strValue in arrValues)
                {
                    gridCriteria.Rows.Add(strValue);
                }
            }
        }

        protected void Save()
        {
            string strResult = "";

            string strValue;
            foreach (DataGridViewRow row in gridCriteria.Rows)
            {
                strValue = (string)row.Cells[0].Value;
                if (!String.IsNullOrEmpty(strValue))
                {
                    if (!String.IsNullOrEmpty(strResult))
                        strResult += "|";

                    strResult += strValue;
                }
            }
            //Save result
            currentRow[currentPosition] = strResult;

            //Re calculate
            _mainForm.ProcessData();
        }

        #endregion

        #region "Events"

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //Just close without saving
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //Save and close
            Save();

            this.Close();
        }

        #endregion
    }
}
