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
    public class CustomMetaInstructionsTests
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

        const string numberOfPropertiesXPath = "/*[local-name()='MessageInfo' and namespace-uri()='']/*[local-name()='ContextInfo' and namespace-uri()='']/@*[local-name()='PropertiesCount' and namespace-uri()='']";

        /// <summary>
        ///Tests that custom MetaInstructions can be dynamically loaded using Instruction Loader Policies and then exercised in instruction loader policies.
        ///</summary>
        [TestMethod()]
        public void Test_CustomMetaInstruction()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_CustomMetaInstruction Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Element'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='Test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }
    }
}
