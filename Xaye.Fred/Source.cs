using System;
using System.Collections;
using System.Collections.Generic;

namespace Xaye.Fred
{
    /// <summary>
    /// The source of an economic release.
    /// </summary>
    /// <remarks>See http://api.stlouisfed.org/docs/fred/realtime_period.html for information 
    /// about real-time periods.</remarks>    
    public class Source : Item, IEnumerable<Release>
    {
        #region OrderBy enum

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

        #endregion

        private List<Release> _releases;

        internal Source(Fred fred) : base(fred)
        {
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
        /// Provides an enumerator over the
        /// <see cref="Release"/> by the source.
        /// </summary>
        public IEnumerable<Release> Releases
        {
            get
            {
                lock (Lock)
                {
                    if (_releases == null)
                    {
                        const int limit = 1000;
                        _releases = (List<Release>) Fred.GetSourceReleases(Id);
                        var count = _releases.Count;
                        var call = 1;
                        while (count == limit)
                        {
                            var more = (List<Release>)Fred.GetSourceReleases(Id, DateTime.Today, DateTime.Today, limit, call*limit);
                            _releases.AddRange(more);
                            count = more.Count;
                            call++;
                        }
                    }
                }

                return _releases;
            }
        }

        #region IEnumerable<Release> Members

        public IEnumerator<Release> GetEnumerator()
        {
            return Releases.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}