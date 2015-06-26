using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b = BizUnit;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BREPipelineFramework.CustomBizUnitTestSteps;
using BizUnit.Xaml;

namespace BREPipelineFramework.UnitTests
{
    /// <summary>
    /// Contains helper methods that can be used to create common BizUnit test cases
    /// </summary>
    public static class TestHelpers
    {
        public static b.BizUnit BREPipelineFrameworkReceivePipelineBaseTest(string InputFileName, TestContext testContextInstance, string InstanceConfigFilePath = null, 
            XPathCollection contextXPathCollection = null, XPathCollection bodyXPathCollection = null, int ExpectedNumberOfFiles = 1, 
            string PipelineType = "BREPipelineFramework.TestProject.Rcv_BREPipelineFramework", string ExpectedOutputFileName = null,
            string inputMessageType = "BREPipelineFramework.TestProject.Message", string InputContextFileName = null, DataLoaderBase instanceConfigLoader = null, 
            DataLoaderBase inputContextLoader = null, string additionalInputType = null, string yetAnotherInputType = null)
        {
            var _BREPipelineFrameworkTest = new b.Xaml.TestCase();

            var pipelineTestStep = new BREPipelineFramework.CustomBizUnitTestSteps.ExecuteReceivePiplineWithNullablePropertyStep
            {
                PipelineAssemblyPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll",
                PipelineTypeName = PipelineType,
                Source = InputFileName,
                DestinationDir = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                DestinationFileFormat = "Output {0}.txt",
                OutputContextFileFormat = "Context {0}.xml",
            };

            if (!string.IsNullOrEmpty(InstanceConfigFilePath))
            {
                pipelineTestStep.InstanceConfigFile = InstanceConfigFilePath;
            }

            if (instanceConfigLoader != null)
            {
                pipelineTestStep.InstanceConfigLoader = instanceConfigLoader;
            }

            if (!string.IsNullOrEmpty(InputContextFileName))
            {
                pipelineTestStep.InputContextFile = InputContextFileName;
            }

            if (inputContextLoader != null)
            {
                pipelineTestStep.InputContextLoader = inputContextLoader;
            }

            var docSpecDefinition = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

            docSpecDefinition.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
            docSpecDefinition.TypeName = "BREPipelineFramework.TestProject.Envelope";

            pipelineTestStep.DocSpecs.Add(docSpecDefinition);

            var docSpecDefinition1 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

            docSpecDefinition1.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
            docSpecDefinition1.TypeName = inputMessageType;

            pipelineTestStep.DocSpecs.Add(docSpecDefinition1);

            if (!string.IsNullOrEmpty(additionalInputType))
            {
                var docSpecDefinition2 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

                docSpecDefinition2.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
                docSpecDefinition2.TypeName = additionalInputType;

                pipelineTestStep.DocSpecs.Add(docSpecDefinition2);
            }

            if (!string.IsNullOrEmpty(yetAnotherInputType))
            {
                var docSpecDefinition3 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

                docSpecDefinition3.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
                docSpecDefinition3.TypeName = yetAnotherInputType;

                pipelineTestStep.DocSpecs.Add(docSpecDefinition3);
            }

            _BREPipelineFrameworkTest.ExecutionSteps.Add(pipelineTestStep);

            var fileReadMultipleStepContext = new BREPipelineFramework.CustomBizUnitTestSteps.FileReadMultipleStep
            {
                ExpectedNumberOfFiles = ExpectedNumberOfFiles,
                DeleteFiles = false,
                DirectoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                SearchPattern = "Context*.xml",
                Timeout = 5000
            };

            if (ExpectedNumberOfFiles > 0)
            {
                if (contextXPathCollection != null)
                {
                    var xmlValidateContextStep = new BREPipelineFramework.CustomBizUnitTestSteps.XmlValidationStep();

                    foreach (KeyValuePair<string, string> pair in contextXPathCollection.XPathQueryList)
                    {
                        var xPathDefinitionPropertyValue = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
                        {
                            Description = "Property Value Test",
                            XPath = pair.Key,
                            Value = pair.Value
                        };

                        xmlValidateContextStep.XPathValidations.Add(xPathDefinitionPropertyValue);
                    }

                    fileReadMultipleStepContext.SubSteps.Add(xmlValidateContextStep);
                }
            }

            _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepContext);

