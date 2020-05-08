using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Text;

using OpenQA.Selenium;

namespace WebAppTest.Code
{
    public abstract class Action
    {
		#region Types

		public enum Result
		{
			#region Constants
			Continue,
			Stop,
			StopAndCloseBrowser
			#endregion
		}

		#endregion

		#region Fields

		protected readonly UserControl _userControl;

	    protected static bool _stopActions;

		#endregion

		#region Properties

		public string Name { get; set; }

		public XmlDocument Xml { get; set; }

        public string Message { get; set; }

		#endregion

		#region Ctors

		protected Action(UserControl userControl, string name)
	    {
		    _userControl = userControl;
		    _userControl.Tag = this;
			this.Name = name;
	    }

		#endregion

		#region Operations

	    public void SaveState()
	    {
			XmlDocument document = new XmlDocument();
			
			var node = WriteToXML(document);
		    document.AppendChild(node);

		    this.Xml = document;
	    }

		public void RestoreState()
		{
			if (this.Xml != null)
			{
				ReadFromXML(this.Xml.FirstChild);
			}
		}

		public abstract XmlNode WriteToXML(XmlDocument document);

        public abstract void ReadFromXML(XmlNode element);

	    public UserControl GetForm()
	    {
		    return _userControl;
	    }

        public virtual Result Run(IWebDriver browser, Stopwatch sw, string URL)
        {
            var retVal = Result.StopAndCloseBrowser;
            try
            {
                sw.Restart();
                retVal = this.Run2(browser, sw, URL);
                sw.Stop();
                LogTask logMsg = new LogTask
                {
                    message = "Completed. " + (String.IsNullOrEmpty(this.Message)? string.Empty : this.Message),
                    status = TaskCompletionStatus.Passed,
                    TaskName = this.Name,
                    TimeNetto = sw.Elapsed
                };
                Log.WriteLog(logMsg);
            }
            catch (TestErrorException testEx)
            {
                sw.Stop();
                LogTask logMsg = new LogTask
                {
                    message = testEx.Message,
                    status = TaskCompletionStatus.Error,
                    TaskName = this.Name,
                    TimeNetto = sw.Elapsed
                };
                Log.WriteLog(logMsg);
                retVal = testEx.result;
            }
            catch (Exception ex)
            {
                sw.Stop();
                LogTask logMsg = new LogTask
                {
                    message = ex.Message,
                    status = TaskCompletionStatus.Error,
                    TaskName = this.Name,
                    TimeNetto = sw.Elapsed
                };
                Log.WriteLog(logMsg);
            }
            
            return retVal;
        }

        public abstract Result Run2(IWebDriver browser, Stopwatch sw, string URL);

        public virtual void FocusElement(IWebDriver browser, string target_jquery_selector)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)browser;
            string js_command = "$( '" + target_jquery_selector + "' ).focus();";
            js.ExecuteScript(js_command);
        }

        public virtual void SetHeaderField(IWebDriver browser, string target_jquery_selector, string fid, string fval)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)browser;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("var xyz = $(\'" + target_jquery_selector + "\');");
            stringBuilder.Append("xyz.focus(); xyz.val('" + fval + "'); xyz.blur(); OMWClientContext.HeaderSearch.updateElementValue('" + fid + "', '" + fval + "');return true;");
            js.ExecuteScript(stringBuilder.ToString());
        }

        // CKEDITOR.instances["Story_Text"].resetDirty();
        public virtual void ResetStoryEditorDirty(IWebDriver browser)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)browser;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CKEDITOR.instances['Story_Text'].resetDirty();");
            stringBuilder.Append("return true;");
            js.ExecuteScript(stringBuilder.ToString());
        }

        public virtual void OnTestEnd()
        {

        }

        public virtual void PrepareToRun()
        {

        }

        public static void StopActions()
        {
            _stopActions = true;
        }

        public static bool IsStopped()
        {
            return _stopActions;
        }

        public static void StartActions()
        {
            _stopActions = false;
        }

        protected delegate void SetTextCallback(string text);
        protected delegate string GetTextCallback();

#endregion
    }
}
