using EchoNest;
using EchoNest.Playlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Data.Xml.Dom;

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
            var seedArtists = new TermList();

            seedArtists.Add(Query);

            StaticArgument staticArgument = new StaticArgument
            {
                Results = 40,
                Artist = seedArtists,
                Type = "artist-radio"
            };

            using (var session = new EchoNestSession(apiKey))
            {
                PlaylistResponse searchResponse = session.Query<Static>().Execute(staticArgument);
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
