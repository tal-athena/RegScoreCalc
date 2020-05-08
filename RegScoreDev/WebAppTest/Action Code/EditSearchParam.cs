
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using OpenQA.Selenium.Support.UI;

using OpenQA.Selenium;

using Action = WebAppTest.Code.Action;

using TestErrorException = WebAppTest.Code.TestErrorException;

namespace WebAppTest.Action_Code
{
    public class EditSearchParam : Action
    {
        public EditSearchParam()
            : base(new EditSearchParamControl(), "Search param")
        {
            
        }

        public override XmlNode WriteToXML(XmlDocument document)
        {
            XmlNode node = document.CreateElement("EditSearchParam");

            TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true).FirstOrDefault();
            TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true).FirstOrDefault();
            ComboBox cmbType = (ComboBox)_userControl.Controls.Find("comboBox_fieldType", true).FirstOrDefault();

            string fieldId = txtFieldID.Text;
            string fieldValue = txtFieldValue.Text;
            string fieldType = cmbType.Text;

            XmlNode fieldIDNode = document.CreateElement("FieldID");
            XmlNode fieldValueNode = document.CreateElement("FieldValue");
            XmlNode fieldTypeNode = document.CreateElement("FieldType");

            XmlNode fieldIdValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            fieldIdValueNode.Value = fieldId;
            fieldIDNode.AppendChild(fieldIdValueNode);

            XmlNode fieldValueNodeValue = document.CreateNode(XmlNodeType.Text, "", "");
            fieldValueNodeValue.Value = fieldValue;
            fieldValueNode.AppendChild(fieldValueNodeValue);

            XmlNode fieldTypeNodeValue = document.CreateNode(XmlNodeType.Text, "", "");
            fieldTypeNodeValue.Value = fieldType;
            fieldTypeNode.AppendChild(fieldTypeNodeValue);

            node.AppendChild(fieldIDNode);
            node.AppendChild(fieldValueNode);
            node.AppendChild(fieldTypeNode);
            return node;
        }

        public override void ReadFromXML(XmlNode element)
        {
            string FieldID = element.SelectSingleNode("FieldID").FirstChild.Value;
            string FieldValue = element.SelectSingleNode("FieldValue").FirstChild.Value;
            string FieldType = element.SelectSingleNode("FieldType").FirstChild.Value;


            SetFieldID(FieldID);
            SetFieldValue(FieldValue);
            SetFieldType(FieldType);
        }

        public void SetFieldID(string text)
        {
            TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true).FirstOrDefault();
            if (txtFieldID.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetFieldID);
                txtFieldID.Invoke(d, new object[] { text });
            }
            else
            {
                txtFieldID.Text = text;
            }
        }

        public void SetFieldType(string text)
        {
            ComboBox cmbType = (ComboBox)_userControl.Controls.Find("comboBox_fieldType", true).FirstOrDefault();
            if (cmbType.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetFieldValue);
                cmbType.Invoke(d, new object[] { text });
            }
            else
            {
                cmbType.Text = text;
            }
        }

        public void SetFieldValue(string text)
        {
            TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true).FirstOrDefault();
            if (txtFieldValue.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetFieldValue);
                txtFieldValue.Invoke(d, new object[] { text });
            }
            else
            {
                txtFieldValue.Text = text;
            }
        }

        private string GetFieldType()
        {
            ComboBox cmbType = (ComboBox)_userControl.Controls.Find("comboBox_fieldType", true).FirstOrDefault();
            string outVal = string.Empty;
            if (cmbType.InvokeRequired)
            {
                cmbType.Invoke((MethodInvoker)delegate()
                {
                    outVal = cmbType.Text;
                });
            }
            else
            {
                outVal = cmbType.Text;
            }
            return outVal;
        }

        private string GetFieldID()
        {
            TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true).FirstOrDefault();
            string outVal = string.Empty;
            if (txtFieldID.InvokeRequired)
            {
                txtFieldID.Invoke((MethodInvoker)delegate()
                {
                    outVal = txtFieldID.Text;
                });
            }
            else
            {
                outVal = txtFieldID.Text;
            }
            return outVal;
        }

        private string GetFieldVal()
        {
            TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true).FirstOrDefault();
            string outVal = string.Empty;
            if (txtFieldValue.InvokeRequired)
            {
                txtFieldValue.Invoke((MethodInvoker)delegate()
                {
                    outVal = txtFieldValue.Text;
                });
            }
            else
            {
                outVal = txtFieldValue.Text;
            }
            return outVal;
        }

        public static void KendoSelectByValue(IWebDriver driver, IWebElement select, string value, string id)
        {
            var selectElement = new SelectElement(select);
            for (int i = 0; i < selectElement.Options.Count; i++)
            {
                if (selectElement.Options[i].GetAttribute("value") == value || selectElement.Options[i].GetAttribute("text") == value)
                {
                    ((IJavaScriptExecutor)driver).ExecuteScript(
                        String.Format("$($('#{0}').parent().find('select')[0]).data('kendoComboBox').select({1});", id, i));
                    break;
                }
            }
        }

        public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
        {
            Log.WriteLog("Edit search field action", Color.Blue);
            //Append new line
            Log.WriteLog("", Color.White);

            sw.Restart();
            try
            {

                string fieldId = "OMW_SearchTextBox_" + GetFieldID();
                string fieldValue = GetFieldVal();
                string fieldType = GetFieldType();
                IWebElement element = null;

                switch (fieldType)
                {
                    case "Text":
                        element = browser.FindElement(By.Id(fieldId));
                        if (element != null && 0 == String.Compare(element.TagName, "INPUT", true))
                        {
                            element.SendKeys(fieldValue);
                        }
                        else
                        {
                            sw.Stop();
                            throw new TestErrorException("Element of type input with Id:" + fieldId + " not found", Result.Continue);
                        }
                        break;
                    case "PickerDate-Combo":
                        fieldId = "OMW_SearchDatePickerDate_" + GetFieldID();
                        var element1 = browser.FindElement(By.Id(fieldId));
                        var selectElement = element1.FindElement(By.XPath("../span/select"));
                        KendoSelectByValue(browser, selectElement, fieldValue, fieldId);
                        break;
                    default:
                        break;
                }

                

                System.Threading.Thread.Sleep(1500);

                var serachButton = browser.FindElement(By.Id("searchButton"));
                if (serachButton != null)
                {
                    serachButton.Click();
                }
                else
                {
                    sw.Stop();
                    throw new TestErrorException("Element of type input with Id:" + "searchButton" + " not found", Result.Continue);
                }

                System.Threading.Thread.Sleep(1500);
            }
            catch (Exception ex)
            {
                throw new TestErrorException(ex.Message, ex, Result.StopAndCloseBrowser);
            }


            return Action.Result.Continue;
        }

        
    }
}
