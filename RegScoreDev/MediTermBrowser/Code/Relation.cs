using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediTermBrowser.Code
{
    public class Relation
    {
        public string RelationName { get; set; }
        public List<Concept> Concepts { get; set; }

        public Relation()
        {
            Concepts = new List<Concept>();
        }
    }
}
