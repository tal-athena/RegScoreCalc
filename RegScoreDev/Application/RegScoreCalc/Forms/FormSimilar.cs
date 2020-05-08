using DocumentsServiceInterfaceLib;
using RegScoreCalc.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc
{
    public partial class FormSimilar : Form
    {
        #region Fields
        protected List<Tuple<string, string>> _words;
        FormRegularExpressionEditor _parent;

        #endregion

        #region Ctors

        public FormSimilar(FormRegularExpressionEditor editor,  List<Tuple<string, string>> rows)
        {
            InitializeComponent();

            _parent = editor;

            _words = rows;

            this.BackColor = MainForm.ColorBackground;

            gridColumns.AutoGenerateColumns = false;
            gridColumns.EnableHeadersVisualStyles = false;

            colWord.HeaderCell.Style.BackColor = MainForm.ColorBackground;
            colDistance.HeaderCell.Style.BackColor = MainForm.ColorBackground;
        }

        #endregion

        #region Events

        private void FormColumns_Load(object sender, EventArgs e)
        {
            try
            {
                FillGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormColumns_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.DialogResult == DialogResult.OK)
                {
                    if (this.gridColumns.SelectedRows.Count != 1)
                        return;
                }
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var similar = GetWord();
            if (string.IsNullOrWhiteSpace(similar))
            {
                MessageBox.Show("Please select similar word");
                return;
            }
            if (_parent.IsColRegExp)
            {
                _parent.AddNewColRegExp(similar);
            } else
            {
                _parent.AddNewRegExp(similar);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GridColumns_SelectionChanged(object sender, System.EventArgs e)
        {
            if (this.gridColumns.SelectedRows.Count == 1)
            {
                this.btnOK.Enabled = true;
            } else
            {
                this.btnOK.Enabled = false;
            }
        }

        #endregion

        #region Implementation

        protected void FillGrid()
        {
            gridColumns.Rows.Clear();

            foreach (var row in _words)
            {
                var index = gridColumns.Rows.Add(row.Item1, row.Item2);
            }
        }

        public string GetWord()
        {
            if (gridColumns.SelectedRows.Count == 1)
                return gridColumns.SelectedRows[0].Cells[0].Value.ToString();
            return "";
        }

        #endregion
    }
}