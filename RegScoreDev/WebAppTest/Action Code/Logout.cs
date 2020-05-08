using System.Diagnostics;
using System.Drawing;
using System.Xml;

using OpenQA.Selenium;

using WebAppTest.Action_Controls;

using Action = WebAppTest.Code.Action;

using TestErrorException = WebAppTest.Code.TestErrorException;

namespace WebAppTest.Action_Code
{
    class Logout : Action
    {
        public Logout() : base(new LogoutControl(), "Logout")
        {
            
        }

        public override XmlNode WriteToXML(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Logout");
            return node;
        }

        public override void ReadFromXML(XmlNode element)
        {
            // not parameters
        }
		public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
        {
            sw.Restart();

            //icon_logoff_function
            browser.FindElement(By.XPath("//span[contains(@class, 'button32_icon') and contains(@class, 'icon_logoff_function')]")).Click();

            System.Threading.Thread.Sleep(1500);

            browser.WaitForPageLoad();

            sw.Stop();

            var url = browser.Url;
            if (url.Contains("Authentication/Account/LogOff"))
            {
                
            }
            else
            {
                throw new TestErrorException("Login failed.", Result.Continue);
            }

            return Result.Continue;            
        }

        
    }
}
