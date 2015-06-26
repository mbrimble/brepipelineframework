using BREPipelineFramework.SampleInstructions.MetaInstructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BREPipelineFramework.SampleInstructions;
using BREPipelineFramework.Helpers;
using b = BizUnit;
using System.Collections.Generic;
using System.Runtime.Caching;
namespace BREPipelineFramework.UnitTests
{
    [TestClass]
    public class TypedXMLDocumentMetaInstructionsTests
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

        /// <summary>
        ///Tests that the AddAttribute vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_AddAttribute()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddAttribute Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddAttribute.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNode vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_AddNode()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNode Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNode.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNodeIfNotThere vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_AddNodeIfNotThere()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNodeIfNotThere Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNodeIfNotThere.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNodeIfNotThere vocabulary definition doesn't do anything if the node already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddNodeIfNotThere_AlreadyThere()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_AddNodeIfNotThere.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNodeIfNotThere Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNodeIfNotThere.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNodeWithNamespace vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_AddNodeWithNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNodeWithNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNodeWithNamespace.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNodeWithNamespaceAndValue vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_AddNodeWithNamespaceAndValue()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNodeWithNamespaceAndValue Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNodeWithNamespaceAndValue.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNodeWithNamespaceIfNotThere vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_AddNodeWithNamespaceIfNotThere()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNodeWithNamespaceIfNotThere Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNodeWithNamespaceIfNotThere.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNodeWithNamespaceIfNotThere vocabulary definition doesn't do anything if the node already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddNodeWithNamespaceIfNotThere_AlreadyThere()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_AddNodeWithNamespaceIfNotThere.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNodeWithNamespaceIfNotThere Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNodeWithNamespaceIfNotThere.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddNodeWithValue vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_AddNodeWithValue()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddNodeWithValue Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddNodeWithValue.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ApplyTypedXmlDocument vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_XmlFactsApplicationStage_Explicit()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_XmlFactsApplicationStage_Explicit Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_XmlFactsApplicationStage_Explicit.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ApplyTypedXmlDocument vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_XmlFactsApplicationStage_BeforeInstructionExecution()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_XmlFactsApplicationStage_BeforeInstructionExecution Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_XmlFactsApplicationStage_BeforeInstructionExecution.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }


        /// <summary>
        ///Tests that the ApplyTypedXmlDocument vocabulary definition fulfills it's function
        ///</summary>
        [TestMethod()]
        public void Test_XmlFactsApplicationStage_AfterInstructionExecution()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_XmlFactsApplicationStage_AfterInstructionExecution Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_XmlFactsApplicationStage_AfterInstructionExecution.xml";

            XPathCollection _XPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

    }
}
