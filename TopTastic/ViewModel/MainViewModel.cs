using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;

namespace TopTastic.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _helloWorld;
        private IList<BBCTop40PlaylistDataItem> _playlistItems;
        

        public string HelloWorld
        {
            get
            {
                return _helloWorld;
            }
            set
            {
                Set(() => HelloWorld, ref _helloWorld, value);
            }
        }

        public IList<BBCTop40PlaylistDataItem> PlaylistItems
        {
            get
            {
                return _playlistItems;
            }
            set
            {
                Set(() => PlaylistItems, ref _playlistItems, value);
            }
        }
        

        public MainViewModel()
        {
            HelloWorld = IsInDesignMode
                ? "Runs in design mode"
                : "Runs in runtime mode";

            var service = new DataService(); // MJDTODO - use DI

            service.GetPlaylistData((playlistData, err) =>
            {
                if (err != null)
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
                else
                {
                    this.PlaylistItems = playlistData.Items;
                }
            });
        }
    }
}
