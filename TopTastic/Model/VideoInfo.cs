using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public class VideoInfo
    {
        private VideoInfo() { }
        public VideoInfo(int index, string thumbnailUrl, string videoId)
        {
            this.Index = index;
            this.ThumbnailUrl = thumbnailUrl;
            this.VideoId = videoId;
        }
        public int Index { get;}
        public string ThumbnailUrl { get;}
        public string VideoId { get; }
    }
}
