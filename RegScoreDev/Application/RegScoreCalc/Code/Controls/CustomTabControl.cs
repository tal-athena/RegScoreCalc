using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace RegScoreCalc
{
	public class CustomTabControl : TabControl
	{
		#region Fields

		protected SolidBrush _brush;
		protected Font _font;

		#endregion

		#region Properties

		public bool ShowIndicators { get; set; }

		#endregion

		#region Ctors

		public CustomTabControl()
		{
			this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
			this.UpdateStyles();

			_brush = new SolidBrush(MainForm.ColorBackground);
		}

		#endregion

		#region Events

		#endregion

		#region Overrides

		[StructLayout(LayoutKind.Sequential)]
		protected struct PAINTSTRUCT
		{
			public IntPtr hdc;
			public bool fErase;
			public RECT rcPaint;
			public bool fRestore;
			public bool fIncUpdate;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
			public byte[] rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		protected struct RECT
		{
			public Int32 left;
			public Int32 top;
			public Int32 right;
			public Int32 bottom;
		}

		[DllImport("user32.dll")]
		protected static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

		[DllImport("user32.dll")]
		protected static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x000f)
			{
				try
				{
					PAINTSTRUCT paintStruct = new PAINTSTRUCT();
					IntPtr hDC = BeginPaint(this.Handle, out paintStruct);

					Graphics g = Graphics.FromHdc(hDC);

					Bitmap backBuffer = new Bitmap(this.ClientSize.Width,this.ClientSize.Height);

					//this.Parent.Update();

					Graphics gMemory = Graphics.FromImage(backBuffer);

					gMemory.FillRectangle(_brush, this.ClientRectangle);

					Rectangle rc;

					DrawItemEventArgs e;

					for (int i = 0; i < this.TabPages.Count; i++)
					{
						rc = GetTabRect(i);

						rc.Y -= 4;
						rc.Height += 6;
						rc.Width--;

						if (i == 0)
						{
							rc.X += 2;
							rc.Width -= 2;
						}

						if (_font == null)
							_font = new Font(this.Parent.Font, FontStyle.Bold);

						e = new DrawItemEventArgs(gMemory, _font, rc, i, DrawItemState.None);
						DrawTab(e);
					}

					g.DrawImageUnscaled(backBuffer, 0, 0);

					EndPaint(this.Handle, ref paintStruct);
					g.Dispose();

					m.Result = IntPtr.Zero;
				}
				catch (System.Exception ex)
				{
					ex.ToString();
				}
			}
			else if (m.Msg == 0x0014)
			{
				var g = Graphics.FromHdc(m.WParam);
				g.FillRectangle(_brush, this.ClientRectangle);

				m.Result = (IntPtr)1;
			}
			else
				base.WndProc(ref m);
		}

		#endregion

		#region Implementation

		protected void DrawTab(DrawItemEventArgs e)
		{
			var page = (CustomTabPage) this.TabPages[e.Index];

			string strText = page.Text;

			Rectangle rc = new Rectangle(e.Bounds.X, e.Bounds.Y + 3, e.Bounds.Width, e.Bounds.Height - 3);

			if (this.Alignment == TabAlignment.Bottom)
			{
				rc.Y -= 2;
				rc.Height += 2;
			}

			e.Graphics.DrawRectangle(new Pen(Color.Black, 1), rc);

			rc.Y++;
			rc.Height--;

			if (e.Index == 0)
			{
				rc.X++;
				rc.Width--;
			}

			e.Graphics.DrawRectangle(new Pen(Color.Black, 1), rc);

			Brush br;

			if (e.Index == this.SelectedIndex)
			{
				Color clrHighlight = MainForm.ColorBackground;
				e.Graphics.FillRectangle(new SolidBrush(clrHighlight), rc);

				br = new SolidBrush(Color.Black);
			}
			else
			{
				Color clrHighlight = Color.FromArgb(180, 219, 255);
				e.Graphics.FillRectangle(new SolidBrush(clrHighlight), rc);

				br = new SolidBrush(Color.Black);
			}

			var sf = new StringFormat
						{
							Alignment = StringAlignment.Center,
							LineAlignment = StringAlignment.Center
						};

			Rectangle rcText = rc;

			if (this.Alignment == TabAlignment.Top)
			{
				rcText.X += 1;
				rcText.Width -= 2;
			}

			if (this.ShowIndicators && page.Tag != null)
			{
				var rcIndicator = rc;
				rcIndicator.Inflate(-4, -4);
				rcIndicator.Y -= 1;
				rcIndicator.Width = 16;

				e.Graphics.FillRectangle(Brushes.LawnGreen, rcIndicator);
				e.Graphics.DrawRectangle(Pens.Black, rcIndicator);

				rcText.X += 16;
				rcText.Width -= 16;
			}
				
			///////////////////////////////////////////////////////////////////////////////

			if (e.Index == this.SelectedIndex)
				e.Graphics.DrawString(strText, e.Font, br, rcText, sf);
			else
				e.Graphics.DrawString(strText, this.Parent.Font, br, rcText, sf);

			///////////////////////////////////////////////////////////////////////////////

			Point pt1;
			Point pt2;

			if (this.Alignment == TabAlignment.Top)
			{
				if (e.Index == this.SelectedIndex)
				{

				}
				else 
				{
					pt1 = new Point(rc.Left, rc.Bottom - 1);
					pt2 = new Point(rc.Right, rc.Bottom - 1);

					e.Graphics.DrawLine(new Pen(Color.Black, 1), pt1, pt2);
				}
			}
			else
			{
				pt1 = new Point(rc.Left, rc.Bottom - 1);
				pt2 = new Point(rc.Right, rc.Bottom - 1);

				e.Graphics.DrawLine(new Pen(Color.Black, 1), pt1, pt2);

				if (e.Index == this.SelectedIndex)
				{

				}
				else
				{
					pt1 = new Point(rc.Left, rc.Top);
					pt2 = new Point(rc.Right, rc.Top);

					e.Graphics.DrawLine(new Pen(Color.Black, 1), pt1, pt2);
				}

			}
		}

		#endregion
	}

	public class CustomTabPage : TabPage
	{
		#region Constants

		protected int WM_ERASEBKGND = 0x0014;

		#endregion

		#region Fields

		protected SolidBrush _brush;

		#endregion

		#region Ctors

		public CustomTabPage() : base()
		{
			_brush = new SolidBrush(MainForm.ColorBackground);
		}

		public CustomTabPage(string strTitle) : base(strTitle)
		{
			_brush = new SolidBrush(MainForm.ColorBackground);
		}

		#endregion

		#region Events

		#endregion

		#region Overrides

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_ERASEBKGND)
			{
				Graphics g = Graphics.FromHdc(m.WParam);
				g.FillRectangle(_brush, this.ClientRectangle);

				m.Result = (IntPtr)1;
			}
			else
				base.WndProc(ref m);
		}

		#endregion
	}
}
