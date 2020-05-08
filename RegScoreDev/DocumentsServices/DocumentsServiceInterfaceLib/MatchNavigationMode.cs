using System;

namespace DocumentsServiceInterfaceLib
{
	[Serializable]
	public enum MatchNavigationMode
	{
		#region Constants

		Refresh = 0,
		Invalidate,
		NextDocument,
		PrevDocument,
		NextMatch,
		PrevMatch,
		NextUniqueMatch,
		PrevUniqueMatch

		#endregion
	}
}

