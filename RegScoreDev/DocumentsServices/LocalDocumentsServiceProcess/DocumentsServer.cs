using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentsServiceInterfaceLib;

using LocalDocumentsServiceTypes;

using RegExpLib.Core;
using RegExpLib.Processing;

namespace LocalDocumentsServiceProcess
{
	internal class DocumentsServer
	{
		#region Fields

		protected OleDbConnection _connection;

		protected readonly Logger _logger;

		protected List<RegExpMatchResult> _matches;

		protected long _navigationContextID;

		protected int _currentMatchIndex;        
		protected int _position;
        protected bool _unique;

        protected string _lastRegExpression;

        protected int _navigationStartPosition;

		protected double _lastDocumentID;
        protected string _lastNoteColumnName;
		protected string _lastDocumentText;

		protected string _filter;
		protected int _totalDocumentsCount;
        protected int _noteColumnCount;

		protected NamedPipeServerStream _namedPipeServer;

		protected Dictionary<int, List<string>> _cachedDocuments;

        protected Dictionary<double, Dictionary<string, string>> _cachedNoteDocuments;

		#endregion

		#region Ctors

		public DocumentsServer(Logger logger)
		{
			_logger = logger;

			_cachedDocuments = new Dictionary<int, List<string>>();
            _cachedNoteDocuments = new Dictionary<double, Dictionary<string, string>>();

            CleanupNavigationContext();
		}

		#endregion

		#region	Properties

		protected int QueryLimit => 0;

		#endregion

		#region Operations

