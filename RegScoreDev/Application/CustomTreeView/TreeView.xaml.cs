using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomTreeView
{
    public partial class TreeView : UserControl
    {
        public delegate void CustomTreeViewEventHandler(object sender, CustomTreeViewEventArgs args);
        public event CustomTreeViewEventHandler OnSelectionChange;
        public event CustomTreeViewEventHandler OnAddToGroup;
        private List<CustomTreeViewItem> _items;

        public TreeView()
        {
            InitializeComponent();
        }


        public void SetData(List<CustomTreeViewItem> source)
        {
            for (int i = 0; i < source.Count; i++)
            {
                source[i].ID = i;
            }
            _items = source;
            tree.ItemsSource = null;
            tree.ItemsSource = source;

        }
        public void SetWidth(int width)
        {
            width -= 30;
            var items = (List<CustomTreeViewItem>)tree.ItemsSource;
            if (items != null)
            {
                foreach (var item in items)
                {
                    item.Width = width;
                }
                tree.ItemsSource = null;
                tree.ItemsSource = items;
            }
        }

        private void treeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            var treeView = (System.Windows.Controls.TreeView)e.Source;

            CustomTreeViewItem selectedItem = (CustomTreeViewItem)treeView.SelectedItem;
        
            if (OnSelectionChange != null)
                OnSelectionChange(this, new CustomTreeViewEventArgs(selectedItem));

        }
        //AddFilesToFolder_Click
        private void AddToGroup_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((MenuItem)e.Source).Tag;


            CustomTreeViewItem selectedItem = _items.Where(p=>p.ID == id).FirstOrDefault();

            if (OnAddToGroup != null)
                OnAddToGroup(this, new CustomTreeViewEventArgs(selectedItem));
        }

     

        public class CustomTreeViewEventArgs : EventArgs
        {
            private CustomTreeViewItem _Item;
            public CustomTreeViewItem Item
            {
                get
                {
                    return _Item;
                }
                set
                {
                    _Item = value;
                }
            }
            public CustomTreeViewEventArgs(CustomTreeViewItem item)
            {
                _Item = item;
            }
        }

      

    }
}
