using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public class BBCTop40PlaylistDataItem
    {
        public int Position { get; set; }
        public string Status { get; set; }
        public int Previous { get; set; }
        public int Weeks { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string VideoId { get; set; }
        public string ThumnailUrl { get; set; }
    }
}
