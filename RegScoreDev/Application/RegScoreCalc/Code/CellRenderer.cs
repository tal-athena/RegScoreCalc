using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace RegScoreCalc.Code
{
	public class CellRenderer
	{
		#region Fields

		protected Pen _gridPen;
		protected Brush _normalBgBrush;
		protected Brush _selBgBrush;
		protected Color _normalFgColor;
		protected Color _selFgColor;
		protected Font _normalFont;
		protected Font _boldFont;

		#endregion

		#region Ctors

		public CellRenderer(DataGridView grid)
		{
			_gridPen = new Pen(grid.GridColor);
			_normalBgBrush = new SolidBrush(grid.DefaultCellStyle.BackColor);
			_selBgBrush = new SolidBrush(grid.DefaultCellStyle.SelectionBackColor);

			_normalFgColor = grid.DefaultCellStyle.ForeColor;
			_selFgColor = grid.DefaultCellStyle.SelectionForeColor;

			UpdateFonts(grid);
		}

		#endregion

		#region Operations

		public void UpdateFonts(DataGridView grid)
		{
			_normalFont = grid.Font;
			_boldFont = new Font(_normalFont, FontStyle.Bold);
		}

		public void RenderCell(DataGridViewCell cell, DataGridViewCellPaintingEventArgs e, string filter)
		{
			e.Graphics.FillRectangle(cell.Selected ? _selBgBrush : _normalBgBrush, e.CellBounds);

			e.Graphics.DrawLine(_gridPen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
			e.Graphics.DrawLine(_gridPen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

			var value = e.Value is int ? e.Value.ToString() : e.Value as string;

		var fgColor = cell.Selected ? _selFgColor : _normalFgColor;
			var offset = e.CellBounds.X + 2 + 6;

			var flags = TextFormatFlags.Left | TextFormatFlags.NoPadding;

			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			if (!String.IsNullOrEmpty(value) && !String.IsNullOrEmpty(filter))
			{
				var start = value.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase);
				if (start != -1)
				{
					var end = start + filter.Length;

					var match = value.Substring(start, filter.Length);
					var prefix = value.Substring(0, start);
					var suffix = value.Substring(end);

					Size size;
					var bounds = new Size(1000, 1000);

					///////////////////////////////////////////////////////////////////////////////

					if (!String.IsNullOrEmpty(prefix))
					{
						size = TextRenderer.MeasureText(e.Graphics, prefix, _normalFont, bounds, flags);
						TextRenderer.DrawText(e.Graphics, prefix, _normalFont, new Point(offset, e.CellBounds.Y + 2), fgColor, flags);

						offset += size.Width;
					}

					///////////////////////////////////////////////////////////////////////////////

					size = TextRenderer.MeasureText(e.Graphics, match, _boldFont, bounds, flags);
					TextRenderer.DrawText(e.Graphics, match, _boldFont, new Point(offset, e.CellBounds.Y + 2), fgColor, flags);

					offset += size.Width;

					///////////////////////////////////////////////////////////////////////////////

					if (!String.IsNullOrEmpty(suffix))
						TextRenderer.DrawText(e.Graphics, suffix, _normalFont, new Point(offset, e.CellBounds.Y + 2), fgColor, flags);
				}
				else
					TextRenderer.DrawText(e.Graphics, value, _normalFont, new Point(offset, e.CellBounds.Y + 2), fgColor, flags);
			}
			else
				TextRenderer.DrawText(e.Graphics, value, _normalFont, new Point(offset, e.CellBounds.Y + 2), fgColor, flags);
		}

		/*public void RenderCell(DataGridViewCell cell, DataGridViewCellPaintingEventArgs e, string filter)
		{
			e.Graphics.FillRectangle(cell.Selected ? _selBgColor : _normalBgBrush, e.CellBounds);

			e.Graphics.DrawLine(_gridPen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
			e.Graphics.DrawLine(_gridPen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

			var value = e.Value is int ? e.Value.ToString() : e.Value as string;

			var fgBrush = cell.Selected ? _selFgColor : _normalFgColor;
			float offset = e.CellBounds.X + 2;

			var sf = new StringFormat { FormatFlags = StringFormatFlags.MeasureTrailingSpaces | StringFormatFlags.NoWrap | StringFormatFlags.NoClip };

			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

			if (!String.IsNullOrEmpty(value) && !String.IsNullOrEmpty(filter))
			{
				var start = value.IndexOf(filter, StringComparison.InvariantCultureIgnoreCase);
				if (start != -1)
				{
					var end = start + filter.Length;

					var match = value.Substring(start, filter.Length);
					var prefix = value.Substring(0, start);
					var suffix = value.Substring(end);

					SizeF size;

					///////////////////////////////////////////////////////////////////////////////

					if (!String.IsNullOrEmpty(prefix))
					{
						size = e.Graphics.MeasureString(prefix, _normalFont, 1000, sf);
						e.Graphics.DrawString(prefix, _normalFont, fgBrush, offset, e.CellBounds.Y + 2, sf);

						offset += size.Width - 6;
					}

					///////////////////////////////////////////////////////////////////////////////

					size = e.Graphics.MeasureString(match, _boldFont, 1000, sf);
					e.Graphics.DrawString(match, _boldFont, fgBrush, offset, e.CellBounds.Y + 2, sf);

					offset += size.Width - 6;

					///////////////////////////////////////////////////////////////////////////////

					if (!String.IsNullOrEmpty(suffix))
						e.Graphics.DrawString(suffix, _normalFont, fgBrush, offset, e.CellBounds.Y + 2, sf);
				}
				else
					e.Graphics.DrawString(value, _normalFont, fgBrush, offset, e.CellBounds.Y + 2, sf);
			}
			else
				e.Graphics.DrawString(value, _normalFont, fgBrush, offset, e.CellBounds.Y + 2, sf);
		}*/

		#endregion

		#region Implementation

		#endregion
	}
}