Xaye.Fred is a simple .NET wrapper around the Federal Reserve Economic Data (FRED) API. The library is part of the Xaye project, but doesn't depend on any other parts of Xaye (or any other library). It is licensed under the permissive Simplified BSD license. 

Available via NuGet: https://nuget.org/packages/Xaye.Fred

Note: A asynchronous version of the library will be released shortly after the .NET 4.5 is release.

To use the library, you must have a FRED API key. A key can be obtained from http://research.stlouisfed.org/useraccount/apikey

Usage
Create a Xaye.Fred object:
var fred = new Fred("api key");

The Fred object methods map directly to the FRED Web API call. 

Calls using the FRED API default values:
	IEnumerable<Release> GetReleases() -> http://api.stlouisfed.org/fred/releases
	Release GetRelease(int releaseId) -> http://api.stlouisfed.org/fred/release?release_id=releaseId
	IEnumerable<ReleaseDate> GetReleasesDates() -> http://api.stlouisfed.org/fred/releases/dates
	IEnumerable<ReleaseDate> GetReleaseDates(int releaseId) -> http://api.stlouisfed.org/fred/release/dates?release_id=int releaseId
	IEnumerable<Series> GetReleaseSeries(int releaseId) -> http://api.stlouisfed.org/fred/release/series?release_id=releaseId
	IEnumerable<Source> GetReleaseSources(int releaseId) -> http://api.stlouisfed.org/fred/release/sources?release_id=releaseId
	Category GetCategory(int category) -> http://api.stlouisfed.org/fred/category?category_id=category
	IEnumerable<Category> GetCategoryRelated(int categoryId) -> http://api.stlouisfed.org/fred/category/related?category_id=categoryId
	IEnumerable<Category> GetCategoryChildern(int categoryId) -> http://api.stlouisfed.org/fred/category/children?category_id=categoryId
	IEnumerable<Series> GetCategorySeries(int categoryId) -> http://api.stlouisfed.org/fred/category/series?category_id=categoryId
	Series GetSeries(string seriesId) -> http://api.stlouisfed.org/fred/category/series?category_id=categoryId
	IEnumerable<Category> GetSeriesCategories(string seriesId) -> http://api.stlouisfed.org/fred/series/categories?series_id=seriesId
	Release GetSeriesRelease(string seriesId) -> http://api.stlouisfed.org/fred/series/release?series_id=seriesId
	IEnumerable<Series> GetSeriesSearch(string searchText) -> http://api.stlouisfed.org/fred/series/search?search_text=searchText	
	IEnumerable<Series> GetSeriesUpdates() -> http://api.stlouisfed.org/fred/series/updates
	IEnumerable<DateTime> GetSeriesVintageDates(string seriesId) -> http://api.stlouisfed.org/fred/series/vintagedates?series_id=seriesId
	IEnumerable<Observation> GetSeriesObservations(string seriesId) -> http://api.stlouisfed.org/fred/series/observations?series_id=seriesId
	IEnumerable<Source> GetSources() -> http://api.stlouisfed.org/fred/sources
	Source GetSource(int sourceId) -> http://api.stlouisfed.org/fred/source?source_id=sourceId
	IEnumerable<Release> GetSourceReleases(int sourceId) -> http://api.stlouisfed.org/fred/source/releases?source_id=sourceId

Overloaded versions are provided so user's can override FRED's default values. 

Note: Where properties on Release, Category, Source, and Series objects return an enumeration, the enumeration is lazily loaded. That is, a FRED API call is not made until that property is accessed.

