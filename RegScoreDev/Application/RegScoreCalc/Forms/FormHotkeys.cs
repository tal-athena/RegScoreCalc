using System;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
	public partial class FormHotkeys : Form
	{
		#region Fields

		protected ViewsManager _views;

		#endregion

		#region Ctors

		public FormHotkeys(ViewsManager views)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			_views = views;
		}

		#endregion

		#region Events

		private void FormHotkeys_Load(object sender, System.EventArgs e)
		{
			try
			{
				var selectCategoryHotkey = _views.GetHotkeyByCode(PaneDocuments.HotkeyCode_SelectCategory);
				txtSelectCategory.Hotkey = selectCategoryHotkey.Hotkey;
				txtSelectCategory.HotkeyModifiers = selectCategoryHotkey.Modifiers;

				var editValueHotkey = _views.GetHotkeyByCode(PaneDocuments.HotkeyCode_EditValue);
				txtEditValue.Hotkey = editValueHotkey.Hotkey;
				txtEditValue.HotkeyModifiers = selectCategoryHotkey.Modifiers;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void FormHotkeys_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (txtSelectCategory.Hotkey == txtEditValue.Hotkey && txtSelectCategory.HotkeyModifiers == txtEditValue.HotkeyModifiers)
						throw new Exception("Same hotkey defined for more than one action");

					///////////////////////////////////////////////////////////////////////////////

					if (txtSelectCategory.Hotkey == Keys.Return && txtSelectCategory.HotkeyModifiers == Keys.Alt
						|| txtEditValue.Hotkey == Keys.Return && txtEditValue.HotkeyModifiers == Keys.Alt)
						throw new Exception("Combination '" + txtSelectCategory.Text + "' is not supported");

					///////////////////////////////////////////////////////////////////////////////

					_views.SetHotkeyByCode(PaneDocuments.HotkeyCode_SelectCategory, new HotkeyInfo { Hotkey = txtSelectCategory.Hotkey, Modifiers = txtSelectCategory.HotkeyModifiers });
					_views.SetHotkeyByCode(PaneDocuments.HotkeyCode_EditValue, new HotkeyInfo { Hotkey = txtEditValue.Hotkey, Modifiers = txtEditValue.HotkeyModifiers });

					///////////////////////////////////////////////////////////////////////////////

					_views.SaveHotkeys();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);

				e.Cancel = true;
			}
		}

		#endregion
	}
}