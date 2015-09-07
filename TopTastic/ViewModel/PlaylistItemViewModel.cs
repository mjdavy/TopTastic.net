using GalaSoft.MvvmLight;
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
        BBCTop40PlaylistDataItem item;
        string imageUrl = null; 

        public PlaylistItemViewModel(BBCTop40PlaylistDataItem item)
        {
            if (item ==null)
            {
                throw new NullReferenceException("null item passed to contructor");
            }
            this.item = item;
        }

        public string Position { get { return item.Position; }}
        public string Status { get { return item.Status; } }
        public string Previous { get { return item.Previous; } }
        public string Weeks { get { return item.Weeks; } }
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

    }
}
