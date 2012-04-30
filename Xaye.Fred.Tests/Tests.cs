using System;
using NUnit.Framework;
using System.Linq;

namespace Xaye.Fred.Tests
{
    [TestFixture]
    public class Tests
    {
        private static readonly string RealtimeNow = "&realtime_start=" + DateTime.Today.ToFredDateString() + "&realtime_end=" + DateTime.Today.ToFredDateString();

        [Test]
        public void CanGetACategory()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<categories>
<category id=""1"" name=""Production &amp; Business Activity"" parent_id=""0""/>
</categories>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var category = fred.GetCategory(1);
            const string expectedUrl = "http://api.stlouisfed.org/fred/category?api_key=key&category_id=1";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(1, category.Id);
            Assert.AreEqual("Production & Business Activity", category.Name);
            Assert.AreEqual(0, category.ParentId);
        }

        [Test]
        public void CanGetCategoryChildren()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<categories>
  <category id=""32262"" name=""Business Cycle Expansions &amp; Contractions"" parent_id=""1""/>
  <category id=""32436"" name=""Construction"" parent_id=""1""/>
  <category id=""32216"" name=""Health Insurance"" parent_id=""1""/>
  <category id=""97"" name=""Housing"" parent_id=""1""/>
  <category id=""3"" name=""Industrial Production &amp; Capacity Utilization"" parent_id=""1""/>
  <category id=""32295"" name=""Institute for Supply Management Report on Business"" parent_id=""1""/>
  <category id=""32429"" name=""Manufacturing"" parent_id=""1""/>
  <category id=""32993"" name=""Motor Vehicles"" parent_id=""1""/>
  <category id=""6"" name=""Retail Sales"" parent_id=""1""/>
</categories>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var categories = fred.GetCategoryChildern(1);
            string expectedUrl =
                "http://api.stlouisfed.org/fred/category/children?api_key=key&category_id=1" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(9, categories.Count());
        }

        [Test]
        public void CanGetCategoryRelated()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<categories>
  <category id=""149"" name=""Arkansas"" parent_id=""27281""/>
  <category id=""150"" name=""Illinois"" parent_id=""27281""/>
  <category id=""151"" name=""Indiana"" parent_id=""27281""/>
  <category id=""152"" name=""Kentucky"" parent_id=""27281""/>
  <category id=""153"" name=""Mississippi"" parent_id=""27281""/>
  <category id=""154"" name=""Missouri"" parent_id=""27281""/>
  <category id=""193"" name=""Tennessee"" parent_id=""27281""/>
</categories>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var categories = fred.GetCategoryRelated(32073);
            var expectedUrl =
                "http://api.stlouisfed.org/fred/category/related?api_key=key&category_id=32073" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(7, categories.Count());
        }

        [Test]
        public void CanGetCategorySeries()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<seriess realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" order_by=""series_id"" sort_order=""asc"" count=""2"" offset=""0"" limit=""1000"">
  <series id=""BUSINV"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Inventories: Total Business"" observation_start=""1992-01-01"" observation_end=""2012-02-01"" frequency=""Monthly, End of Period"" frequency_short=""M"" units=""Millions of Dollars, End of Period"" units_short=""Mil. of $, End of Period"" seasonal_adjustment=""Seasonally Adjusted"" seasonal_adjustment_short=""SA"" last_updated=""2012-04-16 09:46:03-05"" popularity=""62"" notes=""Effective June 14, 2001, data were reconstructed to reflect the switch from the Standard Industrial Classification (SIC) system to the North American Industry Classification System (NAICS).""/>
  <series id=""ISRATIO"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Inventory to Sales Ratio: Total Business"" observation_start=""1992-01-01"" observation_end=""2012-02-01"" frequency=""Monthly"" frequency_short=""M"" units=""Ratio"" units_short=""Ratio"" seasonal_adjustment=""Seasonally Adjusted"" seasonal_adjustment_short=""SA"" last_updated=""2012-04-16 09:46:03-05"" popularity=""69"" notes=""Effective June 14, 2001, data were reconstructed to reflect the switch from the Standard Industrial Classification (SIC) system to the North American Industry Classification System (NAICS).""/>
