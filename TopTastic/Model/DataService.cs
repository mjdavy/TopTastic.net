using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.Multimedia;
using EchoNest;

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

        public async void GetVideoInfo(IPlaylistData playlistData, Action<IList<VideoInfo>, Exception> callback)
        { 
            var videoList = new List<VideoInfo>();
            Exception ex = null;

            try
            {
                int index = 0;
                YouTubeService service = YouTubeHelper.CreateService("Top40");
                foreach (var searchKey in playlistData.SearchKeys)
                {
                    var results = await YouTubeHelper.SearchVideos(service, searchKey);

                    if (results.Count == 0)
                    {
                        var video = new VideoInfo(index, "ms-appx:///Assets/p030kf95.jpg", "DK_0jXPuIr0");
                        videoList.Add(video);
                    }
                    else
                    {
                        var firstResult = results.First();
                        var details = YouTubeHelper.GetThumnailDetails(firstResult);
                        var video = new VideoInfo(index, details.Default__.Url, firstResult.Id.VideoId);
                        videoList.Add(video);
                    }
                    index++;
                    callback(videoList, ex);
                }
            }
            catch (Exception e)
            {
                ex = e;
                callback(new List<VideoInfo>(), ex);
            }
            
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
