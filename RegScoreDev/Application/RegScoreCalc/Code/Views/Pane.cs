using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public class Pane : Form
	{
		#region Fields

		protected ViewsManager _views;
		protected View _ownerView;

		public virtual event EventHandler _eventDataModified;

		#endregion

		#region Designer

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// Pane
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Pane";
			this.Dock = DockStyle.Fill;
			this.ShowInTaskbar = false;
			this.TopLevel = false;
			this.ResumeLayout(false);
		}

		#endregion

		#region Ctors

		public Pane()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion

		#region Events

		protected void OnDataModified(object sender, EventArgs e)
		{
			UpdatePane();
		}

		#endregion

		#region Operations: general

		public virtual void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
		{
			this.Parent = panel;

			_views = views;
			_ownerView = ownerView;

			InitPaneCommands(tab);
		}

		public virtual void UpdatePane()
		{

		}

		public virtual void DestroyPane()
		{
			this.Close();
		}

		public virtual bool OnHotkey(string code)
		{
			return false;
		}

		public void ShowPane()
		{
			this.Show();
			this.UpdatePane();
		}

		#endregion

		#region Operations: pane data - under construction!

		/*public T FindPaneData<T>(ViewDataBase viewData) where T : PaneDataBase, new()
		{
			if (viewData == null)
				return null;

			///////////////////////////////////////////////////////////////////////////////

			var paneData = viewData.PaneDataList.FirstOrDefault(x => x.PaneName == this.Name) as T;
			if (paneData == null)
				paneData = new T();

			return paneData;
		}

		public void SetPaneData(ViewDataBase viewData, PaneDataBase paneData)
		{
			if (viewData == null)
				return;

			///////////////////////////////////////////////////////////////////////////////

			var index = viewData.PaneDataList.FindIndex(x => x.PaneName == this.Name);
			if (index != -1)
				viewData.PaneDataList[index] = paneData;
			else
				viewData.PaneDataList.Add(paneData);
		}*/

		#endregion

		#region Implementation

		protected virtual void InitPaneCommands(RibbonTab tab)
		{
		}

		protected virtual void RaiseDataModifiedEvent()
		{
			if (_eventDataModified != null)
				_eventDataModified(this, new EventArgs());
		}

		#endregion
	}
}