</seriess>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var series = fred.GetCategorySeries(1);
            var expectedUrl ="http://api.stlouisfed.org/fred/category/series?api_key=key&category_id=1" + RealtimeNow + "&limit=1000&offset=0&order_by=series_id&sort_order=asc&filter_variable=&filter_value=";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(2, series.Count());
        }

        [Test]
        public void CanGetReleases()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<releases realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" order_by=""release_id"" sort_order=""asc"" count=""132"" offset=""0"" limit=""1000"">
  <release id=""9"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Advance Monthly Sales for Retail and Food Services"" press_release=""true"" link=""http://www.census.gov/svsd/www/advtable.html""/>
  <release id=""10"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Consumer Price Index"" press_release=""true"" link=""http://www.bls.gov/cpi/""/>
  <release id=""11"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Employment Cost Index"" press_release=""true"" link=""http://www.bls.gov/ncs/ect/""/>
  <release id=""13"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.17 Industrial Production and Capacity Utilization"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g17/""/>
  <release id=""14"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.19 Consumer Credit"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g19/""/>
  <release id=""15"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.5 Foreign Exchange Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g5/""/>
  <release id=""17"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.10 Foreign Exchange Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h10/""/>
  <release id=""18"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.15 Selected Interest Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h15/""/>
  <release id=""19"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.3 Aggregate Reserves of Depository Institutions and the Monetary Base"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h3/""/>
  <release id=""20"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.4.1 Factors Affecting Reserve Balances"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h41/""/>
  <release id=""21"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.6 Money Stock Measures"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h6/""/>
  <release id=""22"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.8 Assets and Liabilities of Commercial Banks in the United States"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h8/""/>
 
  <release id=""186"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.5A Foreign Exchange Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g5a/""/>
  <release id=""187"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""St. Louis Financial Stress Index"" press_release=""false"" notes=""To obtain detailed information regarding the construction of the St. Louis Financial Stress Index, please see the online appendix at

http://research.stlouisfed.org/publications/net/NETJan2010Appendix.pdf""/>
</releases>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var releases = fred.GetReleases();
            var expectedUrl = "http://api.stlouisfed.org/fred/releases?api_key=key" + RealtimeNow + "&limit=1000&offset=0&order_by=release_id&sort_order=asc";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(14, releases.Count());
        }

        [Test]
        public void CanGetReleasesDates()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<release_dates realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" order_by=""release_id"" sort_order=""asc"" count=""1"" offset=""0"" limit=""1000"">
  <release_date release_id=""219"" release_name=""Chicago Fed National Activity Index"">2012-04-26</release_date>
</release_dates>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var dates = fred.GetReleasesDates();
            var expectedUrl = "http://api.stlouisfed.org/fred/releases/dates?api_key=key" + RealtimeNow + "&limit=1000&offset=0&order_by=release_id&sort_order=asc&include_release_dates_with_no_data=false";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(1, dates.Count());
        }

        [Test]
        public void CanGetRelease()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<releases realtime_start=""2012-04-26"" realtime_end=""2012-04-26"">
  <release id=""10"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Consumer Price Index"" press_release=""true"" link=""http://www.bls.gov/cpi/""/>
