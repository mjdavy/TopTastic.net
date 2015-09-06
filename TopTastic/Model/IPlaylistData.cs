using System.Collections.Generic;

namespace TopTastic.Model
{
    public interface IPlaylistData
    {
        string Title { get; set; }
        string Description { get; set; }
        IList<string> SearchKeys {get;}
    }
}
