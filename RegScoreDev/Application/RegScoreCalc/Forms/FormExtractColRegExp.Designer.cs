namespace RegScoreCalc.Forms
{
    partial class FormExtractColRegExp
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
            this.chbExtract = new System.Windows.Forms.CheckBox();
            this.numericOrder = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.rbFirstInstance = new System.Windows.Forms.RadioButton();
            this.groupBoxMatch = new System.Windows.Forms.GroupBox();
            this.rbMultipleValues = new System.Windows.Forms.RadioButton();
            this.numericNthInstnce = new System.Windows.Forms.NumericUpDown();
            this.rbNthInstance = new System.Windows.Forms.RadioButton();
            this.rbLastInstance = new System.Windows.Forms.RadioButton();
            this.chbAddToPrevious = new System.Windows.Forms.CheckBox();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.lbFormat = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboDocument = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericOrder)).BeginInit();
            this.groupBoxMatch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNthInstnce)).BeginInit();
            this.SuspendLayout();
            // 
            // chbExtract
            // 
            this.chbExtract.AutoSize = true;
            this.chbExtract.Location = new System.Drawing.Point(15, 12);
            this.chbExtract.Name = "chbExtract";
            this.chbExtract.Size = new System.Drawing.Size(59, 17);
            this.chbExtract.TabIndex = 0;
            this.chbExtract.Text = "Extract";
            this.chbExtract.UseVisualStyleBackColor = true;
            this.chbExtract.CheckedChanged += new System.EventHandler(this.chbExtract_CheckedChanged);
            // 
            // numericOrder
            // 
            this.numericOrder.Location = new System.Drawing.Point(73, 65);
            this.numericOrder.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericOrder.Name = "numericOrder";
            this.numericOrder.Size = new System.Drawing.Size(121, 20);
            this.numericOrder.TabIndex = 1;
            this.numericOrder.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Order: ";
            // 
            // rbFirstInstance
            // 
            this.rbFirstInstance.AutoSize = true;
            this.rbFirstInstance.Location = new System.Drawing.Point(15, 19);
            this.rbFirstInstance.Name = "rbFirstInstance";
            this.rbFirstInstance.Size = new System.Drawing.Size(87, 17);
            this.rbFirstInstance.TabIndex = 3;
            this.rbFirstInstance.TabStop = true;
            this.rbFirstInstance.Text = "First instance";
            this.rbFirstInstance.UseVisualStyleBackColor = true;
            // 
            // groupBoxMatch
            // 
            this.groupBoxMatch.Controls.Add(this.rbMultipleValues);
            this.groupBoxMatch.Controls.Add(this.numericNthInstnce);
            this.groupBoxMatch.Controls.Add(this.rbNthInstance);
            this.groupBoxMatch.Controls.Add(this.rbLastInstance);
            this.groupBoxMatch.Controls.Add(this.rbFirstInstance);
            this.groupBoxMatch.Location = new System.Drawing.Point(12, 91);
            this.groupBoxMatch.Name = "groupBoxMatch";
            this.groupBoxMatch.Size = new System.Drawing.Size(260, 122);
            this.groupBoxMatch.TabIndex = 4;
            this.groupBoxMatch.TabStop = false;
            this.groupBoxMatch.Text = "Match";
            // 
            // rbMultipleValues
            // 
            this.rbMultipleValues.AutoSize = true;
            this.rbMultipleValues.Location = new System.Drawing.Point(15, 88);
            this.rbMultipleValues.Name = "rbMultipleValues";
            this.rbMultipleValues.Size = new System.Drawing.Size(95, 17);
            this.rbMultipleValues.TabIndex = 7;
            this.rbMultipleValues.TabStop = true;
            this.rbMultipleValues.Text = "Multiple values";
            this.rbMultipleValues.UseVisualStyleBackColor = true;
            this.rbMultipleValues.Visible = false;
            // 
            // numericNthInstnce
            // 
            this.numericNthInstnce.Location = new System.Drawing.Point(109, 65);
            this.numericNthInstnce.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericNthInstnce.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericNthInstnce.Name = "numericNthInstnce";
            this.numericNthInstnce.Size = new System.Drawing.Size(57, 20);
            this.numericNthInstnce.TabIndex = 6;
            this.numericNthInstnce.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rbNthInstance
            // 
            this.rbNthInstance.AutoSize = true;
            this.rbNthInstance.Location = new System.Drawing.Point(15, 65);
            this.rbNthInstance.Name = "rbNthInstance";
            this.rbNthInstance.Size = new System.Drawing.Size(88, 17);
            this.rbNthInstance.TabIndex = 5;
            this.rbNthInstance.TabStop = true;
            this.rbNthInstance.Text = "N-th instance";
            this.rbNthInstance.UseVisualStyleBackColor = true;
            // 
            // rbLastInstance
            // 
            this.rbLastInstance.AutoSize = true;
            this.rbLastInstance.Location = new System.Drawing.Point(15, 42);
            this.rbLastInstance.Name = "rbLastInstance";
            this.rbLastInstance.Size = new System.Drawing.Size(88, 17);
            this.rbLastInstance.TabIndex = 4;
            this.rbLastInstance.TabStop = true;
            this.rbLastInstance.Text = "Last instance";
            this.rbLastInstance.UseVisualStyleBackColor = true;
            // 
            // chbAddToPrevious
            // 
            this.chbAddToPrevious.AutoSize = true;
            this.chbAddToPrevious.Location = new System.Drawing.Point(15, 217);
            this.chbAddToPrevious.Name = "chbAddToPrevious";
            this.chbAddToPrevious.Size = new System.Drawing.Size(100, 17);
            this.chbAddToPrevious.TabIndex = 5;
            this.chbAddToPrevious.Text = "Add to previous";
            this.chbAddToPrevious.UseVisualStyleBackColor = true;
            this.chbAddToPrevious.Visible = false;
            // 
            // txtFormat
            // 
            this.txtFormat.Location = new System.Drawing.Point(60, 241);
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.Size = new System.Drawing.Size(212, 20);
            this.txtFormat.TabIndex = 6;
            this.txtFormat.Text = "\"MM/dd/yy\",\"MM/dd/yyyy\",\"M/d/yyyy\",\"M/d/yy\"";
            this.txtFormat.Visible = false;
            // 
            // lbFormat
            // 
            this.lbFormat.AutoSize = true;
            this.lbFormat.Location = new System.Drawing.Point(12, 244);
            this.lbFormat.Name = "lbFormat";
            this.lbFormat.Size = new System.Drawing.Size(42, 13);
            this.lbFormat.TabIndex = 7;
            this.lbFormat.Text = "Format:";
            this.lbFormat.Visible = false;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOk.Location = new System.Drawing.Point(27, 276);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(163, 276);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Document: ";
            // 
            // comboDocument
            // 
            this.comboDocument.FormattingEnabled = true;
            this.comboDocument.Location = new System.Drawing.Point(73, 35);
            this.comboDocument.Name = "comboDocument";
            this.comboDocument.Size = new System.Drawing.Size(121, 21);
            this.comboDocument.TabIndex = 11;
            // 
            // FormExtractColRegExp
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(284, 311);
            this.Controls.Add(this.comboDocument);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lbFormat);
            this.Controls.Add(this.txtFormat);
            this.Controls.Add(this.chbAddToPrevious);
            this.Controls.Add(this.groupBoxMatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericOrder);
            this.Controls.Add(this.chbExtract);
            this.MaximumSize = new System.Drawing.Size(300, 350);
            this.MinimumSize = new System.Drawing.Size(300, 350);
            this.Name = "FormExtractColRegExp";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Extract Options";
            ((System.ComponentModel.ISupportInitialize)(this.numericOrder)).EndInit();
            this.groupBoxMatch.ResumeLayout(false);
            this.groupBoxMatch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericNthInstnce)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chbExtract;
        private System.Windows.Forms.NumericUpDown numericOrder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbFirstInstance;
        private System.Windows.Forms.GroupBox groupBoxMatch;
        private System.Windows.Forms.NumericUpDown numericNthInstnce;
        private System.Windows.Forms.RadioButton rbNthInstance;
        private System.Windows.Forms.RadioButton rbLastInstance;
        private System.Windows.Forms.RadioButton rbMultipleValues;
        private System.Windows.Forms.CheckBox chbAddToPrevious;
        private System.Windows.Forms.TextBox txtFormat;
        private System.Windows.Forms.Label lbFormat;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboDocument;
    }
}