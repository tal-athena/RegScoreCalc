using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using RegExpLib.Database;
using RegExpLib.Model;

namespace RegExpLib.Core
{
	[Serializable]
	public class RegExpBase
	{
		#region Constants

		protected const string _defaultPrefixMatch = "||||||---||||------";
		protected const string _defaultSuffixMatch = "||||||---||||------";

		#endregion

		#region Fields

		protected int _id;

		protected Regex _regex;
		protected string _builtExpression;

		///////////////////////////////////////////////////////////////////////////////

		private readonly RegexOptions _options;
		protected string _expression;

		public string PrefixMatch { get; set; }
		public string SuffixMatch { get; set; }

		public int? Score { get; set; }
		public int? Factor { get; set; }

		protected Color? _color;

		public string Description { get; set; }

		public int? Category { get; set; }

		///////////////////////////////////////////////////////////////////////////////

		protected long _totalDocuments;
		protected long _totalMatches;
        protected long _totalRecords;
        protected Dictionary<int, int> _categoryRecords;

        ///////////////////////////////////////////////////////////////////////////////

        public RegExpCriteriaCollection LookAhead { get; set; }
		public RegExpCriteriaCollection LookBehind { get; set; }
		public RegExpCriteriaCollection NegLookAhead { get; set; }
		public RegExpCriteriaCollection NegLookBehind { get; set; }

		public RegExpCriteriaCollection Exceptions { get; set; }

		///////////////////////////////////////////////////////////////////////////////

		#endregion

		#region Properties

		public int ID
		{
			get { return _id; }
		}

		public Regex Regex
		{
			get { return _regex; }
		}

		public string Expression
		{
			get { return _expression; }
		}

		public string BuiltExpression
		{
			get { return _builtExpression; }
		}

		public Color Color
		{
			get
			{
				if (_color != null)
					return _color.Value;

				return DefaultHighlightColor;
			}
			set { _color = value; }
		}

		public static Color DefaultHighlightColor
		{
			get { return Color.Yellow; }
		}

		public long TotalDocuments
		{
			get { return _totalDocuments; }
		}

		public long TotalMatches
		{
			get { return _totalMatches; }
		}

        public long TotalRecords
        {
            get { return _totalRecords; }
        }

        public Dictionary<int, int> CategorizedRecords
        {
            get { return _categoryRecords; }
        }

		#endregion

		#region Ctors

		protected RegExpBase()
		{
			_expression = "";

			///////////////////////////////////////////////////////////////////////////////

			this.Score = 0;
			this.Factor = 0;

			///////////////////////////////////////////////////////////////////////////////

			LookAhead = new RegExpCriteriaCollection(RegExpCriteriaType.LookAhead);
			LookBehind = new RegExpCriteriaCollection(RegExpCriteriaType.LookBehind);
			NegLookAhead = new RegExpCriteriaCollection(RegExpCriteriaType.NegLookAhead);
			NegLookBehind = new RegExpCriteriaCollection(RegExpCriteriaType.NegLookBehind);

			Exceptions = new RegExpCriteriaCollection(RegExpCriteriaType.Exception);

			///////////////////////////////////////////////////////////////////////////////

			_color = null;

            _categoryRecords = new Dictionary<int, int>();
		}

