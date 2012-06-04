using System;
using System.Collections.Generic;
using System.Text;

namespace PriorityIt.DataTierGenerator.Generator {

    class ConcreteBusinessEntityGenerator : GeneratorBase {

        #region private and protected member variables
        #endregion

        #region internal structured members
        #endregion

        #region constructors / desturctors

        public ConcreteBusinessEntityGenerator( ) {
        }

        public ConcreteBusinessEntityGenerator( string rootNamespace )
            : base( rootNamespace ) {
        }

        public ConcreteBusinessEntityGenerator( string rootNamespace, Table table )
            : base( rootNamespace, table ) {
        }

        ~ConcreteBusinessEntityGenerator( ) {
        }

        #endregion

        #region public properties

        public override string CLASS_NAME {
            get {
                if ( m_CLASS_NAME == "" ) {
                    m_CLASS_NAME = Utility.FormatPascal( Table.ProgrammaticAlias );
                    m_CLASS_NAME = m_CLASS_NAME + "BusinessObject";
                }

                return m_CLASS_NAME;
            }
        }

        #endregion

        #region event handlers / overrides

        public override string ToString () {

            base.CLASS_SUMMARY = "Provides business level functionality.";
            base.m_TARGET_NAMESPACE = base.ROOT_NAMESPACE + ".BLL.Entity";
            base.m_SUBCLASS_NAME = "_" + CLASS_NAME;

            return base.ToString ();
        }

        protected override void OnGetUsingStatements () {

            base.OnGetUsingStatements ();

            AppendLine ();
            AppendLine ("using #ROOT_NAMESPACE#.BLL.Entity.Abstract;");
            AppendLine ();
            AppendLine ("using #ROOT_NAMESPACE#.DAL.Entity;");
            AppendLine ("using #ROOT_NAMESPACE#.DAL.Gateway;");

        }

        protected override void OnConstructorBeginBlock () {

            AppendLine ();
            AppendLine ("public #CLASS_NAME#() : base(){");

        }

        protected override void OnPostConstructor () {

            AppendLine ();
            AppendLine ("public #CLASS_NAME#( #CONCRETE_DATA_ENTITY_TYPE_NAME# #CLASS_VARIABLE_NAME_PREFIX#DataObject ){");
            IndentIncrement ();
            AppendLine ("m_#CONCRETE_DATA_ENTITY_TYPE_NAME# = #CLASS_VARIABLE_NAME_PREFIX#DataObject;");
            IndentDecrement ();
            AppendLine ("}");

            if (m_Table.PrimaryKey.Columns.Count > 0) {

                AppendLine ();
                AppendLine ("public #CLASS_NAME#( #PK_PARAMETER_LIST# ) : base( #PK_ARGUMENT_LIST# ) {");
                AppendLine ();
                AppendLine ("}");

            }
        }

        #endregion

        #region public methods
        #endregion

        #region private implementation
        #endregion

    }

}
