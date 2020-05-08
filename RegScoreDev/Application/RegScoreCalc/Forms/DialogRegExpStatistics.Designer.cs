namespace RegScoreCalc.Forms
{
    partial class DialogRegExpStatistics
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvRegExp = new System.Windows.Forms.ListView();
            this.columnRegExp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnReplace = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnReplaceText = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCodesEdit = new System.Windows.Forms.Button();
            this.btnCodesDelete = new System.Windows.Forms.Button();
            this.btnCodesAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(123, 245);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(240, 245);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lvRegExp
            // 
            this.lvRegExp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnRegExp,
            this.columnReplace,
            this.columnReplaceText});
            this.lvRegExp.FullRowSelect = true;
            this.lvRegExp.Location = new System.Drawing.Point(13, 13);
            this.lvRegExp.MultiSelect = false;
            this.lvRegExp.Name = "lvRegExp";
            this.lvRegExp.Size = new System.Drawing.Size(302, 226);
            this.lvRegExp.TabIndex = 3;
            this.lvRegExp.UseCompatibleStateImageBehavior = false;
            this.lvRegExp.View = System.Windows.Forms.View.Details;
            this.lvRegExp.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvRegExp_MouseDoubleClick);
            // 
            // columnRegExp
            // 
            this.columnRegExp.Text = "Regexp";
            this.columnRegExp.Width = 100;
            // 
            // columnReplace
            // 
            this.columnReplace.Text = "Replace";
            // 
            // columnReplaceText
            // 
            this.columnReplaceText.Text = "Replace text";
            this.columnReplaceText.Width = 100;
            // 
            // btnCodesEdit
            // 
            this.btnCodesEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCodesEdit.Image = global::RegScoreCalc.Properties.Resources.update_icon;
            this.btnCodesEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCodesEdit.Location = new System.Drawing.Point(321, 84);
            this.btnCodesEdit.Name = "btnCodesEdit";
            this.btnCodesEdit.Size = new System.Drawing.Size(92, 34);
            this.btnCodesEdit.TabIndex = 8;
            this.btnCodesEdit.Text = "Edit";
            this.btnCodesEdit.UseVisualStyleBackColor = true;
            this.btnCodesEdit.Click += new System.EventHandler(this.btnCodesEdit_Click);
            // 
            // btnCodesDelete
            // 
            this.btnCodesDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCodesDelete.Image = global::RegScoreCalc.Properties.Resources.delete_remove_icon;
            this.btnCodesDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCodesDelete.Location = new System.Drawing.Point(321, 164);
            this.btnCodesDelete.Name = "btnCodesDelete";
            this.btnCodesDelete.Size = new System.Drawing.Size(92, 34);
            this.btnCodesDelete.TabIndex = 7;
            this.btnCodesDelete.Text = "Delete";
            this.btnCodesDelete.UseVisualStyleBackColor = true;
            this.btnCodesDelete.Click += new System.EventHandler(this.btnCodesDelete_Click);
            // 
            // btnCodesAdd
            // 
            this.btnCodesAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCodesAdd.Image = global::RegScoreCalc.Properties.Resources.add_icon;
            this.btnCodesAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCodesAdd.Location = new System.Drawing.Point(321, 124);
            this.btnCodesAdd.Name = "btnCodesAdd";
            this.btnCodesAdd.Size = new System.Drawing.Size(92, 34);
            this.btnCodesAdd.TabIndex = 6;
            this.btnCodesAdd.Text = "Add";
            this.btnCodesAdd.UseVisualStyleBackColor = true;
            this.btnCodesAdd.Click += new System.EventHandler(this.btnCodesAdd_Click);
            // 
            // DialogRegExpStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 280);
            this.Controls.Add(this.btnCodesEdit);
            this.Controls.Add(this.btnCodesDelete);
            this.Controls.Add(this.btnCodesAdd);
            this.Controls.Add(this.lvRegExp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(441, 319);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(441, 319);
            this.Name = "DialogRegExpStatistics";
            this.Text = "DialogRegExpStatistics";
            this.Load += new System.EventHandler(this.DialogRegExpStatistics_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvRegExp;
        private System.Windows.Forms.ColumnHeader columnRegExp;
        private System.Windows.Forms.ColumnHeader columnReplace;
        private System.Windows.Forms.ColumnHeader columnReplaceText;
        private System.Windows.Forms.Button btnCodesEdit;
        private System.Windows.Forms.Button btnCodesDelete;
        private System.Windows.Forms.Button btnCodesAdd;
    }
}