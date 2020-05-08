using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public class RibbonStyleButton : Button
	{
		#region Fields

		protected RibbonProfesionalRendererColorTable _colorTable;

		protected bool _bPressed;
		protected bool _bMouseOver;

		protected Brush _backBrush;
		protected Brush _textBrush;
		protected Brush _disabledTextBrush;

		protected Pen _borderPen;

		public Image NormalImage { get; set; }
		public Image HoverImage { get; set; }
		public Image PressedImage { get; set; }

		public bool IsHighlighted { get; set; }
		public bool DrawNormalBorder { get; set; }

		#endregion

		#region Ctors

		public RibbonStyleButton()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

			_colorTable = new RibbonProfesionalRendererColorTable();

			_backBrush = new SolidBrush(MainForm.ColorBackground);
			_textBrush = new SolidBrush(_colorTable.Text);
			_disabledTextBrush = new SolidBrush(SystemColors.GrayText);

			_borderPen = new Pen(Color.Gray);
		}

		#endregion

		#region Overrides

		protected override void OnMouseEnter(EventArgs e)
		{
			_bMouseOver = true;
			this.Invalidate();
			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			_bMouseOver = false;
			this.Invalidate();
			base.OnMouseLeave(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			_bPressed = true;
			this.Invalidate();
			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			_bPressed = false;
			this.Invalidate();
			base.OnMouseUp(e);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				PaintBackground(e);

				PaintImage(e);

				PaintText(e);
			}
			catch
			{
			}
		}

		#endregion

		#region Implementation

		protected void PaintImage(PaintEventArgs e)
		{
			int x;

			var img = GetImage();
			if (img != null)
			{
				if (this.Text.Length > 0)
					x = 10;
				else
					x = this.Width / 2 - img.Width / 2;

				var y = this.Height / 2 - img.Height / 2;

				var rc = new Rectangle(x, y, img.Width, img.Height);

				e.Graphics.DrawImage(img, rc);
			}
		}

		private void PaintText(PaintEventArgs e)
		{
			if (this.Text.Length > 0)
			{
				var size = e.Graphics.MeasureString(this.Text, this.Font, this.ClientSize.Width);

				var sf = new StringFormat
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center,
				};

				///////////////////////////////////////////////////////////////////////////////

				Rectangle rc;

				var img = GetImage();
				if (img == null)
				{
					rc = new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);

					e.Graphics.DrawString(this.Text, this.Font, this.Enabled ? _textBrush : _disabledTextBrush, rc, sf);

					return;
				}

				///////////////////////////////////////////////////////////////////////////////

				var x = img.Width;
				var y = (int) (this.ClientRectangle.Height / 2 - size.Height / 2);

				rc = new Rectangle(x, 0, this.ClientRectangle.Width - x, this.ClientRectangle.Height);

				e.Graphics.DrawString(this.Text, this.Font, this.Enabled ? _textBrush : _disabledTextBrush, rc, sf);
			}
		}

		protected void PaintBackground(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(_backBrush, e.ClipRectangle);

			if (_bPressed)
				DrawButtonPressed(e.Graphics, e.ClipRectangle);
			else if (_bMouseOver || this.IsHighlighted)
				DrawButtonSelected(e.Graphics, e.ClipRectangle, RibbonProfessionalRenderer.Corners.All);
			else
			{
				if (this.DrawNormalBorder)
				{
					var borderRect = new Rectangle(e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Right - 1, e.ClipRectangle.Bottom - 1);

					e.Graphics.DrawRectangle(_borderPen, borderRect);
				}
			}
		}

		protected Image GetImage()
		{
			var img = this.NormalImage;

			if (_bPressed)
			{
				if (_bMouseOver)
				{
					if (this.PressedImage != null)
						img = this.PressedImage;
				}
			}
			else if (_bMouseOver)
			{
				if (this.HoverImage != null)
					img = this.HoverImage;
			}

			if (img == null)
				img = this.Image;

			return img;
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
				bounds.Top + Convert.ToInt32((double) bounds.Height * .36));

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
				bounds.Top + Convert.ToInt32((double) bounds.Height * .36));

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
	}
}