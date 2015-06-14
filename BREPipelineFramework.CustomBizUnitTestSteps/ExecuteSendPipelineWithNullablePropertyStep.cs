using System;
using System.IO;
using System.Collections.Generic;
using Winterdom.BizTalk.PipelineTesting;
using System.Collections.ObjectModel;
using BizUnit.Common;
using BizUnit.TestSteps.BizTalk.Common;
using BizUnit.Xaml;
using BizUnit.TestSteps.BizTalk.Pipeline;
using BizUnit;
using BizUnit.TestSteps.BizTalk;
using System.Xml;
using System.Text;

namespace BREPipelineFramework.CustomBizUnitTestSteps
{

    public class ExecuteSendPipelineWithNullablePropertyStep : TestStepBase
    {
        private string _pipelineAssemblyPath;
        private string _pipelineTypeName;
        private Collection<DocSpecDefinition> _docSpecsRawList = new Collection<DocSpecDefinition>();
        private Type[] _docSpecs;
        private string _instanceConfigFile;
        private string _sourceDir;
        private string _sourceEncoding;
        private string _searchPattern;
        private string _destination;
        private string _inputContextDir;
        private string _inputContextSearchPattern;
        private string _outputContextFile;
        private DataLoaderBase inputFileLoader;
        private DataLoaderBase inputContextFileLoader;
        private DataLoaderBase instanceConfigLoader;
        private string encoding;

        ///<summary>
        /// Gets and sets the assembly path for the .NET type of the pipeline to be executed
        ///</summary>
        public string PipelineAssemblyPath
        {
            get { return _pipelineAssemblyPath; }
            set { _pipelineAssemblyPath = value; }
        }

        ///<summary>
        /// Gets and sets the type name for the .NET type of the pipeline to be executed
        ///</summary>
        public string PipelineTypeName
        {
            get { return _pipelineTypeName; }
            set { _pipelineTypeName = value; }
        }

        ///<summary>
        /// Gets and sets the docspecs for the pipeline to be executed. Pairs of typeName, assemblyPath.
        ///</summary>
        public Collection<DocSpecDefinition> DocSpecs
        {
            get { return _docSpecsRawList; }
            private set { _docSpecsRawList = value; }
        }

        ///<summary>
        /// Gets and sets the pipeline instance configuration for the pipeline to be executed
        ///</summary>
        public string InstanceConfigFile
        {
            get { return _instanceConfigFile; }
            set { _instanceConfigFile = value; }
        }

        ///<summary>
        /// Gets and sets the source file path for the input file to the pipeline
        ///</summary>
        public string SourceDir
        {
            get { return _sourceDir; }
            set { _sourceDir = value; }
        }

        ///<summary>
        /// Gets and sets the source encoding
        ///</summary>
        public string SourceEncoding
        {
            get { return _sourceEncoding; }
            set { _sourceEncoding = value; }
        }

        ///<summary>
        /// Gets and sets the search pattern for the input file
        ///</summary>
        public string SearchPattern
        {
            get { return _searchPattern; }
            set { _searchPattern = value; }
        }

        ///<summary>
        /// Gets and sets the destination of the pipeline output
        ///</summary>
        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }

        ///<summary>
        /// Gets and sets the directory containing the message context file for the input message
        ///</summary>
        public string InputContextDir
        {
            get { return _inputContextDir; }
            set { _inputContextDir = value; }
        }

        ///<summary>
        /// Gets and sets the message context search pattern for the input message
        ///</summary>
        public string InputContextSearchPattern
        {
            get { return _inputContextSearchPattern; }
            set { _inputContextSearchPattern = value; }
        }

        ///<summary>
        /// Gets and sets the file name for the message context for the output message
        ///</summary>
        public string OutputContextFile
        {
            get { return _outputContextFile; }
            set { _outputContextFile = value; }
        }

        public DataLoaderBase InputFileLoader
        {
            get { return inputFileLoader; }
            set { inputFileLoader = value; }
        }

        public DataLoaderBase InputContextLoader
        {
            get { return inputContextFileLoader; }
            set { inputContextFileLoader = value; }
        }

