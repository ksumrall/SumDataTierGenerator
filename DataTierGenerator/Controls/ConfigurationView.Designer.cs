namespace SumDataTierGenerator.Controls
{
    partial class ConfigurationView
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_GuiConnectionDetailsButton = new System.Windows.Forms.Button();
            this.m_GuiConnectionStringTextBox = new System.Windows.Forms.TextBox();
            this.m_GuiDbProviderTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_GuiOutputPathButton = new System.Windows.Forms.Button();
            this.m_GuiOutputPathTextBox = new System.Windows.Forms.TextBox();
            this.m_GuiNamespaceTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_GuiCancelButton = new System.Windows.Forms.Button();
            this.m_GuiOkButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.m_GuiConnectionDetailsButton);
            this.groupBox1.Controls.Add(this.m_GuiConnectionStringTextBox);
            this.groupBox1.Controls.Add(this.m_GuiDbProviderTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Connection Properties";
            // 
            // m_GuiConnectionDetailsButton
            // 
            this.m_GuiConnectionDetailsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiConnectionDetailsButton.Location = new System.Drawing.Point(253, 36);
            this.m_GuiConnectionDetailsButton.Name = "m_GuiConnectionDetailsButton";
            this.m_GuiConnectionDetailsButton.Size = new System.Drawing.Size(75, 60);
            this.m_GuiConnectionDetailsButton.TabIndex = 4;
            this.m_GuiConnectionDetailsButton.Text = "...";
            this.m_GuiConnectionDetailsButton.UseVisualStyleBackColor = true;
            this.m_GuiConnectionDetailsButton.Click += new System.EventHandler(this.m_GuiConnectionDetailsButton_Click);
            // 
            // m_GuiConnectionStringTextBox
            // 
            this.m_GuiConnectionStringTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiConnectionStringTextBox.Location = new System.Drawing.Point(6, 75);
            this.m_GuiConnectionStringTextBox.Name = "m_GuiConnectionStringTextBox";
            this.m_GuiConnectionStringTextBox.ReadOnly = true;
            this.m_GuiConnectionStringTextBox.Size = new System.Drawing.Size(241, 20);
            this.m_GuiConnectionStringTextBox.TabIndex = 3;
            // 
            // m_GuiDbProviderTextBox
            // 
            this.m_GuiDbProviderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiDbProviderTextBox.Location = new System.Drawing.Point(6, 36);
            this.m_GuiDbProviderTextBox.Name = "m_GuiDbProviderTextBox";
            this.m_GuiDbProviderTextBox.ReadOnly = true;
            this.m_GuiDbProviderTextBox.Size = new System.Drawing.Size(241, 20);
            this.m_GuiDbProviderTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connection String:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Provider:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.m_GuiOutputPathButton);
            this.groupBox2.Controls.Add(this.m_GuiOutputPathTextBox);
            this.groupBox2.Controls.Add(this.m_GuiNamespaceTextBox);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(3, 115);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(334, 103);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Code Generation Properties";
            // 
            // m_GuiOutputPathButton
            // 
            this.m_GuiOutputPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiOutputPathButton.Location = new System.Drawing.Point(305, 69);
            this.m_GuiOutputPathButton.Name = "m_GuiOutputPathButton";
            this.m_GuiOutputPathButton.Size = new System.Drawing.Size(23, 23);
            this.m_GuiOutputPathButton.TabIndex = 5;
            this.m_GuiOutputPathButton.Text = "...";
            this.m_GuiOutputPathButton.UseVisualStyleBackColor = true;
            this.m_GuiOutputPathButton.Click += new System.EventHandler(this.m_GuiOutputPathButton_Click);
            // 
            // m_GuiOutputPathTextBox
            // 
            this.m_GuiOutputPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiOutputPathTextBox.Location = new System.Drawing.Point(6, 71);
            this.m_GuiOutputPathTextBox.Name = "m_GuiOutputPathTextBox";
            this.m_GuiOutputPathTextBox.Size = new System.Drawing.Size(293, 20);
            this.m_GuiOutputPathTextBox.TabIndex = 3;
            // 
            // m_GuiNamespaceTextBox
            // 
            this.m_GuiNamespaceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiNamespaceTextBox.Location = new System.Drawing.Point(6, 32);
            this.m_GuiNamespaceTextBox.Name = "m_GuiNamespaceTextBox";
            this.m_GuiNamespaceTextBox.Size = new System.Drawing.Size(322, 20);
            this.m_GuiNamespaceTextBox.TabIndex = 2;
            this.m_GuiNamespaceTextBox.TextChanged += new System.EventHandler(this.m_GuiNamespaceTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Output Path:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Namespace:";
            // 
            // m_GuiCancelButton
            // 
            this.m_GuiCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_GuiCancelButton.Location = new System.Drawing.Point(256, 224);
            this.m_GuiCancelButton.Name = "m_GuiCancelButton";
            this.m_GuiCancelButton.Size = new System.Drawing.Size(75, 23);
            this.m_GuiCancelButton.TabIndex = 2;
            this.m_GuiCancelButton.Text = "&Close";
            this.m_GuiCancelButton.UseVisualStyleBackColor = true;
            this.m_GuiCancelButton.Click += new System.EventHandler(this.m_GuiCancelButton_Click);
            // 
            // m_GuiOkButton
            // 
            this.m_GuiOkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_GuiOkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_GuiOkButton.Enabled = false;
            this.m_GuiOkButton.Location = new System.Drawing.Point(175, 224);
            this.m_GuiOkButton.Name = "m_GuiOkButton";
            this.m_GuiOkButton.Size = new System.Drawing.Size(75, 23);
            this.m_GuiOkButton.TabIndex = 3;
            this.m_GuiOkButton.Text = "&OK";
            this.m_GuiOkButton.UseVisualStyleBackColor = true;
            this.m_GuiOkButton.Click += new System.EventHandler(this.m_GuiOkButton_Click);
            // 
            // ConfigurationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_GuiOkButton);
            this.Controls.Add(this.m_GuiCancelButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ConfigurationView";
            this.Size = new System.Drawing.Size(340, 258);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button m_GuiConnectionDetailsButton;
        private System.Windows.Forms.TextBox m_GuiConnectionStringTextBox;
        private System.Windows.Forms.TextBox m_GuiDbProviderTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button m_GuiOutputPathButton;
        private System.Windows.Forms.TextBox m_GuiOutputPathTextBox;
        private System.Windows.Forms.TextBox m_GuiNamespaceTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button m_GuiCancelButton;
        private System.Windows.Forms.Button m_GuiOkButton;
    }
}
