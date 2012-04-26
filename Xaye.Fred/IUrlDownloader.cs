using System.Net;

namespace Xaye.Fred
{
    // needed so we could mock web requests. 
    public interface IUrlDownloader
    {
        string Download(string url);
        void DownloadFile(string url, string filename);
    }

    // needed so we could mock web requests. 
    internal class WebClientDownloader : IUrlDownloader
    {
        private readonly WebClient _webClient = new WebClient();

        #region IUrlDownloader Members

        public string Download(string url)
        {
            return _webClient.DownloadString(url);
        }

        public void DownloadFile(string url, string filename)
        {
            _webClient.DownloadFile(url, filename);
        }

        #endregion
    }
}