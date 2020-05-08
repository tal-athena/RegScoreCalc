using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using Newtonsoft.Json;

namespace RegScoreCalc.Code
{
	public class ColumnsViewModel
	{
		#region Fields

		public List<DynamicColumnSettings> ColumnsSettingsList { get; set; }

		#endregion

		#region Ctors

		public ColumnsViewModel()
		{
			this.ColumnsSettingsList = new List<DynamicColumnSettings>();
		}

		#endregion

		#region Operations

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static ColumnsViewModel FromJSON(string json)
		{
			if (String.IsNullOrEmpty(json))
				return new ColumnsViewModel();

			///////////////////////////////////////////////////////////////////////////////

			return JsonConvert.DeserializeObject<ColumnsViewModel>(json);
		}

		#endregion
	}

	[DebuggerDisplay("{Name} - {IsSelected}")]
	public class DynamicColumnSettings
	{
		#region Fields

		public bool IsSelected { get; set; }
		public int ColumnID { get; set; }
		public string Name { get; set; }
		public List<DynamicColumnSettings> HighlightColumnsList { get; set; }

		#endregion

		#region Ctor

		public DynamicColumnSettings()
		{
			this.HighlightColumnsList = new List<DynamicColumnSettings>();
		}

		#endregion
	}

	public class DynamicCategoryProperties
	{
		#region	Properties

		public Color Background { get; set; }

		#endregion

		#region Operations

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, new ColorConverter());
		}

		public static DynamicCategoryProperties FromJSON(string json)
		{
			if (String.IsNullOrEmpty(json))
				return new DynamicCategoryProperties { Background = Color.White };

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				return JsonConvert.DeserializeObject<DynamicCategoryProperties>(json, new ColorConverter());
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}

			return new DynamicCategoryProperties { Background = Color.White };
		}

		#endregion
	}

	public class ColorConverter : JsonConverter
	{
		#region Overrides

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var color = (Color) value;
			writer.WriteValue(color.ToArgb().ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var value = reader.Value as string;
			if (!String.IsNullOrEmpty(value))
				return Color.FromArgb(Convert.ToInt32(value));
			else
				return Color.White;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.Name == "Color";
		}

		#endregion
	}
}