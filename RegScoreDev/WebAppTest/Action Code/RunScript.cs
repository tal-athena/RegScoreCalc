using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using OpenQA.Selenium;

using WebAppTest.Action_Controls;
using WebAppTest.Code;

using Action = WebAppTest.Code.Action;

namespace WebAppTest.Action_Code
{
    class RunScript : Action
    {
        public RunScript() : base(new RunScriptControl(), "Run Script")
        {
            
        }

        public override XmlNode WriteToXML(XmlDocument document)
        {
			XmlNode node = document.CreateElement("RunScript");

            var txt = ((RunScriptControl)_userControl).GetScript();

			XmlNode scriptNode = document.CreateElement("ScriptCode");

			XmlNode scriptValueNode = document.CreateNode(XmlNodeType.Text, "", "");
			scriptValueNode.Value = txt;
			scriptNode.AppendChild(scriptValueNode);

			node.AppendChild(scriptNode);

			return node;
        }

        public override void ReadFromXML(XmlNode element)
        {
			var txt = (RichTextBox)_userControl.Controls.Find("txtScript", true).FirstOrDefault();

            var e = element.SelectSingleNode("ScriptCode").FirstChild;

			string scriptCode = (e != null)? e.Value : string.Empty;

            ((RunScriptControl)_userControl).SetScript(scriptCode);
        }

        private string script { get; set; }
        public override void PrepareToRun()
        {
            script = ((RunScriptControl)_userControl).GetScript();
        }

		public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
        {
            System.Threading.Thread.Sleep(1000);
            if (!String.IsNullOrEmpty(script))
			{
				var scriptManager = new ScriptManager();
                var errors = scriptManager.Compile(script);
				if (String.IsNullOrEmpty(errors))
				{
					var result = scriptManager.Run(browser, sw, URL);
					if (!String.IsNullOrEmpty(result))
                    {
						Log.WriteLog("Script returned: " + result, Color.Blue);
                        this.Message = result;
                    }
					else
						Log.WriteLog("Script returned no result", Color.Blue);
				}
				else
				{
					Log.WriteLog(errors, Color.Red);
				}
			}

			return Action.Result.Continue;
		}
    }
}
