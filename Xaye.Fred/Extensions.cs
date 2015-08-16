using System;
using System.Globalization;

namespace Xaye.Fred
{
    /// <summary>
    /// Utility methods.
    /// </summary>
    public static class Extensions
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm:sszz";
        
        /// <summary>
        /// Converts a string to a FRED formated date string.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>A FRED formatted date string.</returns>
        public static string ToFredDateString(this DateTime date)
        {
            return date.ToString(DateFormat);
        }

        /// <summary>
        /// Converts a FRED formatted date string to a DateTime.
        /// </summary>
        /// <param name="date">The date string to create the date from.</param>
        /// <returns>The converted date.</returns>
        public static DateTime ToFredDate(this string date)
        {
            return DateTime.ParseExact(date, DateFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a FRED formatted date and time string to a DateTime.
        /// </summary>
        /// <param name="date">The date and time string to create the date from.</param>
        /// <returns>The converted date.</returns>
        public static DateTime ToFredDateTime(this string date)
        {
            return DateTime.ParseExact(date, DateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates a string to a <see cref="Frequency"/>.
        /// </summary>
        /// <param name="frequency">The string to convert.</param>
        /// <returns>The converted frequency.</returns>
        public static Frequency ToFrequency(this string frequency)
        {
            var ret = Frequency.None;
 
            if (frequency.Equals("d"))
            {
                ret = Frequency.Daily;
            } 
            else if (frequency.Equals("w"))
            {
                ret = Frequency.Weekly;
            }
            else if (frequency.Equals("bw"))
            {
                ret = Frequency.BiWeekly;
            }
            else if (frequency.Equals("m"))
            {
                ret = Frequency.Monthly;
            }
            else if (frequency.Equals("q"))
            {
                ret = Frequency.Quarterly;
            }
            else if (frequency.Equals("sa"))
            {
                ret = Frequency.SemiAnnual;
            }
            else if (frequency.Equals("a"))
            {
                ret = Frequency.Annual;
            }
            else if (frequency.Equals("wef"))
            {
                ret = Frequency.WeeklyFriday;
            }
            else if (frequency.Equals("weth"))
            {
                ret = Frequency.WeeklyThursday;
            }
            else if (frequency.Equals("wew"))
            {
                ret = Frequency.WeeklyWednesday;
            }
            else if (frequency.Equals("wetu"))
            {
                ret = Frequency.WeeklyTuesday;
            }
            else if (frequency.Equals("wem"))
            {
                ret = Frequency.WeeklyMonday;
            }
            else if (frequency.Equals("bwew"))
            {
                ret = Frequency.BiWeeklyWednesday;
            }
            else if (frequency.Equals("bwem"))
            {
                ret = Frequency.BiWeeklyMonday;
            }

            return ret;
        }

        /// <summary>
        /// Converts a <see cref="Transformation"/> to a string.
        /// </summary>
        /// <param name="transformation">The transformation to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(this Transformation transformation)
        {
            var ret = string.Empty;
            switch (transformation)
            {
                    case Transformation.None:
                    ret = "lin";
                    break;
                    case Transformation.Change:
                    ret = "chg";
                    break;
                    case Transformation.ChangeYear:
                    ret = "ch1";
                    break;
                    case Transformation.PercentChange:
                    ret = "pch";
                    break;
                    case Transformation.PercentChangeYear:
                    ret = "pc1";
                    break;
                    case Transformation.CompoundedAnnualRateChange:
                    ret = "pca";
                    break;
                    case Transformation.ContinuouslyCompoundedRateChange:
                    ret = "cch";
                    break;
                    case Transformation.ContinuouslyCompoundedAnnualRateChange:
                    ret = "cca";
                    break;
                    case Transformation.Log:
                    ret = "log";
                    break;
            }

            return ret;
        }

        /// <summary>
        /// Converts a <see cref="Frequency"/> to a string.
        /// </summary>
        /// <param name="frequency">The frequency to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(this Frequency frequency)
        {
            var ret = "";
            switch (frequency)
            {
                case Frequency.Daily:
                    ret = "d";
                    break;
                case Frequency.Weekly:
                    ret = "w";
                    break;
                case Frequency.BiWeekly:
                    ret = "bw";
                    break;
                case Frequency.Monthly:
                    ret = "m";
                    break;
                case Frequency.Quarterly:
                    ret = "q";
                    break;
                case Frequency.SemiAnnual:
                    ret = "sa";
                    break;
                case Frequency.Annual:
                    ret = "a";
                    break;
                case Frequency.WeeklyFriday:
                    ret = "wef";
                    break;
                case Frequency.WeeklyThursday:
                    ret = "weth";
                    break;
                case Frequency.WeeklyWednesday:
                    ret = "wew";
                    break;
                case Frequency.WeeklyTuesday:
                    ret = "wetu";
                    break;
                case Frequency.WeeklyMonday:
                    ret = "wem";
                    break;
                case Frequency.BiWeeklyWednesday:
                    ret = "bwew";
                    break;
                case Frequency.BiWeeklyMonday:
                    ret = "bwem";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Converts a <see cref="SortOrder"/> to a string.
        /// </summary>
        /// <param name="order">The order to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(SortOrder order)
        {
            return order == SortOrder.Ascending ? "asc" : "desc";
        }

        /// <summary>
        /// Converts a <see cref="AggregationMethod"/> to a string.
        /// </summary>
        /// <param name="method">The aggregation method to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(AggregationMethod method)
        {
            var ret = "";
            switch (method)
            {
                    case AggregationMethod.Average:
                    ret = "avg";
                    break;
                    case AggregationMethod.Sum:
                    ret = "sum";
                    break;
                    case AggregationMethod.EndOfPeriod:
                    ret = "eop";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Converts a <see cref="OutputType"/> to a string.
        /// </summary>
        /// <param name="type">The output type to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(this OutputType type)
        {
            var ret = "";
            switch (type)
            {
                case OutputType.RealTime:
                    ret = "1";
                    break;
                case OutputType.VintageAll:
                    ret = "2";
                    break;
                case OutputType.VintageNewRevised:
                    ret = "3";
                    break;
                case OutputType.IntialReleaseOnly:
                    ret = "4";
                    break;
            }
            return ret;    
        }

        /// <summary>
        /// Converts a <see cref="FileType"/> to a string.
        /// </summary>
        /// <param name="type">The file type to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(this FileType type)
        {
            var ret = "";
            switch (type)
            {
                case FileType.Xml:
                    ret = "xml";
                    break;
                case FileType.Text:
                    ret = "txt";
                    break;
                case FileType.Xls:
                    ret = "xls";
                    break;
                case FileType.Json:
                    ret = "json";
                    break;
            }
            return ret; 
        }

        /// <summary>
        /// Converts a <see cref="SearchType"/> to a string.
        /// </summary>
        /// <param name="type">The search type to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(this SearchType type)
        {
            var ret = String.Empty;
            switch (type)
            {
                case SearchType.FullText:
                    ret = "full_text";
                    break;
                case SearchType.SeriesId:
                    ret = "series_id";
                    break;
            }
            return ret;
        }

        /// <summary>
        /// Converts a <see cref="Series.FilterBy"/> to a string.
        /// </summary>
        /// <param name="filterBy">The Series.FilterBy to convert.</param>
        /// <returns>The converted string.</returns>
        public static string ToString(this Series.FilterBy filterBy)
        {
            var filter = String.Empty;
            switch (filterBy)
            {
                case Series.FilterBy.Frequency:
                    filter = "frequency";
                    break;
                case Series.FilterBy.SeasonalAdjustment:
                    filter = "seasonal_adjustment";
                    break;
                case Series.FilterBy.Units:
                    filter = "units";
                    break;
            }

            return filter;
        }

        /// <summary>
        /// Converts a <see cref="Series.UpdateFilterBy"/> to a string.
        /// </summary>
        /// <param name="filterBy">The Series.UpdateFilterBy to convert.</param>
        /// <returns>The converted string.</returns>        
        public static string ToString(this Series.UpdateFilterBy filterBy)
        {
            var filter = "all";
            switch (filterBy)
            {
                case Series.UpdateFilterBy.Macro:
                    filter = "macro";
                    break;
                case Series.UpdateFilterBy.Regional:
                    filter = "regional";
                    break;
            }

            return filter;
        }

        /// <summary>
        /// Converts a <see cref="Series.OrderBy"/> to a string.
        /// </summary>
        /// <param name="orderBy">The Series.OrderBy to convert.</param>
        /// <returns>The converted string.</returns>                
        public static string ToString(this Series.OrderBy orderBy)
        {
            var order = String.Empty;
            switch (orderBy)
            {
                case Series.OrderBy.SeriesId:
                    order = "series_id";
                    break;
                case Series.OrderBy.Title:
                    order = "title";
                    break;
                case Series.OrderBy.Units:
                    order = "units";
                    break;
                case Series.OrderBy.Frequency:
                    order = "frequency";
                    break;
                case Series.OrderBy.SeasonalAdjustment:
                    order = "seasonal_adjustment";
                    break;
                case Series.OrderBy.RealtimeStart:
                    order = "realtime_start";
                    break;
                case Series.OrderBy.RealtimeEnd:
                    order = "realtime_end";
                    break;
                case Series.OrderBy.LastUpdated:
                    order = "last_updated";
                    break;
                case Series.OrderBy.ObservationStart:
                    order = "observation_start";
                    break;
                case Series.OrderBy.ObservationEnd:
                    order = "observation_end";
                    break;
                case Series.OrderBy.Popularity:
                    order = "popularity";
                    break;
                case Series.OrderBy.SearchRank:
                    order = "search_rank";
                    break;
            }
            return order;
        }

        /// <summary>
        /// Converts a <see cref="Release.OrderBy"/> to a string.
        /// </summary>
        /// <param name="orderBy">The Release.OrderBy to convert.</param>
        /// <returns>The converted string.</returns>      
        public static string ToString(this Release.OrderBy orderBy)
        {
            var order = String.Empty;
            switch (orderBy)
            {
                case Release.OrderBy.ReleaseId:
                    order = "release_id";
                    break;
                case Release.OrderBy.Name:
                    order = "name";
                    break;
                case Release.OrderBy.PressRelease:
                    order = "press_release";
                    break;
                case Release.OrderBy.RealtimeStart:
                    order = "realtime_start";
                    break;
                case Release.OrderBy.RealtimeEnd:
                    order = "realtime_end";
                    break;
            }
            return order;
        }

        /// <summary>
        /// Converts a <see cref="Source.OrderBy"/> to a string.
        /// </summary>
        /// <param name="orderBy">The Source.OrderBy to convert.</param>
        /// <returns>The converted string.</returns>      
        public static string ToString(Source.OrderBy orderBy)
        {
            var order = string.Empty;
            switch (orderBy)
            {
                case Source.OrderBy.SourceId:
                    order = "source_id";
                    break;
                case Source.OrderBy.Name:
                    order = "name";
                    break;
                case Source.OrderBy.RealtimeStart:
                    order = "realtime_start";
                    break;
                case Source.OrderBy.RealtimeEnd:
                    order = "realtime_end";
                    break;
            }
            return order;
        }
    }
}