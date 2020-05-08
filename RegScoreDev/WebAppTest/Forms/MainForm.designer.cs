namespace WebAppTest.Forms
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainerRoot = new System.Windows.Forms.SplitContainer();
            this.splitContainerActionsAndLog = new System.Windows.Forms.SplitContainer();
            this.splitContainerActions = new System.Windows.Forms.SplitContainer();
            this.btnRemoveAction = new System.Windows.Forms.Button();
            this.btnAddAction = new System.Windows.Forms.Button();
            this.lbSelectedActions = new System.Windows.Forms.ListBox();
            this.lblActionName = new System.Windows.Forms.Label();
            this.panelAction = new System.Windows.Forms.Panel();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoadTest = new System.Windows.Forms.Button();
            this.btnSaveTest = new System.Windows.Forms.Button();
            this.chkScreenShots = new System.Windows.Forms.CheckBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtRunTimes = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRootURL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRoot)).BeginInit();
            this.splitContainerRoot.Panel1.SuspendLayout();
            this.splitContainerRoot.Panel2.SuspendLayout();
            this.splitContainerRoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerActionsAndLog)).BeginInit();
            this.splitContainerActionsAndLog.Panel1.SuspendLayout();
            this.splitContainerActionsAndLog.Panel2.SuspendLayout();
            this.splitContainerActionsAndLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerActions)).BeginInit();
            this.splitContainerActions.Panel1.SuspendLayout();
            this.splitContainerActions.Panel2.SuspendLayout();
            this.splitContainerActions.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtRunTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.Filter = "XML Files|*.XML||";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.Filter = "XML Files|*.XML||";
            // 
            // splitContainerRoot
            // 
            this.splitContainerRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRoot.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerRoot.IsSplitterFixed = true;
            this.splitContainerRoot.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRoot.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerRoot.Name = "splitContainerRoot";
            this.splitContainerRoot.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRoot.Panel1
            // 
            this.splitContainerRoot.Panel1.Controls.Add(this.splitContainerActionsAndLog);
            // 
            // splitContainerRoot.Panel2
            // 
            this.splitContainerRoot.Panel2.Controls.Add(this.button1);
            this.splitContainerRoot.Panel2.Controls.Add(this.btnExit);
            this.splitContainerRoot.Panel2.Controls.Add(this.groupBox1);
            this.splitContainerRoot.Panel2.Controls.Add(this.btnLoadTest);
            this.splitContainerRoot.Panel2.Controls.Add(this.btnSaveTest);
            this.splitContainerRoot.Panel2.Controls.Add(this.chkScreenShots);
            this.splitContainerRoot.Panel2.Controls.Add(this.btnRun);
            this.splitContainerRoot.Panel2.Controls.Add(this.btnStop);
            this.splitContainerRoot.Panel2.Controls.Add(this.txtRunTimes);
            this.splitContainerRoot.Panel2.Controls.Add(this.label5);
            this.splitContainerRoot.Panel2.Controls.Add(this.txtRootURL);
            this.splitContainerRoot.Panel2.Controls.Add(this.label4);
            this.splitContainerRoot.Size = new System.Drawing.Size(1439, 679);
            this.splitContainerRoot.SplitterDistance = 565;
            this.splitContainerRoot.SplitterWidth = 11;
            this.splitContainerRoot.TabIndex = 0;
            // 
            // splitContainerActionsAndLog
            // 
            this.splitContainerActionsAndLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerActionsAndLog.Location = new System.Drawing.Point(0, 0);
            this.splitContainerActionsAndLog.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerActionsAndLog.Name = "splitContainerActionsAndLog";
            // 
            // splitContainerActionsAndLog.Panel1
            // 
            this.splitContainerActionsAndLog.Panel1.Controls.Add(this.splitContainerActions);
            // 
            // splitContainerActionsAndLog.Panel2
            // 
            this.splitContainerActionsAndLog.Panel2.Controls.Add(this.btnClearLog);
            this.splitContainerActionsAndLog.Panel2.Controls.Add(this.label1);
            this.splitContainerActionsAndLog.Panel2.Controls.Add(this.panel2);
            this.splitContainerActionsAndLog.Size = new System.Drawing.Size(1439, 565);
            this.splitContainerActionsAndLog.SplitterDistance = 914;
            this.splitContainerActionsAndLog.SplitterWidth = 11;
            this.splitContainerActionsAndLog.TabIndex = 0;
            // 
            // splitContainerActions
            // 
            this.splitContainerActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerActions.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerActions.Location = new System.Drawing.Point(0, 0);
            this.splitContainerActions.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerActions.Name = "splitContainerActions";
            // 
            // splitContainerActions.Panel1
            // 
            this.splitContainerActions.Panel1.Controls.Add(this.btnRemoveAction);
            this.splitContainerActions.Panel1.Controls.Add(this.btnAddAction);
            this.splitContainerActions.Panel1.Controls.Add(this.lbSelectedActions);
            // 
            // splitContainerActions.Panel2
            // 
            this.splitContainerActions.Panel2.Controls.Add(this.lblActionName);
            this.splitContainerActions.Panel2.Controls.Add(this.panelAction);
            this.splitContainerActions.Size = new System.Drawing.Size(914, 565);
            this.splitContainerActions.SplitterDistance = 254;
            this.splitContainerActions.SplitterWidth = 11;
            this.splitContainerActions.TabIndex = 0;
            // 
            // btnRemoveAction
            // 
            this.btnRemoveAction.Location = new System.Drawing.Point(137, 14);
            this.btnRemoveAction.Margin = new System.Windows.Forms.Padding(4);
            this.btnRemoveAction.Name = "btnRemoveAction";
            this.btnRemoveAction.Size = new System.Drawing.Size(109, 26);
            this.btnRemoveAction.TabIndex = 1;
            this.btnRemoveAction.Text = "&Remove Action";
            this.btnRemoveAction.UseVisualStyleBackColor = true;
            this.btnRemoveAction.Click += new System.EventHandler(this.btnRemoveAction_Click);
            // 
            // btnAddAction
            // 
            this.btnAddAction.Location = new System.Drawing.Point(14, 14);
            this.btnAddAction.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddAction.Name = "btnAddAction";
            this.btnAddAction.Size = new System.Drawing.Size(114, 26);
            this.btnAddAction.TabIndex = 0;
            this.btnAddAction.Text = "&Add Action";
            this.btnAddAction.UseVisualStyleBackColor = true;
            this.btnAddAction.Click += new System.EventHandler(this.btnAddAction_Click);
            // 
            // lbSelectedActions
            // 
            this.lbSelectedActions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSelectedActions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSelectedActions.FormattingEnabled = true;
            this.lbSelectedActions.IntegralHeight = false;
            this.lbSelectedActions.ItemHeight = 16;
            this.lbSelectedActions.Location = new System.Drawing.Point(14, 50);
            this.lbSelectedActions.Margin = new System.Windows.Forms.Padding(4);
            this.lbSelectedActions.Name = "lbSelectedActions";
            this.lbSelectedActions.Size = new System.Drawing.Size(239, 513);
            this.lbSelectedActions.TabIndex = 2;
            this.lbSelectedActions.SelectedIndexChanged += new System.EventHandler(this.lbSelectedActions_SelectedIndexChanged);
            // 
            // lblActionName
            // 
            this.lblActionName.AutoSize = true;
            this.lblActionName.Location = new System.Drawing.Point(11, 20);
            this.lblActionName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActionName.Name = "lblActionName";
            this.lblActionName.Size = new System.Drawing.Size(0, 15);
            this.lblActionName.TabIndex = 1;
            // 
            // panelAction
            // 
            this.panelAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelAction.Location = new System.Drawing.Point(6, 50);
            this.panelAction.Margin = new System.Windows.Forms.Padding(4);
            this.panelAction.Name = "panelAction";
            this.panelAction.Size = new System.Drawing.Size(621, 514);
            this.panelAction.TabIndex = 0;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearLog.Location = new System.Drawing.Point(388, 14);
            this.btnClearLog.Margin = new System.Windows.Forms.Padding(4);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(88, 26);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "&Clear";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Log:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtLog);
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(478, 515);
            this.panel2.TabIndex = 5;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLog.Location = new System.Drawing.Point(0, -1);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(477, 510);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            this.txtLog.WordWrap = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1239, 51);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 26);
            this.button1.TabIndex = 9;
            this.button1.Text = "DEBUG";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1334, 50);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(88, 26);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(-155, 1);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(1750, 2);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // btnLoadTest
            // 
            this.btnLoadTest.Location = new System.Drawing.Point(469, 49);
            this.btnLoadTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoadTest.Name = "btnLoadTest";
            this.btnLoadTest.Size = new System.Drawing.Size(88, 26);
            this.btnLoadTest.TabIndex = 3;
            this.btnLoadTest.Text = "&Load Test";
            this.btnLoadTest.UseVisualStyleBackColor = true;
            this.btnLoadTest.Click += new System.EventHandler(this.btnLoadTest_Click);
            // 
            // btnSaveTest
            // 
            this.btnSaveTest.Location = new System.Drawing.Point(564, 49);
            this.btnSaveTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveTest.Name = "btnSaveTest";
            this.btnSaveTest.Size = new System.Drawing.Size(88, 26);
            this.btnSaveTest.TabIndex = 4;
            this.btnSaveTest.Text = "&Save Test";
            this.btnSaveTest.UseVisualStyleBackColor = true;
            this.btnSaveTest.Click += new System.EventHandler(this.btnSaveTest_Click);
            // 
            // chkScreenShots
            // 
            this.chkScreenShots.AutoSize = true;
            this.chkScreenShots.Location = new System.Drawing.Point(217, 53);
            this.chkScreenShots.Margin = new System.Windows.Forms.Padding(4);
            this.chkScreenShots.Name = "chkScreenShots";
            this.chkScreenShots.Size = new System.Drawing.Size(125, 19);
            this.chkScreenShots.TabIndex = 2;
            this.chkScreenShots.Text = "Save screen shots";
            this.chkScreenShots.UseVisualStyleBackColor = true;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(1240, 15);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(88, 25);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "R&un";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStop.Location = new System.Drawing.Point(1334, 15);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(88, 25);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Sto&p";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtRunTimes
            // 
            this.txtRunTimes.Location = new System.Drawing.Point(109, 51);
            this.txtRunTimes.Margin = new System.Windows.Forms.Padding(4);
            this.txtRunTimes.Name = "txtRunTimes";
            this.txtRunTimes.Size = new System.Drawing.Size(59, 21);
            this.txtRunTimes.TabIndex = 1;
            this.txtRunTimes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 53);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 2;
            this.label5.Text = "Run N times:";
            // 
            // txtRootURL
            // 
            this.txtRootURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRootURL.Location = new System.Drawing.Point(109, 16);
            this.txtRootURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtRootURL.Name = "txtRootURL";
            this.txtRootURL.Size = new System.Drawing.Size(1103, 21);
            this.txtRootURL.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Root URL:";
            // 
            // notifyIcon
            // 
            this.notifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Visible = true;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnRun;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnStop;
            this.ClientSize = new System.Drawing.Size(1439, 679);
            this.Controls.Add(this.splitContainerRoot);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Web App Test";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainerRoot.Panel1.ResumeLayout(false);
            this.splitContainerRoot.Panel2.ResumeLayout(false);
            this.splitContainerRoot.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRoot)).EndInit();
            this.splitContainerRoot.ResumeLayout(false);
            this.splitContainerActionsAndLog.Panel1.ResumeLayout(false);
            this.splitContainerActionsAndLog.Panel2.ResumeLayout(false);
            this.splitContainerActionsAndLog.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerActionsAndLog)).EndInit();
            this.splitContainerActionsAndLog.ResumeLayout(false);
            this.splitContainerActions.Panel1.ResumeLayout(false);
            this.splitContainerActions.Panel2.ResumeLayout(false);
            this.splitContainerActions.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerActions)).EndInit();
            this.splitContainerActions.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtRunTimes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.SplitContainer splitContainerRoot;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.SplitContainer splitContainerActionsAndLog;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.SplitContainer splitContainerActions;
		private System.Windows.Forms.NumericUpDown txtRunTimes;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtRootURL;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.CheckBox chkScreenShots;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.Button btnLoadTest;
		private System.Windows.Forms.Button btnSaveTest;
		private System.Windows.Forms.RichTextBox txtLog;
		private System.Windows.Forms.ListBox lbSelectedActions;
		private System.Windows.Forms.Button btnRemoveAction;
		private System.Windows.Forms.Button btnAddAction;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.Panel panelAction;
		private System.Windows.Forms.Button btnClearLog;
		private System.Windows.Forms.Label lblActionName;
        private System.Windows.Forms.Button button1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
	}
}

