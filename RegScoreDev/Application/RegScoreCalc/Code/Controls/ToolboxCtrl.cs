using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Html;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RegScoreCalc.Controls
{
	public class ToolboxCtrl : ListView
	{
		#region Items

		ListViewGroup listViewGroup5 = new ListViewGroup("Character Class", HorizontalAlignment.Left);
		ListViewGroup listViewGroup6 = new ListViewGroup("Sets", HorizontalAlignment.Left);
		ListViewGroup listViewGroup7 = new ListViewGroup("Repetitions", HorizontalAlignment.Left);
		ListViewGroup listViewGroup9 = new ListViewGroup("Apply to selected", HorizontalAlignment.Left);
		ListViewGroup listViewGroup8 = new ListViewGroup("Other", HorizontalAlignment.Left);

        ListViewItem listViewItem13 = new ListViewItem("Any character");
        ListViewItem listViewItem14 = new ListViewItem("Any word character");
		ListViewItem listViewItem15 = new ListViewItem("Any digit");
		ListViewItem listViewItem16 = new ListViewItem("Any whitespace");
		ListViewItem listViewItem17 = new ListViewItem("[a-zA-Z]");
		ListViewItem listViewItem18 = new ListViewItem("[0-9]");
		ListViewItem listViewItem19 = new ListViewItem("Optional (either once or none)");
		ListViewItem listViewItem20 = new ListViewItem("One or more times");
		ListViewItem listViewItem21 = new ListViewItem("Zero or more times");
		ListViewItem listViewItem22 = new ListViewItem("Either or");
		ListViewItem listViewItem23 = new ListViewItem("Range");
		ListViewItem listViewItem27 = new ListViewItem("Grouping");
		ListViewItem listViewItem24 = new ListViewItem("First or last character in word");
		ListViewItem listViewItem25 = new ListViewItem("Word boundaries");
		ListViewItem listViewItem26 = new ListViewItem("Will write my own");

		#endregion

		#region Fields

		protected TextBox _buddy;

		private ColumnHeader headerDefault;
		private ImageList imageList;

		protected int _lviHoverIndex;

		protected HtmlToolTip _toolTip;

		protected RibbonProfesionalRendererColorTable _colorTable;

		protected bool _bPressed;

		protected Brush _backBrush;
		protected Brush _textBrush;
		protected Brush _disabledTextBrush;

		protected Pen _penHeader;

		protected Image _image;

		protected Dictionary<string, string> _help;

		protected string _styles;

		#endregion

		#region Ctors

		public ToolboxCtrl()
		{
			InitControl();

			_lviHoverIndex = -1;

			_toolTip = new HtmlToolTip();

			///////////////////////////////////////////////////////////////////////////////

			_colorTable = new RibbonProfesionalRendererColorTable();

			_backBrush = new SolidBrush(MainForm.ColorBackground);
			_textBrush = new SolidBrush(_colorTable.Text);
			_disabledTextBrush = new SolidBrush(SystemColors.GrayText);

			_penHeader = Pens.LightGray;

			_image = Properties.Resources.help;

			///////////////////////////////////////////////////////////////////////////////

			LoadHelp(GetDefaultHelpFilePath());
		}

		private void OnToolTipOnPopup(object s, PopupEventArgs e)
		{
			var widthMul = e.ToolTipSize.Width / 400;

			e.ToolTipSize = new Size(200, e.ToolTipSize.Height * widthMul);
		}

		#endregion

		#region Events

		private void this_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			try
			{
				/*var dlg = new OpenFileDialog
						  {
							  Filter = "*.html|*.html|*.*|*.*",
							  InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly()
																			   .Location)
						  };

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					LoadHelp(dlg.FileName);

					this.Invalidate();
					this.Update();
				}*/
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_MouseClick(object sender, MouseEventArgs e)
		{
			try
			{
				if (e.X < 20 || e.X > this.ClientSize.Width - 20)
					return;

				var lvi = this.GetItemAt(e.X, e.Y);
				if (lvi != null)
				{
					this.Invalidate();
					this.Update();

					///////////////////////////////////////////////////////////////////////////////

					InsertRegExp(lvi);
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_MouseEnter(object sender, EventArgs e)
		{
			try
			{
				//HideTooltip();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_MouseMove(object sender, MouseEventArgs e)
		{
			try
			{
				var redraw = false;

				if (e.X >= 20 && e.X <= this.ClientSize.Width - 20)
				{
					var lvi = this.GetItemAt(e.X, e.Y);
					if (lvi != null)
					{
						if (lvi.Index != _lviHoverIndex)
						{
							HideTooltip();

							_lviHoverIndex = lvi.Index;
							_bPressed = false;

							redraw = true;
						}

						ShowTooltip(lvi, e.Location);
					}
					else
					{
						HideTooltip();

						_lviHoverIndex = -1;
						_bPressed = false;

						redraw = true;
					}
				}
				else
				{
					HideTooltip();

					_lviHoverIndex = -1;
					redraw = true;
				}

				if (redraw)
				{
					this.Invalidate();
					this.Update();
				}
			}
			catch
			{
			}
		}

		private void this_MouseLeave(object sender, EventArgs e)
		{
			try
			{
				_lviHoverIndex = -1;
				_bPressed = false;

				this.Invalidate();
				this.Update();

				HideTooltip();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_MouseDown(object sender, MouseEventArgs e)
		{
			try
			{
				_bPressed = false;

				if (_lviHoverIndex != -1)
				{
					_bPressed = e.Button == MouseButtons.Left;

					this.Invalidate();
					this.Update();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_MouseUp(object sender, MouseEventArgs e)
		{
			try
			{
				if (_bPressed)
				{
					_bPressed = false;

					this.Invalidate();
					this.Update();
				}
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_SizeChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateHeaderWidth();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			try
			{
				e.Cancel = true;
				e.NewWidth = this.Columns[e.ColumnIndex].Width;
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void this_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			try
			{
				DrawBackground(e);

				DrawImage(e);

				DrawText(e);
			}
			catch
			{
			}
		}

		private void this_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
		{
			try
			{
				e.Graphics.FillRectangle(_backBrush, e.Bounds);
				e.Graphics.DrawLine(_penHeader, e.Bounds.X, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

				var sf = new StringFormat
						 {
							 Alignment = StringAlignment.Center,
							 LineAlignment = StringAlignment.Center
						 };

				e.Graphics.DrawString(e.Header.Text, new Font(e.Font.FontFamily, 12f), Brushes.Black, e.Bounds, sf);
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Operations

		public void SetBuddy(TextBox txt)
		{
			_buddy = txt;

			SendMessage(this.Handle, LVM_SETHOTCURSOR, IntPtr.Zero, Cursors.Arrow.Handle);
		}

		public void UpdateHeaderWidth()
		{
			headerDefault.Width = this.ClientSize.Width;
		}

		public int GetQuickActionsMaximumTextWidth()
		{
			var result = this.ClientSize.Width * 2;

			try
			{
				var longestString = String.Empty;
				foreach (var lvi in this.Items.Cast<ListViewItem>())
				{
					if (lvi != null && !String.IsNullOrEmpty(lvi.Text))
					{
						if (lvi.Text.Length > longestString.Length)
							longestString = lvi.Text;
					}
				}

				if (!String.IsNullOrEmpty(longestString))
				{
					using (var g = this.CreateGraphics())
					{
						var size = g.MeasureString(longestString, this.Font, 500);
						result = (int)size.Width + 80 + 20;
					}
				}
			}
			catch
			{
			}

			headerDefault.Width = result;

			return result;
		}

		#endregion

		#region Implementation

		protected void InitControl()
		{
			imageList = new ImageList
						{
							ColorDepth = ColorDepth.Depth8Bit,
							ImageSize = new Size(28, 28),
							TransparentColor = Color.Transparent
						};

			///////////////////////////////////////////////////////////////////////////////

			headerDefault = new ColumnHeader();

			///////////////////////////////////////////////////////////////////////////////

			this.Activation = ItemActivation.TwoClick;
			this.BorderStyle = BorderStyle.FixedSingle;
			this.HeaderStyle = ColumnHeaderStyle.Nonclickable;
			this.FullRowSelect = true;
			this.GridLines = true;
			this.StateImageList = this.imageList;
			this.Margin = new Padding(0);
			this.MultiSelect = false;
			this.DoubleBuffered = true;
			this.UseCompatibleStateImageBehavior = false;
			this.View = System.Windows.Forms.View.Details;
			this.OwnerDraw = true;

			this.Columns.AddRange(new ColumnHeader[]
			                      {
				                      this.headerDefault
			                      });

			///////////////////////////////////////////////////////////////////////////////

			this.headerDefault.Text = "Toolbox";
			this.headerDefault.Width = 252;

			///////////////////////////////////////////////////////////////////////////////

			listViewGroup5.Header = "Character Class";
			listViewGroup5.Name = "lvgCharacterClass";
			listViewGroup6.Header = "Sets";
			listViewGroup6.Name = "lvgSets";
			listViewGroup7.Header = "Repetitions";
			listViewGroup7.Name = "lvgRepetitions";
			listViewGroup8.Header = "Other";
			listViewGroup8.Name = "lvgOther";
			listViewGroup9.Header = "Apply to selected";
			listViewGroup9.Name = "lgvSelected";

			this.Groups.AddRange(new ListViewGroup[]
			                     {
				                     listViewGroup5,
				                     listViewGroup6,
				                     listViewGroup7,
									 listViewGroup9,
									 listViewGroup8
			                     });

            ///////////////////////////////////////////////////////////////////////////////
            listViewItem13.Group = listViewGroup5;
            listViewItem13.Tag = "(.)";
            listViewItem14.Group = listViewGroup5;
			listViewItem14.Tag = "\\w";
			listViewItem15.Group = listViewGroup5;
			listViewItem15.Tag = "\\d";
			listViewItem16.Group = listViewGroup5;
			listViewItem16.Tag = "\\s";
			listViewItem17.Group = listViewGroup6;
			listViewItem17.Tag = "[a-zA-Z]";
			listViewItem18.Group = listViewGroup6;
			listViewItem18.Tag = "[0-9]";
			listViewItem19.Group = listViewGroup7;
			listViewItem19.Tag = "?";
			listViewItem20.Group = listViewGroup7;
			listViewItem20.Tag = "+";
			listViewItem21.Group = listViewGroup7;
			listViewItem21.Tag = "*";
			listViewItem24.Group = listViewGroup8;
			listViewItem24.Tag = "\\b";
			listViewItem25.Group = listViewGroup8;
			listViewItem25.Tag = "\\b\\b";
			listViewItem26.Group = listViewGroup8;
			listViewItem26.StateImageIndex = 0;
			listViewItem26.Tag = "http://regexlib.com/CheatSheet.aspx?AspxAutoDetectCookieSupport=1";

			listViewItem22.Group = listViewGroup9;
			listViewItem22.Tag = "|";
			listViewItem23.Group = listViewGroup9;
			listViewItem23.Tag = "-";
			listViewItem27.Group = listViewGroup9;
			listViewItem27.Tag = "()";

			this.Items.AddRange(new ListViewItem[]
			                    {
                                    listViewItem13,
                                    listViewItem14,
				                    listViewItem15,
				                    listViewItem16,
				                    listViewItem17,
				                    listViewItem18,
				                    listViewItem19,
				                    listViewItem20,
				                    listViewItem21,
				                    listViewItem22,
				                    listViewItem23,
									listViewItem27,
									listViewItem24,
				                    listViewItem25,
				                    listViewItem26
			                    });

			///////////////////////////////////////////////////////////////////////////////

			this.ColumnWidthChanging += new ColumnWidthChangingEventHandler(this.this_ColumnWidthChanging);
			this.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(this.this_DrawColumnHeader);
			this.DrawItem += new DrawListViewItemEventHandler(this.this_DrawItem);
			this.SizeChanged += new EventHandler(this.this_SizeChanged);
			this.MouseClick += new MouseEventHandler(this.this_MouseClick);
			this.MouseDown += new MouseEventHandler(this.this_MouseDown);
			this.MouseLeave += new EventHandler(this.this_MouseLeave);
			this.MouseEnter += new EventHandler(this.this_MouseEnter);
			this.MouseMove += new MouseEventHandler(this.this_MouseMove);
			this.MouseUp += new MouseEventHandler(this.this_MouseUp);
			this.ColumnClick += new ColumnClickEventHandler(this.this_ColumnClick);
		}

		protected void InsertRegExp(ListViewItem lvi)
		{
			if (_buddy == null)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var selectionStartPosition = _buddy.SelectionStart;
			var selectionLength = _buddy.SelectionLength;

			///////////////////////////////////////////////////////////////////////////////

			var regExpText = _buddy.Text;
			var originalLength = regExpText.Length;
			var tag = (string)lvi.Tag;

			switch (lvi.Group.Name)
			{
				case "lvgCharacterClass":
					regExpText = regExpText.Insert(selectionStartPosition, tag);
					break;

				case "lvgSets":
					regExpText = regExpText.Insert(selectionStartPosition + selectionLength, tag);
					break;

				case "lvgRepetitions":
					regExpText = regExpText.Insert(selectionStartPosition + selectionLength, tag);
					break;

				case "lgvSelected":
					if (tag == "()")
					{
						SurroundAndSelect(regExpText, selectionStartPosition, selectionLength, "(", ")", "");
						return;
					}
					else
					{
						SurroundAndSelect(regExpText, selectionStartPosition, selectionLength, "[", "]", tag);
						return;
					}

				case "lvgOther":
					if (tag.StartsWith("http"))
					{
						Process.Start(tag);
						return;
					}

					if (tag == @"\b")
						regExpText = regExpText.Insert(selectionStartPosition + selectionLength, tag);
					else if (tag == @"\b\b")
					{
						SurroundAndSelect(regExpText, selectionStartPosition, selectionLength, @"\b", @"\b", "");
						return;
					}

					break;
			}

			_buddy.Text = regExpText;

			var selectionOffset = regExpText.Length - originalLength;

			_buddy.Select(selectionStartPosition + selectionOffset, 0);
			_buddy.Select();
		}

		protected void SurroundAndSelect(string regExpText, int selectionStartPosition, int selectionLength, string prefix, string suffix, string insert)
		{
			var selectedText = regExpText.Substring(selectionStartPosition, selectionLength);

			var insertedCharCount = 0;
			if (!String.IsNullOrEmpty(insert))
			{
				selectedText = String.Join<char>(insert, selectedText);
				insertedCharCount = selectedText.Length - selectionLength;
			}

			selectedText = prefix + selectedText + suffix;

			///////////////////////////////////////////////////////////////////////////////

			regExpText = regExpText.Remove(selectionStartPosition, selectionLength);
			regExpText = regExpText.Insert(selectionStartPosition, selectedText);

			_buddy.Text = regExpText;

			_buddy.Select(selectionStartPosition + selectionLength + insertedCharCount + prefix.Length + suffix.Length, 0);

			_buddy.Select();
		}

		protected void ShowTooltip(ListViewItem lvi, Point ptMouse)
		{
			if (_toolTip.Tag == null || _toolTip.Tag != lvi)
			{
				var rc = lvi.GetBounds(ItemBoundsPortion.Entire);
				var helpImageRect = GetHelpImageRect(rc);
				helpImageRect.X += 4;
				helpImageRect.Y = rc.Top;
				helpImageRect.Height = rc.Height;

				if (helpImageRect.Contains(ptMouse))
				{
					var pt = new Point(rc.Left + 20, rc.Bottom + 3);

					_toolTip.Tag = lvi;

					var key = lvi.Tag as string;
					if (!String.IsNullOrEmpty(key))
					{
						var text = String.Empty;
						if (_help.TryGetValue(key, out text))
						{
							var html = String.Format("<html><head>{0}</head><body>{1}</body></html>", _styles, text);

							_toolTip.Show(html, this, pt);
						}
					}
				}
			}
		}

		protected void HideTooltip()
		{
			_toolTip.Tag = null;
			_toolTip.Hide(this);
		}

		#endregion

		#region Implementation: Ribbon style

		protected void DrawBackground(DrawListViewItemEventArgs e)
		{
			var rc = new Rectangle(e.Bounds.X + 20, e.Bounds.Y, e.Bounds.Width - 40, e.Bounds.Height);

			var index = e.Item.Group.Items.IndexOf(e.Item);
			if (index < e.Item.Group.Items.Count - 1)
				rc.Height += 1;

			///////////////////////////////////////////////////////////////////////////////

			if (_lviHoverIndex != -1 && e.ItemIndex == _lviHoverIndex)
			{
				rc.Width++;
				rc.Height++;

				if (_bPressed)
					DrawButtonPressed(e.Graphics, rc);
				else
					DrawButtonSelected(e.Graphics, rc, RibbonProfessionalRenderer.Corners.All);

				rc.Width--;
				rc.Height--;
				e.Graphics.DrawRectangle(Pens.Black, rc);
			}
			else
			{
				e.Graphics.FillRectangle(_backBrush, rc);
				e.Graphics.DrawRectangle(Pens.Black, rc);
			}
		}

		protected Rectangle GetHelpImageRect(Rectangle rc)
		{
			var y = rc.Top + ((rc.Bottom - rc.Top) / 2 - (_image.Height / 2));
			y -= 1;

			return new Rectangle((rc.Width - 20) - (_image.Width * 2), y, _image.Width, _image.Height);
		}

		protected void DrawImage(DrawListViewItemEventArgs e)
		{
			if (HasHint(e.Item))
			{
				var rc = GetHelpImageRect(e.Bounds);
				var pt = new Point(rc.X, rc.Y);

				e.Graphics.DrawImageUnscaled(_image, pt);
			}
		}

		protected void DrawText(DrawListViewItemEventArgs e)
		{
			var lvi = e.Item;
			if (lvi != null && !String.IsNullOrEmpty(lvi.Text))
			{
				var sf = new StringFormat
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center,
				};

				///////////////////////////////////////////////////////////////////////////////

				var rc = new Rectangle(e.Bounds.X + 20, e.Bounds.Y, e.Bounds.Width - 40, e.Bounds.Height);

				e.Graphics.DrawString(lvi.Text, this.Font, this.Enabled ? _textBrush : _disabledTextBrush, rc, sf);
			}
		}

		protected void DrawButtonPressed(Graphics g, Rectangle bounds)
		{
			Rectangle outerR = Rectangle.FromLTRB(
				bounds.Left,
				bounds.Top,
				bounds.Right - 1,
				bounds.Bottom - 1);

			Rectangle innerR = Rectangle.FromLTRB(
				bounds.Left + 1,
				bounds.Top + 1,
				bounds.Right - 2,
				bounds.Bottom - 2);

			Rectangle glossyR = Rectangle.FromLTRB(
				bounds.Left + 1,
				bounds.Top + 1,
				bounds.Right - 2,
				bounds.Top + Convert.ToInt32((double)bounds.Height * .36));

			using (GraphicsPath boundsPath = RibbonProfessionalRenderer.RoundRectangle(outerR, 3, RibbonProfessionalRenderer.Corners.All))
			{
				using (SolidBrush brus = new SolidBrush(_colorTable.ButtonPressedBgOut))
					g.FillPath(brus, boundsPath);

				#region Main Bg

				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
					path.CloseFigure();
					using (PathGradientBrush gradient = new PathGradientBrush(path))
					{
						gradient.WrapMode = WrapMode.Clamp;
						gradient.CenterPoint = new PointF(
							Convert.ToSingle(bounds.Left + bounds.Width / 2),
							Convert.ToSingle(bounds.Bottom));
						gradient.CenterColor = _colorTable.ButtonPressedBgCenter;
						gradient.SurroundColors = new Color[] { _colorTable.ButtonPressedBgOut };

						Blend blend = new Blend(3);
						blend.Factors = new float[] { 0f, 0.8f, 0f };
						blend.Positions = new float[] { 0f, 0.30f, 1f };

						Region lastClip = g.Clip;
						Region newClip = new Region(boundsPath);
						newClip.Intersect(lastClip);
						g.SetClip(newClip.GetBounds(g));
						g.FillPath(gradient, path);
						g.Clip = lastClip;
					}
				}

				#endregion

				//Border
				using (Pen p = new Pen(_colorTable.ButtonPressedBorderOut))
					g.DrawPath(p, boundsPath);

				//Inner border
				using (GraphicsPath path = RibbonProfessionalRenderer.RoundRectangle(innerR, 3, RibbonProfessionalRenderer.Corners.All))
				using (Pen p = new Pen(_colorTable.ButtonPressedBorderIn))
					g.DrawPath(p, path);

				//Glossy effect
				using (GraphicsPath path = RibbonProfessionalRenderer.RoundRectangle(glossyR, 3, (RibbonProfessionalRenderer.Corners.All & RibbonProfessionalRenderer.Corners.NorthWest) | (RibbonProfessionalRenderer.Corners.All & RibbonProfessionalRenderer.Corners.NorthEast)))
				using (LinearGradientBrush b = new LinearGradientBrush(
					glossyR, _colorTable.ButtonPressedGlossyNorth, _colorTable.ButtonPressedGlossySouth, 90))
				{
					b.WrapMode = WrapMode.TileFlipXY;
					g.FillPath(b, path);
				}
			}

			DrawPressedShadow(g, outerR);
		}

		protected void DrawButtonSelected(Graphics g, Rectangle bounds, RibbonProfessionalRenderer.Corners corners)
		{
			Rectangle outerR = Rectangle.FromLTRB(
				bounds.Left,
				bounds.Top,
				bounds.Right - 1,
				bounds.Bottom - 1);

			Rectangle innerR = Rectangle.FromLTRB(
				bounds.Left + 1,
				bounds.Top + 1,
				bounds.Right - 2,
				bounds.Bottom - 2);

			Rectangle glossyR = Rectangle.FromLTRB(
				bounds.Left + 1,
				bounds.Top + 1,
				bounds.Right - 2,
				bounds.Top + Convert.ToInt32((double)bounds.Height * .36));

			using (GraphicsPath boundsPath = RibbonProfessionalRenderer.RoundRectangle(outerR, 3, corners))
			{
				using (SolidBrush brus = new SolidBrush(_colorTable.ButtonSelectedBgOut))
					g.FillPath(brus, boundsPath);

				#region Main Bg

				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddEllipse(new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height * 2));
					path.CloseFigure();
					using (PathGradientBrush gradient = new PathGradientBrush(path))
					{
						gradient.WrapMode = WrapMode.Clamp;
						gradient.CenterPoint = new PointF(
							Convert.ToSingle(bounds.Left + bounds.Width / 2),
							Convert.ToSingle(bounds.Bottom));
						gradient.CenterColor = _colorTable.ButtonSelectedBgCenter;
						gradient.SurroundColors = new Color[] { _colorTable.ButtonSelectedBgOut };

						Blend blend = new Blend(3);
						blend.Factors = new float[] { 0f, 0.8f, 0f };
						blend.Positions = new float[] { 0f, 0.30f, 1f };

						Region lastClip = g.Clip;
						Region newClip = new Region(boundsPath);
						newClip.Intersect(lastClip);
						g.SetClip(newClip.GetBounds(g));
						g.FillPath(gradient, path);
						g.Clip = lastClip;
					}
				}

				#endregion

				//Border
				using (Pen p = new Pen(_colorTable.ButtonSelectedBorderOut))
					g.DrawPath(p, boundsPath);

				//Inner border
				using (GraphicsPath path = RibbonProfessionalRenderer.RoundRectangle(innerR, 3, corners))
				using (Pen p = new Pen(_colorTable.ButtonSelectedBorderIn))
					g.DrawPath(p, path);

				//Glossy effect
				using (GraphicsPath path = RibbonProfessionalRenderer.RoundRectangle(glossyR, 3, (corners & RibbonProfessionalRenderer.Corners.NorthWest) | (corners & RibbonProfessionalRenderer.Corners.NorthEast)))
				using (LinearGradientBrush b = new LinearGradientBrush(
					glossyR, _colorTable.ButtonSelectedGlossyNorth, _colorTable.ButtonSelectedGlossySouth, 90))
				{
					b.WrapMode = WrapMode.TileFlipXY;
					g.FillPath(b, path);
				}
			}
		}

		protected void DrawPressedShadow(Graphics g, Rectangle r)
		{
			Rectangle shadow = Rectangle.FromLTRB(r.Left, r.Top, r.Right, r.Top + 4);

			using (GraphicsPath path = RibbonProfessionalRenderer.RoundRectangle(shadow, 3, RibbonProfessionalRenderer.Corners.NorthEast | RibbonProfessionalRenderer.Corners.NorthWest))
			using (LinearGradientBrush b = new LinearGradientBrush(shadow,
				Color.FromArgb(50, Color.Black),
				Color.FromArgb(0, Color.Black),
				90))
			{
				b.WrapMode = WrapMode.TileFlipXY;
				g.FillPath(b, path);
			}
		}

		#endregion

		#region Implementation: help

		protected string GetDefaultHelpFilePath()
		{
			return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly()
															  .Location), "help.html");
		}

		protected void LoadHelp(string filePath)
		{
			_help = new Dictionary<string, string>();

			///////////////////////////////////////////////////////////////////////////////

			if (File.Exists(filePath))
			{
				var lines = File.ReadAllLines(filePath);

				///////////////////////////////////////////////////////////////////////////////

				var skipLinesCount = 0;
				_styles = ExtractStyles(lines, out skipLinesCount);

				///////////////////////////////////////////////////////////////////////////////

				var previousDataKey = String.Empty;
				var previousBlockIndex = -1;

				for (var i = skipLinesCount; i < lines.Length; i++)
				{
					var line = lines[i];
					var dataKey = ExtractDataKey(line);
					if (!String.IsNullOrEmpty(dataKey) || i == lines.Length - 1)
					{
						if (i == lines.Length - 1)
							i++;

						var block = ExtractBlock(lines, previousBlockIndex, i);
						if (!String.IsNullOrEmpty(block))
							_help.Add(previousDataKey, block.Trim());

						///////////////////////////////////////////////////////////////////////////////

						previousBlockIndex = i;
						previousDataKey = dataKey;
					}
				}

				///////////////////////////////////////////////////////////////////////////////

				var defaultFilePath = GetDefaultHelpFilePath();
				if (_help.Any() && defaultFilePath != filePath)
					File.WriteAllLines(defaultFilePath, lines);
			}
		}

		protected bool HasHint(ListViewItem lvi)
		{
			var key = lvi.Tag as string;
			return !String.IsNullOrEmpty(key) && _help.ContainsKey(key);
		}

		protected string ExtractStyles(string[] lines, out int skipLinesCount)
		{
			skipLinesCount = 0;

			if (lines.Length == 0)
				return String.Empty;

			///////////////////////////////////////////////////////////////////////////////

			var line = lines[0];
			if (!String.Equals(line.Trim(), "<style>", StringComparison.InvariantCultureIgnoreCase))
				return String.Empty;

			///////////////////////////////////////////////////////////////////////////////

			var styleLines = lines.TakeWhile(x => !String.Equals(x.Trim(), "</style>", StringComparison.InvariantCultureIgnoreCase));
			skipLinesCount = styleLines.Count() + 1;

			var styles = String.Concat(styleLines.SelectMany(x => x)) + "</style>";

			return styles;
		}

		protected string ExtractDataKey(string line)
		{
			if (!String.IsNullOrEmpty(line))
			{
				var pattern = "data-key=\"";
				var startPos = line.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase);
				if (startPos != -1)
				{
					startPos += pattern.Length;
					var endPos = line.IndexOf("\"", startPos);
					if (endPos != -1)
					{
						var dataKey = line.Substring(startPos, endPos - startPos);
						return dataKey;
					}
				}
			}

			return String.Empty;
		}

		protected string ExtractBlock(string[] lines, int previousBlockIndex, int currentBlockIndex)
		{
			if (previousBlockIndex == -1)
				return null;

			return String.Concat(lines.Skip(previousBlockIndex)
						.Take(currentBlockIndex - previousBlockIndex)
						.SelectMany(x => x));
		}

		#endregion

		#region Interop

		private const uint LVM_SETHOTCURSOR = 4158;

		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		#endregion
	}
}