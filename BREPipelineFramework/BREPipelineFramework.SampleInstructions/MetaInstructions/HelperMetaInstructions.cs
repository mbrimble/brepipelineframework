using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BREPipelineFramework;
using Microsoft.BizTalk.Message.Interop;
using BREPipelineFramework.Helpers;
using System.IO;
using Microsoft.BizTalk.Streaming;
using System.Xml;
using Microsoft.BizTalk.XPath;
using System.Management;
using System.Data.SqlClient;
using System.Data;
using BREPipelineFramework.SampeInstructions.Instructions;
using System.Text.RegularExpressions;
using BREPipelineFramework.SampleInstructions.Instructions;
using BREPipelineFramework.Helpers.Tracing;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class HelperMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Private Properties

        private FindReplaceStringInstruction findReplaceStringInstruction = null;
        private string bodyString;
        private string callToken = Guid.NewGuid().ToString();

        public string BodyString
        {
            get 
            {
                if (string.IsNullOrEmpty(bodyString))
                {
                    ExtractBody();
                }

                return bodyString; 
            }
        }

        #endregion

        #region Public Helper Methods

        /// <summary>
        /// Check whether a string contains a specified substring or not
        /// </summary>
        /// <param name="StringToCheck">String to be checked</param>
        /// <param name="StringToSearchFor">Substring to search for</param>
        /// <returns>Whether a string contains a specified substring or not</returns>
        public bool StringContains(string StringToCheck, string StringToSearchFor)
        {
            return StringToCheck.Contains(StringToSearchFor);
        }

        /// <summary>
        /// Converts a provided string to Uppercase
        /// </summary>
        /// <param name="inputString">The string to convert</param>
        /// <returns>The string converted to Uppercase</returns>
        public string ReturnUppercaseString(String inputString)
        {
            return inputString.ToUpper();
        }

        /// <summary>
        /// Converts a provided string to Lowercase
        /// </summary>
        /// <param name="inputString">The string to convert</param>
        /// <returns>The string converted to Lowercase</returns>
        public string ReturnLowercaseString(String inputString)
        {
            return inputString.ToLower();
        }

        /// <summary>
        /// Validate that the length of the given string is the specified value, set an exception if not
        /// </summary>
        /// <param name="StringToCheck">String to be checked</param>
        /// <param name="ExpectedLength">The length that the string is expected to be</param>
        public void ValidateStringLength(string StringToCheck, int ExpectedLength)
        {
            if (StringToCheck.Length != ExpectedLength)
            {
                base.SetException(new ArgumentException("Length of string " + StringToCheck + " is invalid.  Was expecting " + ExpectedLength.ToString() + " but was " + StringToCheck.Length.ToString() + "."));
            }
        }

        /// <summary>
        /// Forcibly throws an exception which will be handled by the BRE Pipeline Framework
        /// </summary>
        public void ThrowException(string ExceptionDetails)
        {
            base.SetException(new Exception(ExceptionDetails));
        }

        /// <summary>
        /// Get the length of a string
        /// </summary>
        /// <param name="StringToCheck">String to be checked</param>
        /// <returns>The length of the string</returns>
        public int StringLength(string StringToCheck)
        {
            try
            {
                return StringToCheck.Length;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Return a randomly generated GUID as a string
        /// </summary>
        /// <returns>A randomly generated GUID as a string</returns>
        public string GenerateGUIDAsString()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Return the current date and time in the specified format
        /// </summary>
        /// <param name="timeFormat">The format in which to return the current date and time</param>
        /// <returns>The current date and time in the specified format</returns>
        public string GetCurrentDateTime(string timeFormat)
        {
            return DateTime.Now.ToString(timeFormat);
        }

        /// <summary>
        /// Replace a specified substring with another specified substring in a given string
        /// </summary>
        /// <param name="inputString">The string which is to be changed</param>
        /// <param name="stringToBeReplaced">The substring to be searched for and replaced</param>
        /// <param name="stringToReplaceWith">The substring which should be inserted into the string</param>
        /// <returns>The modified string</returns>
        public string ReplaceString(string inputString, string stringToBeReplaced, string stringToReplaceWith)
        {
            return inputString.Replace(stringToBeReplaced, stringToReplaceWith);
        }

        /// <summary>
        /// Returns the file extension for a given filename
        /// </summary>
        /// <param name="fileName">The filename from which the extension should be extracted</param>
        /// <returns>The extracted file extension</returns>
        public string ReturnFileExtension(string fileName)
        {
            return System.IO.Path.GetExtension(fileName);
        }

        /// <summary>
        /// Get the current date and time rounded up to the given interval in seconds returned in the specified format
        /// </summary>
        /// <param name="roundedSeconds">The interval in seconds to which the current time should be rounded up to</param>
        /// <param name="timeFormat">The time format to be applied to the returned value</param>
        /// <returns>The current date and time rounded up to the given interval in seconds returned in the specified format</returns>
        public string RoundCurrentTime(int roundedSeconds, string timeFormat)
        {
            DateTime dt = DateTime.Now;
            DateTime dateToday = DateTime.Today;
            TimeSpan intervalTimeSpan = dt.Subtract(dateToday);
            int interval = (intervalTimeSpan.Hours * 60 * 60) + (intervalTimeSpan.Minutes * 60) + intervalTimeSpan.Seconds;

            int mod = (interval % roundedSeconds);

            double secondsToRoundUp = roundedSeconds - mod;
            DateTime roundedDate = dt.AddSeconds(secondsToRoundUp);

            return roundedDate.ToString(timeFormat);
        }

        /// <summary>
        /// Get the result of an XPath expression on the given message
        /// </summary>
        /// <param name="_XPathResultType">Whether the resulting node's value, name, or namspace should be treated as the result</param>
        /// <param name="_XPathQuery">The XPath Expression</param>
        /// <param name="exceptionIfNotFound">Whether or not to thrown an exception if the XPath expression does not evaluate</param>
        /// <returns>The result of an XPath expression on the given message</returns>
        public string GetXPathResult(XPathResultTypeEnum _XPathResultType, string _XPathQuery, bool exceptionIfNotFound)
        {
            IBaseMessagePart bodyPart = base.InMsg.BodyPart;
            Stream inboundStream = bodyPart.GetOriginalDataStream();

            VirtualStream virtualStream = new VirtualStream();
            ReadOnlySeekableStream readOnlySeekableStream = new ReadOnlySeekableStream(inboundStream, virtualStream);
            XmlTextReader xmlTextReader = new XmlTextReader(readOnlySeekableStream);

            XPathCollection xPathCollection = new XPathCollection();
            xPathCollection.Add(_XPathQuery);

            XPathReader xPathReader = new XPathReader(xmlTextReader, xPathCollection);
            bool isFound = false;
            string value = null;

            while (xPathReader.ReadUntilMatch())
            {
                if (xPathReader.Match(_XPathQuery))
                {
                    isFound = true;
                    switch (_XPathResultType)
                    {
                        case XPathResultTypeEnum.Name:
                            value = xPathReader.LocalName;
                            break;
                        case XPathResultTypeEnum.Namespace:
                            value = xPathReader.NamespaceURI;
                            break;
                        case XPathResultTypeEnum.Value:
                            value = xPathReader.ReadString();
                            break;
                        default:
                            base.SetException(new Exception(string.Format("Unexpected xpath result type of {0} encountered", _XPathResultType.ToString())));
                            break;
                    }
                }
            }

            if ((isFound == false) && (exceptionIfNotFound))
            {
                base.SetException(new Exception("No result found for XPath query - " + _XPathQuery));
            }

            readOnlySeekableStream.Position = 0;
            bodyPart.Data = readOnlySeekableStream;

            return value;
        }

        /// <summary>
        /// Concatenate two strings
        /// </summary>
        public string ConcatenateString(string FirstString, string SecondString)
        {
            return FirstString + SecondString;
        }

        /// <summary>
        /// Concatenate three strings
        /// </summary>
        public string ConcatenateThreeStrings(string FirstString, string SecondString, string ThirdString)
        {
            return FirstString + SecondString + ThirdString;
        }

        /// <summary>
        /// Concatenate four strings
        /// </summary>
        public string ConcatenateFourStrings(string FirstString, string SecondString, string ThirdString, string FourthString)
        {
            return FirstString + SecondString + ThirdString + FourthString;
        }

        /// <summary>
        /// Concatenate five strings
        /// </summary>
        public string ConcatenateFiveStrings(string FirstString, string SecondString, string ThirdString, string FourthString, string FifthString)
        {
            return FirstString + SecondString + ThirdString + FourthString + FifthString;
        }

        /// <summary>
        /// Concatenate six strings
        /// </summary>
        public string ConcatenateSixStrings(string FirstString, string SecondString, string ThirdString, string FourthString, string FifthString, string SixthString)
        {
            return FirstString + SecondString + ThirdString + FourthString + FifthString + SixthString;
        }

        public string StringFormat(string StringToFormat, string Formatters, string Seperator)
        {
            string[] formatterArray = Formatters.Split(new string[]{Seperator}, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                return string.Format(StringToFormat, formatterArray);
            }
            catch (Exception e)
            {
                base.SetException(new Exception("Encountered an error while trying to format string " + StringToFormat + " with input parameters " + Formatters + " and seperator " + 
                    Seperator + ".  Exception details - " + e.Message, e));
                return "";
            }
        }

        /// <summary>
        /// Add a namespace to the document root node
        /// </summary>
        public void AddDocumentNamespace(string Namespace)
        {
            NamespaceModificationInstructions instruction = new NamespaceModificationInstructions(Namespace);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Add a namespace and namespace prefix to the document root node
        /// </summary>
        public void AddDocumentNamespaceAndPrefix(string Namespace, string NamespacePrefix)
        {
            NamespaceModificationInstructions instruction = new NamespaceModificationInstructions(Namespace, NamespacePrefix);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Replace the namespace on the document root node
        /// </summary>
        public void ReplaceDocumentNamespace(string Namespace)
        {
            NamespaceModificationInstructions instruction = new NamespaceModificationInstructions(Namespace, true);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Replace the namespace and namespace prefix on the document root node
        /// </summary>
        public void ReplaceDocumentNamespaceAndPrefix(string Namespace, string NamespacePrefix)
        {
            NamespaceModificationInstructions instruction = new NamespaceModificationInstructions(Namespace, NamespacePrefix, true);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Replace a specific substring in the message body with another string
        /// </summary>
        public void FindReplaceStringInMessage(string stringToBeReplaced, string stringToReplaceWith)
        {
            InstantiateFindReplaceStringInstructionIfNecessary();

            try
            {
                findReplaceStringInstruction.AddStringReplace(stringToBeReplaced, stringToReplaceWith);
            }
            catch (Exception e)
            {
                base.SetException(new Exception("Error while trying to setup a string replace instruction - " + e.Message, e));
            }
        }

        /// <summary>
        /// Replace the result of a regular expression against the message body with another string
        /// </summary>
        public void FindReplaceRegexInMessage(string regexToBeReplaced, string stringToReplaceWith)
        {
            InstantiateFindReplaceStringInstructionIfNecessary();
            
            try
            {
                findReplaceStringInstruction.AddRegexReplace(regexToBeReplaced, stringToReplaceWith);
            }
            catch (Exception e)
            {
                base.SetException(new Exception("Error while trying to setup a regex replace instruction - " + e.Message, e));
            }
        }

        /// <summary>
        /// Set the message body part to null
        /// </summary>
        public void NullifyMessage()
        {
            NullifyMessageInstruction instruction = new NullifyMessageInstruction();
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Return a boolean that indicates whether a specific substring exists in the message body
        /// </summary>
        public bool CheckIfStringExistsInMessage(string stringToFind)
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            return bodyString.Contains(stringToFind);
        }

        /// <summary>
        /// Return a boolean that indicates whether a specific regular expression evaluates to a match in the message body
        /// </summary>
        public bool CheckIfRegexExistsInMessage(string regexToFind)
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            Match match = Regex.Match(bodyString, regexToFind);
            return match.Success;
        }

        /// <summary>
        /// Get the length of the message body
        /// </summary>
        public int MessageBodyLength()
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            try
            {
                return bodyString.Length;
            }
            catch
            {
                return 0;
            }
        }

        public string ReturnFirstRegexMatch(string regexToFind)
        {
            return ReturnRegexMatchByIndex(regexToFind, 0);
        }

        public string ReturnRegexMatchByIndex(string regexToFind, int index)
        {
            if (string.IsNullOrEmpty(bodyString))
            {
                ExtractBody();
            }

            Match match = Regex.Match(bodyString, regexToFind);

            if (match.Success)
            {
                try
                {
                    return match.Groups[index].Value;
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a party name based on a lookup of business identities contained within business profiles.
        /// </summary>
        public string GetPartyFromAlias(string AliasName, string AliasQualifier, string AliasValue, Boolean ThrowExceptionIfNotFound)
		{
            string party = "";

            try
            {
                SqlConnection connection = new SqlConnection(StaticHelpers.GetMgmtDBConnectionString());
                SqlCommand partyResolveCMD = new SqlCommand("admsvr_GetPartyByAliasNameValue", connection);
                partyResolveCMD.CommandType = CommandType.StoredProcedure;

                if (AliasName.Length > 256) throw new ArgumentException("Length of the party alias name exceed 256 characters");
                SqlParameter param = partyResolveCMD.Parameters.Add("@nvcAliasName", SqlDbType.NVarChar, 256);
                param.Value = AliasName;

                if (AliasQualifier.Length > 64) throw new ArgumentException("Length of the party alias qualifier exceeds 64 characters");
                param = partyResolveCMD.Parameters.Add("@nvcAliasQualifier", SqlDbType.NVarChar, 64);
                param.Value = AliasQualifier;

                if (AliasValue.Length > 256) throw new ArgumentException("Length of the party alias value exceeds 256 characters");
                param = partyResolveCMD.Parameters.Add("@nvcAliasValue", SqlDbType.NVarChar, 256);
                param.Value = AliasValue;

                param = partyResolveCMD.Parameters.Add("@nvcSID", SqlDbType.NVarChar, 256);
                param.Direction = ParameterDirection.Output;
                param.Value = string.Empty;

                param = partyResolveCMD.Parameters.Add("@nvcName", SqlDbType.NVarChar, 256);
                param.Direction = ParameterDirection.Output;
                param.Value = string.Empty;

                connection.Open();
                SqlDataReader reader = partyResolveCMD.ExecuteReader();

                party = partyResolveCMD.Parameters["@nvcName"].Value.ToString();
                reader.Close();
                connection.Close();

                if (String.IsNullOrEmpty(party) && ThrowExceptionIfNotFound)
                {
                    Exception e = new Exception("Unable to locate a party with an alias name of " + AliasName + ", an alias qualifier of " + AliasQualifier + ", and an alias value of " + AliasValue + ".");
                    base.SetException(e);
                }
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                base.SetException(new Exception("An exception was encountered while looking up a party with an alias name of " + AliasName + ", an alias qualifier of " + AliasQualifier + ", and an alias value of " + AliasValue +
                    ".  This could potentially be a SQL permissions issue on the admsvr_GetPartyByAliasNameValue stored procedure in the BizTalk Management database or possibly lack of WMI permissions." +
                "See http://social.msdn.microsoft.com/Forums/en-US/e5b00132-8d05-47a0-8a4c-073429b4c8ad/failed-with-exception-syntax-error-or-access-violation for hints.  Exception details - " + ex.Message));
            }
            catch (Exception e)
            {
                base.SetException(new Exception("An exception was encountered while looking up a party with an alias name of " + AliasName + ", an alias qualifier of " + AliasQualifier + ", and an alias value of " + AliasValue + " - " + e.Message));
            }

			return party;
		}

        /// <summary>
        /// Transform a message using the specified map class
        /// </summary>
        public void TransformMessage(string mapClassName, string mapAssemblyName)
        {
            TransformationInstruction instruction = new TransformationInstruction(mapClassName, mapAssemblyName);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Transform a message using the specified map class without any validation of the source schema (less safe)
        /// </summary>
        public void TransformMessageWithoutValidation(string mapClassName, string mapAssemblyName)
        {
            TransformationInstruction instruction = new TransformationInstruction(mapClassName, mapAssemblyName, false);
            base.AddInstruction(instruction);
        }

        public bool TraceInfo(string infoToTrace)
        {
            TraceManager.RulesComponent.TraceInfo("{0} - {1}", callToken, infoToTrace);
            return true;
        }

        #endregion

        #region Private methods

        private void InstantiateFindReplaceStringInstructionIfNecessary()
        {
            if (findReplaceStringInstruction == null)
            {
                findReplaceStringInstruction = new FindReplaceStringInstruction();
                base.AddInstruction(findReplaceStringInstruction);
            }
        }

        private void ExtractBody()
        {
            VirtualStream newMsgStream = new VirtualStream();
            base.InMsg.BodyPart.Data.CopyTo(newMsgStream);
            base.InMsg.BodyPart.Data.Seek(0, SeekOrigin.Begin);
            newMsgStream.Seek(0, SeekOrigin.Begin);
            System.IO.StreamReader reader = new System.IO.StreamReader(newMsgStream);
            bodyString = reader.ReadToEnd();
            reader.Close();
        }

        #endregion
    }
}
