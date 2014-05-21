//=================================================================================
// Inherited from Microsoft BizTalk CAT Team Best Practices Samples
//
// The Framework library is a set of general best practices for BizTalk developers.
//
//=================================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT.
//=================================================================================

using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BREPipelineFramework.Helpers.Tracing
{
    public static class TraceManager
    {
        #region Private members
        private static readonly IComponentTraceProvider pipelineComponentTracer = new ComponentTraceProvider("PipelineComponent", new Guid("691CB4CB-D20C-408e-8CFF-FD8A01CD2F75"));
        private static readonly IComponentTraceProvider workflowComponentTracer = new ComponentTraceProvider("WorkflowComponent", new Guid("D2316AFB-414B-42e4-BB7F-3AA92B96A98A"));
        private static readonly IComponentTraceProvider dataAccessComponentTracer = new ComponentTraceProvider("DataAccessComponent", new Guid("2E5D65D8-71F9-43e9-B477-733EF6212895"));
        private static readonly IComponentTraceProvider transformComponentTracer = new ComponentTraceProvider("TransformComponent", new Guid("226445A8-5AF3-4dbe-86D2-73E9B965378E"));
        private static readonly IComponentTraceProvider serviceComponentTracer = new ComponentTraceProvider("ServiceComponent", new Guid("E67E8346-90F1-408b-AF40-222B6E3C5ED6"));
        private static readonly IComponentTraceProvider customComponentTracer = new ComponentTraceProvider("CustomComponent", new Guid("6A223DEA-F806-4523-BAD0-312DCC4F63F9"));
        private static readonly IComponentTraceProvider rulesComponentTracer = new ComponentTraceProvider("RulesComponent", new Guid("78E2D466-590F-4991-9287-3F00BA62793D"));
        private static readonly IComponentTraceProvider trackingComponentTracer = new ComponentTraceProvider("TrackingComponent", new Guid("5CBD8BA0-60F8-401b-8FF5-C7F3D5FABE41"));
        #endregion

        #region Public properties
        /// <summary>
        /// The trace provider for user code in the custom pipeline components.
        /// </summary>
        public static IComponentTraceProvider PipelineComponent
        {
            get { return pipelineComponentTracer; }
        }

        /// <summary>
        /// The trace provider for user code in workflows (such as expression shapes in the BizTalk orchestrations).
        /// </summary>
        public static IComponentTraceProvider WorkflowComponent
        {
            get { return workflowComponentTracer; }
        }

        /// <summary>
        /// The trace provider for user code in the custom components responsible for data access operations.
        /// </summary>
        public static IComponentTraceProvider DataAccessComponent
        {
            get { return dataAccessComponentTracer; }
        }

        /// <summary>
        /// The trace provider for user code in the transformation components (such as scripting functoids in the BizTalk maps).
        /// </summary>
        public static IComponentTraceProvider TransformComponent
        {
            get { return transformComponentTracer; }
        }

        /// <summary>
        /// The trace provider for user code in the service components (such as Web Service, WCF Service or service proxy components).
        /// </summary>
        public static IComponentTraceProvider ServiceComponent
        {
            get { return serviceComponentTracer; }
        }

        /// <summary>
        /// The trace provider for user code in the Business Rules components (such as custom fact retrievers, policy executors).
        /// </summary>
        public static IComponentTraceProvider RulesComponent
        {
            get { return rulesComponentTracer; }
        }

        /// <summary>
        /// The trace provider for user code in the business activity tracking components (such as BAM activities).
        /// </summary>
        public static IComponentTraceProvider TrackingComponent
        {
            get { return trackingComponentTracer; }
        }

        /// <summary>
        /// The trace provider for user code in any other custom components which don't fall into any of the standard categories such as Pipeline, Workflow, DataAccess, Transform.
        /// </summary>
        public static IComponentTraceProvider CustomComponent
        {
            get { return customComponentTracer; }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Returns an instance of a trace provider for the specified type. This requires that the type supplies its Guid which will be used
        /// for registering it with the ETW infrastructure.
        /// </summary>
        /// <param name="componentType">The type which must be decarated with a GuidAttribute</param>
        /// <returns>An instance of a trace provider implementing the IComponentTraceProvider interface</returns>
        public static IComponentTraceProvider Create(Type componentType)
        {
            GuidAttribute guidAttribute = FrameworkUtility.GetDeclarativeAttribute<GuidAttribute>(componentType);

            if (guidAttribute != default(GuidAttribute))
            {
                return new ComponentTraceProvider(componentType.FullName, new Guid(guidAttribute.Value));
            }
            else
            {
                throw new MissingMemberException(componentType.FullName, typeof(GuidAttribute).FullName);
            }
        }
        #endregion
    }
}
