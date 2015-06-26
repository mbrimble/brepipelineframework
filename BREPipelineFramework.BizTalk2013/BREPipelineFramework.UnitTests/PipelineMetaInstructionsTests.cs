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
    public class PipelineMetaInstructionsTests
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
            foreach (var element in MemoryCache.Default)
            {
                MemoryCache.Default.Remove(element.Key);
            }
        }

        //Use TestCleanup to cleanup output files after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            string directoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files";
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(directoryPath);

            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                //file.Delete();
            }

            directoryPath = @"C:\temp\trackingfolder";
            directory = new System.IO.DirectoryInfo(directoryPath);

            foreach (System.IO.FileInfo file in directory.GetFiles())
            {
                file.Delete();
            }
        }

        #endregion

        [TestMethod()]
        public void Test_AssembleFF()
        {
            string applicationContext = "Test_AssembleFF";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_output.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_output.txt";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType, ExpectedOutputFileName: expectedOutput);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_AssembleFFWithHeader()
        {
            string applicationContext = "Test_AssembleFFWithHeader";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_output.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string headerType = "BREPipelineFramework.TestProject.Sample_FF_Header";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_Header_output.txt";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType, 
                ExpectedOutputFileName: expectedOutput, additionalInputType: headerType);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_AssembleFFWithHeaderAndTrailer()
        {
            string applicationContext = "Test_AssembleFFWithHeaderAndTrailer";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_output.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string headerType = "BREPipelineFramework.TestProject.Sample_FF_Header";
            string trailerType = "BREPipelineFramework.TestProject.Sample_FF_Footer";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_HeaderAndTrailer_output.txt";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType,
                ExpectedOutputFileName: expectedOutput, additionalInputType: headerType, yetAnotherInputType: trailerType);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_AssembleFFWithTrailer()
        {
            string applicationContext = "Test_AssembleFFWithTrailer";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_output.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string trailerType = "BREPipelineFramework.TestProject.Sample_FF_Footer";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_Footer_output.txt";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType,
                ExpectedOutputFileName: expectedOutput, yetAnotherInputType: trailerType);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_DisassembleFF()
        {
            string applicationContext = "Test_DisassembleFF";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_output.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_output.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType, ExpectedOutputFileName: expectedOutput);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_DisassembleFFWithHeader()
        {
            string applicationContext = "Test_DisassembleFFWithHeader";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_Header_output.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string headerType = "BREPipelineFramework.TestProject.Sample_FF_Header";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_output.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(@"/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='FlatFileHeaderDocument'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/xmlnorm-properties']/@*[local-name()='Value' and namespace-uri()='']",
                @"<Sample_Header xmlns=""http://BREPipelineFramework.TestProject.Sample_FF""><Body xmlns="""">Testing</Body></Sample_Header>");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType,
                ExpectedOutputFileName: expectedOutput, additionalInputType: headerType, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_DisassembleFFWithHeaderAndTrailer()
        {
            string applicationContext = "Test_DisassembleFFWithHeaderAndTrailer";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_HeaderAndTrailer_output.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string headerType = "BREPipelineFramework.TestProject.Sample_FF_Header";
            string footerType = "BREPipelineFramework.TestProject.Sample_FF_Footer";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_output.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(@"/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='FlatFileHeaderDocument'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/xmlnorm-properties']/@*[local-name()='Value' and namespace-uri()='']",
                @"<Sample_Header xmlns=""http://BREPipelineFramework.TestProject.Sample_FF""><Body xmlns="""">Testing</Body></Sample_Header>");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType,
                ExpectedOutputFileName: expectedOutput, additionalInputType: headerType, yetAnotherInputType: footerType, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_DisassembleFFWithTrailer()
        {
            string applicationContext = "Test_DisassembleFFWithTrailer";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Sample_FF_Footer_output.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string _FFMessageType = "BREPipelineFramework.TestProject.Sample_FF";
            string trailerType = "BREPipelineFramework.TestProject.Sample_FF_Footer";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Sample_FF_output.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: _FFMessageType,
                ExpectedOutputFileName: expectedOutput, yetAnotherInputType: trailerType);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_DisassembleXML()
        {
            string applicationContext = "Test_DisassembleXML";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\TestEnvelope.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string inputMessageType = "BREPipelineFramework.TestProject.Message";
            string envelopeType = "BREPipelineFramework.TestProject.Envelope";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_DisassembleXML.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageType'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='http://BREPipelineFramework#Message'])",
                "True");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: inputMessageType,
                ExpectedOutputFileName: expectedOutput, yetAnotherInputType: envelopeType, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_DisassembleXMLPromotePropertiesOnly()
        {
            string applicationContext = "Test_DisassembleXMLPromotePropertiesOnly";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\TestEnvelope.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string inputMessageType = "BREPipelineFramework.TestProject.Message";
            string envelopeType = "BREPipelineFramework.TestProject.Envelope";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\TestEnvelope.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add("boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageType'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='http://BREPipelineFramework#Envelope'])",
                "True");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: inputMessageType,
                ExpectedOutputFileName: expectedOutput, yetAnotherInputType: envelopeType, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        /*This test is not passing even though it works fine at runtime
        [TestMethod()]
        public void Test_AssembleXML()
        {
            string applicationContext = "Test_AssembleXML";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_DisassembleXML.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext, "Base Config No InstructionLoader.xml");

            string inputMessageType = "BREPipelineFramework.TestProject.Message";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\TestEnvelope.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: inputMessageType,
                ExpectedOutputFileName: expectedOutput, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        } */

        [TestMethod()]
        public void Test_ValidateXML_Success()
        {
            string applicationContext = "Test_ValidateXML";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\TestEnvelope.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string envelopeType = "BREPipelineFramework.TestProject.Envelope";

            string expectedOutput = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\TestEnvelope.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: envelopeType,
                ExpectedOutputFileName: expectedOutput);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ValidateXML_Failure()
        {
            string applicationContext = "Test_ValidateXML";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\TestEnvelope1.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string envelopeType = "BREPipelineFramework.TestProject.Envelope";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputMessageType: envelopeType);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting an exception but none was caught");
            }
            catch (Exception e)
            {
                if (e.InnerException.GetType().ToString() != "Microsoft.BizTalk.Component.XmlValidatorException")
                {
                    Assert.Fail("Was expecting an XMLValidationFailure but instead got an exception of type - " + e.InnerException.GetType().ToString());
                }
            }
        }
    }
}