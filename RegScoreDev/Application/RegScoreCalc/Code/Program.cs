using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;

namespace RegScoreCalc
{
    [LicenseProvider(typeof(DRTLicenseProvider))]
    static class Program
    {
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Attempt to validate the license
                LicenseManager.Validate(typeof(Program));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception exception)
            {
                MessageBox.Show("An unhandled application exception occurred." + Environment.NewLine + Environment.NewLine + exception.ToString());
            }
        }
    }
}
