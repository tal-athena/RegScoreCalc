using DocumentsServiceInterfaceLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEntitySettings : Form
	{
        #region Fields
        private string _sqliteFilePath;
        private string _baseOpenFolder;
		#endregion

		#region Ctors

		public FormEntitySettings(string filePath, string dbFilePath = "")
		{
			InitializeComponent();

            _baseOpenFolder = dbFilePath;
            txtSqliteFile.Text = filePath;
            this.BackColor = MainForm.ColorBackground;


        }

		#endregion

		#region Events

		private void Form_Load(object sender, EventArgs e)
		{
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void btnOpen_Click(object sender, EventArgs e)
        {

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.RestoreDirectory = true;
            
            if (!string.IsNullOrEmpty(_baseOpenFolder))
            {
                string path = Path.GetDirectoryName(_baseOpenFolder);
                
                fileDialog.InitialDirectory = path;
                fileDialog.Filter = "Sqlite files (*.*)|*.*";                
            }

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                txtSqliteFile.Text = fileDialog.FileName;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

        }

        private void OnSqliteFileChanged(object sender, EventArgs e)
        {
            _sqliteFilePath = this.txtSqliteFile.Text;
        }
        #endregion

        #region Implementation

        public string GetSqliteFilePath()
        {
            return _sqliteFilePath;
        }


        #endregion


    }
}