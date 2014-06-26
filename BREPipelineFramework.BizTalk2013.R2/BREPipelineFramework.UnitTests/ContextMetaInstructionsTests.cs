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
    public class ContextMetaInstructionsTests
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_FILE_ReceivedFileName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_FILE_ReceivedFileName Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_FILE_ReceivedFileName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_BTS_DestinationParty()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_BTS_DestinationParty Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_BTS_DestinationParty.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName:InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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
               
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_WCF_Action()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_WCF_Action Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_WCF_Action.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_SQL_ConnectionString()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_SQL_ConnectionString Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_SQL_ConnectionString.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='PreserveBom'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/xmlnorm-properties'][@Value='False'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_XMLNORM_PreserveBom()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_XMLNORM_PreserveBom Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_XMLNORM_PreserveBom.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_WSS_Url()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_WSS_Url Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_WSS_Url.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        /// <summary>
        ///Tests that setting the SMTP.From context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_SBMessaging_Label()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_SBMessaging_Label Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Label'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2012/Adapter/BrokeredMessage-properties'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_SBMessaging_Label()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_SBMessaging_Label Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_SBMessaging_Label.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        /// <summary>
        ///Tests that setting the SFTP.UserName context property results in the property being successfully written to the context of a message in a receive pipeline
        ///</summary>
        [TestMethod()]
        public void Test_Set_SFTP_UserName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Set_SFTP_UserName Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='UserName'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2012/Adapter/sftp-properties'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_SFTP_UserName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_SFTP_UserName Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_SFTP_UserName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_SMTP_From()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_Get_SMTP_From Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_Get_SMTP_From.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }


        [TestMethod()]
        public void Test_Set_XPath_NotFound_Ignore()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_Set_XPath_NotFound_Ignore");
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='ApplicationContext'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Set_XPath_NotFound_Exception()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_Set_XPath_NotFound_Exception");
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='ApplicationContext'])";
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
                if (e.GetBaseException().Message.Contains("Unable to evaluate XPath expression"))
                {

                }
                else
                {
                    Assert.Fail("Unexpected exception was encountered - " + e.GetBaseException().Message);
                }
            }
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property3'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='True'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='Property4'][@Promoted='false'][@Namespace='https://BREPipelineFramework.TestProject.BREPipelineFramework_PropSchema'][@Value='30/05/2002 9:00:00 a.m.'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, _XPathCollection);
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

        [TestMethod()]
        public void Test_Get_BTF2_PassAckThrough()
        {
            string propertyName = "PassAckThrough";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/btf2-properties";
            string applicationContext = "Test_Get_BTF2_PassAckThrough";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);         

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_BTF2_PassAckThrough()
        {
            string propertyName = "PassAckThrough";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/btf2-properties";
            string applicationContext = "Test_Set_BTF2_PassAckThrough";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_EDI_AgreementID()
        {
            string propertyName = "AgreementID";
            string propertyNamespace = "http://schemas.microsoft.com/Edi/PropertySchema";
            string applicationContext = "Test_Get_EDI_AgreementID";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_EDI_AgreementID()
        {
            string propertyName = "AgreementID";
            string propertyNamespace = "http://schemas.microsoft.com/Edi/PropertySchema";
            string applicationContext = "Test_Set_EDI_AgreementID";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_EDIAS2_AS2From()
        {
            string propertyName = "AS2From";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2006/as2-properties";
            string applicationContext = "Test_Get_EDIAS2_AS2From";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_EDIAS2_AS2From()
        {
            string propertyName = "AS2From";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2006/as2-properties";
            string applicationContext = "Test_Set_EDIAS2_AS2From";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_EDIOverride_UNA6Suffix()
        {
            string propertyName = "UNA6Suffix";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2006/edi-properties";
            string applicationContext = "Test_Get_EDIOverride_UNA6Suffix";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Get_ErrorReport_ErrorType()
        {
            string propertyName = "ErrorType";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2005/error-report";
            string applicationContext = "Test_Get_ErrorReport_ErrorType";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Get_FTP_CommandLogFileName()
        {
            string propertyName = "CommandLogFileName";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/ftp-properties";
            string applicationContext = "Test_Get_FTP_CommandLogFileName";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_FTP_CommandLogFileName()
        {
            string propertyName = "CommandLogFileName";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/ftp-properties";
            string applicationContext = "Test_Set_FTP_CommandLogFileName";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_HTTP_ContentType()
        {
            string propertyName = "ContentType";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/http-properties";
            string applicationContext = "Test_Get_HTTP_ContentType";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Get_Legacy_DestinationQualifier()
        {
            string propertyName = "DestinationQualifier";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/legacy-properties";
            string applicationContext = "Test_Get_Legacy_DestinationQualifier";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_Legacy_DestinationQualifier()
        {
            string propertyName = "DestinationQualifier";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/legacy-properties";
            string applicationContext = "Test_Set_Legacy_DestinationQualifier";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_MIME_ContentDescription()
        {
            string propertyName = "ContentDescription";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/mime-properties";
            string applicationContext = "Test_Get_MIME_ContentDescription";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_MIME_ContentDescription()
        {
            string propertyName = "ContentDescription";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/mime-properties";
            string applicationContext = "Test_Set_MIME_ContentDescription";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_MSMQT_CorrelationId()
        {
            string propertyName = "CorrelationId";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/msmqt-properties";
            string applicationContext = "Test_Get_MSMQT_CorrelationId";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_MSMQT_CorrelationId()
        {
            string propertyName = "CorrelationId";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/msmqt-properties";
            string applicationContext = "Test_Set_MSMQT_CorrelationId";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_MessageTracking_OriginatingMessage()
        {
            string propertyName = "OriginatingMessage";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/messagetracking-properties";
            string applicationContext = "Test_Get_MessageTracking_OriginatingMessage";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_MessageTracking_OriginatingMessage()
        {
            string propertyName = "OriginatingMessage";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/messagetracking-properties";
            string applicationContext = "Test_Set_MessageTracking_OriginatingMessage";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_POP3_DispositionNotificationTo()
        {
            string propertyName = "DispositionNotificationTo";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/pop3-properties";
            string applicationContext = "Test_Get_POP3_DispositionNotificationTo";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_POP3_DispositionNotificationTo()
        {
            string propertyName = "DispositionNotificationTo";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/pop3-properties";
            string applicationContext = "Test_Set_POP3_DispositionNotificationTo";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_SOAP_MethodName()
        {
            string propertyName = "MethodName";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/soap-properties";
            string applicationContext = "Test_Get_SOAP_MethodName";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_SOAP_MethodName()
        {
            string propertyName = "MethodName";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/soap-properties";
            string applicationContext = "Test_Set_SOAP_MethodName";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_XLANGS_SendingOrchestrationType()
        {
            string propertyName = "SendingOrchestrationType";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/xlangs-properties";
            string applicationContext = "Test_Get_XLANGS_SendingOrchestrationType";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_XLANGS_SendingOrchestrationType()
        {
            string propertyName = "SendingOrchestrationType";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/xlangs-properties";
            string applicationContext = "Test_Set_XLANGS_SendingOrchestrationType";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_ESBExceptions_Application()
        {
            string propertyName = "Application";
            string propertyNamespace = "http://schemas.microsoft.biztalk.practices.esb.com/exceptionhandling/system-properties";
            string applicationContext = "Test_Get_ESBExceptions_Application";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_ESBExceptions_Application()
        {
            string propertyName = "Application";
            string propertyNamespace = "http://schemas.microsoft.biztalk.practices.esb.com/exceptionhandling/system-properties";
            string applicationContext = "Test_Set_ESBExceptions_Application";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_ESBItinerary_ServiceName()
        {
            string propertyName = "ServiceName";
            string propertyNamespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary/system-properties";
            string applicationContext = "Test_Get_ESBItinerary_ServiceName";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Set_ESBItinerary_ServiceName()
        {
            string propertyName = "ServiceName";
            string propertyNamespace = "http://schemas.microsoft.biztalk.practices.esb.com/itinerary/system-properties";
            string applicationContext = "Test_Set_ESBItinerary_ServiceName";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='true'][@Namespace='" + propertyNamespace + "'][@Value='test'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_Get_ContextPropertyNotFound_Exception()
        {
            string applicationContext = "Test_Get_ContextPropertyNotFound_Exception";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting the test to result in an exception but none was raised");
            }
            catch (Exception e)
            {
                if (e.GetBaseException().Message.Contains("Unable to get context property"))
                {

                }
                else
                {
                    Assert.Fail("Unexpected exception was encountered - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_Get_ContextPropertyNotFound_DefaultForType()
        {
            string applicationContext = "Test_Get_ContextPropertyNotFound_DefaultForType";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Get_ContextPropertyNotFound_Null()
        {
            string applicationContext = "Test_Get_ContextPropertyNotFound_Null";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_SetMessageType()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Message_Transform.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_SetMessageType");

            XPathCollection _XPathCollection = new XPathCollection();
            string XPathQuery1 = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='MessageType'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/system-properties'][@Value='http://BREPipelineFramework#Message'])";
            string ExpectedValue = "True";
            _XPathCollection.XPathQueryList.Add(XPathQuery1, ExpectedValue);
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection, ExpectedOutputFileName: InputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_CreateSSOTicket()
        {
            string propertyName = "SSOTicket";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/system-properties";
            string applicationContext = "Test_CreateSSOTicket";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='" + propertyName + "'][@Promoted='false'][@Namespace='" + propertyNamespace + "'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);
            _XPathCollection.XPathQueryList.Add(numberOfPropertiesXPath, "1");

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();
        }
    }
}
