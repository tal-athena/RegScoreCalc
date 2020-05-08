using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegScoreCalc.Helpers
{
    class CategoryListItem
    {
        public CategoryListItem(int id, string category)
        {
            ID = id;
            Category = category;
        }
        public int ID { get; set; }
        public string Category { get; set; }
    }
}
