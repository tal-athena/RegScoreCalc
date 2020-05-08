using System;
using System.Drawing;

using Newtonsoft.Json;

namespace RegScoreCalc.Views.Models
{
	public class ColorConverter : JsonConverter
	{
		#region Overrides

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Color color;

			var value = reader.Value as string;
			if (!String.IsNullOrEmpty(value))
				color = ColorTranslator.FromHtml(value);
			else
				color = Color.White;

			return new SolidBrush(color);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType.Name == "SolidBrush";
		}

		#endregion
	}
}
