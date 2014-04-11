using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using b = BizUnit;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BREPipelineFramework.UnitTests
{
    /// <summary>
    /// Contains helper methods that can be used to create common BizUnit test cases
    /// </summary>
    public static class TestHelpers
    {
        public static b.BizUnit BREPipelineFrameworkReceivePipelineBaseTest(string InputFileName, string InstanceConfigFilePath, XPathCollection contextXPathCollection, XPathCollection bodyXPathCollection, TestContext testContextInstance, int ExpectedNumberOfFiles, string PipelineType)
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
                InstanceConfigFile = InstanceConfigFilePath,
            };

            var docSpecDefinition = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

            docSpecDefinition.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
            docSpecDefinition.TypeName = "BREPipelineFramework.TestProject.Envelope";

            pipelineTestStep.DocSpecs.Add(docSpecDefinition);

            var docSpecDefinition1 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

            docSpecDefinition1.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
            docSpecDefinition1.TypeName = "BREPipelineFramework.TestProject.Message";

            pipelineTestStep.DocSpecs.Add(docSpecDefinition1);

            _BREPipelineFrameworkTest.ExecutionSteps.Add(pipelineTestStep);

            if (ExpectedNumberOfFiles > 0)
            {
                var fileReadMultipleStepContext = new b.TestSteps.File.FileReadMultipleStep
                {
                    ExpectedNumberOfFiles = ExpectedNumberOfFiles,
                    DeleteFiles = false,
                    DirectoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                    SearchPattern = "Context*.xml",
                    Timeout = 5000
                };

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

                _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepContext);

                var fileReadMultipleStepBody = new b.TestSteps.File.FileReadMultipleStep
                {
                    ExpectedNumberOfFiles = ExpectedNumberOfFiles,
                    DeleteFiles = false,
                    DirectoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                    SearchPattern = "Output*.txt",
                    Timeout = 5000
                };

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
                _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepBody);
            }
            var bizUnit = new b.BizUnit(_BREPipelineFrameworkTest);

            return bizUnit;
        }

        public static b.BizUnit BREPipelineFrameworkReceivePipelineBaseTest(string InputFileName, string InstanceConfigFilePath, XPathCollection _XPathCollection, TestContext testContextInstance)
        {
            return BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, null, testContextInstance, 1, "BREPipelineFramework.TestProject.Rcv_BREPipelineFramework");
        }

        public static b.BizUnit BREPipelineFrameworkReceivePipelineBaseTest(string InputFileName, string InstanceConfigFilePath, XPathCollection _XPathCollection, TestContext testContextInstance, int ExpectedNumberOfFiles)
        {
            return BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, null, _XPathCollection, testContextInstance, 1, "BREPipelineFramework.TestProject.Rcv_BREPipelineFramework");
        }

        public static b.BizUnit BREPipelineFrameworkSendPipelineBaseTest(string InputFileName, string InstanceConfigFilePath, XPathCollection _XPathCollection, TestContext testContextInstance)
        {
            var _BREPipelineFrameworkTest = new b.Xaml.TestCase();

            var pipelineTestStep = new b.TestSteps.BizTalk.Pipeline.ExecuteSendPipelineStep
            {
                PipelineAssemblyPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll",
                PipelineTypeName = "BREPipelineFramework.TestProject.Snd_BREPipelineFramework",
                SourceDir = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files",
                SearchPattern = InputFileName,
                Destination = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Output.txt",
                OutputContextFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Context.xml",
                InstanceConfigFile = InstanceConfigFilePath,
            };

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

            foreach (KeyValuePair<string, string> pair in _XPathCollection.XPathQueryList)
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

            var deleteStep = new b.TestSteps.File.DeleteStep();
            deleteStep.FilePathsToDelete = new System.Collections.ObjectModel.Collection<string>();
            deleteStep.FilePathsToDelete.Add(testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Output.txt");
            deleteStep.FilePathsToDelete.Add(testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Context.xml");
            _BREPipelineFrameworkTest.CleanupSteps.Add(deleteStep);

            var bizUnit = new b.BizUnit(_BREPipelineFrameworkTest);

            return bizUnit;
        }
    }
}
