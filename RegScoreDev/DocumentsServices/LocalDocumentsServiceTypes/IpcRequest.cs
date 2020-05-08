using System;
using System.Collections.Generic;
using DocumentsServiceInterfaceLib;

using RegExpLib.Core;

namespace LocalDocumentsServiceTypes
{
	[Serializable]
	public class IpcRequest
	{
		public IpcRequestType RequestType { get; set; }
		public double DocumentID { get; set; }
        public string NoteColumnName { get; set; }
		public string ConnectionString { get; set; }
		public double[] DocumentsBlock { get; set; }
		public int TotalDocumentsCount { get; set; }
		public int Position { get; set; }
		public RegExpBase RegExp { get; set; }
		public long NavigationContextID { get; set; }
		public MatchNavigationMode NavigationMode { get; set; }
		public bool SortAscending { get; set; }
        public List<TabSetting> DocumentColumnSettings { get; set; }
        public string DynamicColumnName { get; set; }        
	}

	[Serializable]
	public enum IpcRequestType
	{
		Init = 0,
		Exit,
		GetDocumentText,
		Navigate,
		SortByCategory,
        GetDynamicColumnID,
        GetDocumentColumnCount,
        GetDocumentColumnSettings,
        SetDocumentColumnSettings,
        GetDynamicColumnDisplayName,
        DeleteAllExtraDocumentColumns
    }    
}