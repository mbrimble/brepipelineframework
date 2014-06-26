using Microsoft.BizTalk.SSOClient.Interop;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace BREPipelineFramework.Helpers
{
    /// <summary>
    /// Helper class used to access the SSO Configuration Database
    /// </summary>
    public class SSOConfigurationPropertyBag : IPropertyBag
    {
        private HybridDictionary properties;
        public SSOConfigurationPropertyBag()
        {
            properties = new HybridDictionary();
        }
        public void Read(string propName, out object ptrVar, int errLog)
        {
            ptrVar = properties[propName];
        }
        public void Write(string propName, ref object ptrVar)
        {
            properties.Add(propName, ptrVar);
        }

        [ExcludeFromCodeCoverage]
        public bool Contains(string key)
        {
            return properties.Contains(key);
        }

        [ExcludeFromCodeCoverage]
        public void Remove(string key)
        {
            properties.Remove(key);
        }
    }
}
