using System;
using System.Windows.Forms;

using RegExpLib.Core;

using RegScoreCalc.Forms;

namespace RegScoreCalc
{
	public class ViewStatistics: View
	{
		#region Fields

		protected PaneRegExp _paneRegExp;
		protected PaneStatistics _paneStatistics;

		protected FormRegularExpressionEditor _editor;

		#endregion

		#region Ctors

		public ViewStatistics(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
		}

		#endregion

		#region Events

		protected void OnDataModified(object sender, EventArgs e)
		{
			UpdateView();
		}

		#endregion

		#region Overrides

		protected override void InitViewCommands(RibbonPanel panel)
		{
			panel.OwnerTab.Panels.Remove(panel);
		}

		protected override void InitViewPanes(RibbonTab tab)
		{
			this.Orientation = Orientation.Horizontal;

			_paneRegExp = new PaneRegExp(_views);
			_paneRegExp.InitPane(_views, this, this.Panel1, tab);
			_paneRegExp._eventDataModified += new EventHandler(OnDataModified);
			_paneRegExp.EnableNavigation = false;

			this.Panel1.Controls.Add(_paneRegExp);
			_paneRegExp.ShowPane();

			//InitializeRegExpEditor(false);

			//////////////////////////////////////////////////////////////////////////

			_paneStatistics = new PaneStatistics();
			_paneStatistics.InitPane(_views, this, this.Panel2, tab);
			_paneStatistics._eventDataModified += new EventHandler(OnDataModified);
			tab.Panels.RemoveAt(tab.Panels.Count - 2);

			this.Panel2.Controls.Add(_paneStatistics);
			_paneStatistics.ShowPane();

			this.SplitterDistance -= 280;
		}

		public override void UpdateView()
		{
			if (_paneRegExp != null)
				_paneRegExp.UpdatePane();

			if (_paneStatistics != null)
				_paneStatistics.UpdatePane();
		}

		#endregion
	}
}
