using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.Helpers
{
    public enum MessageModificationInstructionTypeEnum
    {
        RemoveNamespace = 0,
        UpdateNamespaceAndPrefix = 1,
        AddRootNodeNamespaceAndPrefix = 2,
        UpdateRootNodeNamespaceAndPrefix = 3,
        UpdateElementName = 4,
        UpdateElementValue = 5,
        UpdateAttributeName = 6,
        UpdateAttributeValue = 7,
        RemoveAttribute = 8,
        RemoveElement = 9,
        RemoveElementAndChildElements = 10
    }
}
