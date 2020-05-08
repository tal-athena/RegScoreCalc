using System;
using System.Drawing;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public class ImageButton : Button
	{
		#region Fields

		protected bool _bPressed;
		protected bool _bMouseOver;

		public Image NormalImage { get; set; }
		public Image HoverImage { get; set; }
		public Image PressedImage { get; set; }
		public bool Stretch { get; set; }

		#endregion

		#region Ctors

		public ImageButton()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
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
				Image img = this.NormalImage;

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

				//////////////////////////////////////////////////////////////////////////

				e.Graphics.FillRectangle(Brushes.White, this.ClientRectangle);

				if (img != null)
				{
					var rc = this.Stretch ? this.ClientRectangle : new Rectangle(0, 0, img.Width, img.Height);

					e.Graphics.DrawImage(img, rc);
				}
			}
			catch { }
		}

		#endregion
	}
}
