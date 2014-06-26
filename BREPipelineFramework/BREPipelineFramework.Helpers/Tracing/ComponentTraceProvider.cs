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
using System.Diagnostics;
using System.Reflection;
using System.Globalization;
using Microsoft.BizTalk.Diagnostics;
using EtwTraceLevel = Microsoft.BizTalk.Tracing.TraceLevel;
using System.Diagnostics.CodeAnalysis;

namespace BREPipelineFramework.Helpers.Tracing
{
    /// <summary>
    /// Encapsulates tracing provider for a given application component.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ComponentTraceProvider : IComponentTraceProvider
    {
        #region Private members
        private readonly TraceProvider traceProvider;
        private readonly string componentName;
        private readonly Guid componentGuid;
        private readonly HighResTimer highResTimer;

        private const string NullParameterValue = "NULL";
        private const string UnknownMethodName = "UNKNOWN";
        private const string NoReturnValue = "<void>";
        private const string FormatStringTraceIn = "TRACEIN: {0}({1}) => [{2}]";
        private const string FormatStringTraceOut = "TRACEOUT: {0}(...) = {1} <= [{2}]";
        private const string FormatStringTraceOutNoToken = "TRACEOUT: {0}(...) = {1}";
        private const string FormatStringTraceOutNoTokenAndParams = "TRACEOUT: {0}(...)";
        private const string FormatStringTraceInfo = "INFO: {0}";
        private const string FormatStringTraceError = "ERROR: {0} <= [{1}]";
        private const string FormatStringTraceErrorNoToken = "ERROR: {0}";
        private const string FormatStringTraceWarning = "WARN: {0}";
        private const string FormatStringTraceScopeStart = "START -> {0}({1})";
        private const string FormatStringTraceScopeStartNoParams = "START -> {0}";
        private const string FormatStringTraceScopeEnd = "END <- {0}({1}): {2}ms";
        private const string FormatStringTraceScopeEndNoParams = "END <- {0}: {1}ms";
        #endregion

        #region Constructor
        public ComponentTraceProvider(string componentName, Guid componentGuid)
        {
            Guard.ArgumentNotNullOrEmptyString(componentName, "componentName");
            Guard.ArgumentNotDefaultValue<Guid>(componentGuid, "componentGuid");

            this.componentName = componentName;
            this.componentGuid = componentGuid;

            this.traceProvider = new TraceProvider(componentName, componentGuid);
            this.highResTimer = new HighResTimer();
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Returns a flag indicating whether or not tracing is enabled.
        /// </summary>
        public bool IsEnabled
        {
            [DebuggerStepThrough()]
            get
            {
                return this.traceProvider.IsEnabled;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes an information message to the trace.
        /// </summary>
        /// <param name="format">A string containing zero or more format items.</param>
        /// <param name="parameters">A list containing zero or more data items to format.</param>
        [DebuggerStepThrough()]
        public void TraceInfo(string format, params object[] parameters)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(format, parameters);
#endif
            if (IsEnabled)
            {
                TraceMessage(EtwTraceLevel.Info, format, parameters);
            }
        }

        /// <summary>
        /// Writes an information message to the trace. This method is intended to be used when the data that needs to be
        /// written to the trace is expensive to be fetched. The method represented by the Func(T) delegate will only be invoked if
        /// tracing is enabled.
        /// </summary>
        /// <param name="expensiveDataProvider">A method that has no parameters and returns a value that needs to be traced.</param>
        public void TraceInfo(Func<string> expensiveDataProvider)
        {
#if DEBUG
            string data = expensiveDataProvider();

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceInfo, data);
#endif
            if (IsEnabled)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Info, FormatStringTraceInfo, data);
#else
                TraceMessage(EtwTraceLevel.Info, FormatStringTraceInfo, expensiveDataProvider());
#endif
            }
        }

        /// <summary>
        /// Writes an information message to the trace. This method is provided for optimal performance when
        /// tracing simple messages which don't require a format string.
        /// </summary>
        /// <param name="message">A string containing the message to be traced.</param>
        [DebuggerStepThrough()]
        public void TraceInfo(string message)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceInfo, message);
