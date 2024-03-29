using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [Designer(typeof(RibbonOrbMenuItemDesigner))]
    public class RibbonOrbMenuItem
        : RibbonButton
    {
        #region Fields

        #endregion

        #region Ctor

        public RibbonOrbMenuItem()
        {
            DropDownArrowDirection = RibbonArrowDirection.Left;
            SetDropDownMargin(new Padding(10));
            DropDownShowing += new EventHandler(RibbonOrbMenuItem_DropDownShowing);
        }

        public RibbonOrbMenuItem(string text)
            : this()
        {
            Text = text;
        }



		#endregion

		#region Props

	    public virtual bool KeepOpen { get; set; }

		protected override bool ClosesDropDownAt(Point p)
		{
			if (this.KeepOpen)
				return false;

		    return base.ClosesDropDownAt(p);
	    }

	    public override System.Drawing.Image Image
        {
            get
            {
                return base.Image;
            }
            set
            {
                base.Image = value;

                SmallImage = value;
            }
        }

        [Browsable(false)]
        public override System.Drawing.Image SmallImage
        {
            get
            {
                return base.SmallImage;
            }
            set
            {
                base.SmallImage = value;
            }
        }

        #endregion

        #region Methods

        private void RibbonOrbMenuItem_DropDownShowing(object sender, EventArgs e)
        {
            if (DropDown != null)
            {
                DropDown.DrawIconsBar = false;
            }
        }

        public override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (RibbonDesigner.Current == null)
            {
                if (Owner.OrbDropDown.LastPoppedMenuItem != null)
                {
                    Owner.OrbDropDown.LastPoppedMenuItem.CloseDropDown();
                }

                if (Style == RibbonButtonStyle.DropDown || Style == RibbonButtonStyle.SplitDropDown)
                {
                    //ShowDropDown();

                    //Owner.OrbDropDown.LastPoppedMenuItem = this;
                }
                
            }
            
        }

        public override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
		}

        internal override Point OnGetDropDownMenuLocation()
        {
            if (Owner == null) return base.OnGetDropDownMenuLocation();

            Rectangle b = Owner.RectangleToScreen(Bounds);
            Rectangle c = Owner.OrbDropDown.RectangleToScreen(Owner.OrbDropDown.ContentRecentItemsBounds);

            return new Point(b.Right, c.Top);
        }

        internal override Size OnGetDropDownMenuSize()
        {
            Rectangle r = Owner.OrbDropDown.ContentRecentItemsBounds;
            r.Inflate(-1, -1);
            return r.Size;
        }

        #endregion
    }
}
