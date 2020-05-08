using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using RegExpLib.Model;

namespace RegScoreCalc.Forms
{
	public partial class FormExtractColRegExp : Form
	{

        private Dictionary<string, int> _noteDocumentColumn;

		public bool extract;
		public string json;
		private ExtractOptions extractObj;

		DynamicColumnType formType;

		public FormExtractColRegExp(DynamicColumnType type, string json, Dictionary<string, int> noteDocumentcolum)
		{

            _noteDocumentColumn = noteDocumentcolum;

            InitializeComponent();
			formType = type;
			if (formType == DynamicColumnType.FreeText)
			{
				rbMultipleValues.Visible = true;
				chbAddToPrevious.Visible = true;
			}
			else if (formType == DynamicColumnType.DateTime)
			{
				lbFormat.Visible = true;
				txtFormat.Visible = true;
			}
			rbFirstInstance.Checked = true;

			//Create object from json
			if (String.IsNullOrEmpty(json))
				extractObj = new ExtractOptions();
			else
			{
				extractObj = Newtonsoft.Json.JsonConvert.DeserializeObject<ExtractOptions>(json);
				chbExtract.Checked = extractObj.Extract;
				numericOrder.Value = extractObj.Order;
				if (extractObj.InstanceNo == 1)
					rbFirstInstance.Checked = true;
				else if (extractObj.InstanceNo == 2)
					rbLastInstance.Checked = true;
				else if (extractObj.InstanceNo == 3)
				{
					rbNthInstance.Checked = true;
					numericNthInstnce.Value = (decimal) extractObj.NthInstaceNumber;
				}
				else if (extractObj.InstanceNo == 4)
					rbMultipleValues.Checked = true;

				if (formType == DynamicColumnType.FreeText)
					chbAddToPrevious.Checked = extractObj.AddToPrevious ?? false;
				else if (formType == DynamicColumnType.DateTime)
					txtFormat.Text = extractObj.DateTimeFormat;

			}

            comboDocument.Items.AddRange(_noteDocumentColumn.Keys.ToArray());
            comboDocument.SelectedIndex = 0;

            foreach (var item in comboDocument.Items)
            {
                if (_noteDocumentColumn[item.ToString()] == extractObj.NoteTextColumn)
                    comboDocument.SelectedItem = item;
            }

            UpdateEnableState();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			extract = chbExtract.Checked;

			//Create object
			extractObj = new ExtractOptions();
			extractObj.Extract = chbExtract.Checked;
			extractObj.Order = (int) numericOrder.Value;
			if (rbFirstInstance.Checked)
				extractObj.InstanceNo = 1;
			else if (rbLastInstance.Checked)
				extractObj.InstanceNo = 2;
			else if (rbNthInstance.Checked)
			{
				extractObj.InstanceNo = 3;
				extractObj.NthInstaceNumber = (int) numericNthInstnce.Value;
			}
			else if (rbMultipleValues.Checked)
				extractObj.InstanceNo = 4;

			if (formType == DynamicColumnType.FreeText)
				extractObj.AddToPrevious = chbAddToPrevious.Checked;
			else if (formType == DynamicColumnType.DateTime)
				extractObj.DateTimeFormat = txtFormat.Text;

            if (comboDocument.SelectedIndex == -1)
                comboDocument.SelectedIndex = 0;
            
            extractObj.NoteTextColumn = _noteDocumentColumn[comboDocument.SelectedItem.ToString()];

            json = Newtonsoft.Json.JsonConvert.SerializeObject(extractObj);

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void chbExtract_CheckedChanged(object sender, EventArgs e)
		{
			UpdateEnableState();
		}

		protected void UpdateEnableState()
		{
			foreach (var ctrl in this.Controls.Cast<Control>().Where(x => x is Button == false && x != chbExtract))
			{
				ctrl.Enabled = chbExtract.Checked;
			}
		}
	}
}