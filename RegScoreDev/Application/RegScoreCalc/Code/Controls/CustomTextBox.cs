using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RegScoreCalc.Code.Controls
{
	public class CustomTextBox : TextBox
	{
		#region Ctors

		public CustomTextBox()
		{
			KeyDown += OnKeyDown;
		}

		#endregion

		#region Events

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				if (e.Control && e.KeyCode == Keys.A)
				{
					SelectAll();

					e.Handled = true;
					e.SuppressKeyPress = true;
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Overrides

		protected override void WndProc(ref Message m)
		{
			try
			{
				const int WM_PASTE = 0x302;

				if (m.Msg == WM_PASTE && Clipboard.ContainsText())
				{
					var chars = new[]
				        {
					        '\\', '*', '+', '?', '|', '{', '[', '(', ')', '^', '$', '.', '#', ' '
				        };

					var pasteText = Clipboard.GetText();
					if (pasteText.IndexOfAny(chars) != -1)
					{
						var dlgres = MessageBox.Show(String.Format("Clipboard text contains special characters.{0}{0}Do you wish to escape them", Environment.NewLine), "Special characters", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

						if (dlgres == DialogResult.Yes)
						{
							pasteText = Regex.Escape(pasteText);
							this.SelectedText = pasteText;
							return;
						}
						else if (dlgres == DialogResult.Cancel)
							return;
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}

			///////////////////////////////////////////////////////////////////////////////

			base.WndProc(ref m);
		}

		#endregion
	}
}
