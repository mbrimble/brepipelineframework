using System;
using Microsoft.XLANGs.RuntimeTypes;
using System.Globalization;
using System.Xml.XPath;
using System.IO;
using System.Xml;
using BREPipelineFramework.Helpers;
using Microsoft.BizTalk.Streaming;
using BREPipelineFramework.Helpers.Tracing;
using Microsoft.BizTalk.Component;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class TransformationInstruction : IBREPipelineInstruction
    {
        #region Private fields

        private string mapName;
        private Type mapType;
        private TransformationSourceSchemaValidation validateSourceSchema;
        private string callToken;

        #endregion

        #region Constructors

        public TransformationInstruction(string mapClassName, string mapAssemblyName, TransformationSourceSchemaValidation validateSourceSchema, string callToken)
        {
            this.mapName = string.Format("{0}, {1}", mapClassName, mapAssemblyName);
            this.validateSourceSchema = validateSourceSchema;
            mapType = ObjectCreator.ResolveType(mapName);
            this.callToken = callToken;

            if (mapType == null)
            {
                throw new Exception("Unable to load map with name - " + mapName);
            }
        }

        #endregion

        #region Public methods

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            dynamic transformMetaData;
            SchemaMetadata sourceSchemaMetadata;
            string schemaName;
            SchemaMetadata targetSchemaMetadata;

            try
            {
                transformMetaData = TransformMetaData.For(mapType);
                sourceSchemaMetadata = transformMetaData.SourceSchemas[0];
                schemaName = sourceSchemaMetadata.SchemaName;
                targetSchemaMetadata = transformMetaData.TargetSchemas[0];
            }
            catch (Exception e)
            {
                throw new Exception(String.Format("Exception encountered while trying to instantiate map {0}, exception details - {1}", mapName, e.Message));
            }

            if (validateSourceSchema == TransformationSourceSchemaValidation.ValidateSourceSchema || validateSourceSchema == TransformationSourceSchemaValidation.ValidateSourceSchemaIfKnown)
            {
                object property = inmsg.Context.Read(BizTalkGlobalPropertySchemaEnum.MessageType.ToString(), ContextPropertyNamespaces._BTSPropertyNamespace);

                if (property == null)
                {
                    if (validateSourceSchema == TransformationSourceSchemaValidation.ValidateSourceSchema)
                    {
                        throw new Exception("Unable to read source messageType while performing transformation against map " + mapName);
                    }
                }
                else
                {
                    string messageType = property.ToString();

                    if (!string.IsNullOrEmpty(messageType))
                    {
                        if (string.Compare(messageType, schemaName, false, CultureInfo.CurrentCulture) != 0)
                        {
                            throw new Exception(String.Format("Transformation mismatch exception for map {0}, was expecting source schema to be {1} but was actually {2}.", mapName, schemaName, messageType));
                        }
                    }
                }
            }

            try
            {
                XPathDocument input = new XPathDocument(inmsg.BodyPart.GetOriginalDataStream());
                pc.ResourceTracker.AddResource(input);
                dynamic transform = transformMetaData.Transform;
                Stream output = new VirtualStream();

                TraceManager.PipelineComponent.TraceInfo("{0} - Applying transformation {1} to the message", callToken, mapName);
                TraceManager.PipelineComponent.TraceInfo("{0} - Message is being transformed from message type {1} to message type {2}", callToken, schemaName, targetSchemaMetadata.SchemaName);
                
                transform.Transform(input, transformMetaData.ArgumentList, output, new XmlUrlResolver());
                output.Position = 0;
                pc.ResourceTracker.AddResource(output);

                inmsg.BodyPart.Data = output;
                inmsg.Context.Write(BizTalkGlobalPropertySchemaEnum.SchemaStrongName.ToString(), ContextPropertyNamespaces._BTSPropertyNamespace, null);
                inmsg.Context.Promote(BizTalkGlobalPropertySchemaEnum.MessageType.ToString(), ContextPropertyNamespaces._BTSPropertyNamespace, targetSchemaMetadata.SchemaName);
            }
            catch (Exception e)
            {
                throw new Exception("Exception encountered while trying to execute map - " + e.Message);
            }
        }

        #endregion
    }
}