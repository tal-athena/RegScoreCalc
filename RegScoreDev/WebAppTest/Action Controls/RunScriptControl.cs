using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using WebAppTest.Code;
using WebAppTest.Forms;

namespace WebAppTest.Action_Controls
{
	public partial class RunScriptControl : UserControl
	{
		#region Ctors

		public RunScriptControl()
		{
			InitializeComponent();

			btnSaveScript.Enabled = false;
			btnSaveScriptAs.Enabled = false;
		}

		#endregion

		#region Events

		private void btnLoadScript_Click(object sender, System.EventArgs e)
		{
			try
			{
				LoadScript();
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		private void btnSaveScriptAs_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveScriptAs();
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		private void btnSaveScript_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveScript();
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		private void txtScript_TextChanged(object sender, EventArgs e)
		{
			try
			{
				var enableSave = !String.IsNullOrEmpty(txtScript.Text);

				btnSaveScript.Enabled = enableSave;
				btnSaveScriptAs.Enabled = enableSave;
				btnCompile.Enabled = enableSave;
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		private void btnCompile_Click(object sender, EventArgs e)
		{
			try
			{
				CompileScript();
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		#endregion

		#region Implementation

		protected void LoadScript()
		{
			var dlgOpenFile = new OpenFileDialog
			{
				
			};

			if (dlgOpenFile.ShowDialog() == DialogResult.OK)
			{
				txtScript.Text = File.ReadAllText(dlgOpenFile.FileName);
			}
		}

		protected void SaveScript()
		{
			File.WriteAllText(GetScriptFilePath(), txtScript.Text);
		}

		protected void SaveScriptAs()
		{
			var dlgSaveFile = new SaveFileDialog
			                  {

			                  };

			if (dlgSaveFile.ShowDialog() == DialogResult.OK)
				File.WriteAllText(dlgSaveFile.FileName, txtScript.Text);
		}

		protected string GetScriptFilePath()
		{
			var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
			                                           .Location);

			var filePath = Path.Combine(folder, "Script.cs");

			return filePath;
		}

		protected void CompileScript()
		{
			if (!String.IsNullOrEmpty(txtScript.Text))
			{
				var scriptManager = new ScriptManager();
				var errors = scriptManager.Compile(txtScript.Text);
				if (String.IsNullOrEmpty(errors))
				{
					Log.WriteLog("Compilation succeeded", Color.Green);
				}
				else
				{
					Log.WriteLog(errors, Color.Red);
				}
			}
		}

		#endregion

        #region Thread safe access

        public string GetScript()
        {
            string outVal = string.Empty;
            if (this.txtScript.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate()
                {
                    outVal = txtScript.Text;
                });
            }
            else
            {
                outVal = txtScript.Text;
            }
            return outVal;
        }

        delegate void SetScriptCallback(string text);

        public void SetScript(string text)
        {
            if (this.txtScript.InvokeRequired)
            {
                SetScriptCallback d = new SetScriptCallback(SetScript);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtScript.Text = text;
            }
        }

        #endregion
    }
}