#endif
            if (IsEnabled)
            {
                TraceMessage(EtwTraceLevel.Info, FormatStringTraceInfo, message);
            }
        }

        /// <summary>
        /// Writes a message to the trace. This method can be used to trace detailed information
        /// which is only required in particular cases.
        /// </summary>
        /// <param name="format">A string containing zero or more format items.</param>
        /// <param name="parameters">A list containing zero or more data items to format.</param>
        [DebuggerStepThrough()]
        public void TraceDetails(string format, params object[] parameters)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(format, parameters);
#endif
            if (IsEnabled)
            {
                TraceMessage(EtwTraceLevel.Messages, format, parameters);
            }
        }

        /// <summary>
        /// Writes a warning message to the trace.
        /// </summary>
        /// <param name="format">A string containing zero or more format items.</param>
        /// <param name="parameters">A list containing zero or more data items to format.</param>
        [DebuggerStepThrough()]
        public void TraceWarning(string format, params object[] parameters)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceWarning, String.Format(format, parameters));
#endif
            if (IsEnabled)
            {
                TraceMessage(EtwTraceLevel.Warning, format, parameters);
            }
        }

        /// <summary>
        /// Writes a warning message to the trace. This method is provided for optimal performance when
        /// tracing simple messages which don't require a format string.
        /// </summary>
        /// <param name="message">A string containing the message to be traced.</param>
        [DebuggerStepThrough()]
        public void TraceWarning(string message)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceWarning, message);
#endif
            if (IsEnabled)
            {
                TraceMessage(EtwTraceLevel.Warning, FormatStringTraceWarning, message);
            }
        }

        /// <summary>
        /// Writes an error message to the trace.
        /// </summary>
        /// <param name="format">A string containing zero or more format items.</param>
        /// <param name="parameters">A list containing zero or more data items to format.</param>
        [DebuggerStepThrough()]
        public void TraceError(string format, params object[] parameters)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceErrorNoToken, String.Format(format, parameters));
#endif
            if (IsEnabled)
            {
                TraceMessage(EtwTraceLevel.Error, format, parameters);
            }
        }

        /// <summary>
        /// Writes an error message to the trace. This method is provided for optimal performance when
        /// tracing simple messages which don't require a format string.
        /// </summary>
        /// <param name="message">A string containing the error message to be traced.</param>
        [DebuggerStepThrough()]
        public void TraceError(string message)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceErrorNoToken, message);
#endif
            if (IsEnabled)
            {
                TraceMessage(EtwTraceLevel.Error, FormatStringTraceErrorNoToken, message);
            }
        }

        /// <summary>
        /// Writes the exception details to the trace.
        /// </summary>
        /// <param name="ex">An exception to be formatted and written to the trace.</param>
        [DebuggerStepThrough()]
        public void TraceError(Exception ex)
        {
            TraceError(ex, true);
        }

        /// <summary>
        /// Writes the exception details to the trace.
        /// </summary>
        /// <param name="ex">An exception to be formatted and written to the trace.</param>
        /// <param name="includeStackTrace">A flag indicating whether or not call stack details should be included.</param>
        [DebuggerStepThrough()]
        public void TraceError(Exception ex, bool includeStackTrace)
        {
#if DEBUG
            // Write a formatted exception message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            ExceptionFormatter exceptionFormatter = new ExceptionFormatter(this.componentName);
            string exceptionDetails = exceptionFormatter.FormatException(ex);

            DebugTimeTrace(FormatStringTraceErrorNoToken, exceptionDetails);
#endif
            // Check if tracing is enabled. In addition, check if the trace flag used by this method is also enabled.
            // This is an additional optimization to prevent from calling the ETW tracing component if we don't have to.
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Error) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Error, FormatStringTraceErrorNoToken, exceptionDetails);
#else
                ExceptionFormatter exceptionFormatter = new ExceptionFormatter(this.componentName);
                TraceMessage(EtwTraceLevel.Error, FormatStringTraceErrorNoToken, exceptionFormatter.FormatException(ex, includeStackTrace));
