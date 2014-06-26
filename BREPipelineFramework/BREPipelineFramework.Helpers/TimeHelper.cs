using System;

namespace BREPipelineFramework.Helpers
{
    public static class TimeHelper
    {
        public static int GetTimeInMilliseconds(int expiryTime, TimeEnum expiryUnits)
        {
            switch(expiryUnits)
            {
                case TimeEnum.Seconds:
                    return expiryTime * 1000;
                case TimeEnum.Minutes:
                    return expiryTime * 60 * 1000;
                case TimeEnum.Hours:
                    return expiryTime * 60 * 60 * 1000;
                case TimeEnum.Days:
                    return expiryTime *24 * 60 * 60 * 1000;
                default: 
                    return expiryTime;
            }
        }
    }
}
