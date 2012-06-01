namespace TSHOU.DataTierGenerator {
    partial class NewProjectWizard {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent () {
            this.components = new System.ComponentModel.Container();
            UtilityLibrary.Wizards.WizardFinalPage m_GuiFinishPage;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( NewProjectWizard ) );
            this.m_GuiWizardForm = new UtilityLibrary.Wizards.WizardForm();
            this.m_GuiWelcomePage = new UtilityLibrary.Wizards.WizardWelcomePage();
            this.m_GuiInfoPage = new UtilityLibrary.Wizards.WizardPageBase();
            this.m_GuiClassNamespace = new System.Windows.Forms.TextBox();
            this.m_GuiClassNamespaceLabel = new System.Windows.Forms.Label();
            this.m_GuiBrowseButton = new System.Windows.Forms.Button();
            this.m_GuiOutputPath = new System.Windows.Forms.TextBox();
            this.m_GuiOutputPathLabel = new System.Windows.Forms.Label();
            this.m_GuiDbLoginPage = new UtilityLibrary.Wizards.WizardPageBase();
            this.m_GuiSqlLogin = new TSHOU.DataTierGenerator.Controls.SqlServerLogin();
            this.m_GuiTablePage = new UtilityLibrary.Wizards.WizardPageBase();
            this.m_GuiDeSelectAllLabel = new System.Windows.Forms.Label();
            this.m_GuiSelectAllLabel = new System.Windows.Forms.Label();
            this.m_GuiTableList = new System.Windows.Forms.CheckedListBox();
            this.m_GuiImageList = new System.Windows.Forms.ImageList( this.components );
            this.m_GuiBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
            m_GuiFinishPage = new UtilityLibrary.Wizards.WizardFinalPage();
            this.m_GuiInfoPage.SuspendLayout();
            this.m_GuiDbLoginPage.SuspendLayout();
            this.m_GuiTablePage.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_GuiFinishPage
            // 
            m_GuiFinishPage.BackColor = System.Drawing.Color.White;
            m_GuiFinishPage.Description = "Your Data Tier Project is now ready for use.";
            m_GuiFinishPage.Description2 = "";
            m_GuiFinishPage.FinishPage = true;
            m_GuiFinishPage.HeaderImage = ( (System.Drawing.Image)( resources.GetObject( "m_GuiFinishPage.HeaderImage" ) ) );
            m_GuiFinishPage.Index = 4;
            m_GuiFinishPage.Name = "m_GuiFinishPage";
            m_GuiFinishPage.Size = new System.Drawing.Size( 496, 325 );
            m_GuiFinishPage.TabIndex = 0;
            m_GuiFinishPage.Title = "Project Creation Complete";
            m_GuiFinishPage.WatermarkVisible = false;
            m_GuiFinishPage.WelcomePage = true;
            m_GuiFinishPage.WizardPageParent = this.m_GuiWizardForm;
            // 
            // m_GuiWizardForm
            // 
            this.m_GuiWizardForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_GuiWizardForm.Location = new System.Drawing.Point( 0, 0 );
            this.m_GuiWizardForm.Name = "m_GuiWizardForm";
            this.m_GuiWizardForm.PageIndex = 1;
            this.m_GuiWizardForm.Pages.AddRange( new UtilityLibrary.Wizards.WizardPageBase[] {
            this.m_GuiWelcomePage,
            this.m_GuiInfoPage,
            this.m_GuiDbLoginPage,
            this.m_GuiTablePage,
            m_GuiFinishPage} );
            this.m_GuiWizardForm.Size = new System.Drawing.Size( 496, 374 );
            this.m_GuiWizardForm.TabIndex = 0;
            this.m_GuiWizardForm.WizardClosed += new System.EventHandler( this.m_GuiWizardForm_WizardClosed );
            this.m_GuiWizardForm.Finish += new UtilityLibrary.Wizards.WizardForm.WizardNextEventHandler( this.m_GuiWizardForm_Finish );
            this.m_GuiWizardForm.Cancel += new UtilityLibrary.Wizards.WizardForm.WizardCancelEventHandler( this.m_GuiWizardForm_Cancel );
            this.m_GuiWizardForm.Next += new UtilityLibrary.Wizards.WizardForm.WizardNextEventHandler( this.m_GuiWizardForm_Next );
            // 
            // m_GuiWelcomePage
            // 
            this.m_GuiWelcomePage.BackColor = System.Drawing.Color.White;
            this.m_GuiWelcomePage.Description = "This wizard will guide you through the process of creating a new Data Tier Projec" +
                "t.";
            this.m_GuiWelcomePage.Description2 = "";
            this.m_GuiWelcomePage.DontShow = true;
            this.m_GuiWelcomePage.HeaderImage = ( (System.Drawing.Image)( resources.GetObject( "m_GuiWelcomePage.HeaderImage" ) ) );
            this.m_GuiWelcomePage.Index = 0;
            this.m_GuiWelcomePage.Name = "m_GuiWelcomePage";
            this.m_GuiWelcomePage.Size = new System.Drawing.Size( 496, 327 );
            this.m_GuiWelcomePage.TabIndex = 0;
            this.m_GuiWelcomePage.Title = "New Project Wizard";
            this.m_GuiWelcomePage.WatermarkVisible = false;
            this.m_GuiWelcomePage.WizardPageParent = this.m_GuiWizardForm;
            // 
            // m_GuiInfoPage
            // 
            this.m_GuiInfoPage.Controls.Add( this.m_GuiClassNamespace );
            this.m_GuiInfoPage.Controls.Add( this.m_GuiClassNamespaceLabel );
            this.m_GuiInfoPage.Controls.Add( this.m_GuiBrowseButton );
            this.m_GuiInfoPage.Controls.Add( this.m_GuiOutputPath );
            this.m_GuiInfoPage.Controls.Add( this.m_GuiOutputPathLabel );
            this.m_GuiInfoPage.Description = "Please enter some basic project information.";
            this.m_GuiInfoPage.Index = 1;
            this.m_GuiInfoPage.Name = "m_GuiInfoPage";
            this.m_GuiInfoPage.Size = new System.Drawing.Size( 480, 263 );
            this.m_GuiInfoPage.TabIndex = 0;
            this.m_GuiInfoPage.Title = "Project Information";
            this.m_GuiInfoPage.WizardPageParent = this.m_GuiWizardForm;
            // 
            // m_GuiClassNamespace
            // 
            this.m_GuiClassNamespace.Location = new System.Drawing.Point( 41, 99 );
            this.m_GuiClassNamespace.Name = "m_GuiClassNamespace";
            this.m_GuiClassNamespace.Size = new System.Drawing.Size( 383, 20 );
            this.m_GuiClassNamespace.TabIndex = 4;
            this.m_GuiClassNamespace.Text = "Testing";
            this.m_GuiClassNamespace.TextChanged += new System.EventHandler( this.m_GuiClassNamespace_TextChanged );
            // 
            // m_GuiClassNamespaceLabel
            // 
            this.m_GuiClassNamespaceLabel.AutoSize = true;
            this.m_GuiClassNamespaceLabel.Location = new System.Drawing.Point( 27, 83 );
            this.m_GuiClassNamespaceLabel.Name = "m_GuiClassNamespaceLabel";
            this.m_GuiClassNamespaceLabel.Size = new System.Drawing.Size( 92, 13 );
            this.m_GuiClassNamespaceLabel.TabIndex = 3;
            this.m_GuiClassNamespaceLabel.Text = "Class Namespace";
            // 
            // m_GuiBrowseButton
            // 
            this.m_GuiBrowseButton.Location = new System.Drawing.Point( 388, 39 );
            this.m_GuiBrowseButton.Name = "m_GuiBrowseButton";
            this.m_GuiBrowseButton.Size = new System.Drawing.Size( 36, 21 );
            this.m_GuiBrowseButton.TabIndex = 2;
            this.m_GuiBrowseButton.Text = "...";
            this.m_GuiBrowseButton.UseVisualStyleBackColor = true;
            this.m_GuiBrowseButton.Click += new System.EventHandler( this.m_GuiBrowseButton_Click );
            // 
            // m_GuiOutputPath
            // 
            this.m_GuiOutputPath.Location = new System.Drawing.Point( 41, 40 );
            this.m_GuiOutputPath.Name = "m_GuiOutputPath";
            this.m_GuiOutputPath.Size = new System.Drawing.Size( 341, 20 );
            this.m_GuiOutputPath.TabIndex = 1;
            this.m_GuiOutputPath.Text = "c:\\temp\\tmp";
            this.m_GuiOutputPath.TextChanged += new System.EventHandler( this.m_GuiOutputPath_TextChanged );
            // 
            // m_GuiOutputPathLabel
            // 
            this.m_GuiOutputPathLabel.AutoSize = true;
            this.m_GuiOutputPathLabel.Location = new System.Drawing.Point( 27, 23 );
            this.m_GuiOutputPathLabel.Name = "m_GuiOutputPathLabel";
            this.m_GuiOutputPathLabel.Size = new System.Drawing.Size( 64, 13 );
            this.m_GuiOutputPathLabel.TabIndex = 0;
            this.m_GuiOutputPathLabel.Text = "Output Path";
            // 
            // m_GuiDbLoginPage
            // 
            this.m_GuiDbLoginPage.Controls.Add( this.m_GuiSqlLogin );
            this.m_GuiDbLoginPage.Description = "Please enter your SQL Server login information below. The provided information wi" +
                "ll be used to extract this projects database schema.";
            this.m_GuiDbLoginPage.Index = 2;
            this.m_GuiDbLoginPage.Name = "m_GuiDbLoginPage";
            this.m_GuiDbLoginPage.Size = new System.Drawing.Size( 480, 263 );
            this.m_GuiDbLoginPage.TabIndex = 0;
            this.m_GuiDbLoginPage.Title = "Database Login";
            this.m_GuiDbLoginPage.WizardPageParent = this.m_GuiWizardForm;
            // 
            // m_GuiSqlLogin
            // 
            this.m_GuiSqlLogin.Location = new System.Drawing.Point( 115, 15 );
            this.m_GuiSqlLogin.Name = "m_GuiSqlLogin";
            this.m_GuiSqlLogin.Size = new System.Drawing.Size( 251, 229 );
            this.m_GuiSqlLogin.TabIndex = 0;
            this.m_GuiSqlLogin.DatabindComplete += new System.EventHandler( this.m_GuiSqlLogin_DatabindComplete );
            // 
            // m_GuiTablePage
            // 
            this.m_GuiTablePage.Controls.Add( this.m_GuiDeSelectAllLabel );
            this.m_GuiTablePage.Controls.Add( this.m_GuiSelectAllLabel );
            this.m_GuiTablePage.Controls.Add( this.m_GuiTableList );
            this.m_GuiTablePage.Description = "Select which tables you would like to generate with this project.";
            this.m_GuiTablePage.Index = 3;
            this.m_GuiTablePage.Name = "m_GuiTablePage";
            this.m_GuiTablePage.Size = new System.Drawing.Size( 480, 261 );
            this.m_GuiTablePage.TabIndex = 0;
            this.m_GuiTablePage.Title = "Select Tables";
            this.m_GuiTablePage.WizardPageParent = this.m_GuiWizardForm;
            // 
            // m_GuiDeSelectAllLabel
            // 
            this.m_GuiDeSelectAllLabel.AutoSize = true;
            this.m_GuiDeSelectAllLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.m_GuiDeSelectAllLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.m_GuiDeSelectAllLabel.Location = new System.Drawing.Point( 95, 250 );
            this.m_GuiDeSelectAllLabel.Name = "m_GuiDeSelectAllLabel";
            this.m_GuiDeSelectAllLabel.Size = new System.Drawing.Size( 65, 13 );
            this.m_GuiDeSelectAllLabel.TabIndex = 5;
            this.m_GuiDeSelectAllLabel.Text = "DeSelect All";
            this.m_GuiDeSelectAllLabel.Click += new System.EventHandler( this.m_GuiDeSelectAllLabel_Click );
            // 
            // m_GuiSelectAllLabel
            // 
            this.m_GuiSelectAllLabel.AutoSize = true;
            this.m_GuiSelectAllLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.m_GuiSelectAllLabel.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
            this.m_GuiSelectAllLabel.Location = new System.Drawing.Point( 38, 250 );
            this.m_GuiSelectAllLabel.Name = "m_GuiSelectAllLabel";
            this.m_GuiSelectAllLabel.Size = new System.Drawing.Size( 51, 13 );
            this.m_GuiSelectAllLabel.TabIndex = 2;
            this.m_GuiSelectAllLabel.Text = "Select All";
            this.m_GuiSelectAllLabel.Click += new System.EventHandler( this.m_GuiSelectAllLabel_Click );
            // 
            // m_GuiTableList
            // 
            this.m_GuiTableList.CheckOnClick = true;
            this.m_GuiTableList.FormattingEnabled = true;
            this.m_GuiTableList.Location = new System.Drawing.Point( 41, 21 );
            this.m_GuiTableList.Name = "m_GuiTableList";
            this.m_GuiTableList.Size = new System.Drawing.Size( 399, 214 );
            this.m_GuiTableList.TabIndex = 0;
            this.m_GuiTableList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler( this.m_GuiTableList_ItemCheck );
            // 
            // m_GuiImageList
            // 
            this.m_GuiImageList.ImageStream = ( (System.Windows.Forms.ImageListStreamer)( resources.GetObject( "m_GuiImageList.ImageStream" ) ) );
            this.m_GuiImageList.TransparentColor = System.Drawing.Color.White;
            this.m_GuiImageList.Images.SetKeyName( 0, "cs.ico" );
            // 
            // m_GuiBrowseFolder
            // 
            this.m_GuiBrowseFolder.Description = "Project Output Path";
            this.m_GuiBrowseFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // NewProjectWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size( 496, 374 );
            this.Controls.Add( this.m_GuiWizardForm );
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectWizard";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Project";
            this.m_GuiInfoPage.ResumeLayout( false );
            this.m_GuiInfoPage.PerformLayout();
            this.m_GuiDbLoginPage.ResumeLayout( false );
            this.m_GuiTablePage.ResumeLayout( false );
            this.m_GuiTablePage.PerformLayout();
            this.ResumeLayout( false );

        }

        #endregion

        private UtilityLibrary.Wizards.WizardForm m_GuiWizardForm;
        private UtilityLibrary.Wizards.WizardWelcomePage m_GuiWelcomePage;
        private UtilityLibrary.Wizards.WizardPageBase m_GuiDbLoginPage;
        private System.Windows.Forms.ImageList m_GuiImageList;
        private TSHOU.DataTierGenerator.Controls.SqlServerLogin m_GuiSqlLogin;
        private UtilityLibrary.Wizards.WizardPageBase m_GuiTablePage;
        private System.Windows.Forms.CheckedListBox m_GuiTableList;
        private UtilityLibrary.Wizards.WizardPageBase m_GuiInfoPage;
        private System.Windows.Forms.Label m_GuiClassNamespaceLabel;
        private System.Windows.Forms.Button m_GuiBrowseButton;
        private System.Windows.Forms.TextBox m_GuiOutputPath;
        private System.Windows.Forms.Label m_GuiOutputPathLabel;
        private System.Windows.Forms.TextBox m_GuiClassNamespace;
        private System.Windows.Forms.FolderBrowserDialog m_GuiBrowseFolder;
        private System.Windows.Forms.Label m_GuiDeSelectAllLabel;
        private System.Windows.Forms.Label m_GuiSelectAllLabel;
    }
}