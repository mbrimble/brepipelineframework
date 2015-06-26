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
    public class MessagePartMetaInstructionsTests
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

        [TestMethod()]
        public void Test_MIME_PartNames()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\MIMEMessage.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_MIME_PartNames Config.xml";
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkMIME");
            _BREPipelineFrameworkTest.RunTest();

            string bodyName = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Body"].ToString();
            Assert.IsTrue(bodyName == "body", "Did not find the expected body part name in the message - " + bodyName);

            string index0 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index0"].ToString();
            Assert.IsTrue(index0 == "body", "Did not find the expected part name in the message at index 0 - " + index0);

            string index1 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index1"].ToString();
            Assert.IsTrue(index1 == "Attachment", "Did not find the expected part name in the message at index 1 - " + index1);

            string index2 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index2"].ToString();
            Assert.IsTrue(index2 == "Hello", "Did not find the expected part name in the message at index 2 - " + index2);

            string index3 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index3"].ToString();
            Assert.IsTrue(index3 == "What", "Did not find the expected part name in the message at index 3 - " + index3);
        }

        [TestMethod()]
        public void Test_MIME_GetContentType()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\MIMEMessage.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_MIME_GetContentType Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkMIME");
            _BREPipelineFrameworkTest.RunTest();

            string bodyName = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Body"].ToString();
            Assert.IsTrue(bodyName == "text/xml", "Did not find the expected body part content type in the message - " + bodyName);

            string index0 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index0"].ToString();
            Assert.IsTrue(index0 == "text/xml", "Did not find the expected part content type in the message at index 0 - " + index0);

            string index1 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index1"].ToString();
            Assert.IsTrue(index1 == "text/xml", "Did not find the expected part content type in the message at index 1 - " + index1);

            string index2 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index2"].ToString();
            Assert.IsTrue(index2 == "text/plain", "Did not find the expected part content type in the message at index 2 - " + index2);

            string index3 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index3"].ToString();
            Assert.IsTrue(index3 == "text/plain", "Did not find the expected part content type in the message at index 3 - " + index3);

            string nameBody = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameBody"].ToString();
            Assert.IsTrue(nameBody == "text/xml", "Did not find the expected part content type in the message in the part named body - " + nameBody);

            string nameAttachment = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameAttachment"].ToString();
            Assert.IsTrue(nameAttachment == "text/xml", "Did not find the expected part content type in the message in the part named Attachment - " + nameAttachment);

            string nameHello = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameHello"].ToString();
            Assert.IsTrue(nameHello == "text/plain", "Did not find the expected part content type in the message in the part named Hello - " + nameHello);

            string nameWhat = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameWhat"].ToString();
            Assert.IsTrue(nameWhat == "text/plain", "Did not find the expected part content type in the message in the part named What - " + nameWhat);
        }

        [TestMethod()]
        public void Test_MIME_GetPartCount()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\MIMEMessage.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_MIME_GetPartCount Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkMIME");
            _BREPipelineFrameworkTest.RunTest();

            string partCount = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["PartCount"].ToString();
            Assert.IsTrue(partCount == "4", "Did not find the expected number of parts in the message - " + partCount);
        }

        [TestMethod()]
        public void Test_MIME_GetCharSet()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\MIMEMessage.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_MIME_GetCharSet Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkMIME");
            _BREPipelineFrameworkTest.RunTest();

            string bodyName = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Body"].ToString();
            Assert.IsTrue(bodyName == "UTF-8", "Did not find the expected body part character set in the message - " + bodyName);

            string index0 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index0"].ToString();
            Assert.IsTrue(index0 == "UTF-8", "Did not find the expected part character set in the message at index 0 - " + index0);

            string index1 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index1"].ToString();
            Assert.IsTrue(index1 == "UTF-8", "Did not find the expected part character set in the message at index 1 - " + index1);

            string index2 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index2"].ToString();
            Assert.IsTrue(index2 == "", "Did not find the expected part character set in the message at index 2 - " + index2);

            string index3 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index3"].ToString();
            Assert.IsTrue(index3 == "", "Did not find the expected part character set in the message at index 3 - " + index3);

            string nameBody = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameBody"].ToString();
            Assert.IsTrue(nameBody == "UTF-8", "Did not find the expected part character set in the message in the part named body - " + nameBody);

            string nameAttachment = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameAttachment"].ToString();
            Assert.IsTrue(nameAttachment == "UTF-8", "Did not find the expected part character set in the message in the part named Attachment - " + nameAttachment);

            string nameHello = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameHello"].ToString();
            Assert.IsTrue(nameHello == "", "Did not find the expected part character set in the message in the part named Hello - " + nameHello);

            string nameWhat = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["NameWhat"].ToString();
            Assert.IsTrue(nameWhat == "", "Did not find the expected part character set in the message in the part named What - " + nameWhat);
        }

        [TestMethod()]
        public void Test_SetPartMIMEFileName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\MIMEMessage.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_SetPartMIMEFileName Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkMIME");
            _BREPipelineFrameworkTest.RunTest();

            string index0 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index0"].ToString();
            Assert.IsTrue(index0 == "Body.txt", "Did not find the expected part property in the message at index 0 - " + index0);

            string index1 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index1"].ToString();
            Assert.IsTrue(index1 == "Attachment.txt", "Did not find the expected part property in the message at index 1 - " + index1);

            string index2 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index2"].ToString();
            Assert.IsTrue(index2 == "Hello.txt", "Did not find the expected part property in the message at index 2 - " + index2);

            string index3 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Index3"].ToString();
            Assert.IsTrue(index3 == "What.txt", "Did not find the expected part property in the message at index 3 - " + index3);
        }

        [TestMethod()]
        public void Test_SetPartDetails()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\MIMEMessage.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_SetPartDetails Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, PipelineType: "BREPipelineFramework.TestProject.Rcv_BREPipelineFrameworkMIME");
            _BREPipelineFrameworkTest.RunTest();

            string item1 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["1"].ToString();
            Assert.IsTrue(item1 == "test", "Did not find the expected value in the cache for key 1 - " + item1);

            string item2 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["2"].ToString();
            Assert.IsTrue(item2 == "test", "Did not find the expected value in the cache for key 2 - " + item1);

            string item3 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["3"].ToString();
            Assert.IsTrue(item3 == "test", "Did not find the expected value in the cache for key 3 - " + item1);

            string item4 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["4"].ToString();
            Assert.IsTrue(item4 == "test", "Did not find the expected value in the cache for key 4 - " + item1);

            string item5 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["5"].ToString();
            Assert.IsTrue(item5 == "test", "Did not find the expected value in the cache for key 5 - " + item1);

            string item6 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["6"].ToString();
            Assert.IsTrue(item6 == "test", "Did not find the expected value in the cache for key 6 - " + item1);

            string item7 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["7"].ToString();
            Assert.IsTrue(item7 == "test", "Did not find the expected value in the cache for key 7 - " + item1);

            string item8 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["8"].ToString();
            Assert.IsTrue(item8 == "test", "Did not find the expected value in the cache for key 8 - " + item1);

            string item9 = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["9"].ToString();
            Assert.IsTrue(item9 == "test", "Did not find the expected value in the cache for key 9 - " + item1);

        }
    }
}
