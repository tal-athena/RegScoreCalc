using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace RegScoreCalc
{
	public class IndProgressBar : ProgressBar
	{
		#region Constants

		protected const int _nSegmentWidth = 7;

		protected const int _nSegmentDistance = 4;

		#endregion

		#region Fields

		protected Timer _timer;

		protected int _nOffset;

		protected int _nSectionWidth = 60;
		protected int _nSectionDistance = 150;

		#endregion

		#region Ctors

		public IndProgressBar()
		{
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

			_nOffset = 0;

			_timer = new Timer();
			_timer.Interval = 30;
			_timer.Tick += new EventHandler(timer_Tick);
		}

		#endregion

		#region Events

		protected void timer_Tick(object sender, EventArgs e)
		{
			_nOffset += 1;

			if (_nOffset > _nSectionWidth + _nSectionDistance)
				_nOffset = 0;

			this.Invalidate();
			this.Update();
		}

		#endregion

		#region Overrides

		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				Rectangle rc = new Rectangle(0, 0, this.Width, this.Height);

				SolidBrush brBackground = new SolidBrush(MainForm.ColorBackground);
				e.Graphics.FillRectangle(brBackground, rc);

				if (VisualStyleInformation.IsEnabledByUser)
				{
					ControlPaint.DrawBorder3D(e.Graphics, rc, Border3DStyle.SunkenInner, Border3DSide.Left | Border3DSide.Top | Border3DSide.Right | Border3DSide.Bottom);

					rc.Inflate(-2, -2);
				}
				else
				{
					rc.Y += 1;
					rc.Height -= 4;

					rc.Inflate(-2, 0);
				}

				e.Graphics.IntersectClip(rc);

				Rectangle rcSegment;

				for (int i = _nOffset - (_nSectionWidth + _nSectionDistance); i < rc.Width; i += _nSectionWidth + _nSectionDistance)
				{
					for (int j = i; j < i + _nSectionWidth; j += _nSegmentWidth + _nSegmentDistance)
					{
						rcSegment = new Rectangle(j, 0, _nSegmentWidth, this.Height);

						e.Graphics.FillRectangle(SystemBrushes.Highlight, rcSegment);
					}
				}
			}
			catch { }
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x0014) // WM_ERASEBKGND
			{
				m.Result = (IntPtr)1;
			}
			else
				base.WndProc(ref m);
		}

		#endregion

		#region Operations

		public void StartProgress()
		{
			StopProgress();

			_nOffset = 0;

			_nSectionWidth = (int)(this.Width / 6.5);
			_nSectionDistance = (int)(this.Width / 3);

			_timer.Start();
		}

		public void StopProgress()
		{
			if (_timer.Enabled)
			{
				_timer.Stop();

				_nOffset = -1;

				this.Invalidate();
				this.Update();
			}
		}

		#endregion
	}
}