		protected RegExpBase(RegexOptions options, object id, string regExp, bool build, object score, object factor, string prefixMatch, string suffixMatch, object color, string lookAhead, string lookBehind, string negLookAhead, string negLookBehind, string exceptions, string description, object categoryID)
		{
			_id = Convert.ToInt32(id);

			_expression = regExp;
			_options = options;

            _categoryRecords = new Dictionary<int, int>();

            this.Description = description;

			///////////////////////////////////////////////////////////////////////////////

			this.PrefixMatch = prefixMatch;
			this.SuffixMatch = suffixMatch;

			if (String.IsNullOrEmpty(this.PrefixMatch))
				this.PrefixMatch = _defaultPrefixMatch;

			if (String.IsNullOrEmpty(this.SuffixMatch))
				this.SuffixMatch = _defaultSuffixMatch;

			///////////////////////////////////////////////////////////////////////////////

			if (score != null)
				this.Score = Convert.ToInt32(score);

			if (factor != null)
				this.Factor = Convert.ToInt32(factor);

			if (color != null)
			{
				var argb = Convert.ToInt32(color);

				_color = argb != 0 ? Color.FromArgb(argb) : DefaultHighlightColor;
			}

			this.Category = (int?) categoryID;

			this.LookAhead = RegExpCriteriaCollection.FromString(RegExpCriteriaType.LookAhead, lookAhead);
			this.LookBehind = RegExpCriteriaCollection.FromString(RegExpCriteriaType.LookBehind, lookBehind);
			this.NegLookAhead = RegExpCriteriaCollection.FromString(RegExpCriteriaType.NegLookAhead, negLookAhead);
			this.NegLookBehind = RegExpCriteriaCollection.FromString(RegExpCriteriaType.NegLookBehind, negLookBehind);

			this.Exceptions = RegExpCriteriaCollection.FromString(RegExpCriteriaType.Exception, exceptions);

			///////////////////////////////////////////////////////////////////////////////

			this.LookAhead.Enabled = !this.NegLookAhead.Enabled;
			this.LookBehind.Enabled = !this.NegLookBehind.Enabled;

			NormalizeCriteria();

			///////////////////////////////////////////////////////////////////////////////

			if (build)
				Build();
		}

		protected void NormalizeCriteria()
		{
			if (this.LookAhead.Enabled && this.LookAhead.IsEmpty)
			{
				if (this.NegLookAhead.Items.Any())
				{
					this.LookAhead.Enabled = false;
					this.NegLookAhead.Enabled = true;
				}
			}

			if (this.NegLookAhead.Enabled && this.NegLookAhead.IsEmpty)
			{
				if (this.LookAhead.Items.Any())
				{
					this.NegLookAhead.Enabled = false;
					this.LookAhead.Enabled = true;
				}
			}

			///////////////////////////////////////////////////////////////////////////////

			if (this.LookBehind.Enabled && this.LookBehind.IsEmpty)
			{
				if (this.NegLookBehind.Items.Any())
				{
					this.LookBehind.Enabled = false;
					this.NegLookBehind.Enabled = true;
				}
			}

			if (this.NegLookBehind.Enabled && this.NegLookBehind.IsEmpty)
			{
				if (this.LookBehind.Items.Any())
				{
					this.NegLookBehind.Enabled = false;
					this.LookBehind.Enabled = true;
				}
			}
		}

		#endregion

		#region Static operations

		public static string GetRegExpValue(DataRow row)
		{
			string regExp = String.Empty;
			try
			{
				regExp = (string) row["RegExp"];
			}
			catch { }

			return regExp;
		}

		#endregion

		#region Operations

		public void SetExpression(string value)
		{
			if (_expression != value)
			{
				_expression = value;

				Build();
			}
		}

		public void Build()
		{
			var lookAhead = String.Empty;

			if (!LookAhead.IsEmpty)
			{
				lookAhead = "(?=" + LookAhead.ToExpression() + ")";
			}
			else if (!this.NegLookAhead.IsEmpty)
			{
				lookAhead = "(?!" + NegLookAhead.ToExpression() + ")";
			}

			///////////////////////////////////////////////////////////////////////////////

			var lookBehind = String.Empty;

			if (!LookBehind.IsEmpty)
			{
				lookBehind = "(?<=" + LookBehind.ToExpression() + ")";
			}
			else if (!NegLookBehind.IsEmpty)
			{
				lookBehind = "(?<!" + NegLookBehind.ToExpression() + ")";
			}

			///////////////////////////////////////////////////////////////////////////////

			_builtExpression = lookBehind + this.Expression + lookAhead;

			_regex = new Regex(_builtExpression, _options);

			///////////////////////////////////////////////////////////////////////////////

			// TODO
			//InvalidateMatches();
		}

		public void AddException(string value)
		{
			AddCriteria(RegExpCriteriaType.Exception, value);
		}

