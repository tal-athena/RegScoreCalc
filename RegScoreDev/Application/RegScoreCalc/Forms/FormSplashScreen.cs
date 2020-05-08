using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormSplashScreen : Form
	{
		#region Ctors

		public FormSplashScreen()
		{
			InitializeComponent();

			lblAssemblyDescription.Parent = picSplashScreen;
			lblAssemblyDescription.Text = string.Empty;
		}

		#endregion

		#region Events

		private void FormSplashScreen_Load(object sender, System.EventArgs e)
		{
			try
			{
				var attribute = Assembly.GetExecutingAssembly()
                .GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)
                .Cast<AssemblyCopyrightAttribute>()
				.FirstOrDefault();

				if (attribute != null)
					lblAssemblyDescription.Text = attribute.Copyright;
			}
			catch { }
		}

		#endregion
	}
}
