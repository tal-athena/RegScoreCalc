using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

using WebAppTest.ActionControls;
using WebAppTest.Action_Code;
using WebAppTest.Code;
using WebAppTest.Selenium;

using Action = WebAppTest.Code.Action;

namespace WebAppTest.Forms
{
	public partial class MainForm : System.Windows.Forms.Form
	{
		#region Fields

		protected readonly ActionRunnerArgs _actionArgs;

		protected string TestFileName { get; set; }
		protected string LogFileName { get; set; }

        public string LogDir { get; set; }

		protected Action _currentAction;

		#endregion

		#region Ctors

		public MainForm(ActionRunnerArgs actionArgs)
		{
			InitializeComponent();

			this.FormClosing += MainForm_FormClosing;

			_actionArgs = actionArgs;
		}

		#endregion

		#region Events

		private void MainForm_Load(object sender, EventArgs e)
		{
			try
			{

				Log.Initialize(txtLog, this);

                if (_actionArgs != null)
                {
                    this.Visible = false;
                    notifyIcon.Visible = false;

                    txtRootURL.Text = _actionArgs.URL;
                    txtRunTimes.Value = _actionArgs.RunTimes;

                    lbSelectedActions.DisplayMember = "Name";

                    LoadTest(_actionArgs.TestFilePath);

                    PrepareToRun();

                    RunSelenium(GetActionsList());
                }
                else
                {
                    LoadSettigs();

                    LoadTestFile();

                    lbSelectedActions.DisplayMember = "Name";

                    chkScreenShots.DataBindings.Add("Checked", GlobalSettings.Instance, "SaveScreenShots");
                }
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				SaveSetttings();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnRun_Click(object sender, EventArgs e)
		{
			try
			{
				SaveSetttings();

				ShowNotification(true);

                PrepareToRun();

                //RunSelenium(GetActionList());
                backgroundWorker1.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        private void PrepareToRun()
        {
            var actions = GetActionsList();
            foreach (var action in actions)
            {
                action.PrepareToRun();
            }
        }

		private void btnStop_Click(object sender, EventArgs e)
		{
			try
			{
				Action.StopActions();
                backgroundWorker1.CancelAsync();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			try
			{
				Close();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnLoadTest_Click(object sender, EventArgs e)
		{
			try
			{
				if (openFileDialog1.ShowDialog() == DialogResult.OK)
				{
					LoadTest(openFileDialog1.FileName);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnSaveTest_Click(object sender, EventArgs e)
		{
			try
			{
				if (saveFileDialog1.ShowDialog() == DialogResult.OK)
				{
					var actionsList = GetActionsList();

					Test.SaveToXml(saveFileDialog1.FileName, actionsList);
					this.TestFileName = saveFileDialog1.FileName;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void btnClearLog_Click(object sender, EventArgs e)
		{
			try
			{
				txtLog.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void btnAddAction_Click(object sender, EventArgs e)
		{
			try
			{
				AddNewActionAfterSelection();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void btnRemoveAction_Click(object sender, EventArgs e)
		{
			try
			{
				RemoveSelectedAction();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		private void lbSelectedActions_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				SwitchToSelectedAction();
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion

		#region Implementation: general

		protected void LoadSettigs()
		{
			txtRootURL.Text = Properties.Settings.Default.RootURL;
			txtRunTimes.Value = Properties.Settings.Default.RunTimes;
		}

		protected void SaveSetttings()
		{
			Properties.Settings.Default.RootURL = txtRootURL.Text;
			Properties.Settings.Default.RunTimes = Convert.ToInt32(txtRunTimes.Value);

			Properties.Settings.Default.Save();
		}

		protected void CloseBrowserIfNeeded(IWebDriver browser, bool keepBrowser = false)
		{
			try
			{
				browser.Quit();
				browser = null;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		protected void ShowNotification(bool started)
		{
			if (_actionArgs == null)
			{
				const int timeout = 5000;

				if (started)
					notifyIcon.ShowBalloonTip(timeout, "Test started", "The test has been started", ToolTipIcon.Info);
				else
					notifyIcon.ShowBalloonTip(timeout, "Test finished", "The test has been finished", ToolTipIcon.Info);
			}
		}

		#endregion

		#region Implmenetation: action management

		protected void AddNewActionAfterSelection()
		{
			var formAddNewAction = new FormAddNewAction();
			if (formAddNewAction.ShowDialog() == DialogResult.OK)
			{
				var actionType = formAddNewAction.SelectedActionType;
				var action = (Action)Activator.CreateInstance(actionType);

				var insertIndex = lbSelectedActions.SelectedIndex != -1 ? lbSelectedActions.SelectedIndex + 1 : lbSelectedActions.Items.Count;

				lbSelectedActions.Items.Insert(insertIndex, action);
				lbSelectedActions.SelectedIndex = insertIndex;
			}
		}

		protected void RemoveSelectedAction()
		{
			if (lbSelectedActions.SelectedIndex != -1)
			{
				lbSelectedActions.Items.RemoveAt(lbSelectedActions.SelectedIndex);
			}

			if (_currentAction != null)
			{
				panelAction.Controls.Remove(_currentAction.GetForm());
				_currentAction = null;
			}

			if (lbSelectedActions.Items.Count > 0)
				lbSelectedActions.SelectedIndex = 0;
		}

		public void RemoveAllActions()
		{
			lbSelectedActions.Items.Clear();

			if (_currentAction != null)
			{
				panelAction.Controls.Remove(_currentAction.GetForm());
				_currentAction = null;
			}
		}

		public void AddActionToEndOfList(Action action)
		{
			lbSelectedActions.Items.Add(action);
		}

		protected void SwitchToSelectedAction()
		{
			if (_currentAction != null)
			{
				var currentUserControl = _currentAction.GetForm();
				_currentAction.SaveState();

				panelAction.Controls.Remove(currentUserControl);
			}

			///////////////////////////////////////////////////////////////////////////////

			var newAction = (Action)lbSelectedActions.SelectedItem;
			if (newAction != null)
			{
				var newControl = newAction.GetForm();
				newControl.Dock = DockStyle.Fill;

				panelAction.Controls.Add(newControl);

				newAction.RestoreState();

				newControl.Focus();
			}

			///////////////////////////////////////////////////////////////////////////////

			_currentAction = newAction;

			///////////////////////////////////////////////////////////////////////////////

			UpdateActionTitleLabel();
		}

		protected void UpdateActionTitleLabel()
		{
			var item = lbSelectedActions.SelectedItem as Action;
			if (item != null)
			{
				lblActionName.Text = item.Name + " action settings";
				lblActionName.Font = new Font(lblActionName.Font, FontStyle.Regular);
			}
			else
			{
				lblActionName.Text = "No action selected";
				lblActionName.Font = new Font(lblActionName.Font, FontStyle.Italic);
			}
		}

		public void SwitchToFirstAction()
		{
			if (lbSelectedActions.Items.Count > 0)
				lbSelectedActions.SelectedIndex = 0;
		}
		
		protected void LoadTest(string filePath)
		{
			var actionsList = Test.ReadFromXml(filePath);

			RemoveAllActions();

			foreach (var action in actionsList)
			{
				AddActionToEndOfList(action);
			}

			SwitchToFirstAction();
		}

		#endregion

        #region thread safe forms access

        delegate void SetTextLogCallback(string text);

        public void SetLogText(string text)
        {
            if (this.txtLog.InvokeRequired)
            {
                SetTextLogCallback d = new SetTextLogCallback(SetLogText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtLog.Text = text;
            }
        }

        private string GetLogText()
        {
            string outVal = string.Empty;
            if (this.txtLog.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    outVal = txtLog.Text;
                });
            }
            else
            {
                outVal = txtLog.Text;
            }
            return outVal;
        }

        delegate void EnablePaneActionCallback(bool enabled);

        public void EnablePaneAction(bool enabled)
        {
            if (this.panelAction.InvokeRequired)
            {
                EnablePaneActionCallback d = new EnablePaneActionCallback(EnablePaneAction);
                this.Invoke(d, new object[] { enabled });
            }
            else
            {
                this.panelAction.Enabled = enabled;
            }
        }

        delegate void SetActionIndexCallback(int index);

        public void SetPaneAction(int index)
        {
            if (this.lbSelectedActions.InvokeRequired)
            {
                SetActionIndexCallback d = new SetActionIndexCallback(SetPaneAction);
                this.Invoke(d, new object[] { index });
            }
            else
            {
                lbSelectedActions.SelectedIndex = index;
            }
        }

        

        #endregion

        #region Implementation: Selenium
		
		protected void RunSelenium(List<Action> actionsList)
		{
            SetLogText("");
            LogDir = (_actionArgs != null) ? _actionArgs.LogFilePath : GetCurrentFolder();
            Log.Reset(LogDir);

            if (txtRootURL.Text == "" && _actionArgs == null)
			{
				MessageBox.Show("Please provide root url!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			try
			{
                EnablePaneAction(false);

				///////////////////////////////////////////////////////////////////////////////

				var actionRunner = new ActionRunner(new ActionRunnerArgs
				                                    {
                                                        LogFilePath = LogDir,
					                                    URL = txtRootURL.Text,
					                                    RunTimes = Convert.ToInt32(txtRunTimes.Value)
				                                    });

				actionRunner.RunActions(this, GetActionsList());
			}
			catch (Exception ex)
			{
                LogTask logMsg = new LogTask
                {
                    message = ex.Message,
                    status = TaskCompletionStatus.Error,
                    TaskName = "MainForm class - RunSelenium",
                    TimeNetto = TimeSpan.MinValue
                };
                Log.WriteLog(logMsg);
			}
            finally
            {
                Log.Close();
            }

            EnablePaneAction(true);

			ShowNotification(false);

			if (_actionArgs != null)
				Close();
		}

        protected void RunSelenium_(List<Action> actionsList)
		{
			SetLogText("");
			Stopwatch sw = new Stopwatch();
			Stopwatch sw_internal = new Stopwatch();
			Action.StartActions();

			if (txtRootURL.Text == "")
			{
				MessageBox.Show("Please provide root url!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
            IWebDriver browser = null;
			try
			{
				browser = new FirefoxDriver();
                Thread.Sleep(5 * 1000);
				{

                    EnablePaneAction(false);

					for (int i = 0; i < txtRunTimes.Value; i++)
					{
                        if (backgroundWorker1.CancellationPending)
                            break;

						Log.WriteLog(">> Started " + (i + 1).ToString() + " iteration.", Color.Blue);
						int actionIndex = 0;

						foreach (var action in actionsList)
						{
                            if (backgroundWorker1.CancellationPending)
                                break;

							action.RestoreState();

							//Append new line
							Log.WriteLog("", Color.White);

                            SetPaneAction(actionIndex++);

                            if (GlobalSettings.Instance.SaveScreenShots)
                            {
                                string before_img_name = LogDir + "\\" + "TestScreen_.jpg".AppendTimeStamp();
							    browser.CaptureWebPageToFile(before_img_name);
							    Log.WriteLog("Before Image Name: " + before_img_name, Color.Blue);
                            }

							Action theAction = (Action)action;
							sw_internal.Restart();
							Action.Result result = theAction.Run(browser, sw, txtRootURL.Text);
							sw_internal.Stop();

                            if (GlobalSettings.Instance.SaveScreenShots)
                            {
                                string after_img_name = LogDir + "\\" + "TestScreen_.jpg".AppendTimeStamp();
							    browser.CaptureWebPageToFile(after_img_name);
							    Log.WriteLog("After Image Name: " + after_img_name, Color.Blue);
                            }

							Log.WriteLog("### Time Elapsed:" + sw_internal.Elapsed.ToString(), Color.Green);

							if (result == Action.Result.StopAndCloseBrowser)
							{
								CloseBrowserIfNeeded(browser);
								break;
							}
							else if (result == Action.Result.Stop)
								break;
							//Append new line
							Log.WriteLog("", Color.White);

							System.Threading.Thread.Sleep(1500);
						}
						//Append new line
						Log.WriteLog("", Color.White);
						//New iteration
						Log.WriteLog("@@@@@ Finished " + (i + 1).ToString() + " iteration.", Color.Blue);
						//Append new line
						Log.WriteLog("", Color.White);

						foreach (var action in actionsList)
						{
							action.OnTestEnd();
						}
					}
					Log.WriteLog("Finished running actions.", Color.Blue);
					if (!String.IsNullOrEmpty(this.TestFileName))
						Log.Dump(this.TestFileName);

					
				}
			}
			catch (Exception ex)
			{
				Log.WriteLog(ex.Message, Color.Red);
			}
            finally
            {
                if (browser != null)
                    CloseBrowserIfNeeded(browser, true);
            }

            EnablePaneAction(true);

			ShowNotification(false);
		}

		public List<Action> GetActionsList()
		{
			List<Action> actionList = lbSelectedActions.Items.Cast<Action>()
			                                           .ToList();

			return actionList;
		}

		#endregion

		#region Helpers

		public static void HandleException(Exception ex)
		{
			Log.WriteLog(ex.Message, Color.Red);
		}

		protected void LoadTestFile()
		{
			try
			{
				var filePath = @"D:\Test.xml";
				if (File.Exists(filePath))
				{
					LoadTest(filePath);
					this.TestFileName = filePath;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		
		protected string GetCurrentFolder()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly()
												 .Location);
		}

		#endregion

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            RunSelenium(GetActionsList());
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

        }

		#region Not used

		#endregion
	}
}