		public ErrorCode StartServer(string pipeName)
		{
			var errorCounter = 0;

			while (true)
			{
				using (_namedPipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut))
				{
					try
					{
						_namedPipeServer.WaitForConnection();

						var formatter = (IFormatter) new BinaryFormatter();

						var request = (IpcRequest) formatter.Deserialize(_namedPipeServer);

						Console.WriteLine("Received request: " + request.RequestType);

						if (!ProcessRequest(request))
						{
							Console.WriteLine("\tExiting...");

							_namedPipeServer.Close();

							return ErrorCode.ClientRequest;
						}

						Console.WriteLine("\tSent response");
					}
					catch (Exception ex)
					{
						Console.WriteLine(String.Format("\tError {0}{1}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));

						SendErrorResponse(ex.Message, ex);

						_logger.HandleException(ex);

						errorCounter++;

						if (errorCounter > 1000)
							return ErrorCode.ErrorLimitReached;
					}
				}
			}
		}

		public void CloseConnection(bool notifyParentProcess)
		{
			try
			{
				if (_connection != null && _connection.State == ConnectionState.Open)
					_connection.Close();
			}
			finally
			{
				_connection = null;

				if (notifyParentProcess)
					SendSuccessResponse();
			}
		}

		#endregion

		#region	Implementation: request/response

		protected bool ProcessRequest(IpcRequest request)
		{
			switch (request.RequestType)
			{
				case IpcRequestType.Exit:
					CloseConnection(true);
					return false;

				case IpcRequestType.Init:
					OpenConnection(request.ConnectionString);
					break;

				case IpcRequestType.GetDocumentText:
					GetDocumentText(request.DocumentID, request.NoteColumnName);
					break;

				case IpcRequestType.Navigate:
					Navigate(request.NavigationContextID, request.NavigationMode, request.RegExp, request.TotalDocumentsCount, request.DocumentsBlock.ToList(), request.Position);
					break;

				case IpcRequestType.SortByCategory:
					SortByCategory(request.SortAscending);
					break;

                case IpcRequestType.GetDocumentColumnCount:
                    GetDocumentColumnCount();
                    break;
                case IpcRequestType.GetDynamicColumnID:
                    GetDynamicColumnID(request.DynamicColumnName);
                    break;
                case IpcRequestType.GetDocumentColumnSettings:
                    GetDocumentColumnSettings();
                    break;
                case IpcRequestType.GetDynamicColumnDisplayName:
                    GetDynamicColumnDisplayName(request.DynamicColumnName);
                    break;
                case IpcRequestType.SetDocumentColumnSettings:
                    SetDocumentColumnSettings(request.DocumentColumnSettings);
                    break;
                case IpcRequestType.DeleteAllExtraDocumentColumns:
                    DeleteAllExtraDocumentColumns();
                    break;
                default:
					SendErrorResponse("Invalid request code");
					break;
			}

			return true;
		}

		protected void SendResponse(IpcResponse response)
		{
			var formatter = (IFormatter)new BinaryFormatter();
			formatter.Serialize(_namedPipeServer, response);

			_namedPipeServer.WaitForPipeDrain();
		}

		protected void SendSuccessResponse()
		{
			SendResponse(new IpcResponse { ResponseType = IpcResponseType.Success });
		}

		protected void SendErrorResponse(string error, Exception innerException = null)
		{
			try
			{
				SendResponse(new IpcResponse { ResponseType = IpcResponseType.Error, ErrorMessage = error, Exception = innerException });
			}
			catch (Exception ex)
			{
				_logger.HandleException(ex);
			}
		}

		#endregion

		#region Implementation: navigation

		protected void CleanupNavigationContext()
		{
			_cachedDocuments.Clear();

			_navigationContextID = -1;

			_currentMatchIndex = -1;
            _position = -1;
			_unique = false;

			_navigationStartPosition = -1;
		}

		protected void Navigate(long navigationContextID, MatchNavigationMode mode, RegExpBase regExp, int totalDocsCount, List<double> documentsBlock, int position)
		{
			RegExpMatchResult result = null;

			if (navigationContextID != _navigationContextID || _lastRegExpression != regExp.BuiltExpression)
			{
				CleanupNavigationContext();

				_navigationContextID = navigationContextID;
				_navigationStartPosition = position;
                _lastRegExpression = regExp.BuiltExpression;
			}

			switch (mode)
			{
				case MatchNavigationMode.Refresh:
					SendErrorResponse("Invalid navigation mode");
					return;

				case MatchNavigationMode.NextDocument:
					result = Navigate_Document(regExp, totalDocsCount, documentsBlock, position, position, true);
					break;

				case MatchNavigationMode.PrevDocument:
					result = Navigate_Document(regExp, totalDocsCount, documentsBlock, position, position, false);
					break;

				case MatchNavigationMode.NextMatch:
					result = Navigate_Match(regExp, totalDocsCount, documentsBlock, position, position, true);
					break;

				case MatchNavigationMode.PrevMatch:
					result = Navigate_Match(regExp, totalDocsCount, documentsBlock, position, position, false);
					break;

				case MatchNavigationMode.NextUniqueMatch:
					result = Navigate_UniqueMatch(regExp, totalDocsCount, documentsBlock, position, position, true);
					break;

				case MatchNavigationMode.PrevUniqueMatch:
					result = Navigate_UniqueMatch(regExp, totalDocsCount, documentsBlock, position, position, false);
					break;
			}

			SendResponse(new IpcResponse
			{
				ResponseType = IpcResponseType.Success,
				MatchResult = result
			});
		}

		protected RegExpMatchResult Navigate_Document(RegExpBase regExp, int totalDocsCount, List<double> documents, int blockStartPosition, int position, bool forward)
		{
			var increment = forward ? 1 : -1;

			_position = position + increment;

			_unique = false;

			///////////////////////////////////////////////////////////////////////////////

			if (forward && _position >= totalDocsCount)
				_position = 0;
			else if (!forward && _position < 0)
				_position = totalDocsCount - 1;

			///////////////////////////////////////////////////////////////////////////////

			while (_position >= 0 && _position < totalDocsCount)
			{
                _matches.Clear();
                _matches = new List<RegExpMatchResult>();
                for (int columnIndex = 0; columnIndex < _noteColumnCount; columnIndex++)
                {
                    string documentText;
                    if (!GetDocumentTextByPosition(documents, blockStartPosition, _position, forward, out documentText, columnIndex))
                        return RegExpMatchResult.NeedMoreDataResult();

                    ///////////////////////////////////////////////////////////////////////////////

                    _matches.AddRange(regExp.GetFilteredMatches(documentText)
                                     .Select(x => new RegExpMatchResult(regExp, _position, x, columnIndex))
                                     .ToList());                    
                }
                if (_matches.Count > 0)
                {
                    _currentMatchIndex = 0;

                    return _matches[_currentMatchIndex];
                }
                ///////////////////////////////////////////////////////////////////////////////

                _position += increment;
			}

			///////////////////////////////////////////////////////////////////////////////

			return RegExpMatchResult.EmptyResult();
		}

		protected RegExpMatchResult Navigate_Match(RegExpBase regExp, int totalDocsCount, List<double> documentsBlock, int blockStartPosition, int position, bool forward)
		{
			var increment = forward ? 1 : -1;

			if (_position != position)
			{
				_matches = null;
				_currentMatchIndex = -1;                

                _position = position;
			}

			if (_unique)
			{
				_matches = null;
				_currentMatchIndex = -1;                

				_unique = false;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (_matches != null)
			{
				_currentMatchIndex += increment;

				if (_currentMatchIndex >= 0 && _currentMatchIndex < _matches.Count)
					return _matches[_currentMatchIndex];

				_position += increment;
			}

			if (forward && _position >= totalDocsCount)
				_position = 0;
			else if (!forward && _position < 0)
				_position = totalDocsCount - 1;

			///////////////////////////////////////////////////////////////////////////////

			while (_position >= 0 && _position < totalDocsCount)
			{                
                _matches = new List<RegExpMatchResult>();

                for (int columnIndex = 0; columnIndex < _noteColumnCount; columnIndex++)
                {
                    string documentText;
                    if (!GetDocumentTextByPosition(documentsBlock, blockStartPosition, _position, forward, out documentText, columnIndex))
                        return RegExpMatchResult.NeedMoreDataResult();

                    _matches.AddRange(regExp.GetFilteredMatches(documentText)
                                     .Select(x => new RegExpMatchResult(regExp, _position, x, columnIndex))
                                     .ToList());                   
                }
                if (_matches.Count > 0)
                {
                    _currentMatchIndex = forward ? 0 : _matches.Count - 1;

                    return _matches[_currentMatchIndex];
                }

                ///////////////////////////////////////////////////////////////////////////////

                _position += increment;
			}

			///////////////////////////////////////////////////////////////////////////////

			return RegExpMatchResult.EmptyResult();
		}

		protected RegExpMatchResult Navigate_UniqueMatch(RegExpBase regExp, int totalDocsCount, List<double> documentsBlock, int blockStartPosition, int position, bool forward)
		{
			var increment = forward ? 1 : -1;

			if (_position != position)
			{
				_matches = null;
				_currentMatchIndex = -1;

				_position = position;
				_navigationStartPosition = _position;
			}

			if (!_unique)
			{
				_matches = null;
				_currentMatchIndex = -1;

				_unique = true;
			}

			///////////////////////////////////////////////////////////////////////////////

			if (_matches != null)
			{
				if (_currentMatchIndex == -1)
					_currentMatchIndex = forward ? 0 : _matches.Count - 1;

				_currentMatchIndex += increment;

				if (_currentMatchIndex >= 0 && _currentMatchIndex < _matches.Count)
				{
					_position = _matches[_currentMatchIndex]
						.Position;

					_navigationStartPosition = _position;

					return _matches[_currentMatchIndex];
				}

				_position += increment;
			}

			if (forward && _position >= totalDocsCount || !forward && _position < 0)
			{
				_position = position;
				_navigationStartPosition = _position;

				if (_matches != null)
					_currentMatchIndex = forward ? _matches.Count - 1 : 0;

				return RegExpMatchResult.EmptyResult();
			}

			///////////////////////////////////////////////////////////////////////////////

			var comparer = new MatchEqualityComparer();

			///////////////////////////////////////////////////////////////////////////////

			while (_position >= 0 && _position < totalDocsCount)
			{
                var hasUniqueMatches = false;

                for (int columnIndex = 0; columnIndex < _noteColumnCount; columnIndex++)
                {                    
                    string documentText;
                    if (!GetDocumentTextByPosition(documentsBlock, blockStartPosition, _position, forward, out documentText, columnIndex))
                        return RegExpMatchResult.NeedMoreDataResult();
                    
                    var matches = regExp.GetFilteredMatches(documentText)
                                        .Distinct(comparer)
                                        .ToList();
                    if (matches.Any())
                    {
                        if (_matches != null)
                        {
                            foreach (var match in matches)
                            {
                                if (_matches.All(x => x.Match.Value != match.Value))
                                {
                                    if (forward)
                                        _matches.Add(new RegExpMatchResult(regExp, _position, match, columnIndex));
                                    else
                                        _matches.Insert(0, new RegExpMatchResult(regExp, _position, match, columnIndex));

                                    hasUniqueMatches = true;
                                }
                            }
                        }
                        else
                        {
                            _matches = matches.Select(x => new RegExpMatchResult(regExp, _position, x, columnIndex))
                                              .ToList();

                            hasUniqueMatches = true;
                        }

                    }
                        ///////////////////////////////////////////////////////////////////////////////

                }

                if (hasUniqueMatches)
                {
                    if (_currentMatchIndex == -1)
                    {
                        if (forward)
                            _currentMatchIndex = 0;
                        else
                            _currentMatchIndex = _matches.Count - 1;
                    }
                    else if (_currentMatchIndex < 0)
                        _currentMatchIndex = 0;
                    else if (_currentMatchIndex >= _matches.Count)
                        _currentMatchIndex = _matches.Count - 1;

                    _position = _matches[_currentMatchIndex]
                        .Position;
                    _navigationStartPosition = _position;

                    return _matches[_currentMatchIndex];
                }

                ///////////////////////////////////////////////////////////////////////////////

                _position += increment;
			}

			///////////////////////////////////////////////////////////////////////////////

			_currentMatchIndex = -1;
			_position = _navigationStartPosition;

			return RegExpMatchResult.EmptyResult();
		}

        #endregion

        #region Implementaion: get column count

        protected void GetDocumentColumnCount()
        {
            _noteColumnCount = 0;

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Documents";
            var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);


            DataTable table = reader.GetSchemaTable();
            var nameCol = table.Columns["ColumnName"];
            

            foreach (DataRow row in table.Rows)
            {
                if (row[nameCol].ToString().Contains("NOTE_TEXT"))
                    _noteColumnCount++;
                Console.WriteLine(row[nameCol]);
            }            

            SendResponse(new IpcResponse
            {
                ResponseType = IpcResponseType.Success,
                DocumentColumnCount = _noteColumnCount,                
            });            
        }
        protected bool IsColumnDynamic(string columnName)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM DynamicColumns WHERE Title=@Title";
            cmd.Parameters.AddWithValue("@Title", columnName);

            int count = (int)cmd.ExecuteScalar();
            return count != 0;
            
        }

