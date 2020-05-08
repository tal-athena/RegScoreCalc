using System;
using System.Windows.Forms;
using Helpers;

namespace ExternalSort
{
	public class Program
	{
		public static int Main(string[] args)
		{
			try
			{
				if (args.Length < 8)
					return -1;

				if (args[0] == "-m")
					MessageBox.Show("Tool started");

				///////////////////////////////////////////////////////////////////////////////

				var sortGroupsByCriteria = SortGroupsBy.None;

				var groupByColumn = args[5];
				var sortGroupsByColumn = args[7];

				switch (args[6])
				{
					case "-min":
						sortGroupsByCriteria = SortGroupsBy.Min;
						break;

					case "-max":
						sortGroupsByCriteria = SortGroupsBy.Max;
						break;

					case "-sum":
						sortGroupsByCriteria = SortGroupsBy.Sum;
						break;
				}

				var password = String.Empty;
				if (args.Length >= 9)
					password = args[8];

				///////////////////////////////////////////////////////////////////////////////

				var sorter = new Sorter(args[1], args[2], args[3], args[4], groupByColumn, sortGroupsByCriteria, sortGroupsByColumn, password);
				return sorter.Sort();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);

				MessageBox.Show(ex.Message, "External Sort Tool", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

				return -2;
			}
		}
	}
}
