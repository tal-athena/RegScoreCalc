using System;
using System.Collections.Generic;
using DocumentsServiceInterfaceLib;
using RegExpLib.Processing;

namespace LocalDocumentsServiceTypes
{
	[Serializable]
	public class IpcResponse
	{
		public IpcResponse()
		{
			
		}

		public IpcResponseType ResponseType { get; set; }
		public double DocumentID { get; set; }
        public int DocumentColumnCount { get; set; }
        public List<TabSetting> DocumentColumnSettings { get; set; }
		public string DocumentText { get; set; }
		public RegExpMatchResult MatchResult { get; set; }
		public Exception Exception { get; set; }
		public string ErrorMessage { get; set; }
		public double[] SortedDocuments { get; set; }
        public int DynamicColumnId { get; set; }
        public string DynamicColumnDisplayName { get; set; }

    }

	[Serializable]
	public enum IpcResponseType
	{
		Success = 0,
		Error
	}
}