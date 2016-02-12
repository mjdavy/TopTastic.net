using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return CreateTestlist();
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
