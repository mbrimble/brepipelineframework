using System;
using BREPipelineFramework.Helpers;
using System.Xml;
using Microsoft.BizTalk.XPath;
using System.Data.SqlClient;
using System.Data;
using BREPipelineFramework.SampeInstructions.Instructions;
using System.Text.RegularExpressions;
using BREPipelineFramework.SampleInstructions.Instructions;
using BREPipelineFramework.Helpers.Tracing;
using System.Collections.Generic;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class HelperMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Private Properties

        private FindReplaceStringInstruction findReplaceStringInstruction = null;
        private MessageModificationInstructions messageModificationInstructions;

        public string BodyString
        {
            get 
            {
                System.IO.StreamReader reader = new System.IO.StreamReader(base.InMsg.BodyPart.GetOriginalDataStream());
                base.Pc.ResourceTracker.AddResource(reader);
                string bodyString = reader.ReadToEnd();
                base.InMsg.BodyPart.Data.Position = 0;

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
            return inputString.ToUpperInvariant();
        }

        /// <summary>
        /// Converts a provided string to Lowercase
        /// </summary>
        /// <param name="inputString">The string to convert</param>
        /// <returns>The string converted to Lowercase</returns>
        public string ReturnLowercaseString(String inputString)
        {
            return inputString.ToLowerInvariant();
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
            return StringToCheck.Length;
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
            XmlTextReader xmlTextReader = new XmlTextReader(base.InMsg.BodyPart.GetOriginalDataStream());
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
                    }
                }
            }

            if ((isFound == false) && (exceptionIfNotFound))
            {
                base.SetException(new Exception("No result found for XPath query - " + _XPathQuery));
            }

            base.InMsg.BodyPart.Data.Position = 0;
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
        /// Replace a specific substring in the message body with another string
        /// </summary>
        public void FindReplaceStringInMessage(string stringToBeReplaced, string stringToReplaceWith)
        {
            InstantiateFindReplaceStringInstructionIfNecessary();
            findReplaceStringInstruction.AddStringReplace(stringToBeReplaced, stringToReplaceWith);
        }

        /// <summary>
        /// Replace the result of a regular expression against the message body with another string
        /// </summary>
        public void FindReplaceRegexInMessage(string regexToBeReplaced, string stringToReplaceWith)
        {
            InstantiateFindReplaceStringInstructionIfNecessary();
            findReplaceStringInstruction.AddRegexReplace(regexToBeReplaced, stringToReplaceWith);
        }

        /// <summary>
        /// Return the first Regex match in a given string
        /// </summary>
        public string ReturnFirstRegexMatchInString(string regexToFind, string inputString)
        {
            return ReturnRegexMatchInStringByIndex(regexToFind, inputString, 0);
        }

        /// <summary>
        /// Return the Regex match at a given index in a string 
        /// </summary>
        public string ReturnRegexMatchInStringByIndex(string regexToFind, string inputString, int index)
        {
            TraceManager.CustomComponent.TraceIn(CallToken);
            string matchedString = String.Empty;

            MatchCollection matchedStrings = Regex.Matches(inputString, regexToFind);

            if (matchedStrings.Count >= index + 1)
            {
                matchedString = matchedStrings[index].Value;
            }

            TraceManager.CustomComponent.TraceOut(CallToken);
            return matchedString;
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
            System.IO.StreamReader reader = new System.IO.StreamReader(InMsg.BodyPart.GetOriginalDataStream());
            Pc.ResourceTracker.AddResource(reader);
            bool found = false;

            while (!reader.EndOfStream && !found)
            {
                string body = reader.ReadLine();
                found = body.Contains(stringToFind);
            }

            InMsg.BodyPart.Data.Position = 0;
            return found;
        }

        /// <summary>
        /// Return a boolean that indicates whether a specific regular expression evaluates to a match in the message body
        /// </summary>
        public bool CheckIfRegexExistsInMessage(string regexToFind)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(InMsg.BodyPart.GetOriginalDataStream());
            Pc.ResourceTracker.AddResource(reader);
            bool found = false;

            while (!reader.EndOfStream && !found)
            {
                string body = reader.ReadLine();
                Match match = Regex.Match(body, regexToFind);
                found = match.Success;
            }

            InMsg.BodyPart.Data.Position = 0;
            return found;
        }

        /// <summary>
        /// Get the length of the message body
        /// </summary>
        public int MessageBodyLength()
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(InMsg.BodyPart.GetOriginalDataStream());
            Pc.ResourceTracker.AddResource(reader);
            int length = 0;

            while (!reader.EndOfStream)
            {
                string body = reader.ReadLine();
                length = length + body.Length;
            }

            InMsg.BodyPart.Data.Position = 0;
            return length;
        }

        public string ReturnFirstRegexMatch(string regexToFind)
        {
            return ReturnRegexMatchByIndex(regexToFind, 0);
        }

        public string ReturnRegexMatchByIndex(string regexToFind, int index)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(InMsg.BodyPart.GetOriginalDataStream());
            Pc.ResourceTracker.AddResource(reader);
            string matchedString = String.Empty;
            List<string> matchedStrings = new List<string>();

            while (!reader.EndOfStream && matchedStrings.Count < index + 1)
            {
                string body = reader.ReadLine();
                MatchCollection matchCollection = Regex.Matches(body, regexToFind);

                foreach (Match match in matchCollection)
                {
                    matchedStrings.Add(match.Value);
                }
            }

            if (matchedStrings.Count >= index + 1)
            {
                matchedString = matchedStrings[index].ToString();
            }
            
            InMsg.BodyPart.Data.Position = 0;
            return matchedString;           
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
                "See http://social.msdn.microsoft.com/Forums/en-US/e5b00132-8d05-47a0-8a4c-073429b4c8ad/failed-with-exception-syntax-error-or-access-violation for hints.  Exception details - " + ex.Message, ex));
            }
            catch (Exception e)
            {
                base.SetException(new Exception("An exception was encountered while looking up a party with an alias name of " + AliasName + ", an alias qualifier of " + AliasQualifier + ", and an alias value of " + AliasValue + " - " + e.Message, e));
            }

			return party;
		}
        
        /// <summary>
        /// Transform a message using the specified map class with the specified level of source schema validation
        /// </summary>
        public void TransformMessage(string mapClassName, string mapAssemblyName, TransformationSourceSchemaValidation validation)
        {
            TransformationInstruction instruction = new TransformationInstruction(mapClassName, mapAssemblyName, validation, CallToken);
            base.AddInstruction(instruction);
        }

        /// <summary>
        /// Transform a message using the specified map class with the specified level of source schema validation and with property promotion on the target message
        /// </summary>
        public void TransformMessageWithPromotion(string mapClassName, string mapAssemblyName, TransformationSourceSchemaValidation validation)
        {
            SetContextPropertyPipelineInstruction noDebatchInstruction = new SetContextPropertyPipelineInstruction(BizTalkXMLNORMPropertySchemaEnum.PromotePropertiesOnly.ToString(),
    ContextPropertyNamespaces._XMLNormPropertyNamespace, true, ContextInstructionTypeEnum.Write);
            base.AddInstruction(noDebatchInstruction);

            TransformationInstruction instruction = new TransformationInstruction(mapClassName, mapAssemblyName, validation, CallToken);
            base.AddInstruction(instruction);

            ApplyXmlDisassemblerInstruction promotionInstruction = new ApplyXmlDisassemblerInstruction();
            base.AddInstruction(promotionInstruction);
        }

        public bool TraceInfo(string infoToTrace)
        {
            TraceManager.RulesComponent.TraceInfo("{0} - {1}", base.CallToken, infoToTrace);
            return true;
        }

        public string ConvertToString(object obj)
        {
            return obj.ToString();
        }

        public int ConvertToInt(object obj)
        {
            return (int)TypeCaster.GetTypedObject(obj, TypeEnum.Integer);
        }

        public bool ConvertToBool(object obj)
        {
            return (bool)TypeCaster.GetTypedObject(obj, TypeEnum.Boolean);
        }

        public DateTime ConvertToDateTime(object obj)
        {
            return (DateTime)TypeCaster.GetTypedObject(obj, TypeEnum.DateTime);
        }

        public string GetValueFromSSOConfigStore(string applicationName, string key, FailureActionEnum failureAction)
        {
            string value = null;

            try
            {
                value = StaticHelpers.ReadFromSSO(applicationName, key, failureAction, value);
            }
            catch (Exception e)
            {
                base.SetException(e);
            }

            return value;
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

        #endregion

        #region Deprecated methods - not supported in latest vocabularies but kept for backwards compatibility

        /// <summary>
        /// Add a namespace to the document root node
        /// </summary>
        public void AddDocumentNamespace(string Namespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.AddRootNodeNamespaceAndPrefix, Namespace, prefix: "ns0");
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        /// <summary>
        /// Add a namespace and namespace prefix to the document root node
        /// </summary>
        public void AddDocumentNamespaceAndPrefix(string Namespace, string NamespacePrefix)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.AddRootNodeNamespaceAndPrefix, Namespace, prefix: NamespacePrefix);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        /// <summary>
        /// Replace the namespace on the document root node
        /// </summary>
        public void ReplaceDocumentNamespace(string Namespace)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateRootNodeNamespaceAndPrefix, Namespace, prefix: "ns0");
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        /// <summary>
        /// Replace the namespace and namespace prefix on the document root node
        /// </summary>
        public void ReplaceDocumentNamespaceAndPrefix(string Namespace, string NamespacePrefix)
        {
            MessageModificationDetails messageModificationInstruction = new MessageModificationDetails(MessageModificationInstructionTypeEnum.UpdateRootNodeNamespaceAndPrefix, Namespace, prefix: NamespacePrefix);
            AddMessageModificationInstruction(messageModificationInstruction);
        }

        private void AddMessageModificationInstruction(MessageModificationDetails messageModificationInstruction)
        {
            if (messageModificationInstructions == null)
            {
                messageModificationInstructions = new MessageModificationInstructions(CallToken);
                base.AddInstruction(messageModificationInstructions);
            }

            messageModificationInstructions._MessageModificationDetails.Add(messageModificationInstruction);
        }

        #endregion
    }
}
