using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace BREPipelineFramework.UnitTests
{
    [TestClass()]
    public class JSONMetaInstructionsTests
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
        public void Test_RcvJSONMessageWithContentTypeSpecifiedAsJSON()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\SampleJSON.txt";
            string InputContextFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\ContentTypeJSON.xml";
            string ExpectedOutputFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\JSONConvertedToXML.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_API", InputContextFileName: InputContextFile, ExpectedOutputFileName: ExpectedOutputFile);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["BRE Pipeline Framework Accept Header Caching {3C66F687-FD72-46F7-84FC-6CD9CDFB8B5D}"].ToString();
            Assert.IsTrue(propertyValue == "application/json", "Did not find the expected HTTP accept header value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_RcvJSONMessageWithContentTypeSpecifiedAsXML()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\SampleJSON.txt";
            string InputContextFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\ContentTypeXML.xml";
            string ExpectedOutputFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\SampleJSON.txt";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_API", InputContextFileName: InputContextFile, ExpectedOutputFileName: ExpectedOutputFile);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["BRE Pipeline Framework Accept Header Caching {3C66F687-FD72-46F7-84FC-6CD9CDFB8B5D}"].ToString();
            Assert.IsTrue(propertyValue == "application/xml", "Did not find the expected HTTP accept header value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_RcvJSONMessageWithNoContentTypeSpecifiedNoAcceptSpecified()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\SampleJSON.txt";
            string InputContextFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\ContentTypeNotSpecified.xml";
            string ExpectedOutputFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\JSONConvertedToXML.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_API", InputContextFileName: InputContextFile, ExpectedOutputFileName: ExpectedOutputFile);
            _BREPipelineFrameworkTest.RunTest();

            object propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["BRE Pipeline Framework Accept Header Caching {3C66F687-FD72-46F7-84FC-6CD9CDFB8B5D}"];
            Assert.IsTrue(propertyValue == null, "Did not find the expected HTTP accept header value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_RcvJSONMessageWithNoContentTypeSpecifiedWithWhitespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\SampleJSONWithLeadingWhitespace.txt";
            string InputContextFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\ContentTypeNotSpecified.xml";
            string ExpectedOutputFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\JSONConvertedToXML.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Rcv_API", InputContextFileName: InputContextFile, ExpectedOutputFileName: ExpectedOutputFile);
            _BREPipelineFrameworkTest.RunTest();

            object propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["BRE Pipeline Framework Accept Header Caching {3C66F687-FD72-46F7-84FC-6CD9CDFB8B5D}"];
            Assert.IsTrue(propertyValue == null, "Did not find the expected HTTP accept header value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_SndXmlMessageWithCachedJSONAcceptHeader()
        {
            BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["BRE Pipeline Framework Accept Header Caching {3C66F687-FD72-46F7-84FC-6CD9CDFB8B5D}"] = "application/json";

            string InputFileName = "JSONConvertedToXML.xml";
            string InputContextFile = "ContentTypeNotSpecified.xml";
            string ExpectedOutputFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\XMLConvertedToJSONWithRootStripped.txt";

            XPathCollection contextXPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Snd_API", InputContextFileName: InputContextFile, contextXPathCollection: contextXPathCollection, ExpectedOutputFileName: ExpectedOutputFile);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_SndXmlMessageWithCachedJSONAcceptHeaderWithRootNodeNotStrippedOut()
        {
            BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["BRE Pipeline Framework Accept Header Caching {3C66F687-FD72-46F7-84FC-6CD9CDFB8B5D}"] = "application/json";

            string InputFileName = "JSONConvertedToXML.xml";
            string InputContextFile = "ContentTypeNotSpecified.xml";
            string ExpectedOutputFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\XMLConvertedToJSONWithRootNotStripped.txt";

            XPathCollection contextXPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Snd_API2", InputContextFileName: InputContextFile, contextXPathCollection: contextXPathCollection, ExpectedOutputFileName: ExpectedOutputFile);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_SndXmlMessageNoCachedAcceptHeader()
        {
            string InputFileName = "JSONConvertedToXML.xml";
            string InputContextFile = "ContentTypeNotSpecified.xml";
            string ExpectedOutputFile = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\JSONConvertedToXML.xml";

            XPathCollection contextXPathCollection = new XPathCollection();

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, testContextInstance, PipelineType: "BREPipelineFramework.TestProject.Snd_API2", InputContextFileName: InputContextFile, contextXPathCollection: contextXPathCollection, ExpectedOutputFileName: ExpectedOutputFile);
            _BREPipelineFrameworkTest.RunTest();
        }
    }
}
