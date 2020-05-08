using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc
{

    public delegate void CodesListChangedEventHandler(object sender, EventArgs e);

    public partial class PaneCodesList : Pane
    {
        #region Data members

        private bool _expanded;

        public bool Expanded
        {
            get
            {
                return _expanded;
            }
            set
            {
                _expanded = value;
            }
        }

        #endregion

        #region Ctors

        public PaneCodesList()
        {
            InitializeComponent();
            Expanded = false;
        }

        #endregion

        #region Events
        public event CodesListChangedEventHandler ChangedState;

        // Invoke the Changed event; called whenever list changes
        protected virtual void OnChangedState(EventArgs e)
        {
            if (ChangedState != null)
                ChangedState(this, e);
        }

        private void btnColorKey_Click(object sender, EventArgs e)
        {
            Expanded = !Expanded;

            //Set up icon
            if (Expanded)
            {
                btnColorKey.Image = Properties.Resources.up_icon;
            }
            else//Set down icon
            {
                btnColorKey.Image = Properties.Resources.down_icon;
            }
            
            //Event for resizeing pane
            OnChangedState(new EventArgs());
        }


        #endregion

        #region Overrides

		public override void InitPane(ViewsManager views, View ownerView, SplitterPanel panel, RibbonTab tab)
        {
            base.InitPane(views, ownerView, panel, tab);

			btnColorKey.BackColor = MainForm.ColorBackground;

        }

        #endregion

        #region Implementation


        #endregion
    }
}
