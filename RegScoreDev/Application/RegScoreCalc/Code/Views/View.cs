using RegScoreCalc.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Xml.Linq;

using Newtonsoft.Json;

namespace RegScoreCalc
{
	public class View : SplitContainer
	{
		#region Fields

		protected ViewType _viewtype;

		protected ViewsManager _views;

		protected RibbonTab _tab;

		protected string _strLayoutName;

		protected bool _helpOpened;

		protected HelpForm help;

		#endregion

		#region Properties

		public ViewType ViewType
		{
			get { return _viewtype; }
		}

		public virtual string LayoutName
		{
			get { return _strLayoutName; }
			set { _strLayoutName = value; }
		}

		public object Argument { get; set; }

		public RibbonTab RibbonTab
		{
			get { return _tab; }
		}

		#endregion

		#region Ctors

		public View(ViewType viewtype, string strTitle, ViewsManager views, object objArgument)
		{
			this.Name = viewtype.Name;
			this.Panel1MinSize = 0;
			this.Panel2MinSize = 0;

			//////////////////////////////////////////////////////////////////////////

			_viewtype = viewtype;
			_views = views;

			this.Argument = objArgument;

			_helpOpened = false;

			//////////////////////////////////////////////////////////////////////////

			this.BorderStyle = BorderStyle.Fixed3D;

			this.Left = _views.Ribbon.Left;
			this.Top = _views.Ribbon.Bottom;
			this.Width = _views.Ribbon.Width;
			this.Height = (_views.MainForm.Height - this.Top) - 38;

			this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
			this.BackColor = MainForm.ColorBackground;

			//////////////////////////////////////////////////////////////////////////

			_views.MainForm.Controls.Add(this);

			_tab = new RibbonTab
				   {
					   Text = strTitle,
					   Tag = this
				   };


			RibbonPanel panelGeneral = new RibbonPanel("General");
			
			_tab.Panels.Add(panelGeneral);

			InitViewCommands(panelGeneral);
			InitViewPanes(_tab);

			//////////////////////////////////////////////////////////////////////////
			RibbonPanel panelMisc = new RibbonPanel("Misc");
		   

			_tab.Panels.Add(panelMisc);


			RibbonButton btnCloseView = new RibbonButton("Close View");
			panelMisc.Items.Add(btnCloseView);
			btnCloseView.Image = Properties.Resources.CloseView;
			btnCloseView.SmallImage = Properties.Resources.CloseView;
			btnCloseView.Click += new EventHandler(_views.OnCloseView_Clicked);
			btnCloseView.MouseEnter += _views.MainForm.RibbonButton_MouseEnter;

			//////////////////////////////////////////////////////////////////////////

			//RibbonButton btnHelp = new RibbonButton("RegExp Help");
			//btnHelp.Image = Properties.Resources.HelpIcon;
			//btnHelp.Click += btnHelp_Click;

			//RibbonPanel helpGeneral = new RibbonPanel("Help");
			//helpGeneral.Items.Add(btnHelp);

			//tab.Panels.Add(helpGeneral);


			_views.Ribbon.Tabs.Add(_tab);
			_views.Ribbon.ActiveTab = _tab;
		}

		#endregion

		#region Events

		#endregion

		#region Operations: general

		public void ShowView()
		{
			this.Visible = true;

			UpdateView();
		}

		public void HideView()
		{
			this.Visible = false;
		}

		public virtual void UpdateView()
		{

		}

		public virtual void DestroyView()
		{
			foreach (RibbonTab tab in _views.Ribbon.Tabs)
			{
				if (tab.Tag == this)
				{
					_views.Ribbon.Tabs.Remove(tab);
					break;
				}
			}

			_views.Ribbon.ActiveTab = _views.Ribbon.Tabs[_views.Ribbon.Tabs.Count - 1];

			HideView();

			this.Dispose();
		}

		public virtual bool OnHotkey(string code)
		{
			return false;
		}

		public virtual void PerformDefaultAction()
		{

		}

		public virtual void BeforeDocumentsTableLoad(bool removeDynamicColumns)
		{
			
		}

