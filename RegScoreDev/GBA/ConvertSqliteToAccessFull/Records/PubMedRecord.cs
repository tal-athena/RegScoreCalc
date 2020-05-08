using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertSqliteToAccessFull.Records
{
    class PubMedRecord
    {
        #region Fields

        [PrimaryKey]
        public double ED_ENC_NUM { get; set; }

        public string NOTE_TEXT { get; set; }

        public string NOTE_TEXT1 { get; set; }

        public string PubMedLink { get; set; }

        public string FullTextLink { get; set; }

        #endregion
    }
}
