namespace TotalSafety.DataTierGenerator {
    partial class MiscSettings {
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
            this.components = new System.ComponentModel.Container();
            this.m_GuiDataLayerNamespaceTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_GuiDataLayerOutputDirectoryButton = new System.Windows.Forms.Button();
            this.m_GuiDataLayerOutputDirectory = new System.Windows.Forms.TextBox();
            this.m_GuiOkButton = new System.Windows.Forms.Button();
            this.m_GuiCancelButton = new System.Windows.Forms.Button();
            this.m_GuiConnectionStringDialogButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_GuiConnectionString = new System.Windows.Forms.TextBox();
            this.miscSettingsModelBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.m_GuiDbProviderTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.miscSettingsModelBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // m_GuiDataLayerNamespaceTextBox
            // 
            this.m_GuiDataLayerNamespaceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiDataLayerNamespaceTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.miscSettingsModelBindingSource, "Namespace", true));
            this.m_GuiDataLayerNamespaceTextBox.Location = new System.Drawing.Point(9, 115);
            this.m_GuiDataLayerNamespaceTextBox.Name = "m_GuiDataLayerNamespaceTextBox";
            this.m_GuiDataLayerNamespaceTextBox.Size = new System.Drawing.Size(431, 20);
            this.m_GuiDataLayerNamespaceTextBox.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 96);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Data Layer Namespace:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 141);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Data Layer Project Path:";
            // 
            // m_GuiDataLayerOutputDirectoryButton
            // 
            this.m_GuiDataLayerOutputDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiDataLayerOutputDirectoryButton.Location = new System.Drawing.Point(419, 157);
            this.m_GuiDataLayerOutputDirectoryButton.Name = "m_GuiDataLayerOutputDirectoryButton";
            this.m_GuiDataLayerOutputDirectoryButton.Size = new System.Drawing.Size(24, 24);
            this.m_GuiDataLayerOutputDirectoryButton.TabIndex = 22;
            this.m_GuiDataLayerOutputDirectoryButton.TabStop = false;
            this.m_GuiDataLayerOutputDirectoryButton.Text = "...";
            this.m_GuiDataLayerOutputDirectoryButton.Click += new System.EventHandler(this.m_GuiDataLayerOutputDirectoryButton_Click);
            // 
            // m_GuiDataLayerOutputDirectory
            // 
            this.m_GuiDataLayerOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiDataLayerOutputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.miscSettingsModelBindingSource, "OutputPath", true));
            this.m_GuiDataLayerOutputDirectory.Location = new System.Drawing.Point(9, 160);
            this.m_GuiDataLayerOutputDirectory.Name = "m_GuiDataLayerOutputDirectory";
            this.m_GuiDataLayerOutputDirectory.Size = new System.Drawing.Size(404, 20);
            this.m_GuiDataLayerOutputDirectory.TabIndex = 21;
            // 
            // m_GuiOkButton
            // 
            this.m_GuiOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiOkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_GuiOkButton.Location = new System.Drawing.Point(284, 187);
            this.m_GuiOkButton.Name = "m_GuiOkButton";
            this.m_GuiOkButton.Size = new System.Drawing.Size(75, 23);
            this.m_GuiOkButton.TabIndex = 23;
            this.m_GuiOkButton.Text = "&OK";
            this.m_GuiOkButton.UseVisualStyleBackColor = true;
            this.m_GuiOkButton.Click += new System.EventHandler(this.m_GuiOkButton_Click);
            // 
            // m_GuiCancelButton
            // 
            this.m_GuiCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_GuiCancelButton.Location = new System.Drawing.Point(365, 187);
            this.m_GuiCancelButton.Name = "m_GuiCancelButton";
            this.m_GuiCancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_GuiCancelButton.TabIndex = 24;
            this.m_GuiCancelButton.Text = "&Cancel";
            this.m_GuiCancelButton.UseVisualStyleBackColor = true;
            this.m_GuiCancelButton.Click += new System.EventHandler(this.m_GuiCancelButton_Click);
            // 
            // m_GuiConnectionStringDialogButton
            // 
            this.m_GuiConnectionStringDialogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiConnectionStringDialogButton.Location = new System.Drawing.Point(419, 67);
            this.m_GuiConnectionStringDialogButton.Name = "m_GuiConnectionStringDialogButton";
            this.m_GuiConnectionStringDialogButton.Size = new System.Drawing.Size(24, 24);
            this.m_GuiConnectionStringDialogButton.TabIndex = 27;
            this.m_GuiConnectionStringDialogButton.TabStop = false;
            this.m_GuiConnectionStringDialogButton.Text = "...";
            this.m_GuiConnectionStringDialogButton.Click += new System.EventHandler(this.m_GuiConnectionStringDialogButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Connection String:";
            // 
            // m_GuiConnectionString
            // 
            this.m_GuiConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiConnectionString.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.miscSettingsModelBindingSource, "GeneratedDataProjectPath", true));
            this.m_GuiConnectionString.Location = new System.Drawing.Point(12, 70);
            this.m_GuiConnectionString.Name = "m_GuiConnectionString";
            this.m_GuiConnectionString.ReadOnly = true;
            this.m_GuiConnectionString.Size = new System.Drawing.Size(401, 20);
            this.m_GuiConnectionString.TabIndex = 26;
            // 
            // miscSettingsModelBindingSource
            // 
            this.miscSettingsModelBindingSource.DataSource = typeof(TotalSafety.DataTierGenerator.MVP.MiscSettingsModel);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 28;
            this.label4.Text = "Provider:";
            // 
            // m_GuiDbProviderTextBox
            // 
            this.m_GuiDbProviderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiDbProviderTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.miscSettingsModelBindingSource, "DbConnectionType", true));
            this.m_GuiDbProviderTextBox.Location = new System.Drawing.Point(12, 25);
            this.m_GuiDbProviderTextBox.Name = "m_GuiDbProviderTextBox";
            this.m_GuiDbProviderTextBox.ReadOnly = true;
            this.m_GuiDbProviderTextBox.Size = new System.Drawing.Size(428, 20);
            this.m_GuiDbProviderTextBox.TabIndex = 29;
            // 
            // MiscSettings
            // 
            this.AcceptButton = this.m_GuiOkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_GuiCancelButton;
            this.ClientSize = new System.Drawing.Size(455, 218);
            this.Controls.Add(this.m_GuiDbProviderTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_GuiConnectionStringDialogButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_GuiConnectionString);
            this.Controls.Add(this.m_GuiCancelButton);
            this.Controls.Add(this.m_GuiOkButton);
            this.Controls.Add(this.m_GuiDataLayerNamespaceTextBox);
            this.Controls.Add(this.m_GuiDataLayerOutputDirectoryButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_GuiDataLayerOutputDirectory);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MiscSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Misc Settings";
            ((System.ComponentModel.ISupportInitialize)(this.miscSettingsModelBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_GuiDataLayerNamespaceTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_GuiDataLayerOutputDirectoryButton;
        private System.Windows.Forms.TextBox m_GuiDataLayerOutputDirectory;
        private System.Windows.Forms.Button m_GuiOkButton;
        private System.Windows.Forms.Button m_GuiCancelButton;
        private System.Windows.Forms.BindingSource miscSettingsModelBindingSource;
        private System.Windows.Forms.Button m_GuiConnectionStringDialogButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_GuiConnectionString;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_GuiDbProviderTextBox;
    }
}