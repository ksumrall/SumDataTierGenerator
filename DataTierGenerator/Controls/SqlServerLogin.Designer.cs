namespace TSHOU.DataTierGenerator.Controls {
    partial class SqlServerLogin {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.components = new System.ComponentModel.Container ();
            this.m_GuiServerNameTextbox = new System.Windows.Forms.TextBox ();
            this.sqlConnectionSettingsModelBindingSource = new System.Windows.Forms.BindingSource (this.components);
            this.m_GuiDatabaseCombo = new System.Windows.Forms.ComboBox ();
            this.databaseLabel = new System.Windows.Forms.Label ();
            this.serverLabel = new System.Windows.Forms.Label ();
            this.authenticationGroupBox = new System.Windows.Forms.GroupBox ();
            this.m_GuiPasswordTextBox = new System.Windows.Forms.TextBox ();
            this.m_GuiUserIdTextBox = new System.Windows.Forms.TextBox ();
            this.m_GuiPasswordLabel = new System.Windows.Forms.Label ();
            this.m_GuiUserIdLabel = new System.Windows.Forms.Label ();
            this.m_GuiSqlServerAuthenticationRadioButton = new System.Windows.Forms.RadioButton ();
            this.m_GuiWindowsAuthenticationRadioButton = new System.Windows.Forms.RadioButton ();
            ((System.ComponentModel.ISupportInitialize)(this.sqlConnectionSettingsModelBindingSource)).BeginInit ();
            this.authenticationGroupBox.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // m_GuiServerNameTextbox
            // 
            this.m_GuiServerNameTextbox.DataBindings.Add (new System.Windows.Forms.Binding ("Text", this.sqlConnectionSettingsModelBindingSource, "ServerName", true));
            this.m_GuiServerNameTextbox.Location = new System.Drawing.Point (6, 163);
            this.m_GuiServerNameTextbox.Name = "m_GuiServerNameTextbox";
            this.m_GuiServerNameTextbox.Size = new System.Drawing.Size (241, 20);
            this.m_GuiServerNameTextbox.TabIndex = 22;
            this.m_GuiServerNameTextbox.TextChanged += new System.EventHandler (this.ConnectionDetails_DataChanged);
            // 
            // sqlConnectionSettingsModelBindingSource
            // 
            this.sqlConnectionSettingsModelBindingSource.DataSource = typeof (TSHOU.DataTierGenerator.MVP.SqlConnectionSettingsModel);
            // 
            // m_GuiDatabaseCombo
            // 
            this.m_GuiDatabaseCombo.DataBindings.Add (new System.Windows.Forms.Binding ("SelectedValue", this.sqlConnectionSettingsModelBindingSource, "DatabaseName", true));
            this.m_GuiDatabaseCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_GuiDatabaseCombo.FormattingEnabled = true;
            this.m_GuiDatabaseCombo.Location = new System.Drawing.Point (6, 203);
            this.m_GuiDatabaseCombo.Name = "m_GuiDatabaseCombo";
            this.m_GuiDatabaseCombo.Size = new System.Drawing.Size (244, 21);
            this.m_GuiDatabaseCombo.TabIndex = 19;
            this.m_GuiDatabaseCombo.DropDown += new System.EventHandler (this.m_GuiDatabaseCombo_DropDown);
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Location = new System.Drawing.Point (3, 187);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size (56, 13);
            this.databaseLabel.TabIndex = 18;
            this.databaseLabel.Text = "Database:";
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point (3, 147);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size (72, 13);
            this.serverLabel.TabIndex = 16;
            this.serverLabel.Text = "Server Name:";
            // 
            // authenticationGroupBox
            // 
            this.authenticationGroupBox.Controls.Add (this.m_GuiPasswordTextBox);
            this.authenticationGroupBox.Controls.Add (this.m_GuiUserIdTextBox);
            this.authenticationGroupBox.Controls.Add (this.m_GuiPasswordLabel);
            this.authenticationGroupBox.Controls.Add (this.m_GuiUserIdLabel);
            this.authenticationGroupBox.Controls.Add (this.m_GuiSqlServerAuthenticationRadioButton);
            this.authenticationGroupBox.Controls.Add (this.m_GuiWindowsAuthenticationRadioButton);
            this.authenticationGroupBox.Location = new System.Drawing.Point (3, 3);
            this.authenticationGroupBox.Name = "authenticationGroupBox";
            this.authenticationGroupBox.Size = new System.Drawing.Size (244, 141);
            this.authenticationGroupBox.TabIndex = 17;
            this.authenticationGroupBox.TabStop = false;
            this.authenticationGroupBox.Text = "Authentication";
            // 
            // m_GuiPasswordTextBox
            // 
            this.m_GuiPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiPasswordTextBox.DataBindings.Add (new System.Windows.Forms.Binding ("Text", this.sqlConnectionSettingsModelBindingSource, "Password", true));
            this.m_GuiPasswordTextBox.Enabled = false;
            this.m_GuiPasswordTextBox.Location = new System.Drawing.Point (92, 108);
            this.m_GuiPasswordTextBox.Name = "m_GuiPasswordTextBox";
            this.m_GuiPasswordTextBox.PasswordChar = '*';
            this.m_GuiPasswordTextBox.Size = new System.Drawing.Size (140, 20);
            this.m_GuiPasswordTextBox.TabIndex = 9;
            this.m_GuiPasswordTextBox.TextChanged += new System.EventHandler (this.ConnectionDetails_DataChanged);
            // 
            // m_GuiUserIdTextBox
            // 
            this.m_GuiUserIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiUserIdTextBox.DataBindings.Add (new System.Windows.Forms.Binding ("Text", this.sqlConnectionSettingsModelBindingSource, "UserId", true));
            this.m_GuiUserIdTextBox.Enabled = false;
            this.m_GuiUserIdTextBox.Location = new System.Drawing.Point (92, 78);
            this.m_GuiUserIdTextBox.Name = "m_GuiUserIdTextBox";
            this.m_GuiUserIdTextBox.Size = new System.Drawing.Size (140, 20);
            this.m_GuiUserIdTextBox.TabIndex = 7;
            this.m_GuiUserIdTextBox.TextChanged += new System.EventHandler (this.ConnectionDetails_DataChanged);
            // 
            // m_GuiPasswordLabel
            // 
            this.m_GuiPasswordLabel.AutoSize = true;
            this.m_GuiPasswordLabel.Enabled = false;
            this.m_GuiPasswordLabel.Location = new System.Drawing.Point (19, 111);
            this.m_GuiPasswordLabel.Name = "m_GuiPasswordLabel";
            this.m_GuiPasswordLabel.Size = new System.Drawing.Size (56, 13);
            this.m_GuiPasswordLabel.TabIndex = 8;
            this.m_GuiPasswordLabel.Text = "Password:";
            // 
            // m_GuiUserIdLabel
            // 
            this.m_GuiUserIdLabel.AutoSize = true;
            this.m_GuiUserIdLabel.Enabled = false;
            this.m_GuiUserIdLabel.Location = new System.Drawing.Point (19, 81);
            this.m_GuiUserIdLabel.Name = "m_GuiUserIdLabel";
            this.m_GuiUserIdLabel.Size = new System.Drawing.Size (44, 13);
            this.m_GuiUserIdLabel.TabIndex = 6;
            this.m_GuiUserIdLabel.Text = "User Id:";
            // 
            // m_GuiSqlServerAuthenticationRadioButton
            // 
            this.m_GuiSqlServerAuthenticationRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_GuiSqlServerAuthenticationRadioButton.Location = new System.Drawing.Point (10, 48);
            this.m_GuiSqlServerAuthenticationRadioButton.Name = "m_GuiSqlServerAuthenticationRadioButton";
            this.m_GuiSqlServerAuthenticationRadioButton.Size = new System.Drawing.Size (156, 24);
            this.m_GuiSqlServerAuthenticationRadioButton.TabIndex = 5;
            this.m_GuiSqlServerAuthenticationRadioButton.Text = "SQL Server Authentication";
            this.m_GuiSqlServerAuthenticationRadioButton.CheckedChanged += new System.EventHandler (this.m_GuiSqlServerAuthenticationRadioButton_CheckedChanged);
            // 
            // m_GuiWindowsAuthenticationRadioButton
            // 
            this.m_GuiWindowsAuthenticationRadioButton.Checked = true;
            this.m_GuiWindowsAuthenticationRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.m_GuiWindowsAuthenticationRadioButton.Location = new System.Drawing.Point (10, 19);
            this.m_GuiWindowsAuthenticationRadioButton.Name = "m_GuiWindowsAuthenticationRadioButton";
            this.m_GuiWindowsAuthenticationRadioButton.Size = new System.Drawing.Size (156, 24);
            this.m_GuiWindowsAuthenticationRadioButton.TabIndex = 4;
            this.m_GuiWindowsAuthenticationRadioButton.TabStop = true;
            this.m_GuiWindowsAuthenticationRadioButton.Text = "Windows Authentication";
            this.m_GuiWindowsAuthenticationRadioButton.CheckedChanged += new System.EventHandler (this.m_GuiWindowsAuthenticationRadioButton_CheckedChanged);
            // 
            // SqlServerLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add (this.m_GuiServerNameTextbox);
            this.Controls.Add (this.m_GuiDatabaseCombo);
            this.Controls.Add (this.databaseLabel);
            this.Controls.Add (this.serverLabel);
            this.Controls.Add (this.authenticationGroupBox);
            this.Name = "SqlServerLogin";
            this.Size = new System.Drawing.Size (251, 229);
            ((System.ComponentModel.ISupportInitialize)(this.sqlConnectionSettingsModelBindingSource)).EndInit ();
            this.authenticationGroupBox.ResumeLayout (false);
            this.authenticationGroupBox.PerformLayout ();
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.TextBox m_GuiServerNameTextbox;
        private System.Windows.Forms.ComboBox m_GuiDatabaseCombo;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.GroupBox authenticationGroupBox;
        private System.Windows.Forms.TextBox m_GuiPasswordTextBox;
        private System.Windows.Forms.TextBox m_GuiUserIdTextBox;
        private System.Windows.Forms.Label m_GuiPasswordLabel;
        private System.Windows.Forms.Label m_GuiUserIdLabel;
        private System.Windows.Forms.RadioButton m_GuiSqlServerAuthenticationRadioButton;
        private System.Windows.Forms.RadioButton m_GuiWindowsAuthenticationRadioButton;
        private System.Windows.Forms.BindingSource sqlConnectionSettingsModelBindingSource;
    }
}
