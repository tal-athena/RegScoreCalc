using System;
using System.Drawing;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormNotes : Form
	{
		#region Fields

		protected Control _parent;
		protected Control _hostedControl;

		#endregion

		#region Ctors

		public FormNotes(Control control)
		{
			InitializeComponent();

			///////////////////////////////////////////////////////////////////////////////

			_hostedControl = control;
			_parent = _hostedControl.Parent;

			DetachControlFromParent();
		}

		#endregion

		#region Events

		private void FormNotes_Load(object sender, System.EventArgs e)
		{
			
		}

		private void FormNotes_FormClosing(object sender, FormClosingEventArgs e)
		{
			AttachControlToParent();
		}

		#endregion

		#region Operations

		public void AttachControlToParent()
		{
			this.Controls.Remove(_hostedControl);

			_hostedControl.Parent = _parent;
			_parent.Controls.Add(_hostedControl);
		}

		public void DetachControlFromParent()
		{
			_parent.Controls.Remove(_hostedControl);
			this.Controls.Add(_hostedControl);

			_hostedControl.Parent = this;
		}

		#endregion
	}
}