</releases>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var release = fred.GetRelease(10);
            var expectedUrl = "http://api.stlouisfed.org/fred/release?api_key=key&release_id=10" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(10, release.Id);
            Assert.AreEqual("Consumer Price Index", release.Name);
            Assert.AreEqual(true, release.PressRelease);
            Assert.AreEqual("http://www.bls.gov/cpi/", release.Link);
            Assert.AreEqual("", release.Notes);
            Assert.AreEqual(new DateTime(2012,4,26), release.RealtimeStart);
            Assert.AreEqual(new DateTime(2012, 4, 26), release.RealtimeEnd);
        }

        [Test]
        public void CanGetReleaseDates()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<release_dates realtime_start=""1776-07-04"" realtime_end=""2012-04-26"" order_by=""release_date"" sort_order=""asc"" count=""767"" offset=""0"" limit=""1000"">
  <release_date release_id=""10"">1949-03-24</release_date>
  <release_date release_id=""10"">1949-04-22</release_date>
  <release_date release_id=""10"">1949-05-23</release_date>
  <release_date release_id=""10"">1949-06-24</release_date>
  <release_date release_id=""10"">1949-07-22</release_date>
  <release_date release_id=""10"">1949-08-26</release_date>
  <release_date release_id=""10"">1949-09-28</release_date>
  <release_date release_id=""10"">1949-10-28</release_date>
 </release_dates>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var dates = fred.GetReleaseDates(10);
            string expectedUrl = "http://api.stlouisfed.org/fred/release/dates?api_key=key&release_id=10&realtime_start=1776-07-04&realtime_end=" + DateTime.Today.ToFredDateString() + "&limit=1000&offset=0&sort_order=asc&include_release_dates_with_no_data=false";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(8, dates.Count());
        }

        [Test]
        public void CanGetReleaseSeries()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<seriess realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" order_by=""series_id"" sort_order=""asc"" count=""4400"" offset=""0"" limit=""1000"">
  <series id=""CPIAPPNS"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Consumer Price Index for All Urban Consumers: Apparel"" observation_start=""1914-12-01"" observation_end=""2012-03-01"" frequency=""Monthly"" frequency_short=""M"" units=""Index 1982-84=100"" units_short=""Index 1982-84=100"" seasonal_adjustment=""Not Seasonally Adjusted"" seasonal_adjustment_short=""NSA"" last_updated=""2012-04-13 08:56:19-05"" popularity=""45""/>
  <series id=""CPIAPPSL"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Consumer Price Index for All Urban Consumers: Apparel"" observation_start=""1947-01-01"" observation_end=""2012-03-01"" frequency=""Monthly"" frequency_short=""M"" units=""Index 1982-84=100"" units_short=""Index 1982-84=100"" seasonal_adjustment=""Seasonally Adjusted"" seasonal_adjustment_short=""SA"" last_updated=""2012-04-13 09:03:50-05"" popularity=""38""/>
  <series id=""CPIAUCNS"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Consumer Price Index for All Urban Consumers: All Items"" observation_start=""1913-01-01"" observation_end=""2012-03-01"" frequency=""Monthly"" frequency_short=""M"" units=""Index 1982-84=100"" units_short=""Index 1982-84=100"" seasonal_adjustment=""Not Seasonally Adjusted"" seasonal_adjustment_short=""NSA"" last_updated=""2012-04-13 08:53:00-05"" popularity=""78"" notes=""Handbook of Methods - (http://www.bls.gov/opub/hom/pdf/homch17.pdf) Understanding the CPI: Frequently Asked Questions - (http://stats.bls.gov:80/cpi/cpifaq.htm)""/>
  <series id=""CPIAUCSL"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Consumer Price Index for All Urban Consumers: All Items"" observation_start=""1947-01-01"" observation_end=""2012-03-01"" frequency=""Monthly"" frequency_short=""M"" units=""Index 1982-84=100"" units_short=""Index 1982-84=100"" seasonal_adjustment=""Seasonally Adjusted"" seasonal_adjustment_short=""SA"" last_updated=""2012-04-13 08:49:51-05"" popularity=""100"" notes=""Handbook of Methods - (http://www.bls.gov/opub/hom/pdf/homch17.pdf) Understanding the CPI: Frequently Asked Questions - (http://stats.bls.gov:80/cpi/cpifaq.htm)""/>
  <series id=""CPIEDUNS"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Consumer Price Index for All Urban Consumers: Education &amp; Communication"" observation_start=""1993-01-01"" observation_end=""2012-03-01"" frequency=""Monthly"" frequency_short=""M"" units=""Index December 1997=100"" units_short=""Index Dec 1997=100"" seasonal_adjustment=""Not Seasonally Adjusted"" seasonal_adjustment_short=""NSA"" last_updated=""2012-04-13 08:55:23-05"" popularity=""18""/>
