using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;
using RegScoreCalc.Code;
using RegScoreCalc.Code.HTML_Views;
using RegScoreCalc.Views.Models;

namespace RegScoreCalc.Panes
{
	public class PaneHtmlBase : Pane
	{
		#region Fields

		protected HtmlViewInfo _htmlViewInfo;
		protected BrowserManager _manager;
		protected bool _isBrowserInitialized;
		protected PictureBox _loadingImage;

		protected static PythonNotebookServer _pythonServer;
		protected string _baseUrl;

		#endregion

		#region Properties

		#endregion

		#region Ctors

		public PaneHtmlBase()
			: base()
		{
			_loadingImage = new PictureBox
			                {
				                SizeMode = PictureBoxSizeMode.CenterImage,
				                Dock = DockStyle.Fill,
								Image = Properties.Resources.Loading
			                };
	

			this.Controls.Add(_loadingImage);
		}

		#endregion

		#region Events

		private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
		{
			if (e.IsLoading)
				return;

			if (_isBrowserInitialized)
			{
				if (!_manager.Browser.Visible)
					_manager.Browser.Visible = true;

				if (_loadingImage.Visible)
					_loadingImage.Hide();
			}
			else
			{
				_isBrowserInitialized = true;

				LoadIndexPage();
			}
		}

