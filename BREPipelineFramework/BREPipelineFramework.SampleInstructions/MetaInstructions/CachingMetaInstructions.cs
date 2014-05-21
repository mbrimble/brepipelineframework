using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.SampleInstructions.Instructions;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class CachingMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Private fields

        private static MemoryCache cache = new MemoryCache("BREPipelineFramework.Cache", null);
        private Dictionary<string, object> cacheItems = new Dictionary<string, object>();
        private TimeEnum contextExpiryUnits = TimeEnum.Minutes;
        private int contextExpiryTime = 30;
        private bool deleteContextFromCache = false;
        private bool previouslyCachedContextFetched = false;
        private bool addToCache = false;
        private string contextKeyName = BizTalkGlobalPropertySchemaEnum.TransmitWorkID.ToString();
        private string contextKeyNamespace = ContextPropertyNamespaces._BTSPropertyNamespace.ToString();
        private string contextKey;

        #endregion

        #region Public methods

        public void AddCustomContextPropertyToCache(string propertyName, string propertyNamespace, CacheFailureEnum failureAction)
        {
            FetchCachedContext();

            object property = null;

            try
            {
                property = base.InMsg.Context.Read(propertyName, propertyNamespace);
            }
            catch (Exception e)
            {
                if (failureAction == CacheFailureEnum.RaiseAnException)
                {
                    Exception exc = new Exception(string.Format("Unable to cache context property {0} in namespace {1}.  Error encountered was - {2}", propertyName, propertyNamespace, e.Message, e));
                    base.SetException(exc);
                }
                else if (failureAction == CacheFailureEnum.IgnoreAndCarryOn)
                {
                    // Take no action
                }
                else
                {
                    base.SetException(new Exception(String.Format("Unhandled failureAction of {0} encountered", failureAction.ToString())));
                }
            }

            cacheItems[string.Format("{0}#{1}", propertyNamespace, propertyName)] = property;
            addToCache = true;
        }

        public string GetCustomContextPropertyFromCache(string propertyName, string propertyNamespace, FailureActionEnum failureAction)
        {
            string property = null;
            object obj = null;

            FetchCachedContext();
            bool propertyFound = cacheItems.TryGetValue(string.Format("{0}#{1}", propertyNamespace, propertyName), out obj);

            if (!propertyFound)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception("Unable to find cached context property " + propertyName + " in namespace " + propertyNamespace);
                    base.SetException(exc);
                }
                else if (failureAction == FailureActionEnum.DefaultForType)
                {
                    property = string.Empty;
                }
                else if (failureAction == FailureActionEnum.Null)
                {
                    // Do nothing, leave as null
                }
                else
                {
                    base.SetException(new Exception(String.Format("Unhandled failureAction of {0} encountered", failureAction.ToString())));
                }
            }
            else
            {
                property = obj.ToString();
            }

            return property;
        }

        public void ChangeKeyContextProperty(string propertyName, string propertyNamespace)
        {
            contextKeyName = propertyName;
            contextKeyNamespace = propertyNamespace;
            contextKey = null;
            SetContextKey();
        }

        public void DeleteContextFromCacheIfItStillExists()
        {
            deleteContextFromCache = true;
        }

        public void OverrideDefaultContextCachingExpiry(int expiryTime, TimeEnum expiryUnits)
        {
            this.contextExpiryTime = expiryTime;
            this.contextExpiryUnits = expiryUnits;
        }

        public void ReapplyCachedContextProperty(string propertyName, string propertyNamespace, ContextInstructionTypeEnum promotion, CacheFailureEnum failureAction)
        {
            object property;

            FetchCachedContext();
            bool propertyFound = cacheItems.TryGetValue(string.Format("{0}#{1}", propertyNamespace, propertyName), out property);

            if (!propertyFound)
            {
                if (failureAction == CacheFailureEnum.RaiseAnException)
                {
                    Exception exc = new Exception(String.Format("Unable to get cached context property {0}#{1}."));
                    base.SetException(exc);
                }
                else if (failureAction == CacheFailureEnum.IgnoreAndCarryOn)
                {
                    // Take no action
                }
                else
                {
                    base.SetException(new Exception(String.Format("Unhandled failureAction of {0} encountered", failureAction.ToString())));
                }
            }
            else
            {
                SetContextPropertyPipelineInstruction instruction = new SetContextPropertyPipelineInstruction(propertyName, propertyNamespace, property, promotion);
                base.AddInstruction(instruction);
            }
        }

        public void UpdateCacheExpiryTime(int expiryTime, TimeEnum expiryTimeUnits)
        {
            contextExpiryTime = expiryTime;
            contextExpiryUnits = expiryTimeUnits;
        }

        public void AddCustomStringToCache(string key, string value, int expiryTime, TimeEnum expiryTimeUnits)
        {
            AddToCache(key, value, expiryTime, expiryTimeUnits);
        }

        public string GetCustomStringFromCache(string key, FailureActionEnum failureAction)
        {
            string value = null;
            object obj = null;

            obj = cache.Get(key);

            if (obj == null)
            {
                if (failureAction == FailureActionEnum.Exception)
                {
                    Exception exc = new Exception(string.Format("Unable to fetch item from cache with a key of {0}.", key));
                    base.SetException(exc);
                }
                else if (failureAction == FailureActionEnum.DefaultForType)
                {
                    value = string.Empty;
                }
                else if (failureAction == FailureActionEnum.Null)
                {
                    // Do nothing, leave as null
                }
                else
                {
                    base.SetException(new Exception(String.Format("Unhandled failureAction of {0} encountered", failureAction.ToString())));
                }
            }
            else
            {
                value = obj.ToString();
            }

            return value;
        }

        #endregion

        #region BREPipelineMetaInstructionBase implementation

        public override void ExecutionPreProcessing()
        {
            CacheSelectedContext();
            DeleteContextFromCache();
        }

        public override void Compensate()
        {
            if (deleteContextFromCache && cacheItems != null)
            {
                CacheSelectedContext();
            }

            base.Compensate();
        }

        #endregion

        #region Private methods

        private void AddToCache(string key, object value, int expiryTime, TimeEnum expiryUnits)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMilliseconds(TimeHelper.GetTimeInMilliseconds(expiryTime, expiryUnits));

            cache.Set(key, value, policy, null);
        }

        private void FetchCachedContext()
        {
            SetContextKey();

            if (!previouslyCachedContextFetched)
            {
                Dictionary<string, object> previouslyCachedItems = new Dictionary<string, object>();
                previouslyCachedItems = (Dictionary<string, object>)cache[contextKey];

                if (previouslyCachedItems != null)
                {
                    foreach (KeyValuePair<string, object> kp in previouslyCachedItems)
                    {
                        cacheItems[kp.Key] = kp.Value;
                    }
                }

                previouslyCachedContextFetched = true;
            }
        }

        private void SetContextKey()
        {
            if (string.IsNullOrEmpty(contextKey))
            {
                string contextKeyValue = base.InMsg.Context.Read(contextKeyName, contextKeyNamespace).ToString();
                contextKey = string.Format("{0}#{1} = {2}", contextKeyNamespace, contextKeyName, contextKeyValue);
            }
        }

        private void CacheSelectedContext()
        {
            if (addToCache)
            {
                SetContextKey();
                AddToCache(contextKey, cacheItems, contextExpiryTime, contextExpiryUnits);
            }
        }

        private void DeleteContextFromCache()
        {
            if (deleteContextFromCache)
            {
                FetchCachedContext();
                cache.Remove(contextKey, null);
            }
        }

        #endregion
    }
}
