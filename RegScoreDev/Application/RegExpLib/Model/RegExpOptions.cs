using System.Text.RegularExpressions;

namespace RegExpLib.Model
{
	public static class RegExpOptions
	{
		#region Static operations

		public static RegexOptions CreateOptions(bool ignoreCase = true, bool compiled = false)
		{
			var options = RegexOptions.None;

			if (ignoreCase)
				options |= RegexOptions.IgnoreCase;

			if (compiled)
				options |= RegexOptions.Compiled;

			return options;
		}

		#endregion
	}
}
