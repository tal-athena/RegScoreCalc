using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using OpenQA.Selenium;

using WebAppTest.Action_Controls;
using WebAppTest.Code;

using Action = WebAppTest.Code.Action;

namespace WebAppTest.Action_Code
{
	public class EditCollectionRecord : Action
	{
		#region Ctors

		public EditCollectionRecord() : base(new EditCollectionRecordControl(), "Edit Record")
		{
			
		}

		#endregion

		#region Overrides

		public override XmlNode WriteToXML(XmlDocument document)
		{
			XmlNode node = document.CreateElement("EditCollectionRecord");

			TextBox txtRecordID = (TextBox) _userControl.Controls.Find("txtRecordID", true)
			                                       .FirstOrDefault();
			TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true)
			                                      .FirstOrDefault();
			TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true)
			                                         .FirstOrDefault();

			string recordId = txtRecordID.Text;
			string fieldId = txtFieldID.Text;
			string fieldValue = txtFieldValue.Text;

			XmlNode recordIDNode = document.CreateElement("RecordID");
			XmlNode fieldIDNode = document.CreateElement("FieldID");
			XmlNode fieldValueNode = document.CreateElement("FieldValue");

			XmlNode recordIdValueNode = document.CreateNode(XmlNodeType.Text, "", "");
			recordIdValueNode.Value = recordId;
			recordIDNode.AppendChild(recordIdValueNode);

			XmlNode fieldIdValueNode = document.CreateNode(XmlNodeType.Text, "", "");
			fieldIdValueNode.Value = fieldId;
			fieldIDNode.AppendChild(fieldIdValueNode);

			XmlNode fieldValueNodeValue = document.CreateNode(XmlNodeType.Text, "", "");
			fieldValueNodeValue.Value = fieldValue;
			fieldValueNode.AppendChild(fieldValueNodeValue);

			node.AppendChild(recordIDNode);
			node.AppendChild(fieldIDNode);
			node.AppendChild(fieldValueNode);
			return node;
		}

		public override void ReadFromXML(XmlNode element)
		{
			string RecordID = element.SelectSingleNode("RecordID")
			                         .FirstChild.Value;
			string FieldID = element.SelectSingleNode("FieldID")
			                        .FirstChild.Value;
			string FieldValue = element.SelectSingleNode("FieldValue")
			                           .FirstChild.Value;

			TextBox txtRecordID = (TextBox)_userControl.Controls.Find("txtRecordID", true)
			                                       .FirstOrDefault();
			TextBox txtFieldID = (TextBox)_userControl.Controls.Find("txtFieldID", true)
			                                      .FirstOrDefault();
			TextBox txtFieldValue = (TextBox)_userControl.Controls.Find("txtFieldValue", true)
			                                         .FirstOrDefault();

			txtRecordID.Text = RecordID;
			txtFieldID.Text = FieldID;
			txtFieldValue.Text = FieldValue;
		}

		public override Result Run2(IWebDriver browser, Stopwatch sw, string URL)
		{
			return Action.Result.Continue;
		}

		

		#endregion
	}
}