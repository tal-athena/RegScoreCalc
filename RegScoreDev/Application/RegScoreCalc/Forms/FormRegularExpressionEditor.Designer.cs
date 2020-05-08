using System.Windows.Forms;

using RegScoreCalc.Code.Controls;

namespace RegScoreCalc.Forms
{
    partial class FormRegularExpressionEditor : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegularExpressionEditor));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelNavButtons = new System.Windows.Forms.Panel();
            this.btnPrevDocument = new RegScoreCalc.RibbonStyleButton();
            this.btnNextDocument = new RegScoreCalc.RibbonStyleButton();
            this.btnNextUniqueMatch = new RegScoreCalc.RibbonStyleButton();
            this.btnNextMatch = new RegScoreCalc.RibbonStyleButton();
            this.btnPrevUniqueMatch = new RegScoreCalc.RibbonStyleButton();
            this.btnPrevMatch = new RegScoreCalc.RibbonStyleButton();
            this.miniToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.lblDescription = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEditDescription = new RegScoreCalc.RibbonStyleButton();
            this.toolTipGeneral = new System.Windows.Forms.ToolTip(this.components);
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btnApply = new RegScoreCalc.RibbonStyleButton();
            this.tabControlEditRegEx = new RegScoreCalc.CustomTabControl();
            this.tabPageEditor = new RegScoreCalc.CustomTabPage();
            this.splitterMain = new System.Windows.Forms.SplitContainer();
            this.splitterRegExpAndExamples = new System.Windows.Forms.SplitContainer();
            this.btnShowToolbox = new RegScoreCalc.RibbonStyleButton();
            this.btnShowExamples = new RegScoreCalc.RibbonStyleButton();
            this.txtRegExp = new RegScoreCalc.Code.Controls.CustomTextBox();
            this.dataGridViewExamples = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnRegEx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lvQuickActions = new RegScoreCalc.Controls.ToolboxCtrl();
            this.tabPageLookbehind = new RegScoreCalc.CustomTabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonLookbehind = new System.Windows.Forms.RadioButton();
            this.gridCriteriaLookbehind = new System.Windows.Forms.DataGridView();
            this.colLookBehindEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridCriteriaNegativeLookbehind = new System.Windows.Forms.DataGridView();
            this.colNegLookBehindEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radioButtonNegativeLookbehind = new System.Windows.Forms.RadioButton();
            this.btnDeleteLookBehind = new RegScoreCalc.RibbonStyleButton();
            this.btnDeleteNegLookBehind = new RegScoreCalc.RibbonStyleButton();
            this.btnAddLookBehind = new RegScoreCalc.RibbonStyleButton();
            this.btnAddNegLookBehind = new RegScoreCalc.RibbonStyleButton();
            this.tabPageLookahead = new RegScoreCalc.CustomTabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButtonLookahead = new System.Windows.Forms.RadioButton();
            this.gridCriteriaLookahead = new System.Windows.Forms.DataGridView();
            this.colLookAheadEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radioButtonNegativeLookahead = new System.Windows.Forms.RadioButton();
            this.btnDeleteLookAhead = new RegScoreCalc.RibbonStyleButton();
            this.gridCriteriaNegativeLookahead = new System.Windows.Forms.DataGridView();
            this.colNegLookAheadEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteNegLookAhead = new RegScoreCalc.RibbonStyleButton();
            this.btnAddLookAhead = new RegScoreCalc.RibbonStyleButton();
            this.btnAddNegLookAhead = new RegScoreCalc.RibbonStyleButton();
            this.tabPageExceptions = new RegScoreCalc.CustomTabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.gridCriteriaExceptions = new System.Windows.Forms.DataGridView();
            this.colExceptionsEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddException = new RegScoreCalc.RibbonStyleButton();
            this.btnDeleteExceptions = new RegScoreCalc.RibbonStyleButton();
            this.tabPageStatistics = new RegScoreCalc.CustomTabPage();
            this.gridStatistics = new System.Windows.Forms.DataGridView();
            this.colExceptions = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colWord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchesCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchesCountPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchinglDocsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchinglRecordsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchingDocsPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlllDocsPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.chkbExcludeLookArounds = new System.Windows.Forms.ToolStripButton();
            this.btnCalculate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStatisticsPrevMatch = new System.Windows.Forms.ToolStripButton();
            this.btnStatisticsNextMatch = new System.Windows.Forms.ToolStripButton();
            this.btnSimilar = new System.Windows.Forms.ToolStripButton();
            this.tabPageAssignCategory = new RegScoreCalc.CustomTabPage();
            this.btnClear = new RegScoreCalc.RibbonStyleButton();
            this.btnSelectCategory = new RegScoreCalc.RibbonStyleButton();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelNavButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControlEditRegEx.SuspendLayout();
            this.tabPageEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterMain)).BeginInit();
            this.splitterMain.Panel1.SuspendLayout();
            this.splitterMain.Panel2.SuspendLayout();
            this.splitterMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterRegExpAndExamples)).BeginInit();
            this.splitterRegExpAndExamples.Panel1.SuspendLayout();
            this.splitterRegExpAndExamples.Panel2.SuspendLayout();
            this.splitterRegExpAndExamples.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExamples)).BeginInit();
            this.tabPageLookbehind.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaLookbehind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaNegativeLookbehind)).BeginInit();
            this.tabPageLookahead.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaLookahead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaNegativeLookahead)).BeginInit();
            this.tabPageExceptions.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaExceptions)).BeginInit();
            this.tabPageStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStatistics)).BeginInit();
            this.toolStripTop.SuspendLayout();
            this.tabPageAssignCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(924, 0);
            this.panel2.TabIndex = 0;
            // 
            // panelNavButtons
            // 
            this.panelNavButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelNavButtons.Controls.Add(this.btnPrevDocument);
            this.panelNavButtons.Controls.Add(this.btnNextDocument);
            this.panelNavButtons.Controls.Add(this.btnNextUniqueMatch);
            this.panelNavButtons.Controls.Add(this.btnNextMatch);
            this.panelNavButtons.Controls.Add(this.btnPrevUniqueMatch);
            this.panelNavButtons.Controls.Add(this.btnPrevMatch);
            this.panelNavButtons.Location = new System.Drawing.Point(6, 495);
            this.panelNavButtons.Name = "panelNavButtons";
            this.panelNavButtons.Size = new System.Drawing.Size(276, 30);
            this.panelNavButtons.TabIndex = 3;
            // 
            // btnPrevDocument
            // 
            this.btnPrevDocument.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrevDocument.DrawNormalBorder = false;
            this.btnPrevDocument.HoverImage = global::RegScoreCalc.Properties.Resources.double_left_m_over;
            this.btnPrevDocument.IsHighlighted = false;
            this.btnPrevDocument.Location = new System.Drawing.Point(0, 0);
            this.btnPrevDocument.Name = "btnPrevDocument";
            this.btnPrevDocument.NormalImage = global::RegScoreCalc.Properties.Resources.double_left_normal;
            this.btnPrevDocument.PressedImage = global::RegScoreCalc.Properties.Resources.double_left_pressed;
            this.btnPrevDocument.Size = new System.Drawing.Size(41, 30);
            this.btnPrevDocument.TabIndex = 0;
            this.toolTip.SetToolTip(this.btnPrevDocument, "Previous Document");
            this.btnPrevDocument.UseVisualStyleBackColor = true;
            this.btnPrevDocument.Click += new System.EventHandler(this.btnPrevDocument_Click);
            // 
            // btnNextDocument
            // 
            this.btnNextDocument.BackColor = System.Drawing.SystemColors.Control;
            this.btnNextDocument.DrawNormalBorder = false;
            this.btnNextDocument.HoverImage = global::RegScoreCalc.Properties.Resources.double_right_m_over;
            this.btnNextDocument.IsHighlighted = false;
            this.btnNextDocument.Location = new System.Drawing.Point(141, 0);
            this.btnNextDocument.Name = "btnNextDocument";
            this.btnNextDocument.NormalImage = global::RegScoreCalc.Properties.Resources.double_right_normal;
            this.btnNextDocument.PressedImage = global::RegScoreCalc.Properties.Resources.double_right_pressed;
            this.btnNextDocument.Size = new System.Drawing.Size(41, 30);
            this.btnNextDocument.TabIndex = 3;
            this.toolTip.SetToolTip(this.btnNextDocument, "Next Document");
            this.btnNextDocument.UseVisualStyleBackColor = true;
            this.btnNextDocument.Click += new System.EventHandler(this.btnNextDocument_Click);
            // 
            // btnNextUniqueMatch
            // 
            this.btnNextUniqueMatch.BackColor = System.Drawing.SystemColors.Control;
            this.btnNextUniqueMatch.DrawNormalBorder = false;
            this.btnNextUniqueMatch.HoverImage = global::RegScoreCalc.Properties.Resources.right_m_over_u_arrow;
            this.btnNextUniqueMatch.IsHighlighted = false;
            this.btnNextUniqueMatch.Location = new System.Drawing.Point(235, 0);
            this.btnNextUniqueMatch.Name = "btnNextUniqueMatch";
            this.btnNextUniqueMatch.NormalImage = global::RegScoreCalc.Properties.Resources.right_normal_u_arrow;
            this.btnNextUniqueMatch.PressedImage = global::RegScoreCalc.Properties.Resources.right_pressed_u_arrow;
            this.btnNextUniqueMatch.Size = new System.Drawing.Size(41, 30);
            this.btnNextUniqueMatch.TabIndex = 5;
            this.toolTip.SetToolTip(this.btnNextUniqueMatch, "Next Unique Match");
            this.btnNextUniqueMatch.UseVisualStyleBackColor = true;
            this.btnNextUniqueMatch.Click += new System.EventHandler(this.btnNextUniqueMatch_Click);
            // 
            // btnNextMatch
            // 
            this.btnNextMatch.BackColor = System.Drawing.SystemColors.Control;
            this.btnNextMatch.DrawNormalBorder = false;
            this.btnNextMatch.HoverImage = global::RegScoreCalc.Properties.Resources.right_m_over;
            this.btnNextMatch.IsHighlighted = false;
            this.btnNextMatch.Location = new System.Drawing.Point(94, 0);
            this.btnNextMatch.Name = "btnNextMatch";
            this.btnNextMatch.NormalImage = global::RegScoreCalc.Properties.Resources.right_normal;
            this.btnNextMatch.PressedImage = global::RegScoreCalc.Properties.Resources.right_pressed;
            this.btnNextMatch.Size = new System.Drawing.Size(41, 30);
            this.btnNextMatch.TabIndex = 2;
            this.toolTip.SetToolTip(this.btnNextMatch, "Next Match");
            this.btnNextMatch.UseVisualStyleBackColor = true;
            this.btnNextMatch.Click += new System.EventHandler(this.btnNextMatch_Click);
            // 
            // btnPrevUniqueMatch
            // 
            this.btnPrevUniqueMatch.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrevUniqueMatch.DrawNormalBorder = false;
            this.btnPrevUniqueMatch.HoverImage = global::RegScoreCalc.Properties.Resources.left_m_over_u_arrow;
            this.btnPrevUniqueMatch.IsHighlighted = false;
            this.btnPrevUniqueMatch.Location = new System.Drawing.Point(188, 0);
            this.btnPrevUniqueMatch.Name = "btnPrevUniqueMatch";
            this.btnPrevUniqueMatch.NormalImage = global::RegScoreCalc.Properties.Resources.left_normal_u_arrow;
            this.btnPrevUniqueMatch.PressedImage = global::RegScoreCalc.Properties.Resources.left_pressed_u_arrow;
            this.btnPrevUniqueMatch.Size = new System.Drawing.Size(41, 30);
            this.btnPrevUniqueMatch.TabIndex = 4;
            this.toolTip.SetToolTip(this.btnPrevUniqueMatch, "Previous Unique Match");
            this.btnPrevUniqueMatch.UseVisualStyleBackColor = true;
            this.btnPrevUniqueMatch.Click += new System.EventHandler(this.btnPrevUniqueMatch_Click);
            // 
            // btnPrevMatch
            // 
            this.btnPrevMatch.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrevMatch.DrawNormalBorder = false;
            this.btnPrevMatch.HoverImage = global::RegScoreCalc.Properties.Resources.left_m_over;
            this.btnPrevMatch.IsHighlighted = false;
            this.btnPrevMatch.Location = new System.Drawing.Point(47, 0);
            this.btnPrevMatch.Name = "btnPrevMatch";
            this.btnPrevMatch.NormalImage = global::RegScoreCalc.Properties.Resources.left_normal;
            this.btnPrevMatch.PressedImage = global::RegScoreCalc.Properties.Resources.left_pressed;
            this.btnPrevMatch.Size = new System.Drawing.Size(41, 30);
            this.btnPrevMatch.TabIndex = 1;
            this.toolTip.SetToolTip(this.btnPrevMatch, "Previous Match");
            this.btnPrevMatch.UseVisualStyleBackColor = true;
            this.btnPrevMatch.Click += new System.EventHandler(this.btnPrevMatch_Click);
            // 
            // miniToolStrip
            // 
            this.miniToolStrip.AutoSize = false;
            this.miniToolStrip.CanOverflow = false;
            this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.miniToolStrip.Location = new System.Drawing.Point(101, 1);
            this.miniToolStrip.Name = "miniToolStrip";
            this.miniToolStrip.Padding = new System.Windows.Forms.Padding(10, 0, 0, 3);
            this.miniToolStrip.Size = new System.Drawing.Size(103, 25);
            this.miniToolStrip.Stretch = true;
            this.miniToolStrip.TabIndex = 5;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescription.Location = new System.Drawing.Point(38, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(886, 25);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDescription);
            this.panel1.Controls.Add(this.btnEditDescription);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(924, 25);
            this.panel1.TabIndex = 6;
            // 
            // btnEditDescription
            // 
            this.btnEditDescription.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnEditDescription.DrawNormalBorder = false;
            this.btnEditDescription.HoverImage = null;
            this.btnEditDescription.Image = global::RegScoreCalc.Properties.Resources.edit_description;
            this.btnEditDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditDescription.IsHighlighted = false;
            this.btnEditDescription.Location = new System.Drawing.Point(0, 0);
            this.btnEditDescription.Name = "btnEditDescription";
            this.btnEditDescription.NormalImage = null;
            this.btnEditDescription.PressedImage = null;
            this.btnEditDescription.Size = new System.Drawing.Size(38, 25);
            this.btnEditDescription.TabIndex = 6;
            this.toolTipGeneral.SetToolTip(this.btnEditDescription, "Edit Description");
            this.btnEditDescription.UseVisualStyleBackColor = true;
            this.btnEditDescription.Click += new System.EventHandler(this.btnEditDescription_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "add_small.png");
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DrawNormalBorder = false;
            this.btnApply.HoverImage = null;
            this.btnApply.Image = global::RegScoreCalc.Properties.Resources.apply;
            this.btnApply.IsHighlighted = false;
            this.btnApply.Location = new System.Drawing.Point(757, 495);
            this.btnApply.Name = "btnApply";
            this.btnApply.NormalImage = null;
            this.btnApply.PressedImage = null;
            this.btnApply.Size = new System.Drawing.Size(158, 30);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply Changes";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // tabControlEditRegEx
            // 
            this.tabControlEditRegEx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEditRegEx.Controls.Add(this.tabPageEditor);
            this.tabControlEditRegEx.Controls.Add(this.tabPageLookbehind);
            this.tabControlEditRegEx.Controls.Add(this.tabPageLookahead);
            this.tabControlEditRegEx.Controls.Add(this.tabPageExceptions);
            this.tabControlEditRegEx.Controls.Add(this.tabPageStatistics);
            this.tabControlEditRegEx.Controls.Add(this.tabPageAssignCategory);
            this.tabControlEditRegEx.ImageList = this.imageList;
            this.tabControlEditRegEx.Location = new System.Drawing.Point(3, 25);
            this.tabControlEditRegEx.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlEditRegEx.Name = "tabControlEditRegEx";
            this.tabControlEditRegEx.SelectedIndex = 0;
            this.tabControlEditRegEx.ShowIndicators = false;
            this.tabControlEditRegEx.Size = new System.Drawing.Size(916, 464);
            this.tabControlEditRegEx.TabIndex = 1;
            this.tabControlEditRegEx.SelectedIndexChanged += new System.EventHandler(this.tabControlEditRegEx_SelectedIndexChanged);
            // 
            // tabPageEditor
            // 
            this.tabPageEditor.Controls.Add(this.splitterMain);
            this.tabPageEditor.Location = new System.Drawing.Point(4, 25);
            this.tabPageEditor.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageEditor.Name = "tabPageEditor";
            this.tabPageEditor.Size = new System.Drawing.Size(908, 435);
            this.tabPageEditor.TabIndex = 0;
            this.tabPageEditor.Text = "Editor";
            this.tabPageEditor.UseVisualStyleBackColor = true;
            // 
            // splitterMain
            // 
            this.splitterMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitterMain.IsSplitterFixed = true;
            this.splitterMain.Location = new System.Drawing.Point(0, 0);
            this.splitterMain.Margin = new System.Windows.Forms.Padding(0);
            this.splitterMain.Name = "splitterMain";
            // 
            // splitterMain.Panel1
            // 
            this.splitterMain.Panel1.Controls.Add(this.splitterRegExpAndExamples);
            // 
            // splitterMain.Panel2
            // 
            this.splitterMain.Panel2.Controls.Add(this.lvQuickActions);
            this.splitterMain.Size = new System.Drawing.Size(908, 435);
            this.splitterMain.SplitterDistance = 410;
            this.splitterMain.SplitterWidth = 8;
            this.splitterMain.TabIndex = 15;
            this.splitterMain.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitterMain_SplitterMoved);
            // 
            // splitterRegExpAndExamples
            // 
            this.splitterRegExpAndExamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterRegExpAndExamples.Location = new System.Drawing.Point(0, 0);
            this.splitterRegExpAndExamples.Margin = new System.Windows.Forms.Padding(0);
            this.splitterRegExpAndExamples.Name = "splitterRegExpAndExamples";
            this.splitterRegExpAndExamples.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterRegExpAndExamples.Panel1
            // 
            this.splitterRegExpAndExamples.Panel1.Controls.Add(this.btnShowToolbox);
            this.splitterRegExpAndExamples.Panel1.Controls.Add(this.btnShowExamples);
            this.splitterRegExpAndExamples.Panel1.Controls.Add(this.txtRegExp);
            this.splitterRegExpAndExamples.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitterRegExpAndExamples.Panel2
            // 
            this.splitterRegExpAndExamples.Panel2.Controls.Add(this.dataGridViewExamples);
            this.splitterRegExpAndExamples.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitterRegExpAndExamples.Size = new System.Drawing.Size(410, 435);
            this.splitterRegExpAndExamples.SplitterDistance = 224;
            this.splitterRegExpAndExamples.SplitterWidth = 8;
            this.splitterRegExpAndExamples.TabIndex = 14;
            // 
            // btnShowToolbox
            // 
            this.btnShowToolbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowToolbox.DrawNormalBorder = false;
            this.btnShowToolbox.HoverImage = null;
            this.btnShowToolbox.Image = global::RegScoreCalc.Properties.Resources.ShowToolbox;
            this.btnShowToolbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowToolbox.IsHighlighted = false;
            this.btnShowToolbox.Location = new System.Drawing.Point(253, 191);
            this.btnShowToolbox.Name = "btnShowToolbox";
            this.btnShowToolbox.NormalImage = null;
            this.btnShowToolbox.PressedImage = null;
            this.btnShowToolbox.Size = new System.Drawing.Size(158, 32);
            this.btnShowToolbox.TabIndex = 2;
            this.btnShowToolbox.Text = "       Show toolbox";
            this.btnShowToolbox.UseVisualStyleBackColor = true;
            this.btnShowToolbox.Click += new System.EventHandler(this.btnShowToolbox_Click);
            // 
            // btnShowExamples
            // 
            this.btnShowExamples.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowExamples.DrawNormalBorder = false;
            this.btnShowExamples.HoverImage = null;
            this.btnShowExamples.Image = global::RegScoreCalc.Properties.Resources.ShowExamples;
            this.btnShowExamples.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnShowExamples.IsHighlighted = false;
            this.btnShowExamples.Location = new System.Drawing.Point(-1, 191);
            this.btnShowExamples.Name = "btnShowExamples";
            this.btnShowExamples.NormalImage = null;
            this.btnShowExamples.PressedImage = null;
            this.btnShowExamples.Size = new System.Drawing.Size(158, 32);
            this.btnShowExamples.TabIndex = 1;
            this.btnShowExamples.Text = "       Show examples";
            this.btnShowExamples.UseVisualStyleBackColor = true;
            this.btnShowExamples.Click += new System.EventHandler(this.btnShowExamples_Click);
            // 
            // txtRegExp
            // 
            this.txtRegExp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRegExp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRegExp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtRegExp.Location = new System.Drawing.Point(0, 2);
            this.txtRegExp.Margin = new System.Windows.Forms.Padding(0);
            this.txtRegExp.Multiline = true;
            this.txtRegExp.Name = "txtRegExp";
            this.txtRegExp.Size = new System.Drawing.Size(410, 182);
            this.txtRegExp.TabIndex = 0;
            this.txtRegExp.TextChanged += new System.EventHandler(this.txtRegExp_TextChanged);
            this.txtRegExp.Enter += new System.EventHandler(this.txtRegExp_Enter);
            this.txtRegExp.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtRegExp_KeyUp);
            this.txtRegExp.Leave += new System.EventHandler(this.txtRegExp_Leave);
            this.txtRegExp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtRegExp_MouseUp);
            // 
            // dataGridViewExamples
            // 
            this.dataGridViewExamples.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewExamples.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnDescription,
            this.dataGridViewTextBoxColumnRegEx});
            this.dataGridViewExamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewExamples.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExamples.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridViewExamples.Name = "dataGridViewExamples";
            this.dataGridViewExamples.Size = new System.Drawing.Size(410, 203);
            this.dataGridViewExamples.TabIndex = 0;
            this.dataGridViewExamples.SelectionChanged += new System.EventHandler(this.dataGridViewExamples_SelectionChanged);
            // 
            // dataGridViewTextBoxColumnDescription
            // 
            this.dataGridViewTextBoxColumnDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnDescription.HeaderText = "Description";
            this.dataGridViewTextBoxColumnDescription.Name = "dataGridViewTextBoxColumnDescription";
            // 
            // dataGridViewTextBoxColumnRegEx
            // 
            this.dataGridViewTextBoxColumnRegEx.HeaderText = "RegEx";
            this.dataGridViewTextBoxColumnRegEx.Name = "dataGridViewTextBoxColumnRegEx";
            // 
            // lvQuickActions
            // 
            this.lvQuickActions.Activation = System.Windows.Forms.ItemActivation.TwoClick;
            this.lvQuickActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvQuickActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvQuickActions.FullRowSelect = true;
            this.lvQuickActions.GridLines = true;
            this.lvQuickActions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvQuickActions.HideSelection = false;
            this.lvQuickActions.Location = new System.Drawing.Point(0, 0);
            this.lvQuickActions.Margin = new System.Windows.Forms.Padding(0);
            this.lvQuickActions.MultiSelect = false;
            this.lvQuickActions.Name = "lvQuickActions";
            this.lvQuickActions.OwnerDraw = true;
            this.lvQuickActions.Size = new System.Drawing.Size(490, 435);
            this.lvQuickActions.TabIndex = 0;
            this.lvQuickActions.UseCompatibleStateImageBehavior = false;
            this.lvQuickActions.View = System.Windows.Forms.View.Details;
            // 
            // tabPageLookbehind
            // 
            this.tabPageLookbehind.Controls.Add(this.tableLayoutPanel1);
            this.tabPageLookbehind.ImageIndex = 0;
            this.tabPageLookbehind.Location = new System.Drawing.Point(4, 25);
            this.tabPageLookbehind.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageLookbehind.Name = "tabPageLookbehind";
            this.tabPageLookbehind.Size = new System.Drawing.Size(908, 435);
            this.tabPageLookbehind.TabIndex = 1;
            this.tabPageLookbehind.Text = "Lookbehind";
            this.tabPageLookbehind.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.radioButtonLookbehind, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridCriteriaLookbehind, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.gridCriteriaNegativeLookbehind, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.radioButtonNegativeLookbehind, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDeleteLookBehind, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDeleteNegLookBehind, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddLookBehind, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddNegLookBehind, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(908, 435);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // radioButtonLookbehind
            // 
            this.radioButtonLookbehind.Checked = true;
            this.radioButtonLookbehind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonLookbehind.Location = new System.Drawing.Point(3, 3);
            this.radioButtonLookbehind.Name = "radioButtonLookbehind";
            this.radioButtonLookbehind.Size = new System.Drawing.Size(97, 34);
            this.radioButtonLookbehind.TabIndex = 0;
            this.radioButtonLookbehind.TabStop = true;
            this.radioButtonLookbehind.Text = "Look&behind";
            this.radioButtonLookbehind.UseVisualStyleBackColor = true;
            this.radioButtonLookbehind.CheckedChanged += new System.EventHandler(this.radioButtonLookbehind_CheckedChanged);
            // 
            // gridCriteriaLookbehind
            // 
            this.gridCriteriaLookbehind.AllowUserToAddRows = false;
            this.gridCriteriaLookbehind.AllowUserToDeleteRows = false;
            this.gridCriteriaLookbehind.AllowUserToResizeRows = false;
            this.gridCriteriaLookbehind.BackgroundColor = System.Drawing.Color.White;
            this.gridCriteriaLookbehind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCriteriaLookbehind.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLookBehindEnabled,
            this.colValue});
            this.tableLayoutPanel1.SetColumnSpan(this.gridCriteriaLookbehind, 3);
            this.gridCriteriaLookbehind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCriteriaLookbehind.Location = new System.Drawing.Point(0, 40);
            this.gridCriteriaLookbehind.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.gridCriteriaLookbehind.Name = "gridCriteriaLookbehind";
            this.gridCriteriaLookbehind.RowHeadersVisible = false;
            this.gridCriteriaLookbehind.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCriteriaLookbehind.Size = new System.Drawing.Size(450, 395);
            this.gridCriteriaLookbehind.TabIndex = 2;
            this.gridCriteriaLookbehind.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.gridCriteriaLookbehind.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
            this.gridCriteriaLookbehind.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            this.gridCriteriaLookbehind.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_DefaultValuesNeeded);
            this.gridCriteriaLookbehind.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_Validated);
            // 
            // colLookBehindEnabled
            // 
            this.colLookBehindEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colLookBehindEnabled.HeaderText = "";
            this.colLookBehindEnabled.MinimumWidth = 35;
            this.colLookBehindEnabled.Name = "colLookBehindEnabled";
            this.colLookBehindEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colLookBehindEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colLookBehindEnabled.Width = 35;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.HeaderText = "Values";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // gridCriteriaNegativeLookbehind
            // 
            this.gridCriteriaNegativeLookbehind.AllowUserToAddRows = false;
            this.gridCriteriaNegativeLookbehind.AllowUserToDeleteRows = false;
            this.gridCriteriaNegativeLookbehind.AllowUserToResizeRows = false;
            this.gridCriteriaNegativeLookbehind.BackgroundColor = System.Drawing.Color.White;
            this.gridCriteriaNegativeLookbehind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCriteriaNegativeLookbehind.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNegLookBehindEnabled,
            this.dataGridViewTextBoxColumn1});
            this.tableLayoutPanel1.SetColumnSpan(this.gridCriteriaNegativeLookbehind, 3);
            this.gridCriteriaNegativeLookbehind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCriteriaNegativeLookbehind.Location = new System.Drawing.Point(456, 40);
            this.gridCriteriaNegativeLookbehind.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.gridCriteriaNegativeLookbehind.Name = "gridCriteriaNegativeLookbehind";
            this.gridCriteriaNegativeLookbehind.RowHeadersVisible = false;
            this.gridCriteriaNegativeLookbehind.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCriteriaNegativeLookbehind.Size = new System.Drawing.Size(452, 395);
            this.gridCriteriaNegativeLookbehind.TabIndex = 3;
            this.gridCriteriaNegativeLookbehind.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.gridCriteriaNegativeLookbehind.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
            this.gridCriteriaNegativeLookbehind.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            this.gridCriteriaNegativeLookbehind.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_DefaultValuesNeeded);
            this.gridCriteriaNegativeLookbehind.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_Validated);
            // 
            // colNegLookBehindEnabled
            // 
            this.colNegLookBehindEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colNegLookBehindEnabled.HeaderText = "";
            this.colNegLookBehindEnabled.MinimumWidth = 35;
            this.colNegLookBehindEnabled.Name = "colNegLookBehindEnabled";
            this.colNegLookBehindEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colNegLookBehindEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colNegLookBehindEnabled.Width = 35;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Values";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // radioButtonNegativeLookbehind
            // 
            this.radioButtonNegativeLookbehind.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonNegativeLookbehind.Location = new System.Drawing.Point(456, 3);
            this.radioButtonNegativeLookbehind.Name = "radioButtonNegativeLookbehind";
            this.radioButtonNegativeLookbehind.Size = new System.Drawing.Size(104, 34);
            this.radioButtonNegativeLookbehind.TabIndex = 1;
            this.radioButtonNegativeLookbehind.Text = "&Neg Lookbehind";
            this.radioButtonNegativeLookbehind.UseVisualStyleBackColor = true;
            this.radioButtonNegativeLookbehind.CheckedChanged += new System.EventHandler(this.radioButtonNegativeLookbehind_CheckedChanged);
            // 
            // btnDeleteLookBehind
            // 
            this.btnDeleteLookBehind.DrawNormalBorder = false;
            this.btnDeleteLookBehind.HoverImage = null;
            this.btnDeleteLookBehind.Image = global::RegScoreCalc.Properties.Resources.DeselectAll;
            this.btnDeleteLookBehind.IsHighlighted = false;
            this.btnDeleteLookBehind.Location = new System.Drawing.Point(250, 3);
            this.btnDeleteLookBehind.Name = "btnDeleteLookBehind";
            this.btnDeleteLookBehind.NormalImage = null;
            this.btnDeleteLookBehind.PressedImage = null;
            this.btnDeleteLookBehind.Size = new System.Drawing.Size(146, 34);
            this.btnDeleteLookBehind.TabIndex = 4;
            this.btnDeleteLookBehind.Text = "Delete Selected";
            this.btnDeleteLookBehind.UseVisualStyleBackColor = true;
            this.btnDeleteLookBehind.Click += new System.EventHandler(this.btnDeleteCriteria_Click);
            // 
            // btnDeleteNegLookBehind
            // 
            this.btnDeleteNegLookBehind.DrawNormalBorder = false;
            this.btnDeleteNegLookBehind.HoverImage = null;
            this.btnDeleteNegLookBehind.Image = global::RegScoreCalc.Properties.Resources.DeselectAll;
            this.btnDeleteNegLookBehind.IsHighlighted = false;
            this.btnDeleteNegLookBehind.Location = new System.Drawing.Point(703, 3);
            this.btnDeleteNegLookBehind.Name = "btnDeleteNegLookBehind";
            this.btnDeleteNegLookBehind.NormalImage = null;
            this.btnDeleteNegLookBehind.PressedImage = null;
            this.btnDeleteNegLookBehind.Size = new System.Drawing.Size(148, 34);
            this.btnDeleteNegLookBehind.TabIndex = 4;
            this.btnDeleteNegLookBehind.Text = "Delete Selected";
            this.btnDeleteNegLookBehind.UseVisualStyleBackColor = true;
            this.btnDeleteNegLookBehind.Click += new System.EventHandler(this.btnDeleteCriteria_Click);
            // 
            // btnAddLookBehind
            // 
            this.btnAddLookBehind.DrawNormalBorder = false;
            this.btnAddLookBehind.HoverImage = null;
            this.btnAddLookBehind.Image = global::RegScoreCalc.Properties.Resources.add_small;
            this.btnAddLookBehind.IsHighlighted = false;
            this.btnAddLookBehind.Location = new System.Drawing.Point(113, 3);
            this.btnAddLookBehind.Name = "btnAddLookBehind";
            this.btnAddLookBehind.NormalImage = null;
            this.btnAddLookBehind.PressedImage = null;
            this.btnAddLookBehind.Size = new System.Drawing.Size(95, 34);
            this.btnAddLookBehind.TabIndex = 4;
            this.btnAddLookBehind.Text = "Add New";
            this.btnAddLookBehind.UseVisualStyleBackColor = true;
            this.btnAddLookBehind.Click += new System.EventHandler(this.btnAddCriteria_Click);
            // 
            // btnAddNegLookBehind
            // 
            this.btnAddNegLookBehind.DrawNormalBorder = false;
            this.btnAddNegLookBehind.HoverImage = null;
            this.btnAddNegLookBehind.Image = global::RegScoreCalc.Properties.Resources.add_small;
            this.btnAddNegLookBehind.IsHighlighted = false;
            this.btnAddNegLookBehind.Location = new System.Drawing.Point(566, 3);
            this.btnAddNegLookBehind.Name = "btnAddNegLookBehind";
            this.btnAddNegLookBehind.NormalImage = null;
            this.btnAddNegLookBehind.PressedImage = null;
            this.btnAddNegLookBehind.Size = new System.Drawing.Size(95, 34);
            this.btnAddNegLookBehind.TabIndex = 4;
            this.btnAddNegLookBehind.Text = "Add New";
            this.btnAddNegLookBehind.UseVisualStyleBackColor = true;
            this.btnAddNegLookBehind.Click += new System.EventHandler(this.btnAddCriteria_Click);
            // 
            // tabPageLookahead
            // 
            this.tabPageLookahead.Controls.Add(this.tableLayoutPanel2);
            this.tabPageLookahead.ImageIndex = 0;
            this.tabPageLookahead.Location = new System.Drawing.Point(4, 25);
            this.tabPageLookahead.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageLookahead.Name = "tabPageLookahead";
            this.tabPageLookahead.Size = new System.Drawing.Size(908, 435);
            this.tabPageLookahead.TabIndex = 2;
            this.tabPageLookahead.Text = "Lookahead";
            this.tabPageLookahead.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Controls.Add(this.radioButtonLookahead, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gridCriteriaLookahead, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.radioButtonNegativeLookahead, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDeleteLookAhead, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.gridCriteriaNegativeLookahead, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnDeleteNegLookAhead, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAddLookAhead, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAddNegLookAhead, 4, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(908, 435);
            this.tableLayoutPanel2.TabIndex = 8;
            // 
            // radioButtonLookahead
            // 
            this.radioButtonLookahead.Checked = true;
            this.radioButtonLookahead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonLookahead.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.radioButtonLookahead.Location = new System.Drawing.Point(3, 3);
            this.radioButtonLookahead.Name = "radioButtonLookahead";
            this.radioButtonLookahead.Size = new System.Drawing.Size(95, 34);
            this.radioButtonLookahead.TabIndex = 0;
            this.radioButtonLookahead.TabStop = true;
            this.radioButtonLookahead.Text = "Look&ahead";
            this.radioButtonLookahead.UseVisualStyleBackColor = true;
            this.radioButtonLookahead.CheckedChanged += new System.EventHandler(this.radioButtonLookahead_CheckedChanged);
            // 
            // gridCriteriaLookahead
            // 
            this.gridCriteriaLookahead.AllowUserToAddRows = false;
            this.gridCriteriaLookahead.AllowUserToDeleteRows = false;
            this.gridCriteriaLookahead.AllowUserToResizeRows = false;
            this.gridCriteriaLookahead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCriteriaLookahead.BackgroundColor = System.Drawing.Color.White;
            this.gridCriteriaLookahead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCriteriaLookahead.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLookAheadEnabled,
            this.dataGridViewTextBoxColumn3});
            this.tableLayoutPanel2.SetColumnSpan(this.gridCriteriaLookahead, 3);
            this.gridCriteriaLookahead.Location = new System.Drawing.Point(0, 40);
            this.gridCriteriaLookahead.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.gridCriteriaLookahead.Name = "gridCriteriaLookahead";
            this.gridCriteriaLookahead.RowHeadersVisible = false;
            this.gridCriteriaLookahead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCriteriaLookahead.Size = new System.Drawing.Size(450, 395);
            this.gridCriteriaLookahead.TabIndex = 2;
            this.gridCriteriaLookahead.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.gridCriteriaLookahead.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
            this.gridCriteriaLookahead.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            this.gridCriteriaLookahead.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_DefaultValuesNeeded);
            this.gridCriteriaLookahead.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_Validated);
            // 
            // colLookAheadEnabled
            // 
            this.colLookAheadEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colLookAheadEnabled.HeaderText = "";
            this.colLookAheadEnabled.MinimumWidth = 35;
            this.colLookAheadEnabled.Name = "colLookAheadEnabled";
            this.colLookAheadEnabled.Width = 35;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Values";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // radioButtonNegativeLookahead
            // 
            this.radioButtonNegativeLookahead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radioButtonNegativeLookahead.Location = new System.Drawing.Point(456, 3);
            this.radioButtonNegativeLookahead.Name = "radioButtonNegativeLookahead";
            this.radioButtonNegativeLookahead.Size = new System.Drawing.Size(104, 34);
            this.radioButtonNegativeLookahead.TabIndex = 1;
            this.radioButtonNegativeLookahead.Text = "Neg L&ookahead";
            this.radioButtonNegativeLookahead.UseVisualStyleBackColor = true;
            this.radioButtonNegativeLookahead.CheckedChanged += new System.EventHandler(this.radioButtonNegativeLookahead_CheckedChanged);
            // 
            // btnDeleteLookAhead
            // 
            this.btnDeleteLookAhead.DrawNormalBorder = false;
            this.btnDeleteLookAhead.HoverImage = null;
            this.btnDeleteLookAhead.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteLookAhead.Image")));
            this.btnDeleteLookAhead.IsHighlighted = false;
            this.btnDeleteLookAhead.Location = new System.Drawing.Point(250, 3);
            this.btnDeleteLookAhead.Name = "btnDeleteLookAhead";
            this.btnDeleteLookAhead.NormalImage = null;
            this.btnDeleteLookAhead.PressedImage = null;
            this.btnDeleteLookAhead.Size = new System.Drawing.Size(146, 34);
            this.btnDeleteLookAhead.TabIndex = 6;
            this.btnDeleteLookAhead.Text = "Delete Selected";
            this.btnDeleteLookAhead.UseVisualStyleBackColor = true;
            this.btnDeleteLookAhead.Click += new System.EventHandler(this.btnDeleteCriteria_Click);
            // 
            // gridCriteriaNegativeLookahead
            // 
            this.gridCriteriaNegativeLookahead.AllowUserToAddRows = false;
            this.gridCriteriaNegativeLookahead.AllowUserToDeleteRows = false;
            this.gridCriteriaNegativeLookahead.AllowUserToResizeRows = false;
            this.gridCriteriaNegativeLookahead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCriteriaNegativeLookahead.BackgroundColor = System.Drawing.Color.White;
            this.gridCriteriaNegativeLookahead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCriteriaNegativeLookahead.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNegLookAheadEnabled,
            this.dataGridViewTextBoxColumn2});
            this.tableLayoutPanel2.SetColumnSpan(this.gridCriteriaNegativeLookahead, 3);
            this.gridCriteriaNegativeLookahead.Location = new System.Drawing.Point(456, 40);
            this.gridCriteriaNegativeLookahead.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.gridCriteriaNegativeLookahead.Name = "gridCriteriaNegativeLookahead";
            this.gridCriteriaNegativeLookahead.RowHeadersVisible = false;
            this.gridCriteriaNegativeLookahead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCriteriaNegativeLookahead.Size = new System.Drawing.Size(452, 395);
            this.gridCriteriaNegativeLookahead.TabIndex = 3;
            this.gridCriteriaNegativeLookahead.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.gridCriteriaNegativeLookahead.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellEndEdit);
            this.gridCriteriaNegativeLookahead.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            this.gridCriteriaNegativeLookahead.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_DefaultValuesNeeded);
            this.gridCriteriaNegativeLookahead.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_Validated);
            // 
            // colNegLookAheadEnabled
            // 
            this.colNegLookAheadEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colNegLookAheadEnabled.HeaderText = "";
            this.colNegLookAheadEnabled.MinimumWidth = 35;
            this.colNegLookAheadEnabled.Name = "colNegLookAheadEnabled";
            this.colNegLookAheadEnabled.Width = 35;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Values";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // btnDeleteNegLookAhead
            // 
            this.btnDeleteNegLookAhead.DrawNormalBorder = false;
            this.btnDeleteNegLookAhead.HoverImage = null;
            this.btnDeleteNegLookAhead.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteNegLookAhead.Image")));
            this.btnDeleteNegLookAhead.IsHighlighted = false;
            this.btnDeleteNegLookAhead.Location = new System.Drawing.Point(703, 3);
            this.btnDeleteNegLookAhead.Name = "btnDeleteNegLookAhead";
            this.btnDeleteNegLookAhead.NormalImage = null;
            this.btnDeleteNegLookAhead.PressedImage = null;
            this.btnDeleteNegLookAhead.Size = new System.Drawing.Size(148, 34);
            this.btnDeleteNegLookAhead.TabIndex = 5;
            this.btnDeleteNegLookAhead.Text = "Delete Selected";
            this.btnDeleteNegLookAhead.UseVisualStyleBackColor = true;
            this.btnDeleteNegLookAhead.Click += new System.EventHandler(this.btnDeleteCriteria_Click);
            // 
            // btnAddLookAhead
            // 
            this.btnAddLookAhead.DrawNormalBorder = false;
            this.btnAddLookAhead.HoverImage = null;
            this.btnAddLookAhead.Image = global::RegScoreCalc.Properties.Resources.add_small;
            this.btnAddLookAhead.IsHighlighted = false;
            this.btnAddLookAhead.Location = new System.Drawing.Point(113, 3);
            this.btnAddLookAhead.Name = "btnAddLookAhead";
            this.btnAddLookAhead.NormalImage = null;
            this.btnAddLookAhead.PressedImage = null;
            this.btnAddLookAhead.Size = new System.Drawing.Size(95, 34);
            this.btnAddLookAhead.TabIndex = 6;
            this.btnAddLookAhead.Text = "Add New";
            this.btnAddLookAhead.UseVisualStyleBackColor = true;
            this.btnAddLookAhead.Click += new System.EventHandler(this.btnAddCriteria_Click);
            // 
            // btnAddNegLookAhead
            // 
            this.btnAddNegLookAhead.DrawNormalBorder = false;
            this.btnAddNegLookAhead.HoverImage = null;
            this.btnAddNegLookAhead.Image = global::RegScoreCalc.Properties.Resources.add_small;
            this.btnAddNegLookAhead.IsHighlighted = false;
            this.btnAddNegLookAhead.Location = new System.Drawing.Point(566, 3);
            this.btnAddNegLookAhead.Name = "btnAddNegLookAhead";
            this.btnAddNegLookAhead.NormalImage = null;
            this.btnAddNegLookAhead.PressedImage = null;
            this.btnAddNegLookAhead.Size = new System.Drawing.Size(95, 34);
            this.btnAddNegLookAhead.TabIndex = 5;
            this.btnAddNegLookAhead.Text = "Add New";
            this.btnAddNegLookAhead.UseVisualStyleBackColor = true;
            this.btnAddNegLookAhead.Click += new System.EventHandler(this.btnAddCriteria_Click);
            // 
            // tabPageExceptions
            // 
            this.tabPageExceptions.Controls.Add(this.tableLayoutPanel3);
            this.tabPageExceptions.ImageIndex = 0;
            this.tabPageExceptions.Location = new System.Drawing.Point(4, 25);
            this.tabPageExceptions.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageExceptions.Name = "tabPageExceptions";
            this.tabPageExceptions.Size = new System.Drawing.Size(908, 435);
            this.tabPageExceptions.TabIndex = 3;
            this.tabPageExceptions.Text = "Exceptions";
            this.tabPageExceptions.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.gridCriteriaExceptions, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnAddException, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDeleteExceptions, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(908, 435);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // gridCriteriaExceptions
            // 
            this.gridCriteriaExceptions.AllowUserToAddRows = false;
            this.gridCriteriaExceptions.AllowUserToDeleteRows = false;
            this.gridCriteriaExceptions.AllowUserToResizeRows = false;
            this.gridCriteriaExceptions.BackgroundColor = System.Drawing.Color.White;
            this.gridCriteriaExceptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCriteriaExceptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colExceptionsEnabled,
            this.dataGridViewTextBoxColumn4});
            this.tableLayoutPanel3.SetColumnSpan(this.gridCriteriaExceptions, 2);
            this.gridCriteriaExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCriteriaExceptions.Location = new System.Drawing.Point(0, 40);
            this.gridCriteriaExceptions.Margin = new System.Windows.Forms.Padding(0);
            this.gridCriteriaExceptions.Name = "gridCriteriaExceptions";
            this.gridCriteriaExceptions.RowHeadersVisible = false;
            this.gridCriteriaExceptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCriteriaExceptions.Size = new System.Drawing.Size(908, 395);
            this.gridCriteriaExceptions.TabIndex = 0;
            this.gridCriteriaExceptions.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            this.gridCriteriaExceptions.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_DefaultValuesNeeded);
            this.gridCriteriaExceptions.RowValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_Validated);
            // 
            // colExceptionsEnabled
            // 
            this.colExceptionsEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.colExceptionsEnabled.HeaderText = "";
            this.colExceptionsEnabled.MinimumWidth = 35;
            this.colExceptionsEnabled.Name = "colExceptionsEnabled";
            this.colExceptionsEnabled.Width = 35;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "Values";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // btnAddException
            // 
            this.btnAddException.DrawNormalBorder = false;
            this.btnAddException.HoverImage = null;
            this.btnAddException.Image = global::RegScoreCalc.Properties.Resources.add_small;
            this.btnAddException.IsHighlighted = false;
            this.btnAddException.Location = new System.Drawing.Point(3, 3);
            this.btnAddException.Name = "btnAddException";
            this.btnAddException.NormalImage = null;
            this.btnAddException.PressedImage = null;
            this.btnAddException.Size = new System.Drawing.Size(95, 34);
            this.btnAddException.TabIndex = 8;
            this.btnAddException.Text = "Add New";
            this.btnAddException.UseVisualStyleBackColor = true;
            this.btnAddException.Click += new System.EventHandler(this.btnAddCriteria_Click);
            // 
            // btnDeleteExceptions
            // 
            this.btnDeleteExceptions.DrawNormalBorder = false;
            this.btnDeleteExceptions.HoverImage = null;
            this.btnDeleteExceptions.Image = global::RegScoreCalc.Properties.Resources.DeselectAll;
            this.btnDeleteExceptions.IsHighlighted = false;
            this.btnDeleteExceptions.Location = new System.Drawing.Point(133, 3);
            this.btnDeleteExceptions.Name = "btnDeleteExceptions";
            this.btnDeleteExceptions.NormalImage = null;
            this.btnDeleteExceptions.PressedImage = null;
            this.btnDeleteExceptions.Size = new System.Drawing.Size(158, 34);
            this.btnDeleteExceptions.TabIndex = 7;
            this.btnDeleteExceptions.Text = "Delete Selected";
            this.btnDeleteExceptions.UseVisualStyleBackColor = true;
            this.btnDeleteExceptions.Click += new System.EventHandler(this.btnDeleteCriteria_Click);
            // 
            // tabPageStatistics
            // 
            this.tabPageStatistics.Controls.Add(this.gridStatistics);
            this.tabPageStatistics.Controls.Add(this.toolStripTop);
            this.tabPageStatistics.Location = new System.Drawing.Point(4, 25);
            this.tabPageStatistics.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Size = new System.Drawing.Size(908, 435);
            this.tabPageStatistics.TabIndex = 3;
            this.tabPageStatistics.Text = "Statistics";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            // 
            // gridStatistics
            // 
            this.gridStatistics.AllowUserToAddRows = false;
            this.gridStatistics.AllowUserToDeleteRows = false;
            this.gridStatistics.AllowUserToResizeRows = false;
            this.gridStatistics.BackgroundColor = System.Drawing.Color.White;
            this.gridStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colExceptions,
            this.colWord,
            this.colMatchesCount,
            this.colMatchesCountPercentage,
            this.colMatchinglDocsCount,
            this.colMatchinglRecordsCount,
            this.colMatchingDocsPercentage,
            this.colAlllDocsPercentage});
            this.gridStatistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridStatistics.Location = new System.Drawing.Point(0, 25);
            this.gridStatistics.Margin = new System.Windows.Forms.Padding(0);
            this.gridStatistics.Name = "gridStatistics";
            this.gridStatistics.RowHeadersVisible = false;
            this.gridStatistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStatistics.Size = new System.Drawing.Size(908, 410);
            this.gridStatistics.TabIndex = 1;
            this.gridStatistics.VirtualMode = true;
            this.gridStatistics.DataSourceChanged += new System.EventHandler(this.GridStatistics_DataSourceChanged);
            this.gridStatistics.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridStatistics_CellValueNeeded);
            this.gridStatistics.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.gridStatistics_CellValuePushed);
            this.gridStatistics.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridStatistics_ColumnHeaderMouseClick);
            this.gridStatistics.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridStatistics_CurrentCellDirtyStateChanged);
            this.gridStatistics.SelectionChanged += new System.EventHandler(this.GridStatistics_SelectionChanged);
            // 
            // colExceptions
            // 
            this.colExceptions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colExceptions.DataPropertyName = "Exception";
            this.colExceptions.HeaderText = "Ex";
            this.colExceptions.MinimumWidth = 35;
            this.colExceptions.Name = "colExceptions";
            this.colExceptions.Width = 35;
            // 
            // colWord
            // 
            this.colWord.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colWord.DataPropertyName = "Word";
            this.colWord.HeaderText = "Word";
            this.colWord.Name = "colWord";
            this.colWord.ReadOnly = true;
            // 
            // colMatchesCount
            // 
            this.colMatchesCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMatchesCount.DataPropertyName = "MatchesCount";
            this.colMatchesCount.HeaderText = "Count";
            this.colMatchesCount.Name = "colMatchesCount";
            this.colMatchesCount.ReadOnly = true;
            this.colMatchesCount.Width = 67;
            // 
            // colMatchesCountPercentage
            // 
            this.colMatchesCountPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMatchesCountPercentage.DataPropertyName = "MatchesCountPercentage";
            dataGridViewCellStyle1.Format = "0.00";
            this.colMatchesCountPercentage.DefaultCellStyle = dataGridViewCellStyle1;
            this.colMatchesCountPercentage.HeaderText = "% Count";
            this.colMatchesCountPercentage.Name = "colMatchesCountPercentage";
            this.colMatchesCountPercentage.ReadOnly = true;
            this.colMatchesCountPercentage.Width = 76;
            // 
            // colMatchinglDocsCount
            // 
            this.colMatchinglDocsCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMatchinglDocsCount.DataPropertyName = "MatchingDocsCount";
            this.colMatchinglDocsCount.HeaderText = "Docs";
            this.colMatchinglDocsCount.Name = "colMatchinglDocsCount";
            this.colMatchinglDocsCount.ReadOnly = true;
            this.colMatchinglDocsCount.Width = 65;
            // 
            // colMatchinglRecordsCount
            // 
            this.colMatchinglRecordsCount.DataPropertyName = "MatchingRecordsCount";
            this.colMatchinglRecordsCount.HeaderText = "Records";
            this.colMatchinglRecordsCount.Name = "colMatchinglRecordsCount";
            this.colMatchinglRecordsCount.ReadOnly = true;
            // 
            // colMatchingDocsPercentage
            // 
            this.colMatchingDocsPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMatchingDocsPercentage.DataPropertyName = "MatchingDocsPercentage";
            dataGridViewCellStyle2.Format = "0.00";
            this.colMatchingDocsPercentage.DefaultCellStyle = dataGridViewCellStyle2;
            this.colMatchingDocsPercentage.HeaderText = "% Matching Docs";
            this.colMatchingDocsPercentage.Name = "colMatchingDocsPercentage";
            this.colMatchingDocsPercentage.ReadOnly = true;
            this.colMatchingDocsPercentage.Width = 125;
            // 
            // colAlllDocsPercentage
            // 
            this.colAlllDocsPercentage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAlllDocsPercentage.DataPropertyName = "AllDocsPercentage";
            dataGridViewCellStyle3.Format = "0.00";
            this.colAlllDocsPercentage.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAlllDocsPercentage.HeaderText = "% All Docs";
            this.colAlllDocsPercentage.Name = "colAlllDocsPercentage";
            this.colAlllDocsPercentage.ReadOnly = true;
            this.colAlllDocsPercentage.Width = 90;
            // 
            // toolStripTop
            // 
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chkbExcludeLookArounds,
            this.btnCalculate,
            this.toolStripSeparator1,
            this.btnStatisticsPrevMatch,
            this.btnStatisticsNextMatch,
            this.btnSimilar});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(908, 25);
            this.toolStripTop.TabIndex = 2;
            // 
            // chkbExcludeLookArounds
            // 
            this.chkbExcludeLookArounds.CheckOnClick = true;
            this.chkbExcludeLookArounds.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.chkbExcludeLookArounds.Image = ((System.Drawing.Image)(resources.GetObject("chkbExcludeLookArounds.Image")));
            this.chkbExcludeLookArounds.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.chkbExcludeLookArounds.Name = "chkbExcludeLookArounds";
            this.chkbExcludeLookArounds.Size = new System.Drawing.Size(123, 22);
            this.chkbExcludeLookArounds.Text = "Exclude Lookarounds";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Image = global::RegScoreCalc.Properties.Resources.CalcScores;
            this.btnCalculate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(76, 22);
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnStatisticsPrevMatch
            // 
            this.btnStatisticsPrevMatch.Image = global::RegScoreCalc.Properties.Resources.left_normal;
            this.btnStatisticsPrevMatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStatisticsPrevMatch.Name = "btnStatisticsPrevMatch";
            this.btnStatisticsPrevMatch.Size = new System.Drawing.Size(109, 22);
            this.btnStatisticsPrevMatch.Text = "Previous Match";
            this.btnStatisticsPrevMatch.Click += new System.EventHandler(this.btnStatisticsPrevMatch_Click);
            // 
            // btnStatisticsNextMatch
            // 
            this.btnStatisticsNextMatch.Image = global::RegScoreCalc.Properties.Resources.right_normal;
            this.btnStatisticsNextMatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStatisticsNextMatch.Name = "btnStatisticsNextMatch";
            this.btnStatisticsNextMatch.Size = new System.Drawing.Size(88, 22);
            this.btnStatisticsNextMatch.Text = "Next Match";
            this.btnStatisticsNextMatch.Click += new System.EventHandler(this.btnStatisticsNextMatch_Click);
            // 
            // btnSimilar
            // 
            this.btnSimilar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSimilar.Enabled = false;
            this.btnSimilar.Image = ((System.Drawing.Image)(resources.GetObject("btnSimilar.Image")));
            this.btnSimilar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSimilar.Name = "btnSimilar";
            this.btnSimilar.Size = new System.Drawing.Size(47, 22);
            this.btnSimilar.Text = "Similar";
            this.btnSimilar.Click += new System.EventHandler(this.btnSimilar_Click);
            // 
            // tabPageAssignCategory
            // 
            this.tabPageAssignCategory.Controls.Add(this.btnClear);
            this.tabPageAssignCategory.Controls.Add(this.btnSelectCategory);
            this.tabPageAssignCategory.Controls.Add(this.txtCategory);
            this.tabPageAssignCategory.Location = new System.Drawing.Point(4, 25);
            this.tabPageAssignCategory.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageAssignCategory.Name = "tabPageAssignCategory";
            this.tabPageAssignCategory.Size = new System.Drawing.Size(908, 435);
            this.tabPageAssignCategory.TabIndex = 3;
            this.tabPageAssignCategory.Text = "Assign Category";
            this.tabPageAssignCategory.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.DrawNormalBorder = false;
            this.btnClear.HoverImage = null;
            this.btnClear.IsHighlighted = false;
            this.btnClear.Location = new System.Drawing.Point(417, 9);
            this.btnClear.Name = "btnClear";
            this.btnClear.NormalImage = null;
            this.btnClear.PressedImage = null;
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSelectCategory
            // 
            this.btnSelectCategory.DrawNormalBorder = false;
            this.btnSelectCategory.HoverImage = null;
            this.btnSelectCategory.IsHighlighted = false;
            this.btnSelectCategory.Location = new System.Drawing.Point(286, 8);
            this.btnSelectCategory.Name = "btnSelectCategory";
            this.btnSelectCategory.NormalImage = null;
            this.btnSelectCategory.PressedImage = null;
            this.btnSelectCategory.Size = new System.Drawing.Size(125, 23);
            this.btnSelectCategory.TabIndex = 1;
            this.btnSelectCategory.Text = "Select Category";
            this.btnSelectCategory.UseVisualStyleBackColor = true;
            this.btnSelectCategory.Click += new System.EventHandler(this.btnSelectCategory_Click);
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(8, 8);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.ReadOnly = true;
            this.txtCategory.Size = new System.Drawing.Size(272, 22);
            this.txtCategory.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "Values";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.HeaderText = "Values";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn7.HeaderText = "Values";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // FormRegularExpressionEditor
            // 
            this.ClientSize = new System.Drawing.Size(924, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.panelNavButtons);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControlEditRegEx);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegularExpressionEditor";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Regular Expression Editor";
            this.Load += new System.EventHandler(this.FormRegularExpressionEditor_Load);
            this.Leave += new System.EventHandler(this.FormRegularExpressionEditor_Leave);
            this.panelNavButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControlEditRegEx.ResumeLayout(false);
            this.tabPageEditor.ResumeLayout(false);
            this.splitterMain.Panel1.ResumeLayout(false);
            this.splitterMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterMain)).EndInit();
            this.splitterMain.ResumeLayout(false);
            this.splitterRegExpAndExamples.Panel1.ResumeLayout(false);
            this.splitterRegExpAndExamples.Panel1.PerformLayout();
            this.splitterRegExpAndExamples.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterRegExpAndExamples)).EndInit();
            this.splitterRegExpAndExamples.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExamples)).EndInit();
            this.tabPageLookbehind.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaLookbehind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaNegativeLookbehind)).EndInit();
            this.tabPageLookahead.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaLookahead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaNegativeLookahead)).EndInit();
            this.tabPageExceptions.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCriteriaExceptions)).EndInit();
            this.tabPageStatistics.ResumeLayout(false);
            this.tabPageStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStatistics)).EndInit();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.tabPageAssignCategory.ResumeLayout(false);
            this.tabPageAssignCategory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomTabControl tabControlEditRegEx;
        private CustomTabPage tabPageEditor;
        private CustomTabPage tabPageLookbehind;
        private CustomTabPage tabPageLookahead;
		private CustomTabPage tabPageExceptions;
		private CustomTabPage tabPageStatistics;
		private CustomTabPage tabPageAssignCategory;
		private CustomTextBox txtRegExp;
		private System.Windows.Forms.DataGridView gridCriteriaNegativeLookahead;
		private System.Windows.Forms.DataGridView gridCriteriaLookahead;
        private System.Windows.Forms.RadioButton radioButtonNegativeLookahead;
        private System.Windows.Forms.RadioButton radioButtonLookahead;
		private System.Windows.Forms.DataGridView gridCriteriaExceptions;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridView dataGridViewExamples;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnRegEx;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.SplitContainer splitterRegExpAndExamples;
		private System.Windows.Forms.SplitContainer splitterMain;
		private Controls.ToolboxCtrl lvQuickActions;
		private RibbonStyleButton btnShowExamples;
		private RibbonStyleButton btnShowToolbox;
		private Panel panelNavButtons;
		private RibbonStyleButton btnPrevDocument;
		private RibbonStyleButton btnNextDocument;
		private RibbonStyleButton btnNextUniqueMatch;
		private RibbonStyleButton btnNextMatch;
		private RibbonStyleButton btnPrevUniqueMatch;
		private RibbonStyleButton btnPrevMatch;
		private DataGridViewCheckBoxColumn colExceptionsEnabled;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private TableLayoutPanel tableLayoutPanel1;
		private RadioButton radioButtonNegativeLookbehind;
		private RadioButton radioButtonLookbehind;
		private DataGridView gridCriteriaLookbehind;
		private DataGridView gridCriteriaNegativeLookbehind;
		private ToolStrip miniToolStrip;
		private TableLayoutPanel tableLayoutPanel3;
		private ToolTip toolTip;
		private RibbonStyleButton btnApply;
		private RibbonStyleButton btnDeleteLookBehind;
		private RibbonStyleButton btnDeleteNegLookBehind;
		private RibbonStyleButton btnDeleteLookAhead;
		private RibbonStyleButton btnDeleteNegLookAhead;
		private RibbonStyleButton btnDeleteExceptions;
		private ToolStrip toolStripTop;
		private DataGridView gridStatistics;
		private ToolStripButton btnCalculate;
		private ToolStripButton btnStatisticsPrevMatch;
		private ToolStripButton btnStatisticsNextMatch;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton chkbExcludeLookArounds;
		private Label lblDescription;
		private Panel panel1;
		private RibbonStyleButton btnEditDescription;
		private ToolTip toolTipGeneral;
		private RibbonStyleButton btnClear;
		private RibbonStyleButton btnSelectCategory;
		private TextBox txtCategory;
		private RibbonStyleButton btnAddLookBehind;
		private RibbonStyleButton btnAddNegLookBehind;
		private RibbonStyleButton btnAddLookAhead;
		private RibbonStyleButton btnAddNegLookAhead;
		private DataGridViewCheckBoxColumn colLookAheadEnabled;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private DataGridViewCheckBoxColumn colNegLookAheadEnabled;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private DataGridViewCheckBoxColumn colLookBehindEnabled;
		private DataGridViewTextBoxColumn colValue;
		private DataGridViewCheckBoxColumn colNegLookBehindEnabled;
		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private ImageList imageList;
		private RibbonStyleButton btnAddException;
        private DataGridViewCheckBoxColumn colExceptions;
        private DataGridViewTextBoxColumn colWord;
        private DataGridViewTextBoxColumn colMatchesCount;
        private DataGridViewTextBoxColumn colMatchesCountPercentage;
        private DataGridViewTextBoxColumn colMatchinglDocsCount;
        private DataGridViewTextBoxColumn colMatchinglRecordsCount;
        private DataGridViewTextBoxColumn colMatchingDocsPercentage;
        private DataGridViewTextBoxColumn colAlllDocsPercentage;
        private ToolStripButton btnSimilar;
    }
}