            var fileReadMultipleStepBody = new BREPipelineFramework.CustomBizUnitTestSteps.FileReadMultipleStep
            {
                ExpectedNumberOfFiles = ExpectedNumberOfFiles,
                DeleteFiles = false,
                DirectoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                SearchPattern = "Output*.txt",
                Timeout = 5000
            };

            if (ExpectedNumberOfFiles > 0)
            {
                if (bodyXPathCollection != null)
                {
                    var xmlValidateBodyStep = new BREPipelineFramework.CustomBizUnitTestSteps.XmlValidationStep();

                    foreach (KeyValuePair<string, string> pair in bodyXPathCollection.XPathQueryList)
                    {
                        var xPathDefinitionPropertyValue = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
                        {
                            Description = "Body Value Test",
                            XPath = pair.Key,
                            Value = pair.Value
                        };

                        xmlValidateBodyStep.XPathValidations.Add(xPathDefinitionPropertyValue);
                    }

                    fileReadMultipleStepBody.SubSteps.Add(xmlValidateBodyStep);
                }
                if (!String.IsNullOrEmpty(ExpectedOutputFileName))
                {
                    var binaryStep = new BinaryComparisonTestStep();
                    binaryStep.ComparisonDataPath = ExpectedOutputFileName;
                    //binaryStep.CompareAsUTF8 = true;
                    fileReadMultipleStepBody.SubSteps.Add(binaryStep);
                }
            }

            _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepBody);
            
            var bizUnit = new b.BizUnit(_BREPipelineFrameworkTest);

