using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using DocumentsServiceInterfaceLib;

using LocalDocumentsServiceTypes;

using RegExpLib.Core;
using RegExpLib.Processing;

namespace LocalDocumentsServiceLib
{
	public class DocumentsServiceClient : IDocumentsService
	{
		#region	Fields

		protected string _connectionString;
		protected Process _process;
		protected string _pipeName;
		protected NamedPipeClientStream _pipeClientStream;
		protected IFormatter _formatter;

        private object myLock = new object();

        private object baseLock = new object();
        #endregion

        #region Ctors

        public DocumentsServiceClient()
		{
			_formatter = new BinaryFormatter();
		}

		#endregion

		#region Operations: IDocumentsService

		public void OpenConnection(string connectionString, bool debug)
		{
            lock (myLock)
            {
                try
                {
                    CloseConnection();

                    _connectionString = connectionString;

                    _pipeName = "DocumentsService_" + DateTime.Now.Ticks;

                    var workingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
                                                                      .Location);

                    var filePath = Path.Combine(workingFolder, "LocalDocumentsServiceProcess.exe");

                    _process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = filePath,
                            WorkingDirectory = workingFolder,
                            CreateNoWindow = !debug,
                            UseShellExecute = false,
                            WindowStyle = debug ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                            Arguments = Process.GetCurrentProcess().Id + " " + _pipeName
                        }
                    };

                    _process.Start();

                    ///////////////////////////////////////////////////////////////////////////////

                    OpenPipe();

                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.Init,
                        ConnectionString = connectionString
                    });

                    GetResponse();
                }
                catch (Exception ex)
                {
                    _process = null;

                    CloseConnection();

                    throw new Exception("Documents service failed to initialize", ex);
                }
                finally
                {
                    ClosePipe();
                }
            }
		}

		public void CloseConnection()
		{
            lock (myLock)
            {
                ClosePipe();

                _connectionString = null;
            }
		}

		public ErrorCode Exit()
		{
            lock (myLock)
            {
                if (_process != null)
                {
                    try
                    {
                        /*OpenPipe();

                        SendRequest(new IpcRequest { RequestType = IpcRequestType.Exit });
                        GetResponse();

                        ClosePipe();

                        if (!_process.WaitForExit(1000))
                            _process.Kill();

                        return (ErrorCode) _process.ExitCode;*/
                        return ErrorCode.ClientRequest;
                    }
                    catch
                    {
                        if (!_process.HasExited)
                            _process.Kill();
                    }
                    finally
                    {
                        _process = null;
                    }
                }

                _connectionString = null;

                return ErrorCode.ClientRequest;
            }
		}

        public int GetDynamicColumnID(string columnName)
        {
            lock (myLock)
            {
                try
                {
                    OpenPipe();
                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.GetDynamicColumnID,
                        DynamicColumnName = columnName,
                    });
                    var response = GetResponse();
                    if (response.ResponseType == IpcResponseType.Success)
                        return response.DynamicColumnId;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
        }
        public string GetDynamicColumnDisplayName(string columnName)
        {
            lock (myLock)
            {
                try
                {
                    OpenPipe();
                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.GetDynamicColumnDisplayName,
                        DynamicColumnName = columnName,
                    });
                    var response = GetResponse();
                    if (response.ResponseType == IpcResponseType.Success)
                        return response.DynamicColumnDisplayName;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
        }
        public int GetDocumentColumnCount()
        {
            lock (myLock)
            {
                try
                {
                    OpenPipe();
                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.GetDocumentColumnCount,
                    });
                    var response = GetResponse();
                    if (response.ResponseType == IpcResponseType.Success)
                        return response.DocumentColumnCount;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
        }

        public List<TabSetting> GetDocumentColumnSettings()
        {
            lock (myLock)
            {
                try
                {
                    OpenPipe();
                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.GetDocumentColumnSettings,
                    });
                    var response = GetResponse();
                    if (response.ResponseType == IpcResponseType.Success)
                        return response.DocumentColumnSettings;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
        }
        public bool DeleteAllExtraDocumentColumns()
        {
            lock (myLock)
            {
                try
                {
                    OpenPipe();
                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.DeleteAllExtraDocumentColumns,                        
                    });
                    var response = GetResponse();
                    if (response.ResponseType == IpcResponseType.Success)
                        return true;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
        }
        public bool SetDocumentColumnSettings(List<TabSetting> tabSettings)
        {
            lock (myLock)
            {
                try
                {
                    OpenPipe();
                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.SetDocumentColumnSettings,
                        DocumentColumnSettings = tabSettings,
                    });
                    var response = GetResponse();
                    if (response.ResponseType == IpcResponseType.Success)
                        return true;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
        }


        public string GetDocumentText(double documentID, string columnName = "NOTE_TEXT")
		{
            lock (myLock)
            {
                try
                {
                    OpenPipe();

                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.GetDocumentText,
                        DocumentID = documentID,
                        NoteColumnName = columnName
                    });

                    var response = GetResponse();

                    ///////////////////////////////////////////////////////////////////////////////

                    if (response.ResponseType == IpcResponseType.Success)
                        return response.DocumentText;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
		}

		public RegExpMatchResult Navigate(long navigationContextID, RegExpBase regExp, MatchNavigationMode mode, int totalDocumentsCount, double[] documentsBlock, int position)
		{
            lock (myLock)
            {
                try
                {
                    OpenPipe();

                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.Navigate,
                        NavigationContextID = navigationContextID,
                        NavigationMode = mode,
                        DocumentsBlock = documentsBlock,
                        TotalDocumentsCount = totalDocumentsCount,
                        Position = position,
                        RegExp = regExp
                    });

                    var response = GetResponse();

                    ///////////////////////////////////////////////////////////////////////////////

                    if (response.ResponseType == IpcResponseType.Success)
                        return response.MatchResult;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
		}

		public double[] SortByCategory(bool ascending)
		{
            lock (myLock)
            {
                try
                {
                    OpenPipe();

                    SendRequest(new IpcRequest
                    {
                        RequestType = IpcRequestType.SortByCategory,
                        SortAscending = ascending
                    });

                    var response = GetResponse();

                    ///////////////////////////////////////////////////////////////////////////////

                    if (response.ResponseType == IpcResponseType.Success)
                        return response.SortedDocuments;

                    throw new Exception(response.ErrorMessage, response.Exception);
                }
                finally
                {
                    ClosePipe();
                }
            }
		}

		#endregion

		#region	Implementation

		protected void OpenPipe()
		{   lock (baseLock)
            {
                try
                {
                    _pipeClientStream = new NamedPipeClientStream(".", _pipeName, PipeDirection.InOut);
                    _pipeClientStream.Connect(5000);
                }
                catch (Exception ex)
                {
                    Exit();

                    throw ex;
                }
            }
		}

		protected void ClosePipe()
		{
            lock (baseLock)
            {
                if (_pipeClientStream != null)
                {
                    _pipeClientStream.Close();
                    _pipeClientStream = null;
                }
            }
		}

		protected void SendRequest(IpcRequest request)
		{
            lock (baseLock)
            {
                _formatter.Serialize(_pipeClientStream, request);

                _pipeClientStream.WaitForPipeDrain();
            }

		}

		protected IpcResponse GetResponse()
		{
            lock (baseLock)
            {
                return _formatter.Deserialize(_pipeClientStream) as IpcResponse;
            }
		}

		#endregion
	}
}
