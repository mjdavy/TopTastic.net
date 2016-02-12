using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public class PlaylistSourceFactory<T> where T : new()
    {
        public static T GetNewPlaylistSource()
        {
            return new T();
        }
    }
}
