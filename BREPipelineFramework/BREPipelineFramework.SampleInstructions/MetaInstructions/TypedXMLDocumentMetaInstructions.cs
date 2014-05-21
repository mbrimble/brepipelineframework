using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.RuleEngine;
using BREPipelineFramework.SampleInstructions.Instructions;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class TypedXMLDocumentMetaInstructions : BREPipelineMetaInstructionBase
    {
        private TypedXmlDocument doc;
        private XMLFactsApplicationStageEnum xmlFactsApplicationStage;

        public TypedXMLDocumentMetaInstructions(TypedXmlDocument doc, XMLFactsApplicationStageEnum xmlFactsApplicationStage)
        {
            this.doc = doc;
            this.xmlFactsApplicationStage = xmlFactsApplicationStage;
        }

        public void AddNodeWithValue(TypedXmlDocument document, string xpath, string nodeName, string value)
        {
            XmlHelper.AddNodeWithValue(document, xpath, nodeName, value);
        }

        public void AddNodeWithNamespaceAndValue(TypedXmlDocument document, string xpath, string nodeName, string nodeNamespace, string value)
        {
            XmlHelper.AddNodeWithValue(document, xpath, nodeName, nodeNamespace, value);
        }

        public void AddAttribute(TypedXmlDocument document, string xpath, string attributeName, object attributeValue)
        {
            XmlHelper.AddAttribute(document, xpath, attributeName, attributeValue);
        }

        public void AddNode(TypedXmlDocument document, string xpath, string nodeName)
        {
            XmlHelper.AddNode(document, xpath, nodeName);
        }

        public void AddNodeWithNamespace(TypedXmlDocument document, string xpath, string nodeName, string nodeNamespace)
        {
            XmlHelper.AddNode(document, xpath, nodeName, nodeNamespace);
        }

        public void AddNodeIfNotThere(TypedXmlDocument document, string xpath, string nodeName)
        {
            XmlHelper.AddNodeIfNotThere(document, xpath, nodeName);
        }

        public void AddNodeWithNamespaceIfNotThere(TypedXmlDocument document, string xpath, string nodeName, string nodeNamespace)
        {
            XmlHelper.AddNodeIfNotThere(document, xpath, nodeName, nodeNamespace);
        }

        public void ApplyTypedXmlDocument()
        {
            ApplyTypedXMLDocumentInstruction instruction = new ApplyTypedXMLDocumentInstruction(doc, xmlFactsApplicationStage);
            base.AddInstruction(instruction);
        }
    }
}
