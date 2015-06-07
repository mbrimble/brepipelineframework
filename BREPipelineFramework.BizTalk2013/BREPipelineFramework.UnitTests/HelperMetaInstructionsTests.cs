using BREPipelineFramework.SampleInstructions.MetaInstructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BREPipelineFramework.SampleInstructions;
using BREPipelineFramework.Helpers;
using b = BizUnit;
using System.Collections.Generic;
using BizUnit.Xaml;
using System.Runtime.Caching;
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

        //Use TestInitialize to clear cache before each test runs
        [TestInitialize()]
        public void MyTestSetup()
        {
            var oldCache = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache;
            BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache = MemoryCache.Default;
            oldCache.Dispose();
        }

        //Use TestCleanup to cleanup output files after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            string directoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files";
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(directoryPath);

            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }

            directoryPath = @"C:\temp\trackingfolder";
            directory = new System.IO.DirectoryInfo(directoryPath);

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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ReturnRegexMatchByIndex()
        {
            string applicationContext = "Test_ReturnRegexMatchByIndex";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == ">BREPipelineFramework ExecutionPolicy<", "Did not find the expected regex match in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_ReturnFirstRegexMatch()
        {
            string applicationContext = "Test_ReturnFirstRegexMatch";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == ">true<", "Did not find the expected regex match in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_ReturnRegexMatchByIndex_NotFound()
        {
            string applicationContext = "Test_ReturnRegexMatchByIndex";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "", "Did not find the expected regex match in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_ReturnFirstRegexMatch_NotFound()
        {
            string applicationContext = "Test_ReturnFirstRegexMatch";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "", "Did not find the expected regex match in the message - " + propertyValue);
        }


        [TestMethod()]
        public void Test_CheckIfRegexExistsInMessage_True()
        {
            string applicationContext = "Test_CheckIfRegexExistsInMessage_True";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_CheckIfRegexExistsInMessage_False()
        {
            string applicationContext = "Test_CheckIfRegexExistsInMessage_False";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_CheckIfStringExistsInMessage_True()
        {
            string applicationContext = "Test_CheckIfStringExistsInMessage_True";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_CheckIfStringExistsInMessage_False()
        {
            string applicationContext = "Test_CheckIfStringExistsInMessage_False";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_MessageBodyLength()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\TestCount.txt";
            string applicationContext = "Test_MessageBodyLength";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["TestKey"].ToString();
            Assert.IsTrue(propertyValue == "20", "Did not find the expected message length in the message - " + propertyValue);
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


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_GetXPathResult_Name()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_GetXPathResult_Name");
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='ApplicationContext'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_GetXPathResult_Namespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_GetXPathResult_Namespace");
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='http://www.w3.org/2001/XMLSchema-instance'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_GetXPathResult_NotFound_Exception()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_GetXPathResult_NotFound_Exception");
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='Test_Set_BTS_DestinationParty'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting the test to result in an exception but none was raised");
            }
            catch (Exception e)
            {
                if (e.GetBaseException().Message.Contains("No result found for XPath query"))
                {

                }
                else
                {
                    Assert.Fail("Unexpected exception was encountered - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_GetXPathResult_NotFound_Ignore()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_GetXPathResult_NotFound_Ignore");
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value=''])";
            string ExpectedValue = "False";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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
        public void Test_AddDocumentNamespace_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespace_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespaceWithPrefix vocabulary definition adds the namespace to the XML document root node with the specified prefix
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceAndPrefix_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespaceAndPrefix_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespaceAndPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespace vocabulary definition doesn't add the namespace to the XML document root node with the ns0 prefix if a root namespace already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceExistingNamespace_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespace_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespaceExistingNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespaceWithPrefix vocabulary definition doesn't add the namespace to the XML document root node with the specified prefix if a root namespace already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceAndPrefixExistingNamespace_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespaceAndPrefix_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespaceAndPrefixExistingNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespace vocabulary definition replaces the current namespace at the XML document root node with the ns0 prefix
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespace_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespace_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespace.xml";
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespaceWithPrefix vocabulary definition replaces the current namespace at the XML document root node with the specified prefix
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceAndPrefix_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespaceAndPrefix_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespaceAndPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespace vocabulary definition adds the current namespace at the XML document root node with the ns0 prefix if it doesn't currently exists
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceDoesNotExist_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespace_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespaceDoesNotExist.xml";


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespaceWithPrefix vocabulary definition adds the current namespace at the XML document root node with the specified prefix if it doesn't currently exists
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceAndPrefixDoesNotExist_Deprecated()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespaceAndPrefix_Deprecated Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespaceAndPrefixDoesNotExist.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessageWithoutValidation vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageVaildateSourceIfKnownWithDisassemble()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageVaildateSourceIfKnown Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageVaildateSourceIfKnown.xml";

            string message2Type = "BREPipelineFramework.TestProject.Message2";

            XPathCollection _XPathCollection = new XPathCollection();
            string XPathQuery1 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageType'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='http://BREPipelineFramework.TestProject.Message2#Message2'])";
            string ExpectedValue1 = "True";
            _XPathCollection.XPathQueryList.Add(XPathQuery1, ExpectedValue1);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection, ExpectedOutputFileName:ExpectedOutputFileName, additionalInputType: message2Type);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessageWithoutValidation vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageVaildateSourceIfKnown()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageVaildateSourceIfKnown Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageVaildateSourceIfKnown.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            string XPathQuery1 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageType'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='http://BREPipelineFramework.TestProject.Message2#Message2'])";
            string ExpectedValue1 = "True";
            _XPathCollection.XPathQueryList.Add(XPathQuery1, ExpectedValue1);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessageWithoutValidation vocabulary definition fulfills it's function when chained
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageVaildateSourceIfKnownTwice()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageVaildateSourceIfKnownTwice Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageVaildateSourceIfKnownTwice.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessageWithoutValidation vocabulary definition fulfills it's function when chained
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageVaildateSourceIfKnownTwiceWithDisassemble()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageVaildateSourceIfKnownTwice Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageVaildateSourceIfKnownTwice.xml";

            string message2Type = "BREPipelineFramework.TestProject.Message2";
            string message3Type = "BREPipelineFramework.TestProject.Message3";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName, additionalInputType: message2Type, yetAnotherInputType: message3Type);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType:"BREPipelineFramework.TestProject.Rcv_TransformMessage", ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageWithDisassemble()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessage.xml";

            string message2Type = "BREPipelineFramework.TestProject.Message2";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_TransformMessage", ExpectedOutputFileName: ExpectedOutputFileName, additionalInputType: message2Type);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType:"BREPipelineFramework.TestProject.Rcv_TransformMessageTwice", ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fulfills it's function when chained
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageTwiceWithDisassembles()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessageTwice.xml";

            string message2Type = "BREPipelineFramework.TestProject.Message2";
            string message3Type = "BREPipelineFramework.TestProject.Message3";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_TransformMessageTwice", ExpectedOutputFileName: ExpectedOutputFileName, additionalInputType: message2Type, yetAnotherInputType: message3Type);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fails if no message type exists on the message and validation is required
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageNoMessageType()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TransformMessageNoMessageType Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessage.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
            }
            catch (Exception e)
            {
                if (!e.InnerException.Message.Contains("Unable to read source messageType while performing transformation"))
                {
                    Assert.Fail("Was expecting a no messageType error but instead got - " + e.InnerException.Message);
                }
            }
        }

        /// <summary>
        ///Tests that the TransformMessage vocabulary definition fails if the message type doesn't match that of the map
        ///</summary>
        [TestMethod()]
        public void Test_TransformMessageMessageTypeMismatch()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message2.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_TransformMessage.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType:"BREPipelineFramework.TestProject.Rcv_TransformMessage", ExpectedOutputFileName:ExpectedOutputFileName, inputMessageType:"BREPipelineFramework.TestProject.Message2");

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
            }
            catch (Exception e)
            {
                if (!e.InnerException.Message.StartsWith("Transformation mismatch exception for map"))
                {
                    Assert.Fail("Was expecting a transformation mismatch error but instead got - " + e.InnerException.Message);
                }
            }
        }

        [TestMethod()]
        public void Test_ConcatenateMultipleStrings()
        {
            string applicationContext = "Test_ConcatenateMultipleStrings";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string cacheItem2 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["2"].ToString();
            string cacheItem3 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["3"].ToString();
            string cacheItem4 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["4"].ToString();
            string cacheItem5 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["5"].ToString();
            string cacheItem6 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["6"].ToString();

            Assert.IsTrue(cacheItem2 == "12", "Unexpected concatenated value found in cache - " + cacheItem2);
            Assert.IsTrue(cacheItem3 == "123", "Unexpected concatenated value found in cache - " + cacheItem3);
            Assert.IsTrue(cacheItem4 == "1234", "Unexpected concatenated value found in cache - " + cacheItem4);
            Assert.IsTrue(cacheItem5 == "12345", "Unexpected concatenated value found in cache - " + cacheItem5);
            Assert.IsTrue(cacheItem6 == "123456", "Unexpected concatenated value found in cache - " + cacheItem6);
        }
    }
}
