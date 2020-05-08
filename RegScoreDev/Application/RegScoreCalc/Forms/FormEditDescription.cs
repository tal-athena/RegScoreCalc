using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormEditDescription : Form
	{
		#region Fields

		public string Description
		{
			get { return txtDescription.Text; }
			set { txtDescription.Text = value; }
		}

		#endregion

		#region Ctors

		public FormEditDescription()
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;

			var pt = PointToScreen(Cursor.Position);

			this.Left = pt.X - this.Width / 2;
			this.Top = pt.Y - this.Height / 2;
		}

		#endregion

		#region Events

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		#endregion
	}
}