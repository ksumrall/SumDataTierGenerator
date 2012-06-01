using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TSHOU.DataTierGenerator.MVP;

namespace TSHOU.DataTierGenerator {

    public partial class DatabaseConnectionSettings : Form {

        #region private and protected member variables

        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public DatabaseConnectionSettings() {
            InitializeComponent();
        }

        #endregion

        #region public properties

        public SqlConnectionSettingsModel Model {
            set {
                m_GuiLogin.Model = value;
            }
        }

        #endregion

        #region event handlers / overrides

        private void m_GuiOkButton_Click(object sender, EventArgs e) {
            m_GuiLogin.Commit ();
        }

        private void m_GuiCancelButton_Click(object sender, EventArgs e) {
            m_GuiLogin.Rollback ();
        }

        #endregion

        #region public methods

        #endregion

        #region private implementation

        #endregion


    }

}