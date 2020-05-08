using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WebAppTest.Action_Code;

namespace WebAppTest.Forms
{
	public partial class FormAddNewAction : Form
	{
		#region Ctors

		public FormAddNewAction()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		public Type SelectedActionType { get; set; }

		#endregion

		#region Events

		private void FormAddNewAction_Load(object sender, EventArgs e)
		{
			try
			{
				FillActionsListBox();

				lbActions.Select();
				lbActions.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		private void FormAddNewAction_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (this.DialogResult == DialogResult.OK)
				{
					if (lbActions.SelectedItem != null)
					{
						var item = (ListBoxItem) lbActions.SelectedItem;
						this.SelectedActionType = item.Type;
					}
					else
					{
						MessageBox.Show("Please select action from list", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
						e.Cancel = true;
					}
				}
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		private void lbActions_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				this.DialogResult = DialogResult.OK;
				Close();
			}
			catch (Exception ex)
			{
				MainForm.HandleException(ex);
			}
		}

		#endregion

		#region Implementation

		protected void FillActionsListBox()
		{
			lbActions.DisplayMember = "Text";

            lbActions.Items.Add(CreateActionListViewItem("PubMedSearch", typeof(PubMedSearch)));

            lbActions.Items.Add(CreateActionListViewItem("Login", typeof (Login)));
			lbActions.Items.Add(CreateActionListViewItem("Load Story", typeof (LoadStory)));
            lbActions.Items.Add(CreateActionListViewItem("Load Search", typeof(LoadSearch)));
			lbActions.Items.Add(CreateActionListViewItem("Load Collection", typeof (LoadCollection)));
			lbActions.Items.Add(CreateActionListViewItem("Header Fields", typeof (EditStoryFields)));
            lbActions.Items.Add(CreateActionListViewItem("Search Param", typeof(EditSearchParam)));
			lbActions.Items.Add(CreateActionListViewItem("Edit Record", typeof (EditCollectionRecord)));
			lbActions.Items.Add(CreateActionListViewItem("Run Script", typeof(RunScript)));
			lbActions.Items.Add(CreateActionListViewItem("Logout", typeof (Logout)));
		}

		protected ListBoxItem CreateActionListViewItem(string text, Type actionType)
		{
			return new ListBoxItem { Type = actionType, Text = text };
		}

		#endregion
	}

	public class ListBoxItem
	{
		#region Fields

		public Type Type { get; set; }
		public string Text { get; set; }

		#endregion
	}
}