using System;
using System.Collections.Generic;
using System.Text;

using SumDataTierGenerator.Common;

namespace SumDataTierGenerator.CodeGenerationFactory
{

    public class UserViewGateway : GeneratorBase {

        #region constructors / desturctors

        public UserViewGateway( ):this(null) {
        }

        public UserViewGateway(string rootNamespace)
            : this(rootNamespace, null)
        {
        }

        public UserViewGateway(string rootNamespace, string providerType)
            : this(rootNamespace, providerType, null)
        {
        }

        public UserViewGateway(string rootNamespace, string providerType, View view)
            : base(rootNamespace, providerType, view)
        {
            this.SUBCLASS_NAME = "IGateway";
        }

        #endregion

        #region public properties

        /// <summary>
        /// pascal case table name with spaces as underline
        /// </summary>
        public override string CLASS_NAME {
            get {
                if ( m_CLASS_NAME == "" ) {
                    m_CLASS_NAME = CLASS_NAME_PREFIX + "Gateway";
                }

                return m_CLASS_NAME;
            }
        }

        #endregion

        #region event handlers / overrides

        public override string ToString( ) {

            base.CLASS_SUMMARY = "Provides CRUD functionality for the #TABLE_NAME# table.";

            return base.ToString( );
        }

        protected override void OnGetUsingStatements( ) {

            base.OnGetUsingStatements( );

            AppendLine( "using System.Collections.Generic;" );
            AppendLine( "using System.Data;" );
            AppendLine( "using System.Data.Common;" );
            AppendLine( "using System.Text;" );

        }

        protected override void OnGetRegion_ConstructorsDestructors() {

            AppendLine();
            AppendLine( "static #CLASS_NAME#(){" );
            AppendLine( "}" );

            AppendLine();
            AppendLine( "private #CLASS_NAME#(){" );
            AppendLine( "}" );

        }

        #endregion

        #region CRUD methods

        #endregion

        #region private implementation
        #endregion

    }

}
