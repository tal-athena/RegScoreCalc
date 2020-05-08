using RegExpLib.Model;
using System.Collections.Generic;

namespace RegExpLib.Processing
{

    public class EntitiesProcessingParam : EntitiesProcessingParamBase
    {
        #region Fields

        public string Command { get; set; }
        public string SqliteFilePath { get; set; }
        public string AccessFilePath { get; set; }

        public string AnacondaPath { get; set; }
        public string VirtualEnv { get; set; }

        #endregion 
       
    }

    public class CalculatedEntitiesResult
    {
        public EntityLabelResult EntityLabels { get; set; }
        public List<EntityResult> Documents { get; set; }
    }
    public class EntityLabelResult
    {
        public List<string> Labels { get; set; }
    }

    public class EntityResult
    {
        public double DocumentID { get; set; }
        public string Entities { get; set; }
    }

}
