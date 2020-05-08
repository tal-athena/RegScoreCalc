using System;
using System.Collections.Generic;
using System.Windows.Forms;

using WebAppTest.Forms;
using WebAppTest.Selenium;

namespace WebAppTest.Code
{
    static class Program
    {
	    [STAThread]
	    static int Main(string[] args)
	    {
		    try
		    {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

			    if (args.Length == 4)
				{
					var testFilePath = args[0];

					var actionArgs = new ActionRunnerArgs
					{
						LogFilePath = args[1],
						URL = args[2],
						RunTimes = Convert.ToInt32(args[3]),
						TestFilePath = testFilePath
					};

					///////////////////////////////////////////////////////////////////////////////

					Application.Run(new MainForm(actionArgs));

					return 0;
				}
				else
				{
					Application.Run(new MainForm(null));

					return 1;
				}
			}
		    catch (Exception)
		    {
			    return -1;
		    }
        }
	}
}
