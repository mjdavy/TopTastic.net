using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public class DataService : IDataService
    {
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
    }
}
