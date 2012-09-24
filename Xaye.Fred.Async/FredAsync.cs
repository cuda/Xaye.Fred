using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Xaye.Fred
{
    public partial class Fred
    {
        /// <summary>
        ///   Get all releases of economic data. Corresponds to http://api.stlouisfed.org/fred/releases
        /// </summary>
        /// <param name="realtimeStart"> The start of the real-time period. </param>
        /// <param name="realtimeEnd"> The end of the real-time period. </param>
        /// <param name="limit"> The maximum number of results to return. An integer between 1 and 1000, defaults to 1000. </param>
        /// <param name="offset"> non-negative integer, optional, default: 0 </param>
        /// <param name="orderBy"> Order results by values of the specified attribute. </param>
        /// <param name="order"> Sort results is ascending or descending order for attribute values specified by order_by. </param>
        /// <returns> All releases in the FRED database. </returns>
        public async Task<IEnumerable<Release>> GetReleasesAsync(DateTime realtimeStart, DateTime realtimeEnd,
                                                                 int limit = 1000,
                                                                 int offset = 0,
                                                                 Release.OrderBy orderBy = Release.OrderBy.ReleaseId,
                                                                 SortOrder order = SortOrder.Ascending)
        {
            var url = String.Format(Urls.Releases, _key, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(orderBy),
                                    Extensions.ToString(order));

            var root = await GetRootAsync(url);
            return root.Elements("release").Select(CreateRelease).ToList();
        }

        /// <summary>
        /// Get all releases of economic data using system defaults. Corresponds to http://api.stlouisfed.org/fred/releases
        /// </summary>
        /// <returns>All releases in the FRED database.</returns>
        public async Task<IEnumerable<Release>> GetReleasesAsync()
        {
            return await GetReleasesAsync(DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get a release of economic data. Corresponds to http://api.stlouisfed.org/fred/release
        /// </summary>
        /// <param name="releaseId">The ID of the release to retrieve.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period. </param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 1000, defaults to 1000. </param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="orderBy">Order results by values of the specified attribute.</param>
        /// <param name="order">Sort results is ascending or descending order for attribute values specified by order_by.</param>
        /// <returns>The requested release.</returns>
        /// <remarks>Note that release dates are published by data sources and do not necessarily represent when data will be available on the FRED or ALFRED websites.</remarks>

        public async Task<Release> GetReleaseAsync(int releaseId, DateTime realtimeStart, DateTime realtimeEnd, int limit = 1000,
                                  int offset = 0,
                                  Release.OrderBy orderBy = Release.OrderBy.ReleaseId,
                                  SortOrder order = SortOrder.Ascending)
        {
            var url = String.Format(Urls.Release, _key, releaseId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            var root = await GetRootAsync(url);
            var element = root.Elements("release").First();
            return CreateRelease(element);
        }

        /// <summary>
        /// Get a release of economic data using system defaults. Corresponds to http://api.stlouisfed.org/fred/release
        /// </summary>
        /// <param name="releaseId">The ID of the release to retrieve.</param>
        /// <returns>The requested release.</returns>
        /// <remarks>Note that release dates are published by data sources and do not necessarily represent when data will be available on the FRED or ALFRED websites.</remarks>
        public async Task<Release> GetReleaseAsync(int releaseId)
        {
            return await GetReleaseAsync(releaseId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get release dates for all releases of economic data. Corresponds to http://api.stlouisfed.org/fred/releases/dates
        /// </summary>
        /// <param name="realtimeStart">The start of the real-time period. </param>
        /// <param name="realtimeEnd">The end of the real-time period. </param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 1000, optional, default: 1000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="orderBy">Order results by values of the specified attribute.</param>
        /// <param name="order">Sort results is ascending or descending release date order.</param>
        /// <param name="includeReleaseWithNoData">Determines whether release dates with no data available are returned. The default value 'false' excludes release dates that do not have data.</param>
        /// <returns>Release dates for all releases of economic data.</returns>
        public async Task<IEnumerable<ReleaseDate>> GetReleasesDatesAsync(DateTime realtimeStart, DateTime realtimeEnd, int limit = 1000,
                                                         int offset = 0,
                                                         Release.OrderBy orderBy = Release.OrderBy.ReleaseId,
                                                         SortOrder order = SortOrder.Ascending,
                                                         bool includeReleaseWithNoData = false)
        {
            var url = String.Format(Urls.ReleasesDates, _key, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(orderBy),
                                    Extensions.ToString(order),
                                    includeReleaseWithNoData.ToString(CultureInfo.GetCultureInfo("en-US")).
                                        ToLowerInvariant());

            var root = await GetRootAsync(url);
            return (from releaseDate in root.Elements("release_date")
                    select new ReleaseDate
                    {
                        ReleaseId = int.Parse(releaseDate.Attribute("release_id").Value),
                        ReleaseName = releaseDate.Attribute("release_name").Value,
                        Date = releaseDate.Value.ToFredDate()
                    }).ToList();
        }

        /// <summary>
        /// Get release dates for all releases of economic data using system defaults. Corresponds to http://api.stlouisfed.org/fred/releases/dates
        /// </summary>
        /// <returns>Release dates for all releases of economic data.</returns>
        /// <remarks>Note that release dates are published by data sources and do not necessarily represent when data will be available on the FRED or ALFRED websites.</remarks>    
        public async Task<IEnumerable<ReleaseDate>> GetReleasesDatesAsync()
        {
            return await GetReleasesDatesAsync(DateTime.Today, DateTime.Today);
        }


        /// <summary>
        /// Get release dates for a release of economic data. Corresponds to http://api.stlouisfed.org/fred/release/dates
        /// </summary>
        /// <param name="releaseId">The id for a release.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="limit">The maximum number of results to return. Integer between 1 and 10000, optional, default: 10000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="order">Sort results is ascending or descending release date order.</param>
        /// <param name="includeReleaseWithNoData">Determines whether release dates with no data available are returned. The defalut value 'false' excludes release dates that do not have data.</param>
        /// <returns>Release dates for a release of economic data</returns>
        /// <remarks>Note that release dates are published by data sources and do not necessarily represent when data will be available on the FRED or ALFRED websites.</remarks>
        public async Task<IEnumerable<ReleaseDate>> GetReleaseDatesAsync(int releaseId, DateTime realtimeStart, DateTime realtimeEnd,
                                                        int limit = 1000,
                                                        int offset = 0,
                                                        SortOrder order = SortOrder.Ascending,
                                                        bool includeReleaseWithNoData = false)
        {
            var url = String.Format(Urls.ReleaseDates, _key, releaseId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(order),
                                    includeReleaseWithNoData.ToString(CultureInfo.GetCultureInfo("en-US")).
                                        ToLowerInvariant());

            var root = await GetRootAsync(url);
            return (from releaseDate in root.Elements("release_date")
                    select new ReleaseDate
                    {
                        ReleaseId = int.Parse(releaseDate.Attribute("release_id").Value),
                        ReleaseName = "",
                        Date = releaseDate.Value.ToFredDate()
                    }).ToList();
        }

        /// <summary>
        /// Get release dates for a release of economic data using system defaults. Corresponds to http://api.stlouisfed.org/fred/release/dates
        /// </summary>
        /// <param name="releaseId">The id for a release.</param>
        /// <returns>Release dates for a release of economic data</returns>
        /// <remarks>Note that release dates are published by data sources and do not necessarily represent when data will be available on the FRED or ALFRED websites.</remarks>
        public async Task<IEnumerable<ReleaseDate>> GetReleaseDatesAsync(int releaseId)
        {
            return await GetReleaseDatesAsync(releaseId, new DateTime(1776, 7, 4), DateTime.Today);
        }

        /// <summary>
        /// Get the series on a release of economic data. Corresponds to http://api.stlouisfed.org/fred/release/series
        /// </summary>
        /// <param name="releaseId">The id for a release.</param>
        /// <param name="realtimeStart">The start of the real-time period</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 1000, optional, default: 1000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="orderBy">Order results by values of the specified attribute.</param>
        /// <param name="order">Sort results is ascending or descending order for attribute values specified by order_by.</param>
        /// <param name="filter">The attribute to filter results by.</param>
        /// <param name="filterValue">The value of the filter_variable attribute to filter results by.</param>
        /// <returns>The series on a release of economic data. </returns>
        public async Task<IEnumerable<Series>> GetReleaseSeriesAsync(int releaseId, DateTime realtimeStart, DateTime realtimeEnd,
                                                    int limit = 1000,
                                                    int offset = 0,
                                                    Series.OrderBy orderBy = Series.OrderBy.SeriesId,
                                                    SortOrder order = SortOrder.Ascending,
                                                    Series.FilterBy filter = Series.FilterBy.None,
                                                    string filterValue = "")
        {
            var url = String.Format(Urls.ReleaseSeries, _key, releaseId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(orderBy),
                                    Extensions.ToString(order), Extensions.ToString(filter), filterValue);

            return await CreateSeriesAsync(url);
        }


        /// <summary>
        /// Get the series on a release of economic data. Corresponds to http://api.stlouisfed.org/fred/release/series
        /// </summary>
        /// <param name="releaseId">The id for a release.</param>
        /// <returns>The series on a release of economic data. </returns>
        public async Task<IEnumerable<Series>> GetReleaseSeriesAsync(int releaseId)
        {
            return await GetReleaseSeriesAsync(releaseId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get the sources for a release of economic data. Corresponds to http://api.stlouisfed.org/fred/release/sources
        /// </summary>
        /// <param name="releaseId">The id for a release.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <returns>The sources for a release of economic data.</returns>
        public async Task<IEnumerable<Source>> GetReleaseSourcesAsync(int releaseId, DateTime realtimeStart, DateTime realtimeEnd)
        {
            var url = String.Format(Urls.ReleaseSources, _key, releaseId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            return await CreateSourcesAsync(url);
        }

        /// <summary>
        /// Get the sources for a release of economic data using system defaults. Corresponds to http://api.stlouisfed.org/fred/release/sources
        /// </summary>
        /// <param name="releaseId">The id for a release.</param>
        /// <returns>The sources for a release of economic data.</returns>
        public async Task<IEnumerable<Source>> GetReleaseSourcesAsync(int releaseId)
        {
            return await GetReleaseSourcesAsync(releaseId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get a category. Corresponds to http://api.stlouisfed.org/fred/category
        /// </summary>
        /// <param name="id">The id for a category.</param>
        /// <returns>The requested category.</returns>
        public async Task<Category> GetCategoryAsync(int id)
        {
            if (_categoryCache.ContainsKey(id))
            {
                return _categoryCache[id];
            }

            var url = String.Format(Urls.Category, _key, id);

            var root = await GetRootAsync(url);
            var element = root.Elements("category").First();
            return CreateCategory(element);
        }

        /// <summary>
        /// Get the related categories for a category. A related category is a one-way relation between 2 categories that is not part of a parent-child category hierarchy. 
        /// Most categories do not have related categories.
        /// Corresponds to http://api.stlouisfed.org/fred/category/related
        /// </summary>
        /// <param name="categoryId">The id for a category.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <returns>The related categories for a category.</returns>
        public async Task<IEnumerable<Category>> GetCategoryRelatedAsync(int categoryId, DateTime realtimeStart, DateTime realtimeEnd)
        {
            var url = String.Format(Urls.CategoryRelated, _key, categoryId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            return await CreateCategoriesAsync(url);
        }

        /// <summary>
        /// Get the related categories for a category using system defaults. 
        /// A related category is a one-way relation between 2 categories that is not part of a parent-child category hierarchy. 
        /// Most categories do not have related categories.
        /// Corresponds to http://api.stlouisfed.org/fred/category/related
        /// </summary>
        /// <param name="categoryId">The id for a category.</param>
        /// <returns>The related categories for a category.</returns>
        public async Task<IEnumerable<Category>> GetCategoryRelatedAsync(int categoryId)
        {
            return await GetCategoryRelatedAsync(categoryId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get the child categories for a specified parent category. Corresponds to http://api.stlouisfed.org/fred/category/children
        /// </summary>
        /// <param name="categoryId">The id for a category.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <returns>The child categories for a specified parent category</returns>
        public async Task<IEnumerable<Category>> GetCategoryChildernAsync(int categoryId, DateTime realtimeStart, DateTime realtimeEnd)
        {
            var url = String.Format(Urls.CategoryChildern, _key, categoryId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            return await CreateCategoriesAsync(url);
        }

        /// <summary>
        /// Get the child categories for a specified parent category. Corresponds to http://api.stlouisfed.org/fred/category/children
        /// </summary>
        /// <param name="categoryId">The id for a category.</param>
        /// <returns>The child categories for a specified parent category</returns>        
        public async Task<IEnumerable<Category>> GetCategoryChildernAsync(int categoryId)
        {
            return await GetCategoryChildernAsync(categoryId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get the series in a category. Corresponds to http://api.stlouisfed.org/fred/category/series
        /// </summary>
        /// <param name="categoryId">The id for a category.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 1000, optional, default: 1000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="orderBy">Order results by values of the specified attribute.</param>
        /// <param name="order">Sort results is ascending or descending order for attribute values specified by order_by.</param>
        /// <param name="filter">The attribute to filter results by.</param>
        /// <param name="filterValue">The value of the filter_variable attribute to filter results by.</param>
        /// <returns>The series in a category.</returns>
        public async Task<IEnumerable<Series>> GetCategorySeriesAsync(int categoryId, DateTime realtimeStart, DateTime realtimeEnd,
                                                     int limit = 1000,
                                                     int offset = 0,
                                                     Series.OrderBy orderBy = Series.OrderBy.SeriesId,
                                                     SortOrder order = SortOrder.Ascending,
                                                     Series.FilterBy filter = Series.FilterBy.None,
                                                     string filterValue = "")
        {
            var url = String.Format(Urls.CategorySeries, _key, categoryId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(orderBy),
                                    Extensions.ToString(order), Extensions.ToString(filter), filterValue);
            return await CreateSeriesAsync(url);
        }

        /// <summary>
        /// Get the series in a category using system defaults. Corresponds to http://api.stlouisfed.org/fred/category/series
        /// </summary>
        /// <param name="categoryId">The id for a category.</param>
        /// <returns>The series in a category.</returns>
        public async Task<IEnumerable<Series>> GetCategorySeriesAsync(int categoryId)
        {
            return await GetCategorySeriesAsync(categoryId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get an economic data series. Corresponds to http://api.stlouisfed.org/fred/series
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <returns>An economic data series.</returns>
        public async Task<Series> GetSeriesAsync(string seriesId, DateTime realtimeStart, DateTime realtimeEnd)
        {
            var url = String.Format(Urls.Series, _key, seriesId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            var root = await GetRootAsync(url);
            var element = root.Elements("series").First();
            return CreateSeries(element);
        }

        /// <summary>
        /// Get an economic data series using system defaults. Corresponds to http://api.stlouisfed.org/fred/series
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <returns>An economic data series.</returns>
        public async Task<Series> GetSeriesAsync(string seriesId)
        {
            return await GetSeriesAsync(seriesId, DateTime.Today, DateTime.Today);
        }

        /// <summary> 
        /// Get the categories for an economic data series. Corresponds to http://api.stlouisfed.org/fred/series/categories
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <returns>The categories for an economic data series.</returns>
        public async Task<IEnumerable<Category>> GetSeriesCategoriesAsync(string seriesId, DateTime realtimeStart, DateTime realtimeEnd)
        {
            var url = String.Format(Urls.SeriesCategories, _key, seriesId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            return await CreateCategoriesAsync(url);
        }

        /// <summary> 
        /// Get the categories for an economic data series using system defaults. Corresponds to http://api.stlouisfed.org/fred/series/categories
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <returns>The categories for an economic data series.</returns>        
        public async Task<IEnumerable<Category>> GetSeriesCategoriesAsync(string seriesId)
        {
            return await GetSeriesCategoriesAsync(seriesId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get the release for an economic data series. Corresponds to http://api.stlouisfed.org/fred/series/release
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <returns>The release for an economic data series.</returns>
        public async Task<Release> GetSeriesReleaseAsync(string seriesId, DateTime realtimeStart, DateTime realtimeEnd)
        {
            var url = String.Format(Urls.SeriesRelease, _key, seriesId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            var root = await GetRootAsync(url);
            var element = root.Elements("release").First();
            return CreateRelease(element);
        }

        /// <summary>
        /// Get the release for an economic data series using system defaults. Corresponds to http://api.stlouisfed.org/fred/series/release
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <returns>The release for an economic data series.</returns>
        public async Task<Release> GetSeriesReleaseAsync(string seriesId)
        {
            return await GetSeriesReleaseAsync(seriesId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get economic data series that match keywords. Corresponds to http://api.stlouisfed.org/fred/series/search
        /// </summary>
        /// <param name="searchText">The words to match against economic data series.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="type">Determines the type of search to perform.
        /// 'full_text' searches series attributes title, units, frequency, and notes by parsing words into stems. 
        /// This makes it possible for searches like 'Industry' to match series containing related words such as 'Industries'. 
        /// Of course, you can search for multiple words like 'money' and 'stock'. Remember to url encode spaces (e.g. 'money%20stock').
        /// 'series_id' performs a substring search on series IDs. Searching for 'ex' will find series containing 'ex' anywhere in
        ///  a series ID. '*' can be used to anchor searches and match 0 or more of any character. Searching for 'ex*' will 
        /// find series containing 'ex' at the beginning of a series ID. Searching for '*ex' will find series containing 
        /// 'ex' at the end of a series ID. It's also possible to put an '*' in the middle of a string. 'm*sl' finds any 
        /// series starting with 'm' and ending with 'sl'.
        /// optional, default: full_text.</param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 1000, optional, default: 1000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="orderBy">Order results by values of the specified attribute.</param>
        /// <param name="order">Sort results is ascending or descending order for attribute values specified by order_by.</param>
        /// <param name="filter">The attribute to filter results by.</param>
        /// <param name="filterValue">The value of the filter_variable attribute to filter results by.</param>
        /// <returns>Economic data series that match keywords</returns>
        public async Task<IEnumerable<Series>> GetSeriesSearchAsync(string searchText, DateTime realtimeStart, DateTime realtimeEnd,
                                                   SearchType type = SearchType.FullText,
                                                   int limit = 1000,
                                                   int offset = 0,
                                                   Series.OrderBy orderBy = Series.OrderBy.SearchRank,
                                                   SortOrder order = SortOrder.Ascending,
                                                   Series.FilterBy filter = Series.FilterBy.None,
                                                   string filterValue = "")
        {
            var url = String.Format(Urls.SeriesSearch, _key, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(orderBy),
                                    Extensions.ToString(order), Extensions.ToString(filter),
                                    filterValue, Extensions.ToString(type), Uri.EscapeUriString(searchText));

            return await CreateSeriesAsync(url);
        }

        /// <summary>
        /// Get economic data series that match keywords using system defaults. Corresponds to http://api.stlouisfed.org/fred/series/search
        /// </summary>
        /// <param name="searchText">The words to match against economic data series.</param>
        /// <param name="type">Determines the type of search to perform.
        /// 'full_text' searches series attributes title, units, frequency, and notes by parsing words into stems. 
        /// This makes it possible for searches like 'Industry' to match series containing related words such as 'Industries'. 
        /// Of course, you can search for multiple words like 'money' and 'stock'. Remember to url encode spaces (e.g. 'money%20stock').
        /// 'series_id' performs a substring search on series IDs. Searching for 'ex' will find series containing 'ex' anywhere in
        ///  a series ID. '*' can be used to anchor searches and match 0 or more of any character. Searching for 'ex*' will 
        /// find series containing 'ex' at the beginning of a series ID. Searching for '*ex' will find series containing 
        /// 'ex' at the end of a series ID. It's also possible to put an '*' in the middle of a string. 'm*sl' finds any 
        /// series starting with 'm' and ending with 'sl'.
        /// optional, default: full_text.</param>
        /// <returns>Economic data series that match keywords</returns>
        public async Task<IEnumerable<Series>> GetSeriesSearchAsync(string searchText, SearchType type = SearchType.FullText)
        {
            return await GetSeriesSearchAsync(searchText, DateTime.Today, DateTime.Today, type);
        }

        /// <summary>
        /// Get economic data series sorted in descending order of when transactions to update data values where processed on the FRED® server.
        /// Corresponds  to http://api.stlouisfed.org/fred/series/updates
        /// </summary>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period. </param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 100, optional, default: 100</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="filter">Limit results by geographic type of economic data series; namely 'macro', 'regional', and 'all'.
        /// String, optional, default: 'all' meaning no filter.
        /// One of the values: 'macro', 'regional', 'all'
        /// 'macro' limits results to macroeconomic data series. In other words, series representing the entire United States. 
        /// 'regional' limits results to series for parts of the US; namely, series for US states, counties, and Metropolitan 
        /// Statistical Areas (MSA). 'all' does not filter results.</param>
        /// <returns>Economic data series.</returns>
        public async Task<IEnumerable<Series>> GetSeriesUpdatesAsync(DateTime realtimeStart, DateTime realtimeEnd,
                                                    int limit = 100,
                                                    int offset = 0,
                                                    Series.UpdateFilterBy filter = Series.UpdateFilterBy.All)
        {
            var url = String.Format(Urls.SeriesUpdates, _key, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(filter));
            return await CreateSeriesAsync(url);
        }

        /// <summary>
        /// Get economic data series sorted in descending order of when transactions to update data values where processed on the FRED® server
        /// using system defaults.
        /// Corresponds  to http://api.stlouisfed.org/fred/series/updates
        /// </summary>
        /// <returns>An economic data series.</returns>
        public async Task<IEnumerable<Series>> GetSeriesUpdatesAsync()
        {
            return await GetSeriesUpdatesAsync(DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get the dates in history when a series' data values were revised or new data values were released. Vintage dates are 
        /// the release dates for a series excluding release dates when the data for a series did not change.
        /// Corresponds to http://api.stlouisfed.org/fred/series/vintagedates
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 10000, optional, default: 10000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="order">Sort results is ascending or descending vintage_date order.</param>
        /// <returns>The dates in history when a series' data values were revised or new data values were released.</returns>
        public async Task<IEnumerable<DateTime>> GetSeriesVintageDatesAsync(string seriesId, DateTime realtimeStart, DateTime realtimeEnd,
                                                           int limit = 10000,
                                                           int offset = 0,
                                                           SortOrder order = SortOrder.Ascending)
        {
            var url = String.Format(Urls.SeriesVintageDates, _key, seriesId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset,
                                    Extensions.ToString(order));
            var root = await GetRootAsync(url);
            return (from date in root.Elements("vintage_date")
                    select date.Value.ToFredDate()).ToList();
        }

        /// <summary>
        /// Get the dates in history when a series' data values were revised or new data values were released using
        /// system defaults.  Vintage dates are  the release dates for a series excluding release dates when the data 
        /// for a series did not change.
        /// Corresponds to http://api.stlouisfed.org/fred/series/vintagedates
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <returns>The dates in history when a series' data values were revised or new data values were released.</returns>
        public async Task<IEnumerable<DateTime>> GetSeriesVintageDatesAsync(string seriesId)
        {
            return await GetSeriesVintageDatesAsync(seriesId, new DateTime(1776, 07, 04), new DateTime(9999, 12, 31));
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
        public async Task GetSeriesObservationsFileAsync(string seriesId, FileType fileType, string filename,
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
            var vintageString = vintageDates.Aggregate(string.Empty,
                                                       (current, vintageDate) =>
                                                       current + (vintageDate.ToFredDateString() + ","));
            if (vintageString.Length > 0)
            {
                vintageString = vintageString.Substring(0, vintageString.Length - 1);
            }

            var realtimeStartStr = (vintageString.Length == 0) ? realtimeStart.ToFredDateString() : string.Empty;
            var realtimeEndStr = (vintageString.Length == 0) ? realtimeEnd.ToFredDateString() : string.Empty;
            var url = String.Format(Urls.SeriesObservations, _key, seriesId, realtimeStartStr,
                                    realtimeEndStr, limit, offset, Extensions.ToString(order),
                                    observationStart.ToFredDateString(),
                                    observationEnd.ToFredDateString(),
                                    Extensions.ToString(transformation), Extensions.ToString(frequency),
                                    Extensions.ToString(method), Extensions.ToString(outputType),
                                    Extensions.ToString(fileType), vintageString);

            try
            {
                if (fileType != FileType.Xml && !Path.GetExtension(filename).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    filename += ".zip";
                }
                await _downloader.DownloadFileAsync(url, filename);
            }
            catch (WebException exp)
            {
                var response = (HttpWebResponse)exp.Response;
                var buffer = new byte[response.ContentLength];
                response.GetResponseStream().Read(buffer, 0, buffer.Length);
                var xml = Encoding.UTF8.GetString(buffer);
                var start = xml.IndexOf("message=", StringComparison.OrdinalIgnoreCase);
                if (start < 0)
                {
                    throw;
                }
                start += 9;
                var end = xml.LastIndexOf("\"", StringComparison.OrdinalIgnoreCase);
                var message = xml.Substring(start, end - start);
                throw new FredExecption(message, exp);
            }
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
        public async Task GetSeriesObservationsFileAsync(string seriesId, FileType fileType, string filename,
                                              DateTime observationStart,
                                              DateTime observationEnd)
        {
            await GetSeriesObservationsFileAsync(seriesId, fileType, filename, observationStart, observationEnd, DateTime.Today,
                                      DateTime.Today, Enumerable.Empty<DateTime>());
        }

        /// <summary>
        /// Get the observations or data values for an economic data series using system defaults. 
        /// Corresponds to http://api.stlouisfed.org/fred/series/observations
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="fileType">The type of file to send.</param>
        /// <param name="filename">The where to save the file.</param>
        /// <returns>Observations or data values for an economic data series.</returns>
        public async Task GetSeriesObservationsFileAsync(string seriesId, FileType fileType, string filename)
        {
            await GetSeriesObservationsFileAsync(seriesId, fileType, filename, new DateTime(1776, 07, 04),
                                      new DateTime(9999, 12, 31));
        }

        /// <summary>
        /// Get the observations or data values for an economic data series. Corresponds to http://api.stlouisfed.org/fred/series/observations
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
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
        public async Task<IEnumerable<Observation>> GetSeriesObservationsAsync(string seriesId, DateTime observationStart,
                                                              DateTime observationEnd, DateTime realtimeStart,
                                                              DateTime realtimeEnd,
                                                              IEnumerable<DateTime> vintageDates, int limit = 100000,
                                                              int offset = 0, SortOrder order = SortOrder.Ascending,
                                                              Transformation transformation = Transformation.None,
                                                              Frequency frequency = Frequency.None,
                                                              AggregationMethod method = AggregationMethod.Average,
                                                              OutputType outputType = OutputType.RealTime)
        {
            var vintageString = vintageDates.Aggregate(string.Empty,
                                                       (current, vintageDate) =>
                                                       current + (vintageDate.ToFredDateString() + ","));
            if (vintageString.Length > 0)
            {
                vintageString = vintageString.Substring(0, vintageString.Length - 1);
            }

            var realtimeStartStr = (vintageString.Length == 0) ? realtimeStart.ToFredDateString() : string.Empty;
            var realtimeEndStr = (vintageString.Length == 0) ? realtimeEnd.ToFredDateString() : string.Empty;
            var url = String.Format(Urls.SeriesObservations, _key, seriesId, realtimeStartStr,
                                    realtimeEndStr, limit, offset, Extensions.ToString(order),
                                    observationStart.ToFredDateString(),
                                    observationEnd.ToFredDateString(),
                                    Extensions.ToString(transformation), Extensions.ToString(frequency),
                                    Extensions.ToString(method), Extensions.ToString(outputType), "xml", vintageString);

            var root = await GetRootAsync(url);
            return root.Elements("observation").Select(
                element =>
                {
                    var valElm = element.Attribute("value");
                    double? value = null;
                    if (valElm != null && !string.IsNullOrWhiteSpace(valElm.Value))
                    {
                        double dOut;
                        var success = double.TryParse(valElm.Value, out dOut);
                        if (success)
                        {
                            value = dOut;
                        }

                    }

                    return new Observation
                    {
                        RealtimeStart = element.Attribute("realtime_start").Value.ToFredDate(),
                        RealtimeEnd = element.Attribute("realtime_end").Value.ToFredDate(),
                        Date = element.Attribute("date").Value.ToFredDate(),
                        Value = value
                    };
                }
                ).ToList();
        }

        /// <summary>
        /// Get the observations or data values for an economic data series using system defaults except for the date range. 
        /// Corresponds to http://api.stlouisfed.org/fred/series/observations
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <param name="observationStart">The start of the observation period.</param>
        /// <param name="observationEnd">The end of the observation period.</param>
        /// <returns>Observations or data values for an economic data series.</returns>        
        public async Task<IEnumerable<Observation>> GetSeriesObservationsAsync(string seriesId, DateTime observationStart,
                                                              DateTime observationEnd)
        {
            return await GetSeriesObservationsAsync(seriesId, observationStart, observationEnd, DateTime.Today, DateTime.Today,
                                         Enumerable.Empty<DateTime>());
        }

        /// <summary>
        /// Get the observations or data values for an economic data series using system defaults. 
        /// Corresponds to http://api.stlouisfed.org/fred/series/observations
        /// </summary>
        /// <param name="seriesId">The id for a series.</param>
        /// <returns>Observations or data values for an economic data series.</returns>
        public async Task<IEnumerable<Observation>> GetSeriesObservationsAsync(string seriesId)
        {
            return await GetSeriesObservationsAsync(seriesId, new DateTime(1776, 07, 04), new DateTime(9999, 12, 31));
        }

        /// <summary>
        /// Get all sources of economic data. Corresponds to http://api.stlouisfed.org/fred/sources
        /// </summary>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 1000, optional, default: 1000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="orderBy">Order results by values of the specified attribute.</param>
        /// <param name="order">Sort results is ascending or descending order for attribute values specified by order_by.</param>
        /// <returns>All sources of economic data.</returns>
        public async Task<IEnumerable<Source>> GetSourcesAsync(DateTime realtimeStart, DateTime realtimeEnd,
                                              int limit = 1000,
                                              int offset = 0,
                                              Source.OrderBy orderBy = Source.OrderBy.SourceId,
                                              SortOrder order = SortOrder.Ascending)
        {
            var url = String.Format(Urls.Sources, _key, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(orderBy),
                                    Extensions.ToString(order));

            return await CreateSourcesAsync(url);
        }

        /// <summary>
        /// Get all sources of economic data using system defaults. Corresponds to http://api.stlouisfed.org/fred/sources
        /// </summary>
        /// <returns>All sources of economic data.</returns> 
        public async Task<IEnumerable<Source>> GetSourcesAsync()
        {
            return await GetSourcesAsync(DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get a source of economic data. Corresponds to http://api.stlouisfed.org/fred/source
        /// </summary>
        /// <param name="sourceId">The id for a source.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <returns>A source of economic data.</returns>
        public async Task<Source> GetSourceAsync(int sourceId, DateTime realtimeStart, DateTime realtimeEnd)
        {
            var url = String.Format(Urls.Source, _key, sourceId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString());

            var root = await GetRootAsync(url);
            var element = root.Elements("source").First();
            return CreateSource(element);
        }

        /// <summary>
        /// Get a source of economic data using system defaults. Corresponds to http://api.stlouisfed.org/fred/source
        /// </summary>
        /// <param name="sourceId">The id for a source.</param>
        /// <returns>A source of economic data.</returns>
        public async Task<Source> GetSourceAsync(int sourceId)
        {
            return await GetSourceAsync(sourceId, DateTime.Today, DateTime.Today);
        }

        /// <summary>
        /// Get the releases for a source. Corresponds to http://api.stlouisfed.org/fred/source/releases
        /// </summary>
        /// <param name="sourceId">The id for a source.</param>
        /// <param name="realtimeStart">The start of the real-time period.</param>
        /// <param name="realtimeEnd">The end of the real-time period.</param>
        /// <param name="limit">The maximum number of results to return. An integer between 1 and 1000, optional, default: 1000</param>
        /// <param name="offset">non-negative integer, optional, default: 0</param>
        /// <param name="orderBy">Order results by values of the specified attribute.</param>
        /// <param name="order">Sort results is ascending or descending order for attribute values specified by order_by.</param>
        /// <returns>The releases for a source.</returns>
        public async Task<IEnumerable<Release>> GetSourceReleasesAsync(int sourceId, DateTime realtimeStart, DateTime realtimeEnd,
                                                      int limit = 1000,
                                                      int offset = 0,
                                                      Release.OrderBy orderBy = Release.OrderBy.ReleaseId,
                                                      SortOrder order = SortOrder.Ascending)
        {
            var url = String.Format(Urls.SourceReleases, _key, sourceId, realtimeStart.ToFredDateString(),
                                    realtimeEnd.ToFredDateString(), limit, offset, Extensions.ToString(orderBy),
                                    Extensions.ToString(order));

            return await CreateReleasesAsync(url);
        }

        /// <summary>
        /// Get the releases for a source using system defaults. Corresponds to http://api.stlouisfed.org/fred/source/releases
        /// </summary>
        /// <param name="sourceId">The id for a source.</param>
        /// <returns>The releases for a source.</returns>
        public async Task<IEnumerable<Release>> GetSourceReleasesAsync(int sourceId)
        {
            return await GetSourceReleasesAsync(sourceId, DateTime.Today, DateTime.Today);
        }


        private async Task<IEnumerable<Source>> CreateSourcesAsync(string url)
        {
            var root = await GetRootAsync(url);
            return root.Elements("source").Select(CreateSource).ToList();
        }

        private async Task<IEnumerable<Series>> CreateSeriesAsync(string url)
        {
            var root = await GetRootAsync(url);
            return root.Elements("series").Select(CreateSeries).ToList();
        }

        private async Task<IEnumerable<Category>> CreateCategoriesAsync(string url)
        {
            var root = await GetRootAsync(url);
            return root.Elements("category").Select(CreateCategory).ToList();
        }

        private async Task<IEnumerable<Release>> CreateReleasesAsync(string url)
        {
            var root = await GetRootAsync(url);
            return root.Elements("release").Select(CreateRelease).ToList();
        }

        private async Task<XElement> GetRootAsync(string url)
        {
            XElement root;
            try
            {
                var response = await _downloader.DownloadAsync(url);
                root = XDocument.Load(new StringReader(response)).Root;
            }
            catch (WebException exp)
            {
                throw CreateException(exp);
            }

            return root;
        }
    }
}