using RegExpMerge.Data;
namespace RegExpMerge
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ribbonMain = new System.Windows.Forms.Ribbon();
            this.ribbonTabMain = new System.Windows.Forms.RibbonTab();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataGridViewLeft = new System.Windows.Forms.DataGridView();
            this.panelLeftTop = new System.Windows.Forms.Panel();
            this.cmbLeftHistory = new System.Windows.Forms.ComboBox();
            this.btnOpenLeft = new System.Windows.Forms.Button();
            this.dataGridViewRight = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbRightHistory = new System.Windows.Forms.ComboBox();
            this.btnOpenRight = new System.Windows.Forms.Button();
            this.adapterRegExpLeft = new RegExpMerge.Data.DataSetMainTableAdapters.RegExpTableAdapter();
            this.adapterRegExpRight = new RegExpMerge.Data.DataSetMainTableAdapters.RegExpTableAdapter();
            this.datasetLeft = new RegExpMerge.Data.DataSetMain();
            this.datasetRight = new RegExpMerge.Data.DataSetMain();
            this.bindingSourceLeft = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceRight = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeft)).BeginInit();
            this.panelLeftTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datasetLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datasetRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceRight)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonMain
            // 
            this.ribbonMain.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.ribbonMain.Location = new System.Drawing.Point(0, 0);
            this.ribbonMain.Minimized = false;
            this.ribbonMain.Name = "ribbonMain";
            // 
            // 
            // 
            this.ribbonMain.OrbDropDown.BorderRoundness = 8;
            this.ribbonMain.OrbDropDown.Location = new System.Drawing.Point(0, 0);
            this.ribbonMain.OrbDropDown.Name = "";
            this.ribbonMain.OrbDropDown.Size = new System.Drawing.Size(527, 447);
            this.ribbonMain.OrbDropDown.TabIndex = 0;
            this.ribbonMain.OrbImage = null;
            // 
            // 
            // 
            this.ribbonMain.QuickAcessToolbar.AltKey = null;
            this.ribbonMain.QuickAcessToolbar.Image = null;
            this.ribbonMain.QuickAcessToolbar.Tag = null;
            this.ribbonMain.QuickAcessToolbar.Text = null;
            this.ribbonMain.QuickAcessToolbar.ToolTip = null;
            this.ribbonMain.QuickAcessToolbar.ToolTipImage = null;
            this.ribbonMain.QuickAcessToolbar.ToolTipTitle = null;
            this.ribbonMain.Size = new System.Drawing.Size(987, 138);
            this.ribbonMain.TabIndex = 0;
            this.ribbonMain.Tabs.Add(this.ribbonTabMain);
            this.ribbonMain.TabSpacing = 6;
            this.ribbonMain.Text = "RegExpMerge";
            // 
            // ribbonTabMain
            // 
            this.ribbonTabMain.Tag = null;
            this.ribbonTabMain.Text = "Main";
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 138);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataGridViewLeft);
            this.splitContainer.Panel1.Controls.Add(this.panelLeftTop);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataGridViewRight);
            this.splitContainer.Panel2.Controls.Add(this.panel1);
            this.splitContainer.Size = new System.Drawing.Size(987, 349);
            this.splitContainer.SplitterDistance = 489;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 1;
            // 
            // dataGridViewLeft
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewLeft.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewLeft.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewLeft.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLeft.Location = new System.Drawing.Point(0, 28);
            this.dataGridViewLeft.Name = "dataGridViewLeft";
            this.dataGridViewLeft.Size = new System.Drawing.Size(489, 321);
            this.dataGridViewLeft.TabIndex = 1;
            this.dataGridViewLeft.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLeft_CellDoubleClick);
            this.dataGridViewLeft.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLeft_CellValueChanged);
            this.dataGridViewLeft.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewLeft_ColumnHeaderMouseClick);
            this.dataGridViewLeft.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridViewLeft_Scroll);
            this.dataGridViewLeft.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewLeft_UserDeletedRow);
            // 
            // panelLeftTop
            // 
            this.panelLeftTop.Controls.Add(this.cmbLeftHistory);
            this.panelLeftTop.Controls.Add(this.btnOpenLeft);
            this.panelLeftTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLeftTop.Location = new System.Drawing.Point(0, 0);
            this.panelLeftTop.Name = "panelLeftTop";
            this.panelLeftTop.Size = new System.Drawing.Size(489, 28);
            this.panelLeftTop.TabIndex = 0;
            // 
            // cmbLeftHistory
            // 
            this.cmbLeftHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbLeftHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLeftHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLeftHistory.FormattingEnabled = true;
            this.cmbLeftHistory.Location = new System.Drawing.Point(0, 0);
            this.cmbLeftHistory.Name = "cmbLeftHistory";
            this.cmbLeftHistory.Size = new System.Drawing.Size(420, 28);
            this.cmbLeftHistory.TabIndex = 0;
            this.cmbLeftHistory.SelectedIndexChanged += new System.EventHandler(this.cmbLeftHistory_SelectedIndexChanged);
            // 
            // btnOpenLeft
            // 
            this.btnOpenLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOpenLeft.Location = new System.Drawing.Point(420, 0);
            this.btnOpenLeft.Name = "btnOpenLeft";
            this.btnOpenLeft.Size = new System.Drawing.Size(69, 28);
            this.btnOpenLeft.TabIndex = 1;
            this.btnOpenLeft.Text = "...";
            this.btnOpenLeft.UseVisualStyleBackColor = true;
            this.btnOpenLeft.Click += new System.EventHandler(this.btnOpenLeft_Click);
            // 
            // dataGridViewRight
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewRight.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewRight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewRight.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRight.Location = new System.Drawing.Point(0, 28);
            this.dataGridViewRight.Name = "dataGridViewRight";
            this.dataGridViewRight.Size = new System.Drawing.Size(493, 321);
            this.dataGridViewRight.TabIndex = 2;
            this.dataGridViewRight.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRight_CellDoubleClick);
            this.dataGridViewRight.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRight_CellValueChanged);
            this.dataGridViewRight.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewRight_ColumnHeaderMouseClick);
            this.dataGridViewRight.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridViewRight_Scroll);
            this.dataGridViewRight.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dataGridViewRight_UserDeletedRow);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbRightHistory);
            this.panel1.Controls.Add(this.btnOpenRight);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(493, 28);
            this.panel1.TabIndex = 1;
            // 
            // cmbRightHistory
            // 
            this.cmbRightHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbRightHistory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRightHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRightHistory.FormattingEnabled = true;
            this.cmbRightHistory.Location = new System.Drawing.Point(0, 0);
            this.cmbRightHistory.Name = "cmbRightHistory";
            this.cmbRightHistory.Size = new System.Drawing.Size(424, 28);
            this.cmbRightHistory.TabIndex = 0;
            this.cmbRightHistory.SelectedIndexChanged += new System.EventHandler(this.cmbRightHistory_SelectedIndexChanged);
            // 
            // btnOpenRight
            // 
            this.btnOpenRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOpenRight.Location = new System.Drawing.Point(424, 0);
            this.btnOpenRight.Name = "btnOpenRight";
            this.btnOpenRight.Size = new System.Drawing.Size(69, 28);
            this.btnOpenRight.TabIndex = 1;
            this.btnOpenRight.Text = "...";
            this.btnOpenRight.UseVisualStyleBackColor = true;
            this.btnOpenRight.Click += new System.EventHandler(this.btnOpenRight_Click);
            // 
            // adapterRegExpLeft
            // 
            this.adapterRegExpLeft.ClearBeforeFill = true;
            // 
            // adapterRegExpRight
            // 
            this.adapterRegExpRight.ClearBeforeFill = true;
            // 
            // datasetLeft
            // 
            this.datasetLeft.DataSetName = "DataSetRight";
            this.datasetLeft.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // datasetRight
            // 
            this.datasetRight.DataSetName = "DataSetMain";
            this.datasetRight.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingSourceLeft
            // 
            this.bindingSourceLeft.DataMember = "RegExp";
            this.bindingSourceLeft.DataSource = this.datasetLeft;
            // 
            // bindingSourceRight
            // 
            this.bindingSourceRight.DataMember = "RegExp";
            this.bindingSourceRight.DataSource = this.datasetRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 487);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.ribbonMain);
            this.Name = "MainForm";
            this.Text = "RegExpMerge";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLeft)).EndInit();
            this.panelLeftTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRight)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datasetLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datasetRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DataSetMain datasetLeft;
        public DataSetMain datasetRight;
        public RegExpMerge.Data.DataSetMainTableAdapters.RegExpTableAdapter adapterRegExpLeft;
        public RegExpMerge.Data.DataSetMainTableAdapters.RegExpTableAdapter adapterRegExpRight;

        private System.Windows.Forms.Ribbon ribbonMain;
        private System.Windows.Forms.RibbonTab ribbonTabMain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelLeftTop;
        private System.Windows.Forms.Button btnOpenLeft;
        private System.Windows.Forms.ComboBox cmbLeftHistory;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbRightHistory;
        private System.Windows.Forms.Button btnOpenRight;
        private System.Windows.Forms.DataGridView dataGridViewLeft;
        private System.Windows.Forms.BindingSource bindingSourceLeft;
        private System.Windows.Forms.BindingSource bindingSourceRight;
        private System.Windows.Forms.DataGridView dataGridViewRight;
    }
}

