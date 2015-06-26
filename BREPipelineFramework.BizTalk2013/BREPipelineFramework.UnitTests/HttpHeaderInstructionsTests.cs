using BizUnit.TestSteps.DataLoaders.File;
using BizUnit.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BREPipelineFramework.UnitTests
{
    [TestClass]
    public class HttpHeaderInstructionsTests
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

        [TestMethod()]
        public void Test_HTTPHeaderManipulation()
        {
            string applicationContext = "Test_HTTPHeaderManipulation";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\HTTPHeaders_Input.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='HttpHeaders'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties']/@*[local-name()='Value' and namespace-uri()='']";
            string ExpectedValue = "Test: overwrite" + Environment.NewLine + "Important_User: True" + Environment.NewLine + "whoah: help" + Environment.NewLine;

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, InputContextFileName: InputContextFileName, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }
    }
}
