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
    public class ContextMetaInstructionsTest
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
        ///Tests that setting the FILE.ReceivedFileName context property results in the property being successfully written to the context of a message in a receive pipeline
        ///Note that this test only writes the property and expects the output config file to have a promoted attribute of false
        ///</summary>
        [TestMethod()]
        public void Test_Set_FILE_ReceivedFileName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_FILE_ReceivedFileName Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ReceivedFileName'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/file-properties'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the FILE.ReceivedFileName context property results in the property being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_FILE_ReceivedFileName()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_FILE_ReceivedFileName Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }


        /// <summary>
        ///Tests that setting the BTS.DestinationParty context property results in the property being successfully written to the context of a message in a receive pipeline
        ///Note that this test promotes the property and expects the output config file to have a promoted attribute of true
        ///</summary>
        [TestMethod()]
        public void Test_Set_BTS_DestinationParty()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_BTS_DestinationParty Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='DestinationParty'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the BTS.DestinationParty context property results in the property being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_BTS_DestinationParty()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_BTS_DestinationParty Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the WCF.Action context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_WCF_Action()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_WCF_Action Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Action'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the WCF.Action context property results in the property being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_WCF_Action()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_WCF_Action Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SQL.ConnectionString context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_SQL_ConnectionString()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_SQL_ConnectionString Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ConnectionString'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/sql-properties'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SQL.ConnectionString context property results in the property being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_SQL_ConnectionString()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_SQL_ConnectionString Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the XMLNORM.PreserveBom context property results in the property being successfully written to the context of a message in a receive pipeline
        ///Note that this test is setting a boolean context property
        ///</summary>
        [TestMethod()]
        public void Test_Set_XMLNORM_PreserveBom()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_XMLNORM_PreserveBom Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='PreserveBom'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/xmlnorm-properties'][@Value='false'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the XMLNORM.PreserveBom context property results in the property being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_XMLNORM_PreserveBom()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_XMLNORM_PreserveBom Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the WSS.Url context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_WSS_Url()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_WSS_Url Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Url'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2006/WindowsSharePointServices-properties'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the WSS.Url context property results in the property being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_WSS_Url()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_WSS_Url Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SMTP.From context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_SMTP_From()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_SMTP_From Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SMTP.From context property results in the property being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_SMTP_From()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_SMTP_From Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting multiple context property results in the properties being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_Multiple()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_Multiple Config.xml";
            string XPathQuery1 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ConnectionString'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/sql-properties'][@Value='ApplicationContext'])";
            string ExpectedValue1 = "True";
            string XPathQuery2 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Url'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2006/WindowsSharePointServices-properties'][@Value='Test_Set_BTS_DestinationParty'])";
            string ExpectedValue2 = "True";
            string XPathQuery3 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Action'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties'][@Value='http://www.w3.org/2001/XMLSchema-instance'])";
            string ExpectedValue3 = "True";
            string XPathQuery4 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ReceivedFileName'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/file-properties'][@Value='TestSSO'])";
            string ExpectedValue4 = "True";
            string XPathQuery5 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Description'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2005/error-report'][@Value='Description'])";
            string ExpectedValue5 = "True";
            string XPathQuery6 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='UNB11'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2006/edi-properties'][@Value='UNB11'])";
            string ExpectedValue6 = "True";
            string XPathQuery7 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Username'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/http-properties'][@Value='Username'])";
            string ExpectedValue7 = "True";
            string XPathQuery8 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='DestinationParty'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='DestinationParty'])";
            string ExpectedValue8 = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery1, ExpectedValue1);
            _XPathCollection.XPathQueryList.Add(XPathQuery2, ExpectedValue2);
            _XPathCollection.XPathQueryList.Add(XPathQuery3, ExpectedValue3);
            _XPathCollection.XPathQueryList.Add(XPathQuery4, ExpectedValue4);
            _XPathCollection.XPathQueryList.Add(XPathQuery5, ExpectedValue5);
            _XPathCollection.XPathQueryList.Add(XPathQuery6, ExpectedValue6);
            _XPathCollection.XPathQueryList.Add(XPathQuery7, ExpectedValue7);
            _XPathCollection.XPathQueryList.Add(XPathQuery8, ExpectedValue8);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "8");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting multiple context properties results in them being successfully written to the context of a message in a send pipeline
        ///and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1 to FoundExpectedResult if the properties are found
        ///</summary>
        [TestMethod()]
        public void Test_Get_Multiple()
        {
            string InputFileName = "Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_Multiple Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "9");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SMTP.From context property based on the resulting node value of an XPath expression against the input document
        ///results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_XPath_Value()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_XPath_Value Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='Test_Set_BTS_DestinationParty'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SMTP.From context property based on the resulting node Name of an XPath expression against the input document
        ///results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_XPath_Name()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_XPath_Name Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='ApplicationContext'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SMTP.From context property based on the resulting node namespace of an XPath expression against the input document
        ///results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_XPath_Namespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_XPath_Namespace Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='http://www.w3.org/2001/XMLSchema-instance'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the SMTP.From context property based on the resulting value of a lookup of an SSO Key/Value pair
        ///results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_SSO()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_SSO Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='TestSSO'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting a custom string context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_CustomProperty_String()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_CustomProperty_String Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='ExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting a custom int context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_CustomProperty_Int()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_CustomProperty_Int Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property2'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='414'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting the https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property2 context property results in the property being successfully written
        ///to the context of a message in a send pipeline and then sets the custom context property https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema#Property1
        ///to FoundExpectedResult if the property is found
        ///</summary>
        [TestMethod()]
        public void Test_Get_CustomProperty_Int()
        {
            string InputFileName = "Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_CustomProperty_Int Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property1'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='FoundExpectedResult'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "2");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkSendPipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting a custom boolean context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_CustomProperty_Boolean()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_CustomProperty_Boolean Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property3'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='true'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that setting a custom DateTime context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_CustomProperty_DateTime()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_CustomProperty_DateTime Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property4'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='2002-05-30T09:00:00'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
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

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "0");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, InstanceConfigFilePath, _XPathCollection, testContextInstance);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that removing a context property works
        ///</summary>
        [TestMethod()]
        public void Test_RemoveContextProperty()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_RemoveContextProperty Config.xml";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = new b.Xaml.TestCase();

            var pipelineTestStep = new BREPipelineFramework.CustomBizUnitTestSteps.ExecuteReceivePiplineWithNullablePropertyStep
            {
                PipelineAssemblyPath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.TestProject\bin\Debug\BREPipelineFramework.TestProject.dll",
                PipelineTypeName = "BREPipelineFramework.TestProject.Rcv_Double_BREPipelineFramework",
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

            foreach (KeyValuePair<string, string> pair in _XPathCollection.XPathQueryList)
            {
                var xPathDefinitionPropertyValue = new BREPipelineFramework.CustomBizUnitTestSteps.XPathDefinition
                {
                    Description = "Property Value Test",
                    XPath = pair.Key,
                    Value = pair.Value
                };

                xmlValidateContextStep.XPathValidations.Add(xPathDefinitionPropertyValue);
            }

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
    }
}
