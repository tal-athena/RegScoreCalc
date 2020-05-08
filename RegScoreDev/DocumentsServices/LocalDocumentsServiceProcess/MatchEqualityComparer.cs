using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LocalDocumentsServiceProcess
{
	internal class MatchEqualityComparer : IEqualityComparer<Match>
	{
		#region Implementation: IEqualityComparer<Match>

		public bool Equals(Match x, Match y)
		{
			return x.Value == y.Value;
		}

		public int GetHashCode(Match obj)
		{
			return obj.Value.GetHashCode();
		}

		#endregion
	}
}
