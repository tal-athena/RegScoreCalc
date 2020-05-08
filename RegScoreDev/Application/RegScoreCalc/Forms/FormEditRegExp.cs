using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEditRegExp : Form
	{
		#region Properties

		public string RegExpValue
		{
			get { return txtRegExp.Text; }
			set
			{
				try
				{
					txtRegExp.Text = value;
					txtRegExp.SelectionLength = 0;
					txtRegExp.SelectionStart = value.Length;
				}
				catch
				{
					txtRegExp.Text = "";
				}
			}
		}

		#endregion

		#region Ctors

		public FormEditRegExp()
		{
			InitializeComponent();
		}

		#endregion

		#region Events
		#endregion
	}
}
