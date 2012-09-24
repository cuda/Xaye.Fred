using System.Net;
using System.Threading.Tasks;

namespace Xaye.Fred
{
    // needed so we could mock web requests. 
    public partial interface IUrlDownloader
    {
        Task<string> DownloadAsync(string url);
        Task DownloadFileAsync(string url, string filename);
    }

    // needed so we could mock web requests. 
    internal partial class WebClientDownloader
    {
        #region IUrlDownloader Members

        public async Task<string> DownloadAsync(string url)
        {
            return await new WebClient().DownloadStringTaskAsync(url);
        }

        public async Task DownloadFileAsync(string url, string filename)
        {
            await new WebClient().DownloadFileTaskAsync(url, filename);
        }

        #endregion
    }
}