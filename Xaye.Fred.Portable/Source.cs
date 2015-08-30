using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xaye.Fred
{
    /// <summary>
    /// The source of an economic release.
    /// </summary>
    /// <remarks>See http://api.stlouisfed.org/docs/fred/realtime_period.html for information 
    /// about real-time periods.</remarks>    
    public class Source : Item, IEnumerable<Release>
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

        private readonly Lazy<Task<List<Release>>> _releases;

        internal Source(Fred fred) : base(fred)
        {
            _releases = new Lazy<Task<List<Release>>>(
                async () =>
                {
                    var releases = (List<Release>) await Fred.GetSourceReleasesAsync(Id);
                    var count = releases.Count;
                    var call = 1;
                    while (count == CallLimit)
                    {
                        var more = (List<Release>) await Fred.GetSourceReleasesAsync(Id, DateTime.Today, DateTime.Today, CallLimit, call*CallLimit);
                        releases.AddRange(more);
                        count = more.Count;
                        call++;
                    }
                    return releases;
                });
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
        [Obsolete("Please use GetReleases() instead. This property will be removed in the next release.")]
        public IEnumerable<Release> Releases => GetReleases();

        public IEnumerator<Release> GetEnumerator() => GetReleases().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Provides an enumerator over the
        /// <see cref="Release"/> by the source.
        /// </summary>
        public IEnumerable<Release> GetReleases() => _releases.Value.Result;

        /// <summary>
        /// Provides an enumerator over the
        /// <see cref="Release"/> by the source.
        /// </summary>
        public async Task<IEnumerable<Release>> GetReleasesAsync() => await _releases.Value;
    }
}