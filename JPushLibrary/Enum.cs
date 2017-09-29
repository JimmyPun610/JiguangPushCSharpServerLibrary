using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPushLibrary
{
    public class Enum
    {
        public enum Platform
        {
            iOSDevelopment = 1,
            iOSProduction = 2,
            Android = 4,
            iOSDevelopmentAndAndroid = 5,
            iOSProductionAndAndroid = 6,
        }

        public enum AndroidPriority
        {
            PRIORITY_DEFAULT = 0,
            PRIORITY_HIGH = 1,
            PRIORITY_LOW = -1,
            PRIORITY_MAX = 2,
            PRIORITY_MIN = -2
        }
        public enum AndroidAlertType
        {
            All = -1,
            SoundOnly = 1,
            VibrateOnly = 2,
            SoundAndVibrate = 3,
            LightOnly = 4,
            SoundAndLight = 5,
            VibrateAndLight = 6
        }
    }
}
