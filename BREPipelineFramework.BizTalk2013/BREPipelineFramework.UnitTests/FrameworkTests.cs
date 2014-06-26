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
    [TestClass()]
    public class FrameworkTests
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
            BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache = new MemoryCache("BREPipelineFramework.Cache", null);
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
        ///Tests that setting the BTS.DestinationParty context property results in the property being successfully written to the context of a message in a receive pipeline
        ///Note that this test promotes the property and expects the output config file to have a promoted attribute of true
        ///This test uses an instruction loader policy that specifies an out of the box MetaInstruction and the aim of the test is to prove that the framework can handle it.
        ///</summary>
        [TestMethod()]
        public void Test_DuplicateContextMetaInstructions()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_DuplicateContextMetaInstructions Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='DestinationParty'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        ///Tests that the ThrowException vocabulary definition throws an exception
        ///This test uses an instruction loader policy that specifies an out of the box MetaInstruction and the aim of the test is to prove that the framework can handle it.
        ///</summary>
        [TestMethod()]
        public void Test_DuplicateHelperMetaInstructions()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_DuplicateHelperMetaInstructions Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='10'])";
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
                if (e.InnerException.Message != "Duplicate throw exception helper worked as expected")
                {
                    Assert.Fail("Was expecting for the pipeline test to fail with the specific error - Duplicate throw exception helper worked as expected, but instead got - " + e.InnerException.Message);
                }
            }
        }

        ///Tests that having no application context does not result in an error
        ///</summary>
        [TestMethod()]
        public void Test_NoApplicationContext()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_NoApplicationContext Config.xml";
            
            XPathCollection _XPathCollection = new XPathCollection();
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        ///Tests that having an invalid tracking folder directory results in an error
        ///</summary>
        [TestMethod()]
        public void Test_TrackingFolder_Bad()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TrackingFolder_Bad Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
            }
            catch (Exception e)
            {
                if (!e.GetBaseException().Message.Contains("does not resolve to a valid folder location"))
                {
                    Assert.Fail("Was expecting an exception due to a malformed directory but instead got the following exception - " + e.GetBaseException().Message);
                }
            }
        }

        ///Tests that having a tracking folder directory that doesn't exist results in an error
        ///</summary>
        [TestMethod()]
        public void Test_TrackingFolder_NotExist()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TrackingFolder_NotExist Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
            }
            catch (Exception e)
            {
                if (!e.GetBaseException().Message.Contains("does not resolve to a valid folder location"))
                {
                    Assert.Fail("Was expecting an exception due to a nonexistant directory but instead got the following exception - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_BadMetaInstruction()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_BadMetaInstruction Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
            }
            catch (Exception e)
            {
                if (!e.GetBaseException().Message.Contains("Unable to instantiate MetaInstruction"))
                {
                    Assert.Fail("Was expecting an exception due to a bad metainstruction but instead got the following exception - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_WrongTypeMetaInstruction()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_WrongTypeMetaInstruction Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
            }
            catch (Exception e)
            {
                if (!e.GetBaseException().Message.Contains("Unable to instantiate MetaInstruction"))
                {
                    Assert.Fail("Was expecting an exception due to a bad metainstruction but instead got the following exception - " + e.GetBaseException().Message);
                }
            }
        }

        ///Tests that exceptions encountered during policy execution will be handled property
        ///</summary>
        [TestMethod()]
        public void Test_UncaughtPolicyException()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_UncaughtPolicyException Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test step to fail");
            }
            catch (Exception e)
            {
                if (!e.GetBaseException().Message.Contains("Forced exception"))
                {
                    Assert.Fail("Was expecting an exception due to a nonexistant directory but instead got the following exception - " + e.GetBaseException().Message);
                }
            }
        }

        ///Tests that exceptions encountered during policy execution will be handled property with RIP enabled
        ///</summary>
        [TestMethod()]
        public void Test_UncaughtPolicyExceptionRIP()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_UncaughtPolicyExceptionRIP Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            string XPathQuery1 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageDestination'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='SuspendQueue'])";
            string XPathQuery2 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='SuspendMessageOnRoutingFailure'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='True'])";
            string ExpectedValue = "True";
            _XPathCollection.XPathQueryList.Add(XPathQuery1, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(XPathQuery2, ExpectedValue);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();        
        }

        ///Tests that exceptions encountered during instruction execution will be handled properly with RIP enabled and that the message body and context will be rolled back
        ///</summary>
        [TestMethod()]
        public void Test_UncaughtPolicyExceptionAfterUpdate()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            string XPathQuery1 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageDestination'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='SuspendQueue'])";
            string XPathQuery2 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='SuspendMessageOnRoutingFailure'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='True'])";
            string XPathQuery3 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageType'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='http://BREPipelineFramework#Message'])";
            string XPathQuery4 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Element'][@Promoted='true'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='Test'])";
            string XPathQuery5 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Password'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/ftp-properties'])";
            string ExpectedValue = "True";
            _XPathCollection.XPathQueryList.Add(XPathQuery1, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(XPathQuery2, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(XPathQuery3, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(XPathQuery4, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(XPathQuery5, "False");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, contextXPathCollection: _XPathCollection, ExpectedOutputFileName: InputFileName, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkXMLRIP");
            _BREPipelineFrameworkTest.RunTest();
        }

        ///Tests that having a valid tracking folder results in a tracking file being written out
        ///</summary>
        [TestMethod()]
        public void Test_TrackingFolder()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_TrackingFolder Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = new b.Xaml.TestCase();

            var pipelineTestStep = new BREPipelineFramework.CustomBizUnitTestSteps.ExecuteReceivePiplineWithNullablePropertyStep
            {
                PipelineAssemblyPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll",
                PipelineTypeName = "BREPipelineFramework.TestProject.Rcv_BREPipelineFramework",
                Source = InputFileName,
                DestinationDir = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                DestinationFileFormat = "Output {0}.xml",
                OutputContextFileFormat = "Context {0}.xml",
                InstanceConfigFile = InstanceConfigFilePath
            };

            var docSpecDefinition1 = new b.TestSteps.BizTalk.Pipeline.DocSpecDefinition();

            docSpecDefinition1.AssemblyPath = @"..\..\..\BREPipelineFramework.TestProject\bin\debug\BREPipelineFramework.TestProject.dll";
            docSpecDefinition1.TypeName = "BREPipelineFramework.TestProject.Message";

            pipelineTestStep.DocSpecs.Add(docSpecDefinition1);

            _BREPipelineFrameworkTest.ExecutionSteps.Add(pipelineTestStep);

            var fileReadMultipleStep = new b.TestSteps.File.FileReadMultipleStep
            {
                ExpectedNumberOfFiles = 2,
                DeleteFiles = true,
                DirectoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files",
                SearchPattern = "*.xml",
                Timeout = 5000
            };

            _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStep);

            var fileReadMultipleStepTracking = new b.TestSteps.File.FileReadMultipleStep
            {
                ExpectedNumberOfFiles = 2,
                DeleteFiles = true,
                DirectoryPath = @"c:\temp\trackingfolder",
                SearchPattern = "*.txt",
                Timeout = 5000
            };

            _BREPipelineFrameworkTest.ExecutionSteps.Add(fileReadMultipleStepTracking);

            var bizUnit = new b.BizUnit(_BREPipelineFrameworkTest);
            bizUnit.RunTest();
        }

        [TestMethod()]
        public void Test_SQLConnection()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_SQLConnection Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ProxyName'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/http-properties'][@Value='hello'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_SQLConnectionFromSSO()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_SQLConnectionFromSSO Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ProxyName'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/http-properties'][@Value='hello'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_OverrideMe()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_OverrideMe Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNode.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_OverrideXmlFactsApplicationStage()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_OverrideXmlFactsApplicationStage Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_XmlFactsApplicationStage_Explicit.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ExplicitPolicyVersions()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ExplicitPolicyVersions Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ExplicitPolicyVersions.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that when the enabled property on the pipeline component is set to false no actions are taken even if the ApplicationContext specified would normally result 
        ///in some action being taken
        ///</summary>
        [TestMethod()]
        public void Test_NotEnabled()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_NotEnabled Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "0");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Compensate()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddCustomContextPropertyToCache Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            object cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#TransmitWorkID = 123"];

            if (cacheItemsObj != null)
            {
                Dictionary<string, object> cacheItems = (Dictionary<string, object>)cacheItemsObj;
                object _SMTPFromObj = cacheItems["http://schemas.microsoft.com/BizTalk/2003/smtp-properties#From"];

                if (_SMTPFromObj != null)
                {
                    Assert.IsTrue(_SMTPFromObj.ToString() == "jcooper1982@aitm.com", "Unexpected SMTP.From value of " + _SMTPFromObj.ToString() + " found in cache.");
                }
                else
                {
                    Assert.Fail("Cache did not contain a value for SMTP.From");
                }
            }
            else
            {
                Assert.Fail("Cache did not contain a context property dictionary from BTS.TransmitWorkID of 123.");
            }

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Compensate Config.xml";

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting the pipeline test to thrown an exception but it didn't");
            }
            catch (Exception)
            {
                cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#TransmitWorkID = 123"];

                if (cacheItemsObj != null)
                {
                    Dictionary<string, object> cacheItems = (Dictionary<string, object>)cacheItemsObj;
                    object _SMTPFromObj = cacheItems["http://schemas.microsoft.com/BizTalk/2003/smtp-properties#From"];

                    if (_SMTPFromObj != null)
                    {
                        Assert.IsTrue(_SMTPFromObj.ToString() == "jcooper1982@aitm.com", "Unexpected SMTP.From value of " + _SMTPFromObj.ToString() + " found in cache.");
                    }
                    else
                    {
                        Assert.Fail("Cache did not contain a value for SMTP.From");
                    }
                }
                else
                {
                    Assert.Fail("Cache did not contain a context property dictionary from BTS.TransmitWorkID of 123, compensation must have failed.");
                }
            }
        }

        [TestMethod()]
        public void Test_NullifyMessage()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_NullifyMessage Config.xml";
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedNumberOfFiles:0);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_NullifyMessageWithTypedXMLDocument()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_NullifyMessageWithTypedXMLDocument Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_XmlFactsApplicationStage_AfterInstructionExecution.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_XMLDisassemblerProperties()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message.xml";
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkXML");
            _BREPipelineFrameworkTest.RunTest();

            object propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["TestKey"];

            if (propertyValue == null)
            {
                Assert.Fail("Expected context property was not found");
            }
            else
            {
                Assert.IsTrue(propertyValue.ToString() == "2", "Did not find the expected context property value in the message - " + propertyValue);
            }
        }

        [TestMethod()]
        public void Test_XMLDisassemblerPropertiesNoStream()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message.xml";

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Element'][@Promoted='true'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='2'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, 
                PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkXML_NoReadStream", contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();            
        }

        [TestMethod()]
        public void Test_FFDisassemblerProperties()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\bretest.txt";
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_BRETestFF",
                inputMessageType: "BREPipelineFramework.TestProject.BRETest");
            _BREPipelineFrameworkTest.RunTest();

            object propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["TestKey"];

            if (propertyValue == null)
            {
                Assert.Fail("Expected context property was not found");
            }
            else
            {
                Assert.IsTrue(propertyValue.ToString() == "Today", "Did not find the expected context property value in the message - " + propertyValue);
            }
        }

        [TestMethod()]
        public void Test_FFDisassemblerPropertiesNoStream()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\bretest.txt";

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Element'][@Promoted='true'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='Today'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance,
                PipelineType: "BREPipelineFramework.TestProject.Rcv_BRETestFFNoStream", contextXPathCollection: _XPathCollection, inputMessageType: "BREPipelineFramework.TestProject.BRETest");
            _BREPipelineFrameworkTest.RunTest();
        }
    }
}
