using System;
using System.IO;
using System.Threading;
using System.Collections.ObjectModel;
using BizUnit.Common;
using BizUnit.TestSteps.Common;
using BizUnit.Xaml;
using BizUnit;

namespace BREPipelineFramework.CustomBizUnitTestSteps
{
    /// <summary>
    /// Borrowed from the out of the box FileReadMultipleStep but updated to cater for 0 files in directory
    /// </summary>
    public class FileReadMultipleStep : TestStepBase
    {
        public FileReadMultipleStep()
        {
            SubSteps = new Collection<SubStepBase>();
        }

        ///<summary>
        /// The time to wait in milisecs for the file, after which the step will fail if the file is not found
        ///</summary>
        public int Timeout { get; set; }

        ///<summary>
        /// The directory path to search
        ///</summary>
        public string DirectoryPath { get; set; }

        ///<summary>
        /// Filter to apply to directory path, e.g. "*.xml" or "MyFile*.txt"
        ///</summary>
        public string SearchPattern { get; set; }

        ///<summary>
        /// Flag to specify whether the files should be deleted upon completion of the test step
        ///</summary>
        public bool DeleteFiles { get; set; }

        ///<summary>
        /// The number of files expected to be found
        ///</summary>
        public int ExpectedNumberOfFiles { get; set; }

        /// <summary>
        /// TestStepBase.Execute() implementation
        /// </summary>
        /// <param name='context'>The context for the test, this holds state that is passed beteen tests</param>
        public override void Execute(Context context)
        {
            var endTime = DateTime.Now.AddMilliseconds(Timeout);
            bool found = false;
            string[] filelist;
            int timesLogged = 0;

            context.LogInfo("Searching directory: {0}, search pattern: {1}", DirectoryPath, SearchPattern);

            do
            {
                Thread.Sleep(100);

                // Get the list of files in the directory
                filelist = Directory.GetFiles(DirectoryPath, SearchPattern);

                if (filelist.Length == ExpectedNumberOfFiles && ExpectedNumberOfFiles > 0)
                    break;

            } while (endTime > DateTime.Now);

            context.LogInfo("Number of files found: {0}", filelist.Length);

            if (filelist.Length != ExpectedNumberOfFiles)
            {
                // Expecting a specified number of files
                throw new ApplicationException(String.Format("Directory contained: {0} files, but the step expected: {1} files", filelist.Length, ExpectedNumberOfFiles));
            }

            // For each file in the file list
            foreach (string filePath in filelist)
            {
                context.LogInfo("FileReadMultipleStep validating file: {0}", filePath);

                Stream fileData = StreamHelper.LoadFileToStream(filePath, Timeout);
                context.LogData("File: " + filePath, fileData);
                fileData.Seek(0, SeekOrigin.Begin);

                // Check it against the validate steps to see if it matches one of them
                foreach (var subStep in SubSteps)
                {
                    try
                    {
                        // Try the validation and catch the exception
                        fileData = subStep.Execute(fileData, context);
                    }
                    catch (Exception ex)
                    {
                        context.LogException(ex);
                        throw;
                    }
                }

                if (DeleteFiles)
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        public override void Validate(Context context)
        {
            ArgumentValidation.CheckForEmptyString(DirectoryPath, "DirectoryPath");
            ArgumentValidation.CheckForEmptyString(SearchPattern, "SearchPattern");
        }
    }
}
