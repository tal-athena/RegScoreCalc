using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Collections.ObjectModel;

using OpenQA.Selenium;

using Action = WebAppTest.Code.Action;

using TestErrorException = WebAppTest.Code.TestErrorException;

namespace WebAppTest.Action_Code
{
    public class PubMedSearch : Action
    {
        public PubMedSearch() : base(new ActionControls.PubMedSearchControl(), "PubMedSearch")
        {

        }

        public override XmlNode WriteToXML(XmlDocument document)
        {
            XmlNode node = document.CreateElement("PubMedSearch");

           
            return node;
        }

        public override void ReadFromXML(XmlNode element)
        {

            var e = element.SelectSingleNode("PubMedSearch").FirstChild;
        }

        public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
        {
            //ReadOnlyCollection<IWebElement> elements = browser.FindElements(By.("a"));
            //foreach (IWebElement element in elements)
            //{
            //    if (element.Text.equals("Searched text")) ;
            //    // Perform Acrion on 
            //}
            return Result.Continue;
        }


    }
}
