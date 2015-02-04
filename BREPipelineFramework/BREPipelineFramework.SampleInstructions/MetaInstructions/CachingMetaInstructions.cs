using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using BREPipelineFramework.Helpers;
using BREPipelineFramework.SampleInstructions.Instructions;
using BREPipelineFramework.Helpers.Tracing;

namespace BREPipelineFramework.SampleInstructions.MetaInstructions
{
    public class CachingMetaInstructions : BREPipelineMetaInstructionBase
    {
        #region Private fields / Public properties

        internal static MemoryCache cache = new MemoryCache("BREPipelineFramework.Cache", null);
        private Dictionary<string, object> cacheItems = new Dictionary<string, object>();
        private TimeEnum contextExpiryUnits = TimeEnum.Minutes;
        private int contextExpiryTime = 30;
        private bool deleteContextFromCache = false;
        private bool previouslyCachedContextFetched = false;
        private bool addToCache = false;
        private string contextKeyName = BizTalkGlobalPropertySchemaEnum.TransmitWorkID.ToString();
        private string contextKeyNamespace = ContextPropertyNamespaces._BTSPropertyNamespace.ToString();
        private string contextKey;
        private CacheItemPriority priority = CacheItemPriority.Default;

        public CacheItemPriority Priority
        {
            set { priority = value; }
        }

        #endregion

        #region Public methods

        public void AddCustomContextPropertyToCache(string propertyName, string propertyNamespace, CacheFailureEnum failureAction)
        {
            FetchCachedContext();

            object property = base.InMsg.Context.Read(propertyName, propertyNamespace);

            if (property == null)
            {
                if (failureAction == CacheFailureEnum.RaiseAnException)
                {
                    Exception exc = new Exception(string.Format("Unable to cache context property {0} in namespace {1}.", propertyName, propertyNamespace));
                    base.SetException(exc);
                }
                else if (failureAction == CacheFailureEnum.IgnoreAndCarryOn)
                {
                    // Take no action
                }
            }
            else
            {
                TraceManager.PipelineComponent.TraceInfo("{0} - Adding {1}#{2} context property  value {3} to collection to be cached", CallToken, propertyNamespace, propertyName, property.ToString());
                cacheItems[string.Format("{0}#{1}", propertyNamespace, propertyName)] = property;
                addToCache = true;
            }
        }

        public void CacheAllContextProperties()
        {
            FetchCachedContext();

            for (int contextCounter = 0; contextCounter < base.InMsg.Context.CountProperties; contextCounter++)
            {
                string propertyName;
                string propertyNamespace;

                object property = base.InMsg.Context.ReadAt(contextCounter, out propertyName, out propertyNamespace);
                TraceManager.PipelineComponent.TraceInfo("{0} - Adding {1}#{2} context property  value {3} to collection to be cached", CallToken, propertyNamespace, propertyName, property.ToString());
                cacheItems[string.Format("{0}#{1}", propertyNamespace, propertyName)] = property;
            }

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
        
        public void ReapplyCachedContextProperty(string propertyName, string propertyNamespace, ContextInstructionTypeEnum promotion, CacheFailureEnum failureAction)
        {
            object property;

            FetchCachedContext();
            bool propertyFound = cacheItems.TryGetValue(string.Format("{0}#{1}", propertyNamespace, propertyName), out property);

            if (!propertyFound)
            {
                if (failureAction == CacheFailureEnum.RaiseAnException)
                {
                    Exception exc = new Exception(String.Format("Unable to get cached context property {0}#{1}.", propertyNamespace, propertyName));
                    base.SetException(exc);
                }
                else if (failureAction == CacheFailureEnum.IgnoreAndCarryOn)
                {
                    // Take no action
                }
            }
            else
            {
                SetContextPropertyPipelineInstruction instruction = new SetContextPropertyPipelineInstruction(propertyName, propertyNamespace, property, promotion);
                base.AddInstruction(instruction);
            }
        }

        public void ReapplyAllCachedContextProperties(ContextInstructionTypeEnum promotion)
        {
            FetchCachedContext();

            foreach (KeyValuePair<string, object> kp in cacheItems)
            {
                string[] contextPropertyType = kp.Key.Split('#');

                string propertyName = string.Empty;
                string propertyNamespace = string.Empty;

                if (contextPropertyType.Length == 2)
                {
                    propertyNamespace = contextPropertyType[0];
                    propertyName = contextPropertyType[1];
                }
                else
                {
                    propertyName = contextPropertyType[0];
                }

                SetContextPropertyPipelineInstruction instruction = new SetContextPropertyPipelineInstruction(propertyName, propertyNamespace, kp.Value, promotion);
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
            if (value == null)
            {
                value = String.Empty;
            }

            TraceManager.PipelineComponent.TraceInfo("{0} - Adding string value {1} to the cache with a key of {2}", CallToken, value, key);
            AddToCache(key, value, expiryTime, expiryTimeUnits, priority);
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
            }
            else
            {
                value = obj.ToString();
            }

            return value;
        }

        public void DeleteCustomStringFromCache(string key)
        {
            cache.Remove(key, null);
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
                addToCache = true;
                CacheSelectedContext();
            }

            base.Compensate();
        }

        #endregion

        #region Private methods

        private static void AddToCache(string key, object value, int expiryTime, TimeEnum expiryUnits, CacheItemPriority priority)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMilliseconds(TimeHelper.GetTimeInMilliseconds(expiryTime, expiryUnits));
            policy.Priority = priority;

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
                object contextKeyobj = base.InMsg.Context.Read(contextKeyName, contextKeyNamespace);

                if (contextKeyobj != null)
                {
                    string contextKeyValue = base.InMsg.Context.Read(contextKeyName, contextKeyNamespace).ToString();
                    contextKey = string.Format("{0}#{1} = {2}", contextKeyNamespace, contextKeyName, contextKeyValue);
                }
                else
                {
                    throw new Exception("Unable to create caching key based on context property " + contextKeyNamespace + "#" + contextKeyName);
                }
            }
        }

        private void CacheSelectedContext()
        {
            if (addToCache)
            {
                SetContextKey();
                TraceManager.PipelineComponent.TraceInfo("{0} - Added context properties in collection to the cache with a key of {1}", CallToken, contextKey);
                AddToCache(contextKey, cacheItems, contextExpiryTime, contextExpiryUnits, priority);
            }
        }

        private void DeleteContextFromCache()
        {
            if (deleteContextFromCache)
            {
                FetchCachedContext();
                TraceManager.PipelineComponent.TraceInfo("{0} - Removing context property collection from cache with a key of {1}", CallToken, contextKey);
                cache.Remove(contextKey, null);
            }
        }

        #endregion
    }
}
