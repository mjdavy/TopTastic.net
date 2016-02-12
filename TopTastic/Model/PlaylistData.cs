using System.Collections.Generic;

namespace TopTastic.Model
{
    public class PlaylistData : IPlaylistData
    {
        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public IList<PlaylistDataItem> Items
        {
            get;
            set;
        }

        public IList<string> SearchKeys
        {
            get;
            set;
        }
    }
}
