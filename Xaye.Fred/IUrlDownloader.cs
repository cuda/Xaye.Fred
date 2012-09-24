using System.Net;

namespace Xaye.Fred
{
    // needed so we could mock web requests. 
    public partial interface IUrlDownloader
    {
        string Download(string url);
        void DownloadFile(string url, string filename);
    }

    // needed so we could mock web requests. 
    internal partial class WebClientDownloader : IUrlDownloader
    {
        #region IUrlDownloader Members

        public string Download(string url)
        {
            return new WebClient().DownloadString(url);
        }

        public void DownloadFile(string url, string filename)
        {
            new WebClient().DownloadFile(url, filename);
        }

        #endregion
    }
}