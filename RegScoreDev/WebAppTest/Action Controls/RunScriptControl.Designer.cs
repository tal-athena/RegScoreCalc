namespace WebAppTest.Action_Controls
{
    partial class RunScriptControl
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
            this.txtScript = new System.Windows.Forms.RichTextBox();
            this.btnSaveScript = new System.Windows.Forms.Button();
            this.btnSaveScriptAs = new System.Windows.Forms.Button();
            this.btnLoadScript = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCompile = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtScript
            // 
            this.txtScript.AcceptsTab = true;
            this.txtScript.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScript.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtScript.Location = new System.Drawing.Point(0, 0);
            this.txtScript.Name = "txtScript";
            this.txtScript.Size = new System.Drawing.Size(341, 223);
            this.txtScript.TabIndex = 0;
            this.txtScript.Text = "";
            this.txtScript.TextChanged += new System.EventHandler(this.txtScript_TextChanged);
            // 
            // btnSaveScript
            // 
            this.btnSaveScript.Location = new System.Drawing.Point(85, 3);
            this.btnSaveScript.Name = "btnSaveScript";
            this.btnSaveScript.Size = new System.Drawing.Size(75, 23);
            this.btnSaveScript.TabIndex = 1;
            this.btnSaveScript.Text = "Save";
            this.btnSaveScript.UseVisualStyleBackColor = true;
            this.btnSaveScript.Click += new System.EventHandler(this.btnSaveScript_Click);
            // 
            // btnSaveScriptAs
            // 
            this.btnSaveScriptAs.Location = new System.Drawing.Point(166, 3);
            this.btnSaveScriptAs.Name = "btnSaveScriptAs";
            this.btnSaveScriptAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveScriptAs.TabIndex = 2;
            this.btnSaveScriptAs.Text = "Save As";
            this.btnSaveScriptAs.UseVisualStyleBackColor = true;
            this.btnSaveScriptAs.Click += new System.EventHandler(this.btnSaveScriptAs_Click);
            // 
            // btnLoadScript
            // 
            this.btnLoadScript.Location = new System.Drawing.Point(4, 3);
            this.btnLoadScript.Name = "btnLoadScript";
            this.btnLoadScript.Size = new System.Drawing.Size(75, 23);
            this.btnLoadScript.TabIndex = 0;
            this.btnLoadScript.Text = "Load";
            this.btnLoadScript.UseVisualStyleBackColor = true;
            this.btnLoadScript.Click += new System.EventHandler(this.btnLoadScript_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtScript);
            this.panel1.Location = new System.Drawing.Point(3, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(343, 225);
            this.panel1.TabIndex = 2;
            // 
            // btnCompile
            // 
            this.btnCompile.Location = new System.Drawing.Point(247, 4);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(75, 23);
            this.btnCompile.TabIndex = 3;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.btnCompile_Click);
            // 
            // RunScriptControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLoadScript);
            this.Controls.Add(this.btnSaveScriptAs);
            this.Controls.Add(this.btnSaveScript);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "RunScriptControl";
            this.Size = new System.Drawing.Size(349, 260);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.RichTextBox txtScript;
		private System.Windows.Forms.Button btnSaveScript;
		private System.Windows.Forms.Button btnSaveScriptAs;
		private System.Windows.Forms.Button btnLoadScript;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnCompile;
	}
}
