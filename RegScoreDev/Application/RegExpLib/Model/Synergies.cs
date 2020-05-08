using System.Collections.Generic;
using System.Diagnostics;

namespace RegExpLib.Core
{
	[DebuggerDisplay("id = {RegExpID}, count = {Synergies.Count}")]
	public class RegExpSynergy
	{
		public RegExpSynergy()
		{
			this.Synergies = new List<Synergy>();
		}

		public int RegExpID { get; set; }
		public List<Synergy> Synergies { get; set; }
	}

	[DebuggerDisplay("id = {MatchingRegExpId}, factor = {Factor}")]
	public class Synergy
	{
		public int MatchingRegExpId { get; set; }
		public double Factor { get; set; }
	}
}