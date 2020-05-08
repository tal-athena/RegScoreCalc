using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEditColumnNumeric : Form
	{
		#region Fields

		protected NumericColumnProperties _props;
		protected bool _lockValueSync;

		#endregion

		#region Properties

		public object Value
		{
			get { return txtValue.Text.Length > 0 ? Convert.ToDouble(txtValue.Value) : (object) null; }
			set { txtValue.Value = Convert.ToDecimal(value); }
		}

		#endregion

		#region Ctors

		public FormEditColumnNumeric(NumericColumnProperties props)
		{
			InitializeComponent();

			_props = props;

			txtValue.Minimum = Decimal.MinValue;
			txtValue.Maximum = Decimal.MaxValue;

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		private void FormEditColumnNumeric_Load(object sender, EventArgs e)
		{
			try
			{
				var format = _props.GetFormatString();

				lblMin.Text = _props.Min.ToString(format);

				if (_props.Max != _props.Min)
					lblMax.Text = _props.Max.ToString(format);

				if (_props.IsDecimal)
					txtValue.DecimalPlaces = _props.DecimalPlaces;

				txtValue.Minimum = _props.Min;

				if (_props.Max != _props.Min)
					txtValue.Maximum = _props.Max;

				///////////////////////////////////////////////////////////////////////////////

				txtValue.Select();

				txtValue.Select(0, txtValue.Text.Length);

				ShowValueComboBox();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormEditColumnNumeric_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (txtValue.Text.Length > 0 && !_props.IsValidValue(txtValue.Value))
					{
						MessageBox.Show("Specified value does not match the allowed range", MainForm.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						e.Cancel = true;
					}
					else
					{
						if (!_props.IsDecimal && txtValue.DecimalPlaces > 0)
						{
							_props.IsDecimal = true;
							_props.DecimalPlaces = txtValue.DecimalPlaces;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void cmbValues_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_lockValueSync)
				return;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				_lockValueSync = true;

				///////////////////////////////////////////////////////////////////////////////

				if (cmbValues.SelectedIndex != -1)
					txtValue.Value = (int) cmbValues.SelectedItem;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
			finally
			{
				_lockValueSync = false;
			}
		}

		private void txtValue_ValueChanged(object sender, EventArgs e)
		{
			if (_lockValueSync)
				return;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				_lockValueSync = true;

				///////////////////////////////////////////////////////////////////////////////

				if (cmbValues.Visible)
				{
					var currentValue = Convert.ToInt32(txtValue.Value);

					if (cmbValues.Items.Contains(currentValue))
						cmbValues.SelectedItem = currentValue;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
			finally
			{
				_lockValueSync = false;
			}
		}

		private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (!_props.IsDecimal)
				{
					var culture = Thread.CurrentThread.CurrentCulture;
					if (e.KeyChar.ToString() == culture.NumberFormat.NumberDecimalSeparator)
					{
						if (this.txtValue.DecimalPlaces == 0)
						{
							this.txtValue.DecimalPlaces = 1;
							this.txtValue.Select(this.txtValue.Text.Length - 1, 1);

							e.Handled = true;
						}
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void txtValue_Validated(object sender, EventArgs e)
		{
			try
			{
				UpdateDecimalPlaces();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormEditColumnNumeric_Deactivate(object sender, EventArgs e)
		{
			try
			{
				UpdateDecimalPlaces();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void txtValue_Leave(object sender, EventArgs e)
		{
			try
			{
				UpdateDecimalPlaces();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Implementation

		protected void ShowValueComboBox()
		{
			if (_props.IsDecimal)
				return;

			if (_props.Min == _props.Max)
				return;

			var valueCount = Convert.ToInt32(_props.Max - _props.Min) + 1;
			if (valueCount > 1000)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var values = Enumerable.Range(Convert.ToInt32(_props.Min), valueCount)
								   .Cast<object>()
								   .ToArray();

			cmbValues.Items.AddRange(values);

			var currentValue = Convert.ToInt32(txtValue.Value);

			if (values.Contains(currentValue))
				cmbValues.SelectedItem = currentValue;

			cmbValues.Visible = true;
		}

		protected void UpdateDecimalPlaces()
		{
			if (!_props.IsDecimal && txtValue.DecimalPlaces > 0)
			{
				var culture = Thread.CurrentThread.CurrentCulture;
				var split = txtValue.Value.ToString().Split(new[] { culture.NumberFormat.NumberDecimalSeparator }, StringSplitOptions.None);
				if (split.Length == 2)
					txtValue.DecimalPlaces = split[1].Length;
			}
		}

		#endregion
	}
}