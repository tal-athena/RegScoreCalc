using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Bootstrapper.Forms
{
	using Code;

	public partial class FormMain : Form
	{
		#region Constants

		protected readonly Dictionary<string, string> _windowsFriendlyNames = new Dictionary<string, string>
		                                                                      {
			                                                                      { "5.0", "2000" },
			                                                                      { "5.1", "XP" },
			                                                                      { "5.2", "XP" },
			                                                                      { "6.0", "Vista" }
		                                                                      };

		protected readonly Version _minWindowsVersion = new Version(6, 1);
		protected readonly Version _minNetFrameworkVersion = new Version(4, 5, 2);
		protected readonly Version _minVcVersion = new Version(14, 0);
		protected readonly Version _minAnacondaVersion = new Version(2, 3, 0);

		protected const string _ace12DriverName = @"Microsoft.ACE.OLEDB.12.0";
		protected const string _ace15DriverName = @"Microsoft.ACE.OLEDB.15.0";

		protected const string _ace12SubKey = @"CLSID\{3BE786A0-0366-4F5C-9434-25CF162E475E}\InprocServer32";
		protected const string _ace15SubKey = @"CLSID\{3BE786A1-0366-4F5C-9434-25CF162E475E}\InprocServer32";

		protected const string _vcSubKey = @"SOFTWARE\Microsoft\DevDiv\VC\Servicing\14.0\RuntimeMinimum";
        protected const string _vcSubKey_2013 = @"SOFTWARE\Microsoft\DevDiv\VC\Servicing\12.0\RuntimeMinimum";

        protected const string _minWindowsSuccessMessage = "Windows version";
		protected const string _minNetFrameworkSuccessMessage = ".NET Framework version";
		protected const string _aceDriverSuccessMessage = "ACE drivers";
		protected const string _minVcSuccessMessage = "Visual C++ Runtime";
		
		protected const string _pythonSuccessMessage = "Anaconda 2.3.0 (Python 2.7)";

		protected const string _minWindowsFailMessage = "Application requires Windows 7 or higher";
		protected const string _minNetFrameworkFailMessage = "Please download and install .NET Framework 4.5.2";
		protected const string _minVc32FailMessage = "Please download and install 32-bit version of Visual C++ Runtime 2017";

		protected const string _minVc64FailMessage = "Please download and install 64-bit version of Visual C++ Runtime 2017";
		protected const string _ace32FailMessage = "Please download and install 32-bit version of Access Database Engine";
		protected const string _ace64FailMessage = "Please download and install 64-bit version of Access Database Engine";

		protected const string _python32FailMessage = "Please download and install 32-bit version of Anaconda 2.3.0 (Python 2.7) and NLTK library";
		protected const string _python64FailMessage = "Please download and install 64-bit version of Anaconda 2.3.0 (Python 2.7) and NLTK library";

		protected const string _netDownloadUrl = "https://www.microsoft.com/en-US/download/details.aspx?id=42643";
		protected const string _aceDownloadUrl = "https://www.microsoft.com/en-US/download/details.aspx?id=13255";
		protected const string _vcDownloadUrl = "https://www.microsoft.com/en-US/download/details.aspx?id=52685";

		protected const string _python32DownloadUrl = "https://repo.continuum.io/archive/Anaconda-2.3.0-Windows-x86.exe";
		protected const string _python64DownloadUrl = "https://repo.continuum.io/archive/Anaconda-2.3.0-Windows-x86_64.exe";

		protected const string _confirmCreateShortcut = "Your system meets all the requirements to run {0} version of RegScoreCalc.{1}Do you want to create desktop shortcut?";
		protected const string _confirmRunApplication = "Do you want to run RegScoreCalc now?";

		protected const string _app32BitFolder = "32Bit";
		protected const string _app64BitFolder = "64Bit";

		protected const string _executableName = "RegScoreCalc.exe";

		#endregion

		#region Fields

		protected int _normalFormWidth;
		protected int _extendedFormWidth;

		protected List<SystemRequirement> _listSystemRequirements;

		protected SynchronizationContext _context;

		#endregion

		#region Ctors

		public FormMain()
		{
			InitializeComponent();

			colLink.VisitedLinkColor = colLink.LinkColor;
			colLink.ActiveLinkColor = colLink.LinkColor;
		}

		#endregion

		#region Events

		private void FormMain_Load(object sender, EventArgs e)
		{
			try
			{
				_normalFormWidth = this.Width;
				_extendedFormWidth = (int)(this.Width * 1.80);

				_context = SynchronizationContext.Current;

				PerformCheck();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void FormMain_SizeChanged(object sender, EventArgs e)
		{
			try
			{
				CenterButtons();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnRetry_Click(object sender, EventArgs e)
		{
			try
			{
				PerformCheck();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void gridRequirements_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				if (e.ColumnIndex == colLink.Index)
				{
					var url = gridRequirements[e.ColumnIndex, e.RowIndex].Value as string;
					if (!String.IsNullOrEmpty(url))
					{
						url = url.Trim();
						if (url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
							NavigateToUrl(url);
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			try
			{
				e.Result = CheckSystemRequirements();
			}
			catch (Exception ex)
			{
				ShowInfoMessageBox(ex.Message);

				Logger.WriteErrorToLog(ex);
			}
		}

		private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				bool errorsFound = false;

				foreach (var item in _listSystemRequirements)
				{
					var index = gridRequirements.Rows.Add(item.Success ? Properties.Resources.Success : Properties.Resources.Fail, "   " + item.Message + "   ", item.Url);
					gridRequirements.AutoResizeRow(index);

					var row = gridRequirements.Rows[index];
					row.Height += 10;

					///////////////////////////////////////////////////////////////////////////////

					if (!String.IsNullOrEmpty(item.Url) && item.Url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
						errorsFound = true;
				}

				///////////////////////////////////////////////////////////////////////////////

				gridRequirements.AutoResizeColumns();
				gridRequirements.Columns[0].Width += 10;

				gridRequirements.CurrentCell = null;

				///////////////////////////////////////////////////////////////////////////////

				if (errorsFound)
					this.Width = _extendedFormWidth;
				else
					this.Width = _normalFormWidth;

				this.CenterToScreen();

				///////////////////////////////////////////////////////////////////////////////

				var result = (bool?)e.Result;
				if (result.HasValue && !errorsFound)
				{
					var appPath = GetRegScoreCalcPath(result.Value);

					if (ShowYesNoMessageBox(String.Format(_confirmCreateShortcut, FormatBitness(result.Value), Environment.NewLine + Environment.NewLine)))
					{
						var fileVersionInfo = FileVersionInfo.GetVersionInfo(appPath);

						CreateDesktopShortcut(String.Format("{0} {1}", fileVersionInfo.FileDescription, fileVersionInfo.FilePrivatePart), appPath);
					}

					if (ShowYesNoMessageBox(_confirmRunApplication))
						RunApplication(appPath);

					this.Close();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			this.Enabled = true;
			this.Cursor = Cursors.Default;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion

		#region Implementation

		protected void PerformCheck()
		{
			this.Enabled = false;
			this.Cursor = Cursors.WaitCursor;

			///////////////////////////////////////////////////////////////////////////////

			_listSystemRequirements = new List<SystemRequirement>();

			///////////////////////////////////////////////////////////////////////////////

			gridRequirements.Rows.Clear();

			worker.RunWorkerAsync();
		}

		protected bool? CheckSystemRequirements()
		{

			var currentWindowsVersion = GetWindowsVersion();
			if (currentWindowsVersion < _minWindowsVersion)
			{
				_listSystemRequirements.Add(new SystemRequirement(_minWindowsFailMessage, ""));
				return null;
			}

			_listSystemRequirements.Add(new SystemRequirement(_minWindowsSuccessMessage));

			///////////////////////////////////////////////////////////////////////////////

			var currentNetFrameworkVersion = GetNetFrameworkVersion();
			if (currentNetFrameworkVersion < _minNetFrameworkVersion)
				_listSystemRequirements.Add(new SystemRequirement(_minNetFrameworkFailMessage, _netDownloadUrl));
			else
				_listSystemRequirements.Add(new SystemRequirement(_minNetFrameworkSuccessMessage));

			///////////////////////////////////////////////////////////////////////////////

			var anacondaVersion = GetPythonVersion();
			if (anacondaVersion < _minAnacondaVersion)
			{
				if (IsWindows64Bit())
					_listSystemRequirements.Add(new SystemRequirement(_python64FailMessage, _python64DownloadUrl));
				else
					_listSystemRequirements.Add(new SystemRequirement(_python32FailMessage, _python32DownloadUrl));
			}
			else
				_listSystemRequirements.Add(new SystemRequirement(_pythonSuccessMessage));

			///////////////////////////////////////////////////////////////////////////////

			var isAceDriver64Bit = IsAce12Driver64Bit();
			if (isAceDriver64Bit != null)
			{
				var vcVersion = isAceDriver64Bit.Value ? Get64BitVcVersion() : Get32BitVcVersion();
				if (vcVersion < _minVcVersion)
					_listSystemRequirements.Add(new SystemRequirement(isAceDriver64Bit.Value ? _minVc64FailMessage : _minVc32FailMessage, _vcDownloadUrl));
				else
					_listSystemRequirements.Add(new SystemRequirement(_minVcSuccessMessage));

				///////////////////////////////////////////////////////////////////////////////

				if (isAceDriver64Bit.Value || TestConnectionUsingDriver(_ace12DriverName))
				{
					_listSystemRequirements.Add(new SystemRequirement(_aceDriverSuccessMessage));

					return isAceDriver64Bit.Value;
				}

				_listSystemRequirements.Add(new SystemRequirement(_ace32FailMessage, _aceDownloadUrl));

				return null;
			}

			///////////////////////////////////////////////////////////////////////////////

			isAceDriver64Bit = IsAce15Driver64Bit();
			if (isAceDriver64Bit != null)
			{
				var vcVersion = isAceDriver64Bit.Value ? Get64BitVcVersion() : Get32BitVcVersion();
				if (vcVersion < _minVcVersion)
					_listSystemRequirements.Add(new SystemRequirement(isAceDriver64Bit.Value ? _minVc64FailMessage : _minVc32FailMessage, _vcDownloadUrl));
				else
					_listSystemRequirements.Add(new SystemRequirement(_minVcSuccessMessage));

				///////////////////////////////////////////////////////////////////////////////

				if (isAceDriver64Bit.Value || TestConnectionUsingDriver(_ace15DriverName))
				{
					_listSystemRequirements.Add(new SystemRequirement(_aceDriverSuccessMessage));

					return isAceDriver64Bit.Value;
				}

				_listSystemRequirements.Add(new SystemRequirement(_ace32FailMessage, _aceDownloadUrl));

				return null;
			}

            
			///////////////////////////////////////////////////////////////////////////////

			_listSystemRequirements.Add(new SystemRequirement(IsWindows64Bit() ? _ace64FailMessage : _ace32FailMessage, _aceDownloadUrl));

			///////////////////////////////////////////////////////////////////////////////

			return null;
		}

		protected bool TestConnectionUsingDriver(string driverName)
		{
			if (TestConnection(driverName, Properties.Resources.AceTestDatabase)
				&& TestConnection(driverName, Properties.Resources.JetTestDatabase))
			{
				return true;
			}

			return false;
		}

		protected bool TestConnection(string driverName, byte[] databaseBytes)
		{
			var result = false;

			var tempFilePath = String.Empty;

			try
			{
				tempFilePath = Path.GetTempFileName();
				File.WriteAllBytes(tempFilePath, databaseBytes);

				///////////////////////////////////////////////////////////////////////////////

				var connectionString = String.Format("Provider={0};Data Source=\"{1}\"", driverName, tempFilePath);

				using (var connection = new OleDbConnection(connectionString))
				{
					connection.Open();

					result = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				if (!String.IsNullOrEmpty(tempFilePath))
					File.Delete(tempFilePath);
			}

			return result;
		}

		protected void CreateDesktopShortcut(string appName, string targetPath)
		{
			try
			{
				var desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
				var linkPath = GenerateLinkPath(desktopFolder, appName);

				var guid = new Guid("{72C24DD5-D70A-438B-8A42-98424B88AFB8}");
				var type = Type.GetTypeFromCLSID(guid);

				var iShell = Activator.CreateInstance(type);

				var iShortcut = type.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, null, iShell, new object[] { linkPath });
				if (iShortcut != null)
				{
					type.InvokeMember("TargetPath", BindingFlags.SetProperty, null, iShortcut, new object[] { targetPath });

					type.InvokeMember("Save", BindingFlags.InvokeMethod, null, iShortcut, null);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		protected void RunApplication(string filePath)
		{
			if (File.Exists(filePath))
				Process.Start(filePath);
			else
				ShowErrorMessageBox("Cannot find application file");
		}

		protected string GetRegScoreCalcPath(bool is64Bit)
		{
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
													 .Location);

			path = Path.Combine(path, is64Bit ? _app64BitFolder : _app32BitFolder);
			path = Path.Combine(path, _executableName);

			return path;
		}

		protected string GenerateLinkPath(string parentFolder, string appName)
		{
			string result = String.Empty;

			int counter = 0;
			while (counter <= 100)
			{
				var linkName = appName;
				if (counter > 0)
					linkName += String.Format(" ({0})", counter + 1);

				linkName += ".lnk";

				var linkPath = Path.Combine(parentFolder, linkName);
				if (!File.Exists(linkPath))
				{
					result = linkPath;
					break;
				}

				counter++;
			}

			return result;
		}

		#endregion

		#region Implementation: system info

		protected Version GetWindowsVersion()
		{
			return Environment.OSVersion.Version;
		}

		protected Version GetNetFrameworkVersion()
		{
			Version result = null;

			try
			{
				var versionsList = new List<Version>();

				using (var ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
				{
					foreach (var versionKeyName in ndpKey.GetSubKeyNames())
					{
						if (versionKeyName.StartsWith("v"))
						{
							var versionKey = ndpKey.OpenSubKey(versionKeyName);
							var version = ExtractVersion(versionKey.GetValue("Version", "") as string);

							if (version != null)
							{
								versionsList.Add(version);
								continue;
							}

							foreach (var subKeyName in versionKey.GetSubKeyNames())
							{
								var subKey = versionKey.OpenSubKey(subKeyName);
								version = ExtractVersion(subKey.GetValue("Version", "") as string);

								if (version != null)
									versionsList.Add(version);
							}
						}
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				if (versionsList.Count > 0)
				{
					foreach (var version in versionsList)
					{
						if (result == null || version > result)
							result = version;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return result;
		}


        // https://stackoverflow.com/questions/46178559/how-to-detect-if-visual-c-2017-redistributable-is-installed
        protected Version Get32BitVcVersion()
		{
			var versionString = GetWowInvariantRegistryValue(Registry.LocalMachine, _vcSubKey, "Version", RegistryHelper.RegWow64Options.KEY_WOW64_32KEY);
			if (!String.IsNullOrEmpty(versionString))
				return new Version(versionString);

			return new Version(0, 0, 0, 0);
		}

		protected Version Get64BitVcVersion()
		{
			var versionString = GetWowInvariantRegistryValue(Registry.LocalMachine, _vcSubKey, "Version", RegistryHelper.RegWow64Options.KEY_WOW64_64KEY);
			if (!String.IsNullOrEmpty(versionString))
				return new Version(versionString);

			return new Version(0, 0, 0, 0);
		}

		protected bool IsWindows64Bit()
		{
			try
			{
				if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1)
					|| Environment.OSVersion.Version.Major >= 6)
				{
					using (var process = Process.GetCurrentProcess())
					{
						bool result;
						if (!WinAPI.IsWow64Process(process.Handle, out result))
							return false;

						return result;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return false;
		}

		protected bool? IsAce12Driver64Bit()
		{
            //var path = GetRegistryValueInvariant(Registry.ClassesRoot, _ace12SubKey, "");
            var path32 = @"C:\Program Files (x86)\Common Files\Microsoft Shared\OFFICE14\ACEOLEDB.DLL";

            if (File.Exists(path32))
                return false;
            var path64 = @"C:\Program Files\Common Files\Microsoft Shared\OFFICE14\ACEOLEDB.DLL";
            if (File.Exists(path64))
                return true;

            return null;
		}

		protected bool? IsAce15Driver64Bit()
		{
			var path = GetRegistryValueInvariant(Registry.ClassesRoot, _ace15SubKey, "");
			if (File.Exists(path))
				return WinAPI.IsDll64Bit(path);

			return null;
		}

		protected Version GetPythonVersion()
		{
			var tempFile = Path.GetTempFileName();

			try
			{
				File.WriteAllBytes(tempFile, Properties.Resources.version);

				var psi = new ProcessStartInfo
				             {
					             FileName = "python.exe",
					             Arguments = tempFile,
					             UseShellExecute = false,
					             RedirectStandardOutput = true
				             };

				using (var process = Process.Start(psi))
				{
					using (var reader = process.StandardOutput)
					{
						var result = reader.ReadToEnd();
						var anaconda = "Anaconda";
						var pos = result.IndexOf(anaconda, StringComparison.InvariantCultureIgnoreCase);
						if (pos != -1)
						{
							result = result.Remove(0, pos + anaconda.Length);
							var split = result.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
							return new Version(split[0]);
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				File.Delete(tempFile);
			}

			return new Version(0, 0, 0, 0);
		}

		public string SelectFolderDialog(string initialFolder)
		{
			try
			{
				var folder = new SelectFolderParam { SelectedFolder = initialFolder };

				_context.Send(state =>
				{
					var param = (SelectFolderParam)state;

					var dlg = new FolderBrowserDialog();
					if (Directory.Exists(param.SelectedFolder))
						dlg.SelectedPath = param.SelectedFolder;

					if (dlg.ShowDialog(this) == DialogResult.OK)
						param.SelectedFolder = dlg.SelectedPath;

				}, folder);

				return folder.SelectedFolder;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			return null;
		}

		protected string GetRegistryValueInvariant(RegistryKey rootKey, string subKey, string value)
		{
			var result = GetWowInvariantRegistryValue(rootKey, subKey, value, RegistryHelper.RegWow64Options.KEY_WOW64_32KEY);
			if (String.IsNullOrEmpty(result))
				result = GetWowInvariantRegistryValue(rootKey, subKey, value, RegistryHelper.RegWow64Options.KEY_WOW64_64KEY);

			return result;
		}

		protected string GetWowInvariantRegistryValue(RegistryKey rootKey, string subKey, string value, RegistryHelper.RegWow64Options options)
		{
			string result = String.Empty;

			try
			{
				using (var key = RegistryHelper.OpenSubKey(rootKey, subKey, false, options))
				{
					result = key.GetValue(value, "") as string;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

			///////////////////////////////////////////////////////////////////////////////

			return result;
		}

		#endregion

		#region Helpers

		protected void HandleException(Exception ex)
		{
			Logger.WriteErrorToLog(ex);
		}

		protected void ShowInfoMessageBox(string message)
		{
			MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		protected void ShowErrorMessageBox(string message)
		{
			MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		protected bool ShowYesNoMessageBox(string message)
		{
			return MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes;
		}

		protected void CenterButtons()
		{
			var formCenter = this.ClientSize.Width / 2;
			var distance = btnClose.Left - btnRetry.Right;

			btnRetry.Left = (formCenter - btnRetry.Width) - distance / 2;
			btnClose.Left = formCenter + distance / 2;
		}

		protected void NavigateToUrl(string url)
		{
			Process.Start(url);
		}

		protected Version ExtractVersion(string versionString)
		{
			if (!String.IsNullOrEmpty(versionString))
			{
				var major = 0;
				var minor = 0;

				var split = versionString.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				if (split.Length >= 1)
					major = Convert.ToInt32(split.GetValue(0) as string);

				if (split.Length >= 2)
					minor = Convert.ToInt32(split.GetValue(1) as string);

				if (major > 0)
					return new Version(major, minor);
			}

			return null;
		}

		public string ResolveFriendlyWindowsName(Version version)
		{
			var versionString = version.ToString(2);

			string friendlyName;
			if (_windowsFriendlyNames.TryGetValue(versionString, out friendlyName))
				return friendlyName;

			return versionString;
		}

		public static string FormatBitness(bool? value)
		{
			if (value == null)
				return "bitness not identified";

			return value.Value ? "64-bit" : "32-bit";
		}

		#endregion
	}

	public class SystemRequirement
	{
		#region Properties

		public string Message { get; set; }
		public bool Success { get; set; }
		public string Url { get; set; }

		#endregion

		#region Ctors

		public SystemRequirement(string description)
		{
			Message = description;
			this.Url = "OK";
			Success = true;
		}

		public SystemRequirement(string description, string url)
		{
			this.Message = description;
			this.Url = url;
			this.Success = false;
		}

		#endregion
	}

	public class SelectFolderParam
	{
		#region Fields

		public string SelectedFolder { get; set; }

		#endregion
	}
}
