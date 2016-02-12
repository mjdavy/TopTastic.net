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
        public Task<PlaylistData> GetPlaylistAsync()
        {
            throw new NotImplementedException();
        }
 
    }
}