        protected void GetDynamicColumnDisplayName(string columnName)
        {
            string displayName = columnName;
            try
            {
                var cmd = _connection.CreateCommand();
                cmd.CommandText = "SELECT [Name] FROM DocumentColumnNames WHERE DocumentColumn=@DocumentColumn";
                cmd.Parameters.AddWithValue("@DocumentColumn", columnName);

                displayName = (string)cmd.ExecuteScalar();
            } catch (Exception e)
            {
                Console.WriteLine("GetDynamicColumnDisplayName: " + e.Message);
            } finally
            {
                SendResponse(new IpcResponse
                {
                    ResponseType = IpcResponseType.Success,
                    DynamicColumnDisplayName = displayName,
                });
            }
        }

        protected void GetDynamicColumnID(string columnName)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT ID FROM DynamicColumns WHERE Title=@Title";
            cmd.Parameters.AddWithValue("@Title", columnName);

            int Id = (int)cmd.ExecuteScalar();

            SendResponse(new IpcResponse
            {
                ResponseType = IpcResponseType.Success,
                DynamicColumnId = Id,
            });

        }
        protected void GetDocumentColumnSettings()
        {

            List<TabSetting> columnSettings = new List<TabSetting>();

            _noteColumnCount = 0;

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Documents";
            var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);

