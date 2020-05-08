using System;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace RegScoreCalc
{
	public partial class FormColumnPropsNumeric : Form
	{
		#region Fields

		protected ViewsManager _views;

		#endregion

		#region Ctors

		public FormColumnPropsNumeric(ViewsManager views, MainDataSet.DynamicColumnsRow dynamicColumnsRow)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			_views = views;

			txtMinimum.Minimum = Decimal.MinValue;
			txtMinimum.Maximum = Decimal.MaxValue;

			txtMaximum.Minimum = Decimal.MinValue;
			txtMaximum.Maximum = Decimal.MaxValue;

			LoadProps(dynamicColumnsRow);
		}

		#endregion

		#region Events

		private void rdbInteger_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateControls();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void rdbDecimal_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateControls();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void txtDecimalPlaces_ValueChanged(object sender, EventArgs e)
		{
			try
			{
				txtMinimum.DecimalPlaces = (int)txtDecimalPlaces.Value;
				txtMaximum.DecimalPlaces = (int)txtDecimalPlaces.Value;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public string SaveProps()
		{
			var props = new NumericColumnProperties();
			props.IsDecimal = rdbDecimal.Checked;
			props.Min = txtMinimum.Value;
			props.Max = txtMaximum.Value;
			props.DecimalPlaces = (int)txtDecimalPlaces.Value;

			if (props.Max < props.Min)
				props.Max = props.Min;

			return props.Save();
		}

		#endregion

		#region Implementation

		protected void LoadProps(MainDataSet.DynamicColumnsRow dynamicColumnsRow)
		{
			try
			{
				var props = NumericColumnProperties.Load(dynamicColumnsRow);

				if (props.IsDecimal)
					rdbDecimal.Checked = true;
				else
					rdbInteger.Checked = true;

				txtMinimum.Value = props.Min;
				txtMaximum.Value = props.Max;

				txtDecimalPlaces.Value = props.DecimalPlaces;

				UpdateControls();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void UpdateControls()
		{
			if (rdbInteger.Checked)
			{
				txtDecimalPlaces.Enabled = false;

				txtMinimum.DecimalPlaces = 0;
				txtMaximum.DecimalPlaces = 0;
			}
			else
			{
				txtDecimalPlaces.Enabled = true;

				txtMinimum.DecimalPlaces = (int)txtDecimalPlaces.Value;
				txtMaximum.DecimalPlaces = (int)txtDecimalPlaces.Value;
			}
		}

		#endregion
	}

	public class NumericColumnProperties
	{
		#region Fields

		public bool IsDecimal { get; set; }
		public decimal Max { get; set; }
		public decimal Min { get; set; }
		public int DecimalPlaces { get; set; }

		#endregion

		#region Operations

		public bool IsValidValue(decimal value)
		{
			var isValid = value >= this.Min;
			if (isValid)
			{
				if (this.Max != this.Min && value > this.Max)
					isValid = false;
			}

			return isValid;
		}

		public static NumericColumnProperties Load(MainDataSet.DynamicColumnsRow row)
		{
			NumericColumnProperties result = null;

			///////////////////////////////////////////////////////////////////////////////

			if (row != null && !row.IsPropertiesNull())
			{
				var propsJson = row.Properties;
				if (!String.IsNullOrEmpty(propsJson))
					result = JsonConvert.DeserializeObject<NumericColumnProperties>(propsJson);
			}

			///////////////////////////////////////////////////////////////////////////////

			if (result == null)
			{
				result = new NumericColumnProperties
				{
					IsDecimal = false,
					DecimalPlaces = 1
				};
			}

			return result;
		}

		public string Save()
		{
			return JsonConvert.SerializeObject(this);
		}

		public string GetFormatString()
		{
			if (this.IsDecimal)
				return "N" + this.DecimalPlaces;

			return "N0";
		}

		#endregion
	}
}
