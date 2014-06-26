using System;
using System.Management;
using Microsoft.BizTalk.SSOClient.Interop;

namespace BREPipelineFramework.Helpers
{
    public static class StaticHelpers
    {
        /// <summary>
        ///  Looks in the registry to determine the database connection string for the
        ///  BizTalk Management Database for the BizTalk Group that this machine belongs
        ///  to.
        /// </summary>
        /// <returns>Database connection string to BTS Management Database</returns>
        public static string GetMgmtDBConnectionString()
        {
            string BTSMgmtDBName = String.Empty, BTSMgmtDBServerName = String.Empty;
            string MgmtDBConnString = String.Empty;
            object syncRoot = new object();

            //check to see if the string has already been retrieved.  If not, get it and store it.
            if (MgmtDBConnString.Length == 0)
            {
                lock (syncRoot)
                {
                    if (MgmtDBConnString.Length == 0)
                    {
                        ManagementObjectSearcher searcher =
                            new ManagementObjectSearcher(@"root\MicrosoftBizTalkServer", "SELECT * FROM MSBTS_GroupSetting");
                        foreach (ManagementObject Group in searcher.Get())
                        {
                            if (Group != null)
                            {
                                Group.Get();
                                BTSMgmtDBName = Group["MgmtDbName"].ToString();
                                BTSMgmtDBServerName = Group["MgmtDbServerName"].ToString();
                            }
                        }

                        if (BTSMgmtDBName.Length == 0 || BTSMgmtDBServerName.Length == 0)
                            throw new ApplicationException("Unable to find Management Database Name or Management Database Server Name");

                        // Assuming Integrated Security is being used for database connection.
                        MgmtDBConnString = string.Format("SERVER={0};DATABASE={1};Integrated Security=SSPI", BTSMgmtDBServerName, BTSMgmtDBName);
                    }
                }
            }
            return MgmtDBConnString;
        }

        /// <summary>
        /// Read a key/value pair from an SSO configuration store
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public static string ReadFromSSO(string appName, string propName)
        {
            SSOConfigStore ssoStore = new SSOConfigStore();
            SSOConfigurationPropertyBag appMgmtBag = new SSOConfigurationPropertyBag();
            ((ISSOConfigStore)ssoStore).GetConfigInfo(appName, "ConfigProperties", SSOFlag.SSO_FLAG_RUNTIME, (IPropertyBag)appMgmtBag);
            object propertyValue = null;
            appMgmtBag.Read(propName, out propertyValue, 0);
            return (string)propertyValue;
        }
    }
}
