using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BREPipelineFramework.Helpers
{
    /// <summary>
    /// Static class used to instantiate context properties
    /// </summary>
    public static class ContextPropertyCreator
    {
        /// <summary>
        /// Instantiate a context property by specifying the Namespace prefix of the context property's class, the assembly within which it is contained and the name of the property
        /// </summary>
        /// <param name="namespacePrefix">The namespace prefix of the context property's class</param>
        /// <param name="assembly">The assembly within which the context property's class is contained</param>
        /// <param name="propertyName">The name of the context property</param>
        /// <returns>Return a context property</returns>
        public static Microsoft.XLANGs.BaseTypes.MessageContextPropertyBase GetMessageContextPropertyBase(string namespacePrefix, Assembly assembly, string propertyName)
        {
            Type propertyType = assembly.GetType(namespacePrefix + "." + propertyName);
            ConstructorInfo info = propertyType.GetConstructor(Type.EmptyTypes);
            ObjectCreator inv = new ObjectCreator(info);
            object o = null;
            o = inv.CreateInstance();

            Microsoft.XLANGs.BaseTypes.MessageContextPropertyBase contextProperty = (Microsoft.XLANGs.BaseTypes.MessageContextPropertyBase)o;
            return contextProperty;
        }
    }
}