		private void ribbonButton_Click(object sender, EventArgs e)
		{
			try
			{
				var button = (RibbonButton) sender;

				_manager.InvokeJs_OnCmd_Event(button.Text);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void copyNotebook_Click(object sender, EventArgs e)
		{
			try
			{
				var originalFilePath = (string)_ownerView.Argument;
				var newFileName = GetFileCopyName(originalFilePath);
				var form = new FormCopyNotebook(Path.GetFileNameWithoutExtension(newFileName));
				if (form.ShowDialog() == DialogResult.OK)
				{
					var ext = Path.GetExtension(newFileName);

					newFileName = form.FileName;

					var newFilePath = Path.Combine(Path.GetDirectoryName(originalFilePath), newFileName + ext);
					File.Copy(originalFilePath, newFilePath);

					_ownerView.Argument = newFilePath;
					_ownerView.RibbonTab.Text = newFileName;

					_manager.Browser.Load(_baseUrl + "/notebooks/" + newFileName + ext);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void copyToClipboard_Click(object sender, EventArgs e)
		{
			try
			{
				var list = new List<FolderInfo>();

				var json = BrowserManager.GetViewData(_views, "Binary Classification");
				var viewData = BinaryClassificationViewModel.FromJSON(json);
				if (viewData != null)
					list = viewData.outputFolders;

				var formOutputFolders = new FormOutputFolders(list);
				formOutputFolders.ShowDialog();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			try
			{
				base.InitPane(views, ownerView, panel, tab);

				if (!Cef.IsInitialized)
					Cef.Initialize(new CefSettings());
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected override void InitPaneCommands(RibbonTab tab)
		{
			if (_htmlViewInfo != null)
			{
				if (_htmlViewInfo.ButtonGroups != null)
				{
					foreach (var group in _htmlViewInfo.ButtonGroups)
					{
						int panelIndex;
						if (tab.Panels.Count > 1)
							panelIndex = tab.Panels.Count - 1;
						else if (tab.Panels.Count == 1)
							panelIndex = 1;
						else
							panelIndex = 0;

						var panel = new RibbonPanel(group.Title);
						tab.Panels.Insert(panelIndex, panel);

						foreach (var htmlButton in group.Buttons)
						{
							var ribbonButton = new RibbonButton(htmlButton.Text);
							ribbonButton.ToolTip = htmlButton.TooltipText;
							ribbonButton.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
							ribbonButton.Click += ribbonButton_Click;

							try
							{
								if (!String.IsNullOrEmpty(htmlButton.Icon))
								{
									var iconFullPath = Path.Combine(_htmlViewInfo.ViewFolderPath, htmlButton.Icon);
									if (File.Exists(iconFullPath))
									{
										var image = Image.FromFile(iconFullPath);

										ribbonButton.Image = image;
										ribbonButton.SmallImage = image;
									}
								}
							}
							catch (Exception ex)
							{
								MainForm.ShowExceptionMessage(ex);
							}

							panel.Items.Add(ribbonButton);
						}
					}
				}

				if (_htmlViewInfo.Type == HtmlViewType.Python)
					InitPythonCommands(tab);
			}
		}

		public override void UpdatePane()
		{
			
		}

		public override void DestroyPane()
		{
			if (_htmlViewInfo.Type == HtmlViewType.Default)
			{
				if (_manager != null)
					_manager.InvokeJs_OnBeforeClose_Event();
			}
			else if (_htmlViewInfo.Type == HtmlViewType.Python)
			{
				lock (_views.MainForm)
				{
					if (_pythonServer != null)
						_pythonServer.StopServer();
				}
			}
		}

		#endregion

		#region Operations

		public void InitHtmlPane(RibbonTab tab, HtmlViewInfo htmlViewInfo)
		{
			_htmlViewInfo = htmlViewInfo;

			InitPaneCommands(tab);

			InitBrowser();
		}

		public void Reload()
		{
			LoadIndexPage();
		}

		public void ShowDevTools()
		{
			if (_manager != null)
				_manager.Browser.ShowDevTools();
		}

		#endregion

		#region Implementation

		protected void InitBrowser()
		{
			if (_isBrowserInitialized)
				return;

			_manager = new BrowserManager(_htmlViewInfo, _views, new ChromiumWebBrowser("about:blank") { Dock = DockStyle.Fill });

			//_manager.Browser.LifeSpanHandler = new LifespanHandler();
			_manager.Browser.LoadingStateChanged += OnLoadingStateChanged;
			_manager.Browser.Parent = this;
			_manager.Browser.Visible = false;

			this.Controls.Add(_manager.Browser);
		}

		protected void InitPythonCommands(RibbonTab tab)
		{
			var panel = new RibbonPanel("Utilities");
			tab.Panels.Insert(1, panel);

			///////////////////////////////////////////////////////////////////////////////

			var btn = new RibbonButton("Copy Notebook");
			btn.ToolTip = "";
			btn.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
			btn.Click += copyNotebook_Click;
			btn.Image = Properties.Resources.copy;
			btn.SmallImage = Properties.Resources.copy;
			panel.Items.Add(btn);

			///////////////////////////////////////////////////////////////////////////////

			btn = new RibbonButton("Output Folders");
			btn.ToolTip = "";
			btn.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
			btn.Click += copyToClipboard_Click;
			btn.Image = Properties.Resources.clipboard;
			btn.SmallImage = Properties.Resources.clipboard;
			panel.Items.Add(btn);
		}

		protected void LoadIndexPage()
		{
			try
			{
				if (_htmlViewInfo != null)
				{
					if (_htmlViewInfo.Type == HtmlViewType.Default)
					{
						if (File.Exists(_htmlViewInfo.IndexHtmlPath))
							_manager.Browser.Load(_htmlViewInfo.IndexHtmlPath);
					}
					else if (_htmlViewInfo.Type == HtmlViewType.Python)
					{
						var notebooksFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Notebooks");

						lock (_views.MainForm)
						{
							if (_pythonServer == null)
								_pythonServer = new PythonNotebookServer(notebooksFolder, _views.PythonPort);

							_baseUrl = _pythonServer.StartServer(_views.AnacondaPath, _views.PythonEnv, _views.PythonVersion);
						}

						var fileName = Path.GetFileName((string) _ownerView.Argument);

                        if (_pythonServer.AccessToken == null)
						    _manager.Browser.Load(_baseUrl + "/notebooks/" + fileName);
                        else
                            _manager.Browser.Load(_baseUrl + "/notebooks/" + fileName + "?token=" + _pythonServer.AccessToken);
                    }
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		protected string GetFileCopyName(string originalFilePath)
		{
			var folder = Path.GetDirectoryName(originalFilePath);
			var fileName = Path.GetFileNameWithoutExtension(originalFilePath);
			var ext = Path.GetExtension(originalFilePath);

			var baseNewFileName = fileName;
			var counter = 1;
			const string prefix = "-Copy";

			var regex = new Regex(prefix + @"\d+$");
			var match = regex.Match(fileName);
			if (match.Success)
			{
				baseNewFileName = fileName.Remove(match.Index);
				var counterStr = fileName.Substring(match.Index + prefix.Length);
				counter = Convert.ToInt32(counterStr) + 1;
			}

			const int maxIterations = 1000;
			while (counter < maxIterations)
			{
				var newFileName = baseNewFileName + prefix + counter + ext;
				var newFilePath = Path.Combine(folder, newFileName);
				if (!File.Exists(newFilePath))
					return newFileName;

				///////////////////////////////////////////////////////////////////////////////

				counter++;
			}

			throw new Exception("Failed to create copy of the file");
		}

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PaneHtmlBase
            // 
            this.ClientSize = new System.Drawing.Size(1084, 849);
            this.Name = "PaneHtmlBase";
            this.ResumeLayout(false);

        }
    }
}
