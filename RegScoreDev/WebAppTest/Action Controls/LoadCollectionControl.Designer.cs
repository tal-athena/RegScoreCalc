namespace WebAppTest.ActionControls
{
    partial class LoadCollectionControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.lblId = new System.Windows.Forms.Label();
			this.lblRefresh = new System.Windows.Forms.Label();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblTimeout = new System.Windows.Forms.Label();
			this.numericRefresh = new System.Windows.Forms.NumericUpDown();
			this.numericTimeout = new System.Windows.Forms.NumericUpDown();
			this.checkBoxScreen = new System.Windows.Forms.CheckBox();
			this.checkBoxOpenInNewTab = new System.Windows.Forms.CheckBox();
			this.checkBoxDontWait = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numericRefresh)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).BeginInit();
			this.SuspendLayout();
			// 
			// lblId
			// 
			this.lblId.AutoSize = true;
			this.lblId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblId.Location = new System.Drawing.Point(8, 8);
			this.lblId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblId.Name = "lblId";
			this.lblId.Size = new System.Drawing.Size(21, 13);
			this.lblId.TabIndex = 0;
			this.lblId.Text = "ID:";
			// 
			// lblRefresh
			// 
			this.lblRefresh.AutoSize = true;
			this.lblRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblRefresh.Location = new System.Drawing.Point(8, 36);
			this.lblRefresh.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblRefresh.Name = "lblRefresh";
			this.lblRefresh.Size = new System.Drawing.Size(47, 13);
			this.lblRefresh.TabIndex = 1;
			this.lblRefresh.Text = "Refresh:";
			// 
			// txtID
			// 
			this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtID.Location = new System.Drawing.Point(109, 5);
			this.txtID.Margin = new System.Windows.Forms.Padding(2);
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(171, 20);
			this.txtID.TabIndex = 2;
			// 
			// lblTimeout
			// 
			this.lblTimeout.AutoSize = true;
			this.lblTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblTimeout.Location = new System.Drawing.Point(8, 63);
			this.lblTimeout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblTimeout.Name = "lblTimeout";
			this.lblTimeout.Size = new System.Drawing.Size(97, 13);
			this.lblTimeout.TabIndex = 3;
			this.lblTimeout.Text = "Timeout (seconds):";
			// 
			// numericRefresh
			// 
			this.numericRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.numericRefresh.Location = new System.Drawing.Point(109, 34);
			this.numericRefresh.Margin = new System.Windows.Forms.Padding(2);
			this.numericRefresh.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.numericRefresh.Name = "numericRefresh";
			this.numericRefresh.Size = new System.Drawing.Size(57, 20);
			this.numericRefresh.TabIndex = 4;
			this.numericRefresh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// numericTimeout
			// 
			this.numericTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.numericTimeout.Location = new System.Drawing.Point(109, 63);
			this.numericTimeout.Margin = new System.Windows.Forms.Padding(2);
			this.numericTimeout.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.numericTimeout.Name = "numericTimeout";
			this.numericTimeout.Size = new System.Drawing.Size(57, 20);
			this.numericTimeout.TabIndex = 5;
			this.numericTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// checkBoxScreen
			// 
			this.checkBoxScreen.AutoSize = true;
			this.checkBoxScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.checkBoxScreen.Location = new System.Drawing.Point(11, 100);
			this.checkBoxScreen.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxScreen.Name = "checkBoxScreen";
			this.checkBoxScreen.Size = new System.Drawing.Size(109, 17);
			this.checkBoxScreen.TabIndex = 6;
			this.checkBoxScreen.Text = "Take screen shot";
			this.checkBoxScreen.UseVisualStyleBackColor = true;
			// 
			// checkBoxOpenInNewTab
			// 
			this.checkBoxOpenInNewTab.AutoSize = true;
			this.checkBoxOpenInNewTab.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.checkBoxOpenInNewTab.Location = new System.Drawing.Point(11, 152);
			this.checkBoxOpenInNewTab.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxOpenInNewTab.Name = "checkBoxOpenInNewTab";
			this.checkBoxOpenInNewTab.Size = new System.Drawing.Size(104, 17);
			this.checkBoxOpenInNewTab.TabIndex = 7;
			this.checkBoxOpenInNewTab.Text = "Open in new tab";
			this.checkBoxOpenInNewTab.UseVisualStyleBackColor = true;
			// 
			// checkBoxDontWait
			// 
			this.checkBoxDontWait.AutoSize = true;
			this.checkBoxDontWait.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.checkBoxDontWait.Location = new System.Drawing.Point(11, 126);
			this.checkBoxDontWait.Margin = new System.Windows.Forms.Padding(2);
			this.checkBoxDontWait.Name = "checkBoxDontWait";
			this.checkBoxDontWait.Size = new System.Drawing.Size(73, 17);
			this.checkBoxDontWait.TabIndex = 8;
			this.checkBoxDontWait.Text = "Don\'t wait";
			this.checkBoxDontWait.UseVisualStyleBackColor = true;
			// 
			// LoadCollectionControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.checkBoxDontWait);
			this.Controls.Add(this.checkBoxOpenInNewTab);
			this.Controls.Add(this.checkBoxScreen);
			this.Controls.Add(this.numericTimeout);
			this.Controls.Add(this.numericRefresh);
			this.Controls.Add(this.lblTimeout);
			this.Controls.Add(this.txtID);
			this.Controls.Add(this.lblRefresh);
			this.Controls.Add(this.lblId);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.Name = "LoadCollectionControl";
			this.Size = new System.Drawing.Size(284, 179);
			((System.ComponentModel.ISupportInitialize)(this.numericRefresh)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblRefresh;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown numericRefresh;
        private System.Windows.Forms.NumericUpDown numericTimeout;
        private System.Windows.Forms.CheckBox checkBoxScreen;
        private System.Windows.Forms.CheckBox checkBoxOpenInNewTab;
        private System.Windows.Forms.CheckBox checkBoxDontWait;
    }
}
