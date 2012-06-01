using System;
using System.Collections.Generic;
using System.Text;

namespace #ROOT_NAMESPACE# {
    public interface IGateway<TIDataObject, TPK> {

        // properties
        FieldDefinition[] FieldDefinitionArray {
            get;
        }

        string DatabaseName {
            get;
        }

        string SchemaName {
            get;
        }

        string TableName {
            get;
        }

        string SchemaTableName {
            get;
        }

        string FullyQualifiedTableName {
            get;
        }

        // methods
        List<TIDataObject> SelectAll();

        TIDataObject SelectByPrimaryKey(TPK id);

        void Insert( TIDataObject dataObject );

        void Update( TIDataObject dataObject );

        void Delete( TIDataObject dataObject );

    }
}
