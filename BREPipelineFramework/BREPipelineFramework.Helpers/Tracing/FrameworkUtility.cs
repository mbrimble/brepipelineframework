//=================================================================================
// Inherited from Microsoft BizTalk CAT Team Best Practices Samples
//
// The Framework library is a set of general best practices for BizTalk developers.
//
//=================================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
//=================================================================================

using System;
using System.Diagnostics.CodeAnalysis;

namespace BREPipelineFramework.Helpers.Tracing
{
    public delegate T Func<T>();
    
    [ExcludeFromCodeCoverage]
    public static class FrameworkUtility
    {
        public static T GetDeclarativeAttribute<T>(object obj) where T : class
        {
            Guard.ArgumentNotNull(obj, "obj");

            return GetDeclarativeAttribute<T>(obj.GetType());
        }

        public static T GetDeclarativeAttribute<T>(Type type) where T : class
        {
            Guard.ArgumentNotNull(type, "type");

            object[] attributes = type.GetCustomAttributes(true);

            if (attributes != null && attributes.Length > 0)
            {
                foreach (object attrObject in attributes)
                {
                    if (attrObject.GetType() == typeof(T))
                    {
                        return attrObject as T;
                    }
                }
            }

            return default(T);
        }
    }
}
