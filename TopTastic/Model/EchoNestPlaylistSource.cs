using EchoNest;
using EchoNest.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Data.Xml.Dom;
using EchoNest.Song;

namespace TopTastic.Model
{
    public class EchoNestPlaylistSource : IPlaylistSource
    {
        public string Query
        {
            get;
            set;
        }

        public async Task<PlaylistData> GetPlaylistAsync()
        {
            var apiKey = await LoadApiKey();
            return CreateEchonestPlaylist(apiKey);
        }

        public async Task<string> LoadApiKey()
        {
            var secrets = new Uri("ms-appx:///Assets/echonest_secrets.xml");
            StorageFile sFile = await StorageFile.GetFileFromApplicationUriAsync(secrets);
            XmlDocument doc = await XmlDocument.LoadFromFileAsync(sFile);
            var nodes = doc.GetElementsByTagName("EchoNestApiKey");
            return nodes[0].InnerText;
        }
        private PlaylistData CreateEchonestPlaylist(string apiKey)
        {
            var query = Query.Replace('&', '+');
            using (var session = new EchoNestSession(apiKey))
            {
                var searchReponse = GetArtistPlaylist(session, query);

                // Do we have a match for the artist? If not look for a matching song
                if (searchReponse.Songs != null && searchReponse.Songs.Count > 0)
                {
                    return CreatePlaylistDataFromEchoNestResponse(searchReponse);
                }
                else
                {
                    searchReponse = GetSongPlaylist(session, query);
                    return CreatePlaylistDataFromEchoNestResponse(searchReponse);
                }
            }
        }
       
        private PlaylistResponse GetSongPlaylist(EchoNestSession session, string query)
        {
            var id = GetSongId(session, query);
            var songIds = new TermList();
            songIds.Add(id);

            StaticArgument staticArgument = new StaticArgument
            {
                Type = "song-radio",
                SongID = songIds,
                Results = 40,
                Variety = 1
            };

            return session.Query<Static>().Execute(staticArgument);
        }

        private PlaylistData CreatePlaylistDataFromEchoNestResponse(PlaylistResponse searchResponse)
        {
            var playlistData = new PlaylistData();
            playlistData.Items = new List<PlaylistDataItem>();
            playlistData.Description = Query;
            playlistData.Title = string.Format("{0} playlist", Query);
            playlistData.SearchKeys = new List<string>();

            foreach (var song in searchResponse.Songs)
            {
                var item = new PlaylistDataItem();
                item.Artist = song.ArtistName;
                item.Title = song.Title;
                playlistData.Items.Add(item);

                var searchKey = string.Format("{0} {1}", item.Artist, item.Title);
                playlistData.SearchKeys.Add(searchKey);
            }

            return playlistData;
        }

        private string GetSongId(EchoNestSession session, string title)
        {
            SearchArgument searchArgument = new SearchArgument
            {
                Title = title
            };

            SearchResponse searchResponse = session.Query<Search>().Execute(searchArgument);
            return searchResponse.Songs.Count > 0 ? searchResponse.Songs.First().ID : string.Empty;
        }

        private PlaylistResponse GetArtistPlaylist(EchoNestSession session, string query)
        {
            
            var seedArtists = new TermList();

            foreach (var term in query.Split(','))
            {
                seedArtists.Add(term);
            }

            StaticArgument staticArgument = new StaticArgument
            {
                Results = 40,
                Artist = seedArtists,
                Type = "artist-radio"
            };

            return session.Query<Static>().Execute(staticArgument);
        }

        private PlaylistData CreateTestlist()
        {
            var playlistData = new PlaylistData();
            playlistData.Items = new List<PlaylistDataItem>();
            playlistData.Description = "Test playlist";
            playlistData.Title = string.Format("{0} playlist", Query);
            playlistData.SearchKeys = new List<string>();

            var item1 = new PlaylistDataItem()
            {
                Artist = "Above & Beyond",
                Title = "Lonely Girl"
            };

            var item2 = new PlaylistDataItem()
            {
                Artist = "Schiller",
                Title = "I Feel You"
            };

            var item3 = new PlaylistDataItem()
            {
                Artist = "Radiohead",
                Title = "Creep"
            };

            var item4 = new PlaylistDataItem()
            {
                Artist = "Avril Lavigne",
                Title = "Nobody's Home"
            };

            playlistData.Items.Add(item1);
            playlistData.Items.Add(item2);
            playlistData.Items.Add(item3);
            playlistData.Items.Add(item4);

            foreach (var item in playlistData.Items)
            {
                var searchKey = string.Format("{0} {1}", item.Artist, item.Title);
                playlistData.SearchKeys.Add(searchKey);
            }

            return playlistData;
        }

    }
}
