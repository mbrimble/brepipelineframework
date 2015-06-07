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
    [TestClass]
    public class XMLTranslatorMetaInstructionsTests
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
            BREPipelineFramework.SampleInstructions.MetaInstructions.CachingMetaInstructions.cache = MemoryCache.Default;
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

        /// <summary>
        ///Tests that the AddDocumentNamespace vocabulary definition adds the namespace to the XML document root node with the ns0 prefix
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName:ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespaceWithPrefix vocabulary definition adds the namespace to the XML document root node with the specified prefix
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceAndPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespaceAndPrefix Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespaceAndPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespace vocabulary definition doesn't add the namespace to the XML document root node with the ns0 prefix if a root namespace already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceExistingNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespaceExistingNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the AddDocumentNamespaceWithPrefix vocabulary definition doesn't add the namespace to the XML document root node with the specified prefix if a root namespace already exists
        ///</summary>
        [TestMethod()]
        public void Test_AddDocumentNamespaceAndPrefixExistingNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_AddDocumentNamespaceAndPrefix Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_AddDocumentNamespaceAndPrefixExistingNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespace vocabulary definition replaces the current namespace at the XML document root node with the ns0 prefix
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespace.xml";
            
            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespaceWithPrefix vocabulary definition replaces the current namespace at the XML document root node with the specified prefix
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceAndPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test1.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespaceAndPrefix Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespaceAndPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespace vocabulary definition adds the current namespace at the XML document root node with the ns0 prefix if it doesn't currently exists
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceDoesNotExist()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespaceDoesNotExist.xml";


            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        /// <summary>
        ///Tests that the ReplaceDocumentNamespaceWithPrefix vocabulary definition adds the current namespace at the XML document root node with the specified prefix if it doesn't currently exists
        ///</summary>
        [TestMethod()]
        public void Test_ReplaceDocumentNamespaceAndPrefixDoesNotExist()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceDocumentNamespaceAndPrefix Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceDocumentNamespaceAndPrefixDoesNotExist.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ReplaceNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_ReplaceNamespace.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_RemoveNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_RemoveNamespace.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_RemoveNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_RemoveNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ChainNamespaceModifications()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_ReplaceNamespace.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ChainNamespaceModifications Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ChainNamespaceModifications.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ReplacePrefixForGivenNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_ReplacePrefixForGivenNamespace.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplacePrefixForGivenNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplacePrefixForGivenNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ReplaceNamespaceAndPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_RemoveNamespace.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_ReplaceNamespaceAndPrefix Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceNamespaceAndPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateElementValueByNodeName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateElementValueByNodeName.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_UpdateElementValueByNodeName Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateElementValueByNodeName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateElementValueByNodeNameAndNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateElementValueByNodeName.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_UpdateElementValueByNodeNameAndNamespace Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateElementValueByNodeNameAndNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateElementValueByNodeNameAndOldValue()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateElementValueByNodeNameAndOldValue.xml";
            string InstanceConfigFilePath = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Instance Config Files\Test_UpdateElementValueByNodeNameAndOldValue Config.xml";
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateElementValueByNodeNameAndOldValue.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, InstanceConfigFilePath, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateElementValueByNodeNameNamespaceAndOldValue()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateElementValueByNodeNameAndOldValue.xml";
            DataLoaderBase InstanceConfigLoader =  TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateElementValueByNodeNameNamespaceAndOldValue");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateElementValueByNodeNameNamespaceAndOldValue.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateElementValueByOldValue()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateElementValueByNodeNameAndOldValue.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateElementValueByOldValue");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateElementValueByOldValue.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateElementNameByOldName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateElementValueByNodeNameAndOldValue.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateElementNameByOldName");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateElementNameByOldName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateElementNameByOldNameAndNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateElementValueByNodeNameAndOldValue.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateElementNameByOldNameAndNamespace");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateElementNameByOldNameAndNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateAttributeValueByName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateAttributeValueByName");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateAttributeValueByName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateAttributeValueByNameAndOldValue()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateAttributeValueByNameAndOldValue");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateAttributeValueByNameAndOldValue.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateAttributeValueByNameAndNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateAttributeValueByNameAndNamespace");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateAttributeValueByNameAndNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateAttributeNameByName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateAttributeNameByName");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateAttributeNameByName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_UpdateAttributeNameByNameAndNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_UpdateAttributeNameByNameAndNamespace");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_UpdateAttributeNameByNameAndNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_RemoveElementByName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_RemoveElementByName");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_RemoveElementByName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_RemoveElementByNameAndNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_RemoveElementByNameAndNamespace");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_RemoveElementByNameAndNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_RemoveAttributeByName()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_RemoveAttributeByName");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_RemoveAttributeByName.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_RemoveAttributeByNameAndNamespace()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_RemoveAttributeByNameAndNamespace");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_RemoveAttributeByNameAndNamespace.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ReplaceNamespaceWithNoPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_ReplaceNamespaceWithNoPrefix.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_ReplaceNamespaceWithNoPrefix");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceNamespaceWithNoPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_RemoveNamespaceWithNoPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_ReplaceNamespaceWithNoPrefix.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_RemoveNamespaceWithNoPrefix");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_RemoveNamespaceWithNoPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_ReplaceNamespaceAndPrefixNoCurrentPrefix()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_ReplaceNamespaceWithNoPrefix.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_ReplaceNamespaceAndPrefixNoCurrentPrefix");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_ReplaceNamespaceAndPrefixNoCurrentPrefix.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }

        [TestMethod()]
        public void Test_RemoveAttributeByName_ResetPriority()
        {
            string InputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Input Files\Test_UpdateAttributeDetails.xml";
            DataLoaderBase InstanceConfigLoader = TestHelpers.CreateInstanceConfig(testContextInstance, "Test_RemoveAttributeByName_ResetPriority");
            string ExpectedOutputFileName = testContextInstance.TestDir + @"\..\..\BREPipelineFramework.UnitTests\Sample Files\Expected Output Files\Test_RemoveAttributeByName_ResetPriority.xml";

            var _BREPipelineFrameworkTest = TestHelpers.BREPipelineFrameworkReceivePipelineBaseTest(InputFileName, testContextInstance, instanceConfigLoader: InstanceConfigLoader, ExpectedOutputFileName: ExpectedOutputFileName);
            _BREPipelineFrameworkTest.RunTest();
        }
    }
}
