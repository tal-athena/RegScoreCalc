using System;
using System.Drawing;
using System.Windows.Forms;

using FastColoredTextBoxNS;

namespace RegScoreCalc
{
    public class PaneNotesCommandsBilling : PaneCommands
    {
        #region Data members

        protected FastColoredTextBox _textBox1;
        protected FastColoredTextBox _textBox2;
        protected RibbonComboBox _cmbLineSpacing;

        #endregion

        #region Ctors

        public PaneNotesCommandsBilling(ViewsManager views, RibbonPanel panel, FastColoredTextBox textBox1, FastColoredTextBox textBox2)
            : base(views, panel)
        {
            _textBox1 = textBox1;
            _textBox2 = textBox2;

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
                    RibbonButton btn = (RibbonButton)sender;
                    int nIndex = (int)btn.Tag;

					SetLineSpacing(nIndex);
				}
			}
			catch
			{
			}
		}

        #endregion

        #region Overrides

        protected override void InitCommands(RibbonPanel panel)
        {
            base.InitCommands(panel);

            _cmbLineSpacing = new RibbonComboBox();

            panel.Items.Add(_cmbLineSpacing);

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

            panel.Items.Add(btnSelectFont);
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
			dlgFont.FixedPitchOnly = true;
			dlgFont.Font = _textBox1.Font;

			if (dlgFont.ShowDialog() == DialogResult.OK)
			{
				_textBox1.Font = dlgFont.Font;
				_textBox1.Refresh();
				
				_textBox2.Font = dlgFont.Font;
				_textBox2.Refresh();

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
					_textBox1.Font = font;
					_textBox1.Refresh();
					
					_textBox2.Font = font;
					_textBox2.Refresh();

                    RaiseDataModifiedEvent();
                }
            }
            catch { }
        }

        protected void SaveFont()
        {
            try
            {
				Font font = _textBox1.Font;

                _views.FontFamily = font.FontFamily.Name;
                _views.FontSize = font.Size;
                _views.FontStyle = font.Style;

                _views.SaveConfig();
            }
            catch { }
        }

		public void SetLineSpacing(int nIndex)
		{
			if (nIndex >= 0 && nIndex <= 3)
			{
				string strText = "";
				int interval = 5;

				switch (nIndex)
				{
					case 0:
						strText = "1";
						interval = 1;
						break;

					case 1:
						strText = "1,25";
						interval = 5;
						break;

					case 2:
						strText = "1,5";
						interval = 10;
						break;

					case 3:
						strText = "2";
						interval = 15;
						break;
				}

				if (!String.IsNullOrEmpty(strText))
					_cmbLineSpacing.TextBoxText = strText;

				_views.LineSpacing = nIndex;
				_views.SaveConfig();

				_textBox1.LineInterval = interval;
				_textBox1.Refresh();

				_textBox2.LineInterval = interval;
				_textBox2.Refresh();
			}
		}

		#endregion
    }
}