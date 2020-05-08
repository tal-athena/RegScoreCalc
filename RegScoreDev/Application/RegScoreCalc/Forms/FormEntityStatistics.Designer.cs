using System.Windows.Forms;

using RegScoreCalc.Code.Controls;

namespace RegScoreCalc.Forms
{
    partial class FormEntityStatistics : Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEntityStatistics));
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
            this.tabPageStatistics = new RegScoreCalc.CustomTabPage();
            this.gridStatistics = new System.Windows.Forms.DataGridView();
            this.toolStripTop = new System.Windows.Forms.ToolStrip();
            this.btnCalculate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStatisticsPrevMatch = new System.Windows.Forms.ToolStripButton();
            this.btnStatisticsNextMatch = new System.Windows.Forms.ToolStripButton();
            this.btnSimilar = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExceptions = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colWord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchesCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchesCountPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchinglDocsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchinglRecordsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMatchingDocsPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlllDocsPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelNavButtons.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControlEditRegEx.SuspendLayout();
            this.tabPageStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStatistics)).BeginInit();
            this.toolStripTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(809, 0);
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
            this.btnPrevDocument.Visible = false;
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
            this.btnNextDocument.Visible = false;
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
            this.btnNextUniqueMatch.Visible = false;
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
            this.btnNextMatch.Visible = false;
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
            this.btnPrevUniqueMatch.Visible = false;
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
            this.btnPrevMatch.Visible = false;
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
            this.lblDescription.Size = new System.Drawing.Size(771, 25);
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
            this.panel1.Size = new System.Drawing.Size(809, 25);
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
            this.btnApply.Location = new System.Drawing.Point(642, 495);
            this.btnApply.Name = "btnApply";
            this.btnApply.NormalImage = null;
            this.btnApply.PressedImage = null;
            this.btnApply.Size = new System.Drawing.Size(158, 30);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "Apply Changes";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Visible = false;
            // 
            // tabControlEditRegEx
            // 
            this.tabControlEditRegEx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlEditRegEx.Controls.Add(this.tabPageStatistics);
            this.tabControlEditRegEx.ImageList = this.imageList;
            this.tabControlEditRegEx.Location = new System.Drawing.Point(3, 25);
            this.tabControlEditRegEx.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlEditRegEx.Name = "tabControlEditRegEx";
            this.tabControlEditRegEx.SelectedIndex = 0;
            this.tabControlEditRegEx.ShowIndicators = false;
            this.tabControlEditRegEx.Size = new System.Drawing.Size(801, 464);
            this.tabControlEditRegEx.TabIndex = 1;
            // 
            // tabPageStatistics
            // 
            this.tabPageStatistics.Controls.Add(this.gridStatistics);
            this.tabPageStatistics.Controls.Add(this.toolStripTop);
            this.tabPageStatistics.Location = new System.Drawing.Point(4, 25);
            this.tabPageStatistics.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Size = new System.Drawing.Size(793, 435);
            this.tabPageStatistics.TabIndex = 3;
            this.tabPageStatistics.Text = "Statistics";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            // 
            // gridStatistics
            // 
            this.gridStatistics.AllowUserToAddRows = false;
            this.gridStatistics.AllowUserToDeleteRows = false;
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
            this.gridStatistics.ReadOnly = true;
            this.gridStatistics.RowHeadersVisible = false;
            this.gridStatistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridStatistics.Size = new System.Drawing.Size(793, 410);
            this.gridStatistics.TabIndex = 1;
            // 
            // toolStripTop
            // 
            this.toolStripTop.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCalculate,
            this.toolStripSeparator1,
            this.btnStatisticsPrevMatch,
            this.btnStatisticsNextMatch,
            this.btnSimilar});
            this.toolStripTop.Location = new System.Drawing.Point(0, 0);
            this.toolStripTop.Name = "toolStripTop";
            this.toolStripTop.Size = new System.Drawing.Size(793, 25);
            this.toolStripTop.TabIndex = 2;
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
            this.btnStatisticsPrevMatch.Visible = false;
            // 
            // btnStatisticsNextMatch
            // 
            this.btnStatisticsNextMatch.Image = global::RegScoreCalc.Properties.Resources.right_normal;
            this.btnStatisticsNextMatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStatisticsNextMatch.Name = "btnStatisticsNextMatch";
            this.btnStatisticsNextMatch.Size = new System.Drawing.Size(88, 22);
            this.btnStatisticsNextMatch.Text = "Next Match";
            this.btnStatisticsNextMatch.Visible = false;
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
            this.btnSimilar.Visible = false;
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
            // colExceptions
            // 
            this.colExceptions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colExceptions.HeaderText = "Ex";
            this.colExceptions.MinimumWidth = 35;
            this.colExceptions.Name = "colExceptions";
            this.colExceptions.ReadOnly = true;
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
            this.colMatchesCountPercentage.Width = 82;
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
            this.colAlllDocsPercentage.DataPropertyName = "MatchingRecordsPercentage";
            dataGridViewCellStyle3.Format = "0.00";
            this.colAlllDocsPercentage.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAlllDocsPercentage.HeaderText = "% All Docs";
            this.colAlllDocsPercentage.Name = "colAlllDocsPercentage";
            this.colAlllDocsPercentage.ReadOnly = true;
            this.colAlllDocsPercentage.Width = 90;
            // 
            // FormEntityStatistics
            // 
            this.ClientSize = new System.Drawing.Size(809, 530);
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
            this.Name = "FormEntityStatistics";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Regular Expression Editor";
            this.Load += new System.EventHandler(this.FormRegularExpressionEditor_Load);
            this.panelNavButtons.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControlEditRegEx.ResumeLayout(false);
            this.tabPageStatistics.ResumeLayout(false);
            this.tabPageStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridStatistics)).EndInit();
            this.toolStripTop.ResumeLayout(false);
            this.toolStripTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CustomTabControl tabControlEditRegEx;
		private CustomTabPage tabPageStatistics;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
		private System.Windows.Forms.Panel panel2;
		private Panel panelNavButtons;
		private RibbonStyleButton btnPrevDocument;
		private RibbonStyleButton btnNextDocument;
		private RibbonStyleButton btnNextUniqueMatch;
		private RibbonStyleButton btnNextMatch;
		private RibbonStyleButton btnPrevUniqueMatch;
		private RibbonStyleButton btnPrevMatch;
		private ToolStrip miniToolStrip;
		private ToolTip toolTip;
		private RibbonStyleButton btnApply;
		private ToolStrip toolStripTop;
		private DataGridView gridStatistics;
		private ToolStripButton btnCalculate;
		private ToolStripSeparator toolStripSeparator1;
		private Label lblDescription;
		private Panel panel1;
		private RibbonStyleButton btnEditDescription;
		private ToolTip toolTipGeneral;
		private ImageList imageList;
        private ToolStripButton btnSimilar;
        private ToolStripButton btnStatisticsPrevMatch;
        private ToolStripButton btnStatisticsNextMatch;
        private DataGridViewCheckBoxColumn colExceptions;
        private DataGridViewTextBoxColumn colWord;
        private DataGridViewTextBoxColumn colMatchesCount;
        private DataGridViewTextBoxColumn colMatchesCountPercentage;
        private DataGridViewTextBoxColumn colMatchinglDocsCount;
        private DataGridViewTextBoxColumn colMatchinglRecordsCount;
        private DataGridViewTextBoxColumn colMatchingDocsPercentage;
        private DataGridViewTextBoxColumn colAlllDocsPercentage;
    }
}