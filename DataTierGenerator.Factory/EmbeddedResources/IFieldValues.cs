using System;
using System.Collections.Generic;
using System.Text;

namespace #ROOT_NAMESPACE# {

    internal interface IFieldValues {

        FieldValue[] FieldValues {
            get;
        }

        FieldValue[] PrimaryKeyFieldValues {
            get;
        }

        FieldValue[] DirtyFieldValues {
            get;
        }

    }

}
