using System;
using System.Text.RegularExpressions;

namespace RegExpLib.Core
{
	[Serializable]
	public class RegExp : RegExpBase
	{
		#region Ctors

		public RegExp()
		{
			
		}

		public RegExp(RegexOptions options, object id, string regExp, bool build, object score, object factor, string prefixMatch, string suffixMatch, object color, string lookAhead, string lookBehind, string negLookAhead, string negLookBehind, string exceptions, string description, object categoryID)
			: base(options, id, regExp, build, score, factor, prefixMatch, suffixMatch, color, lookAhead, lookBehind, negLookAhead, negLookBehind, exceptions, description, categoryID)
		{
			
		}

		#endregion
	}
}
