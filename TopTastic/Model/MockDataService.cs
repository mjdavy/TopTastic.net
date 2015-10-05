using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.Multimedia;
using Windows.Storage;

namespace TopTastic.Model
{
    public class MockDataService : IDataService
    {
        public void CreatePlaylist(IPlaylistData playlistData, Action<string, Exception> callback)
        {
            throw new NotImplementedException();
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

        public void GetThumnails(IPlaylistData playlistData, Action<IList<Tuple<string, string>>, Exception> callback)
        {
            var defaultThumbnail = new Tuple<string, string>("DK_0jXPuIr0", "ms-appx:///Assets/p030kf95.jpg");
            var thumbnails = new List<Tuple<string, string>>();
            Exception ex = null;

            try
            {
                foreach (var searchKey in playlistData.SearchKeys)
                {
                    thumbnails.Add(defaultThumbnail);
                }
            }
            catch (Exception e)
            {
                ex = e;
            }
            callback(thumbnails, ex);
        }

        public void GetYoutubeVideoUri(string videoId, Action<YouTubeUri, Exception> callback)
        {
            
        }
    }
}