</seriess>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var series = fred.GetReleaseSeries(10);
            var expectedUrl = "http://api.stlouisfed.org/fred/release/series?api_key=key&release_id=10" + RealtimeNow + "&limit=1000&offset=0&order_by=series_id&sort_order=asc&filter_variable=&filter_value=";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(5, series.Count());
        }

        [Test]
        public void CanGetReleaseSources()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<sources realtime_start=""2012-04-26"" realtime_end=""2012-04-26"">
  <source id=""22"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""U.S. Department of Labor: Bureau of Labor Statistics"" link=""http://www.bls.gov/""/>
</sources>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var sources = fred.GetReleaseSources(10);
            var expectedUrl = "http://api.stlouisfed.org/fred/release/sources?api_key=key&release_id=10" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(1, sources.Count());
        }

        [Test]
        public void CanGetSeries()
        {
            const string response =
                @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<seriess realtime_start=""2012-04-26"" realtime_end=""2012-04-26"">
  <series id=""CPIAUCNS"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Consumer Price Index for All Urban Consumers: All Items"" observation_start=""1913-01-01"" observation_end=""2012-03-01"" frequency=""Monthly"" frequency_short=""M"" units=""Index 1982-84=100"" units_short=""Index 1982-84=100"" seasonal_adjustment=""Not Seasonally Adjusted"" seasonal_adjustment_short=""NSA"" last_updated=""2012-04-13 08:53:00-05"" popularity=""78"" notes=""Handbook of Methods - (http://www.bls.gov/opub/hom/pdf/homch17.pdf) Understanding the CPI: Frequently Asked Questions - (http://stats.bls.gov:80/cpi/cpifaq.htm)""/>
</seriess>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var series = fred.GetSeries("CPIAUCNS");
            var expectedUrl = "http://api.stlouisfed.org/fred/series?api_key=key&series_id=CPIAUCNS" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual("CPIAUCNS", series.Id);
            Assert.AreEqual("Consumer Price Index for All Urban Consumers: All Items", series.Title);
            Assert.AreEqual(new DateTime(2012, 4, 26), series.RealtimeStart);
            Assert.AreEqual(new DateTime(2012, 4, 26), series.RealtimeEnd);
            Assert.AreEqual(new DateTime(1913, 1, 1), series.ObservationStart);
            Assert.AreEqual(new DateTime(2012, 3, 1), series.ObservationEnd);
            Assert.AreEqual(Frequency.Monthy, series.Frequency);
            Assert.AreEqual("Index 1982-84=100", series.Units);
            Assert.AreEqual(false, series.SeasonalAdjusted);
            Assert.AreEqual(new DateTime(2012, 4, 13, 9, 53, 0), series.LastUpdated);
            Assert.AreEqual(78, series.Popularity);
            Assert.AreEqual("Handbook of Methods - (http://www.bls.gov/opub/hom/pdf/homch17.pdf) Understanding the CPI: Frequently Asked Questions - (http://stats.bls.gov:80/cpi/cpifaq.htm)", series.Notes);
        }

        [Test]
        public void CanGetSeriesCategory()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<categories>
  <category id=""95"" name=""Monthly Rates"" parent_id=""15""/>
  <category id=""275"" name=""Japan"" parent_id=""158""/>
</categories>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var categories = fred.GetSeriesCategories("EXJPUS");
            var expectedUrl =
                "http://api.stlouisfed.org/fred/series/categories?api_key=key&series_id=EXJPUS" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(2, categories.Count());
        }

        [Test]
        public void CanGetSeriesRelease()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<releases realtime_start=""2012-04-26"" realtime_end=""2012-04-26"">
  <release id=""15"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.5 Foreign Exchange Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g5/""/>
