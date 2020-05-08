using System;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public class ViewTemplate: View
	{
		#region Data members

		protected PaneCodes _paneCodes;
		protected PaneTemplate _paneTemplate;

		#endregion

		#region Ctors

		public ViewTemplate(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
			: base(viewtype, strTitle, views, objArgument)
		{
		}

		#endregion

		#region Events

		protected void OnDataModified(object sender, EventArgs e)
		{
			UpdateView();
		}

		protected void OnMaximizeCodes_Clicked(object sender, EventArgs e)
		{
			MaximizeNotesView();
		}

		#endregion

		#region Overrides

		protected override void InitViewCommands(RibbonPanel panel)
		{
			RibbonButton btnMaximizeCodes = new RibbonButton("Maximize Codes");
            
            panel.Items.Add(btnMaximizeCodes);

			btnMaximizeCodes.Image = Properties.Resources.MaximizeNotes;
            btnMaximizeCodes.SmallImage = Properties.Resources.MaximizeNotes;
			btnMaximizeCodes.Click += new EventHandler(OnMaximizeCodes_Clicked);
            btnMaximizeCodes.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;
			
		}

		protected override void InitViewPanes(RibbonTab tab)
		{
			this.Orientation = Orientation.Horizontal;

			_paneCodes = new PaneCodes();
			_paneCodes.InitPane(_views, this, this.Panel1, tab);
			_paneCodes._eventDataModified += new EventHandler(OnDataModified);

			this.Panel1.Controls.Add(_paneCodes);
			_paneCodes.ShowPane();

			//////////////////////////////////////////////////////////////////////////

			_paneTemplate = new PaneTemplate();
			_paneTemplate.InitPane(_views, this, this.Panel2, tab);
			_paneTemplate._eventDataModified += new EventHandler(OnDataModified);

			this.Panel2.Controls.Add(_paneTemplate);
			_paneTemplate.ShowPane();

			ResetView();
		}

		public override void UpdateView()
		{
			if (_paneCodes != null)
				_paneCodes.UpdatePane();

			if (_paneTemplate != null)
				_paneTemplate.UpdatePane();
		}

		#endregion

		#region Implementation

		protected void MaximizeNotesView()
		{
			this.Panel2Collapsed = true;
		}

		protected void ResetView()
		{
			this.Panel1Collapsed = false;
		}

		#endregion
	}
}
