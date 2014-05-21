using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.Helpers
{
    /// <summary>
    /// Static class that aids in type casting
    /// </summary>
    public static class TypeCaster
    {
        /// <summary>
        /// Cast a string to an object of a given type
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetTypedObject(string sourceString, TypeEnum type)
        {
            object o = sourceString;

            return GetTypedObject(o, type);
        }

        /// <summary>
        /// Cast an object to an object of another type
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetTypedObject(object sourceString, TypeEnum type)
        {
            object o = null;

            switch (type)
            {
                case TypeEnum.Boolean:
                    o = Boolean.Parse(sourceString.ToString());
                    break;
                case TypeEnum.DateTime:
                    o = DateTime.Parse(sourceString.ToString());
                    break;
                case TypeEnum.Integer:
                    o = int.Parse(sourceString.ToString());
                    break;
                case TypeEnum.String:
                    o = sourceString.ToString();
                    break;
                default:
                    throw new Exception("Unexpected type to cast to of " + type.ToString());
            }

            return o;
        }
    }
}
