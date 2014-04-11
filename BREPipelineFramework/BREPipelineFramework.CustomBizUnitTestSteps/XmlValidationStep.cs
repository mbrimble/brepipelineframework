using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using BizUnit.Xaml;
using BizUnit.TestSteps.ValidationSteps.Xml;
using System.IO;
using BizUnit.Common;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using reg = System.Text.RegularExpressions;
using BizUnit;

namespace BREPipelineFramework.CustomBizUnitTestSteps
{
    public class XmlValidationStep : SubStepBase
    {
        private Collection<XPathDefinition> _xPathValidations = new Collection<XPathDefinition>();
        private Collection<SchemaDefinition> _xmlSchemas = new Collection<SchemaDefinition>();
        private Exception _validationException;
        private Context _context;

        public Collection<SchemaDefinition> XmlSchemas
        {
            set
            {
                _xmlSchemas = value;
            }
            get
            {
                return _xmlSchemas;
            }
        }

        public Collection<XPathDefinition> XPathValidations
        {
            get
            {
                return _xPathValidations;
            }
            set
            {
                _xPathValidations = value;
            }
        }

        /// <summary>
        /// ITestStep.Execute() implementation
        /// </summary>
        /// <param name='data'>The stream cintaining the data to be validated.</param>
        /// <param name='context'>The context for the test, this holds state that is passed beteen tests</param>
        public override Stream Execute(Stream data, Context context)
        {
            _context = context;

            if (_xmlSchemas.Count > 0)
            {
                ValidateXmlInstance(data, context);
            }

            if (_xPathValidations.Count > 0)
            {
                XmlReader reader = XmlReader.Create(data);
                var document = new XmlDocument();
                document.Load(reader);

                ValidateXPathExpressions(document, context);
            }

            data.Seek(0, SeekOrigin.Begin);

            return data;
        }

        public override void Validate(Context context)
        {
            foreach (var schema in XmlSchemas)
            {
                ArgumentValidation.CheckForNullReference(schema.XmlSchemaPath, "schema.XmlSchemaPath");
                ArgumentValidation.CheckForNullReference(schema.XmlSchemaNameSpace, "schema.XmlSchemaNameSpace");
            }

            foreach (var xpath in XPathValidations)
            {
                ArgumentValidation.CheckForNullReference(xpath.XPath, "xpath.XPath");
            }
        }

        private void ValidateXmlInstance(Stream data, Context context)
        {
            try
            {
                var settings = new XmlReaderSettings();
                foreach (var xmlSchema in _xmlSchemas)
                {
                    settings.Schemas.Add(xmlSchema.XmlSchemaNameSpace, xmlSchema.XmlSchemaPath);
                }
                settings.ValidationType = ValidationType.Schema;

                XmlReader reader = XmlReader.Create(data, settings);
                var document = new XmlDocument();
                document.Load(reader);

                var eventHandler = new ValidationEventHandler(ValidationEventHandler);

                document.Validate(eventHandler);

            }
            catch (Exception ex)
            {
                context.LogException(ex);
                throw new ValidationStepExecutionException("Failed to validate document instance", ex, context.TestName);
            }
        }

        private void ValidateXPathExpressions(XmlDocument doc, Context context)
        {
            foreach (XPathDefinition validation in _xPathValidations)
            {
                var xpathExp = validation.XPath;
                var expectedValue = validation.Value;

                if (null != validation.Description)
                {
                    context.LogInfo("XPath: {0}", validation.Description);
                }
                context.LogInfo("Evaluting XPath {0} equals \"{1}\"", xpathExp, expectedValue);

                XPathNavigator xpn = doc.CreateNavigator();
                object result = xpn.Evaluate(xpathExp);

                string actualValue = null;
                if (result.GetType().Name == "XPathSelectionIterator")
                {
                    var xpi = result as XPathNodeIterator;
                    xpi.MoveNext(); // BUGBUG!
                    actualValue = xpi.Current.ToString();
                }
                else
                {
                    actualValue = result.ToString();
                }

                if (!string.IsNullOrEmpty(validation.ContextKey))
                {
                    context.Add(validation.ContextKey, actualValue);
                }

                if (!string.IsNullOrEmpty(expectedValue))
                {

                    if (0 != expectedValue.CompareTo(actualValue))
                    {
                        context.LogError("XPath evaluation failed. Expected:<{0}>. Actual:<{1}>.", expectedValue, actualValue);

                        throw new ApplicationException(
                            string.Format("XmlValidationStep failed, compare {0} != {1}, xpath query used: {2}",
                                          expectedValue, actualValue, xpathExp));
                    }

                    context.LogInfo("XPath evaluation succeeded. Expected:<{0}>. Actual:<{1}>.", expectedValue, actualValue);
                }

                if (!string.IsNullOrEmpty(validation.RegexValue))
                {
                    reg.Regex regex = new reg.Regex(validation.RegexValue, reg.RegexOptions.IgnoreCase | reg.RegexOptions.Singleline);
                    var match = regex.Match(actualValue);

                    if (match.Success)
                    {
                        context.LogInfo("XPath evaluation succeeded. Regex expression run:<{0}>. Actual:<{1}>.", validation.RegexValue, actualValue);
                    }
                    else
                    {
                        context.LogError("XPath evaluation failed. Regex expression run:<{0}>. Actual:<{1}>.", validation.RegexValue, actualValue);

                        throw new ApplicationException(
                            string.Format("XmlValidationStep failed, expected regex {0} to evaluate, actual value was {1}, xpath query used: {2}",
                                          validation.RegexValue, actualValue, xpathExp));
                    }
                }
            }
        }

        void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    _context.LogError(e.Message);
                    _validationException = e.Exception;
                    break;
                case XmlSeverityType.Warning:
                    _context.LogWarning(e.Message);
                    _validationException = e.Exception;
                    break;
            }
        }
    }
}
