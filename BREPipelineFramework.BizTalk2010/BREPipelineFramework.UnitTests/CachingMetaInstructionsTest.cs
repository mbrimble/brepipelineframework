using BREPipelineFramework.SampleInstructions.MetaInstructions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BREPipelineFramework.SampleInstructions;
using BREPipelineFramework.Helpers;
using b = BizUnit;
using System.Collections.Generic;
using System.Threading;
using BizUnit.Xaml;
using System.Runtime.Caching;

namespace BREPipelineFramework.UnitTests
{
    [TestClass()]
    public class CachingMetaInstructionsTests
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

        [TestMethod()]
        public void Test_AddStringToCache()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddStringToCache Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath);
            _BREPipelineFrameworkTest.RunTest();

            string value = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["TestKey"].ToString();
            Assert.IsTrue(value == "TestCache", "Did not find the expected value in the cache");

            Thread.Sleep(5000);

            bool stillThere = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache.Contains("TestKey");
            Assert.IsFalse(stillThere, "Cached item should have been deleted by now");
        }

        [TestMethod()]
        public void Test_AddAndGetStringFromCache()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddStringToCache Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath);
            _BREPipelineFrameworkTest.RunTest();

            string value = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["TestKey"].ToString();
            Assert.IsTrue(value == "TestCache", "Did not find the expected value in the cache");

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetStringFromCache Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ProxyName'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/http-properties'][@Value='TestCache'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();

            Thread.Sleep(5000);

            bool stillThere = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache.Contains("TestKey");
            Assert.IsFalse(stillThere, "Cached item should have been deleted by now");
        }

        [TestMethod()]
        public void Test_AddAndDeleteStringFromCache()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddStringToCache Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath);
            _BREPipelineFrameworkTest.RunTest();

            string value = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["TestKey"].ToString();
            Assert.IsTrue(value == "TestCache", "Did not find the expected value in the cache");

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_DeleteStringFromCache Config.xml";

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath);
            _BREPipelineFrameworkTest.RunTest();

            bool stillThere = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache.Contains("TestKey");
            Assert.IsFalse(stillThere, "Cached item should have been deleted by now");
        }

        [TestMethod()]
        public void Test_DeleteStringFromCache()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddStringToCache Config.xml";
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath);
            _BREPipelineFrameworkTest.RunTest();

            string value = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["TestKey"].ToString();
            Assert.IsTrue(value == "TestCache", "Did not find the expected value in the cache");

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_DeleteStringFromCache Config.xml";

            var _BREPipelineFrameworkTest1 = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath);
            _BREPipelineFrameworkTest1.RunTest();

            bool stillThere = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache.Contains("TestKey");
            Assert.IsFalse(stillThere, "Cached item should have been deleted");
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddCustomContextPropertyToCache Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName:InputContextFileName);
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

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_GetCustomContextPropertyFromCache Config.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='ProxyName'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/http-properties'][@Value='jcooper1982@aitm.com'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, contextXPathCollection: _XPathCollection, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReapplyCachedContext_SMTPFrom Config.xml";
            InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_ReapplyCachedContext_SMTPFrom.xml";
            XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='true'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='jcooper1982@aitm.com'])";
            ExpectedValue = "True";

            XPathCollection _XPathCollection1 = new XPathCollection();
            _XPathCollection1.XPathQueryList.Add(XPathQuery, ExpectedValue);

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName, contextXPathCollection: _XPathCollection1);
            _BREPipelineFrameworkTest.RunTest();

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
                Assert.Fail("Cache did not contain a context property dictionary from BTS.TransmitWorkID of 123.");
            }

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_DeleteContextFromCache Config.xml";

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#TransmitWorkID = 123"];

            if (cacheItemsObj != null)
            {
                Assert.Fail("Cached item should have been deleted but still existed.");
            }
        }

        [TestMethod()]
        public void Test_AddAllContextPropertiesToCache()
        {
            string applicationContext = "Test_AddAllContextPropertiesToCache";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, InputContextFileName: InputContextFileName);
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

                object _BTSTransmitWorkIDObj = cacheItems["http://schemas.microsoft.com/BizTalk/2003/system-properties#TransmitWorkID"];

                if (_BTSTransmitWorkIDObj != null)
                {
                    Assert.IsTrue(_BTSTransmitWorkIDObj.ToString() == "123", "Unexpected BTS.TransmitWorkID value of " + _BTSTransmitWorkIDObj.ToString() + " found in cache.");
                }
                else
                {
                    Assert.Fail("Cache did not contain a value for BTS.TransmitWorkID");
                }
            }
            else
            {
                Assert.Fail("Cache did not contain a context property dictionary with a key matching BTS.TransmitWorkID of 123.");
            }
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache_NotFound_Exception()
        {
            string propertyName = "TransmitWorkID";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/system-properties";
            string applicationContext = "Test_AddCustomContextPropertyToCache";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting the test to fail but it didn't");
            }
            catch (Exception e)
            {
                if (!e.GetBaseException().Message.Contains("Unable to cache context property"))
                {
                    Assert.Fail("Was expecting a failure due to context property not being found but instead got the following - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache_NotFound_IgnoreCarryOn()
        {
            string propertyName = "TransmitWorkID";
            string propertyNamespace = "http://schemas.microsoft.com/BizTalk/2003/system-properties";
            string applicationContext = "Test_AddCustomContextPropertyToCache_NotFound_IgnoreCarryOn";

            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);
            DataLoaderBase InputContextLoader = TestHelpers.CreateInputContext(testContextInstance, propertyName, propertyNamespace);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, inputContextLoader: InputContextLoader);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache_NoContextKeyProperty()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddCustomContextPropertyToCache Config.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath);

            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting to catch an exception but the BizUnit test passed.");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.GetBaseException().Message.Contains(""), "Caught an unexpected exception - " + e.GetBaseException().Message);
            }
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache_OverrideExpiryTime()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddCustomContextPropertyToCache_OverrideExpiryTime Config.xml";
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

            Thread.Sleep(7000);

            cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#TransmitWorkID = 123"];

            if (cacheItemsObj != null)
            {
                Assert.Fail("Cached context for message with BTS.TransmitWorkID value of 123 should have expired but didn't.");
            }
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache_OverrideExpiryTimeSetPriorityDefault()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddCustomContextPropertyToCache_OverrideExpiryTimeSetPriorityDefault Config.xml";
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

            Thread.Sleep(7000);

            cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#TransmitWorkID = 123"];

            if (cacheItemsObj != null)
            {
                Assert.Fail("Cached context for message with BTS.TransmitWorkID value of 123 should have expired but didn't.");
            }
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache_OverrideExpiryTimeSetPriorityNotRemovable()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddCustomContextPropertyToCache_OverrideExpiryTimeSetPriorityNotRemovable Config.xml";
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

            Thread.Sleep(7000);

            cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#TransmitWorkID = 123"];

            if (cacheItemsObj != null)
            {
                Assert.Fail("Cached context for message with BTS.TransmitWorkID value of 123 should have expired but didn't.");
            }
        }

        [TestMethod()]
        public void Test_AddCustomContextPropertyToCache_OverrideContextKey()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddCustomContextPropertyToCache_OverrideContextKey Config.xml";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache_OverrideContextKey.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            object cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#InterchangeID = 123"];

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

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReapplyContextPropertiesFromCache_OverrideContextKey Config.xml";
            InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_ReapplyContextPropertiesFromCache_OverrideContextKey.xml";
            string XPathQuery = "boolean(/*[local-name()='MessageInfo']/*[local-name()='ContextInfo']/*[local-name()='Property'][@Name='From'][@Promoted='false'][@Namespace='http://schemas.microsoft.com/BizTalk/2003/smtp-properties'][@Value='jcooper1982@aitm.com'])";
            string ExpectedValue = "True";

            XPathCollection _XPathCollection = new XPathCollection();
            _XPathCollection.XPathQueryList.Add(XPathQuery, ExpectedValue);

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName, contextXPathCollection: _XPathCollection);
            _BREPipelineFrameworkTest.RunTest();

            InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_DeleteContextFromCache_OverrideContextKey Config.xml";

            _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            cacheItemsObj = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["http://schemas.microsoft.com/BizTalk/2003/system-properties#InterchangeID = 123"];

            if (cacheItemsObj != null)
            {
                Assert.Fail("Was expecting cached context to be deleted but it was still there.");
            }
        }

        [TestMethod()]
        public void Test_Get_ContextPropertyFromCacheNotFound_Exception()
        {
            string applicationContext = "Test_Get_ContextPropertyFromCacheNotFound_Exception";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, InputContextFileName: InputContextFileName);
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting the test to result in an exception but none was raised");
            }
            catch (Exception e)
            {
                if (e.GetBaseException().Message.Contains("Unable to find cached context property"))
                {

                }
                else
                {
                    Assert.Fail("Unexpected exception was encountered - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_Get_ContextPropertyFromCacheNotFound_DefaultForType()
        {
            string applicationContext = "Test_Get_ContextPropertyFromCacheNotFound_DefaultForType";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Get_ContextPropertyFromCacheNotFound_Null()
        {
            string applicationContext = "Test_Get_ContextPropertyFromCacheNotFound_Null";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Get_CustomStringFromCacheNotFound_Exception()
        {
            string applicationContext = "Test_Get_CustomStringFromCacheNotFound_Exception";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting the test to result in an exception but none was raised");
            }
            catch (Exception e)
            {
                if (e.GetBaseException().Message.Contains("Unable to fetch item from cache with a key of"))
                {

                }
                else
                {
                    Assert.Fail("Unexpected exception was encountered - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_Get_CustomStringFromCacheNotFound_DefaultForType()
        {
            string applicationContext = "Test_Get_CustomStringFromCacheNotFound_DefaultForType";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_Get_CustomStringFromCacheNotFound_Null()
        {
            string applicationContext = "Test_Get_CustomStringFromCacheNotFound_Null";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "", "Did not find the expected context property value in the message - " + propertyValue);
        }

        [TestMethod()]
        public void Test_ReapplyContextFromCacheNotFound_Exception()
        {
            string applicationContext = "Test_ReapplyContextFromCacheNotFound_Exception";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, InputContextFileName: InputContextFileName);
            try
            {
                _BREPipelineFrameworkTest.RunTest();
                Assert.Fail("Was expecting the test to result in an exception but none was raised");
            }
            catch (Exception e)
            {
                if (e.GetBaseException().Message.Contains("Unable to get cached context property"))
                {

                }
                else
                {
                    Assert.Fail("Unexpected exception was encountered - " + e.GetBaseException().Message);
                }
            }
        }

        [TestMethod()]
        public void Test_ReapplyContextFromCacheNotFound_Ignore()
        {
            string applicationContext = "Test_ReapplyContextFromCacheNotFound_Ignore";
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.txt";
            string InputContextFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Context Files\Test_AddCustomContextPropertyToCache.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, applicationContext);

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, InputContextFileName: InputContextFileName);
            _BREPipelineFrameworkTest.RunTest();

            string propertyValue = BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache["Output"].ToString();
            Assert.IsTrue(propertyValue == "ExpectedResult", "Did not find the expected context property value in the message - " + propertyValue);
        }
    }
}
