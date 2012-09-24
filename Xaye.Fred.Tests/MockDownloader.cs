using System;
using System.Threading.Tasks;


namespace Xaye.Fred.Tests
{
    internal class MockDownloader : IUrlDownloader
    {
        private readonly string _response;

        public MockDownloader(string resposnse)
        {
            _response = resposnse;
        }

        public string Url { get; private set; }


        public string Download(string url)
        {
            Url = url;
            return _response;
        }

        public void DownloadFile(string url, string filename)
        {
            throw new NotImplementedException();
        }

        public async Task<string> DownloadAsync(string url)
        {
            Url = url;
            return await Task<string>.Factory.StartNew(() => _response);
        }

        public Task DownloadFileAsync(string url, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
