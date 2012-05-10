using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xaye.Fred
{
    /// <summary>A release of economic data.</summary>
    /// <remarks>See http://api.stlouisfed.org/docs/fred/realtime_period.html for information 
    /// about real-time periods.</remarks>
    public class Release : Item, IEnumerable<Series>
    {
        #region OrderBy enum
        /// <summary>
        /// What to order releases by.
        /// </summary>
        public enum OrderBy
        {
            ReleaseId,
            Name,
            PressRelease,
            RealtimeStart,
            RealtimeEnd
        }

        #endregion

        private volatile List<Series> _series;

        internal Release(Fred fred) : base(fred)
        {
        }

        /// <summary>
        /// The release's id.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The release's name.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// A link to information about the release.
        /// </summary>
        public string Link { get; set; }
       
        /// <summary>
        /// Does the link provide a press release.
        /// </summary>
        public bool PressRelease { get; set; }
       
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
        /// <see cref="Series"/> in the release.
        /// </summary>
        public IEnumerable<Series> Series
        {
            get
            {
                if (_series == null)
                {
                    lock (Lock)
                    {
                        if (_series == null)
                        {
                            const int limit = 1000;
                            _series = (List<Series>) Fred.GetReleaseSeries(Id, RealtimeStart, RealtimeEnd, limit, 0);
                            var count = _series.Count;
                            var call = 1;
                            while (count == limit)
                            {
                                var more = (List<Series>) Fred.GetReleaseSeries(Id, DateTime.Today, DateTime.Today, limit, call*limit);
                                _series.AddRange(more);
                                count = more.Count;
                                call++;
                            }
                        }
                    }
                }

                return _series;
            }
        }

        #region IEnumerable<Series> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<Series> GetEnumerator()
        {
            return Series.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}