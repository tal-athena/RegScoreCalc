using System;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace RegScoreCalc
{
	public partial class FormColumnPropsDateTime : Form
	{
		#region Fields

		protected ViewsManager _views;

		#endregion

		#region Ctors

		public FormColumnPropsDateTime(ViewsManager views, MainDataSet.DynamicColumnsRow dynamicColumnsRow)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			_views = views;

			cmbDateFormat.SelectedIndex = 0;
			cmbTimeFormat.SelectedIndex = 0;

			LoadProps(dynamicColumnsRow);
		}

		#endregion

		#region Events
	
		private void rdbDateOnly_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateFormatControls();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void rdbTimeOnly_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateFormatControls();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void rdbDateTime_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateFormatControls();
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
			var props = new DateTimeColumnProperties();

			if (rdbDateOnly.Checked)
				props.Format = DateTimeColumnFormat.DateOnly;
			else if (rdbTimeOnly.Checked)
				props.Format = DateTimeColumnFormat.TimeOnly;
			else
				props.Format = DateTimeColumnFormat.DateTime;

			props.DateFormat = cmbDateFormat.Text;
			props.TimeFormat = cmbTimeFormat.Text;

			return props.Save();
		}

		#endregion

		#region Implementation

		protected void LoadProps(MainDataSet.DynamicColumnsRow dynamicColumnsRow)
		{
			try
			{
				var props = DateTimeColumnProperties.Load(dynamicColumnsRow);

				switch (props.Format)
				{
					case DateTimeColumnFormat.DateOnly:
						rdbDateOnly.Checked = true;
						break;

					case DateTimeColumnFormat.TimeOnly:
						rdbTimeOnly.Checked = true;
						break;

					case DateTimeColumnFormat.DateTime:
						rdbDateTime.Checked = true;
						break;
				}

				if (!String.IsNullOrEmpty(props.DateFormat))
					cmbDateFormat.Text = props.DateFormat;

				if (!String.IsNullOrEmpty(props.TimeFormat))
					cmbTimeFormat.Text = props.TimeFormat;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected void UpdateFormatControls()
		{
			cmbDateFormat.Enabled = rdbDateOnly.Checked || rdbDateTime.Checked;
			cmbTimeFormat.Enabled = rdbTimeOnly.Checked || rdbDateTime.Checked;
		}

		#endregion
	}

	public class DateTimeColumnProperties
	{
		#region Fields

		public DateTimeColumnFormat Format { get; set; }
		public string DateFormat { get; set; }
		public string TimeFormat { get; set; }

		#endregion

		#region Ctors

		public DateTimeColumnProperties()
		{
			this.Format = DateTimeColumnFormat.DateOnly;
			this.DateFormat = "MM/dd/yy";
			this.TimeFormat = "HH:mm:ss (24-hour clock)";
		}

		#endregion

		#region Operations

		public static DateTimeColumnProperties Load(MainDataSet.DynamicColumnsRow row)
		{
			DateTimeColumnProperties result = null;

			///////////////////////////////////////////////////////////////////////////////

			if (row != null && !row.IsPropertiesNull())
			{
				var propsJson = row.Properties;
				if (!String.IsNullOrEmpty(propsJson))
					result = JsonConvert.DeserializeObject<DateTimeColumnProperties>(propsJson);
			}

			///////////////////////////////////////////////////////////////////////////////

			if (result == null)
			{
				result = new DateTimeColumnProperties
				{
					Format = DateTimeColumnFormat.DateOnly,
					DateFormat = "MM/dd/yy",
					TimeFormat = "HH:mm:ss (24-hour clock)"
				};
			}

			///////////////////////////////////////////////////////////////////////////////

			return result;
		}

		public string Save()
		{
			return JsonConvert.SerializeObject(this);
		}

		public string GetFormatString()
		{
			string dateFormat = String.Empty;
			string timeFormat = String.Empty;

			if (this.Format == DateTimeColumnFormat.DateTime || this.Format == DateTimeColumnFormat.DateOnly)
				dateFormat = this.DateFormat;

			if (this.Format == DateTimeColumnFormat.DateTime || this.Format == DateTimeColumnFormat.TimeOnly)
			{
				timeFormat = this.TimeFormat;

				if (!String.IsNullOrEmpty(timeFormat))
				{
					var pos = timeFormat.IndexOf(" ");
					if (pos != -1)
						timeFormat = timeFormat.Remove(pos, timeFormat.Length - pos);
				}
			}

			string customFormat;
			if (!String.IsNullOrEmpty(dateFormat))
			{
				customFormat = dateFormat;

				if (!String.IsNullOrEmpty(timeFormat))
					customFormat += " " + timeFormat;
			}
			else
				customFormat = timeFormat;

			return customFormat;
		}

		#endregion
	}

	public enum DateTimeColumnFormat
	{
		#region Constants

		DateOnly = 0,
		TimeOnly,
		DateTime

		#endregion
	}
}
