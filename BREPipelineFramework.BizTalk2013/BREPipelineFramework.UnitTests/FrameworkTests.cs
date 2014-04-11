using BREPipelineFramework.SampleInstructions.MetaInstructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BREPipelineFramework.SampleInstructions;
using BREPipelineFramework.Helpers;
using b = BizUnit;
using System.Collections.Generic;

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

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
            string directoryPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Output Files";
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(directoryPath);

            foreach(System.IO.FileInfo file in directory.GetFiles())
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
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting for the pipeline test to fail");
            }
            catch (Exception e)
            {
                if (e.InnerException.Message != "Duplicate throw exception helper worked as expected")
                {
                    Assert.Fail("Was expecting for the pipeline test to fail with the specific error - Duplicate throw exception helper worked as expected, but instead got - " + e.InnerException.Message);
                }
            }
        }
    }
}
