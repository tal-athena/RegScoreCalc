using MediTermBrowser.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MediTermBrowser
{
    public partial class MainForm : Form
    {
        #region "Variables"

        private string searchText;
		private string baseAddress = "http://localhost:8088/snomedct/";

		private List<Concept> concepts = new List<Concept>();
        private ImageList treeImageList = new ImageList();

        Font tagFont = new Font("Helvetica", 8, FontStyle.Bold);
        
        Color lastNodeColor = Color.Orange;


        #endregion

        #region "Constructors"

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(string SearchText, string BaseAddress)
        {
            InitializeComponent();

            baseAddress = BaseAddress;

            if (String.IsNullOrEmpty(SearchText))
            {
                searchText = "Cardiac structure";
            }
            else
            {
                searchText = SearchText;
            }

            txtSearch.Text = searchText;
        }

		#endregion

		#region "Implementation"

		private void MainForm_Load(object sender, EventArgs e)
		{
			if (!DoSearch(txtSearch.Text))
				this.Close();
		}

		private bool DoSearch(string searchText)
        {
			try
			{
				string endPoint = baseAddress + Uri.EscapeDataString(searchText);

                // Retrieve XML document  
                XmlDocument doc = new XmlDocument();
                doc.Load(endPoint);

                concepts.Clear();
                treeViewNavigation.Nodes.Clear();

                XmlNode ConceptsListNode =
                doc.SelectSingleNode("/concepts");
                XmlNodeList ConceptNodeList =
                    ConceptsListNode.SelectNodes("concept");

                foreach (XmlNode node in ConceptNodeList)
                {
                    Concept concept = new Concept();
                    concept.Code = node.SelectSingleNode("code").InnerText;
                    concept.Term = node.SelectSingleNode("term").InnerText;
                    concept.Synonims = new List<string>();

                    XmlNodeList SynonimsNodeList = node.SelectSingleNode("synonyms").SelectNodes("synonym");

                    foreach (XmlNode synonim in SynonimsNodeList)
                    {
                        concept.Synonims.Add(synonim.InnerText);
                    }

                    GetConceptSubItems(concept);

                    concepts.Add(concept);
                }

                ShowData();

				return true;

			}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

	            return false;
            }
        }

        private void GetConceptSubItems(Concept concept)
        {
            //Get parents
            GetConceptData(concept, "parents", concept.Parents);

            //GetChildren
            GetConceptData(concept, "children", concept.Children);

            //Get part of
            GetConceptData(concept, "part_of", concept.PartOf);

            //Get inverse part of
            GetConceptData(concept, "INVERSE_part_of", concept.InversePartOf);

            //Get ancestor parts
            GetConceptData(concept, "ancestor_parts", concept.AncestorParts);

            //Get descendant parts
            GetConceptData(concept, "descendant_parts", concept.DescendantParts);

            //Get relations
            GetConceptRelations(concept, "otherrelations", concept.Relations);
        }

        public void GetConceptData(Concept concept, string addr, List<Concept> list)
        {
            try
            {
                string endPoint = baseAddress + concept.Code + "/" + addr;

                // Retrieve XML document  
                XmlDocument doc = new XmlDocument();
                doc.Load(endPoint);


                XmlNode ConceptsListNode =
                doc.SelectSingleNode("/concepts");
                XmlNodeList ConceptNodeList =
                    ConceptsListNode.SelectNodes("concept");

                foreach (XmlNode node in ConceptNodeList)
                {
                    Concept conc = new Concept();
                    conc.Code = node.SelectSingleNode("code").InnerText;
                    conc.Term = node.SelectSingleNode("term").InnerText;
                    conc.Synonims = new List<string>();

                    XmlNodeList SynonimsNodeList = node.SelectSingleNode("synonyms").SelectNodes("synonym");

                    foreach (XmlNode synonim in SynonimsNodeList)
                    {
                        conc.Synonims.Add(synonim.InnerText);
                    }

                    list.Add(conc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetConceptRelations(Concept concept, string addr, List<Relation> list)
        {
            try
            {
                string endPoint = baseAddress + concept.Code + "/" + addr;

                // Retrieve XML document  
                XmlDocument doc = new XmlDocument();
                doc.Load(endPoint);


                XmlNode RelationsListNode = doc.SelectSingleNode("/relations");
                XmlNodeList RelationNodeList = RelationsListNode.SelectNodes("relation");

                foreach (XmlNode node in RelationNodeList)
                {
                    Relation relation = new Relation();
                    relation.RelationName = node.SelectSingleNode("rname").InnerText;

                    XmlNode ConceptsListNode = node.SelectSingleNode("concepts");
                    XmlNodeList ConceptNodeList = ConceptsListNode.SelectNodes("concept");

                    foreach (XmlNode cNode in ConceptNodeList)
                    {
                        Concept conc = new Concept();
                        conc.Code = cNode.SelectSingleNode("code").InnerText;
                        conc.Term = cNode.SelectSingleNode("term").InnerText;
                        conc.Synonims = new List<string>();

                        XmlNodeList SynonimsNodeList = cNode.SelectSingleNode("synonyms").SelectNodes("synonym");

                        foreach (XmlNode synonim in SynonimsNodeList)
                        {
                            conc.Synonims.Add(synonim.InnerText);
                        }

                        relation.Concepts.Add(conc);
                    }

                    list.Add(relation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ShowData()
        {
            try
            {
                treeImageList.Images.Add("concept",Properties.Resources.concept);

            }
            catch { }

            treeViewNavigation.ImageList = treeImageList;

            treeViewNavigation.SelectedImageKey = "Copy";
            treeViewNavigation.SelectedImageIndex = 1;

            foreach (var concept in concepts)
            {
                int imageIndex = GetImageIndex(concept.Term, concept.Code);

                TreeNode node = new TreeNode(concept.Term, imageIndex, imageIndex);

                node.BackColor = Color.Orange;

                node.Tag = concept;
                treeViewNavigation.Nodes.Add(node);

                //Now add parents, children, part_of .....

                AddTreeItemsToConcept(concept, node);

                if (treeViewNavigation.Nodes[0] != null)
                {
                    treeViewNavigation.SelectedNode = treeViewNavigation.Nodes[0];
                }

            }
        }

        private static void AddTreeItemsToConcept(Concept concept, TreeNode node)
        {
            //Parents
            AddSubTree(node, "Parents", concept.Parents);

            //Children
            AddSubTree(node, "Children", concept.Children);

            //Part of
            AddSubTree(node, "part_of", concept.PartOf);

            //INVERSE_part_of
            AddSubTree(node, "INVERSE_part_of", concept.InversePartOf);

            //Ancestors
            AddSubTree(node, "ancestor_parts", concept.AncestorParts);

            //Descendants
            AddSubTree(node, "descendant_parts", concept.DescendantParts);


            AddOtherRelations(node, "relations", concept.Relations);
        }

        private string GetIconName(string term, string code)
        {
            return "";
        }
        public static int GetImageIndex(string term, string code)
        {
            //Check if the image exists in the imageList

            //if it doesnt exists add it to that and return index

            return 0;
        }

        private static void AddOtherRelations(TreeNode node, string name, List<Relation> list)
        {
            TreeNode parentNode = new TreeNode(name, 9999, 9999);
            //parentNode.Text = name;

            parentNode.BackColor = Color.LightBlue;

            node.Nodes.Add(parentNode);
            foreach (var item in list)
            {
                TreeNode parentN = new TreeNode(item.RelationName, 9999, 9999);
                //parentN.Text = item.RelationName;

                parentN.BackColor = Color.LightGreen;

                parentN.Tag = item;

                foreach (Concept concept in item.Concepts)
                {
                    int imageIndex = GetImageIndex(concept.Term, concept.Code);

                    TreeNode Cnode = new TreeNode(concept.Term, imageIndex, imageIndex);
                    //Cnode.Text = concept.Term;

                    Cnode.BackColor = Color.Orange;

                    Cnode.Tag = concept;

                    parentN.Nodes.Add(Cnode);
                }

                parentNode.Nodes.Add(parentN);
            }

        }

        private static void AddSubTree(TreeNode node, string name, List<Concept> list)
        {
            TreeNode parentNode = new TreeNode(name, 9999, 9999);

            parentNode.BackColor = Color.LightGreen;

            //parentNode.Text = name;
            node.Nodes.Add(parentNode);
            foreach (var item in list)
            {
                int imageIndex = GetImageIndex(item.Term, item.Code);

                TreeNode parentN = new TreeNode(item.Term, imageIndex, imageIndex);
                parentN.Tag = item;

                parentN.BackColor = Color.Orange;

                parentNode.Nodes.Add(parentN);
            }
        }

        private void ShowDetailsOfConcept(Concept concept)
        {
            //Change details
            txtDetailsCode.Text = concept.Code;
            txtDetailsTerm.Text = concept.Term;

            //Change synonims
            listBoxDetailsSynonims.Items.Clear();
            foreach (var synonim in concept.Synonims)
            {
                listBoxDetailsSynonims.Items.Add(synonim);
            }


            SetDataInTab(concept.Parents, listViewParents);
            SetDataInTab(concept.Children, listViewChildren);
            SetDataInTab(concept.PartOf, listViewPartOf);
            SetDataInTab(concept.InversePartOf, listViewInversePartOf);
            SetDataInTab(concept.AncestorParts, listViewAncestorParts);
            SetDataInTab(concept.DescendantParts, listViewDescendantParts);



            //Add data to the other relations tab
            listViewOtherRelations.Items.Clear();
            foreach (Relation relation in concept.Relations)
            {
                ListViewItem lwi = new ListViewItem();
                lwi.Text = relation.RelationName;
                lwi.Tag = relation;
                listViewOtherRelations.Items.Add(lwi);
                foreach (Concept con in relation.Concepts)
                {
                    //System.Windows.Forms.ListViewItem.ListViewSubItem lwi1 = new System.Windows.Forms.ListViewItem.ListViewSubItem();
                    ListViewItem lwi1 = new ListViewItem();
                    lwi1.Text = "           " + con.Term;
                    lwi1.Tag = con;

                    listViewOtherRelations.Items.Add(lwi1);

                    //lwi.SubItems.Add(con.Term);
                    //lwi.SubItems.Add(lwi1);
                }
                //listViewOtherRelations.Items.Add(lwi);
            }



        }

        public void SetDataInTab(List<Concept> conceptList, ListView listView)
        {
            listView.Items.Clear();

            foreach (Concept concept1 in conceptList)
            {
                ListViewItem lwi = new ListViewItem();
                lwi.Text = concept1.Term;
                lwi.Tag = concept1;
                listView.Items.Add(lwi);
            }
        }

        #endregion

        #region "Events"

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtSearch.Text))
            {
                DoSearch(txtSearch.Text);
            }
            else
            {
                //Remove all data from all controls
                treeViewNavigation.Nodes.Clear();

                txtDetailsTerm.Text = "";
                txtDetailsCode.Text = "";
                listBoxDetailsSynonims.Items.Clear();

                listViewAncestorParts.Items.Clear();
                listViewChildren.Items.Clear();
                listViewDescendantParts.Items.Clear();
                listViewInversePartOf.Items.Clear();
                listViewOtherRelations.Items.Clear();
                listViewParents.Items.Clear();
                listViewPartOf.Items.Clear();
            }
        }

        private void btnCopyTermToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtDetailsTerm.Text);
        }
        private void btnCopySynonimsToClipboard_Click(object sender, EventArgs e)
        {
            string synonims = "";
            foreach (var item in listBoxDetailsSynonims.Items)
            {
                synonims += item.ToString() + ", ";
            }
            if (!String.IsNullOrEmpty(synonims))
            {
                synonims = synonims.Substring(0, synonims.Length - 2);
            }

            Clipboard.SetText(synonims);
        }

        private void btnCopySelectedTermToClipboard_Click(object sender, EventArgs e)
        {
            var currentControls = tabElements.SelectedTab.Controls;
            //Get listView from current tab
            foreach (var control in currentControls)
            {
                try
                {
                    ListView lw = (ListView)control;
                    if (lw != null)
                    {
                        if (lw.SelectedItems.Count > 0)
                        {
                            var selectedItem = lw.SelectedItems[0];

                            Clipboard.SetText(selectedItem.Text.TrimStart(' '));
                        }
                        else
                        {
                            //Put something else in clipboard
                        }

                        break;
                    }
                }
                catch (Exception ex)
                {
                    //Clipboard.SetText("");
                }
            }
        }
        private void btnCopyAllTermsToClipboard_Click(object sender, EventArgs e)
        {
            var currentControls = tabElements.SelectedTab.Controls;
            //Get listView from current tab
            foreach (var control in currentControls)
            {
                try
                {
                    ListView lw = (ListView)control;
                    if (lw != null)
                    {
                        string items = "";
                        foreach (ListViewItem item in lw.Items)
                        {
                            items += item.Text.TrimStart(' ') + ", ";
                        }
                        if (!String.IsNullOrEmpty(items))
                        {
                            items = items.Substring(0, items.Length - 2);
                        }

                        Clipboard.SetText(items);
                        break;
                    }
                }
                catch { }
            }
        }
        private void btnSelectInTheNavigationTree_Click(object sender, EventArgs e)
        {
            var currentControls = tabElements.SelectedTab.Controls;
            foreach (var control in currentControls)
            {
                try
                {
                    ListView lw = (ListView)control;
                    if (lw != null)
                    {
                        if (lw.SelectedItems.Count > 0)
                        {
                            ListViewItem lwItem = lw.SelectedItems[0];
                            Concept selectedConcept = (Concept)lwItem.Tag;
                            if (selectedConcept != null)
                            {
                                //Find the selected item in tree view and select it
                                TreeNode currentNode = treeViewNavigation.SelectedNode;

                                //Get n-th node (the node position should be the same as the position of the tab)
                                TreeNode categoryNode = currentNode.Nodes[tabElements.SelectedIndex];
                                if (categoryNode.Nodes.Count > 0)
                                {
                                    if (tabElements.SelectedIndex == 6)
                                    {
                                        int relationIndex = -1;
                                        for (int i = 0; i < listViewOtherRelations.Items.Count; i++)
                                        {
                                            if (listViewOtherRelations.Items[i].Tag is Relation)
                                            {
                                                relationIndex++;
                                            }
                                            else if (((Concept)listViewOtherRelations.Items[i].Tag) == selectedConcept)
                                            {
                                                categoryNode = categoryNode.Nodes[relationIndex];
                                                break;
                                            }
                                        }
                                    }

                                    TreeNode newNode = categoryNode.Nodes[0];
                                    foreach (TreeNode node in categoryNode.Nodes)
                                    {
                                        if (node.Text == lwItem.Text.TrimStart(' '))
                                        {
                                            newNode = node;
                                            break;
                                        }
                                    }

                                    treeViewNavigation.SelectedNode = newNode;
                                    treeViewNavigation.Select();
                                    treeViewNavigation.Update();

                                }
                            }

                        }

                        break;
                    }
                }
                catch { }
            }
        }

        private void treeViewNavigation_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeViewNavigation.SelectedNode;
            lastNodeColor = node.BackColor;
            node.BackColor = SystemColors.Highlight;
            node.ForeColor = SystemColors.HighlightText;

            try
            {
                //try converting tag to concept
                //if it tag is concept then show details, else do not show changes
                Concept selectedConcept = (Concept)node.Tag;
                if (selectedConcept != null)
                {
                    ShowDetailsOfConcept(selectedConcept);
                }

            }
            catch { }
        }


        private void treeViewNavigation_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {

            //Remove icon from currently selected node
            TreeNode lastNode = treeViewNavigation.SelectedNode;
            if (lastNode != null)
            {
                lastNode.BackColor = lastNodeColor;
                lastNode.ForeColor = SystemColors.WindowText;
            }
           

            TreeNode node = e.Node;
            try
            {
                if (node.Nodes.Count == 0 && node.Level < 18)
                {
                    Concept concept = (Concept)node.Tag;
                    if (concept != null)
                    {
                        //Get data from server
                        GetConceptSubItems(concept);

                        //Show data
                        AddTreeItemsToConcept(concept, node);
                    }
                }
            }
            catch { }
        }

        private void tabElements_Selecting(object sender, TabControlCancelEventArgs e)
        {
            tabElements.TabPages[e.TabPageIndex].Controls.Add(toolStrip2);
        }

		#endregion
	}
}
