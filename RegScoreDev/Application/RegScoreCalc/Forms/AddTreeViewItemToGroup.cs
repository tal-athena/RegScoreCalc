using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
    public partial class AddTreeViewItemToGroup : Form
    {
        public string icd;
        public string description;
        public bool regExp;
        public int groupId;
        ViewsManager _views;

        public AddTreeViewItemToGroup(ViewsManager views, string ICD9, string Diagnosis)
        {
            icd = ICD9;
            description = Diagnosis;
            _views = views;
            InitializeComponent();

            txtCode.Text = icd;
            txtDiagnosis.Text = description;
        }

        private void AddTreeViewItemToGroup_Load(object sender, EventArgs e)
        {
            //Load all existing groups to the combobox
            var groups = _views.MainForm.datasetBilling.ICD9Groups;
            foreach (var item in groups)
            {
                cmbGroups.Items.Add(new AddGroupComboboxItem() { Text = item.Name, GroupID = item.GroupID });
            }

            //if (cmbGroups.Items.Count > 0)
            //{
            //    cmbGroups.SelectedIndex = 0;
            //}

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (txtDiagnosis.Text != "" && txtCode.Text != "" && cmbGroups.SelectedIndex > -1)
            {
                groupId = ((AddGroupComboboxItem)cmbGroups.SelectedItem).GroupID;

                icd = txtCode.Text;
                description = txtDiagnosis.Text;
                regExp = chbRegExp.Checked;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Please provide all data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }


    }
}
