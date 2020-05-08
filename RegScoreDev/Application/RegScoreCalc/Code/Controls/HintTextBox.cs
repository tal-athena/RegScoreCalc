using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RegScoreCalc.Code.Controls
{
	public class HintTextBox : TextBox
	{
		protected Label _label;

		public HintTextBox()
		{
			CreateLabel();

			UpdateLabel();

			this.TextChanged += OnTextChanged;
			this.GotFocus += OnGotFocus;
			this.LostFocus += OnLostFocus;
		}

		private void OnGotFocus(object sender, EventArgs eventArgs)
		{
			try
			{
				UpdateLabel();
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}
		}

		private void OnLostFocus(object sender, EventArgs eventArgs)
		{
			try
			{
				UpdateLabel();
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}
		}

		private void OnTextChanged(object sender, EventArgs eventArgs)
		{
			try
			{
				UpdateLabel();
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}
		}

		protected void CreateLabel()
		{
			_label = new Label
			{
				AutoSize = false,
				TextAlign = ContentAlignment.MiddleLeft,
				Font = new Font(this.Font, FontStyle.Italic),
				ForeColor = Color.Gray,
				TabStop = true
			};

			this.Controls.Add(_label);
		}

		private void Label_OnClick(object sender, EventArgs e)
		{
			try
			{
				this.Select();
			}
			catch (Exception ex)
			{
				MainForm.ShowErrorToolTip(ex.Message);
			}
		}

		protected void UpdateLabel()
		{
			if (String.IsNullOrEmpty(this.Text) && !this.Focused)
			{
				_label.Left = 0;
				_label.Top = 1;
				_label.Width = this.ClientRectangle.Width - 2;
				_label.Height = this.ClientRectangle.Height - 4;
				_label.Text = "Type to filter";

				_label.Click += Label_OnClick;

				_label.Show();
			}
			else
			{
				_label.Hide();
			}
		}
	}
}