		public void AddCriteria(RegExpCriteriaType criteriaType, string value)
		{
			RegExpCriteriaCollection collection = null;

			switch (criteriaType)
			{
				case RegExpCriteriaType.Exception:
					collection = this.Exceptions;
					break;

				case RegExpCriteriaType.LookAhead:
					collection = this.LookAhead;
					break;

				case RegExpCriteriaType.LookBehind:
					collection = this.LookBehind;
					break;

				case RegExpCriteriaType.NegLookAhead:
					collection = this.NegLookAhead;
					break;

				case RegExpCriteriaType.NegLookBehind:
					collection = this.NegLookBehind;
					break;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (collection == null)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var stringComparison = criteriaType == RegExpCriteriaType.Exception ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

			if (collection.Items.Any(x => String.Compare(x.Expression, value, stringComparison) == 0))
				return;

			collection.Items.Add(new RegExpCriteria
			                     {
				                     Expression = value,
				                     Enabled = true
			                     });
		}

		public virtual void Save(DataRowView rowView)
		{
			if (rowView == null)
				return;

			Save(rowView.Row);
		}

		public virtual void Save(DataRow row)
		{
			if (row == null)
				throw new ArgumentException();

			if (String.IsNullOrEmpty(this.Expression))
				return;

			///////////////////////////////////////////////////////////////////////////////

			DatabaseHelper.SetValue(row, "RegExp", this.Expression);
			DatabaseHelper.SetValue(row, "score", this.Score);
			DatabaseHelper.SetValue(row, "Arithmetic factor", this.Factor);

			DatabaseHelper.SetValue(row, "prefix match", this.PrefixMatch != _defaultSuffixMatch ? this.PrefixMatch : null);
			DatabaseHelper.SetValue(row, "suffix match", this.SuffixMatch != _defaultSuffixMatch ? this.SuffixMatch : null);

			///////////////////////////////////////////////////////////////////////////////

			this.LookBehind.Enabled = !this.NegLookBehind.Enabled;
			this.LookAhead.Enabled = !this.NegLookAhead.Enabled;

			///////////////////////////////////////////////////////////////////////////////

			DatabaseHelper.SetValue(row, "lookahead", this.LookAhead.Serialize());
			DatabaseHelper.SetValue(row, "lookbehind", this.LookBehind.Serialize());
			DatabaseHelper.SetValue(row, "neg lookahead", this.NegLookAhead.Serialize());
			DatabaseHelper.SetValue(row, "neg lookbehind", this.NegLookBehind.Serialize());
			DatabaseHelper.SetValue(row, "exceptions", this.Exceptions.Serialize());
			DatabaseHelper.SetValue(row, "description", this.Description);
			DatabaseHelper.SetValue(row, "categoryID", this.Category);

			///////////////////////////////////////////////////////////////////////////////

			Build();
		}

		public List<Match> GetFilteredMatches(string text)
		{
			text = CleanupDocumentText(text);

			return this.Regex.Matches(text)
			           .Cast<Match>()
			           .Where(x => !IsExceptionMatch(x) && x.Length > 0)
			           .ToList();
		}

		public string ReplaceMatches(string text, string replaceWith)
		{
			text = CleanupDocumentText(text);

			return this.Regex.Replace(text, replaceWith);
		}

		public static string CleanupDocumentText(string text)
		{
			if (!String.IsNullOrEmpty(text))
			{
				if (!text.Contains("\r"))
					text = text.Replace("\n", "\r\n");

				if (!text.Contains("\n"))
					text = text.Replace("\r", "\r\n");

				text = text.Replace("\t", "    ");
			}
			else
				text = String.Empty;

			return text;
		}

		public bool IsExceptionMatch(Match match)
		{
			if (this.Exceptions.Items.Count == 0)
				return false;

			return this.Exceptions.Items.Any(x => x.Enabled && String.Compare(match.Value, x.Expression, StringComparison.InvariantCultureIgnoreCase) == 0);
		}

		public void IncrementTotalDocuments()
		{
			Interlocked.Increment(ref _totalDocuments);
		}

		public void AddTotalMatches(long matches)
		{
			if (matches > 0)
				Interlocked.Add(ref _totalMatches, matches);
		}

        public void IncrementTotalRecords()
        {
            Interlocked.Increment(ref _totalRecords);
        }

        public void IncrementCategoryMatch(int category)
        {            
            if (_categoryRecords.ContainsKey(category))
                _categoryRecords[category]++;
            else
                _categoryRecords.Add(category, 1);
        }

        #endregion
    }
}
