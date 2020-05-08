using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAppTest

{
    public class GlobalSettings
    {
        private static GlobalSettings instance;

        private GlobalSettings() { }

        public static GlobalSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalSettings();
                }
                return instance;
            }
        }

        public bool SaveScreenShots { get; set; }
    }
}