		public virtual void AfterDocumentsTableLoad(bool addDynamicColumns)
		{
			
		}

		#endregion

		#region Operations: layout

		public virtual List<XElement> SaveCustomLayout()
		{
			return null;
		}

		public virtual void LoadCustomLayout(List<XElement> listCustomLayout)
		{

		}

		public virtual void ResetView()
		{

		}

		#endregion

		#region Operations: view data - under construction!

		/*public static T GetCommonViewData<T>(OleDbConnection connection) where T : ViewDataBase
		{
			var viewData = GetViewData<T>(connection, "Common");
			if (viewData == null)
				viewData = (T) new ViewDataBase();

			return viewData;
		}

		public static void SetCommonViewData(OleDbConnection connection, ViewDataBase viewData)
		{
			SetViewData(connection, "Common", viewData);
		}

		public virtual T GetViewData<T>(OleDbConnection connection) where T : ViewDataBase
		{
			return GetViewData<T>(connection, this.ViewType.Name);
		}

		public virtual void SetViewData(OleDbConnection connection, ViewDataBase viewData)
		{
			SetViewData(connection, this.ViewType.Name, viewData);
		}*/

		#endregion

		#region Implementation: general

		protected virtual void InitViewCommands(RibbonPanel panel)
		{

		}

		protected virtual void InitViewPanes(RibbonTab tab)
		{

		}

		protected virtual void ApplyLayout(string strLayoutName)
		{
			_views.SaveLayout(this);

			if (_strLayoutName != strLayoutName)
			{
				_strLayoutName = strLayoutName;

				_views.LoadLayout(this);
			}
		}

		#endregion

		#region Implementation: view data - under construction!

		/*protected static T GetViewData<T>(OleDbConnection connection, string viewName) where T : ViewDataBase
		{
			var closeConnection = false;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				if (connection.State == ConnectionState.Closed)
				{
					connection.Open();
					closeConnection = true;
				}

				///////////////////////////////////////////////////////////////////////////////

				var sql = "SELECT TOP 1 ViewData FROM ViewData WHERE (ViewName = @ViewName)";

				var command = new OleDbCommand(sql, connection);
				command.Parameters.AddWithValue("@ViewName", viewName);

				///////////////////////////////////////////////////////////////////////////////

				var viewData = command.ExecuteScalar() as string;

				if (!String.IsNullOrEmpty(viewData))
					return JsonConvert.DeserializeObject<T>(viewData);
			}
			finally
			{
				if (closeConnection)
					connection.Close();
			}

			///////////////////////////////////////////////////////////////////////////////

			return null;
		}

		protected static void SetViewData(OleDbConnection connection, string viewName, ViewDataBase viewData)
		{
			var closeConnection = false;

			///////////////////////////////////////////////////////////////////////////////

			try
			{
				var json = viewData != null ? JsonConvert.SerializeObject(viewData) : String.Empty;

				///////////////////////////////////////////////////////////////////////////////

				if (connection.State == ConnectionState.Closed)
				{
					connection.Open();
					closeConnection = true;
				}

				///////////////////////////////////////////////////////////////////////////////

				var oldValue = GetViewData<ViewDataBase>(connection, viewName);
				if (oldValue == null)
				{
					var sql = "INSERT INTO ViewData (ViewName, ViewData) VALUES (@ViewName, @ViewData)";

					var command = new OleDbCommand(sql, connection);
					command.Parameters.AddWithValue("@ViewName", viewName);
					command.Parameters.AddWithValue("@ViewData", json);

					var count = command.ExecuteNonQuery();
					count.ToString();
				}
				else
				{
					var sql = "UPDATE ViewData SET ViewData = @ViewData WHERE (ViewName = @ViewName)";

					var command = new OleDbCommand(sql, connection);
					command.Parameters.AddWithValue("@ViewData", viewName);
					command.Parameters.AddWithValue("@ViewName", json);

					var count = command.ExecuteNonQuery();
					count.ToString();
				}
			}
			finally
			{
				if (closeConnection)
					connection.Close();
			}
		}*/

		#endregion
	}
}
