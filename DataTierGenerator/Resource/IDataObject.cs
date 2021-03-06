using System;
using System.Collections.Generic;
using System.Text;

namespace #ROOT_NAMESPACE# {

    public interface IDataObject<T> {

        bool IsNew {
            get;
        }

        bool IsDirty {
            get;
        }

        void Save();

        void Delete();

    }

}
