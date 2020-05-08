using System.Drawing;
using System.Windows.Forms;

namespace RegScoreCalc
{
	partial class PaneRegTabNotes
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.TabNotesControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // TabNotesControl
            // 
            this.TabNotesControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabNotesControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.TabNotesControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TabNotesControl.Location = new System.Drawing.Point(0, 0);
            this.TabNotesControl.Margin = new System.Windows.Forms.Padding(0);
            this.TabNotesControl.Name = "TabNotesControl";
            this.TabNotesControl.Padding = new System.Drawing.Point(0, 0);
            this.TabNotesControl.SelectedIndex = 0;
            this.TabNotesControl.Size = new System.Drawing.Size(0, 0);
            this.TabNotesControl.TabIndex = 1;
            this.TabNotesControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            this.TabNotesControl.SelectedIndexChanged += new System.EventHandler(this.TabNotesControl_SelectedIndexChanged);
            this.TabNotesControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TabNotesControl_MouseClick);
            // 
            // PaneRegTabNotes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(0, 0);
            this.Controls.Add(this.TabNotesControl);
            this.Name = "PaneRegTabNotes";
            this.Text = "PaneTabNotes";
            this.ResumeLayout(false);

		}

		#endregion
        private System.Windows.Forms.TabControl TabNotesControl;
        //private System.Windows.Forms.TabPage tabPage1;

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            TabPage tp = TabNotesControl.TabPages[e.Index];

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;  //optional

            // This is the rectangle to draw "over" the tabpage title
            RectangleF headerRect = new RectangleF(e.Bounds.X, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);

            // This is the default colour to use for the non-selected tabs
            SolidBrush sb = new SolidBrush(Color.AntiqueWhite);

            // This changes the colour if we're trying to draw the selected tabpage
            if (TabNotesControl.SelectedIndex == e.Index)
                sb.Color = MainForm.ColorBackground;

            // Colour the header of the current tabpage based on what we did above
            g.FillRectangle(sb, e.Bounds);

            //Remember to redraw the text - I'm always using black for title text
            g.DrawString(tp.Text, TabNotesControl.Font, new SolidBrush(Color.Black), headerRect, sf);
        }
    }
}