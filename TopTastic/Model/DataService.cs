using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EchoNest;
using Windows.Storage;
using Windows.Data.Xml.Dom;
using EchoNest.Artist;
using System.Net.Http;
using System.IO;
using System.Diagnostics;
using Windows.Media.Transcoding;
using Windows.Media.MediaProperties;

namespace TopTastic.Model
{
    public class DataService : IDataService
    {
        private EchoNestSession echoNestSession;
        private HttpClient client;
        private MediaTranscoder transcoder;
        private MediaEncodingProfile encodingProfile;


        public async void SharePlaylistOnYouTube(IPlaylistData playlistData, Action<string, Exception> callback)
        {
            string playlistId = null;
            Exception ex = null;

            try
            {
                var service = await YouTubeHelper.CreateAuthenticatedService("user","Top40");
                playlistId = await YouTubeHelper.CreatePlaylistFromData(service, playlistData);
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(playlistId, ex);
        }

        // MJDTODO - refactor
        public async void DownloadMedia(Uri videoUri, string artist,  string title, bool extractAudio, Action<string, Exception> callback)
        {
            string status = null;
            Exception ex = null;

            try
            {
                if (this.client == null)
                {
                    this.client = new HttpClient();
                    this.client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
                }

                var response = await this.client.GetAsync(videoUri);

                if (response.IsSuccessStatusCode)
                {
                    var mediaType = response.Content.Headers.ContentType.MediaType;
                    Debug.Assert(mediaType == "video/mp4");

                    var videoStream = await this.client.GetStreamAsync(videoUri);

                    var videoFile = await this.CreateStorageFile(artist, title, KnownLibraryId.Videos);
                    using (var videoOutputStream = await videoFile.OpenStreamForWriteAsync())
                    {
                        await videoStream.CopyToAsync(videoOutputStream);
                        status = "Media Downloaded";
                    }

                    if (extractAudio)
                    {
                        var audioFile = await this.CreateStorageFile(artist, title, KnownLibraryId.Music);
                        this.ExtractAudio(artist, title, videoFile, audioFile);
                        status = "Audio Extracted";
                    }
                }
                else
                {
                    status = "Media operation failed";
                }
                
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(status, ex);
        }

        public async void ExtractAudio(string artist, string title, StorageFile videoFile, StorageFile audioFile)
        {
            if (this.transcoder == null)
            {
                this.transcoder = new MediaTranscoder();
                this.encodingProfile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
            }

            var preparedTranscodeResult = await this.transcoder.PrepareFileTranscodeAsync(videoFile, audioFile, this.encodingProfile);
            await preparedTranscodeResult.TranscodeAsync();
            await TagStorageFile(artist, title, audioFile);
        }

        public async Task<StorageFile> CreateStorageFile(string artist, string title, KnownLibraryId libraryId)
        {
            string fileExtension = string.Empty;
            if (libraryId == KnownLibraryId.Music)
            {
                fileExtension = ".mp3";
            }
            else if (libraryId == KnownLibraryId.Videos)
            {
                fileExtension = ".mp4";
            }
            var fileName = this.CreateFileName(artist, title, fileExtension);
            var library = await Windows.Storage.StorageLibrary.GetLibraryAsync(libraryId);
            var saveFolder = library.SaveFolder;
            var storageFile = await saveFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            return storageFile;
        }

        public string CreateFileName(string artist, string title, string extension)
        {
            string result = artist + " - " + title + extension;
            foreach(var c in Path.GetInvalidFileNameChars())
            {
                result = result.Replace(c.ToString(), string.Empty);
            }
            return result;
        }

        public async Task TagStorageFile(string artist, string title, StorageFile file)
        {
            var id3 = new ID3Library.ID3();
            await id3.GetMusicPropertiesAsync(file);
            id3.Artist = artist;
            id3.Title = title;
            await id3.SaveMusicPropertiesAsync();
        }

        public string FormatArtistQuery(string artist)
        {
            var artistQuery = artist.Replace("&", ",");
            return artistQuery;
        }

        public async void GetArtistInfo(string artist, Action<string, Exception> callback)
        {
            Exception ex = null;
            string artistInfo = null;

            try
            {
                if (this.echoNestSession == null)
                {
                    var echoNestApiKey = await this.LoadApiKey();
                    this.echoNestSession = new EchoNestSession(echoNestApiKey);
                }

                var artistQuery = FormatArtistQuery(artist);
                var result = this.echoNestSession.Query<Biography>().Execute(artistQuery);

                StringBuilder sb = new StringBuilder();
                foreach(var bio in result.Biographies)
                {
                    sb.Append(bio.Text);
                }

                artistInfo = sb.ToString();
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(artistInfo, ex);
        }


        public async void GetBBCPlaylistData(Action<PlaylistData, Exception> callback)
        {
            await GetPlaylistData(PlaylistSourceFactory<BBCTop40PlaylistSource>.GetNewPlaylistSource(), callback);
        }

        public async void GetEchoNestPlaylistData(string searchString, Action<PlaylistData, Exception> callback)
        {
            await GetPlaylistData(PlaylistSourceFactory<EchoNestPlaylistSource>.GetNewPlaylistSource(), callback);
        }

        public async Task GetPlaylistData(IPlaylistSource playlistSource, Action<PlaylistData, Exception> callback)
        {
            PlaylistData playlistData = null;
            Exception err = null;

            try
            {
                playlistData = await playlistSource.GetPlaylistAsync();
            }
            catch (Exception ex)
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

        public async void GetYoutubeVideoUri(string videoId,  Action<Uri, Exception> callback)
        {
            Exception ex = null;
            Uri videoUri = null;

            try
            {
                videoUri = await YoutubeExtractor.DownloadUrlResolver.GetVideoUriAsync(videoId);
                //var youTubeUri = await YouTube.GetVideoUriAsync(videoId, YouTubeQuality.Quality720P);
                //videoUri = youTubeUri.Uri;
            }
            catch(Exception e)
            {
                ex = e;
            }

            callback(videoUri, ex);
        }

        public async Task<string> LoadApiKey()
        {
            var secrets = new Uri("ms-appx:///Assets/echonest_secrets.xml");
            StorageFile sFile = await StorageFile.GetFileFromApplicationUriAsync(secrets);
            XmlDocument doc = await XmlDocument.LoadFromFileAsync(sFile);
            var nodes = doc.GetElementsByTagName("EchoNestApiKey");
            return nodes[0].InnerText;
        }

        public async void SearchYouTube(string searchString, Action<string, Exception> callback)
        {
            Exception ex = null;
            string videoId = null;

            try
            {
                YouTubeService service = YouTubeHelper.CreateService("Top40");
                videoId = await YouTubeHelper.FindVideoId(service, searchString);
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(videoId, ex);
        }

    }
}
