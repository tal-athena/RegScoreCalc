using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RegScoreCalc
{
	public partial class FormSplitNotes : Form
	{
		#region Fields

		protected Control _parent;
		protected Control _hostedControl;

		#endregion

		#region Properties

		public bool IsSwapped { get; set; }

		#endregion

		#region Ctors

		public FormSplitNotes(Control control)
		{
			InitializeComponent();

			this.BackColor = MainForm.ColorBackground;
			splitter.Panel1.BackColor = MainForm.ColorBackground;

			///////////////////////////////////////////////////////////////////////////////

			_hostedControl = control;
			_parent = _hostedControl.Parent;
		}

		#endregion

		#region Events

		private void FormSplitNotes_Load(object sender, EventArgs e)
		{
			splitter.Panel2.Select();
			splitter.Panel2.Focus();
		}

		private void FormSplitNotes_SizeChanged(object sender, EventArgs e)
		{
			var panelCenter = splitter.Panel1.Width / 2;
			var distance = btnMakeSameSize.Left - btnSwap.Right;

			btnSwap.Left = (panelCenter - btnSwap.Width) - distance / 2;
			btnMakeSameSize.Left = panelCenter + distance / 2;
		}

		private void FormSplitNotes_FormClosing(object sender, FormClosingEventArgs e)
		{
			
		}

		private void btnSwap_Click(object sender, EventArgs e)
		{
			SwapPanels();

			this.IsSwapped = !this.IsSwapped;
		}

		private void btnMakeSameSize_Click(object sender, EventArgs e)
		{
			MakeSameSize();
		}

		#endregion

		#region Operations

		public void AttachControlToParent()
		{
			if (this.IsSwapped)
				SwapPanels();

			splitter.Panel2.Controls.Remove(_hostedControl);

			_hostedControl.Parent = _parent;
			_parent.Controls.Add(_hostedControl);
		}

		public void DetachControlFromParent()
		{
			_parent.Controls.Remove(_hostedControl);
			splitter.Panel2.Controls.Add(_hostedControl);

			_hostedControl.Parent = splitter.Panel2;

			if (this.IsSwapped)
				SwapPanels();
		}

		#endregion

		#region Implementation

		protected void SwapPanels()
		{
			var childSplitter = _hostedControl as SplitContainer;
			if (childSplitter != null)
			{
				if (childSplitter.Panel1.Controls.Count > 0 && childSplitter.Panel2.Controls.Count > 0)
				{
					var controlLeft = childSplitter.Panel1.Controls[0];
					var controlRight = childSplitter.Panel2.Controls[0];

					childSplitter.Panel1.Controls.RemoveAt(0);
					childSplitter.Panel2.Controls.RemoveAt(0);

					childSplitter.Panel1.Controls.Add(controlRight);
					childSplitter.Panel2.Controls.Add(controlLeft);

					controlRight.Parent = childSplitter.Panel1;
					controlLeft.Parent = childSplitter.Panel2;

					var leftText = lblLeft.Text;

					lblLeft.Text = lblRight.Text;
					lblRight.Text = leftText;
				}
			}
		}

		private void MakeSameSize()
		{
			var childSplitter = _hostedControl as SplitContainer;
			if (childSplitter != null)
				childSplitter.SplitterDistance = childSplitter.Width / 2;
		}

		#endregion
	}
}
