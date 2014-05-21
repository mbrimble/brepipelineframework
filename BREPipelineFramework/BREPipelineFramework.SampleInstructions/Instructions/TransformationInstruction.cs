using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.XLANGs.RuntimeTypes;
using System.Globalization;
using System.Xml.XPath;
using Microsoft.XLANGs.BaseTypes;
using System.IO;
using Microsoft.BizTalk.Streaming;
using System.Xml;

namespace BREPipelineFramework.SampleInstructions.Instructions
{
    public class TransformationInstruction : IBREPipelineInstruction
    {
        #region Private fields

        private string mapName;
        private bool validate = true;
        private const string btsPropertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/system-properties";

        #endregion

        #region Constructors

        public TransformationInstruction(string mapClassName, string mapAssemblyName)
        {
            this.mapName = string.Format("{0}, {1}", mapClassName, mapAssemblyName);
        }

        public TransformationInstruction(string mapClassName, string mapAssemblyName, bool validate)
        {
            this.mapName = string.Format("{0}, {1}");
            this.validate = validate;
        }

        #endregion

        #region Public methods

        public void Execute(ref Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, Microsoft.BizTalk.Component.Interop.IPipelineContext pc)
        {
            Type mapType = Type.GetType(mapName);

            if (mapType == null)
            {
                throw new Exception("Unable to load map with name - " + mapName);
            }

            TransformMetaData transformMetaData = TransformMetaData.For(mapType);
            SchemaMetadata sourceSchemaMetadata = transformMetaData.SourceSchemas[0];
            string schemaName = sourceSchemaMetadata.SchemaName;
            SchemaMetadata targetSchemaMetadata = transformMetaData.TargetSchemas[0];
            string messageType = string.Empty;

            if (validate)
            {
                try
                {
                    messageType = inmsg.Context.Read("MessageType", btsPropertyNamespace).ToString();
                }
                catch
                {
                    throw new Exception("Unable to read source messageType");
                }

                if (string.Compare(messageType, schemaName, false, CultureInfo.CurrentCulture) != 0)
                {
                    throw new Exception(String.Format("Transformation mismatch exception for map {0}, was expecting source schema to be {1} but was actually {2}.", mapName, schemaName, messageType));
                }
            }

            Stream originalDataStream = inmsg.BodyPart.GetOriginalDataStream();
            if (!originalDataStream.CanSeek)
            {
                ReadOnlySeekableStream stream2 = new ReadOnlySeekableStream(originalDataStream) { Position = 0L };
                originalDataStream = stream2;
            }
            else
            {
                originalDataStream.Position = 0L;
            }

            XPathDocument input = new XPathDocument(originalDataStream);
            ITransform transform = transformMetaData.Transform;
            Stream output = new MemoryStream();
            transform.Transform(input, transformMetaData.ArgumentList, output, new XmlUrlResolver());
            output.Flush();
            output.Seek(0L, SeekOrigin.Begin);

            inmsg.BodyPart.Data = output;
            inmsg.Context.Write("SchemaStrongName", btsPropertyNamespace, null);
            inmsg.Context.Write("MessageType", btsPropertyNamespace, targetSchemaMetadata.SchemaName);
        }

        #endregion
    }
}
