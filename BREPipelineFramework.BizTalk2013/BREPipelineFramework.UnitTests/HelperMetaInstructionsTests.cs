using BREPipelineFramework.SampleInstructions.MetaInstructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BREPipelineFramework.SampleInstructions;
using BREPipelineFramework.Helpers;
using b = BizUnit;
using System.Collections.Generic;
namespace BREPipelineFramework.UnitTests
{
    [TestClass]
    public class HelperMetaInstructionsTests
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            string directoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files";
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(directoryPath);

            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        #endregion

        const string numberOfPropertiesXPath = "/*[local-name()='MessageInfo' and namespace-uri()='']/*[local-name()='ContextInfo' and namespace-uri()='']/@*[local-name()='PropertiesCount' and namespace-uri()='']";

        /// <summary>
        ///Tests that the ConcatenateString vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_ConcatenateString()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ConcatenateString Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the GenerateGUID vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_GenerateGUID()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GenerateGUID Config.xml";
            string XPathQuery = "/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema']/@*[local-name()='Value' and namespace-uri()='']";

            var _BREPipelineFrameworkTest = new b.Xaml.TestCase();

            var pipelineTestStep = new b.TestSteps.BizTalk.Pipeline.ExecuteReceivePipelineStep
            {
                PipelineAssemblyPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.TestProject\bin\Debug\BREPipelineFramework.TestProject.dll",
                PipelineTypeName = "BREPipelineFramework.TestProject.Rcv_BREPipelineFramework",
                Source = InputFileName,
                DestinationDir = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                DestinationFileFormat = "Output.txt",
                OutputContextFileFormat = "Context.xml",
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

            var xPathDefinitionPropertyValue = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
            {
                Description = "Property Value Test",
                XPath = XPathQuery,
                RegexValue = "[0-9A-F]{8}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{4}-[0-9A-F]{12}"
            };

            xmlValidateContextStep.XPathValidations.Add(xPathDefinitionPropertyValue);

            var xPathDefinitionCountProperties = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
            {
                Description = "Property Value Test",
                XPath = numberOfPropertiesXPath,
                Value = "1"
            };

            xmlValidateContextStep.XPathValidations.Add(xPathDefinitionCountProperties);


            fileReadMultipleStepContext.SubSteps.Add(xmlValidateContextStep);

            _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepContext);

