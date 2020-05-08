using System.Collections;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormColors : Form
	{
		#region Data members

		protected ArrayList _arrColors;

		#endregion

		#region Properties

		public ArrayList Colors
		{
			get { return GetColors(); }
			set { SetColors(value); }
		}

		#endregion

		#region Ctors

		public FormColors()
		{
			InitializeComponent();

			_arrColors = new ArrayList();
		}

		#endregion

		#region Implementation

		protected void SetColors(ArrayList arrColors)
		{
			try
			{
				lbColors.Items.Clear();
				_arrColors.Clear();

				if (arrColors != null)
				{
					_arrColors.AddRange(arrColors);

					int nIndex;
					foreach (ColorInfo ci in arrColors)
					{
						nIndex = lbColors.Items.Add(ci.Description);
						if (nIndex != -1)
						{
							if (ci.Selected)
								lbColors.SetItemChecked(nIndex, true);
						}
					}
				}
			}
			catch { }
		}

		protected ArrayList GetColors()
		{
			ArrayList arrColors = new ArrayList();

			try
			{
				ColorInfo ci;
				for (int i = 0; i < _arrColors.Count; i++)
				{
					ci = (ColorInfo)_arrColors[i];
					if (ci != null)
					{
						ci.Selected = lbColors.GetItemChecked(i);
						if (ci.Selected)
							arrColors.Add(ci);
					}
				}
			}
			catch { }

			return arrColors;
		}

		#endregion
	}

	public class ColorInfo
	{
		#region Fields

		protected int _nID;
		protected string _strDescription;
		protected int _nRGB;
		protected bool _bSelected;

		#endregion

		#region Properties

		public int ID
		{
			get { return _nID; }
		}

		public string Description
		{
			get { return _strDescription; }
		}

		public int RGB
		{
			get { return _nRGB; }
		}

		public bool Selected
		{
			get { return _bSelected; }
			set { _bSelected = value; }
		}

		#endregion

		#region Ctors

		public ColorInfo(string strDescription, int nID, int nRGB, bool bSelected)
		{
			_strDescription = strDescription;
			_nID = nID;
			_bSelected = bSelected;
			_nRGB = nRGB;
		}

		#endregion
	}
}
