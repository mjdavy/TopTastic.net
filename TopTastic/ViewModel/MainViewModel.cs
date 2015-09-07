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
        private ObservableCollection<PlaylistItemViewModel> _playlistItems;
        

        public ObservableCollection<PlaylistItemViewModel> PlaylistItems
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
            IDataService service = null;

            // MJDTODO - use DI
            //if (IsInDesignMode)
            //{
            //    service = new MockDataService();
            //}
            //else
            //{
            //    service = new DataService(); 
            //}
            service = new DataService();
            this.InitializePlaylistItems(service);
        }

        void InitializePlaylistItems(IDataService service)
        {
            service.GetPlaylistData((playlistData, err) =>
            {
                if (err == null)
                {
                    var playlistItems = new ObservableCollection<PlaylistItemViewModel>();
                    foreach (var item in playlistData.Items)
                    {
                        playlistItems.Add(new PlaylistItemViewModel(item));
                    }
                    this.PlaylistItems = playlistItems;
                    this.UpdateThumbnails(service);
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });
        }

        void UpdateThumbnails(IDataService service)
        {
            service.GetThumnails((thumbnails, err) =>
            {
                if (err == null)
                {
                    for(int i=0; i<PlaylistItems.Count; i++)
                    {
                        PlaylistItems[i].Thumbnail = thumbnails[i];
                    }
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });
        }
    }
}