        public DataLoaderBase InstanceConfigLoader
        {
            get { return instanceConfigLoader; }
            set { instanceConfigLoader = value; }
        }

        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        /// <summary>
        /// TestStepBase.Execute() implementation
        /// </summary>
        /// <param name='context'>The context for the test, this holds state that is passed beteen tests</param>
        public override void Execute(Context context)
        {
            if (_docSpecsRawList.Count > 0)
            {
                var ds = new List<Type>(_docSpecsRawList.Count);
                foreach (var docSpec in _docSpecsRawList)
                {
                    var ass = AssemblyHelper.LoadAssembly((string)docSpec.AssemblyPath);
                    context.LogInfo("Loading DocumentSpec {0} from location {1}.", docSpec.TypeName, ass.Location);
                    var type = ass.GetType(docSpec.TypeName);

                    ds.Add(type);
                }
                _docSpecs = ds.ToArray();
            }

            context.LogInfo("Loading pipeline {0} from location {1}.", _pipelineTypeName, _pipelineAssemblyPath);
            var pipelineType = ObjectCreator.GetType(_pipelineTypeName, _pipelineAssemblyPath);

            var pipelineWrapper = PipelineFactory.CreateSendPipeline(pipelineType);
            if (!string.IsNullOrEmpty(_instanceConfigFile))
            {
                pipelineWrapper.ApplyInstanceConfig(_instanceConfigFile);
            }

            if (instanceConfigLoader != null)
            {
                XmlReader reader = XmlReader.Create(instanceConfigLoader.Load(context));
                pipelineWrapper.ApplyInstanceConfig(reader);
            }

            if (null != _docSpecs)
            {
                foreach (Type docSpec in _docSpecs)
                {
                    pipelineWrapper.AddDocSpec(docSpec);
                }
            }

            var mc = new MessageCollection();

            if (inputFileLoader != null)
            {
                Stream inputStream = inputFileLoader.Load(context);
                var inputMessage = MessageHelper.CreateFromStream(inputStream);

                if (!string.IsNullOrEmpty(_sourceEncoding))
                {
                    inputMessage.BodyPart.Charset = _sourceEncoding;
                }

                // Load context file, add to message context.
                if (inputContextFileLoader != null)
                {
                    Stream inputContextStream = inputContextFileLoader.Load(context);
                    var mi = MessageInfo.Deserialize(inputContextStream);
                    mi.MergeIntoMessage(inputMessage);

                    mc.Add(inputMessage);
                }
            }
            else
            {
                FileInfo[] contexts = null;
                if (!string.IsNullOrEmpty(_inputContextDir) && !string.IsNullOrEmpty(_inputContextSearchPattern))
                {
                    var cdi = new DirectoryInfo(_inputContextDir);
                    contexts = cdi.GetFiles(_inputContextSearchPattern);
                }

                var di = new DirectoryInfo(_sourceDir);
                int index = 0;
                foreach (FileInfo fi in di.GetFiles(_searchPattern))
                {
                    Stream stream = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read);
                    var inputMessage = MessageHelper.CreateFromStream(stream);
                    if (!string.IsNullOrEmpty(_sourceEncoding))
                    {
                        inputMessage.BodyPart.Charset = _sourceEncoding;
                    }

                    // Load context file, add to message context.
                    if ((null != contexts) && (contexts.Length > index))
                    {
                        string cf = contexts[index].FullName;
                        if (System.IO.File.Exists(cf))
                        {
                            MessageInfo mi = MessageInfo.Deserialize(cf);
                            mi.MergeIntoMessage(inputMessage);
                        }
                    }

                    mc.Add(inputMessage);
                    index++;
                }
            }

            var outputMsg = pipelineWrapper.Execute(mc);
            PersistMessageHelper.PersistMessage(outputMsg, _destination);

            if (!string.IsNullOrEmpty(_outputContextFile))
            {
                if (outputMsg != null)
                {
                    var omi = BizTalkMessageInfoFactory.CreateMessageInfo(outputMsg, _destination);
                    MessageInfo.Serialize(omi, _outputContextFile);
                }
                else
                {
                    using (var fs = new FileStream(_outputContextFile, FileMode.Create))
                    {
                        var enc = System.Text.Encoding.GetEncoding("UTF-8");

                        using (var writer = new StreamWriter(fs, enc))
                        {
                            Stream msgStream = new System.IO.MemoryStream();

                            using (var reader = new StreamReader(msgStream, enc))
                            {
                                const int size = 1024;
                                var buf = new char[size];
                                var charsRead = reader.Read(buf, 0, size);
                                while (charsRead > 0)
                                {
                                    writer.Write(buf, 0, charsRead);
                                    charsRead = reader.Read(buf, 0, size);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// TestStepBase.Validate() implementation
        /// </summary>
        /// <param name='context'>The context for the test, this holds state that is passed beteen tests</param>
        public override void Validate(Context context)
        {
            ArgumentValidation.CheckForEmptyString(_pipelineTypeName, "pipelineTypeName");
            // pipelineAssemblyPath - optional

            _destination = context.SubstituteWildCards(_destination);
        }
    }
}