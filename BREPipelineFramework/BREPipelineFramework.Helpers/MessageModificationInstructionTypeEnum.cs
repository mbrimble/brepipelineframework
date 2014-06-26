using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BREPipelineFramework.Helpers
{
    public enum MessageModificationInstructionTypeEnum
    {
        RemoveNamespace,
        UpdateNamespaceAndPrefix,
        AddRootNodeNamespaceAndPrefix,
        UpdateRootNodeNamespaceAndPrefix,
        UpdateElementName,
        UpdateElementValue,
        UpdateAttributeName,
        UpdateAttributeValue,
        RemoveAttribute,
        RemoveElement
    }
}
