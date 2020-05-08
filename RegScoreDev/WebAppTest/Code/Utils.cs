using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WebAppTest
{
    public static class Utils
    {
        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff"),
                Path.GetExtension(fileName)
                );
        }
    }
}