            return bizUnit;
        }

        public static b.BizUnit BREPipelineFrameworkSendPipelineBaseTest(string InputFileName, TestContext testContextInstance, string InstanceConfigFilePath = null,
            XPathCollection contextXPathCollection = null, XPathCollection bodyXPathCollection = null, string PipelineType = "BREPipelineFramework.TestProject.Snd_BREPipelineFramework", 
            string ExpectedOutputFileName = null, string inputMessageType = "BREPipelineFramework.TestProject.Message", string InputContextFileName = null, 
            DataLoaderBase instanceConfigLoader = null, DataLoaderBase inputContextLoader = null, string additionalInputType = null, string yetAnotherInputType = null)
        {
            var _BREPipelineFrameworkTest = new b.Xaml.TestCase();

            var pipelineTestStep = new ExecuteSendPipelineWithNullablePropertyStep
            {
                PipelineAssemblyPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll",
                PipelineTypeName = PipelineType,
                SourceDir = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files",
                SearchPattern = InputFileName,
                Destination = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Output.txt",
                OutputContextFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Context.xml",
            };

            if (!string.IsNullOrEmpty(InstanceConfigFilePath))
            {
                pipelineTestStep.InstanceConfigFile = InstanceConfigFilePath;
            }

            if (instanceConfigLoader != null)
            {
                pipelineTestStep.InstanceConfigLoader = instanceConfigLoader;
            }
            
            if (inputContextLoader != null)
            {
                pipelineTestStep.InputContextLoader = inputContextLoader;
            }

            var docSpecDefinition = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

            docSpecDefinition.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
            docSpecDefinition.TypeName = "BREPipelineFramework.TestProject.Envelope";

            pipelineTestStep.DocSpecs.Add(docSpecDefinition);

            var docSpecDefinition1 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

            docSpecDefinition1.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
            docSpecDefinition1.TypeName = inputMessageType;

            pipelineTestStep.DocSpecs.Add(docSpecDefinition1);

            if (!string.IsNullOrEmpty(additionalInputType))
            {
                var docSpecDefinition2 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

                docSpecDefinition2.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
                docSpecDefinition2.TypeName = additionalInputType;

                pipelineTestStep.DocSpecs.Add(docSpecDefinition2);
            }

            if (!string.IsNullOrEmpty(yetAnotherInputType))
            {
                var docSpecDefinition3 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

                docSpecDefinition3.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
                docSpecDefinition3.TypeName = yetAnotherInputType;

                pipelineTestStep.DocSpecs.Add(docSpecDefinition3);
            }


            _BREPipelineFrameworkTest.ExecutionSteps.Add(pipelineTestStep);

            var fileReadMultipleStepContext = new b.TestSteps.File.FileReadMultipleStep
            {
                ExpectedNumberOfFiles = 1,
                DeleteFiles = false,
                DirectoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                SearchPattern = "Context.xml",
                Timeout = 3000
            };

            var xmlValidateContextStep = new BREPipelineFramework.CustomBizUnitTestSteps.XmlValidationStep();

            foreach (KeyValuePair<string, string> pair in contextXPathCollection.XPathQueryList)
            {
                var xPathDefinitionPropertyValue = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
                {
                    Description = "Property Value Test",
                    XPath = pair.Key,
                    Value = pair.Value
                };

                xmlValidateContextStep.XPathValidations.Add(xPathDefinitionPropertyValue);
            }

            fileReadMultipleStepContext.SubSteps.Add(xmlValidateContextStep);

            _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepContext);

            var bizUnit = new b.BizUnit(_BREPipelineFrameworkTest);

            return bizUnit;
        }

        public static DataLoaderBase CreateInstanceConfig(TestContext testContextInstance, string applicationContext = null, string fileName = "Base Config.xml")
        {
            var xdl = new b.TestSteps.DataLoaders.Xml.XmlDataLoader();
            xdl.FilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\" + fileName;
            xdl.UpdateXml = new System.Collections.ObjectModel.Collection<b.TestSteps.Common.XPathDefinition>();

            if (applicationContext != null)
            {
                xdl.UpdateXml.Add(new b.TestSteps.Common.XPathDefinition { XPath = "/*[local-name()='Root' and namespace-uri()='']/*[local-name()='Stages' and namespace-uri()='']/*[local-name()='Stage' and namespace-uri()='']/*[local-name()='Components' and namespace-uri()='']/*[local-name()='Component' and namespace-uri()='']/*[local-name()='Properties' and namespace-uri()='']/*[local-name()='ApplicationContext' and namespace-uri()='']", Value = applicationContext });
            }

            return xdl;
        }

        public static DataLoaderBase CreateInputContext(TestContext testContextInstance, string propertyName, string propertyNamespace, string value = "ExpectedResult", bool promoted = false)
        {
            var xdl = new b.TestSteps.DataLoaders.Xml.XmlDataLoader();
            xdl.FilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Base Context.xml"; ;
            xdl.UpdateXml = new System.Collections.ObjectModel.Collection<b.TestSteps.Common.XPathDefinition>();

            xdl.UpdateXml.Add(new b.TestSteps.Common.XPathDefinition { XPath = "/*[local-name()='MessageInfo' and namespace-uri()='']/*[local-name()='ContextInfo' and namespace-uri()='']/*[local-name()='Property' and namespace-uri()='']/@*[local-name()='Name' and namespace-uri()='']", Value = propertyName });
            xdl.UpdateXml.Add(new b.TestSteps.Common.XPathDefinition { XPath = "/*[local-name()='MessageInfo' and namespace-uri()='']/*[local-name()='ContextInfo' and namespace-uri()='']/*[local-name()='Property' and namespace-uri()='']/@*[local-name()='Namespace' and namespace-uri()='']", Value = propertyNamespace });
            xdl.UpdateXml.Add(new b.TestSteps.Common.XPathDefinition { XPath = "/*[local-name()='MessageInfo' and namespace-uri()='']/*[local-name()='ContextInfo' and namespace-uri()='']/*[local-name()='Property' and namespace-uri()='']/@*[local-name()='Value' and namespace-uri()='']", Value = value });
            
            if (promoted)
            {
                xdl.UpdateXml.Add(new b.TestSteps.Common.XPathDefinition { XPath = "/*[local-name()='MessageInfo' and namespace-uri()='']/*[local-name()='ContextInfo' and namespace-uri()='']/*[local-name()='Property' and namespace-uri()='']/@*[local-name()='Promoted' and namespace-uri()='']", Value = "true" });
            }
            else 
            {
                xdl.UpdateXml.Add(new b.TestSteps.Common.XPathDefinition { XPath = "/*[local-name()='MessageInfo' and namespace-uri()='']/*[local-name()='ContextInfo' and namespace-uri()='']/*[local-name()='Property' and namespace-uri()='']/@*[local-name()='Promoted' and namespace-uri()='']", Value = "false" });
            }

            return xdl;
        }
    }
}
