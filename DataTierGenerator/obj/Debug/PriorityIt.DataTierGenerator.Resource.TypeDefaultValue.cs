// ===============================================================================
//
// TypeDefaultValue.cs
//
// This file contains the implementations of the class TypeDefaultValue
//
// ===============================================================================
// Release history
// ACTION   DATE        AUTHOR              NOTES
//
// ===============================================================================

using System;

namespace #ROOT_NAMESPACE#
{
	/// <summary>
	/// Summary description for TypeDefaultValue.
	/// </summary>
	public class TypeDefaultValue
	{
        #region private member variables

        #endregion

        #region constructors / destructors

        public TypeDefaultValue()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        ~TypeDefaultValue()
        {
        }


        #endregion

        #region public properties
        #endregion

        #region public methods

        /// <summary>
        /// Returns a default value for the type specified
        /// </summary>
        /// <param name="defaultValueType">The type which default value should be returned.</param>
        /// <returns>The default value for the type specified.</returns>
        public static object GetDefaultValue(System.Type defaultValueType) {
            object valueToReturn=null;

            switch(defaultValueType.UnderlyingSystemType.FullName) {
                case "System.String":
                    valueToReturn="";
                    break;
                case "System.Byte":
                    valueToReturn = (byte)0;
                    break;
                case "System.Int32":
                    valueToReturn = (int)0;
                    break;
                case "System.Int16":
                    valueToReturn = (short)0;
                    break;
                case "System.Int64":
                    valueToReturn = (long)0;
                    break;
                case "System.DateTime":
                    valueToReturn = DateTime.MinValue;
                    break;
                case "System.Decimal":
                    valueToReturn = 0.0M;
                    break;
                case "System.Double":
                    valueToReturn = 0.0;
                    break;
                case "System.Single":
                    valueToReturn = 0.0f;
                    break;
                case "System.Boolean":
                    valueToReturn = false;
                    break;
                case "System.Byte[]":
                    valueToReturn = new byte[0];
                    break;
                case "System.Guid":
                    valueToReturn = Guid.Empty;
                    break;
                default:
                    valueToReturn = new object();
                    break;
            }

            return valueToReturn;

        }
        
        #endregion

        #region private implementation
        #endregion
    }
}
