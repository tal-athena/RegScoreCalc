
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using ScriptFunctionLibrary;

namespace Helpers
{
	public class CSScriptManager
	{
		private readonly string csScriptPrefix =
            @"  using System;
                using System.Text;
                using System.Text.RegularExpressions;
                using System.Collections.Generic;
                using System.Globalization;
                using ScriptFunctionLibrary;
                public class Scripter { 
                
                public static string Execute(string NOTE_TEXT, List<string> RegExp) {";

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
            parameters.ReferencedAssemblies.Add(typeof(ScriptFunctions).Assembly.Location);
            parameters.ReferencedAssemblies.AddRange(new[] { "System.dll" });
            

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
			if ((paramInfos.Length != 2) ||
			    (paramInfos[0].ParameterType.Name != "String") ||
                (paramInfos[1].ParameterType.Name != "List`1"))
			{
				throw new ArgumentException(
					"The Execute method must take a single string parameter.");
			}


			return "";
		}

		public string Run(string noteText, List<string> regExp)
		{
			// Make the parameter list.
			object[] methodParams = { noteText, regExp };

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

	public class CSScriptData
	{
		public CSScriptData(bool extractValuesWithScript, string script)
		{
			ExtractValuesWithScript = extractValuesWithScript;
			Script = script;
		}

		public bool ExtractValuesWithScript { get; set; }
		public string Script { get; set; }
	}    
}