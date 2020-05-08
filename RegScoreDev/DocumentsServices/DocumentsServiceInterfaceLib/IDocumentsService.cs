using RegExpLib.Core;
using RegExpLib.Processing;
using System;
using System.Collections.Generic;

namespace DocumentsServiceInterfaceLib
{
	public interface IDocumentsService
	{
		#region Operations

		void OpenConnection(string connectionString, bool debug);
		void CloseConnection();
		ErrorCode Exit();
		string GetDocumentText(double documentID, string columnName = "NOTE_TEXT");
        int GetDocumentColumnCount();
        List<TabSetting> GetDocumentColumnSettings();
        RegExpMatchResult Navigate(long navigationContextID, RegExpBase regExp, MatchNavigationMode mode, int totalDocumentsCount, double[] documentsBlock, int currentPosition);
		double[] SortByCategory(bool ascending);
        bool SetDocumentColumnSettings(List<TabSetting> tabSettings);
        bool DeleteAllExtraDocumentColumns();
        int GetDynamicColumnID(string columnName);
        string GetDynamicColumnDisplayName(string columnName);
        #endregion
    }
    [Serializable]
    public class TabSetting
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public bool Visible { get; set; }
        public int Order { get; set; }
        public int Index { get; set; }
        public bool Dynamic { get; set; }
        public DocumentViewType TabType { get; set; }
        public bool Score { get; set; }
    }
    public enum DocumentViewType
    {
        Undefined = -1,
        Normal = 0,
        Browser = 1,
        Html_ListView = 2,
        Html_CalenderView = 3,
        Html_MedaCyEntityView = 4,
        Html_SpaCyEntityView = 5
    }
}