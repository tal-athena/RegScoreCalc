using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using OpenQA.Selenium;

using Action = WebAppTest.Code.Action;

using TestErrorException = WebAppTest.Code.TestErrorException;

namespace WebAppTest.Action_Code
{
    public class Login : Action
    {
        public Login() : base(new ActionControls.LoginControl(), "Login")
        {
            
        }

        public override XmlNode WriteToXML(XmlDocument document)
        {
            XmlNode node = document.CreateElement("Login");

            TextBox txtUsername = (TextBox)_userControl.Controls.Find("txtLogin", true).FirstOrDefault();
            TextBox txtPassword = (TextBox)_userControl.Controls.Find("txtPassword", true).FirstOrDefault();

            string login = txtUsername.Text;
            string password = txtPassword.Text;

            XmlNode loginNode = document.CreateElement("Login");
            XmlNode passwordNode = document.CreateElement("Password");

            XmlNode loginValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            loginValueNode.Value = login;
            loginNode.AppendChild(loginValueNode);

            XmlNode passwordValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            passwordValueNode.Value = password;
            passwordNode.AppendChild(passwordValueNode);

            node.AppendChild(loginNode);
            node.AppendChild(passwordNode);
            return node;
        }

        public override void ReadFromXML(XmlNode element)
        {

            var e = element.SelectSingleNode("Login").FirstChild;

			string login = (e != null)? e.Value : string.Empty;

            string password = string.Empty;
            if (element.SelectSingleNode("Password") != null && element.SelectSingleNode("Password").FirstChild != null) { 

                var e1 = element.SelectSingleNode("Password").FirstChild;

			    password = (e1 != null)? e1.Value : string.Empty;
            }

            TextBox txtUsername = (TextBox)_userControl.Controls.Find("txtLogin", true).FirstOrDefault();
            TextBox txtPassword = (TextBox)_userControl.Controls.Find("txtPassword", true).FirstOrDefault();

            txtUsername.Text = login;
            txtPassword.Text = password;
        }

        public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
        {
            sw.Restart();

            browser.Navigate().GoToUrl(URL + "Authentication/Account/LogOn");
            browser.WaitForPageLoad();

            sw.Stop();

            // Enter login and password
            TextBox txtUsername = (TextBox)_userControl.Controls.Find("txtLogin", true).FirstOrDefault();
            TextBox txtPassword = (TextBox)_userControl.Controls.Find("txtPassword", true).FirstOrDefault();

            var username = txtUsername.Text;
            var password = txtPassword.Text;

            browser.FindElement(By.Id("UserName")).SendKeys(username);
            browser.FindElement(By.Id("Password")).SendKeys(password);

            sw.Restart();

            browser.FindElement(By.ClassName("button24_left")).Click();

            browser.WaitForElement(By.Id("navigationItemsContainer"), 60);

            sw.Stop();

            var url = browser.Url;
            if (url.Contains("Authentication/Account/LogOn"))
            {
                //Login failed
                throw new TestErrorException("Login failed.", Result.StopAndCloseBrowser);
            }
            else if (browser.IsTextPresent("Server Error"))
            {
                //Login failed
                throw new TestErrorException("Login failed. Server error.", Result.StopAndCloseBrowser);
            }
            
            return Result.Continue;
        }

        
    }
}
