namespace Xaye.Fred
{
    internal static class Urls
    {
        public const string BaseUrl = "https://api.stlouisfed.org/fred/";

        public const string Releases =
            BaseUrl +
            "releases?api_key={0}&realtime_start={1}&realtime_end={2}&limit={3}&offset={4}&order_by={5}&sort_order={6}";

        public const string Release =
            BaseUrl +
            "release?api_key={0}&release_id={1}&realtime_start={2}&realtime_end={3}";

        public const string ReleaseSeries =
            BaseUrl +
            "release/series?api_key={0}&release_id={1}&realtime_start={2}&realtime_end={3}&limit={4}&offset={5}&order_by={6}&sort_order={7}&filter_variable={8}&filter_value={9}";

        public const string ReleasesDates =
            BaseUrl +
            "releases/dates?api_key={0}&realtime_start={1}&realtime_end={2}&limit={3}&offset={4}&order_by={5}&sort_order={6}&include_release_dates_with_no_data={7}";

        public const string ReleaseDates =
            BaseUrl +
            "release/dates?api_key={0}&release_id={1}&realtime_start={2}&realtime_end={3}&limit={4}&offset={5}&sort_order={6}&include_release_dates_with_no_data={7}";

        public const string ReleaseSources =
            BaseUrl +
            "release/sources?api_key={0}&release_id={1}&realtime_start={2}&realtime_end={3}";

        public const string Series =
            BaseUrl + "series?api_key={0}&series_id={1}&realtime_start={2}&realtime_end={3}";

        public const string SeriesCategories =
            BaseUrl + "series/categories?api_key={0}&series_id={1}&realtime_start={2}&realtime_end={3}";

        public const string SeriesRelease =
            BaseUrl + "series/release?api_key={0}&series_id={1}&realtime_start={2}&realtime_end={3}";

        public const string SeriesSearch =
            BaseUrl +
            "series/search?api_key={0}&realtime_start={1}&realtime_end={2}&limit={3}&offset={4}&order_by={5}&sort_order={6}&filter_variable={7}&filter_value={8}&search_type={9}&search_text={10}";

        public const string SeriesUpdates =
            BaseUrl +
            "series/updates?api_key={0}&realtime_start={1}&realtime_end={2}&limit={3}&offset={4}&filter_value={5}";

        public const string SeriesVintageDates =
            BaseUrl +
            "series/vintagedates?api_key={0}&series_id={1}&realtime_start={2}&realtime_end={3}&limit={4}&offset={5}&sort_order={6}";

        public const string SeriesObservations =
            BaseUrl +
            "series/observations?api_key={0}&series_id={1}&realtime_start={2}&realtime_end={3}&limit={4}&offset={5}&sort_order={6}&observation_start={7}&observation_end={8}&units={9}&frequency={10}&aggregation_method={11}&output_type={12}&file_type={13}&vintage_dates={14}";

        public const string Category = BaseUrl + "category?api_key={0}&category_id={1}";

        public const string CategoryRelated =
            BaseUrl + "category/related?api_key={0}&category_id={1}&realtime_start={2}&realtime_end={3}";

        public const string CategoryChildern =
            BaseUrl + "category/children?api_key={0}&category_id={1}&realtime_start={2}&realtime_end={3}";

        public const string CategorySeries =
            BaseUrl +
            "category/series?api_key={0}&category_id={1}&realtime_start={2}&realtime_end={3}&limit={4}&offset={5}&order_by={6}&sort_order={7}&filter_variable={8}&filter_value={9}";

        public const string Sources =
            BaseUrl +
            "sources?api_key={0}&realtime_start={1}&realtime_end={2}&limit={3}&offset={4}&order_by={5}&sort_order={6}";

        public const string Source =
            BaseUrl + "source?api_key={0}&source_id={1}&realtime_start={2}&realtime_end={3}";

        public const string SourceReleases =
            BaseUrl +
            "source/releases?api_key={0}&source_id={1}&realtime_start={2}&realtime_end={3}&limit={4}&offset={5}&order_by={6}&sort_order={7}";
    }
}