#endif
            }
        }

        /// <summary>
        /// Writes the exception details to the trace.
        /// </summary>
        /// <param name="ex">An exception to be formatted and written to the trace.</param>
        /// <param name="includeStackTrace">A flag indicating whether or not call stack details should be included.</param>
        /// <param name="callToken">An unique value which is used as a correlation token to correlate TraceIn and TraceError calls.</param>
        [DebuggerStepThrough()]
        public void TraceError(Exception ex, bool includeStackTrace, Guid callToken)
        {
#if DEBUG
            // Write a formatted exception message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            ExceptionFormatter exceptionFormatter = new ExceptionFormatter(this.componentName);
            string exceptionDetails = exceptionFormatter.FormatException(ex);

            DebugTimeTrace(FormatStringTraceError, exceptionDetails, callToken);
#endif
            // Check if tracing is enabled. In addition, check if the trace flag used by this method is also enabled.
            // This is an additional optimization to prevent from calling the ETW tracing component if we don't have to.
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Error) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Error, FormatStringTraceError, exceptionDetails, callToken);
#else
                ExceptionFormatter exceptionFormatter = new ExceptionFormatter(this.componentName);
                TraceMessage(EtwTraceLevel.Error, FormatStringTraceError, exceptionFormatter.FormatException(ex, includeStackTrace), callToken);
