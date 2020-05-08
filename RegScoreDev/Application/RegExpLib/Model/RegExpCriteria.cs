using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace RegExpLib.Model
{
	[Serializable]
	public class RegExpCriteriaCollection
	{
		#region Fields

		[JsonIgnore]
		public RegExpCriteriaType Type { get; set; }

		public bool Enabled { get; set; }
		public List<RegExpCriteria> Items { get; set; }

		#endregion

		#region Properties

		public bool IsEmpty
		{
			get
			{
				if (this.Type != RegExpCriteriaType.Exception && !this.Enabled)
					return true;

				return !this.Items.Any(x => x.Enabled && !String.IsNullOrEmpty(x.Expression));
			}
		}

		#endregion

		#region Ctors

		public RegExpCriteriaCollection(RegExpCriteriaType type)
		{
			this.Type = type;
			this.Items = new List<RegExpCriteria>();
		}

		#endregion

		#region Static pperations

		public static RegExpCriteriaCollection FromString(RegExpCriteriaType type, string look)
		{
			var result = new RegExpCriteriaCollection(type);

			if (String.IsNullOrEmpty(look))
				return result;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				if (look.StartsWith("{") || look.StartsWith("["))
				{
					result = JsonConvert.DeserializeObject<RegExpCriteriaCollection>(look);

					return result;
				}
			}
			catch { }

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				result.Items = look.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries)
								   .Select(x => new RegExpCriteria { Expression = x, Enabled = true })
								   .ToList();
			}
			catch { }

			///////////////////////////////////////////////////////////////////////////////

			return result;
		}

		#endregion

		#region Operations

		public string ToExpression()
		{
			var items = this.Items.Where(x => x.Enabled && !String.IsNullOrEmpty(x.Expression)).ToList();
			if (!items.Any())
				return String.Empty;

			if (items.Count == 1)
				return items.First().Expression;

			return items.Aggregate("", (aggr, x) =>
			                           {
				                           if (!String.IsNullOrEmpty(aggr))
					                           aggr += "|";

				                           aggr += x.Expression;

				                           return aggr;
			                           });
		}

		public object Serialize()
		{
			if (this.Items.Any(x => !String.IsNullOrEmpty(x.Expression)))
				return JsonConvert.SerializeObject(this);

			return DBNull.Value;
		}

		#endregion

		#region Implementation

		#endregion
	}

	[Serializable]
	public class RegExpCriteria
	{
		#region Fields

		public string Expression { get; set; }
		public bool Enabled { get; set; }

		#endregion
	}

	[Serializable]
	public enum RegExpCriteriaType
	{
		#region Constants

		Exception,
		LookAhead,
		LookBehind,
		NegLookAhead,
		NegLookBehind

		#endregion
	}
}