</releases>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var release = fred.GetSeriesRelease("EXJPUS");
            var expectedUrl = "http://api.stlouisfed.org/fred/series/release?api_key=key&series_id=EXJPUS" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(15, release.Id);
            Assert.AreEqual("G.5 Foreign Exchange Rates", release.Name);
            Assert.AreEqual(true, release.PressRelease);
            Assert.AreEqual("http://www.federalreserve.gov/releases/g5/", release.Link);
            Assert.AreEqual("", release.Notes);
            Assert.AreEqual(new DateTime(2012, 4, 26), release.RealtimeStart);
            Assert.AreEqual(new DateTime(2012, 4, 26), release.RealtimeEnd);
        }

        [Test]
        public void CanSearchSeries()
        {
            const string response =@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<seriess realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" order_by=""search_rank"" sort_order=""asc"" count=""145"" offset=""0"" limit=""1000"">
<series id=""MYAGM4MXM189N"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""M4 for Mexico"" observation_start=""1985-12-01"" observation_end=""2012-01-01"" frequency=""Monthly"" frequency_short=""M"" units=""National Currency"" units_short=""National Currency"" seasonal_adjustment=""Not Seasonally Adjusted"" seasonal_adjustment_short=""NSA"" last_updated=""2012-04-02 16:33:55-05"" popularity=""0"" notes=""M4 comprises M3 plus deposits of residents and nonresidents in branches abroad of domestic other depository corporations.""/>
<series id=""MAM1A1ZAM189N"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""M1, Alternate Definition 1 for South Africa"" observation_start=""1979-03-01"" observation_end=""2012-01-01"" frequency=""Monthly"" frequency_short=""M"" units=""National Currency"" units_short=""National Currency"" seasonal_adjustment=""Not Seasonally Adjusted"" seasonal_adjustment_short=""NSA"" last_updated=""2012-04-02 16:35:45-05"" popularity=""0"" notes=""Notes regarding this series can be found in International Financial Statistics Yearbooks produced by the International Monetary Fund (IMF). We have requested these publications from the IMF. Notes on this series will populate once they become available.""/>
<series id=""MAM1A3CAM189S"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""M1, Alternate Definition 3 for Canada"" observation_start=""1968-01-01"" observation_end=""2011-08-01"" frequency=""Monthly"" frequency_short=""M"" units=""National Currency"" units_short=""National Currency"" seasonal_adjustment=""Seasonally Adjusted"" seasonal_adjustment_short=""SA"" last_updated=""2011-11-17 08:01:27-06"" popularity=""0"" notes=""Notes regarding this series can be found in International Financial Statistics Yearbooks produced by the International Monetary Fund (IMF). We have requested these publications from the IMF. Notes on this series will populate once they become available.""/>
</seriess>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var series = fred.GetSeriesSearch("money stock");
            var expectedUrl = "http://api.stlouisfed.org/fred/series/search?api_key=key" + RealtimeNow + "&limit=1000&offset=0&order_by=search_rank&sort_order=asc&filter_variable=&filter_value=&search_type=full_text&search_text=money%20stock";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(3, series.Count());
        }

        [Test]
        public void CanGetSeriesUpdates()
        {
            const string response =@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<seriess realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" filter_variable=""geography"" filter_value=""all"" order_by=""last_updated"" sort_order=""desc"" count=""45314"" offset=""0"" limit=""100"">
  <series id=""STLFSI"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""St. Louis Financial Stress Index"" observation_start=""1993-12-31"" observation_end=""2012-04-20"" frequency=""Weekly, Ending Friday"" frequency_short=""W"" units=""Index"" units_short=""Index"" seasonal_adjustment=""Not Applicable"" seasonal_adjustment_short=""NA"" last_updated=""2012-04-26 10:00:17-05"" popularity=""92"" notes=""To obtain detailed information regarding the construction of the St. Louis Financial Stress Index, please see the online appendix at
http://research.stlouisfed.org/publications/net/NETJan2010Appendix.pdf
 As of 07/15/2010 the Vanguard Financial Exchange-Traded Fund series has been replaced with the S&amp;P 500 Financials Index. This change was made to facilitate a more timely and automated updating of the FSI.  Switching from the Vanguard series to the S&amp;P series produced no meaningful change in the index.""/>
  <series id=""NFINCP"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Commercial Paper of Nonfinancial Companies"" observation_start=""2001-01-03"" observation_end=""2012-04-25"" frequency=""Weekly, Ending Wednesday"" frequency_short=""W"" units=""Billions of Dollars"" units_short=""Bil. of $"" seasonal_adjustment=""Seasonally Adjusted"" seasonal_adjustment_short=""SA"" last_updated=""2012-04-26 09:31:21-05"" popularity=""35"" notes=""For more information, please refer to http://www.federalreserve.gov/releases/cp/about.htm""/>
  <series id=""FFINCP"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" title=""Foreign Financial Commercial Paper Outstanding"" observation_start=""2001-01-03"" observation_end=""2012-04-25"" frequency=""Weekly, Ending Wednesday"" frequency_short=""W"" units=""Billions of Dollars"" units_short=""Bil. of $"" seasonal_adjustment=""Seasonally Adjusted"" seasonal_adjustment_short=""SA"" last_updated=""2012-04-26 09:31:20-05"" popularity=""27"" notes=""For more information, please refer to http://www.federalreserve.gov/releases/cp/about.htm""/>
</seriess>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var series = fred.GetSeriesUpdates();
            var expectedUrl = "http://api.stlouisfed.org/fred/series/updates?api_key=key" + RealtimeNow + "&limit=100&offset=0&filter_value=all";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(3, series.Count());
        }

        [Test]
        public void CanGetSeriesVintageDates()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<vintage_dates realtime_start=""1776-07-04"" realtime_end=""9999-12-31"" order_by=""vintage_date"" sort_order=""asc"" count=""178"" offset=""0"" limit=""10000"">
  <vintage_date>1997-07-01</vintage_date>
  <vintage_date>1997-08-01</vintage_date>
  <vintage_date>1997-09-02</vintage_date>
  <vintage_date>1997-10-01</vintage_date>
  <vintage_date>1997-11-03</vintage_date>
  <vintage_date>1997-12-01</vintage_date>
  <vintage_date>1998-01-05</vintage_date>
