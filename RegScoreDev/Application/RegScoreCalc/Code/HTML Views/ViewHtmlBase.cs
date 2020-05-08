using System;
using System.Windows.Forms;
using RegScoreCalc.Panes;

namespace RegScoreCalc.Views
{
	public class ViewHtmlBase : View
	{
		#region Fields

		protected PaneHtmlBase _paneHtml;

		#endregion

		#region Properties

		public HtmlViewInfo HtmlViewInfo
		{
			set
			{
				if (value != null)
				{
					_paneHtml.InitHtmlPane(_tab, value);

					_views.Ribbon.UpdateRegions();
				}
			}
		}

		#endregion

		#region Ctors

		public ViewHtmlBase(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
			_strLayoutName = "Layout 1";

			this.Panel2Collapsed = true;
			this.Orientation = Orientation.Vertical;
		}

		#endregion

		#region Events

		private void OnBtnReloadClick(object sender, EventArgs eventArgs)
		{
			try
			{
				_paneHtml.Reload();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		private void OnBtnDevToolsClick(object sender, EventArgs eventArgs)
		{
			try
			{
				_paneHtml.ShowDevTools();
			}
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}

		#endregion

		#region Overrides

		protected override void InitViewCommands(RibbonPanel panel)
		{
			var btnReload = new RibbonButton("Reload");
			btnReload.Image = Properties.Resources.Reload;
			btnReload.SmallImage = Properties.Resources.Reload;
			btnReload.Click += OnBtnReloadClick;
			btnReload.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
			panel.Items.Add(btnReload);

			if (this.ViewType.HtmlViewInfo.Type == HtmlViewType.Default)
			{
				var btnDevTools = new RibbonButton("Show Dev Tools");
				btnDevTools.Image = Properties.Resources.ShowDevTools;
				btnDevTools.SmallImage = Properties.Resources.ShowDevTools;
				btnDevTools.Click += OnBtnDevToolsClick;
				btnDevTools.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
				panel.Items.Add(btnDevTools);
			}
		}

		protected override void InitViewPanes(RibbonTab tab)
		{
			_paneHtml = new PaneHtmlBase
			            {
				            Dock = DockStyle.Fill
			            };

			_paneHtml.InitPane(_views, this, this.Panel1, tab);

			this.Panel1.Controls.Add(_paneHtml);
			_paneHtml.ShowPane();
		}

		public override void UpdateView()
		{
			if (_paneHtml != null)
				_paneHtml.UpdatePane();

			_views.Ribbon.Refresh();
		}

		public override void DestroyView()
		{
			_paneHtml.DestroyPane();

			base.DestroyView();
		}

		#endregion

		#region Operations



		#endregion

		#region Implementation

		#endregion
	}
}
