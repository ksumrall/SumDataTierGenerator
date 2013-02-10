namespace SumDataTierGenerator {
    partial class DatabaseConnectionSettings {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.m_GuiOkButton = new System.Windows.Forms.Button ();
            this.m_GuiCancelButton = new System.Windows.Forms.Button ();
            this.m_GuiLogin = new SumDataTierGenerator.Controls.SqlServerLogin ();
            this.SuspendLayout ();
            // 
            // m_GuiOkButton
            // 
            this.m_GuiOkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_GuiOkButton.Location = new System.Drawing.Point (103, 239);
            this.m_GuiOkButton.Name = "m_GuiOkButton";
            this.m_GuiOkButton.Size = new System.Drawing.Size (75, 23);
            this.m_GuiOkButton.TabIndex = 12;
            this.m_GuiOkButton.Text = "&OK";
            this.m_GuiOkButton.UseVisualStyleBackColor = true;
            this.m_GuiOkButton.Click += new System.EventHandler (this.m_GuiOkButton_Click);
            // 
            // m_GuiCancelButton
            // 
            this.m_GuiCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_GuiCancelButton.Location = new System.Drawing.Point (184, 239);
            this.m_GuiCancelButton.Name = "m_GuiCancelButton";
            this.m_GuiCancelButton.Size = new System.Drawing.Size (75, 23);
            this.m_GuiCancelButton.TabIndex = 13;
            this.m_GuiCancelButton.Text = "&Cancel";
            this.m_GuiCancelButton.UseVisualStyleBackColor = true;
            this.m_GuiCancelButton.Click += new System.EventHandler (this.m_GuiCancelButton_Click);
            // 
            // m_GuiLogin
            // 
            this.m_GuiLogin.Location = new System.Drawing.Point (8, 4);
            this.m_GuiLogin.Name = "m_GuiLogin";
            this.m_GuiLogin.Size = new System.Drawing.Size (251, 229);
            this.m_GuiLogin.TabIndex = 14;
            // 
            // DatabaseConnectionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size (268, 274);
            this.Controls.Add (this.m_GuiLogin);
            this.Controls.Add (this.m_GuiCancelButton);
            this.Controls.Add (this.m_GuiOkButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseConnectionSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Database Connection";
            this.ResumeLayout (false);

        }

        #endregion

        private System.Windows.Forms.Button m_GuiOkButton;
        private System.Windows.Forms.Button m_GuiCancelButton;
        private SumDataTierGenerator.Controls.SqlServerLogin m_GuiLogin;
    }
}