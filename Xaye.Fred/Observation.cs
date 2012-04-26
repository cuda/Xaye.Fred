using System;

namespace Xaye.Fred
{
    /// <summary>
    /// A series observation.
    /// </summary>
    /// <remarks>See http://api.stlouisfed.org/docs/fred/realtime_period.html for information 
    /// about real-time periods.</remarks>
    public class Observation
    {
        /// <summary>
        /// The start of the real-time period.
        /// </summary>
        public DateTime RealtimeStart { get; set; }
      
        /// <summary>
        /// The end of the real-time period. 
        /// </summary>
        public DateTime RealtimeEnd { get; set; }
        
        /// <summary>
        /// The date the observation covers.
        /// </summary>
        public DateTime Date { get; set; }
        
        /// <summary>
        /// The value of the observation.
        /// </summary>
        public double? Value { get; set; }
    }
}