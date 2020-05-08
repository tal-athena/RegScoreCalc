namespace RegScoreCalc.Forms
{
    partial class frmModifiedNotesPopUp
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
            this.gbChooseWriteTo = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNewDatabaseName = new System.Windows.Forms.TextBox();
            this.rbNewDatabase = new System.Windows.Forms.RadioButton();
            this.rbInitialModified = new System.Windows.Forms.RadioButton();
            this.rbNewModified = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.gbChooseWriteTo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbChooseWriteTo
            // 
            this.gbChooseWriteTo.Controls.Add(this.label1);
            this.gbChooseWriteTo.Controls.Add(this.txtNewDatabaseName);
            this.gbChooseWriteTo.Controls.Add(this.rbNewDatabase);
            this.gbChooseWriteTo.Controls.Add(this.rbInitialModified);
            this.gbChooseWriteTo.Controls.Add(this.rbNewModified);
            this.gbChooseWriteTo.Location = new System.Drawing.Point(12, 12);
            this.gbChooseWriteTo.Name = "gbChooseWriteTo";
            this.gbChooseWriteTo.Size = new System.Drawing.Size(393, 113);
            this.gbChooseWriteTo.TabIndex = 6;
            this.gbChooseWriteTo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "New database name:";
            // 
            // txtNewDatabaseName
            // 
            this.txtNewDatabaseName.Location = new System.Drawing.Point(137, 82);
            this.txtNewDatabaseName.Name = "txtNewDatabaseName";
            this.txtNewDatabaseName.Size = new System.Drawing.Size(157, 20);
            this.txtNewDatabaseName.TabIndex = 3;
            // 
            // rbNewDatabase
            // 
            this.rbNewDatabase.AutoSize = true;
            this.rbNewDatabase.Location = new System.Drawing.Point(7, 62);
            this.rbNewDatabase.Name = "rbNewDatabase";
            this.rbNewDatabase.Size = new System.Drawing.Size(260, 17);
            this.rbNewDatabase.TabIndex = 2;
            this.rbNewDatabase.Text = "Write modified notes to a new database file (mdb):";
            this.rbNewDatabase.UseVisualStyleBackColor = true;
            // 
            // rbInitialModified
            // 
            this.rbInitialModified.AutoSize = true;
            this.rbInitialModified.Location = new System.Drawing.Point(7, 36);
            this.rbInitialModified.Name = "rbInitialModified";
            this.rbInitialModified.Size = new System.Drawing.Size(262, 17);
            this.rbInitialModified.TabIndex = 1;
            this.rbInitialModified.Text = "Overwrite (or create) the initial ModifiedNotes table";
            this.rbInitialModified.UseVisualStyleBackColor = true;
            // 
            // rbNewModified
            // 
            this.rbNewModified.AutoSize = true;
            this.rbNewModified.Checked = true;
            this.rbNewModified.Location = new System.Drawing.Point(7, 11);
            this.rbNewModified.Name = "rbNewModified";
            this.rbNewModified.Size = new System.Drawing.Size(370, 17);
            this.rbNewModified.TabIndex = 0;
            this.rbNewModified.TabStop = true;
            this.rbNewModified.Text = "Write modified notes in new ModifiedNotes table (in the current database)";
            this.rbNewModified.UseVisualStyleBackColor = true;
            this.rbNewModified.CheckedChanged += new System.EventHandler(this.rbNewModified_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(329, 135);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(12, 135);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmModifiedNotesPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 167);
            this.Controls.Add(this.gbChooseWriteTo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(325, 146);
            this.Name = "frmModifiedNotesPopUp";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modified Notes";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmModifiedNotesPopUp_Load);
            this.gbChooseWriteTo.ResumeLayout(false);
            this.gbChooseWriteTo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbChooseWriteTo;
        private System.Windows.Forms.RadioButton rbInitialModified;
        private System.Windows.Forms.RadioButton rbNewModified;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNewDatabaseName;
        private System.Windows.Forms.RadioButton rbNewDatabase;
    }
}