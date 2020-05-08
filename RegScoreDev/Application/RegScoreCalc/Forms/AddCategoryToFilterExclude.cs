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
    public partial class AddCategoryToFilterExclude : Form
    {

        private ViewsManager _views;
        public int SelectedCategoryID;
        public string Type = "Exclude";

        public AddCategoryToFilterExclude(ViewsManager views)
        {
            InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

            SelectedCategoryID = -1;
            _views = views;
        }
        private void AddCategoryToFilterExclude_Load(object sender, EventArgs e)
        {
            //Load all existing groups to the combobox
            var categories = _views.MainForm.datasetBilling.Categories;
            foreach (var item in categories)
            {
                cmbCategories.Items.Add(new AddGroupComboboxItem() { Text = item.Category, GroupID = item.ID });
            }

            cmbCategories.SelectedValueChanged += cmbCategories_SelectedValueChanged;
        }

        void cmbCategories_SelectedValueChanged(object sender, EventArgs e)
        {
            AddGroupComboboxItem item = (AddGroupComboboxItem)cmbCategories.SelectedItem;
            SelectedCategoryID = item.GroupID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedCategoryID != -1)
            {
                //Get type
                if (radioBtnConcordant.Checked)
                {
                    Type = "Concordant";
                }
                if (radioBtnDiscordant.Checked)
                {
                     Type = "Discordant";
                }
                if (radioBtnExclude.Checked)
                {
                    Type = "Exclude";
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(this, "Select category!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

       
    }
}
