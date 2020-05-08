namespace WebAppTest.Action_Controls
{
    partial class EditCollectionRecordControl
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
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFieldValue = new System.Windows.Forms.TextBox();
			this.txtFieldID = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtRecordID = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(9, 80);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(62, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Field Value:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(9, 46);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(46, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Field ID:";
			// 
			// txtFieldValue
			// 
			this.txtFieldValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFieldValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtFieldValue.Location = new System.Drawing.Point(79, 77);
			this.txtFieldValue.Margin = new System.Windows.Forms.Padding(4);
			this.txtFieldValue.Name = "txtFieldValue";
			this.txtFieldValue.Size = new System.Drawing.Size(324, 20);
			this.txtFieldValue.TabIndex = 5;
			// 
			// txtFieldID
			// 
			this.txtFieldID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFieldID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtFieldID.Location = new System.Drawing.Point(79, 44);
			this.txtFieldID.Margin = new System.Windows.Forms.Padding(4);
			this.txtFieldID.Name = "txtFieldID";
			this.txtFieldID.Size = new System.Drawing.Size(324, 20);
			this.txtFieldID.TabIndex = 4;
			this.txtFieldID.TextChanged += new System.EventHandler(this.txtFieldID_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(9, 13);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Record ID:";
			this.label3.Click += new System.EventHandler(this.label3_Click);
			// 
			// txtRecordID
			// 
			this.txtRecordID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRecordID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.txtRecordID.Location = new System.Drawing.Point(79, 11);
			this.txtRecordID.Margin = new System.Windows.Forms.Padding(4);
			this.txtRecordID.Name = "txtRecordID";
			this.txtRecordID.Size = new System.Drawing.Size(324, 20);
			this.txtRecordID.TabIndex = 8;
			// 
			// EditCollectionRecordControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtRecordID);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFieldValue);
			this.Controls.Add(this.txtFieldID);
			this.Name = "EditCollectionRecordControl";
			this.Size = new System.Drawing.Size(414, 229);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFieldValue;
        private System.Windows.Forms.TextBox txtFieldID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRecordID;
    }
}