            DataTable table = reader.GetSchemaTable();
            var nameCol = table.Columns["ColumnName"];

            foreach (DataRow row in table.Rows)
            {
                if (row[nameCol].ToString().StartsWith("NOTE_TEXT"))
                {
                    var noteIndex = Int32.Parse(row[nameCol].ToString().Replace("NOTE_TEXT", "0"));

                    TabSetting setting = new TabSetting();
                    setting.ColumnName = row[nameCol].ToString();
                    setting.DisplayName = "Document " + (noteIndex + 1).ToString();
                    setting.Visible = true;
                    setting.Index = noteIndex;
                    setting.Order = -1;
                    setting.Dynamic = IsColumnDynamic(setting.ColumnName);
                    setting.TabType = DocumentViewType.Normal;
                    setting.Score = false;
                    columnSettings.Add(setting);

                    _noteColumnCount ++;                                        
                }
                if (row[nameCol].ToString().StartsWith("NOTE_ENTITIES"))
                {
                    TabSetting setting = new TabSetting();
                    setting.ColumnName = row[nameCol].ToString();
                    setting.DisplayName = "Document Entities";
                    setting.Visible = false;
                    setting.Index = -1;
                    setting.Order = -1;
                    setting.Dynamic = false;
                    setting.TabType = DocumentViewType.Html_SpaCyEntityView;
                    setting.Score = false;
                    columnSettings.Add(setting);
                    
                }
                Console.WriteLine(row[nameCol]);
            }

