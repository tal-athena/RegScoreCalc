using System;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public class PaneCommands
	{
		#region Data members

		protected ViewsManager _views;
		public event EventHandler _eventDataModified;

		#endregion

		#region Ctors

		public PaneCommands(ViewsManager views, RibbonPanel panel)
		{
			_views = views;

			InitCommands(panel);
		}

		#endregion

		#region Operations

		#endregion

		#region Implementations

		protected virtual void InitCommands(RibbonPanel panel)
		{
			RibbonSeparator separator = new RibbonSeparator();
			panel.Items.Add(separator);
		}

		protected virtual void RaiseDataModifiedEvent()
		{
			if (_eventDataModified != null)
				_eventDataModified(this, new EventArgs());
		}

		#endregion
	}
}
