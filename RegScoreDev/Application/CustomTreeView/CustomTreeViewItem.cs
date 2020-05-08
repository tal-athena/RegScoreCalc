using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace CustomTreeView
{

    public class CustomTreeViewItem
    {
        public CustomTreeViewItem()
        {
            FrameBackgroundColor = new SolidColorBrush(Colors.White);
            Margin = new System.Windows.Thickness(5, 5, 5, 5);
        }
        public int ID { get; set; }
        public string ICD9 { get; set; }
        public string Diagnosis { get; set; }
        public List<SubList> SubList { get; set; }

        public int Width { get; set; }
        public SolidColorBrush Background { get; set; }
        public SolidColorBrush FrameBackgroundColor { get; set; }
        public System.Windows.Thickness Margin { get; set; }
    }

    public class SubList
    {
        public string Name { get; set; }
    }

}