#endif
            }
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating that a method is invoked. This can be useful for tracing method calls to help analyze the 
        /// code execution flow. The method will also write the same event into default System.Diagnostics trace listener, however this will only occur in the DEBUG code.
        /// A call to the TraceIn method would typically be at the very beginning of an instrumented code.
        /// </summary>
        /// <param name="inParameters">The method parameters which will be included into the traced event (make sure you do not supply any sensitive data).</param>
        /// <returns>An unique value which can be used as a correlation token to correlate TraceIn and TraceOut calls.</returns>
        [DebuggerStepThrough()]
        public Guid TraceIn(params object[] inParameters)
        {
            Guid callToken = Guid.NewGuid();
#if DEBUG
            string methodDetails = GetMethodDetails(GetCallingMethod());
            string parameterList = GetParameterList(inParameters);

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceIn, methodDetails, parameterList, callToken);
#endif
            // Only invoke the tracing code if ETW provider is in Enabled state. We should not be walking on the call stack or performing any kind of formatting 
            // with the parameter list if the tracing provider is disabled. This will save us some processor cycles and ensure that extensive TraceIn and TraceOut
            // will not affect the application performance when tracing is not required.
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceIn, methodDetails, parameterList, callToken);
#else
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceIn, GetMethodDetails(GetCallingMethod()), GetParameterList(inParameters), callToken);
#endif
            }

            return callToken;
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating that a method is invoked. This can be useful for tracing method calls to help analyze the 
        /// code execution flow. The method will also write the same event into default System.Diagnostics trace listener, however this will only occur in the DEBUG code.
        /// A call to the TraceIn method would typically be at the very beginning of an instrumented code.
        /// This method is provided to ensure optimal performance when no parameters are required to be traced.
        /// </summary>
        /// <returns>An unique value which can be used as a correlation token to correlate TraceIn and TraceOut calls.</returns>
        [DebuggerStepThrough()]
        public Guid TraceIn()
        {
            Guid callToken = Guid.NewGuid();
#if DEBUG
            string methodDetails = GetMethodDetails(GetCallingMethod());

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceIn, methodDetails, null, callToken);
#endif
            // Only invoke the tracing code if ETW provider is in Enabled state. We should not be walking on the call stack or performing any kind of formatting 
            // with the parameter list if the tracing provider is disabled. This will save us some processor cycles and ensure that extensive TraceIn and TraceOut
            // will not affect the application performance when tracing is not required.
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceIn, methodDetails, null, callToken);
#else
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceIn, GetMethodDetails(GetCallingMethod()), null, callToken);
#endif
            }

            return callToken;
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating that a method is about to complete. This can be useful for tracing method calls to help analyze the 
        /// code execution flow. The method will also write the same event into default System.Diagnostics trace listener, however this will only occur in the DEBUG code.
        /// A call to the TraceOut method would typically be at the very end of an instrumented code, before the code returns its result (if any).
        /// </summary>
        /// <param name="outParameters">The method parameters which will be included into the traced event (make sure you do not supply any sensitive data).</param>
        [DebuggerStepThrough()]
        public void TraceOut(params object[] outParameters)
        {
#if DEBUG
            string methodDetails = GetMethodDetails(GetCallingMethod());
            string parameterList = outParameters != null & outParameters.Length > 0 ? GetParameterList(outParameters) : NoReturnValue;

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceOutNoToken, methodDetails, parameterList);
#endif
            // Only invoke the tracing code if ETW provider is in Enabled state. We should not be walking on the call stack or performing any kind of formatting 
            // with the parameter list if the tracing provider is disabled. This will save us some processor cycles and ensure that extensive TraceIn and TraceOut
            // will not affect the application performance when tracing is not required.
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceOutNoToken, methodDetails, parameterList);
#else
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceOutNoToken, GetMethodDetails(GetCallingMethod()), outParameters != null & outParameters.Length > 0 ? GetParameterList(outParameters) : NoReturnValue);
#endif
            }
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating that a method is about to complete. This can be useful for tracing method calls to help analyze the 
        /// code execution flow. The method will also write the same event into default System.Diagnostics trace listener, however this will only occur in the DEBUG code.
        /// A call to the TraceOut method would typically be at the very end of an instrumented code, before the code returns its result (if any).
        /// This method is provided to ensure optimal performance when no parameters are required to be traced.
        /// </summary>
        [DebuggerStepThrough()]
        public void TraceOut()
        {
#if DEBUG
            string methodDetails = GetMethodDetails(GetCallingMethod());

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceOutNoTokenAndParams, methodDetails, null);
#endif
            // Only invoke the tracing code if ETW provider is in Enabled state. We should not be walking on the call stack or performing any kind of formatting 
            // with the parameter list if the tracing provider is disabled. This will save us some processor cycles and ensure that extensive TraceIn and TraceOut
            // will not affect the application performance when tracing is not required.
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceOutNoTokenAndParams, methodDetails, null);
#else
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceOutNoTokenAndParams, GetMethodDetails(GetCallingMethod()));
#endif
            }
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating that a method is about to complete. This can be useful for tracing method calls to help analyze the 
        /// code execution flow. The method will also write the same event into default System.Diagnostics trace listener, however this will only occur in the DEBUG code.
        /// A call to the TraceOut method would typically be at the very end of an instrumented code, before the code returns its result (if any).
        /// </summary>
        /// <param name="callToken">An unique value which is used as a correlation token to correlate TraceIn and TraceOut calls.</param>
        /// <param name="outParameters">The method parameters which will be included into the traced event (make sure you do not supply any sensitive data).</param>
        [DebuggerStepThrough()]
        public void TraceOut(Guid callToken, params object[] outParameters)
        {
#if DEBUG
            string methodDetails = GetMethodDetails(GetCallingMethod());
            string parameterList = outParameters != null & outParameters.Length > 0 ? GetParameterList(outParameters) : NoReturnValue;

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceOut, methodDetails, parameterList, callToken);
#endif
            // Only invoke the tracing code if ETW provider is in Enabled state. We should not be walking on the call stack or performing any kind of formatting 
            // with the parameter list if the tracing provider is disabled. This will save us some processor cycles and ensure that extensive TraceIn and TraceOut
            // will not affect the application performance when tracing is not required.
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceOut, methodDetails, parameterList, callToken);
#else
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceOut, GetMethodDetails(GetCallingMethod()), outParameters != null & outParameters.Length > 0 ? GetParameterList(outParameters) : NoReturnValue, callToken);
#endif
            }
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating a start of a scope for which duration will be measured.
        /// </summary>
        /// <param name="scope">A textual identity of a scope for which duration will be traced.</param>
        /// <param name="parameters">A list containing zero or more data items to be included into scope details.</param>
        /// <returns>The number of ticks that represent the date and time when it was invoked. This date/time will be used later when tracing the end of the scope.</returns>
        [DebuggerStepThrough()]
        public long TraceStartScope(string scope, params object[] parameters)
        {
#if DEBUG
            string paramList = GetParameterList(parameters);

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceScopeStart, scope, paramList);
#endif
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeStart, scope, paramList);
#else
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeStart, scope, GetParameterList(parameters));
#endif
            }
            return this.highResTimer.TickCount;
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating the start of a scope for which duration will be measured.
        /// This method is provided in order to ensure optimal performance when no parameters are available for tracing.
        /// </summary>
        /// <param name="scope">A textual identity of a scope for which duration will be traced.</param>
        /// <param name="parameters">A list containing zero or more data items to be included into scope details.</param>
        /// <returns>The number of ticks that represent the date and time when it was invoked. This date/time will be used later when tracing the end of the scope.</returns>
        [DebuggerStepThrough()]
        public long TraceStartScope(string scope)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceScopeStartNoParams, scope);
