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
    public class EditStoryFields : Action
    {
	    public EditStoryFields() : base(new EditStoryFieldsControl(), "Header Fields")
        {
            
        }

        public override XmlNode WriteToXML(XmlDocument document)
        {
            XmlNode node = document.CreateElement("EditStoryFields");

            TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true).FirstOrDefault();
            TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true).FirstOrDefault();

            string fieldId = txtFieldID.Text;
            string fieldValue = txtFieldValue.Text;

            XmlNode fieldIDNode = document.CreateElement("FieldID");
            XmlNode fieldValueNode = document.CreateElement("FieldValue");

            XmlNode fieldIdValueNode = document.CreateNode(XmlNodeType.Text, "", "");
            fieldIdValueNode.Value = fieldId;
            fieldIDNode.AppendChild(fieldIdValueNode);

            XmlNode fieldValueNodeValue = document.CreateNode(XmlNodeType.Text, "", "");
            fieldValueNodeValue.Value = fieldValue;
            fieldValueNode.AppendChild(fieldValueNodeValue);

            node.AppendChild(fieldIDNode);
            node.AppendChild(fieldValueNode);
            return node;
        }

        public override void ReadFromXML(XmlNode element)
        {
            string FieldID = element.SelectSingleNode("FieldID").FirstChild.Value;
            string FieldValue = element.SelectSingleNode("FieldValue").FirstChild.Value;

            TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true).FirstOrDefault();
            TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true).FirstOrDefault();

            txtFieldID.Text = FieldID;
            txtFieldValue.Text = FieldValue;
        }

        public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
        {
            Log.WriteLog("Edit header field action", Color.Blue);
            //Append new line
            Log.WriteLog("", Color.White);

            sw.Restart();
            try
            {
                TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true).FirstOrDefault();
                TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true).FirstOrDefault();

                var rng = new Random();
                int rvalue = rng.Next(100000);
                string rtext = rvalue.ToString("00000");

                string fieldId = "OMW_SearchTextBox_" + txtFieldID.Text;
                string fieldValue = txtFieldValue.Text + rtext;

                var element = browser.FindElement(By.Id(fieldId));
                if (element != null && 0 == String.Compare(element.TagName, "INPUT", true))
                {
                    //element.SendKeys(fieldValue);

                    SetHeaderField(browser, "#" + fieldId, txtFieldID.Text, fieldValue);
                    System.Threading.Thread.Sleep(1000);
                    ResetStoryEditorDirty(browser);
                    sw.Stop();

                }
                else
                {
                    sw.Stop();
                    throw new TestErrorException("Element of type input with Id:" + fieldId + " not found", Result.Continue);
                }

                var url = browser.Url;
                if (browser.IsTextPresent("Server Error"))
                {
                    throw new TestErrorException("Server Error.", Result.Continue);
                }
            }
            catch (Exception ex)
            {
                throw new TestErrorException(ex.Message, ex, Result.StopAndCloseBrowser);
            }


            return Action.Result.Continue;
        }

        
    }
}
