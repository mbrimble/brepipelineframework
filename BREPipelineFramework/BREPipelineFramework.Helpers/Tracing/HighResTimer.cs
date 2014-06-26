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
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;

namespace BREPipelineFramework.Helpers.Tracing
{

    [ExcludeFromCodeCoverage]
    public sealed class HighResTimer
    {
        #region Private members
        private readonly static HighResTimer singleton = new HighResTimer();

        private static readonly bool isHighResolution;
        private static readonly long frequency = 0;
        private static readonly double tickFrequency;

        private const long TicksPerMillisecond = 0x2710L;
        private const string KernelLib = "Kernel32.dll";

        [DllImport(KernelLib)]
        private static extern int QueryPerformanceCounter(ref long count);
        [DllImport(KernelLib)]
        private static extern int QueryPerformanceFrequency(ref long frequency);
        #endregion

        static HighResTimer()
        {
            // Query the high-resolution timer only if it is supported.
            // A returned frequency of 1000 typically indicates that it is not
            // supported and is emulated by the OS using the same value that is
            // returned by Environment.TickCount.
            // A return value of 0 indicates that the performance counter is
            // not supported.
            int returnVal = QueryPerformanceFrequency(ref frequency);

            if (returnVal != 0 && frequency != 1000)
            {
                // The performance counter is supported.
                isHighResolution = true;
                tickFrequency = 10000000.0;
                tickFrequency /= (double)frequency;
            }
            else
            {
                // The performance counter is not supported. Use Environment.TickCount instead.
                frequency = 10000000;
                tickFrequency = 1.0;
                isHighResolution = false;
            }
        }

        public long Frequency
        {
            get { return frequency; }
        }

        public long TickCount
        {
            get
            {
                Int64 tickCount = 0;

                if (isHighResolution)
                {
                    // Get the value here if the counter is supported.
                    QueryPerformanceCounter(ref tickCount);
                    return tickCount;
                }
                else
                {
                    // Otherwise, use Environment.TickCount.
                    return (long)Environment.TickCount;
                }
            }
        }

        public static long CurrentTickCount
        {
            get
            {
                return singleton.TickCount;
            }
        }

        public long GetElapsedMilliseconds(long startTicks)
        {
            return GetElapsedDateTimeTicks(startTicks) / TicksPerMillisecond;
        }

        private long GetElapsedDateTimeTicks(long startTicks)
        {
            long rawElapsedTicks = TickCount - startTicks;

            if (isHighResolution)
            {
                double dateTimeTicks = rawElapsedTicks;
                dateTimeTicks *= tickFrequency;

                return (long)dateTimeTicks;
            }

            return rawElapsedTicks;
        }
    }
}
