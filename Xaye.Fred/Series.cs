using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Xaye.Fred
{
    /// <summary>
    ///   An economic data series
    /// </summary>
    /// <remarks>
    ///   See http://api.stlouisfed.org/docs/fred/realtime_period.html for information 
    ///   about real-time periods.
    /// </remarks>
    public class Series : Item, IEnumerable<Observation>
    {
        #region FilterBy enum

        /// <summary>
        ///   What to filter by when retrieving series. Defaults to FilterBy.None.
        /// </summary>
        public enum FilterBy
        {
            None,
            Frequency,
            Units,
            SeasonalAdjustment
        }

        #endregion

        #region OrderBy enum

        /// <summary>
        ///   When retrieving data, what to order the data by. Defaults to OrderBy.SeriesId.
        /// </summary>
        public enum OrderBy
        {
            SeriesId,
            Title,
            Units,
            Frequency,
            SeasonalAdjustment,
            RealtimeStart,
            RealtimeEnd,
            LastUpdated,
            ObservationStart,
            ObservationEnd,
            Popularity,
            SearchRank
        }

        #endregion

        #region UpdateFilterBy enum

        /// <summary>
        ///   What to filter by when retrieving series updates. Defaults to FilterBy.All.
        /// </summary>
        public enum UpdateFilterBy
        {
            /// <summary>
            ///   Does not filter results.
            /// </summary>
            All,

            /// <summary>
            ///   Representing the entire United States.
            /// </summary>
            Macro,

            /// <summary>
            ///   Limits results to series for parts of the US; namely, series for US states, counties, and Metropolitan Statistical Areas (MSA).
            /// </summary>
            Regional
        }

        #endregion

        private volatile IEnumerable<Category> _categories;
        private volatile List<Observation> _data;
        private volatile Release _release;

        internal Series(Fred fred) : base(fred)
        {
        }

        /// <summary>
        ///   Gets the series id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///   Gets the series title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///   Gets the start of the real-time period.
        /// </summary>
        public DateTime RealtimeStart { get; set; }

        /// <summary>
        ///   Gets the end of the real-time period.
        /// </summary>
        public DateTime RealtimeEnd { get; set; }

        /// <summary>
        ///   Gets the start of the observation period..
        /// </summary>
        public DateTime ObservationStart { get; set; }

        /// <summary>
        ///   Gets the end of the observation period.
        /// </summary>
        public DateTime ObservationEnd { get; set; }

        /// <summary>
        ///   Gets the series's frequency.
        /// </summary>
        public Frequency Frequency { get; set; }

        /// <summary>
        ///   Gets the series unit of measurement.
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        ///   Whether to use <see cref="RealtimeStart" /> and <see cref="RealtimeEnd" /> 
        ///   when making FRED calls (such as getting observations) or not. If false,
        ///   then the current day is used.
        /// </summary>
        public bool UseRealtimeFields { get; set; }

        /// <summary>
        ///   Gets a value indicating whether series is seasonally adjusted.
        /// </summary>
        public bool SeasonalAdjusted { get; set; }

        /// <summary>
        ///   Gets the date the series was last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        ///   Gets the series popularity.
        /// </summary>
        public int Popularity { get; set; }

        /// <summary>
        ///   Gets the release the series belongs to. Lazily loaded.
        /// </summary>
        public Release Release
        {
            get
            {
                if (_release == null)
                {
                    lock (Lock)
                    {
                        if (_release == null)
                        {
                            _release = UseRealtimeFields
                                           ? Fred.GetSeriesRelease(Id, RealtimeStart, RealtimeEnd)
                                           : Fred.GetSeriesRelease(Id);
                        }
                    }
                }

                return _release;
            }
        }

        /// <summary>
        ///   Gets the categories the series belongs to. Lazily loaded.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public IEnumerable<Category> Categories
        {
            get
            {
                if (_categories == null)
                {
                    lock (Lock)
                    {
                        if (_categories == null)
                        {
                            _categories = UseRealtimeFields
                                              ? Fred.GetSeriesCategories(Id, RealtimeStart, RealtimeEnd)
                                              : Fred.GetSeriesCategories(Id);
                        }
                    }
                }
                return _categories;
            }
        }

        /// <summary>
        ///   Gets the series observations. Lazily loaded.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public IEnumerable<Observation> Observations
        {
            get
            {
                if (_data == null)
                {
                    lock (Lock)
                    {
                        if (_data == null)
                        {
                            const int limit = 100000;
                            _data = UseRealtimeFields
                                        ? (List<Observation>)
                                          Fred.GetSeriesObservations(Id, ObservationStart, ObservationEnd, RealtimeStart,
                                                                     RealtimeEnd, Enumerable.Empty<DateTime>())
                                        : (List<Observation>)
                                          Fred.GetSeriesObservations(Id, ObservationStart, ObservationEnd);

                            var count = _data.Count;
                            var call = 1;
                            while (count == limit)
                            {
                                var start = UseRealtimeFields ? RealtimeStart : Fred.CstTime();
                                var end = UseRealtimeFields ? RealtimeEnd : Fred.CstTime();
                                var more =
                                    (List<Observation>)
                                    Fred.GetSeriesObservations(Id, ObservationStart, ObservationEnd, start,
                                                               end, Enumerable.Empty<DateTime>(), limit,
                                                               call*limit);
                                _data.AddRange(more);
                                count = more.Count;
                                call++;
                            }
                        }
                    }
                }

                return _data;
            }
        }

        #region IEnumerable<Observation> Members

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection. </returns>
        /// <remarks>
        /// </remarks>
        public IEnumerator<Observation> GetEnumerator()
        {
            return Observations.GetEnumerator();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection. </returns>
        /// <remarks>
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}