using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        private readonly Lazy<Task<IEnumerable<Category>>> _categories;
        private volatile Lazy<Task<List<Observation>>> _data;
        private volatile Lazy<Task<Release>> _release;

        internal Series(Fred fred) : base(fred)
        {
            _categories = new Lazy<Task<IEnumerable<Category>>>(async () => UseRealtimeFields
                ? await Fred.GetSeriesCategoriesAsync(Id, RealtimeStart, RealtimeEnd)
                : await Fred.GetSeriesCategoriesAsync(Id));

            _release = new Lazy<Task<Release>>(async () => UseRealtimeFields
                ? await Fred.GetSeriesReleaseAsync(Id, RealtimeStart, RealtimeEnd)
                : await Fred.GetSeriesReleaseAsync(Id));

            _data = new Lazy<Task<List<Observation>>>(
                async () =>
                {
                    const int limit = 100000;
                    var data = UseRealtimeFields
                        ? (List<Observation>)
                            await Fred.GetSeriesObservationsAsync(Id, ObservationStart, ObservationEnd, RealtimeStart,
                                RealtimeEnd, Enumerable.Empty<DateTime>())
                        : (List<Observation>)
                            await Fred.GetSeriesObservationsAsync(Id, ObservationStart, ObservationEnd);

                    var count = data.Count;
                    var call = 1;
                    while (count == limit)
                    {
                        var start = UseRealtimeFields ? RealtimeStart : Fred.CstTime();
                        var end = UseRealtimeFields ? RealtimeEnd : Fred.CstTime();
                        var more =
                            (List<Observation>)
                                await Fred.GetSeriesObservationsAsync(Id, ObservationStart, ObservationEnd, start,
                                    end, Enumerable.Empty<DateTime>(), limit,
                                    call*limit);
                        data.AddRange(more);
                        count = more.Count;
                        call++;
                    }
                    return data;
                });
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
        ///   Gets the release the series belongs to.
        /// </summary>
        [Obsolete("Please use GetRelease() instead. This property will be removed in the next release.")]
        public Release Release => GetRelease();

        /// <summary>
        ///   Gets the categories the series belongs to. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use GetCategories() instead. This property will be removed in the next release.")]
        public IEnumerable<Category> Categories => GetCategories();

        /// <summary>
        ///   Gets the series observations.
        /// </summary>
        /// <remarks>
        /// </remarks>
        [Obsolete("Please use GetObservations() instead. This property will be removed in the next release.")]
        public IEnumerable<Observation> Observations => GetObservations();

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns> A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection. </returns>
        /// <remarks>
        /// </remarks>
        public IEnumerator<Observation> GetEnumerator() => GetObservations().GetEnumerator();

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns> An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection. </returns>
        /// <remarks>
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///   Gets the release the series belongs to.
        /// </summary>
        public Release GetRelease() => _release.Value.Result;

        /// <summary>
        ///   Gets the release the series belongs to.
        /// </summary>
        public async Task<Release> GetReleaseAsync() => await _release.Value;

        /// <summary>
        ///   Gets the categories the series belongs to.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public IEnumerable<Category> GetCategories() => _categories.Value.Result;

        /// <summary>
        ///   Gets the categories the series belongs to.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public async Task<IEnumerable<Category>> GetCategoriesAsync() => await _categories.Value;

        /// <summary>
        ///   Gets the series observations.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public IEnumerable<Observation> GetObservations() => _data.Value.Result;

        /// <summary>
        ///   Gets the series observations.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public async Task<IEnumerable<Observation>> GetObservationsAsync() => await _data.Value;
    }
}