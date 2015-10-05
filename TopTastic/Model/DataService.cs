using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.Multimedia;

namespace TopTastic.Model
{
    public class DataService : IDataService
    {
        public async void CreatePlaylist(IPlaylistData playlistData, Action<string, Exception> callback)
        {
            string playlistId = null;
            Exception ex = null;

            try
            {
                var service = await YouTubeHelper.CreateAuthenticatedService("martin.davy@gmail.com","Top40");
                playlistId = await YouTubeHelper.CreatePlaylistFromData(service, playlistData);
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(playlistId, ex);
        }

        public async void GetPlaylistData(Action<BBCTop40PlaylistData, Exception> callback)
        {
            var playlistSource = new BBCTop40PlaylistSource();
            BBCTop40PlaylistData playlistData = null;
            Exception err = null;

            try
            {
                playlistData = await playlistSource.GetPlaylistAsync();
            }
            catch(Exception ex)
            {
                err = ex;
            }

            callback(playlistData, err);
        }

        public async void GetThumnails(IPlaylistData playlistData, Action<IList<Tuple<string, string>>, Exception> callback)
        {
            var defaultThumbnail = new Tuple<string, string>("DK_0jXPuIr0", "ms-appx:///Assets/p030kf95.jpg");
            var thumbnails = new List<Tuple<string,string>>();
            Exception ex = null;

            try
            {
                YouTubeService service = YouTubeHelper.CreateService("Top40");
                foreach (var searchKey in playlistData.SearchKeys)
                {
                    var results = await YouTubeHelper.SearchVideos(service, searchKey);

                    if (results.Count == 0)
                    {
                        thumbnails.Add(defaultThumbnail);
                    }
                    else
                    {
                        var firstResult = results.First();
                        var details = YouTubeHelper.GetThumnailDetails(firstResult);
                        var thumbnail = new Tuple<string, string>(firstResult.Id.VideoId, details.Default__.Url);
                        thumbnails.Add(thumbnail);
                    }
                    
                }
            }
            catch(Exception e)
            {
                ex = e;
            }
            callback(thumbnails, ex);
        }

        public async void GetYoutubeVideoUri(string videoId,  Action<YouTubeUri, Exception> callback)
        {
            Exception ex = null;
            YouTubeUri youTubeUri = null;
            try
            {
                youTubeUri = await YouTube.GetVideoUriAsync(videoId, YouTubeQuality.Quality720P);
            }
            catch(Exception e)
            {
                ex = e;
            }

            callback(youTubeUri, ex);
        }
    }
}
