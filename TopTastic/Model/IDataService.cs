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
    }
}
