using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public interface IDataService
    {
        void GetPlaylistData(Action<BBCTop40PlaylistData, Exception> callback);
        void GetThumnails(IPlaylistData playlistData, Action<IList<Tuple<string,string>>, Exception> callback);
        void GetYoutubeVideoUri(string videoId, Action<YouTubeUri, Exception> callback);
    }
}
