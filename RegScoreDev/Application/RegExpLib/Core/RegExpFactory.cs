using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

using RegExpLib.Database;
using RegExpLib.Model;

namespace RegExpLib.Core
{
	public static class RegExpFactory
	{
		#region RegExp

		public static RegExp Create_RegExp(DataRowView rowView, bool build, bool ignoreCase = true, bool compiled = false)
		{
			if (rowView == null || rowView.Row == null)
				return null;

			return Create_RegExp(rowView.Row, build, ignoreCase, compiled);
		}

		public static RegExp Create_RegExp(DataRow row, bool build, bool ignoreCase = true, bool compiled = false)
		{
			return Create_RegExp(row, build, RegExpOptions.CreateOptions(ignoreCase, compiled));
		}

		public static RegExp Create_RegExp(DataRow row, bool build, RegexOptions options)
		{
			if (row.RowState == DataRowState.Deleted) //|| row.RowState == DataRowState.Detached)
				return null;

			var expression = DatabaseHelper.GetValue<string>(row, "RegExp");

			var result = new RegExp(options,
				DatabaseHelper.GetValue<object>(row, "ID"),
				expression,
				build,
				DatabaseHelper.GetValue<object>(row, "score"),
				DatabaseHelper.GetValue<object>(row, "Arithmetic factor"),
				DatabaseHelper.GetValue<string>(row, "prefix match"),
				DatabaseHelper.GetValue<string>(row, "suffix match"),
				DatabaseHelper.GetValue<object>(row, "RegExpColor"),
				DatabaseHelper.GetValue<string>(row, "lookahead"),
				DatabaseHelper.GetValue<string>(row, "lookbehind"),
				DatabaseHelper.GetValue<string>(row, "neg lookahead"),
				DatabaseHelper.GetValue<string>(row, "neg lookbehind"),
				DatabaseHelper.GetValue<string>(row, "exceptions"),
				DatabaseHelper.GetValue<string>(row, "description"),
				DatabaseHelper.GetValue<object>(row, "categoryID"));

			return result;
		}

		#endregion

		#region ColRegExp

		public static ColRegExp Create_ColRegExp(DataRowView rowView, IEnumerable<int> columnIDs, bool build, bool ignoreCase = true, bool compiled = false)
		{
			if (rowView == null || rowView.Row == null)
				return null;

			return Create_ColRegExp(rowView.Row, columnIDs, build, ignoreCase, compiled);
		}

		public static ColRegExp Create_ColRegExp(DataRow row, IEnumerable<int> columnIDs, bool build, bool ignoreCase = true, bool compiled = false)
		{
			return Create_ColRegExp(row, columnIDs, build, RegExpOptions.CreateOptions(ignoreCase, compiled));
		}

		public static ColRegExp Create_ColRegExp(DataRow row, IEnumerable<int> columnIDs, bool build, RegexOptions options)
		{
			if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
				return null;

			///////////////////////////////////////////////////////////////////////////////

			var rowColumnID = DatabaseHelper.GetValue<object>(row, "ColumnID");
			if (columnIDs != null && columnIDs.Count(x => x == -1) == 0)
			{
				if (!columnIDs.Contains((int) rowColumnID))
					return null;
			}

			var expression = DatabaseHelper.GetValue<string>(row, "RegExp");

			var result = new ColRegExp(options,
				DatabaseHelper.GetValue<object>(row, "ID"),
				rowColumnID,
				expression,
				build,
				DatabaseHelper.GetValue<string>(row, "Extract"),
				null,
				null,
				null,
				null,
				DatabaseHelper.GetValue<object>(row, "RegExpColor"),
				DatabaseHelper.GetValue<string>(row, "lookahead"),
				DatabaseHelper.GetValue<string>(row, "lookbehind"),
				DatabaseHelper.GetValue<string>(row, "neg lookahead"),
				DatabaseHelper.GetValue<string>(row, "neg lookbehind"),
				DatabaseHelper.GetValue<string>(row, "exceptions"),
				DatabaseHelper.GetValue<string>(row, "description"));

			return result;
		}

		#endregion
	}
}
