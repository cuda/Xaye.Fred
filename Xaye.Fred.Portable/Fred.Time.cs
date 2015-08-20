

using System;
using NodaTime;

namespace Xaye.Fred
{
    public partial class Fred
    {
        private static readonly DateTimeZone TimeZone = DateTimeZoneProviders.Tzdb["America/Chicago"];

        /// <summary>
        /// Gets the current time as a DateTime with a Central Standard Time timezone (St. Louis).
        /// </summary>
        /// <returns>A DateTime with a Central Standard Time timezone (St. Louis).</returns>
        public static DateTime CstTime()
        {
            return SystemClock.Instance.Now.InZone(TimeZone).ToDateTimeUnspecified();
        }
    }
}