</vintage_dates>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var dates = fred.GetSeriesVintageDates("EXJPUS");
            const string expectedUrl = "http://api.stlouisfed.org/fred/series/vintagedates?api_key=key&series_id=EXJPUS&realtime_start=1776-07-04&realtime_end=9999-12-31&limit=10000&offset=0&sort_order=asc";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(7, dates.Count());
        }

        [Test]
        public void CanGetSources()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<sources realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" order_by=""source_id"" sort_order=""asc"" count=""39"" offset=""0"" limit=""1000"">
  <source id=""1"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Board of Governors of the Federal Reserve System"" link=""http://www.federalreserve.gov/""/>
  <source id=""3"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Federal Reserve Bank of Philadelphia"" link=""http://www.philadelphiafed.org/""/>
  <source id=""4"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Federal Reserve Bank of St. Louis"" link=""http://www.stlouisfed.org/""/>
  <source id=""6"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Federal Financial Institutions Examination Council"" link=""http://www.ffiec.gov/""/>
  <source id=""11"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Dow Jones &amp; Company"" link=""http://www.dowjones.com""/>
  <source id=""13"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Institute for Supply Management"" link=""http://www.ism.ws/""/>
  <source id=""14"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Thomson Reuters/University of Michigan"" link=""https://customers.reuters.com/community/university/default.aspx""/>
</sources>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var sources = fred.GetSources();
            var expectedUrl = "http://api.stlouisfed.org/fred/sources?api_key=key" + RealtimeNow + "&limit=1000&offset=0&order_by=source_id&sort_order=asc";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(7, sources.Count());
        }

        [Test]
        public void CanGetSource()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<sources realtime_start=""2012-04-26"" realtime_end=""2012-04-26"">
  <source id=""1"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""Board of Governors of the Federal Reserve System"" link=""http://www.federalreserve.gov/""/>
