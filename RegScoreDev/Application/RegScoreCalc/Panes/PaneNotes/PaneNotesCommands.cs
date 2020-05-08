using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RegScoreCalc
{
	/*public class PaneNotesCommands : PaneCommands
	{
		#region Data members

		protected RichTextBox _richNotes;
		protected RibbonComboBox _cmbLineSpacing;

		#endregion

		#region Ctors

		public PaneNotesCommands(ViewsManager views, RibbonPanel _panel, RichTextBox richNotes) : base(views, _panel)
		{
			_richNotes = richNotes;

			LoadFont();
			SetLineSpacing(_views.LineSpacing);
		}

		#endregion

		#region Events

		protected void OnSelectFont_Clicked(object sender, EventArgs e)
		{
			SelectFont();
		}

		protected void OnLineSpacingItem_Clicked(object sender, EventArgs e)
		{
			try
			{
				if (sender is RibbonButton)
				{
					RibbonButton btn = (RibbonButton) sender;
					int nIndex = (int) btn.Tag;

					SetLineSpacing(nIndex);

					_views.LineSpacing = nIndex;
					_views.SaveConfig();
				}
			}
			catch
			{
			}
		}

		#endregion

		#region Overrides

		protected override void InitCommands(RibbonPanel _panel)
		{
			base.InitCommands(_panel);

			_cmbLineSpacing = new RibbonComboBox();

			_panel.Items.Add(_cmbLineSpacing);

			_cmbLineSpacing.Text = "Line spacing:";
			_cmbLineSpacing.TextBoxWidth = 50;
			_cmbLineSpacing.AllowTextEdit = false;

			InsertLineSpacingItem("1", 0);
			InsertLineSpacingItem("1,25", 1);
			InsertLineSpacingItem("1,5", 2);
			InsertLineSpacingItem("2", 3);

			_cmbLineSpacing.TextBoxText = "1";

			//////////////////////////////////////////////////////////////////////////

			RibbonButton btnSelectFont = new RibbonButton("Select Font");

			_panel.Items.Add(btnSelectFont);
			btnSelectFont.Image = Properties.Resources.SelectNotesFont;
			btnSelectFont.SmallImage = Properties.Resources.SelectNotesFont;
			btnSelectFont.Click += new EventHandler(OnSelectFont_Clicked);
			btnSelectFont.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
		}

		#endregion

		#region Implementation

		protected void InsertLineSpacingItem(string strItem, int nItem)
		{
			RibbonButton btnItem = new RibbonButton(strItem);
			_cmbLineSpacing.DropDownItems.Add(btnItem);
			btnItem.Tag = nItem;
			btnItem.Click += new EventHandler(OnLineSpacingItem_Clicked);
		}

		protected void SelectFont()
		{
			FontDialog dlgFont = new FontDialog();
			dlgFont.Font = _richNotes.Font;

			if (dlgFont.ShowDialog() == DialogResult.OK)
			{
				_richNotes.Font = dlgFont.Font;

				SaveFont();

				RaiseDataModifiedEvent();
			}
		}

		protected void LoadFont()
		{
			try
			{
				Font font = new Font(_views.FontFamily, _views.FontSize, _views.FontStyle);
				if (font != null)
				{
					_richNotes.Font = font;

					RaiseDataModifiedEvent();
				}
			}
			catch
			{
			}
		}

		protected void SaveFont()
		{
			try
			{
				Font font = _richNotes.Font;

				_views.FontFamily = font.FontFamily.Name;
				_views.FontSize = font.Size;
				_views.FontStyle = font.Style;

				_views.SaveConfig();
			}
			catch
			{
			}
		}

		public void SetLineSpacing(int nIndex)
		{
			if (nIndex >= 0 && nIndex <= 3)
			{
				string strText = "";

				switch (nIndex)
				{
					case 0:
						_SetLineSpacing(0, 0);
						strText = "1";
						break;

					case 1:
						_SetLineSpacing(5, 25);
						strText = "1,25";
						break;

					case 2:
						_SetLineSpacing(1, 0);
						strText = "1,5";
						break;

					case 3:
						_SetLineSpacing(2, 0);
						strText = "2";
						break;
				}

				if (!String.IsNullOrEmpty(strText))
					_cmbLineSpacing.TextBoxText = strText;

				_views.LineSpacing = nIndex;
				_views.SaveConfig();
			}
		}

		#endregion

		#region RTF

		protected void _SetLineSpacing(byte rule, int space)
		{
			PARAFORMAT fmt = new PARAFORMAT();
			fmt.cbSize = Marshal.SizeOf(fmt);
			fmt.dwMask = PFM_LINESPACING;
			fmt.dyLineSpacing = space;
			fmt.bLineSpacingRule = rule;
			_richNotes.SelectAll();
			SendMessage(new HandleRef(_richNotes, _richNotes.Handle), EM_SETPARAFORMAT, SCF_SELECTION, ref fmt);

			_richNotes.SelectionStart = 0;
			_richNotes.SelectionLength = 0;
		}

		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, ref PARAFORMAT lParam);

		const int PFM_SPACEBEFORE = 0x00000040;
		const int PFM_SPACEAFTER = 0x00000080;
		const int PFM_LINESPACING = 0x00000100;

		const int SCF_SELECTION = 1;
		const int EM_SETPARAFORMAT = 1095;

		[StructLayout(LayoutKind.Sequential)]
		public struct PARAFORMAT
		{
			public int cbSize;
			public uint dwMask;
			public short wNumbering;
			public short wReserved;
			public int dxStartIndent;
			public int dxRightIndent;
			public int dxOffset;
			public short wAlignment;
			public short cTabCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public int[] rgxTabs;

			// PARAFORMAT2 from here onwards
			public int dySpaceBefore;
			public int dySpaceAfter;
			public int dyLineSpacing;
			public short sStyle;
			public byte bLineSpacingRule;
			public byte bOutlineLevel;
			public short wShadingWeight;
			public short wShadingStyle;
			public short wNumberingStart;
			public short wNumberingStyle;
			public short wNumberingTab;
			public short wBorderSpace;
			public short wBorderWidth;
			public short wBorders;
		}

		#endregion
	}*/
}