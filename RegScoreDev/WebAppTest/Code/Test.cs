using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;

using WebAppTest.Action_Code;

namespace WebAppTest.Code
{
    public static class Test
    {
		public static void SaveToXml(string fileName, List<Action> actionsList)
        {
            try
            {
                XmlDocument document = new XmlDocument();

                XmlNode rootNode = document.CreateNode(XmlNodeType.Element, "Actions", "");

                document.InsertAfter(rootNode, null);

	            foreach (var action in actionsList)
	            {
		            var element = (XmlElement) action.WriteToXML(document);
					element.SetAttribute("Title", action.Name);

					rootNode.AppendChild(element);
	            }

                document.Save(fileName);
            }
			catch (Exception ex)
			{
				Log.WriteLog(ex.Message, Color.Red);
			}
		}

        public static List<Action> ReadFromXml(string fileName)
        {
	        var actionsList = new List<Action>();

	        ///////////////////////////////////////////////////////////////////////////////

			try
            {
				var document = new XmlDocument();

                document.Load(fileName);

				foreach (XmlNode childNode in document.DocumentElement.ChildNodes)
                {
	                try
	                {
						var action = CreateAction(childNode.Name);

						var element = (XmlElement)childNode;
						action.Name = element.GetAttribute("Title");

						action.ReadFromXML(childNode);

		                ///////////////////////////////////////////////////////////////////////////////

						actionsList.Add(action);
						
					}
	                catch (Exception ex)
	                {
						Log.WriteLog(ex.Message, Color.Red);
					}
                }
            }
			catch (Exception ex)
			{
				Log.WriteLog(ex.Message, Color.Red);
			}

	        ///////////////////////////////////////////////////////////////////////////////

	        return actionsList;
        }

		// Works as class action factory
		public static Action CreateAction(String name)
		{
			if (name.Equals("Login"))
				return new Login();
			if (name.Equals("Logout"))
				return new Logout();
			if (name.Equals("LoadStory"))
				return new LoadStory();
            if (name.Equals("LoadSearch"))
                return new LoadSearch();
			if (name.Equals("LoadCollection"))
				return new LoadCollection();
			if (name.Equals("EditStoryFields"))
				return new EditStoryFields();
            if (name.Equals("EditSearchParam"))
                return new EditSearchParam();
			if (name.Equals("EditCollectionRecord"))
				return new EditCollectionRecord();
			if (name.Equals("RunScript"))
				return new RunScript();
			return null;
		}
	}
}
