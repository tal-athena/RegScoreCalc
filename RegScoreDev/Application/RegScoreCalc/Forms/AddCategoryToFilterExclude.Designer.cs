namespace RegScoreCalc.Forms
{
    partial class AddCategoryToFilterExclude
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
			this.btnCancel = new RegScoreCalc.RibbonStyleButton();
			this.btnSave = new RegScoreCalc.RibbonStyleButton();
			this.cmbCategories = new System.Windows.Forms.ComboBox();
			this.radioBtnExclude = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.radioBtnDiscordant = new System.Windows.Forms.RadioButton();
			this.radioBtnConcordant = new System.Windows.Forms.RadioButton();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.HoverImage = null;
			this.btnCancel.Location = new System.Drawing.Point(165, 118);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.NormalImage = null;
			this.btnCancel.PressedImage = null;
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnSave
			// 
			this.btnSave.HoverImage = null;
			this.btnSave.Location = new System.Drawing.Point(31, 118);
			this.btnSave.Name = "btnSave";
			this.btnSave.NormalImage = null;
			this.btnSave.PressedImage = null;
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 12;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// cmbCategories
			// 
			this.cmbCategories.FormattingEnabled = true;
			this.cmbCategories.Location = new System.Drawing.Point(13, 12);
			this.cmbCategories.Name = "cmbCategories";
			this.cmbCategories.Size = new System.Drawing.Size(259, 21);
			this.cmbCategories.TabIndex = 11;
			// 
			// radioBtnExclude
			// 
			this.radioBtnExclude.AutoSize = true;
			this.radioBtnExclude.Checked = true;
			this.radioBtnExclude.Location = new System.Drawing.Point(3, 3);
			this.radioBtnExclude.Name = "radioBtnExclude";
			this.radioBtnExclude.Size = new System.Drawing.Size(63, 17);
			this.radioBtnExclude.TabIndex = 14;
			this.radioBtnExclude.TabStop = true;
			this.radioBtnExclude.Text = "Exclude";
			this.radioBtnExclude.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.radioBtnDiscordant);
			this.panel1.Controls.Add(this.radioBtnConcordant);
			this.panel1.Controls.Add(this.radioBtnExclude);
			this.panel1.Location = new System.Drawing.Point(13, 39);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(200, 73);
			this.panel1.TabIndex = 15;
			// 
			// radioBtnDiscordant
			// 
			this.radioBtnDiscordant.AutoSize = true;
			this.radioBtnDiscordant.Location = new System.Drawing.Point(3, 49);
			this.radioBtnDiscordant.Name = "radioBtnDiscordant";
			this.radioBtnDiscordant.Size = new System.Drawing.Size(76, 17);
			this.radioBtnDiscordant.TabIndex = 16;
			this.radioBtnDiscordant.TabStop = true;
			this.radioBtnDiscordant.Text = "Discordant";
			this.radioBtnDiscordant.UseVisualStyleBackColor = true;
			// 
			// radioBtnConcordant
			// 
			this.radioBtnConcordant.AutoSize = true;
			this.radioBtnConcordant.Location = new System.Drawing.Point(3, 26);
			this.radioBtnConcordant.Name = "radioBtnConcordant";
			this.radioBtnConcordant.Size = new System.Drawing.Size(80, 17);
			this.radioBtnConcordant.TabIndex = 15;
			this.radioBtnConcordant.TabStop = true;
			this.radioBtnConcordant.Text = "Concordant";
			this.radioBtnConcordant.UseVisualStyleBackColor = true;
			// 
			// AddCategoryToFilterExclude
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 149);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.cmbCategories);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddCategoryToFilterExclude";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add Category To Filter Exclude";
			this.Load += new System.EventHandler(this.AddCategoryToFilterExclude_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private RibbonStyleButton btnCancel;
        private RibbonStyleButton btnSave;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.RadioButton radioBtnExclude;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioBtnDiscordant;
        private System.Windows.Forms.RadioButton radioBtnConcordant;
    }
}