            var deleteStep = new b.TestSteps.File.DeleteStep();
            deleteStep.FilePathsToDelete = new System.Collections.ObjectModel.Collection<string>();
            deleteStep.FilePathsToDelete.Add(testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Output.txt");
            deleteStep.FilePathsToDelete.Add(testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Context.xml");
            _BREPipelineFrameworkTest.CleanupSteps.Add(deleteStep);

            var bizUnit = new b.BizUnit(_BREPipelineFrameworkTest);

            bizUnit.RunTest();
        }

        /// <summary>
        ///Tests that the GetFileExtension vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_GetFileExtension()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetFileExtension Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='.xml'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the GetLowercaseString vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_GetLowercaseString()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetLowercaseString Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='foundexpectedresult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the GetUppercaseString vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_GetUppercaseString()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetUppercaseString Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FOUNDEXPECTEDRESULT'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the GetXPathResult vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_GetXPathResult()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetXPathResult Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='Test_Set_BTS_DestinationParty'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceSubstring vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceSubstring()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceSubstring Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }
        
        /// <summary>
        ///Tests that the GetStringLength vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_GetStringLength()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetStringLength Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='10'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the RoundCurrentTime vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_RoundCurrentTime()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_RoundCurrentTime Config.xml";
            string XPathQuery = "/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema']/@*[local-name()='Value' and namespace-uri()='']";

            var _BREPipelineFrameworkTest = new b.Xaml.TestCase();

            var pipelineTestStep = new b.TestSteps.BizTalk.Pipeline.ExecuteReceivePipelineStep
            {
                PipelineAssemblyPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.TestProject\bin\Debug\BREPipelineFramework.TestProject.dll",
                PipelineTypeName = "BREPipelineFramework.TestProject.Rcv_BREPipelineFramework",
                Source = InputFileName,
                DestinationDir = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                DestinationFileFormat = "Output.txt",
                OutputContextFileFormat = "Context.xml",
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

            var xPathDefinitionPropertyValue = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
            {
                Description = "Property Value Test",
                XPath = XPathQuery,
                RegexValue = "(\\d)(\\d)(\\d)(\\d)(-)(\\d)(\\d)(-)(\\d)(\\d)(T)((?:(?:[0-1][0-9])|(?:[2][0-3])|(?:[0-9])):(?:[0-5][0-9])(?::[0-5][0-9])?(?:\\s?(?:am|AM|pm|PM))?)(\\.)(\\d)(\\d)(\\d)(\\d)(\\d)(\\d)(\\d)(\\+)(\\d)(\\d)(:)(\\d)(\\d)"
            };

            xmlValidateContextStep.XPathValidations.Add(xPathDefinitionPropertyValue);

            var xPathDefinitionCountProperties = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
            {
                Description = "Property Value Test",
                XPath = numberOfPropertiesXPath,
                Value = "1"
            };

            xmlValidateContextStep.XPathValidations.Add(xPathDefinitionCountProperties);


            fileReadMultipleStepContext.SubSteps.Add(xmlValidateContextStep);

            _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepContext);

            var deleteStep = new b.TestSteps.File.DeleteStep();
            deleteStep.FilePathsToDelete = new System.Collections.ObjectModel.Collection<string>();
            deleteStep.FilePathsToDelete.Add(testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Output.txt");
            deleteStep.FilePathsToDelete.Add(testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files\Context.xml");
            _BREPipelineFrameworkTest.CleanupSteps.Add(deleteStep);

            var bizUnit = new b.BizUnit(_BREPipelineFrameworkTest);

            bizUnit.RunTest();
        }

        /// <summary>
        ///Tests that the ValidateStringLength vocabulary definition correctly validates a string with the specified length
        ///</summary>
        [TestMethod()]
        public void Test_ValidateStringLength_Success()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ValidateStringLength_Success Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ValidateStringLength vocabulary definition correctly throws an exception when the length is invalid
        ///</summary>
        [TestMethod()]
        public void Test_ValidateStringLength_Fail()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ValidateStringLength_Fail Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException.Message == "Length of string Test is invalid.  Was expecting 5 but was 4.", "Was expecting a string length invalid type error but instead got " + e.InnerException.Message);
            }
        }

        /// <summary>
        ///Tests that the ThrowException vocabulary definition throws an exception
        ///</summary>
        [TestMethod()]
        public void Test_ThrowException()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ThrowException Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='10'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test to fail");
            }
            catch (Exception e)
            {
                if (e.InnerException.Message != "Testing that an exception was thrown")
                {
                    Assert.Fail("Was expecting for the pipeline test to fail with the specific error - Testing that an exception was thrown, but instead got - " + e.InnerException.Message);
                }
            }
        }

        /// <summary>
        ///Tests that the AddDocumentNamespace vocabulary definition adds the namespace to the XML document root node with the ns0 prefix
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespace Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "ns0:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://brepipelineframework");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespaceWithPrefix vocabulary definition adds the namespace to the XML document root node with the specified prefix
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceAndPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespaceAndPrefix Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "bre:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://brepipelineframework");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespace vocabulary definition doesn't add the namespace to the XML document root node with the ns0 prefix if a root namespace already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceExistingNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespace Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "ns0:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://www.w3.org/2001/XMLSchema-instance");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespaceWithPrefix vocabulary definition doesn't add the namespace to the XML document root node with the specified prefix if a root namespace already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceAndPrefixExistingNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespaceAndPrefix Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "ns0:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://www.w3.org/2001/XMLSchema-instance");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespace vocabulary definition replaces the current namespace at the XML document root node with the ns0 prefix
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespace Config.xml";


            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "prefixClash:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://brepipelineframework");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespaceWithPrefix vocabulary definition replaces the current namespace at the XML document root node with the specified prefix
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceAndPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespaceAndPrefix Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "bre:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://brepipelineframework");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespace vocabulary definition adds the current namespace at the XML document root node with the ns0 prefix if it doesn't currently exists
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceDoesNotExist()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespace Config.xml";


            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "ns0:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://brepipelineframework");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespaceWithPrefix vocabulary definition adds the current namespace at the XML document root node with the specified prefix if it doesn't currently exists
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceAndPrefixDoesNotExist()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespaceAndPrefix Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("name(/*)", "bre:Root");
            _XPathCollection.XPathQueryList.Add("namespace-uri(/*)", "http://brepipelineframework");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance, 1);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ResolvePartyName vocabulary definition is able to find a party name based on Alias Name/Qualifier/Value.
        ///Note:In order for this test to pass a party called BREPipelineFramework must be setup with a business identity containing an alias with a name of
        ///SourceControlRepository, an alias qualifier of TFS, and an alias value of Codeplex.
        ///</summary>
        [TestMethod()]
        public void Test_GetPartyNameFromAlias()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetPartyNameFromAlias Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='DestinationParty'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='BREPipelineFramework'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ResolvePartyName vocabulary definition returns a blank value when it is unable to find a party name based on Alias Name/Qualifier/Value
        ///and it is configured not to throw an exception
        ///</summary>
        [TestMethod()]
        public void Test_GetPartyNameFromAliasNotFoundNoException()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetPartyNameFromAliasNotFoundNoException Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='DestinationParty'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value=''])";
            string ExpectedValue = "True";


            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        ///</summary>
        ///Tests that the ResolvePartyName vocabulary definition throws an exception when it is unable to find a party name based on Alias Name/Qualifier/Value
        ///and it is configured to throw an exception
        ///</summary>
        [TestMethod()]
        public void Test_GetPartyNameFromAliasNotFoundException()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetPartyNameFromAliasNotFoundException Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='DestinationParty'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value=''])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test to fail");
            }
            catch (Exception e)
            {
                if (e.InnerException.Message != "Unable to locate a party with an alias name of SourceControlRepository, an alias qualifier of SVN, and an alias value of Codeplex.")
                {
                    Assert.Fail("Was expecting for the pipeline test to fail with the specific error - Unable to locate a party with an alias name of SourceControlRepository, an alias qualifier of SVN, and an alias value of Codeplex.... but instead got - " + e.InnerException.Message);
                }
            }
        }

        /// <summary>
        ///Tests that the FindReplaceStringInMessage vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_FindReplaceStringInMessage()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_FindReplaceStringInMessage Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_FindReplaceStringInMessage.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, testContextInstance, ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the FindReplaceRegexInMessage vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_FindReplaceRegexInMessage()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_FindReplaceRegexInMessage Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_FindReplaceRegexInMessage.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, testContextInstance, ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessageWithoutValidation vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageWithoutValidation()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageWithoutValidation Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageWithoutValidation.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, testContextInstance, ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessageWithoutValidation vocabulary definition fulfills it's function when chained
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageWithoutValidationTwice()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageWithoutValidationTwice Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageWithoutValidationTwice.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, testContextInstance, ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessage()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessage.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, "BREPipelineFramework.TestProject.Rcv_TransformMessage", ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fulfills it's function when chained
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageTwice()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageTwice.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, "BREPipelineFramework.TestProject.Rcv_TransformMessageTwice", ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fails if no message type exists on the message (should use TransformMessageWithoutValidation instead)
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageNoMessageType()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageNoMessageType Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessage.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, testContextInstance, ExpectedOutputFileName);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
            }
            catch (Exception e)
            {
                if (e.InnerException.Message != "Unable to read source messageType")
                {
                    Assert.Fail("Was expecting a no messageType error but instead got - " + e.InnerException.Message);
                }
            }
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fails if the message type doesn't match that of the map
        //////</summary>
        [TestMethod()]
        public void Test_TransformMessageMessageTypeMismatch()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message2.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessage.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, "BREPipelineFramework.TestProject.Rcv_TransformMessage", ExpectedOutputFileName, "BREPipelineFramework.TestProject.Message2");

            try
            {
                _BREPipelineFrameworkTest.RunTest();
            }
            catch (Exception e)
            {
                if (!e.InnerException.Message.StartsWith("Transformation mismatch exception for map"))
                {
                    Assert.Fail("Was expecting a transformation mismatch error but instead got - " + e.InnerException.Message);
                }
            }
        }
    }
}
