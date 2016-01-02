using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TopTastic.Model;

namespace TopTastic.tests
{
    [TestClass]
    public class DataServiceTests
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void TestCreateFileName()
        {
            string artist = "Justin Bieber";
            string title = "What do you mean?";
            string extension = ".mp3";

            var service = new DataService();
            var actual = service.CreateFileName(artist, title, extension);

            var expected = "Justin Bieber - What do you mean.mp3";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestPlaylistData()
        {
            var result = GetMockPlaylistData();
            var expected = result.Items.Count;
            Assert.AreEqual(40, expected);
        }

        [TestMethod]
        public async Task TestVevoVideo()
        {
            // Bieber - love yourself
            var videoId = "oyEuk8j8imI";
            var result = await TryVideo(videoId);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task TestNonVevoVideo()
        {
            // NHS choir - Bridge over you
            var videoId = "ve1sevQpQLQ";
            var result = await TryVideo(videoId);
            Assert.IsTrue(result);
        }

        public async Task<bool> TryVideo(string videoId)
        {
            bool result = false;
            try
            {
                var videoUri = GetMockVideoUri(videoId);
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                var response = await client.GetAsync(videoUri);
                if (response.IsSuccessStatusCode)
                {
                    result =  true;
                }
            }

            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                result = false;
            }

            return result;
        }

        public Uri GetMockVideoUri(string videoId)
        {
            var ds = new MockDataService();
            Uri result = null;
            var completion = new ManualResetEvent(false);

            ds.GetYoutubeVideoUri(videoId, (youTubeUri, err) =>
            {
                result = youTubeUri;
                completion.Set();
            });

            completion.WaitOne();
            return result;

        }

        public BBCTop40PlaylistData GetMockPlaylistData()
        {
            var ds = new MockDataService();
            BBCTop40PlaylistData result = null;
            var completion = new ManualResetEvent(false);

            ds.GetPlaylistData((playlistData, err) =>
            {
                result = playlistData;
                completion.Set();
            });

            completion.WaitOne();
            return result;
        }

        public IList<VideoInfo> GetMockVideoInfo(BBCTop40PlaylistData playlistData)
        {
            var ds = new MockDataService();
            var completion = new ManualResetEvent(false);
            IList<VideoInfo> result = null;

            ds.GetVideoInfo(playlistData, (videoInfo, err) =>
            {
                result = videoInfo;
                completion.Set();
            });

            completion.WaitOne();
            return result;
        }


    }
}
