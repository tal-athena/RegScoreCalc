using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WebAppTest.Forms;
using Action = WebAppTest.Code.Action;

using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace WebAppTest.Selenium
{
	public class ActionRunner
	{
		#region Fields

		protected ActionRunnerArgs _args;

		#endregion

		#region Ctors

		public ActionRunner(ActionRunnerArgs args)
		{
			_args = args;
		}

		#endregion

		#region Operations



		#endregion

		#region Implementation

        public void RunActions(MainForm form, List<Action> actionsList)
		{
            RunSelenium(form, actionsList);
		}

        protected void RunSelenium(MainForm form, List<Action> actionsList)
        {
            form.SetLogText("");
            Stopwatch sw = new Stopwatch();
            Stopwatch sw_internal = new Stopwatch();
            Action.StartActions();

            IWebDriver browser = null;
            try
            {

                // READ! !Make sure to add the path to where you extracting the chromedriver.exe:
                // If missing - download from here: http://chromedriver.chromium.org/downloads
                browser = new ChromeDriver(@"E:\RegScore\RegScoreCalc-Ding\RegScoreDev-branch\WebAppTest\chromedriver_win32");
                
                Thread.Sleep(5 * 1000);
                {

                    form.EnablePaneAction(false);

                    for (int i = 0; i < _args.RunTimes; i++)
                    {
                        Log.WriteLog(">>Started " + (i + 1).ToString() + " iteration.", Color.Blue);
                        int actionIndex = 0;

                        foreach (var action in actionsList)
                        {
                            action.RestoreState();

                            //Append new line
                            Log.WriteLog("", Color.White);

                            form.SetPaneAction(actionIndex++);

                            if (GlobalSettings.Instance.SaveScreenShots)
                            {
                                string before_img_name = System.Environment.CurrentDirectory + "\\" + "TestScreen_.jpg".AppendTimeStamp();
                                browser.CaptureWebPageToFile(before_img_name);
                                Log.WriteLog("Before Image Name: " + before_img_name, Color.Blue);
                            }

                            Action theAction = (Action)action;
                            sw_internal.Restart();
                            Action.Result result = theAction.Run(browser, sw, _args.URL);
                            sw_internal.Stop();

                            if (GlobalSettings.Instance.SaveScreenShots)
                            {
                                string after_img_name = System.Environment.CurrentDirectory + "\\" + "TestScreen_.jpg".AppendTimeStamp();
                                browser.CaptureWebPageToFile(after_img_name);
                                Log.WriteLog("After Image Name: " + after_img_name, Color.Blue);
                            }

                            if (result == Action.Result.StopAndCloseBrowser)
                            {
                                CloseBrowserIfNeeded(browser);
                                break;
                            }
                            else if (result == Action.Result.Stop)
                                break;

                            System.Threading.Thread.Sleep(1500);
                        }
                        //New iteration
                        Log.WriteLog(">>Finished " + (i + 1).ToString() + " iteration.", Color.Blue);
                        //Append new line
                        Log.WriteLog("", Color.White);

                        foreach (var action in actionsList)
                        {
                            action.OnTestEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTask logMsg = new LogTask
                {
                    message = ex.Message,
                    status = TaskCompletionStatus.Error,
                    TaskName = "ActionRunner class",
                    TimeNetto = sw.Elapsed
                };
                Log.WriteLog(logMsg);
            }
            finally
            {
                if (browser != null)
                    CloseBrowserIfNeeded(browser, true);
            }
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
                LogTask logMsg = new LogTask
                {
                    message = ex.Message,
                    status = TaskCompletionStatus.Error,
                    TaskName = "ActionRunner class - CloseBrowserIfNeeded",
                    TimeNetto = TimeSpan.MinValue
                };
                Log.WriteLog(logMsg);
			}
		}

		#endregion
	}

	public class ActionRunnerArgs
	{
		#region Fields

		public string URL { get; set; }
		public int RunTimes { get; set; }
		public string LogFilePath { get; set; }
		public string TestFilePath { get; set; }

		#endregion
	}
}