            foreach (DataRow row1 in table.Rows)
            {
                if (row1[nameCol].ToString().StartsWith("Score"))
                {
                    var index = Int32.Parse(row1[nameCol].ToString().Replace("Score", "0"));
                    var srchResult = columnSettings.Find(x => x.Index == index);
                    srchResult.Score = true;
                }
            }

            if (_noteColumnCount == 1)
            {
                columnSettings[0].DisplayName = "Document";                
            }

            int namedColumnCount = 0;

            try
            {
                var cmdNameRead = _connection.CreateCommand();
                cmdNameRead.CommandText = "SELECT DocumentColumn, Name, Order, TabType FROM DocumentColumnNames";

                using (reader = cmdNameRead.ExecuteReader())
                {
                    while (reader.Read())
                    {                        
                        string documentColumn = reader.GetString(0);
                        string Name = reader.GetString(1);
                        int order = reader.GetInt32(2);
                        DocumentViewType type = (DocumentViewType)reader.GetInt32(3);

                        if (documentColumn.StartsWith("NOTE_TEXT") || documentColumn.StartsWith("NOTE_ENTITIES"))
                        {
                            var itemResult = columnSettings.Find(x => x.ColumnName == documentColumn);
                            if (itemResult != null)
                            {
                                itemResult.DisplayName = Name;
                                itemResult.Order = order;
                                if (order == 0)
                                    itemResult.Visible = false;
                                else itemResult.Visible = true;

                                itemResult.TabType = type;
                                namedColumnCount++;
                            }
                        }
                        else
                        {
                            TabSetting setting = new TabSetting();
                            setting.ColumnName = documentColumn;
                            setting.DisplayName = Name;
                            setting.Visible = true;
                            setting.Index = -1;
                            setting.Order = order;
                            setting.Dynamic = false;
                            setting.TabType = DocumentViewType.Browser;
                            setting.Score = false;

                            columnSettings.Add(setting);
                        }
                    }
                }
            } catch(Exception e)
            {
                Console.WriteLine("GetDocumentColumnNames: " + e.Message);             
            }
            finally
            {
                int max_ordernum = 1;
                for (int i = 0; i < _noteColumnCount; i ++)
                {
                    if (max_ordernum < columnSettings[i].Order)
                        max_ordernum = columnSettings[i].Order;
                }
                
                for (int i = 0; i < _noteColumnCount; i ++) {
                    if (columnSettings[i].Order == -1)
                        columnSettings[i].Order = ++ max_ordernum;
                }

                TabSettingCompare gg = new TabSettingCompare();

                columnSettings.Sort(gg);

                if (namedColumnCount < _noteColumnCount)
                {
                    SetDocumentColumnSettings(columnSettings, true);
                }

                SendResponse(new IpcResponse
                {
                    ResponseType = IpcResponseType.Success,
                    DocumentColumnSettings = columnSettings,
                });
            }
        }

        protected void SetDocumentColumnSettings(List<TabSetting> tabSettings, bool interCall = false)
        {
            /*
            var cmd = _connection.CreateCommand();

            cmd.CommandText = "UPDATE DocumentColumnNames SET [Order]=@Order WHERE [DocumentColumn]=@DocumentColumn";
            cmd.Parameters.AddWithValue("@Order", 199);
            cmd.Parameters.AddWithValue("@DocumentColumn", "NOTE_TEXT");
            cmd.ExecuteNonQuery();
           */
           try
            {
                var cmdClear = _connection.CreateCommand();
                cmdClear.CommandText = "DELETE * FROM DocumentColumnNames";
                cmdClear.ExecuteNonQuery();

            } catch (Exception ex)
            {
                Console.WriteLine("SetDocumentColumnSettings: " + ex.ToString());
            }
            

            foreach (var setting in tabSettings)
            {                
                try
                {
                    var cmd = _connection.CreateCommand();

                    cmd.CommandText = "INSERT INTO DocumentColumnNames ([DocumentColumn], [Name], [Order], [TabType]) Values(@DocumentColumn, @Name, @Order, @TabType)";
                    cmd.Parameters.AddWithValue("@DocumentColumn", setting.ColumnName);
                    cmd.Parameters.AddWithValue("@Name", setting.DisplayName);

                    if (setting.Visible == false)
                        cmd.Parameters.AddWithValue("@Order", 0);
                    else
                        cmd.Parameters.AddWithValue("@Order", setting.Order + 1);

                    cmd.Parameters.AddWithValue("@TabType", setting.TabType);

                    cmd.ExecuteNonQuery();

                } catch (Exception e)
                {
                    Console.WriteLine("SetDocumentColumnSettings: " + e.ToString());
                    continue;
                }
            }            

            if (interCall == false)
            {
                SendResponse(new IpcResponse
                {
                    ResponseType = IpcResponseType.Success,
                });
            }
        }
        protected void DeleteAllExtraDocumentColumns()
        {
            try
            {
                var cmdClear = _connection.CreateCommand();
                cmdClear.CommandText = "DELETE FROM DocumentColumnNames WHERE [DocumentColumn] <> 'NOTE_TEXT' and [DocumentColumn] <> 'NOTE_ENTITIES'";
                cmdClear.ExecuteNonQuery();

                for (var i = 1; i < _noteColumnCount; i ++)
                {
                    var noteColumnName = "NOTE_TEXT" + i.ToString();
                    var scoreColumnName = "Score" + i.ToString();
                    try
                    {
                        var cmdNoteColumn = _connection.CreateCommand();
                        cmdNoteColumn.CommandText = string.Format("ALTER TABLE Documents DROP [{0}]", noteColumnName);
                        cmdNoteColumn.ExecuteNonQuery();
                    } catch (Exception e)
                    {

                    }
                    try
                    {
                        var cmdScoreColumn = _connection.CreateCommand();
                        cmdScoreColumn.CommandText = string.Format("ALTER TABLE Documents DROP [{0}]", scoreColumnName);
                        cmdScoreColumn.ExecuteNonQuery();
                    } catch (Exception e)
                    {

                    }
                    try
                    {
                        var cmdDynamic = _connection.CreateCommand();
                        cmdDynamic.CommandText = string.Format("DELETE * FROM DynamicColumns WHERE [Title] = '{0}'", noteColumnName);
                        cmdDynamic.ExecuteNonQuery();
                    } catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetDocumentColumnSettings: " + ex.ToString());
            }

            SendResponse(new IpcResponse
            {
                ResponseType = IpcResponseType.Success,
            });
        }
        #endregion

        #region Implementation: get document text

        protected void GetDocumentText(double documentID, string columnName = "NOTE_TEXT")
		{   
			if (_cachedNoteDocuments.Keys.Contains(documentID) && _cachedNoteDocuments[documentID].Keys.Contains(columnName))
            {
                _lastDocumentID = documentID;
                _lastDocumentText = _cachedNoteDocuments[documentID][columnName];

                _lastNoteColumnName = columnName;

                _lastDocumentText = RegExpBase.CleanupDocumentText(_lastDocumentText);
            }
            else
            {
				var cmd = _connection.CreateCommand();

                cmd.CommandText = String.Format("SELECT {0} FROM Documents WHERE ED_ENC_NUM = @ED_ENC_NUM", columnName);                

				cmd.Parameters.AddWithValue("@ED_ENC_NUM", documentID);

				_lastDocumentText = cmd.ExecuteScalar() as string ?? string.Empty;
				_lastDocumentID = documentID;

                _lastNoteColumnName = columnName;

                _lastDocumentText = RegExpBase.CleanupDocumentText(_lastDocumentText);
			}

            SendResponse(new IpcResponse
			{
				ResponseType = IpcResponseType.Success,
				DocumentID = documentID,
				DocumentText = _lastDocumentText
			});
		}

		protected bool GetDocumentTextByPosition(List<double> documents, int blockStartPosition, int position, bool forward, out string documentText, int columnIndex = 0)
		{
            List<string> documentNotes = null;
			documentText = String.Empty;

			if (_cachedDocuments.TryGetValue(position, out documentNotes))
            {
                documentText = documentNotes[columnIndex];
                return true;
            }
				

			if (!FetchDocuments(documents, blockStartPosition, position, forward))
				return false;

            if (_cachedDocuments.TryGetValue(position, out documentNotes))
            {
                documentText = documentNotes[columnIndex];
                return true;
            }

            return false;
		}

		protected bool FetchDocuments(List<double> documentsBlock, int blockStartPosition, int position, bool forward)
		{
			if (!documentsBlock.Any())
				return false;

			_cachedDocuments.Clear();

			var cmd = _connection.CreateCommand();
            cmd.CommandText = "SELECT ED_ENC_NUM, NOTE_TEXT";
            for (int columnIndex = 1; columnIndex < _noteColumnCount; columnIndex ++)
            {                
                cmd.CommandText += ", NOTE_TEXT" + columnIndex.ToString();
            }
			cmd.CommandText += " FROM Documents WHERE ED_ENC_NUM IN (" + string.Join(",", documentsBlock.Select(x => x.ToString())) + ")";

			using (var reader = cmd.ExecuteReader())
			{
				while (reader.Read())
				{
					var documentID = reader.GetDouble(0);

                    List<string> documentNotes = new List<string>();
                    for (int columnIndex = 0; columnIndex < _noteColumnCount; columnIndex ++)
                    {
                        string documentText = string.Empty;
                        if ( !reader.IsDBNull(columnIndex + 1) )
                            documentText = reader.GetString(columnIndex + 1);

                        documentNotes.Add(documentText);                        
                    }

                    var index = documentsBlock.FindIndex(x => Math.Abs(x - documentID) < 1);
                    var documentPosition = blockStartPosition + (forward ? index : -index);

                    _cachedDocuments.Add(documentPosition, documentNotes);

                }
			}

			documentsBlock.Clear();

			return true;
		}

		protected int GetDocumentsCount(string filter)
		{
			if (filter == _filter)
				return _totalDocumentsCount;

			///////////////////////////////////////////////////////////////////////////////

			var cmd = _connection.CreateCommand();
			cmd.CommandText = "SELECT COUNT(*) FROM Documents";
			if (!string.IsNullOrEmpty(filter))
				cmd.CommandText += " WHERE " + filter;

			///////////////////////////////////////////////////////////////////////////////

			_filter = filter;
			_totalDocumentsCount = Convert.ToInt32(cmd.ExecuteScalar());

			return _totalDocumentsCount;
		}

		#endregion

		#region	Implementation

		public void SortByCategory(bool ascending)
		{
			var cmd = _connection.CreateCommand();
			cmd.CommandText = "SELECT * FROM " +
			                  "(SELECT Documents.ED_ENC_NUM, Categories.Category " +
			                  "FROM Documents " +
			                  "INNER JOIN Categories ON Documents.Category = Categories.ID " +
			                  "WHERE Documents.Category IS NOT NULL) " +
			                  "ORDER BY Category";

			var table = new DataTable();
			var adapter = new OleDbDataAdapter(cmd);
			adapter.Fill(table);

			SendResponse(new IpcResponse
			             {
				             ResponseType = IpcResponseType.Success,
				             SortedDocuments = table.Rows.Cast<DataRow>()
				                                    .Select(x => (double) x.ItemArray[0])
				                                    .ToArray()
			             });
		}

		#endregion

        protected void LoadCachedDocument ()
        {            
            var cmdSchema = _connection.CreateCommand();
            var cmdReader = _connection.CreateCommand();
            cmdSchema.CommandText = "SELECT * FROM Documents";
            cmdReader.CommandText = "SELECT [ED_ENC_NUM]";
            var reader = cmdSchema.ExecuteReader(CommandBehavior.SchemaOnly);


            DataTable table = reader.GetSchemaTable();
            var nameCol = table.Columns["ColumnName"];

            Dictionary<int, string> columeNames = new Dictionary<int, string>();
            int index = 0;

            foreach (DataRow row in table.Rows)
            {
                if (row[nameCol].ToString().Contains("NOTE_TEXT"))
                {                   
                    cmdReader.CommandText += $", [{row[nameCol]}]";

                    columeNames.Add(index, row[nameCol].ToString());
                    index++;
                }               
            }

            cmdReader.CommandText += "FROM Documents";

            using (var rowReader = cmdReader.ExecuteReader())
            {
                while (rowReader.Read())
                {
                    var documentID = rowReader.GetDouble(0);

                    if (!_cachedNoteDocuments.Keys.Contains((int)documentID))
                        _cachedNoteDocuments.Add(documentID, new Dictionary<string, string>());
                    else
                        _cachedNoteDocuments[documentID] = new Dictionary<string, string>();

                    List<string> documentNotes = new List<string>();
                    for (int i = 0; i < index; i++)
                    {
                        string documentText = string.Empty;
                        if (!rowReader.IsDBNull(i + 1))
                            documentText = rowReader.GetString(i + 1);

                        if (!_cachedNoteDocuments[documentID].Keys.Contains(columeNames[i]))
                            _cachedNoteDocuments[documentID].Add(columeNames[i], documentText);
                        else
                            _cachedNoteDocuments[documentID][columeNames[i]] = documentText;
                    }
                }
            }
        }

        #region Implementation: connection

        protected void OpenConnection(string connectionString)
		{
			_connection = new OleDbConnection(connectionString);

			try
			{
				_connection.Open();

                /*
                Task.Run(() =>
                {
                    LoadCachedDocument();
                });
                */
			}
			catch (Exception ex)
			{
				CloseConnection(true);
			}
			finally
			{
				SendSuccessResponse();
			}
		}

		#endregion
	}
    class TabSettingCompare : IComparer<TabSetting>
    {
        public int Compare(TabSetting x, TabSetting y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            // CompareTo() method 
            return x.Order.CompareTo (y.Order);

        }
    }
}