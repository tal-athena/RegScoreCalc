using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace RegScoreCalc
{
	public partial class FormColumnPropsFreeText : Form
	{
		#region Fields

		protected ViewsManager _views;

		#endregion

		#region Ctors

		public FormColumnPropsFreeText()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
		}

		#endregion
	}
}
