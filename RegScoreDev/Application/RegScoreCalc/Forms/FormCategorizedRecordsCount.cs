using DocumentsServiceInterfaceLib;
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
    public partial class FormCategorizedRecordsCount : Form
    {
        #region Fields

        protected ViewsManager _views;
        protected Dictionary<int, int> _categorizedRecords;
        #endregion

        #region Ctors

        public FormCategorizedRecordsCount(ViewsManager views, object categorizedRecords)
        {
            _views = views;

            if (categorizedRecords != DBNull.Value)
            {
                _categorizedRecords = (Dictionary<int, int>)categorizedRecords;
            }
            else
            {
                _categorizedRecords = null;
            }

            InitializeComponent();

            this.BackColor = MainForm.ColorBackground;

            gridView.AutoGenerateColumns = false;
            gridView.EnableHeadersVisualStyles = false;
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

                }
            }
            catch (Exception ex)
            {
                MainForm.ShowExceptionMessage(ex);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        #endregion

        #region Implementation

        protected void FillGrid()
        {
            gridView.Rows.Clear();


            var categories = _views.MainForm.datasetMain.Categories.Rows.Cast<MainDataSet.CategoriesRow>()
                            .Select(x => new CategorizedRecordsModel
                            {
                                ID = x.ID,
                                Category = x.Category,
                                Color = !x.IsColorNull() ? (int?)x.Color : null,
                                Records = 0
                            }).ToList();
            foreach (var item in categories)
            {
                if (_categorizedRecords != null && _categorizedRecords.ContainsKey(item.ID))
                {
                    item.Records = _categorizedRecords[item.ID];
                }
                if (item.ID != 0)
                    gridView.Rows.Add(item.Category, item.Records);
            }
        }
        #endregion
    }
    public class CategorizedRecordsModel
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public int Records { get; set; }
        public int? Color { get; set; }
    }
}