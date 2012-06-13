namespace TotalSafety.DataTierGenerator {
    partial class MainForm {
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("No project to display.");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Please create new or open existing one.");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.m_GuiOpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.m_GuiSaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_GuiSaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.m_GuiExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_GuiProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_GuiConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.m_GuiRefreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_GuiGenerateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.m_GuiSplitContainer = new System.Windows.Forms.SplitContainer();
            this.m_GuiProjectTree = new System.Windows.Forms.TreeView();
            this.m_GuiDetailPaneTabCollection = new System.Windows.Forms.TabControl();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_GuiSplitContainer)).BeginInit();
            this.m_GuiSplitContainer.Panel1.SuspendLayout();
            this.m_GuiSplitContainer.Panel2.SuspendLayout();
            this.m_GuiSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.m_GuiProjectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(792, 24);
            this.menuStrip1.TabIndex = 29;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.m_GuiOpenToolStripMenuItem,
            this.toolStripMenuItem2,
            this.m_GuiSaveToolStripMenuItem,
            this.m_GuiSaveAsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.m_GuiExitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem1});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // projectToolStripMenuItem1
            // 
            this.projectToolStripMenuItem1.Name = "projectToolStripMenuItem1";
            this.projectToolStripMenuItem1.Size = new System.Drawing.Size(120, 22);
            this.projectToolStripMenuItem1.Text = "&Project...";
            // 
            // m_GuiOpenToolStripMenuItem
            // 
            this.m_GuiOpenToolStripMenuItem.Name = "m_GuiOpenToolStripMenuItem";
            this.m_GuiOpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.m_GuiOpenToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.m_GuiOpenToolStripMenuItem.Text = "&Open...";
            this.m_GuiOpenToolStripMenuItem.Click += new System.EventHandler(this.m_GuiOpenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 6);
            // 
            // m_GuiSaveToolStripMenuItem
            // 
            this.m_GuiSaveToolStripMenuItem.Enabled = false;
            this.m_GuiSaveToolStripMenuItem.Name = "m_GuiSaveToolStripMenuItem";
            this.m_GuiSaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.m_GuiSaveToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.m_GuiSaveToolStripMenuItem.Text = "&Save";
            this.m_GuiSaveToolStripMenuItem.Click += new System.EventHandler(this.m_GuiSaveToolStripMenuItem_Click);
            // 
            // m_GuiSaveAsToolStripMenuItem
            // 
            this.m_GuiSaveAsToolStripMenuItem.Name = "m_GuiSaveAsToolStripMenuItem";
            this.m_GuiSaveAsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.m_GuiSaveAsToolStripMenuItem.Text = "Save &As...";
            this.m_GuiSaveAsToolStripMenuItem.Click += new System.EventHandler(this.m_GuiSaveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 6);
            // 
            // m_GuiExitToolStripMenuItem
            // 
            this.m_GuiExitToolStripMenuItem.Name = "m_GuiExitToolStripMenuItem";
            this.m_GuiExitToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.m_GuiExitToolStripMenuItem.Text = "E&xit";
            this.m_GuiExitToolStripMenuItem.Click += new System.EventHandler(this.m_GuiExitToolStripMenuItem_Click);
            // 
            // m_GuiProjectToolStripMenuItem
            // 
            this.m_GuiProjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_GuiConfigurationToolStripMenuItem,
            this.toolStripMenuItem4,
            this.m_GuiRefreshToolStripMenuItem,
            this.m_GuiGenerateToolStripMenuItem});
            this.m_GuiProjectToolStripMenuItem.Name = "m_GuiProjectToolStripMenuItem";
            this.m_GuiProjectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.m_GuiProjectToolStripMenuItem.Text = "&Project";
            // 
            // m_GuiConfigurationToolStripMenuItem
            // 
            this.m_GuiConfigurationToolStripMenuItem.Name = "m_GuiConfigurationToolStripMenuItem";
            this.m_GuiConfigurationToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.m_GuiConfigurationToolStripMenuItem.Text = "&Configuration...";
            this.m_GuiConfigurationToolStripMenuItem.Click += new System.EventHandler(this.m_GuiConfigurationToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(178, 6);
            // 
            // m_GuiRefreshToolStripMenuItem
            // 
            this.m_GuiRefreshToolStripMenuItem.Name = "m_GuiRefreshToolStripMenuItem";
            this.m_GuiRefreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.m_GuiRefreshToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.m_GuiRefreshToolStripMenuItem.Text = "&Refresh...";
            this.m_GuiRefreshToolStripMenuItem.Click += new System.EventHandler(this.m_GuiRefreshToolStripMenuItem_Click);
            // 
            // m_GuiGenerateToolStripMenuItem
            // 
            this.m_GuiGenerateToolStripMenuItem.Name = "m_GuiGenerateToolStripMenuItem";
            this.m_GuiGenerateToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.m_GuiGenerateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.m_GuiGenerateToolStripMenuItem.Text = "&Generate...";
            this.m_GuiGenerateToolStripMenuItem.Click += new System.EventHandler(this.m_GuiGenerateToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // m_GuiSplitContainer
            // 
            this.m_GuiSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GuiSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.m_GuiSplitContainer.Location = new System.Drawing.Point(0, 24);
            this.m_GuiSplitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.m_GuiSplitContainer.Name = "m_GuiSplitContainer";
            // 
            // m_GuiSplitContainer.Panel1
            // 
            this.m_GuiSplitContainer.Panel1.Controls.Add(this.m_GuiProjectTree);
            // 
            // m_GuiSplitContainer.Panel2
            // 
            this.m_GuiSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.m_GuiSplitContainer.Panel2.Controls.Add(this.m_GuiDetailPaneTabCollection);
            this.m_GuiSplitContainer.Size = new System.Drawing.Size(792, 520);
            this.m_GuiSplitContainer.SplitterDistance = 263;
            this.m_GuiSplitContainer.TabIndex = 30;
            // 
            // m_GuiProjectTree
            // 
            this.m_GuiProjectTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m_GuiProjectTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GuiProjectTree.Location = new System.Drawing.Point(0, 0);
            this.m_GuiProjectTree.Name = "m_GuiProjectTree";
            treeNode1.Checked = true;
            treeNode1.Name = "Node0";
            treeNode1.Text = "No project to display.";
            treeNode2.Name = "Node1";
            treeNode2.Text = "Please create new or open existing one.";
            this.m_GuiProjectTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            this.m_GuiProjectTree.Size = new System.Drawing.Size(263, 520);
            this.m_GuiProjectTree.TabIndex = 0;
            this.m_GuiProjectTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_GuiProjectTree_AfterSelect);
            // 
            // m_GuiDetailPaneTabCollection
            // 
            this.m_GuiDetailPaneTabCollection.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.m_GuiDetailPaneTabCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GuiDetailPaneTabCollection.Location = new System.Drawing.Point(0, 0);
            this.m_GuiDetailPaneTabCollection.Name = "m_GuiDetailPaneTabCollection";
            this.m_GuiDetailPaneTabCollection.Padding = new System.Drawing.Point(0, 0);
            this.m_GuiDetailPaneTabCollection.SelectedIndex = 0;
            this.m_GuiDetailPaneTabCollection.Size = new System.Drawing.Size(525, 520);
            this.m_GuiDetailPaneTabCollection.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 31;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.m_GuiSplitContainer);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Tier Generator - Enterprise Library Edition";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.m_GuiSplitContainer.Panel1.ResumeLayout(false);
            this.m_GuiSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_GuiSplitContainer)).EndInit();
            this.m_GuiSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_GuiSaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_GuiSaveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem m_GuiExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m_GuiOpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem m_GuiProjectToolStripMenuItem;
        private System.Windows.Forms.SplitContainer m_GuiSplitContainer;
        private System.Windows.Forms.TreeView m_GuiProjectTree;
        private System.Windows.Forms.ToolStripMenuItem m_GuiGenerateToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem m_GuiRefreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem m_GuiConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.TabControl m_GuiDetailPaneTabCollection;
    }
}