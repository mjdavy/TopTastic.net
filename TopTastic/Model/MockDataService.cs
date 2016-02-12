using System;
using System.Collections.Generic;
using Windows.Storage;
using EchoNest;
using Google.Apis.YouTube.v3;

namespace TopTastic.Model
{
    public class MockDataService : IDataService
    {
        public void SharePlaylistOnYouTube(IPlaylistData playlistData, Action<string, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void DownloadMedia(Uri videoUri, string artist, string title, bool extractAudio, Action<string, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetArtistInfo(string artistQuery, Action<string, Exception> callback)
        {

            Exception ex = null;
            string artistInfo = null;

            try
            {
                var mockEchoNest = new Mock();
                artistInfo =  mockEchoNest.GetArtistInfo();
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(artistInfo, ex);
        }

        public async void GetBBCPlaylistData(Action<PlaylistData, Exception> callback)
        {
            var testFileUri = new Uri("ms-appx:///Assets/TestChart.html");
            PlaylistData playlistData = null;
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

        public async void GetYoutubeVideoUri(string videoId, Action<Uri, Exception> callback)
        {
            Exception ex = null;
            Uri videoUri = null;

            try
            {
                videoUri = await YoutubeExtractor.DownloadUrlResolver.GetVideoUriAsync(videoId);
                //var youTubeUri = await YouTube.GetVideoUriAsync(videoId, YouTubeQuality.Quality720P);
                //videoUri = youTubeUri.Uri;
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(videoUri, ex);
        }

        public void SearchYouTube(string searchString, Action<string, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetEchoNestPlaylistData(string searchString, Action<PlaylistData, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}