#endif
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeStartNoParams, scope);
            }
            return this.highResTimer.TickCount;
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating the start of a scope for which duration will be measured.
        /// This method is provided in order to ensure optimal performance when only 1 parameter of type Guid is available for tracing.
        /// </summary>
        /// <param name="scope">A textual identity of a scope for which duration will be traced.</param>
        /// <param name="callToken">An unique value which is used as a correlation token to correlate TraceStartScope and TraceEndScope calls.</param>
        /// <returns>The number of ticks that represent the date and time when it was invoked. This date/time will be used later when tracing the end of the scope.</returns>
        public long TraceStartScope(string scope, Guid callToken)
        {
#if DEBUG
            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceScopeStart, scope, callToken);
#endif
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeStart, scope, callToken);
            }

            return this.highResTimer.TickCount;
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating the end of a scope for which duration will be measured.
        /// </summary>
        /// <param name="scope">A textual identity of a scope for which duration will be traced.</param>
        /// <param name="startTicks">The number of ticks that represent the date and time when the code entered the scope.</param>
        /// <returns>The calculated duration.</returns>
        [DebuggerStepThrough()]
        public long TraceEndScope(string scope, long startTicks)
        {
            long duration = 0;
#if DEBUG
            duration = this.highResTimer.GetElapsedMilliseconds(startTicks);

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceScopeEndNoParams, scope, duration);
#endif
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeEndNoParams, scope, duration);
#else
                duration = this.highResTimer.GetElapsedMilliseconds(startTicks);

                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeEndNoParams, scope, duration);
