using System;
using System.Windows.Forms;

using RegScoreCalc.Data;
using RegScoreCalc.MainDataSetTableAdapters;

namespace RegScoreCalc
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ribbon = new System.Windows.Forms.Ribbon();
            this.orbitemDocuments = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemDatabase = new System.Windows.Forms.RibbonOrbMenuItem();
	        this.orbitemOpenView = new System.Windows.Forms.RibbonOrbMenuItem();
			this.orbitemOpenDatabase = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemSaveDatabase = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemOpenDocuments = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemSaveDocuments = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemSaveAsDocuments = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemRegExps = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemOpenRegExps = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemSaveRegExps = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemSaveAsRegExps = new System.Windows.Forms.RibbonOrbMenuItem();
            this.ribbonOrbMenuSeparator = new System.Windows.Forms.RibbonSeparator();
            this.orbitemSaveAll = new System.Windows.Forms.RibbonOrbMenuItem();
            this.orbitemExit = new System.Windows.Forms.RibbonOrbOptionButton();
            this.orbitemClearHistory = new System.Windows.Forms.RibbonOrbOptionButton();
            this.tabHome = new System.Windows.Forms.RibbonTab();
            this.panelDocuments = new System.Windows.Forms.RibbonPanel();
            this.panelDatabase = new System.Windows.Forms.RibbonPanel();
            this.btnOpenDatabase = new System.Windows.Forms.RibbonButton();
            this.btnSaveDatabase = new System.Windows.Forms.RibbonButton();
            this.btnOpenDocuments = new System.Windows.Forms.RibbonButton();
            this.btnSaveDocuments = new System.Windows.Forms.RibbonButton();
            this.panelRegExps = new System.Windows.Forms.RibbonPanel();
            this.btnOpenRegExps = new System.Windows.Forms.RibbonButton();
            this.btnSaveRegExps = new System.Windows.Forms.RibbonButton();
            this.panelGeneral = new System.Windows.Forms.RibbonPanel();
            this.btnSaveAll = new System.Windows.Forms.RibbonButton();
            this.panelViews = new System.Windows.Forms.RibbonPanel();
            this.panelFont = new System.Windows.Forms.RibbonPanel();
            this.panelAdmin = new System.Windows.Forms.RibbonPanel();
            this.btnPythonSettings = new System.Windows.Forms.RibbonButton();
            this.btnSelectFont = new System.Windows.Forms.RibbonButton();
            this.cmbViews = new System.Windows.Forms.RibbonComboBox();
            this.cmbLineSpacing = new System.Windows.Forms.RibbonComboBox();
            this.btnOpenView = new System.Windows.Forms.RibbonButton();
            this.btnSaveAsDocuments = new System.Windows.Forms.RibbonButton();
            this.btnSaveAsRegExps = new System.Windows.Forms.RibbonButton();
            this.sourceRegExp = new System.Windows.Forms.BindingSource(this.components);
            this.sourceEntities = new System.Windows.Forms.BindingSource(this.components);
            this.sourceColRegExp = new System.Windows.Forms.BindingSource(this.components);
            this.datasetMain = new RegScoreCalc.MainDataSet();
            this.sourceCategories = new System.Windows.Forms.BindingSource(this.components);
            this.sourceDocuments = new System.Windows.Forms.BindingSource(this.components);
            this.adapterRegExp = new RegScoreCalc.MainDataSetTableAdapters.RegExpTableAdapter();
            this.adapterEntities = new RegScoreCalc.MainDataSetTableAdapters.EntitiesTableAdapter();
            this.adapterColumns = new RegScoreCalc.MainDataSetTableAdapters.ColumnsTableAdapter();
			this.adapterColRegExp = new RegScoreCalc.MainDataSetTableAdapters.ColRegExpTableAdapter();
            this.adapterDocuments = new CustomOleDbDataAdapter();
            this.adapterRelations = new RegScoreCalc.MainDataSetTableAdapters.RelationsTableAdapter();
            this.adapterCategories = new RegScoreCalc.MainDataSetTableAdapters.CategoriesTableAdapter();
            this.sourceColorCodes = new System.Windows.Forms.BindingSource(this.components);
            this.adapterColorCodes = new RegScoreCalc.MainDataSetTableAdapters.ColorCodesTableAdapter();
            this.sourceRelations = new System.Windows.Forms.BindingSource(this.components);
			this.adapterDynamicColumns = new MainDataSetTableAdapters.DynamicColumnsTableAdapter();
			this.adapterDynamicColumnCategories = new MainDataSetTableAdapters.DynamicColumnCategoriesTableAdapter();
            
            this.adapterColScript = new MainDataSetTableAdapters.ColScriptTableAdapter();
            this.sourceColScript = new System.Windows.Forms.BindingSource(this.components);

            this.adapterColPython = new MainDataSetTableAdapters.ColPythonTableAdapter();
            this.sourceColPython = new System.Windows.Forms.BindingSource(this.components);
            //
            this.datasetBilling = new BillingDataSet();
            this.adapterDocumentsBilling = new Data.BillingDataSetTableAdapters.DocumentsTableAdapter();
            this.sourceDocumentsBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterBillingBilling = new Data.BillingDataSetTableAdapters.BillingTableAdapter();
            this.sourceBillingBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterRegExpBilling = new Data.BillingDataSetTableAdapters.RegExpTableAdapter();
            this.sourceRegExpBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterCategoriesBilling = new Data.BillingDataSetTableAdapters.CategoriesTableAdapter();
            this.sourceCategoriesBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterColorCodesBilling = new Data.BillingDataSetTableAdapters.ColorCodesTableAdapter();
            this.sourceColorCodesBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterRelationsBilling = new Data.BillingDataSetTableAdapters.RelationsTableAdapter();
            this.sourceRelationsBilling = new System.Windows.Forms.BindingSource(this.components);

            this.adapterICD9GroupsBilling = new Data.BillingDataSetTableAdapters.ICD9GroupsTableAdapter();
            this.sourceICD9GroupsBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterICDCodesBilling = new Data.BillingDataSetTableAdapters.ICDCodesTableAdapter();
            this.sourceICDCodesBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterICDFiltersBilling = new Data.BillingDataSetTableAdapters.ICDFiltersTableAdapter();
            this.sourceICDFiltersBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterDocsToFiltersBilling = new Data.BillingDataSetTableAdapters.DocsToFiltersTableAdapter();
            this.sourceDocsToFiltersBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterCategoryToFilterExclusionBilling = new Data.BillingDataSetTableAdapters.CategoryToFilterExclusionTableAdapter();
            this.sourceCategoryToFilterExclusionBilling = new System.Windows.Forms.BindingSource(this.components);
            this.adapterRegexpToGroupsBilling = new Data.BillingDataSetTableAdapters.RegexpToGroupsTableAdapter();
            this.sourceRegexpToGroupsBilling = new System.Windows.Forms.BindingSource(this.components);

            this.adapterColScriptBilling = new Data.BillingDataSetTableAdapters.ColScriptTableAdapter();
            this.sourceColScript = new System.Windows.Forms.BindingSource(this.components);

            this.adapterReviewMLDocumentsNew = new CustomOleDbDataAdapter();
	        this.adapterReviewMLDocuments = new ReviewMLDocumentsTableAdapter();

			this.sourceReviewMLDocuments = new System.Windows.Forms.BindingSource(this.components);
	        this.sourceReviewMLDocumentsNew = new System.Windows.Forms.BindingSource(this.components);

			this.adapterColScriptBilling = new Data.BillingDataSetTableAdapters.ColScriptTableAdapter();
            this.sourceColScriptBilling = new System.Windows.Forms.BindingSource(); 

            //
            ((System.ComponentModel.ISupportInitialize)(this.sourceRegExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceEntities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColRegExp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datasetMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceCategories)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColorCodes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRelations)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDocumentsBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRegExpBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceCategoriesBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColorCodesBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRelationsBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceReviewMLDocuments)).BeginInit();
	        ((System.ComponentModel.ISupportInitialize)(this.sourceReviewMLDocumentsNew)).BeginInit();

			((System.ComponentModel.ISupportInitialize)(this.sourceICD9GroupsBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceICDCodesBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceICDFiltersBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDocsToFiltersBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRegexpToGroupsBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceCategoryToFilterExclusionBilling)).BeginInit();

            ((System.ComponentModel.ISupportInitialize)(this.sourceColScriptBilling)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.BorderMode = System.Windows.Forms.RibbonWindowMode.InsideWindow;
            this.ribbon.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.Minimized = false;
            this.ribbon.Name = "ribbon";
            // 
            // 
            // 
            this.ribbon.OrbDropDown.BorderRoundness = 8;
            this.ribbon.OrbDropDown.Location = new System.Drawing.Point(0, 0);
	        this.ribbon.OrbDropDown.MenuItems.Add(this.orbitemOpenView);
	        this.ribbon.OrbDropDown.MenuItems.Add(new RibbonSeparator());
			this.ribbon.OrbDropDown.MenuItems.Add(this.orbitemDatabase);
            this.ribbon.OrbDropDown.MenuItems.Add(this.orbitemDocuments);
            this.ribbon.OrbDropDown.MenuItems.Add(this.orbitemRegExps);
            this.ribbon.OrbDropDown.MenuItems.Add(this.ribbonOrbMenuSeparator);
            this.ribbon.OrbDropDown.MenuItems.Add(this.orbitemSaveAll);
            this.ribbon.OrbDropDown.Name = "";
            this.ribbon.OrbDropDown.OptionItems.Add(this.orbitemExit);
            this.ribbon.OrbDropDown.OptionItems.Add(this.orbitemClearHistory);
            this.ribbon.OrbDropDown.Size = new System.Drawing.Size(527, 207);
            this.ribbon.OrbDropDown.TabIndex = 0;
            this.ribbon.OrbImage = global::RegScoreCalc.Properties.Resources.Orb;
            this.ribbon.QuickAccessVisible = false;
            // 
            // 
            // 
            this.ribbon.QuickAcessToolbar.AltKey = null;
            this.ribbon.QuickAcessToolbar.DropDownButtonVisible = false;
            this.ribbon.QuickAcessToolbar.Enabled = false;
            this.ribbon.QuickAcessToolbar.Image = null;
            this.ribbon.QuickAcessToolbar.Tag = null;
            this.ribbon.QuickAcessToolbar.Text = null;
            this.ribbon.QuickAcessToolbar.ToolTip = null;
            this.ribbon.QuickAcessToolbar.ToolTipImage = null;
            this.ribbon.QuickAcessToolbar.ToolTipTitle = null;
            this.ribbon.Size = new System.Drawing.Size(942, 138);
            this.ribbon.TabIndex = 0;
            this.ribbon.Tabs.Add(this.tabHome);
            this.ribbon.TabSpacing = 6;
			// 
			// orbitemOpenView
			// 
	        this.orbitemOpenView.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
	        this.orbitemOpenView.DropDownArrowSize = new System.Drawing.Size(5, 3);
	        this.orbitemOpenView.Image = Properties.Resources.orb_open_view;
	        this.orbitemOpenView.Style = RibbonButtonStyle.DropDown;
	        this.orbitemOpenView.Text = "Open View";
			// 
			// orbitemDatabase
			// 
			this.orbitemDatabase.AltKey = null;
            this.orbitemDatabase.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemDatabase.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemDatabase.DropDownItems.Add(this.orbitemOpenDatabase);
            this.orbitemDatabase.DropDownItems.Add(this.orbitemSaveDatabase);
            this.orbitemDatabase.Image = global::RegScoreCalc.Properties.Resources.orb_empty;
            this.orbitemDatabase.Style = System.Windows.Forms.RibbonButtonStyle.DropDown;
            this.orbitemDatabase.Tag = null;
            this.orbitemDatabase.Text = "Database";
            this.orbitemDatabase.ToolTip = null;
            this.orbitemDatabase.ToolTipImage = null;
            this.orbitemDatabase.ToolTipTitle = null;
			// 
			// orbitemDocuments
			// 
			this.orbitemDocuments.AltKey = null;
            this.orbitemDocuments.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemDocuments.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemDocuments.DropDownItems.Add(this.orbitemOpenDocuments);
            this.orbitemDocuments.DropDownItems.Add(this.orbitemSaveDocuments);
            this.orbitemDocuments.DropDownItems.Add(this.orbitemSaveAsDocuments);
            this.orbitemDocuments.Image = global::RegScoreCalc.Properties.Resources.orb_empty;
			this.orbitemDocuments.Style = System.Windows.Forms.RibbonButtonStyle.DropDown;
            this.orbitemDocuments.Tag = null;
            this.orbitemDocuments.Text = "Documents Database";
            this.orbitemDocuments.ToolTip = null;
            this.orbitemDocuments.ToolTipImage = null;
            this.orbitemDocuments.ToolTipTitle = null;
            // 
            // orbitemOpenDocuments
            // 
            this.orbitemOpenDocuments.AltKey = null;
            this.orbitemOpenDocuments.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemOpenDocuments.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemOpenDocuments.Image = global::RegScoreCalc.Properties.Resources.Open;
            this.orbitemOpenDocuments.SmallImage = global::RegScoreCalc.Properties.Resources.Open;
            this.orbitemOpenDocuments.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemOpenDocuments.Tag = null;
            this.orbitemOpenDocuments.Text = "Open";
            this.orbitemOpenDocuments.ToolTip = null;
            this.orbitemOpenDocuments.ToolTipImage = null;
            this.orbitemOpenDocuments.ToolTipTitle = null;
            this.orbitemOpenDocuments.Click += new System.EventHandler(this.orbitemOpenDocuments_Click);
            // 
            // orbitemSaveDocuments
            // 
            this.orbitemSaveDocuments.AltKey = null;
            this.orbitemSaveDocuments.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemSaveDocuments.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemSaveDocuments.Image = global::RegScoreCalc.Properties.Resources.Save;
            this.orbitemSaveDocuments.SmallImage = global::RegScoreCalc.Properties.Resources.Save;
            this.orbitemSaveDocuments.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemSaveDocuments.Tag = null;
            this.orbitemSaveDocuments.Text = "Save";
            this.orbitemSaveDocuments.ToolTip = null;
            this.orbitemSaveDocuments.ToolTipImage = null;
            this.orbitemSaveDocuments.ToolTipTitle = null;
            this.orbitemSaveDocuments.Click += new System.EventHandler(this.orbitemSaveDocuments_Click);
            // 
            // orbitemOpenDatabase
            // 
            this.orbitemOpenDatabase.AltKey = null;
            this.orbitemOpenDatabase.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemOpenDatabase.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemOpenDatabase.Image = global::RegScoreCalc.Properties.Resources.Open;
            this.orbitemOpenDatabase.SmallImage = global::RegScoreCalc.Properties.Resources.Open;
            this.orbitemOpenDatabase.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemOpenDatabase.Tag = null;
            this.orbitemOpenDatabase.Text = "Open";
            this.orbitemOpenDatabase.ToolTip = null;
            this.orbitemOpenDatabase.ToolTipImage = null;
            this.orbitemOpenDatabase.ToolTipTitle = null;
            this.orbitemOpenDatabase.Click += new System.EventHandler(this.btnOpenDatabase_Click);
            // 
            // orbitemSaveDatabase
            // 
            this.orbitemSaveDatabase.AltKey = null;
            this.orbitemSaveDatabase.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemSaveDatabase.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemSaveDatabase.Image = global::RegScoreCalc.Properties.Resources.Save;
            this.orbitemSaveDatabase.SmallImage = global::RegScoreCalc.Properties.Resources.Save;
            this.orbitemSaveDatabase.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemSaveDatabase.Tag = null;
            this.orbitemSaveDatabase.Text = "Save";
            this.orbitemSaveDatabase.ToolTip = null;
            this.orbitemSaveDatabase.ToolTipImage = null;
            this.orbitemSaveDatabase.ToolTipTitle = null;
            this.orbitemSaveDatabase.Click += new System.EventHandler(this.btnSaveDatabase_Click);
            this.orbitemSaveDatabase.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // orbitemSaveAsDocuments
            // 
            this.orbitemSaveAsDocuments.AltKey = null;
            this.orbitemSaveAsDocuments.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemSaveAsDocuments.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemSaveAsDocuments.Image = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.orbitemSaveAsDocuments.SmallImage = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.orbitemSaveAsDocuments.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemSaveAsDocuments.Tag = null;
            this.orbitemSaveAsDocuments.Text = "Save As";
            this.orbitemSaveAsDocuments.ToolTip = null;
            this.orbitemSaveAsDocuments.ToolTipImage = null;
            this.orbitemSaveAsDocuments.ToolTipTitle = null;
            this.orbitemSaveAsDocuments.Click += new System.EventHandler(this.orbitemSaveAsDocuments_Click);
            this.orbitemSaveAsDocuments.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // orbitemRegExps
            // 
            this.orbitemRegExps.AltKey = null;
            this.orbitemRegExps.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemRegExps.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemRegExps.DropDownItems.Add(this.orbitemOpenRegExps);
            this.orbitemRegExps.DropDownItems.Add(this.orbitemSaveRegExps);
            this.orbitemRegExps.DropDownItems.Add(this.orbitemSaveAsRegExps);
            this.orbitemRegExps.Image = global::RegScoreCalc.Properties.Resources.orb_empty;
            this.orbitemRegExps.Style = System.Windows.Forms.RibbonButtonStyle.DropDown;
            this.orbitemRegExps.Tag = null;
            this.orbitemRegExps.Text = "RegExp Database";
            this.orbitemRegExps.ToolTip = null;
            this.orbitemRegExps.ToolTipImage = null;
            this.orbitemRegExps.ToolTipTitle = null;
            // 
            // orbitemOpenRegExps
            // 
            this.orbitemOpenRegExps.AltKey = null;
            this.orbitemOpenRegExps.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemOpenRegExps.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemOpenRegExps.Image = ((System.Drawing.Image)(resources.GetObject("orbitemOpenRegExps.Image")));
            this.orbitemOpenRegExps.SmallImage = global::RegScoreCalc.Properties.Resources.Open;
            this.orbitemOpenRegExps.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemOpenRegExps.Tag = null;
            this.orbitemOpenRegExps.Text = "Open";
            this.orbitemOpenRegExps.ToolTip = null;
            this.orbitemOpenRegExps.ToolTipImage = null;
            this.orbitemOpenRegExps.ToolTipTitle = null;
            this.orbitemOpenRegExps.Click += new System.EventHandler(this.orbitemOpenRegExps_Click);
            this.orbitemOpenRegExps.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // orbitemSaveRegExps
            // 
            this.orbitemSaveRegExps.AltKey = null;
            this.orbitemSaveRegExps.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemSaveRegExps.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemSaveRegExps.Image = global::RegScoreCalc.Properties.Resources.Save;
            this.orbitemSaveRegExps.SmallImage = global::RegScoreCalc.Properties.Resources.Save;
            this.orbitemSaveRegExps.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemSaveRegExps.Tag = null;
            this.orbitemSaveRegExps.Text = "Save";
            this.orbitemSaveRegExps.ToolTip = null;
            this.orbitemSaveRegExps.ToolTipImage = null;
            this.orbitemSaveRegExps.ToolTipTitle = null;
            this.orbitemSaveRegExps.Click += new System.EventHandler(this.orbitemSaveRegExps_Click);
            this.orbitemSaveRegExps.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // orbitemSaveAsRegExps
            // 
            this.orbitemSaveAsRegExps.AltKey = null;
            this.orbitemSaveAsRegExps.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemSaveAsRegExps.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemSaveAsRegExps.Image = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.orbitemSaveAsRegExps.SmallImage = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.orbitemSaveAsRegExps.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemSaveAsRegExps.Tag = null;
            this.orbitemSaveAsRegExps.Text = "Save As";
            this.orbitemSaveAsRegExps.ToolTip = null;
            this.orbitemSaveAsRegExps.ToolTipImage = null;
            this.orbitemSaveAsRegExps.ToolTipTitle = null;
            this.orbitemSaveAsRegExps.Click += new System.EventHandler(this.orbitemSaveAsRegExps_Click);
            this.orbitemSaveAsRegExps.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // ribbonOrbMenuSeparator
            // 
            this.ribbonOrbMenuSeparator.AltKey = null;
            this.ribbonOrbMenuSeparator.Enabled = false;
            this.ribbonOrbMenuSeparator.Image = null;
            this.ribbonOrbMenuSeparator.Tag = null;
            this.ribbonOrbMenuSeparator.Text = null;
            this.ribbonOrbMenuSeparator.ToolTip = null;
            this.ribbonOrbMenuSeparator.ToolTipImage = null;
            this.ribbonOrbMenuSeparator.ToolTipTitle = null;
            // 
            // orbitemSaveAll
            // 
            this.orbitemSaveAll.AltKey = null;
            this.orbitemSaveAll.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Left;
            this.orbitemSaveAll.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemSaveAll.Image = global::RegScoreCalc.Properties.Resources.orb_save_all;
            this.orbitemSaveAll.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemSaveAll.Tag = null;
            this.orbitemSaveAll.Text = "Save All";
            this.orbitemSaveAll.ToolTip = null;
            this.orbitemSaveAll.ToolTipImage = null;
            this.orbitemSaveAll.ToolTipTitle = null;
            this.orbitemSaveAll.Click += new System.EventHandler(this.orbitemSaveAll_Click);
            // 
            // orbitemExit
            // 
            this.orbitemExit.AltKey = null;
            this.orbitemExit.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.orbitemExit.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemExit.Image = global::RegScoreCalc.Properties.Resources.Exit;
            this.orbitemExit.SmallImage = global::RegScoreCalc.Properties.Resources.Exit;
            this.orbitemExit.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemExit.Tag = null;
            this.orbitemExit.Text = "Exit Score Calculator";
            this.orbitemExit.ToolTip = null;
            this.orbitemExit.ToolTipImage = null;
            this.orbitemExit.ToolTipTitle = null;
            this.orbitemExit.Click += new System.EventHandler(this.orbitemExit_Click);
            // 
            // orbitemClearHistory
            // 
            this.orbitemClearHistory.AltKey = null;
            this.orbitemClearHistory.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.orbitemClearHistory.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.orbitemClearHistory.Image = global::RegScoreCalc.Properties.Resources.clear_history;
            this.orbitemClearHistory.SmallImage = global::RegScoreCalc.Properties.Resources.clear_history;
            this.orbitemClearHistory.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.orbitemClearHistory.Tag = null;
            this.orbitemClearHistory.Text = "Clear History";
            this.orbitemClearHistory.ToolTip = null;
            this.orbitemClearHistory.ToolTipImage = null;
            this.orbitemClearHistory.ToolTipTitle = null;
            this.orbitemClearHistory.Click += new System.EventHandler(this.orbitemClearHistory_Click);
            // 
            // tabHome
            // 
            this.tabHome.Panels.Add(this.panelDatabase);
            this.tabHome.Panels.Add(this.panelDocuments);
            this.tabHome.Panels.Add(this.panelRegExps);
            this.tabHome.Panels.Add(this.panelGeneral);
            this.tabHome.Panels.Add(this.panelViews);
            this.tabHome.Panels.Add(this.panelAdmin);
            this.tabHome.Panels.Add(this.panelFont);
            this.tabHome.Tag = null;
            this.tabHome.Text = "Home";
            // 
            // panelDatabase
            // 
            this.panelDatabase.Items.Add(this.btnOpenDatabase);
            this.panelDatabase.Items.Add(this.btnSaveDatabase);
            this.panelDatabase.Tag = null;
            this.panelDatabase.Text = "Database";
            // 
            // panelDocuments
            // 
            this.panelDocuments.Items.Add(this.btnOpenDocuments);
            this.panelDocuments.Items.Add(this.btnSaveDocuments);
            this.panelDocuments.Tag = null;
            this.panelDocuments.Text = "Documents Database";
            // 
            // btnOpenDocuments
            // 
            this.btnOpenDocuments.AltKey = null;
            this.btnOpenDocuments.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnOpenDocuments.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnOpenDocuments.Image = global::RegScoreCalc.Properties.Resources.Open;
            this.btnOpenDocuments.SmallImage = global::RegScoreCalc.Properties.Resources.Open;
            this.btnOpenDocuments.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnOpenDocuments.Tag = null;
            this.btnOpenDocuments.Text = "Open";
            this.btnOpenDocuments.ToolTip = null;
            this.btnOpenDocuments.ToolTipImage = null;
            this.btnOpenDocuments.ToolTipTitle = null;
            this.btnOpenDocuments.Click += new System.EventHandler(this.btnOpenDocuments_Click);

            this.btnOpenDocuments.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // btnSaveDocuments
            // 
            this.btnSaveDocuments.AltKey = null;
            this.btnSaveDocuments.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnSaveDocuments.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnSaveDocuments.Image = global::RegScoreCalc.Properties.Resources.Save;
            this.btnSaveDocuments.SmallImage = global::RegScoreCalc.Properties.Resources.Save;
            this.btnSaveDocuments.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnSaveDocuments.Tag = null;
            this.btnSaveDocuments.Text = "Save";
            this.btnSaveDocuments.ToolTip = null;
            this.btnSaveDocuments.ToolTipImage = null;
            this.btnSaveDocuments.ToolTipTitle = null;
            this.btnSaveDocuments.Click += new System.EventHandler(this.btnSaveDocuments_Click);
            this.btnSaveDocuments.MouseEnter += this.RibbonButton_MouseEnter;
            //
            // btnOpenDatabase
            //
            this.btnOpenDatabase.AltKey = null;
            this.btnOpenDatabase.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnOpenDatabase.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnOpenDatabase.Image = global::RegScoreCalc.Properties.Resources.Open;
            this.btnOpenDatabase.SmallImage = global::RegScoreCalc.Properties.Resources.Open;
            this.btnOpenDatabase.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnOpenDatabase.Tag = null;
            this.btnOpenDatabase.Text = "Open";
            this.btnOpenDatabase.ToolTip = null;
            this.btnOpenDatabase.ToolTipImage = null;
            this.btnOpenDatabase.ToolTipTitle = null;
            this.btnOpenDatabase.Click += new System.EventHandler(this.btnOpenDatabase_Click);
            this.btnOpenDatabase.MouseEnter += this.RibbonButton_MouseEnter;
            //
            // btnSaveDatabase
            //
            this.btnSaveDatabase.AltKey = null;
            this.btnSaveDatabase.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnSaveDatabase.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnSaveDatabase.Image = global::RegScoreCalc.Properties.Resources.Save;
            this.btnSaveDatabase.SmallImage = global::RegScoreCalc.Properties.Resources.Save;
            this.btnSaveDatabase.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnSaveDatabase.Tag = null;
            this.btnSaveDatabase.Text = "Save";
            this.btnSaveDatabase.ToolTip = null;
            this.btnSaveDatabase.ToolTipImage = null;
            this.btnSaveDatabase.ToolTipTitle = null;
            this.btnSaveDatabase.Click += new System.EventHandler(this.btnSaveDatabase_Click);
            this.btnSaveDatabase.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // panelRegExps
            // 
            this.panelRegExps.Items.Add(this.btnOpenRegExps);
            this.panelRegExps.Items.Add(this.btnSaveRegExps);
            this.panelRegExps.Tag = null;
            this.panelRegExps.Text = "RegExp Database";
            // 
            // btnOpenRegExps
            // 
            this.btnOpenRegExps.AltKey = null;
            this.btnOpenRegExps.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnOpenRegExps.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnOpenRegExps.Image = global::RegScoreCalc.Properties.Resources.Open;
            this.btnOpenRegExps.SmallImage = global::RegScoreCalc.Properties.Resources.Open;
            this.btnOpenRegExps.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnOpenRegExps.Tag = null;
            this.btnOpenRegExps.Text = "Open";
            this.btnOpenRegExps.ToolTip = null;
            this.btnOpenRegExps.ToolTipImage = null;
            this.btnOpenRegExps.ToolTipTitle = null;
            this.btnOpenRegExps.Click += new System.EventHandler(this.btnOpenRegExps_Click);
            this.btnOpenRegExps.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // btnSaveRegExps
            // 
            this.btnSaveRegExps.AltKey = null;
            this.btnSaveRegExps.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnSaveRegExps.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnSaveRegExps.Image = global::RegScoreCalc.Properties.Resources.Save;
            this.btnSaveRegExps.SmallImage = global::RegScoreCalc.Properties.Resources.Save;
            this.btnSaveRegExps.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnSaveRegExps.Tag = null;
            this.btnSaveRegExps.Text = "Save";
            this.btnSaveRegExps.ToolTip = null;
            this.btnSaveRegExps.ToolTipImage = null;
            this.btnSaveRegExps.ToolTipTitle = null;
            this.btnSaveRegExps.Click += new System.EventHandler(this.btnSaveRegExps_Click);
            this.btnSaveRegExps.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // panelGeneral
            // 
            this.panelGeneral.Items.Add(this.btnSaveAll);
            this.panelGeneral.Tag = null;
            this.panelGeneral.Text = "General";
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.AltKey = null;
            this.btnSaveAll.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnSaveAll.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnSaveAll.Image = global::RegScoreCalc.Properties.Resources.SaveAll;
            this.btnSaveAll.SmallImage = global::RegScoreCalc.Properties.Resources.SaveAll;
            this.btnSaveAll.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnSaveAll.Tag = null;
            this.btnSaveAll.Text = "Save All";
            this.btnSaveAll.ToolTip = null;
            this.btnSaveAll.ToolTipImage = null;
            this.btnSaveAll.ToolTipTitle = null;
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            this.btnSaveAll.MouseEnter += this.RibbonButton_MouseEnter;

            // 
            // btnPythonSettings
            // 
            this.btnPythonSettings.AltKey = null;
            this.btnPythonSettings.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnPythonSettings.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnPythonSettings.Image = global::RegScoreCalc.Properties.Resources.python1;
            this.btnPythonSettings.SmallImage = global::RegScoreCalc.Properties.Resources.python1;
            this.btnPythonSettings.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnPythonSettings.Tag = null;
            this.btnPythonSettings.Text = "Python";
            this.btnPythonSettings.ToolTip = null;
            this.btnPythonSettings.ToolTipImage = null;
            this.btnPythonSettings.ToolTipTitle = null;
            this.btnPythonSettings.Click += new System.EventHandler(this.btnPythonSettings_Click);
            this.btnPythonSettings.MouseEnter += this.RibbonButton_MouseEnter;

            // 
            // panelAdmin
            // 
            this.panelAdmin.Items.Add(this.btnPythonSettings);            
            this.panelAdmin.Tag = null;
            this.panelAdmin.Text = "Admin";

            //
            // cmdLineSpacing
            //
            this.cmbLineSpacing.Text = "Line spacing:";
            this.cmbLineSpacing.TextBoxWidth = 50;
            this.cmbLineSpacing.AllowTextEdit = false;
            

            //
            // btnSelectFont
            //

            this.btnSelectFont.Image = Properties.Resources.SelectNotesFont;
            this.btnSelectFont.SmallImage = Properties.Resources.SelectNotesFont;
            this.btnSelectFont.Click += new EventHandler(OnSelectFont_Clicked);
            this.btnSelectFont.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // panelFont
            // 
            this.panelFont.Items.Add(this.cmbLineSpacing);
            this.panelFont.Items.Add(this.btnSelectFont);
            this.panelFont.Tag = null;
            this.panelFont.Text = "Font";

            // 
            // panelViews
            // 
            this.panelViews.Items.Add(this.cmbViews);
            this.panelViews.Items.Add(this.btnOpenView);
            this.panelViews.Tag = null;
            this.panelViews.Text = "Views";
            // 
            // cmbViews
            // 
            this.cmbViews.AllowTextEdit = false;
            this.cmbViews.AltKey = null;
            this.cmbViews.Image = null;
            this.cmbViews.Tag = null;
            this.cmbViews.Text = "Select view:";
            this.cmbViews.TextBoxText = null;
            this.cmbViews.TextBoxWidth = 170;
            this.cmbViews.ToolTip = null;
            this.cmbViews.ToolTipImage = null;
            this.cmbViews.ToolTipTitle = null;
            // 
            // btnOpenView
            // 
            this.btnOpenView.AltKey = null;
            this.btnOpenView.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnOpenView.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnOpenView.Image = global::RegScoreCalc.Properties.Resources.View;
            this.btnOpenView.SmallImage = global::RegScoreCalc.Properties.Resources.View;
            this.btnOpenView.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnOpenView.Tag = null;
            this.btnOpenView.Text = "Open";
            this.btnOpenView.ToolTip = null;
            this.btnOpenView.ToolTipImage = null;
            this.btnOpenView.ToolTipTitle = null;
            this.btnOpenView.Click += new System.EventHandler(this.btnOpenView_Click);
            this.btnOpenView.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // btnSaveAsDocuments
            // 
            this.btnSaveAsDocuments.AltKey = null;
            this.btnSaveAsDocuments.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnSaveAsDocuments.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnSaveAsDocuments.Image = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.btnSaveAsDocuments.SmallImage = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.btnSaveAsDocuments.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnSaveAsDocuments.Tag = null;
            this.btnSaveAsDocuments.Text = "Save As";
            this.btnSaveAsDocuments.ToolTip = null;
            this.btnSaveAsDocuments.ToolTipImage = null;
            this.btnSaveAsDocuments.ToolTipTitle = null;
            this.btnSaveAsDocuments.Click += new System.EventHandler(this.btnSaveAsDocuments_Click);
            this.btnSaveAsDocuments.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // btnSaveAsRegExps
            // 
            this.btnSaveAsRegExps.AltKey = null;
            this.btnSaveAsRegExps.DropDownArrowDirection = System.Windows.Forms.RibbonArrowDirection.Down;
            this.btnSaveAsRegExps.DropDownArrowSize = new System.Drawing.Size(5, 3);
            this.btnSaveAsRegExps.Image = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.btnSaveAsRegExps.SmallImage = global::RegScoreCalc.Properties.Resources.SaveAs;
            this.btnSaveAsRegExps.Style = System.Windows.Forms.RibbonButtonStyle.Normal;
            this.btnSaveAsRegExps.Tag = null;
            this.btnSaveAsRegExps.Text = "Save As";
            this.btnSaveAsRegExps.ToolTip = null;
            this.btnSaveAsRegExps.ToolTipImage = null;
            this.btnSaveAsRegExps.ToolTipTitle = null;
            this.btnSaveAsRegExps.Click += new System.EventHandler(this.btnSaveAsRegExps_Click);
            this.btnSaveAsRegExps.MouseEnter += this.RibbonButton_MouseEnter;
            // 
            // sourceRegExp
            // 
            this.sourceRegExp.DataMember = "RegExp";
            this.sourceRegExp.DataSource = this.datasetMain;
            // 
            // sourceEntities
            // 
            this.sourceEntities.DataMember = "Entities";
            this.sourceEntities.DataSource = this.datasetMain;

            // 
            // sourceColRegExp
            // 
            this.sourceColRegExp.DataMember = "ColRegExp";
            this.sourceColRegExp.DataSource = this.datasetMain;
            // 
            // datasetMain
            // 
            this.datasetMain.DataSetName = "Test_dataDataSet";
            this.datasetMain.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sourceCategories
            // 
            this.sourceCategories.DataMember = "Categories";
            this.sourceCategories.DataSource = this.datasetMain;
            // 
            // sourceDocuments
            // 
            this.sourceDocuments.DataMember = "Documents";
            this.sourceDocuments.DataSource = this.datasetMain;
            // 
            // sourceReviewMLDocuments
            // 
            this.sourceReviewMLDocuments.DataMember = "ReviewMLDocuments";
            this.sourceReviewMLDocuments.DataSource = this.datasetMain;
			// 
			// sourceRegExpBilling
			// 
			this.sourceRegExpBilling.DataMember = "RegExp";
            this.sourceRegExpBilling.DataSource = this.datasetBilling;
            // 
            // datasetBilling
            // 
            this.datasetBilling.DataSetName = "Test_dataDataSet";
            this.datasetBilling.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sourceCategoriesBilling
            // 
            this.sourceCategoriesBilling.DataMember = "Categories";
            this.sourceCategoriesBilling.DataSource = this.datasetBilling;
            // 
            // sourceDocumentsBilling
            // 
            this.sourceDocumentsBilling.DataMember = "Documents";
            this.sourceDocumentsBilling.DataSource = this.datasetBilling;
            //
            // sourceColorCodesBilling
            //
            this.sourceColorCodesBilling.DataMember = "ColorCodes";
            this.sourceColorCodesBilling.DataSource = this.datasetMain;
            // 
            // sourceRelationsBilling
            // 
            this.sourceRelationsBilling.DataMember = "Relations";
            this.sourceRelationsBilling.DataSource = this.datasetMain;
            // 
            // sourceColScriptBilling
            // 
            this.sourceColScriptBilling.DataMember = "ColScript";
            this.sourceColScriptBilling.DataSource = this.datasetMain;


            // 
            // adapterRegExpBilling
            // 
            this.adapterRegExpBilling.ClearBeforeFill = true;
            // 
            // adapterDocumentsBilling
            // 
            this.adapterDocumentsBilling.ClearBeforeFill = true;
            // 
            // adapterCategoriesBilling
            // 
            this.adapterCategoriesBilling.ClearBeforeFill = true;
            // 
            // adapterCategoriesBilling
            // 
            this.adapterColorCodesBilling.ClearBeforeFill = true;
            // 
            // adapterCategoriesBilling
            // 
            this.adapterRelationsBilling.ClearBeforeFill = true;
            this.adapterBillingBilling.ClearBeforeFill = true;
            this.adapterICD9GroupsBilling.ClearBeforeFill = true;
            this.adapterICDCodesBilling.ClearBeforeFill = true;
            this.adapterICDFiltersBilling.ClearBeforeFill = true;
            this.adapterDocsToFiltersBilling.ClearBeforeFill = true;
            this.adapterCategoryToFilterExclusionBilling.ClearBeforeFill = true;
            this.adapterRegexpToGroupsBilling.ClearBeforeFill = true;

            this.adapterColScriptBilling.ClearBeforeFill = true;
            //this.adapterVisibleDocumentsBilling.ClearBeforeFill = true;

            // 
            // adapterRegExp
            // 
            this.adapterRegExp.ClearBeforeFill = true;
            // 
            // adapterEntities
            // 
            this.adapterEntities.ClearBeforeFill = true;
            // 
            // adapterColumns
            // 
            this.adapterColumns.ClearBeforeFill = true;
			// 
			// adapterColRegExp
			// 
			this.adapterColRegExp.ClearBeforeFill = true;
            // 
            // adapterRelations
            // 
            this.adapterRelations.ClearBeforeFill = true;
            // 
            // adapterCategories
            // 
            this.adapterCategories.ClearBeforeFill = true;
            // 
            // sourceColorCodes
            // 
            this.sourceColorCodes.DataMember = "ColorCodes";
            this.sourceColorCodes.DataSource = this.datasetMain;
            // 
            // adapterColorCodes
            // 
            this.adapterColorCodes.ClearBeforeFill = true;
            // 
            // sourceRelations
            // 
            this.sourceRelations.DataMember = "Relations";
            this.sourceRelations.DataSource = this.datasetMain;
			// 
            // adapterDynamicColumns
            // 
			this.adapterDynamicColumns.ClearBeforeFill = true;
			// 
			// adapterDynamicColumnCategories
            // 
			this.adapterDynamicColumnCategories.ClearBeforeFill = true;
			// 
            // sourceColScript
            // 
            this.sourceColScript.DataMember = "ColScript";
            this.sourceColScript.DataSource = this.datasetMain;           
            // 
            // adapterColScript
            // 
            this.adapterColScript.ClearBeforeFill = true;
            // 
            // sourceColPython
            // 
            this.sourceColPython.DataMember = "ColPython";
            this.sourceColPython.DataSource = this.datasetMain;
            // 
            // adapterColPython
            // 
            this.adapterColPython.ClearBeforeFill = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 698);
            this.Controls.Add(this.ribbon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Review Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sourceRegExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceEntities)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColRegExp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datasetMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceCategories)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColorCodes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRelations)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColScript)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDocumentsBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRegExpBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceCategoriesBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColorCodesBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRelationsBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceReviewMLDocuments)).EndInit();
	        ((System.ComponentModel.ISupportInitialize)(this.sourceReviewMLDocumentsNew)).EndInit();

			((System.ComponentModel.ISupportInitialize)(this.sourceICD9GroupsBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceICDCodesBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceICDFiltersBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceDocsToFiltersBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceCategoryToFilterExclusionBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceRegexpToGroupsBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sourceColScriptBilling)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.sourceVisibleDocumentsBilling)).EndInit();
            this.ResumeLayout(false);

        }

       
        #endregion

        private System.Windows.Forms.Ribbon ribbon;
        private System.Windows.Forms.RibbonTab tabHome;
        private System.Windows.Forms.RibbonPanel panelDatabase;
        private System.Windows.Forms.RibbonPanel panelDocuments;
        private System.Windows.Forms.RibbonPanel panelRegExps;
        private System.Windows.Forms.RibbonPanel panelGeneral;
        private System.Windows.Forms.RibbonPanel panelViews;
        private System.Windows.Forms.RibbonPanel panelFont;
        private System.Windows.Forms.RibbonPanel panelAdmin;
        private System.Windows.Forms.RibbonButton btnOpenDatabase;
        private System.Windows.Forms.RibbonButton btnSaveDatabase;
        private System.Windows.Forms.RibbonButton btnOpenDocuments;
        private System.Windows.Forms.RibbonButton btnSaveDocuments;
        private System.Windows.Forms.RibbonButton btnSaveAsDocuments;
        private System.Windows.Forms.RibbonButton btnOpenRegExps;
        private System.Windows.Forms.RibbonButton btnSaveRegExps;
        private System.Windows.Forms.RibbonButton btnSaveAsRegExps;
        private System.Windows.Forms.RibbonButton btnSaveAll;
        private System.Windows.Forms.RibbonButton btnPythonSettings;
        private System.Windows.Forms.RibbonComboBox cmbLineSpacing;
        private System.Windows.Forms.RibbonButton btnSelectFont;
        private System.Windows.Forms.RibbonComboBox cmbViews;
        public MainDataSet datasetMain;
        public System.Windows.Forms.BindingSource sourceColRegExp;
        public MainDataSetTableAdapters.ColRegExpTableAdapter adapterColRegExp;
        public System.Windows.Forms.BindingSource sourceRegExp;
        public MainDataSetTableAdapters.RegExpTableAdapter adapterRegExp;

        public System.Windows.Forms.BindingSource sourceEntities;
        public MainDataSetTableAdapters.EntitiesTableAdapter adapterEntities;

        public MainDataSetTableAdapters.ColumnsTableAdapter adapterColumns;
		public CustomOleDbDataAdapter adapterDocuments;
        public System.Windows.Forms.BindingSource sourceDocuments;
        public MainDataSetTableAdapters.CategoriesTableAdapter adapterCategories;
		public MainDataSetTableAdapters.DynamicColumnsTableAdapter adapterDynamicColumns;
		public MainDataSetTableAdapters.DynamicColumnCategoriesTableAdapter adapterDynamicColumnCategories;

        public  MainDataSetTableAdapters.ColScriptTableAdapter adapterColScript;
        public System.Windows.Forms.BindingSource sourceColScript;

        public MainDataSetTableAdapters.ColPythonTableAdapter adapterColPython;
        public System.Windows.Forms.BindingSource sourceColPython;

        public CustomOleDbDataAdapter adapterReviewMLDocumentsNew;
	    public MainDataSetTableAdapters.ReviewMLDocumentsTableAdapter adapterReviewMLDocuments;
		public System.Windows.Forms.BindingSource sourceReviewMLDocuments;
	    public System.Windows.Forms.BindingSource sourceReviewMLDocumentsNew;


		public System.Windows.Forms.BindingSource sourceCategories;
        private System.Windows.Forms.RibbonButton btnOpenView;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemOpenDatabase;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemSaveDatabase;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemOpenDocuments;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemSaveDocuments;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemSaveAsDocuments;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemOpenRegExps;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemSaveRegExps;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemSaveAsRegExps;
        public System.Windows.Forms.BindingSource sourceColorCodes;
        public MainDataSetTableAdapters.ColorCodesTableAdapter adapterColorCodes;
        private System.Windows.Forms.RibbonOrbOptionButton orbitemExit;
        private System.Windows.Forms.RibbonOrbOptionButton orbitemClearHistory;
        public MainDataSetTableAdapters.RelationsTableAdapter adapterRelations;
        public System.Windows.Forms.BindingSource sourceRelations;
        private System.Windows.Forms.RibbonSeparator ribbonOrbMenuSeparator;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemDocuments;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemDatabase;
		private System.Windows.Forms.RibbonOrbMenuItem orbitemOpenView;
		private System.Windows.Forms.RibbonOrbMenuItem orbitemRegExps;
        private System.Windows.Forms.RibbonOrbMenuItem orbitemSaveAll;

        public BillingDataSet datasetBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.DocumentsTableAdapter adapterDocumentsBilling;
        public System.Windows.Forms.BindingSource sourceDocumentsBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.BillingTableAdapter adapterBillingBilling;
        public System.Windows.Forms.BindingSource sourceBillingBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.RegExpTableAdapter adapterRegExpBilling;
        public System.Windows.Forms.BindingSource sourceRegExpBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.CategoriesTableAdapter adapterCategoriesBilling;
        public System.Windows.Forms.BindingSource sourceCategoriesBilling;

        public RegScoreCalc.Data.BillingDataSetTableAdapters.ICD9GroupsTableAdapter adapterICD9GroupsBilling;
        public System.Windows.Forms.BindingSource sourceICD9GroupsBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.ICDCodesTableAdapter adapterICDCodesBilling;
        public System.Windows.Forms.BindingSource sourceICDCodesBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.ICDFiltersTableAdapter adapterICDFiltersBilling;
        public System.Windows.Forms.BindingSource sourceICDFiltersBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.DocsToFiltersTableAdapter adapterDocsToFiltersBilling;
        public System.Windows.Forms.BindingSource sourceDocsToFiltersBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.CategoryToFilterExclusionTableAdapter adapterCategoryToFilterExclusionBilling;
        public System.Windows.Forms.BindingSource sourceCategoryToFilterExclusionBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.RegexpToGroupsTableAdapter adapterRegexpToGroupsBilling;
        public System.Windows.Forms.BindingSource sourceRegexpToGroupsBilling;

        //public RegScoreCalc.Data.BillingDataSetTableAdapters.DocumentsTableAdapter adapterVisibleDocumentsBilling;
        //public System.Windows.Forms.BindingSource sourceVisibleDocumentsBilling;

        public RegScoreCalc.Data.BillingDataSetTableAdapters.ColorCodesTableAdapter adapterColorCodesBilling;
        public System.Windows.Forms.BindingSource sourceColorCodesBilling;
        public RegScoreCalc.Data.BillingDataSetTableAdapters.RelationsTableAdapter adapterRelationsBilling;
        public System.Windows.Forms.BindingSource sourceRelationsBilling;


        public RegScoreCalc.Data.BillingDataSetTableAdapters.ColScriptTableAdapter adapterColScriptBilling;
        public System.Windows.Forms.BindingSource sourceColScriptBilling;


        
    }
}
