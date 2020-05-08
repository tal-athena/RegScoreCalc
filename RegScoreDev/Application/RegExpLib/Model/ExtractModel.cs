using System;

namespace RegExpLib.Model
{
    [Serializable]
    public class ExtractOptions
    {
        #region Fields

        public bool Extract { get; set; }
        public int Order { get; set; }
        public int InstanceNo { get; set; }
        public int? NthInstaceNumber { get; set; }
        public bool? AddToPrevious { get; set; }
        public string DateTimeFormat { get; set; }
        public int NoteTextColumn { get; set;}
		#endregion
	}
}