#endif
            }

            return duration;
        }

        /// <summary>
        /// Writes an informational event into the trace log indicating the end of a scope for which duration will be measured.
        /// </summary>
        /// <param name="scope">A textual identity of a scope for which duration will be traced.</param>
        /// <param name="startTicks">The number of ticks that represent the date and time when the code entered the scope.</param>
        /// <param name="callToken">An unique value which is used as a correlation token to correlate TraceStartScope and TraceEndScope calls.</param>
        /// <returns>The calculated duration.</returns>
        [DebuggerStepThrough()]
        public long TraceEndScope(string scope, long startTicks, Guid callToken)
        {
            long duration = 0;
#if DEBUG
            duration = this.highResTimer.GetElapsedMilliseconds(startTicks);

            // Write a formatted message into the active trace listeners such as DebugView.
            // This method will only be invoked in a DEBUG build. Once code is compiled in the RELEASE mode, this method call will be omited.
            DebugTimeTrace(FormatStringTraceScopeEnd, scope, callToken, duration);
#endif
            if (IsEnabled && (this.traceProvider.Flags & EtwTraceLevel.Tracking) != 0)
            {
#if DEBUG
                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeEnd, scope, callToken, duration);
#else
                duration = this.highResTimer.GetElapsedMilliseconds(startTicks);

                TraceMessage(EtwTraceLevel.Tracking, FormatStringTraceScopeEnd, scope, callToken, duration);
#endif
            }

            return duration;
        }
        #endregion

        #region Private methods
        private void TraceMessage(uint traceLevel, string format, params object[] parameters)
        {
            if (parameters == null)
            {
                this.traceProvider.TraceMessage(traceLevel, format);
            }
            else
            {
                switch (parameters.Length)
                {
                    case 0:
                        this.traceProvider.TraceMessage(traceLevel, format);
                        return;

                    case 1:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0]);
                        return;

                    case 2:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0], parameters[1]);
                        return;

                    case 3:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0], parameters[1], parameters[2]);
                        return;

                    case 4:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0], parameters[1], parameters[2], parameters[3]);
                        return;

                    case 5:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
                        return;

                    case 6:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5]);
                        return;

                    case 7:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6]);
                        return;

                    case 8:
                        this.traceProvider.TraceMessage(traceLevel, format, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4], parameters[5], parameters[6], parameters[7]);
                        return;

                    default:
                        throw new ArgumentOutOfRangeException("parameters", parameters.Length, String.Format(CultureInfo.InvariantCulture, "ExceptionMessages.ParameterListTooLong", 8));
                }
            }
        }

        /// <summary>
        /// Returns a string representing a list of parameters to be written into trace log.
        /// </summary>
        /// <param name="parameters">Parameters to be included in the trace log entry.</param>
        /// <returns>A comma-separated list of formatted parameters.</returns>
        private static string GetParameterList(object[] parameters)
        {
            // Make sure we have a parameter array which is safe to pass to Array.ConvertAll.
            if (null == parameters)
            {
                parameters = new object[] { null };
            }

            // Get a string representation of each parameter that we have been passed.
            string[] paramStrings = Array.ConvertAll<object, string>(parameters, ParameterObjectToString);

            // Create a string containing details of each parameter.
            return String.Join(", ", paramStrings);
        }

        private static string ParameterObjectToString(object parameter)
        {
            if (null == parameter)
            {
                return NullParameterValue;
            }

            // Surround string values with quotes.
            if (parameter.GetType() == typeof(string))
            {
                return "\"" + (string)parameter + "\"";
            }

            return parameter.ToString();
        }

        private static string GetMethodDetails(MethodBase callingMethod)
        {
            // Compose and return fully qualified method name.
            return callingMethod != null ? String.Format(CultureInfo.InvariantCulture, "{0}.{1}", callingMethod.DeclaringType.FullName, callingMethod.Name) : UnknownMethodName;
        }

        /// <summary>
        /// Determines if the specified method is part of the tracing library.
        /// </summary>
        /// <param name="methodToCheck">MethodBase describing the method to check.</param>
        /// <returns>True if the method is a member of the tracing library, false if not.</returns>
        private static bool IsTracingMethod(MethodBase methodToCheck)
        {
            return methodToCheck.DeclaringType == typeof(ComponentTraceProvider);
        }

        /// <summary>
        /// Walks the call stack to find the name of the method which invoked this class.
        /// </summary>
        /// <returns>MethodBase representing the most recent method in the stack which is not a member of this class.</returns>
        private static MethodBase GetCallingMethod()
        {
            StackTrace stack = new StackTrace();

            foreach (StackFrame frame in stack.GetFrames())
            {
                MethodBase frameMethod = frame.GetMethod();
                if (!IsTracingMethod(frameMethod))
                {
                    return frameMethod;
                }
            }

            return null;
        }

        [Conditional("DEBUG")]
        private static void DebugTimeTrace(string format, params object[] parameters)
        {
            if (parameters != null && parameters.Length > 0)
            {
                Trace.WriteLine(String.Format(format, parameters));
            }
            else
            {
                Trace.WriteLine(format);
            }
        }

        [Conditional("DEBUG")]
        private static void DebugTimeTrace(string message)
        {
            Trace.WriteLine(message);
        }
        #endregion
    }
}
