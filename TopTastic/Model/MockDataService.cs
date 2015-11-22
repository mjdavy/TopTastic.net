using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.Multimedia;
using Windows.Storage;
using EchoNest;

namespace TopTastic.Model
{
    public class MockDataService : IDataService
    {
        public void CreatePlaylist(IPlaylistData playlistData, Action<string, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetArtistInfo(string artistQuery, Action<string, Exception> callback)
        {

            Exception ex = null;
            string artistInfo = null;

            try
            {
                var c1 = new Mock();
                artistInfo =  c1.Test();
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(artistInfo, ex);
        }

        public async void GetPlaylistData(Action<BBCTop40PlaylistData, Exception> callback)
        {
            var testFileUri = new Uri("ms-appx:///Assets/TestChart.html");
            BBCTop40PlaylistData playlistData = null;
            Exception err = null;
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(testFileUri);
                var html = await FileIO.ReadTextAsync(file);
                playlistData = BBCTop40PlaylistSource.ExtractPlaylistData(html);
            }
            catch(Exception ex)
            {
                err = ex;
            }
            callback(playlistData, err);
        }

        public void GetVideoInfo(IPlaylistData playlistData, Action<IList<VideoInfo>, Exception> callback)
        {
 
            var videoList = new List<VideoInfo>();
            Exception ex = null;

            try
            {
                int index = 0;
                foreach (var searchKey in playlistData.SearchKeys)
                {
                    videoList.Add(new VideoInfo(index++, "ms-appx:///Assets/p030kf95.jpg", "DK_0jXPuIr0"));
                }
            }
            catch (Exception e)
            {
                ex = e;
            }
            callback(videoList, ex);
        }

        public void GetYoutubeVideoUri(string videoId, Action<YouTubeUri, Exception> callback)
        {
            
        }
    }
}