</sources>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var source = fred.GetSource(1);
            var expectedUrl = "http://api.stlouisfed.org/fred/source?api_key=key&source_id=1" + RealtimeNow;
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(1, source.Id);
            Assert.AreEqual(new DateTime(2012, 4, 26), source.RealtimeStart);
            Assert.AreEqual(new DateTime(2012, 4, 26), source.RealtimeEnd);
            Assert.AreEqual("Board of Governors of the Federal Reserve System", source.Name);
            Assert.AreEqual("http://www.federalreserve.gov/", source.Link);
            Assert.AreEqual(string.Empty, source.Notes);
        }

        [Test]
        public void CanGetSourceReleases()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<releases realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" order_by=""release_id"" sort_order=""asc"" count=""26"" offset=""0"" limit=""1000"">
<release id=""13"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.17 Industrial Production and Capacity Utilization"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g17/""/>
<release id=""14"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.19 Consumer Credit"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g19/""/>
<release id=""15"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""G.5 Foreign Exchange Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/g5/""/>
<release id=""17"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.10 Foreign Exchange Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h10/""/>
<release id=""18"" realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" name=""H.15 Selected Interest Rates"" press_release=""true"" link=""http://www.federalreserve.gov/releases/h15/""/>
</releases>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var releases = fred.GetSourceReleases(1);
            var expectedUrl = "http://api.stlouisfed.org/fred/source/releases?api_key=key&source_id=1" + RealtimeNow + "&limit=1000&offset=0&order_by=release_id&sort_order=asc";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(5, releases.Count());
        }

        [Test]
        public void CanGetSeriesObservations()
        {
            const string response = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<observations realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" observation_start=""1776-07-04"" observation_end=""9999-12-31"" units=""lin"" output_type=""1"" file_type=""xml"" order_by=""observation_date"" sort_order=""asc"" count=""495"" offset=""0"" limit=""100000"">
  <observation realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" date=""1971-01-01"" value=""358.02""/>
  <observation realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" date=""1971-02-01"" value=""357.5450""/>
  <observation realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" date=""1971-03-01"" value=""357.5187""/>
  <observation realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" date=""1971-04-01"" value=""357.5032""/>
  <observation realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" date=""1971-05-01"" value=""357.4130""/>
  <observation realtime_start=""2012-04-26"" realtime_end=""2012-04-26"" date=""1971-06-01"" value=""357.4118""/>
</observations>";
            var downloader = new MockDownloader(response);
            var fred = new Fred("key", downloader);
            var observations = fred.GetSeriesObservations("EXJPUS");
            var expectedUrl = "http://api.stlouisfed.org/fred/series/observations?api_key=key&series_id=EXJPUS" + RealtimeNow + "&limit=100000&offset=0&sort_order=asc&observation_start=1776-07-04&observation_end=9999-12-31&units=lin&frequency=&aggregation_method=avg&output_type=1&file_type=xml&vintage_dates=";
            Assert.AreEqual(expectedUrl, downloader.Url);
            Assert.AreEqual(6, observations.Count());
        }

 /*       [Test]
        public void CanGetSeriesObservationsFile()
        {
            var fred = new Fred("");
            fred.GetSeriesObservationsFile("EXJPUS", FileType.Xml, "c:\\temp\\exjpus.xml");
            fred.GetSeriesObservationsFile("EXJPUS", FileType.Xls, "c:\\temp\\exjpus.xls");
            fred.GetSeriesObservationsFile("EXJPUS", FileType.Text, "c:\\temp\\exjpus.txt");
        }*/

        internal class MockDownloader : IUrlDownloader
        {
            private readonly string _response;

            public MockDownloader(string resposnse)
            {
                _response = resposnse;
            }

            public string Url { get; private set; }

            #region IUrlDownloader Members

            public string Download(string url)
            {
                Url = url;
                return _response;
            }

            public void DownloadFile(string url, string filename)
            {
                throw new NotImplementedException();
            }

            #endregion
        }
    }
}