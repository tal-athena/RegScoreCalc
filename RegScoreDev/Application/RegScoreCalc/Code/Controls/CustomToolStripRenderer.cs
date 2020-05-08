using System.Drawing;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public class CustomToolStripRenderer : ToolStripProfessionalRenderer
	{
		#region Fields

		protected Brush _br;

		#endregion

		#region Ctors

		public CustomToolStripRenderer()
		{
			_br = new SolidBrush(MainForm.ColorBackground);
		}

		#endregion

		#region Overrides

		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			base.OnRenderToolStripBackground(e);

			e.Graphics.FillRectangle(_br, e.AffectedBounds);
		}

		#endregion
	}
}
