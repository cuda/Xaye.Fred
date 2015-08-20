using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.String;

namespace Xaye.Fred
{
    public partial class Fred
    {
        /// <summary>
        /// Gets the current time as a DateTime with a Central Standard Time timezone (St. Louis).
        /// </summary>
        /// <returns>A DateTime with a Central Standard Time timezone (St. Louis).</returns>
        public static DateTime CstTime()
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, TimeZoneInfo.Utc.Id, "Central Standard Time");
        }

        /// <summary>
        /// Get the observations or data values for an economic data series. Corresponds to http://api.stlouisfed.org/fred/series/observations
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="fileType">The type of file to send.</param>
        /// <param name="filename">The where to save the file.</param>
        /// <param name="observationStart">The start of the observation period.</param>
        /// <param name="observationEnd">The end of the observation period.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="vintageDates">A comma separated string of YYYY-MM-DD formatted dates in history (e.g. 2000-01-01,2005-02-24).
        ///  Vintage dates are used to download data as it existed on these specified dates in history. Vintage dates can be 
        ///  specified instead of a real-time period using realtime_start and realtime_end.
        ///
        /// Sometimes it may be useful to enter a vintage date that is not a date when the data values were revised. 
        /// For instance you may want to know the latest available revisions on 2001-09-11 (World Trade Center and 
        /// Pentagon attacks) or as of a Federal Open Market Committee (FOMC) meeting date. Entering a vintage date is 
        /// also useful to compare series on different releases with different release dates.</param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 100000, optional, default: 100000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="order">Sort results is ascending or descending observation_date order.</param>
        /// <param name="transformation">Type of data value transformation.</param>
        /// <param name="frequency">An optional parameter that indicates a lower frequency to aggregate values to. 
        /// The FRED frequency aggregation feature converts higher frequency data series into lower frequency data series 
        /// (e.g. converts a monthly data series into an annual data series). In FRED, the highest frequency data is daily, 
        /// and the lowest frequency data is annual. There are 3 aggregation methods available- average, sum, and end of period.</param>
        /// <param name="method">A key that indicates the aggregation method used for frequency aggregation.</param>
        /// <param name="outputType">Output type:
        /// 1. Observations by Real-Time Period
        /// 2. Observations by Vintage Date, All Observations
        /// 3. Observations by Vintage Date, New and Revised Observations Only
        /// 4.  Observations, Initial Release Only
        /// </param>
        /// <returns>Observations or data values for an economic data series.</returns>
        public void GetSeriesObservationsFile(string seriesId, FileType fileType, string filename,
            DateTime observationStart,
            DateTime observationEnd,
            DateTime realtimeStart,
            DateTime realtimeEnd,
            IEnumerable<DateTime> vintageDates, int limit = 100000,
            int offset = 0, SortOrder order = SortOrder.Ascending,
            Transformation transformation = Transformation.None,
            Frequency frequency = Frequency.None,
            AggregationMethod method = AggregationMethod.Average,
            OutputType outputType = OutputType.RealTime)
        {
            var extension = GetExtension(filename);

            if (fileType != FileType.Xml && fileType != FileType.Json && !extension.Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                filename += ".zip";
            }

            GetSeriesObservationsStreamAsync(seriesId, fileType, new FileStream(filename, FileMode.Create), observationStart, observationEnd,
                realtimeStart, realtimeEnd, vintageDates, limit, offset, order, transformation, frequency, method, outputType).RunSynchronously();
        }

        private static string GetExtension(string filename)
        {
            if (IsNullOrWhiteSpace(filename))
            {
                return Empty;
            }
            var index = filename.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase);
            if (index == -1 || index == filename.Length)
            {
                return Empty;
            }

            return filename.Substring(index + 1, filename.Length - index - 1);
        }

        /// <summary>
        /// Get the observations or data values for an economic data series using system defaults except for the date range. 
        /// Corresponds to http://api.stlouisfed.org/fred/series/observations
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="fileType">The type of file to send.</param>
        /// <param name="filename">The where to save the file.</param>
        /// <param name="observationStart">The start of the observation period.</param>
        /// <param name="observationEnd">The end of the observation period.</param>
        /// <returns>Observations or data values for an economic data series.</returns>
        public void GetSeriesObservationsFile(string seriesId, FileType fileType, string filename,
            DateTime observationStart,
            DateTime observationEnd)
        {
            GetSeriesObservationsFile(seriesId, fileType, filename, observationStart, observationEnd, CstTime(),
                CstTime(), Enumerable.Empty<DateTime>());
        }

        /// <summary>
        /// Get the observations or data values for an economic data series using system defaults. 
        /// Corresponds to http://api.stlouisfed.org/fred/series/observations
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="fileType">The type of file to send.</param>
        /// <param name="filename">The where to save the file.</param>
        /// <returns>Observations or data values for an economic data series.</returns>
        public void GetSeriesObservationsFile(string seriesId, FileType fileType, string filename)
        {
            GetSeriesObservationsFile(seriesId, fileType, filename, new DateTime(1776, 07, 04),
                new DateTime(9999, 12, 31));
        }
    }
}