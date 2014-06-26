using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BizUnit.Xaml;
using System.IO;
using BizUnit;
using BizUnit.CoreSteps.Utilities;

namespace BREPipelineFramework.CustomBizUnitTestSteps
{
    public class BinaryComparisonTestStep : SubStepBase
    {
        private string _comparisonDataPath;
        private bool _compareAsUtf8;

        public string ComparisonDataPath
        {
            set { _comparisonDataPath = value; }
        }

        public bool CompareAsUTF8
        {
            set { _compareAsUtf8 = value; }
        }


        public override Stream Execute(Stream data, Context context)
        {
            MemoryStream dataToValidateAgainst = null;

            try
            {
                context.LogInfo("Attempting to perform binary stream validation for current data stream with expected binary stream of {0}", _comparisonDataPath);

                try
                {
                    dataToValidateAgainst = StreamHelper.LoadFileToStream(_comparisonDataPath);
                }
                catch (Exception e)
                {
                    context.LogError("BinaryValidationStep failed, exception caught trying to open comparison file: {0}", _comparisonDataPath);
                    context.LogException(e);

                    throw;
                }

                try
                {
                    data.Seek(0, SeekOrigin.Begin);
                    dataToValidateAgainst.Seek(0, SeekOrigin.Begin);

                    if (_compareAsUtf8)
                    {
                        // Compare the streams, make sure we are comparing like for like
                        StreamHelper.CompareStreams(StreamHelper.EncodeStream(data, System.Text.Encoding.UTF8), StreamHelper.EncodeStream(dataToValidateAgainst, System.Text.Encoding.UTF8));
                    }
                    else
                    {
                        StreamHelper.CompareStreams(data, dataToValidateAgainst);
                    }
                }
                catch (Exception e)
                {
                    context.LogError("Binary validation failed while comparing the two data streams with the following exception: {0}", e.ToString());

                    //// Dump out streams for validation...
                    //data.Seek(0, SeekOrigin.Begin);
                    //dataToValidateAgainst.Seek(0, SeekOrigin.Begin);
                    //context.LogData("Stream 1:", data);
                    //context.LogData("Stream 2:", dataToValidateAgainst);

                    throw;
                }
            }
            finally
            {
                if (null != dataToValidateAgainst)
                {
                    dataToValidateAgainst.Close();
                }
            }

            data.Seek(0, SeekOrigin.Begin);
            context.LogInfo("Binary stream comparison of incoming binary stream and expected binary stream completed successfully.");
            return data;
        }

        public override void Validate(Context context)
        {
            // compareAsUTF8 - optional

            if (string.IsNullOrEmpty(_comparisonDataPath))
            {
                throw new ArgumentNullException("ComparisonDataPath is either null or of zero length");
            }

            _comparisonDataPath = context.SubstituteWildCards(_comparisonDataPath);
        }
    }
}
