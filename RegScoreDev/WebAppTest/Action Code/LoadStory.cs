﻿using System;
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
    class LoadStory : Action
    {
        public LoadStory() : base(new ActionControls.LoadStoryControl(), "Load Story")
        {
            
        }

        public override XmlNode WriteToXML(XmlDocument document)
        {
            XmlNode node = document.CreateElement("LoadStory");

            TextBox txtID = (TextBox)_userControl.Controls.Find("txtID", true).FirstOrDefault();
            NumericUpDown numericRefresh = (NumericUpDown)_userControl.Controls.Find("numericRefresh", true).FirstOrDefault();
            NumericUpDown numericTimeout = (NumericUpDown)_userControl.Controls.Find("numericTimeout", true).FirstOrDefault();
            CheckBox checkBoxScreen = (CheckBox)_userControl.Controls.Find("checkBoxScreen", true).FirstOrDefault();
            CheckBox checkBoxOpenInNewTab = (CheckBox)_userControl.Controls.Find("checkBoxOpenInNewTab", true).FirstOrDefault();
            CheckBox checkBoxDontWait = (CheckBox)_userControl.Controls.Find("checkBoxDontWait", true).FirstOrDefault();

            string id = txtID.Text;
            string refresh = ((int)numericRefresh.Value).ToString();
            string timeout = ((int)numericTimeout.Value).ToString();
            string screenshot = checkBoxScreen.Checked.ToString();
            string newTab = checkBoxOpenInNewTab.Checked.ToString();
            string dontWait = checkBoxDontWait.Checked.ToString();

            XmlNode IDNode = document.CreateElement("ID");
            XmlNode refreshNode = document.CreateElement("Refresh");
            XmlNode timeoutNode = document.CreateElement("Timeout");
            XmlNode screenshotNode = document.CreateElement("Screenshot");
            XmlNode newTabNode = document.CreateElement("OpenInNewTab");
            XmlNode dontWaitNode = document.CreateElement("DontWait");

            XmlNode IdValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            IdValueNode.Value = id;
            IDNode.AppendChild(IdValueNode);

            XmlNode refreshValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            refreshValueNode.Value = refresh;
            refreshNode.AppendChild(refreshValueNode);

            XmlNode timeoutValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            timeoutValueNode.Value = timeout;
            timeoutNode.AppendChild(timeoutValueNode);

            XmlNode screenshotValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            screenshotValueNode.Value = screenshot;
            screenshotNode.AppendChild(screenshotValueNode);

            XmlNode newTavValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            newTavValueNode.Value = newTab;
            newTabNode.AppendChild(newTavValueNode);

            XmlNode dontWaitValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            dontWaitValueNode.Value = dontWait;
            dontWaitNode.AppendChild(dontWaitValueNode);

            node.AppendChild(IDNode);
            node.AppendChild(refreshNode);
            node.AppendChild(timeoutNode);
            node.AppendChild(screenshotNode);
            node.AppendChild(newTabNode);
            node.AppendChild(dontWaitNode);
            return node;
        }

        public override void ReadFromXML(XmlNode element)
        {
            string id = element.SelectSingleNode("ID").FirstChild.Value;
            string refresh = element.SelectSingleNode("Refresh").FirstChild.Value;
            string timeout = element.SelectSingleNode("Timeout").FirstChild.Value;
            string screenshot = element.SelectSingleNode("Screenshot").FirstChild.Value;
            string newTab = element.SelectSingleNode("OpenInNewTab").FirstChild.Value;
            string dontWait = element.SelectSingleNode("DontWait").FirstChild.Value;

            TextBox txtID = (TextBox)_userControl.Controls.Find("txtID", true).FirstOrDefault();
            NumericUpDown numericRefresh = (NumericUpDown)_userControl.Controls.Find("numericRefresh", true).FirstOrDefault();
            NumericUpDown numericTimeout = (NumericUpDown)_userControl.Controls.Find("numericTimeout", true).FirstOrDefault();
            CheckBox checkBoxScreen = (CheckBox)_userControl.Controls.Find("checkBoxScreen", true).FirstOrDefault();
            CheckBox checkBoxOpenInNewTab = (CheckBox)_userControl.Controls.Find("checkBoxOpenInNewTab", true).FirstOrDefault();
            CheckBox checkBoxDontWait = (CheckBox)_userControl.Controls.Find("checkBoxDontWait", true).FirstOrDefault();

            txtID.Text = id;
            numericRefresh.Value = Convert.ToInt32(refresh);
            numericTimeout.Value = Convert.ToInt32(timeout);
            checkBoxScreen.Checked = Convert.ToBoolean(screenshot);
            checkBoxOpenInNewTab.Checked = Convert.ToBoolean(newTab);
            checkBoxDontWait.Checked = Convert.ToBoolean(dontWait);
        }

        public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
        {
            TextBox txtID = (TextBox)_userControl.Controls.Find("txtID", true).FirstOrDefault();
            NumericUpDown numericRefresh = (NumericUpDown)_userControl.Controls.Find("numericRefresh", true).FirstOrDefault();
            NumericUpDown numericTimeout = (NumericUpDown)_userControl.Controls.Find("numericTimeout", true).FirstOrDefault();
            CheckBox checkBoxScreen = (CheckBox)_userControl.Controls.Find("checkBoxScreen", true).FirstOrDefault();
            CheckBox checkBoxOpenInNewTab = (CheckBox)_userControl.Controls.Find("checkBoxOpenInNewTab", true).FirstOrDefault();
            CheckBox checkBoxDontWait = (CheckBox)_userControl.Controls.Find("checkBoxDontWait", true).FirstOrDefault();

            var id = txtID.Text;
            var refresh = (int)numericRefresh.Value;
            var timeout = (int)numericTimeout.Value * 1000;


            {
                sw.Restart();
                for (int i = 0; i <= refresh; i++)
                {

                    browser.Navigate().GoToUrl(URL + "Story/EditStory/WithInserts/" + id);
                    browser.WaitForPageLoad();

                    if (browser.IsTextPresent("Server Error"))
                    {
                        throw new TestErrorException("Error while opening story with id: " + id, Result.Continue);
                    }

                    if (timeout > 0)
                    {
                        System.Threading.Thread.Sleep(timeout);
                    }

                    ResetStoryEditorDirty(browser);
                }

                if (checkBoxScreen.Checked)
                {
                    string name = System.Environment.CurrentDirectory + "\\" + "TestScreen_.jpg".AppendTimeStamp();
                    browser.CaptureWebPageToFile(name);
                }
            }
            sw.Stop();

            return Result.Continue;
        }

        
    }
}
