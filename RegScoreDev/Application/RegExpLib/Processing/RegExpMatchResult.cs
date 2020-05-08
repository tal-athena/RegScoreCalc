using System;
using System.Text.RegularExpressions;

using RegExpLib.Core;

namespace RegExpLib.Processing
{
	[Serializable]
	public class RegExpMatchResult
	{
		#region Fields

		public RegExpBase RegExp { get; private set; }
		public bool RefreshAll { get; private set; }
		public int Position { get; private set; }
		public Match Match { get; private set; }

		public bool NeedMoreData { get; private set; }

        public int ColumnIndex { get; private set; }

		protected int _start;
		protected int _length;

		#endregion

		#region Properties

		public bool IsEmpty
		{
			get
			{
				return this.RegExp == null;
			}
		}

		public int Start
		{
			get
			{
				if (this.Match != null)
					return this.Match.Index;

				if (_start == -1)
					return -1;

				return _start;
			}
		}

		public int End
		{
			get
			{
				if (this.Match != null)
					return this.Match.Index + this.Match.Length;

				if (_start == -1 || _length == -1)
					return -1;

				return _start + _length;
			}
		}

		public int Length
		{
			get
			{
				if (this.Match != null)
					return this.Match.Length;

				if (_length == -1)
					return -1;

				return _length;
			}
		}

		#endregion

		#region Ctors

		public RegExpMatchResult(RegExpBase regExp, int position, Match match, int columnIndex = 0)
		{
			this.RegExp = regExp;
			this.Position = position;
			this.Match = match;
            this.ColumnIndex = columnIndex;

			_start = -1;
			_length = -1;
		}

		public RegExpMatchResult(RegExpBase regExp, int position, int start, int length, int columnIndex = 0)
		{
			this.RegExp = regExp;
			this.Position = position;
            this.ColumnIndex = columnIndex;

			_start = start;
			_length = length;
		}

		#endregion

		#region Static operations

		public static RegExpMatchResult EmptyResult()
		{
			return new RegExpMatchResult(null, -1, null);
		}

		public static RegExpMatchResult NeedMoreDataResult()
		{
			var result = EmptyResult();
			result.NeedMoreData = true;

			return result;
		}

		public static RegExpMatchResult RefreshResult()
		{
			var result = EmptyResult();
			result.RefreshAll = true;

			return result;
		}

		#endregion
	}	
}
