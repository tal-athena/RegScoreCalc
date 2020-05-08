using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediTermBrowser.Code
{
   public class Concept
    {
       public string Code { get; set; }
       public string Term { get; set; }
       public List<string> Synonims { get; set; }

       public List<Concept> Parents { get; set; }
       public List<Concept> Children { get; set; }
       public List<Concept> PartOf { get; set; }
       public List<Concept> InversePartOf { get; set; }
       public List<Concept> AncestorParts { get; set; }
       public List<Concept> DescendantParts { get; set; }

       public List<Relation> Relations { get; set; }

       public Concept()
       {
           Parents = new List<Concept>();
           Children = new List<Concept>();
           PartOf = new List<Concept>();
           InversePartOf = new List<Concept>();
           AncestorParts = new List<Concept>();
           DescendantParts = new List<Concept>();

           Relations = new List<Relation>();
       }
    }
}
