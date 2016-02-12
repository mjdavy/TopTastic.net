using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;

namespace TopTastic.ViewModel
{
    public class PlaylistItemViewModel : ViewModelBase
    {
        PlaylistDataItem item;
        string imageUrl = null;
        string videoId = null;
        private Uri videoUri;

        public enum PositionStatus
        {
            Up,
            Down,
            NoChange
        }

        public PlaylistItemViewModel(PlaylistDataItem item)
        {
            if (item ==null)
            {
                throw new NullReferenceException("null item passed to contructor");
            }
            this.item = item;
        }

        public string Weeks
        {
            get
            {
                switch (item.Weeks)
                {
                    case 0:
                        return string.Empty;
                    case 1:
                        return string.Format("1 WEEK IN THE CHART", item.Weeks.ToString());
                    default:
                        return string.Format("{0} WEEKS IN THE CHART", item.Weeks.ToString());

                }
            }
        }

        public string ArtistAndTitle
        {
            get
            {
                return string.Format("{0} - {1}", item.Title, item.Artist);
            }
        }

        public string Position { get { return item.Position.ToString(); }}
        public string Status { get { return item.Status.ToUpper(); } }
        public string Previous { get { return item.Previous.ToString(); } }
       
        public string Artist { get { return item.Artist; } }
        public string Title { get { return item.Title; } }
        public string Thumbnail
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                Set(() => Thumbnail, ref this.imageUrl, value);
            }
        }

        public string VideoId
        {
            get
            {
                return this.videoId;
            }
            set
            {
                Set(() => VideoId, ref this.videoId, value);
            }
        }

        public Uri VideoUri
        {
            get
            {
                return this.videoUri;
            }
            set
            {
                Set(() => VideoUri, ref this.videoUri, value);
            }
        }

        public string StatusIndicatorImageUrl
        {
            get
            {
                string result;

                if (item.Previous == 0 || item.Previous > 40)
                {
                    result = "ms-appx:///Assets/Star.png";
                }
                else if (item.Previous < item.Position)
                {
                    result = "ms-appx:///Assets/DownArrow.png";
                }
                else if (item.Previous > item.Position)
                {
                    result = "ms-appx:///Assets/UpArrow.png";
                }
                else
                {
                    result = "ms-appx:///Assets/Rectangle.png";
                }

                return result;
            }
        }

    }
}
