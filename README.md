#Xaye.Fred

Xaye.Fred is a simple .NET wrapper around the Federal Reserve Economic Data (FRED) API. The library is part of the Xaye project, but doesn't depend on any other parts of Xaye (or any other library). It is licensed under the permissive Simplified BSD license. 

## Installation
Available via NuGet: https://nuget.org/packages/Xaye.Fred

	PM> Install-Package Xaye.Fred

To use the library, you must have a FRED API key. A key can be obtained from [http://api.stlouisfed.org/api_key.html](http://api.stlouisfed.org/api_key.html)

## Usage
Create a Xaye.Fred object:

	var fred = new Fred("api key");

Calls using the FRED API default values:

	IEnumerable<Release> GetReleases() -> http://api.stlouisfed.org/docs/fred/releases.html
	Release GetRelease(int releaseId) -> http://api.stlouisfed.org/docs/fred/release.html
	IEnumerable<ReleaseDate> GetReleasesDates() -> http://api.stlouisfed.org/docs/fred/releases_dates.html
	IEnumerable<ReleaseDate> GetReleaseDates(int releaseId) -> http://api.stlouisfed.org/docs/fred/release_dates.html
	IEnumerable<Series> GetReleaseSeries(int releaseId) -> http://api.stlouisfed.org/docs/fred/release_series.html
	IEnumerable<Source> GetReleaseSources(int releaseId) -> http://api.stlouisfed.org/docs/fred/release_sources.html
	Category GetCategory(int category) -> http://api.stlouisfed.org/docs/fred/category.html
	IEnumerable<Category> GetCategoryRelated(int categoryId) -> http://api.stlouisfed.org/docs/fred/category_related.html
	IEnumerable<Category> GetCategoryChildern(int categoryId) -> http://api.stlouisfed.org/docs/fred/category_children.html
	IEnumerable<Series> GetCategorySeries(int categoryId) -> http://api.stlouisfed.org/docs/fred/category_series.html
	Series GetSeries(string seriesId) -> http://api.stlouisfed.org/docs/fred/series.html
	IEnumerable<Category> GetSeriesCategories(string seriesId) -> http://api.stlouisfed.org/docs/fred/series_categories.html
	Release GetSeriesRelease(string seriesId) -> http://api.stlouisfed.org/docs/fred/release.html
	IEnumerable<Series> GetSeriesSearch(string searchText) -> http://api.stlouisfed.org/docs/fred/series_search.html
	IEnumerable<Series> GetSeriesUpdates() -> http://api.stlouisfed.org/docs/fred/series_updates.html
	IEnumerable<DateTime> GetSeriesVintageDates(string seriesId) -> http://api.stlouisfed.org/docs/fred/series_vintagedates.html
	IEnumerable<Observation> GetSeriesObservations(string seriesId) -> http://api.stlouisfed.org/docs/fred/series_observations.html
	IEnumerable<Source> GetSources() -> http://api.stlouisfed.org/docs/fred/sources.html
	Source GetSource(int sourceId) -> http://api.stlouisfed.org/docs/fred/source.html
	IEnumerable<Release> GetSourceReleases(int sourceId) -> http://api.stlouisfed.org/docs/fred/source_releases.html

The library caches calls for Categories and Series (it returns a cached version if the object has already been retrieved from the FRED database). If you don't want a Fred instance to cache them, set caching to false in the constructor
	
	var fred = new Fred("api key", false);

If caching in enabled, you can clear it at anytime by calling ClearCache().
	
	fred.ClearCache();

## Notes
1. Overloaded versions are provided so user's can override FRED's default values. 
2. Where properties on Release, Category, Source, and Series objects return an enumeration, the enumeration is lazily loaded. That is, a FRED API call is not made until that property is accessed.
3. .NET 4.5 asynchronous versions of the methods above are available in the project source but not the NuGet package.
