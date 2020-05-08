using System;
using System.Text.RegularExpressions;
using System.Threading;

using Newtonsoft.Json;

using RegExpLib.Model;

namespace RegExpLib.Core
{
	[Serializable]
	public class ColRegExp : RegExpBase
	{
		#region Fields

		protected int _columnID;

		///////////////////////////////////////////////////////////////////////////////
		
		public ExtractOptions ExtractOptions { get; set; }

		protected long _posDocuments;

		///////////////////////////////////////////////////////////////////////////////

		#endregion

		#region Properties

		public int ColumnID
		{
			get { return _columnID; }
		}

		public long PostitiveDocuments
		{
			get { return _posDocuments; }
		}

		#endregion

		#region Ctors

		public ColRegExp() : base()
		{

		}

		public ColRegExp(RegexOptions options, object id, object columnID, string regExp, bool build, string extract, object score, object factor, string prefixMatch, string suffixMatch, object color, string lookAhead, string lookBehind, string negLookAhead, string negLookBehind, string exceptions, string description)
			: base(options, id, regExp, build, score, factor, prefixMatch, suffixMatch, color, lookAhead, lookBehind, negLookAhead, negLookBehind, exceptions, description, null)
		{
			_columnID = Convert.ToInt32(columnID);

			try
			{
				if (extract != null)
					this.ExtractOptions = JsonConvert.DeserializeObject<ExtractOptions>(extract);
			}
			catch { }
		}

		#endregion

		#region Operations

		public void IncrementPositiveDocuments()
		{
			Interlocked.Increment(ref _posDocuments);
		}

		#endregion
	}
}
