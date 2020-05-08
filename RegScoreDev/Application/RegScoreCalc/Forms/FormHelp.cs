using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RegScoreCalc.Forms
{
    public partial class HelpForm : Form
    {
        private string url = "";
        public HelpForm(string Url)
        {
            InitializeComponent();
            url = Url;
        }
        public void NavigateToUrl(string url)
        {
            Uri uri = new Uri(url);
            this.helpBrowser.Navigate(uri);
        }
        protected override void OnLoad(EventArgs e)
        {
            //this.helpBrowser.DocumentText = Properties.Resources.HelpHtml;


            Uri uri = new Uri(url);
            this.helpBrowser.Navigate(uri);

        }
    }
}
