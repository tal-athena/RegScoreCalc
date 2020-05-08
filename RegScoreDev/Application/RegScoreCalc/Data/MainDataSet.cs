using System.Linq;

namespace RegScoreCalc
{
    public partial class MainDataSet
    {
        partial class DocumentsDataTable
        {
            #region Operations

            public DocumentsRow FindByPrimaryKey(double ED_ENC_NUM)
            {
                return this.Rows.Cast<DocumentsRow>()
                           .FirstOrDefault(x => x.ED_ENC_NUM == ED_ENC_NUM);
            }

            #endregion
        }
    }
}