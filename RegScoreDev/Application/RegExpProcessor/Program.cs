using System;
using System.IO;
using System.Windows.Forms;

using RegExpLib.Common;
using RegExpLib.Model;
using RegExpLib.Processing;

namespace RegExpProcessor
{
	public static class Program
	{
		#region Static fields

		private static Logger _logger;

		#endregion

		#region Entry point

		[MTAThread]
		public static int Main(string[] arguments)
		{
			try
			{
				if (arguments.Length < 1)
					return 1;

				///////////////////////////////////////////////////////////////////////////////

				if (Properties.Settings.Default.ShowStartupMessage)
					MessageBox.Show("TOOL STARTED");

				///////////////////////////////////////////////////////////////////////////////

				var startupDelay = Properties.Settings.Default.StartupDelay;
				if (startupDelay > 0)
					System.Threading.Thread.Sleep(startupDelay);

				///////////////////////////////////////////////////////////////////////////////

				var paramsFilePath = arguments[0];

				var param = RegExpProcessingParamsBase.Deserialize<RegExpProcessingParamsBase>(paramsFilePath);

				_logger = new Logger(param.GetFullPath(param.ProgressFileName), param.GetFullPath(param.LogFileName));

				switch (param.Operation)
				{
					case ProcessingOperation.RegExp_CalcScores:
						RegExp_CalcScores(RegExpProcessingParamsBase.Deserialize<RegExpScoreProcessingParams>(paramsFilePath));
						break;

					case ProcessingOperation.RegExp_CalcStatistics:
						RegExp_CalcStatistics(RegExpProcessingParamsBase.Deserialize<RegExpStatisticsProcessingParams>(paramsFilePath));
						break;

					case ProcessingOperation.RegExp_CalcStatistics_Single:
						RegExp_CalcStatistics_Single(RegExpProcessingParamsBase.Deserialize<RegExpStatisticsSingleProcessingParams>(paramsFilePath));
						break;

					case ProcessingOperation.ColRegExp_CalcStatistics_Single:
						ColRegExp_CalcStatistics_Single(RegExpProcessingParamsBase.Deserialize<RegExpStatisticsSingleProcessingParams>(paramsFilePath));
						break;

					case ProcessingOperation.ColRegExp_CalcStatistics:
						ColRegExp_CalcScores(RegExpProcessingParamsBase.Deserialize<ColRegExpStatisticsProcessingParams>(paramsFilePath));
						break;

					case ProcessingOperation.ColRegExp_Extract:
						ColRegExp_Extract(RegExpProcessingParamsBase.Deserialize<ColRegExpExtractProcessingParams>(paramsFilePath));
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}

				///////////////////////////////////////////////////////////////////////////////

				return 0;
			}
			catch (Exception ex)
			{
				try
				{
					if (_logger != null)
						_logger.HandleException(ex);
				}
				catch
				{
				}

				return 2;
			}
		}

		#endregion

		#region Implementation

		private static void RegExp_CalcScores(RegExpScoreProcessingParams param)
		{
			var processor = new RegExpLib.Processing.RegExpProcessor(_logger, param.RegExpDatabaseFilePath, param.Password, true, true);
			processor.CalcScores(param);
		}

		private static void RegExp_CalcStatistics(RegExpStatisticsProcessingParams param)
		{
			var processor = new RegExpLib.Processing.RegExpProcessor(_logger, 0, param.Expression, 0, 0, null, null, null, null, null, null, null, null, null, true, true);
			processor.CalcStatistics(param);
		}

		private static void RegExp_CalcStatistics_Single(RegExpStatisticsSingleProcessingParams param)
		{
			var processor = new RegExpLib.Processing.RegExpProcessor(param.RegExpDatabaseFilePath, param.Password, param.RegExpID, true, true) { Logger = _logger };

			processor.CalcStatisticsSingle(param);
		}

		private static void ColRegExp_CalcStatistics_Single(RegExpStatisticsSingleProcessingParams param)
		{
			var processor = new ColRegExpProcessor(param.RegExpDatabaseFilePath, param.Password, param.RegExpID, true, true) { Logger = _logger };

			processor.CalcStatisticsSingle(param);
		}

		private static void ColRegExp_CalcScores(ColRegExpStatisticsProcessingParams param)
		{
			var processor = new RegExpLib.Processing.ColRegExpProcessor(_logger, param.RegExpDatabaseFilePath, param.Password, new[] { param.ColumnID }, true, true) { Logger = _logger };
			processor.CalcScores(param);
		}

		private static void ColRegExp_Extract(ColRegExpExtractProcessingParams param)
		{
			var processor = new RegExpLib.Processing.ColRegExpProcessor(_logger, param.RegExpDatabaseFilePath, param.Password, new [] { param.ColumnID }, true, true) { Logger = _logger };
			processor.ExtractValues(param);
		}

		#endregion
	}
}