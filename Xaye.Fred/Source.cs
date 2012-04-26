using System;

namespace Xaye.Fred
{
    /// <summary>
    /// The source of an economic release.
    /// </summary>
    /// <remarks>See http://api.stlouisfed.org/docs/fred/realtime_period.html for information 
    /// about real-time periods.</remarks>    
    public class Source
    {
        /// <summary>
        /// When retrieving data, what to order the data by. Defaults to OrderBy.SeriesId.
        /// </summary>
        public enum OrderBy
        {
            SourceId,
            Name,
            RealtimeStart,
            RealtimeEnd
        }

        /// <summary>
        /// The source's ID.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The source's name.
        /// </summary>
        public string Name { get; set; }
       
        /// <summary>
        /// A link to information about the source.
        /// </summary>
        public string Link { get; set; }
        
        /// <summary>
        /// The start of the real-time period. 
        /// </summary>
        public DateTime RealtimeStart { get; set; }
       
        /// <summary>
        /// The end of the real-time period.
        /// </summary>
        public DateTime RealtimeEnd { get; set; }
        
        /// <summary>
        /// Notes about the source.
        /// </summary>
        public string Notes { get; set; }
    }
}