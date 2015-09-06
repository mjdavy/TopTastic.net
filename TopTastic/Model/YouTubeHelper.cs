// -----------------------------------------------------------------------
// <copyright file="YouTubeHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace TopTastic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.YouTube.v3;
    using System.Threading;
    using Google.Apis.Services;
    using Google.Apis.YouTube.v3.Data;
    using Windows.Storage;
    using System.IO;
    using Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class YouTubeHelper
    {
        private YouTubeHelper()
        {
        }

        public static async Task<YouTubeService> Authenticate(string user)
        {
            // Authorization
            UserCredential credential;

            Uri clientSecretsUri= new Uri(
                Path.Combine(ApplicationData.Current.LocalFolder.Path, "client_secrets.json"));
           

            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecretsUri,
                new[] { YouTubeService.Scope.Youtube }, user, CancellationToken.None);

         
            // Create the service.
            var service = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Top40",
            });

            return service;

        }

        public static async Task<string> CreatePlaylist(YouTubeService service, string title, string description)
        {
            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = title;
            newPlaylist.Snippet.Description = description;
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";
            var playlistInsertReq = service.Playlists.Insert(newPlaylist, "snippet,status");
            newPlaylist = await playlistInsertReq.ExecuteAsync();
            return newPlaylist.Id;
        }

        public static async Task<PlaylistItem> AddVideoToPlaylist(YouTubeService service,  string playlistId, string videoId)
        {
            // Add a video to the newly created playlist.
           
            var newPlaylistItem = new PlaylistItem();
            newPlaylistItem.Snippet = new PlaylistItemSnippet();
            newPlaylistItem.Snippet.PlaylistId = playlistId;
            newPlaylistItem.Snippet.ResourceId = new ResourceId();
            newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
            newPlaylistItem.Snippet.ResourceId.VideoId = videoId;
            var playlistItemInsertReq = service.PlaylistItems.Insert(newPlaylistItem, "snippet");
            var item = await playlistItemInsertReq.ExecuteAsync();
            return item;
        }


        public static async Task<string> FindPlaylistByName(YouTubeService service, string title)
        {
            var req = service.Playlists.List("snippet");
            req.Mine = true;
            var resp = await req.ExecuteAsync();

            var m = from x in resp.Items where x.Snippet.Title == title select x.Id;

            if (m == null || m.Count() == 0)
            {
                return string.Empty;
            }

            return m.First();
        }

        public static async void RemoveAllMembersFromPlaylist(YouTubeService service, string playlistId)
        {
            var req = service.PlaylistItems.List("snippet");
            req.PlaylistId = playlistId;
            req.MaxResults = 40;
           
            var resp = await req.ExecuteAsync();

            if (resp == null)
            {
                return;
            }

            foreach (var item in resp.Items)
            {
                var del = service.PlaylistItems.Delete(item.Id);
                await del.ExecuteAsync();
            }

        }

        public static async void UpdatePlaylistInfo(YouTubeService service,  string playlistId, string title, string description = null)
        {
            var list = service.Playlists.List("snippet");
            list.Id = playlistId;
            var listReponse = await list.ExecuteAsync();

            if (listReponse.Items.Count == 0)
            {
                return;
            }

            var pl = listReponse.Items.First();
            pl.Snippet.Title = title;
            pl.Snippet.PublishedAt = DateTime.Now;
            
            if (description != null)
            {
                pl.Snippet.Description = description;
            }

            // Update thumbnail to use the first item in the playlist
            var items = await GetPlaylistItems(service, playlistId, 1);
            var firstItem = items.First();
            pl.Snippet.Thumbnails.Default__ = firstItem.Snippet.Thumbnails.Default__;

            var update = service.Playlists.Update(pl, "snippet");
            await update.ExecuteAsync();
        }

        public static async Task<IList<PlaylistItem>> GetPlaylistItems(YouTubeService service, string playlistId, int maxResults)
        {
            // Get the first item, and update the playlist thumbnail
            var itemsRequest = service.PlaylistItems.List("snippet");
            itemsRequest.PlaylistId = playlistId;
            itemsRequest.MaxResults = maxResults;

            var itemsResponse = await itemsRequest.ExecuteAsync();
            return itemsResponse.Items;
        }

        public static async Task<string> FindVideoId(YouTubeService service, string query)
        {
            var results = await SearchVideos(service, query);

            if (results.Count == 0)
            {
                return string.Empty;
            }

            return results.First().Id.VideoId;
        }

        public static async Task<string> GetVideoTitleFromId(YouTubeService service, string videoId)
        {
            var results = await SearchVideos(service, videoId);

            if (results.Count == 0)
            {
                return string.Empty;
            }

            return results.First().Snippet.Title;
        }

        public static async Task<IList<SearchResult>> SearchVideos(YouTubeService service, string query)
        {
            var req = service.Search.List("snippet");
            req.Q = query;
            req.Type = "youtube#video";
            req.Order = SearchResource.ListRequest.OrderEnum.Relevance;
            var resp = await req.ExecuteAsync();
            return resp.Items;
        }

        public static string DeletePlaylist(YouTubeService service, string playlistId)
        {
            var deleteRequest = service.Playlists.Delete(playlistId);
            return deleteRequest.Execute();
        }

        public static async Task<string> CreatePlaylistFromData(YouTubeService service, IPlaylistData playlistData)
        {
            var playlistId = await YouTubeHelper.CreatePlaylist(service, playlistData.Title, playlistData.Description);

            foreach(var searchKey in playlistData.SearchKeys)
            {
                var videoId = await YouTubeHelper.FindVideoId(service, searchKey);

                if (string.IsNullOrEmpty(videoId))
                {
                    continue;
                }

                await YouTubeHelper.AddVideoToPlaylist(service, playlistId, videoId);
            }

            return playlistId;
        }
    }
}
