using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;

using OpenQA.Selenium;

namespace WebAppTest.Code
{
	public class ScriptManager
	{
		private readonly string csScriptPrefix =
            @"  using System;
                using System.Text;
                using System.Text.RegularExpressions;
                using System.Collections.Generic;
                using System.Globalization;         
				using System.Diagnostics;
                using System.Threading;
                using OpenQA.Selenium;
                using OpenQA.Selenium.Chrome;
                using OpenQA.Selenium.Firefox;
                using OpenQA.Selenium.Support;
                using OpenQA.Selenium.Support.UI;
                
                public static class WebDriverEx
                {
                    public static void CaptureWebPageToFile(this OpenQA.Selenium.IWebDriver browser, string filePath)
                    {
                        Screenshot ss = ((ITakesScreenshot)browser).GetScreenshot();
                        ss.SaveAsFile(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    public static void WaitForPageLoad(this IWebDriver driver)
                    {
                        var wait =
                            new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30.00));

                        wait.Until(driver1 => ((IJavaScriptExecutor)driver).ExecuteScript(""return document.readyState"").Equals(""complete""));
                    }

                    public static void WaitForJqueryAjax(this OpenQA.Selenium.IWebDriver driver)
                    {
                        while (true) // Handle timeout somewhere
                        {
                            Thread.Sleep(100);
                            var ajaxIsComplete = (bool)(driver as IJavaScriptExecutor).ExecuteScript(""return jQuery.active == 0"");
                            if (ajaxIsComplete)
                                break;
                        }
                    }

                    public static void WaitForElement(this OpenQA.Selenium.IWebDriver driver, OpenQA.Selenium.By by, int timeoutInSeconds)
                    {
                        new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds)).Until(ExpectedConditions.ElementExists(by));
                    }

                    public static bool IsTextPresent(this OpenQA.Selenium.IWebDriver driver, string text)
                    {
                        var bodyTag = driver.FindElement(By.TagName(""body""));
                        return bodyTag.Text.Contains(text);
                    }
                }
                

				public class Scripter { 
                
                public static string Execute(OpenQA.Selenium.IWebDriver browser, Stopwatch sw, string URL) {";

		private readonly string csScriptSuffix = "}}";
		private string _code = "";
		private MethodInfo methodInfo;

		public string Compile(string code)
		{
			_code = code;

			// Make a C# code provider.
			var codeProvider = CodeDomProvider.CreateProvider("C#");
			// Generate a non-executable assembly in memory.
			var parameters = new CompilerParameters();
			parameters.GenerateInMemory = true;
			parameters.GenerateExecutable = false;
            parameters.ReferencedAssemblies.AddRange(new[] { "System.dll", "system.drawing.dll", "WebDriver.dll", "WebDriver.Support.dll" });

			// Compile the code.
			var results =
				codeProvider.CompileAssemblyFromSource(parameters, csScriptPrefix + code + csScriptSuffix);

			// See if there are errors.
			if (results.Errors.Count > 0)
			{
				var errors = "";
				foreach (CompilerError error in results.Errors)
				{
					errors +=
						"Error:\r\n" +
						"    Line: " + error.Line + "\r\n" +
						"    Error Number: " + error.ErrorNumber + "\r\n" +
						"    Text: " + error.ErrorText + "\r\n";
				}

				return errors;
			}

			// Get the Scripter class.
			var scripterType = results.CompiledAssembly.GetType("Scripter");
			if (scripterType == null)
				throw new MissingMethodException("Cannot find class Scripter");

			// Get a MethodInfo object describing the Execute method.
			methodInfo = scripterType.GetMethod("Execute");
			if (methodInfo == null)
			{
				throw new MissingMethodException(
					"Cannot find method Execute");
			}

			// Make sure the method takes a single string as a parameter.
			var paramInfos = methodInfo.GetParameters();
			if (paramInfos.Length != 3)
			{
				throw new ArgumentException(
					"The Execute method must take 3 parameters");
			}

			return "";
		}

		public string Run(IWebDriver browser, Stopwatch sw, string URL)
		{
			// Make the parameter list.
			object[] methodParams = { browser, sw, URL };

			// Execute the method.
			var output = methodInfo.Invoke(null, methodParams);

			return output.ToString();
		}

		public bool IsCodeChanged(string newCode)
		{
			return !string.Equals(_code, newCode, StringComparison.CurrentCulture);
		}

		public bool IsCodeCompiled()
		{
			if (methodInfo != null)
				return true;
			return false;
		}
	}
}