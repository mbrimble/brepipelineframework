using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.BizTalk.Streaming;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace BREPipelineFramework.Helpers
{
    public class AddXmlNamespaceStream : XmlTranslatorStream
    {
        private String namespace_;
        private String namespacePrefix;
        private int level_ = 0; // hierarchy level
        private Boolean replaceExistingNS = false;

        public AddXmlNamespaceStream(Stream stream, String @namespace)
            : base(XmlReader.Create(stream))
        {
            namespace_ = @namespace;
            this.namespacePrefix = "ns0";
        }

        public AddXmlNamespaceStream(Stream stream, String @namespace, String namespacePrefix)
            : base(XmlReader.Create(stream))
        {
            namespace_ = @namespace;
            this.namespacePrefix = namespacePrefix;
        }

        public AddXmlNamespaceStream(Stream stream, String @namespace, Boolean replaceExistingNS)
            : base(XmlReader.Create(stream))
        {
            namespace_ = @namespace;
            this.namespacePrefix = "ns0";
            this.replaceExistingNS = replaceExistingNS;
        }

        public AddXmlNamespaceStream(Stream stream, String @namespace, String namespacePrefix, Boolean replaceExistingNS)
            : base(XmlReader.Create(stream))
        {
            namespace_ = @namespace;
            this.namespacePrefix = namespacePrefix;
            this.replaceExistingNS = replaceExistingNS;
        }

        #region XmlTranslatorStream Overrides

        protected override void TranslateStartElement(string prefix, string localName, string nsURI)
        {
            if (level_++ != 0)
            {
                base.TranslateStartElement(prefix, localName, nsURI);
                return;
            }

            if (String.IsNullOrEmpty(nsURI) || replaceExistingNS)
            {
                nsURI = namespace_;
                if (String.IsNullOrEmpty(prefix) || replaceExistingNS)
                {
                    if (prefix == namespacePrefix)
                    {
                        prefix = "prefixClash";
                    }
                    else
                    {
                        prefix = namespacePrefix;
                    }
                }
            }

            base.TranslateStartElement(prefix, localName, nsURI);
        }

        protected override void TranslateEndElement(bool full)
        {
            if (level_-- != 0)
            {
                base.TranslateEndElement(full);
                return;
            }

            base.TranslateEndElement(full);
        }

        #endregion
    }
}
